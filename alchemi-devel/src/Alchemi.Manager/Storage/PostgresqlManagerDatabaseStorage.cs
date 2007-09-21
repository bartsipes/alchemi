#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
* Title         :  PostgresqlManagerDatabaseStorage.cs
* Project       :  Alchemi.Core.Manager.Storage
* Created on    :  10 February 2007
* Copyright     :  Copyright © 2006 The University of Melbourne
*                    This technology has been developed with the support of
*                    the Australian Research Council and the University of Melbourne
*                    research grants as part of the Gridbus Project
*                    within GRIDS Laboratory at the University of Melbourne, Australia.
* Author        :  Tibor Biro (tb@tbiro.com), Anton Melser
* License       :  GPL
*                    This program is free software; you can redistribute it and/or
*                    modify it under the terms of the GNU General Public
*                    License as published by the Free Software Foundation;
*                    See the GNU General Public License
*                    (http://www.gnu.org/copyleft/gpl.html) for more 
details.
*
*/
#endregion

using System;
using System.Data;
using Alchemi.Core;
using Alchemi.Core.Manager.Storage;
using Alchemi.Core.Utility;
using Npgsql;

namespace Alchemi.Manager.Storage
{
    /// <summary>
    /// Override some generic database calls with Postgresql specific calls.
    /// </summary>
    public class PostgresqlManagerDatabaseStorage : GenericManagerDatabaseStorage
    {
        public PostgresqlManagerDatabaseStorage(string connectionString)
            : base(connectionString)
        {
        }

        #region Database objects (Postgresql specific version)

        protected override IDbConnection GetConnection(string connectionString)
        {
            return new NpgsqlConnection(connectionString);
        }

        protected override IDbCommand GetCommand()
        {
            return new NpgsqlCommand();
        }

        protected override IDataParameter GetParameter(string name, object paramValue, DbType datatype)
        {
            object value = paramValue;
            DbType localDatatype = datatype;
            if (datatype == DbType.Guid)
                localDatatype = DbType.String;
            else
                localDatatype = datatype;

            NpgsqlParameter param;
            if (datatype == DbType.Guid && value != DBNull.Value)
            {
                value = new Guid(paramValue.ToString());
                param = new NpgsqlParameter(name, localDatatype);
                param.Value = value;
            }
            else
            {
                param = new NpgsqlParameter(name, value);
                param.DbType = localDatatype;
                
            }
            
            return param;
        }

        protected override IDataAdapter GetDataAdapter(IDbCommand command)
        {
            return new NpgsqlDataAdapter(command as NpgsqlCommand);
        }

        #endregion

        protected override string GetSetupFileLocation()
        {
            return "Postgresql";
        }

        protected override string DatabaseParameterDecoration()
        {
            return ":";
        }

        protected override string IsNullOperator
        {
            get
            {
                return "coalesce";
            }
        }

        protected override string GetPowerUsageSqlQuery()
        {
            return String.Format(
                "select cast(count(*) as int4) as total_executors, " +
                "{0}(cast(sum(cpu_max) as int4), 0) as max_power," +
                "{0}(cast(avg(cpu_usage) as int4), 0) as power_usage, {0}(cast(avg(cpu_avail) as int4), 0) as power_avail," +
                "{0}(sum(cpu_totalusage * cpu_max / (3600 * 1000)), 0) as power_totalusage " +
                "from executor where is_connected = 1 ", IsNullOperator );
        }

        protected override string GetUnfinishedThreadCountSqlQuery()
        {
            return "select cast(count(*) as int4) as unfinished_threads from thread where state not in (3, 4)";
        }

        protected override string GetUnfinishedApplicationCountSqlQuery()
        {
            return "select cast(count(*) as int4) as unfinished_apps " +
                "from application " +
                "where state not in (2) ";
        }

        protected override string GetUserCountSqlQuery( SecurityCredentials sc )
        {
            return String.Format(
                "select cast(count(*) as int4) as authenticated from usr where usr_name = '{0}' and password = '{1}'"
                , Utils.MakeSqlSafe( sc.Username )
                , Utils.MakeSqlSafe( sc.Password )
                );
        }
    }
}
