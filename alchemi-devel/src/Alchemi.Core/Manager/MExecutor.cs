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
    public class MExecutor
    {
        private readonly static Hashtable _DedicatedExecutors = new Hashtable();
        
        private string _Id;
        private IExecutor _Executor;

        public MExecutor(string id)
        {
            _Id = id;
        }

        public MExecutor(string id, IExecutor executor)
        {
            _Id = id;
            _Executor = executor;
        }

        public void ConnectNonDedicated()
        {
            VerifyExists();
            
            // update state in db
            InternalShared.Instance.Database.ExecSql(
                string.Format("update executor set is_connected = 1, is_dedicated = 0 where executor_id = '{0}'", _Id)
                );
        }

        public void ConnectDedicated(RemoteEndPoint ep)
        {
            VerifyExists();

            bool success = false;
            IExecutor executor;
            try
            {
                executor = (IExecutor) GNode.GetRemoteRef(ep);
                executor.PingExecutor();
                success = true;
            }
            catch (Exception e)
            {
                throw new ExecutorCommException(_Id, e);
            }
            finally
            {
                // update state in db
                InternalShared.Instance.Database.ExecSql(
                    string.Format("update executor set is_connected = {1}, is_dedicated = 1, host = '{2}', port = {3} where executor_id = '{0}'", _Id, Utils.BoolToSqlBit(success), ep.Host, ep.Port)
                    );
            }

            // update hashtable
            if (!_DedicatedExecutors.ContainsKey(_Id))
            {
                _DedicatedExecutors.Add(_Id, executor);
            }
        }

        private void VerifyExists()
        {
            bool exists;
            try
            {
                exists = bool.Parse(
                    (string) InternalShared.Instance.Database.ExecSql_Scalar(string.Format("Executor_SelectExists '{0}'", _Id))
                    );
            }
            catch
            {
                throw new InvalidExecutorException("The supplied Executor ID is invalid.", null);
            }
            if (!exists)
            {
                throw new InvalidExecutorException("The supplied Executor ID does not exist.", null);
            }
        }

        public void HeartbeatUpdate(HeartbeatInfo info)
        {
            // update ping time and other heartbeatinfo
            InternalShared.Instance.Database.ExecSql("Executor_Heartbeat '{0}', {1}, {2}, {3}", _Id, info.Interval, info.PercentUsedCpuPower, info.PercentAvailCpuPower);
        }

        public void Disconnect()
        {
            // maybe should reset threads here as part of the disconnection rather than explicitly ...
            InternalShared.Instance.Database.ExecSql(
                string.Format("update executor set is_connected = 0 where executor_id = '{0}'", _Id));
        }

        public IExecutor RemoteRef
        {
            get
            {
                return (IExecutor) _DedicatedExecutors[_Id];
            }
        }

    }
}
