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
using System.Collections.Specialized;
using System.Runtime.Remoting;
using System.Threading;
using Alchemi.Core;
using Alchemi.Core.Utility;

namespace Alchemi.Core.Manager
{
    public class MApplication
    {
        private string _Id;
        
        public MApplication(string id)
        {
            _Id = id;
        }
        
        public MThread this[int threadId]
        {
            get
            {
                return new MThread(_Id, threadId);
            }
        }

        public FileDependencyCollection Manifest
        {
            set 
            {
                Utils.SerializeToFile(value, Path.Combine(DataDir, "manifest.dat"));
                this.State = ApplicationState.Ready;
            }

            get 
            {
                return (FileDependencyCollection) Utils.DeserializeFromFile(
                    Path.Combine(DataDir, "manifest.dat"));
            }
        }

        private string DataDir
        {
            get
            {
                return Path.Combine(InternalShared.Instance.DataRootDirectory, "application_" + _Id);
            }
        }
        
        public byte[][] FinishedThreads
        {
            get
            {
                ArrayList finishedThreads = new ArrayList();

                DataTable dt = InternalShared.Instance.Database.ExecSql_DataTable(
                    string.Format("Threads_UpdateStateAndSelect '{0}', {1}, {2}", _Id, (int) ThreadState.Finished, (int) ThreadState.Dead)
                    );

                foreach (DataRow dr in dt.Rows)
                {
                    finishedThreads.Add(
                        this[int.Parse(dr["thread_id"].ToString())].Value
                        );
                }
                return (byte[][]) finishedThreads.ToArray(typeof(byte[]));
            }
        }

        public void CreateDataDirectory()
        {
            Directory.CreateDirectory(DataDir);
        }

        public ApplicationState State
        {
            get
            {
                int state = (int) InternalShared.Instance.Database.ExecSql_Scalar(
                    string.Format("select state from application where application_id = '{0}'", _Id)
                    );
                return (ApplicationState) state;
            }
            set
            {
                InternalShared.Instance.Database.ExecSql("Application_UpdateState '{0}', {1}", _Id, (int) value);
            }
        }

        public DataSet ThreadList
        {
            get
            {
                return InternalShared.Instance.Database.ExecSql_DataSet(string.Format("select thread_id, state, time_started, time_finished from thread where application_id = '{0}' order by thread_id", _Id));
            }
        }

        public bool IsPrimary
        {
            get 
            {
                string primary = 
                    InternalShared.Instance.Database.ExecSql_Scalar(
                    string.Format("select count(*) from application where application_id = '{0}' and is_primary = 1", _Id)
                    ).ToString();

                return (primary == "0") ? false : true;
            }
        }

        public void Stop()
        {
            DataTable dt = InternalShared.Instance.Database.ExecSql_DataTable("Application_Stop '{0}'", _Id);
            foreach (DataRow thread in dt.Rows)
            {
                GManager.AbortThread(new ThreadIdentifier(_Id, int.Parse(thread["thread_id"].ToString())), thread["executor_id"].ToString());
            }
        }
    }
}
