#region Alchemi copyright notice
/*
  Alchemi [.NET Grid Computing Framework]
  http://www.alchemi.net
  
  Copyright (c) 2002-2004 Akshay Luther & 2003-2004 Rajkumar Buyya 
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
using System.Runtime.Remoting;
using System.Threading;
using Alchemi.Core;
using Alchemi.Core.Utility;

namespace Alchemi.Core.Manager
{
    public class DefaultScheduler : IScheduler
    {
        private MApplicationCollection _Applications;
        private MExecutorCollection _Executors;
        
        public MApplicationCollection Applications { set { _Applications = value; } }
        public MExecutorCollection Executors { set { _Executors = value; } }
        
        public ThreadIdentifier ScheduleNonDedicated(string executorId)
        {
            DataSet ds = InternalShared.Instance.Database.ExecSql_DataSet("Thread_Schedule '{0}'", executorId);
            if (ds.Tables.Count == 0)
            {
                return null;
            }
            DataRow dr = ds.Tables[0].Rows[0];
            return new ThreadIdentifier(dr["application_id"].ToString(), (int) dr["thread_id"], (int) dr["priority"]);
        }

        public DedicatedSchedule ScheduleDedicated()
        {
            // non-optimised code for demonstration on how a custom scheduler might be written
            /*
            DataTable executors = _Executors.AvailableDedicatedExecutors;
            if (executors.Rows.Count == 0)
            {
                return null;
            }
            string executorId = executors.Rows[0]["executor_id"].ToString();

            
            DataTable threads = _Applications.ReadyThreads;
            if (threads.Rows.Count == 0)
            {
                return null;
            }
            ThreadIdentifier ti = new ThreadIdentifier(
                threads.Rows[0]["application_id"].ToString(),
                int.Parse(threads.Rows[0]["thread_id"].ToString()),
                int.Parse(threads.Rows[0]["priority"].ToString())
                );
                */
            
            DataSet ds = InternalShared.Instance.Database.ExecSql_DataSet("Thread_Schedule null");
            if (ds.Tables.Count == 0)
            {
                return null;
            }
            DataRow dr = ds.Tables[0].Rows[0];
            ThreadIdentifier ti = new ThreadIdentifier(dr["application_id"].ToString(), (int) dr["thread_id"], (int) dr["priority"]);
            string executorId = dr["executor_id"].ToString();
            return new DedicatedSchedule(ti, executorId);
        }
    }
}