#region Alchemi copyright notice
/*
  Alchemi [.NET Grid Computing Framework]
  http://www.alchemi.net
  
  Copyright (c)  Akshay Luther (2002-2004) & Rajkumar Buyya (2003-to-date), 
  GRIDS Lab, The University of Melbourne, Australia.
  
  Maintained and Updated by: Krishna Nadiminti (2005-to-date)
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
using System.Collections;
using System.Threading;
using Alchemi.Core.Executor;
using Alchemi.Core.Owner;
using Alchemi.Core.Utility;

namespace Alchemi.Core.Manager
{
	/// <summary>
	/// Represents a container for the executor reference held by the manager.
	/// </summary>
    public class MExecutor
    {
		// Create a logger for use in this class
		private static readonly Logger logger = new Logger();
		
		private readonly static Hashtable _DedicatedExecutors = new Hashtable();
        
        private string _Id;
		private ThreadIdentifier currentTI;

		private const int MAX_CONCURRENT_THREADS = 1;

		/// <summary>
		/// Creates a new instance of the MExecutor class.
		/// </summary>
		/// <param name="id">id of the executor</param>
        public MExecutor(string id)
        {
            _Id = id;
        }

		/// <summary>
		/// Connects to the executor in non-dedicated mode.
		/// </summary>
        public void ConnectNonDedicated(RemoteEndPoint ep)
        {
			logger.Debug("Trying to connect NON-Dedicated to executor: "+_Id);
            VerifyExists(ep);

			//here we dont ping back, since we know non-dedicated nodes can't be pinged back.
            
            // update state in db
            InternalShared.Instance.Database.ExecSql(
                string.Format("update executor set is_connected = 1, is_dedicated = 0 where executor_id = '{0}'", _Id)
                );

			logger.Debug("Connected non-dedicated. Updated db. executor_id="+_Id);
        }

		/// <summary>
		/// Connects to the executor in dedicated mode.
		/// </summary>
		/// <param name="ep">end point of the executor</param>
        public void ConnectDedicated(RemoteEndPoint ep)
        {
			logger.Debug("Trying to connect Dedicated to executor: "+_Id);
			VerifyExists(ep);

            bool success = false;
            IExecutor executor;
            try
            {
                executor = (IExecutor) GNode.GetRemoteRef(ep);
                executor.PingExecutor(); //connect back to executor.
                success = true;
				logger.Debug("Connected dedicated. Executor_id="+_Id);
            }
            catch (Exception e)
            {
				logger.Error("Error connecting to exec: "+_Id,e);
                throw new ExecutorCommException(_Id, e);
            }
            finally
            {
                // update state in db (always happens even if cannnot connect back to executor
                InternalShared.Instance.Database.ExecSql(
                    string.Format("update executor set is_connected = {1}, is_dedicated = 1, host = '{2}', port = {3} where executor_id = '{0}'", _Id, Utils.BoolToSqlBit(success), ep.Host, ep.Port)
                    );
				logger.Debug("Updated db after ping back to executor. dedicated executor_id="+_Id + ", dedicated = true, connected = "+success);
            }

            // update hashtable
            if (!_DedicatedExecutors.ContainsKey(_Id))
            {
                _DedicatedExecutors.Add(_Id, executor);
				logger.Debug("Added to list of dedicated executors: executor_id="+_Id);
            }
        }

        private void VerifyExists(RemoteEndPoint ep)
        {
            bool exists;
            try
            {
				logger.Debug("Checking if executor :"+_Id+" exists in db");
                exists = bool.Parse(
                    (string) InternalShared.Instance.Database.ExecSql_Scalar(string.Format("Executor_SelectExists '{0}','{1}'", _Id, ep.Host))
                    );
				logger.Debug("Executor :"+_Id+" exists in db="+exists);
            }
            catch (Exception ex)
            {
				logger.Error("Executor :"+_Id+ " invalid id? ",ex);
                throw new InvalidExecutorException("The supplied Executor ID is invalid.", ex);
            }
            if (!exists)
            {
				logger.Debug("The supplied Executor ID does not exist.");
                throw new InvalidExecutorException("The supplied Executor ID does not exist.", null);
            }
        }

		/// <summary>
		/// Updates the database with the heartbeat info of this executor
		/// </summary>
		/// <param name="info"></param>
        public void HeartbeatUpdate(HeartbeatInfo info)
        {
            // update ping time and other heartbeatinfo
            InternalShared.Instance.Database.ExecSql("Executor_Heartbeat '{0}', {1}, {2}, {3}", _Id, info.Interval, info.PercentUsedCpuPower, info.PercentAvailCpuPower);
        }

		/// <summary>
		/// Disconnect from the executor. 
		/// (Updates the database to reflect the executor disconnection.)
		/// </summary>
        public void Disconnect()
        {
            // maybe should reset threads here as part of the disconnection rather than explicitly ...
            InternalShared.Instance.Database.ExecSql(
                string.Format("update executor set is_connected = 0 where executor_id = '{0}'", _Id));
        }

		/// <summary>
		/// Gets the remote reference to the executor node
		/// </summary>
        public IExecutor RemoteRef
        {
            get
            {
                return (IExecutor) _DedicatedExecutors[_Id];
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ti"></param>
		/// <returns></returns>
		public bool ExecuteThread(ThreadIdentifier ti)
		{
			bool success = false;
			//kna added this to allow controlling MAX_CONCURRENT_THREADS per executor. Aug19. 05
			//find # of executing threads from the db.
			int numConcurrentThreads = 0;
			try
			{
				string strSQL = string.Format("select count(*) from thread where executor_id='{0}' and state not in (3,4)",_Id);
				numConcurrentThreads = Int32.Parse(InternalShared.Instance.Database.ExecSql_Scalar(strSQL).ToString());
			}
			catch (Exception ex)
			{
				logger.Debug("Error finding executing threads on executor."+ _Id, ex);
			}
			finally
			{
				if (numConcurrentThreads >= MAX_CONCURRENT_THREADS)
				{
					success = false;
				}
				else
				{
					this.currentTI = ti;
					Thread dispatchThread = new Thread(new ThreadStart(this.ExecuteCurrentThread));
					dispatchThread.Name = "ScheduleDispatchThread";
					dispatchThread.Start();
					success = true;
				}
			}
			return success;
		}

		private void ExecuteCurrentThread()
		{
			this.RemoteRef.Manager_ExecuteThread(this.currentTI);
		}

    }
}
