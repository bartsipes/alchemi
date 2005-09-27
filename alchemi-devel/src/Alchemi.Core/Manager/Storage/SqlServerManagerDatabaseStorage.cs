//
// Alchemi.Core.Manager.Storage.SqlServerManagerDatabaseStorage.cs
//
// Author:
//   Tibor Biro (tb@tbiro.com)
//
// Copyright (C) 2005 Tibor Biro (tb@tbiro.com)
//
// This program is free software; you can redistribute it and/or modify 
// it under the terms of the GNU General Public License as published by 
// the Free Software Foundation; either version 2 of the License, or 
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of 
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the 
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License 
// along with this program; if not, write to the Free Software 
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

using System;
using System.Data;
using System.Data.SqlClient;

using Advanced.Data.Provider;

namespace Alchemi.Core.Manager.Storage
{
	/// <summary>
	/// Override some generic database calls with SQL Server specific calls.
	/// This is usually done for performance reasons.
	/// </summary>
	public class SqlServerManagerDatabaseStorage : GenericManagerDatabaseStorage, IManagerStorage, IManagerStorageSetup
	{
		private String m_connectionString;

		public SqlServerManagerDatabaseStorage(String connectionString) : base(connectionString)
		{
			m_connectionString = connectionString;
		}

		#region IManagerStorage Members

		/// <summary>
		/// GetSystemSummary implementation for Sql Server.
		/// This implementation uses a stored procedure to extract the data.
		/// </summary>
		/// <returns></returns>
		public new SystemSummary GetSystemSummary()
		{
			using (AdpDataReader dataReader = RunSpReturnDataReader("Admon_SystemSummary"))
			{
				if (dataReader.Read())
				{
					String maxPower;
					Int32 totalExecutors;
					Int32 powerUsage;
					Int32 powerAvailable;
					String powerTotalUsage;
					Int32 unfinishedThreads;

					maxPower = dataReader.GetString(dataReader.GetOrdinal("max_power"));
					totalExecutors = dataReader.GetInt32(dataReader.GetOrdinal("total_executors"));
					powerUsage = dataReader.GetInt32(dataReader.GetOrdinal("power_usage"));
					powerAvailable = dataReader.GetInt32(dataReader.GetOrdinal("power_avail"));
					powerTotalUsage = dataReader.GetString(dataReader.GetOrdinal("power_totalusage"));
					unfinishedThreads = dataReader.GetInt32(dataReader.GetOrdinal("unfinished_threads"));

					return new SystemSummary(
						maxPower, 
						totalExecutors,
						powerUsage,
						powerAvailable,
						powerTotalUsage,
						unfinishedThreads);
				}
			}

			return null;
		}

		#endregion

		#region IManagerStorageSetup Members

		/// <summary>
		/// Create the tables, stored procedures and other structures needed by this storage.
		/// </summary>
		public void SetUpStorage()
		{
			// TODO: load scripts from resources to do this
			// TODO: it should also contain constrains

			RunSql("CREATE TABLE dbo.usr (usr_name varchar(50),password varchar(50), grp_id int NOT NULL)");

			RunSql("CREATE TABLE dbo.grp (grp_id int NOT NULL, grp_name varchar(50) NOT NULL)");

			RunSql("CREATE PROCEDURE dbo.User_Authenticate(@usr_name varchar(50), @password varchar(50)) AS select count(*) as authenticated from usr where usr_name = @usr_name and password = @password");

			RunSql("if exists (select * from dbo.sysobjects where id = object_id(N'dbo.executor') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table dbo.executor");
			RunSql("CREATE TABLE dbo.executor (executor_id uniqueidentifier, is_dedicated bit NOT NULL, is_connected bit NOT NULL, ping_time datetime NULL, host varchar (100) NULL, port int NULL, usr_name varchar (50) NULL, cpu_max int NULL, cpu_usage int NULL, cpu_avail int NULL, cpu_totalusage float NULL )");

		}

		public void InitializeStorageData()
		{
			// TODO:  Add SqlServerManagerDatabaseStorage.InitializeStorageData implementation
		}

		public void TearDownStorage()
		{
			RunSql("if exists (select * from dbo.sysobjects where id = object_id(N'dbo.User_Authenticate') and OBJECTPROPERTY(id, N'IsProcedure') = 1) drop procedure dbo.User_Authenticate");
			RunSql("DROP TABLE dbo.usr");
			RunSql("DROP TABLE dbo.grp");

			RunSql("if exists (select * from dbo.sysobjects where id = object_id(N'dbo.executor') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table dbo.executor");

		}

		#endregion
	}
}
