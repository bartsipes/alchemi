#region Alchemi copyright notice
/*
  Alchemi [.NET Grid Computing Framework]
  Copyright (c) 2002-2004 Akshay Luther
  http://www.alchemi.net
---------------------------------------------------------------------------

  This program is free software; you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation; either version 2 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program; if not, write to the Free Software
  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/
#endregion

using System;
using System.IO;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Remoting;
using System.Threading;
using Alchemi.Core;
using Alchemi.Core.Utility;

namespace Alchemi.Core.Manager
{
    public class MExecutorCollection
    {
        private Hashtable _DedicatedExecutors = new Hashtable();

        public MExecutor this[string executorId]
        {
            get
            {
                return new MExecutor(executorId);
            }
        }

        public string RegisterNew(SecurityCredentials sc, int cpuPower)
        {
            string executorId = InternalShared.Instance.Database.ExecSql_Scalar("Executor_Insert 0, '{0}', {1}", sc.Username, cpuPower).ToString();
            return executorId;
        }

        public void Init()
        {
            DataTable dt = InternalShared.Instance.Database.ExecSql_DataTable("select * from executor where is_dedicated = 1");

            foreach (DataRow dr in dt.Rows)
            {
                string executorId = dr["executor_id"].ToString();
                RemoteEndPoint ep = new RemoteEndPoint((string) dr["host"], (int) dr["port"], RemotingMechanism.TcpBinary);
                try
                {
                    new MExecutor(executorId).ConnectDedicated(ep);
                }
                catch (ExecutorCommException) {}
            }
        }

        public DataTable AvailableDedicatedExecutors
        {
            get 
            {
                return InternalShared.Instance.Database.ExecSql_DataTable("Executor_SelectAvailableDedicated");
            }
        }
    }
}