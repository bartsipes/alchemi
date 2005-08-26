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

using System.Data;
using Alchemi.Core.Owner;

namespace Alchemi.Core.Manager
{
	/// <summary>
	/// This is the default implementation of the IScheduler interface provided by Alchemi.
	/// The Default scheduler works on the basis of priority-based FIFO.
	/// The threads are all ordered by priority, and time of creation. It ensures that 
	/// the thread with the highest priority execute first on the next available executor.
	/// It also assumes that all executors are equal, and the next thread is given to 
	/// any available dedicated executor.
	/// </summary>
    public class DefaultScheduler : IScheduler
    {
		// Create a logger for use in this class
		private static readonly Logger logger = new Logger();

		private MApplicationCollection _Applications;
        private MExecutorCollection _Executors;

        /// <summary>
        /// Sets the collection of applications.
        /// </summary>
        public MApplicationCollection Applications { set { _Applications = value; } }

		/// <summary>
		/// Sets the collection of executors
		/// </summary>
        public MExecutorCollection Executors { set { _Executors = value; } }
        
		/// <summary>
		/// Return a non-dedicated schedule: i.e a threadIdentifier of the next thread to be executed.
		/// This is to support voluntary / non-dedicated execution, where an executor asks for the next
		/// work unit. 
		/// </summary>
		/// <param name="executorId">The executorId passed in refers to the Executor which will run this thread.</param>
		/// <returns>ThreadIdentifier of the next available thread</returns>
        public ThreadIdentifier ScheduleNonDedicated(string executorId)
        {
			//logger.Debug("Schedule non-dedicated...");
			DataSet ds = InternalShared.Instance.Database.ExecSql_DataSet("Thread_Schedule '{0}'", executorId);
            if (ds.Tables.Count == 0)
            {
				//logger.Debug("no records returned for schedule-non-dedicated");
                return null;
            }
            DataRow dr = ds.Tables[0].Rows[0];
			
			logger.Debug("Schedule non-dedicated. app_id="+dr["application_id"].ToString()+",threadID="+dr["thread_id"]);

			string appid = dr["application_id"].ToString();
			int threadId = (int)dr["thread_id"];
			int priority = (int) dr["priority"];
			
			dr = null;
			ds.Dispose();

            return new ThreadIdentifier(appid, threadId, priority);
        }

		/// <summary>
		/// Queries the database to return the next dedicated schedule.
		/// </summary>
		/// <returns>DedicatedSchedule</returns>
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
			//logger.Debug("Schedule dedicated...");
			DataSet ds = InternalShared.Instance.Database.ExecSql_DataSet("Thread_Schedule null");
            if (ds.Tables.Count == 0)
            {
				//logger.Debug("no records returned for schedule-dedicated");
                return null;
            }
            DataRow dr = ds.Tables[0].Rows[0];
            string executorId = dr["executor_id"].ToString();
			string appid = dr["application_id"].ToString();
			int threadId = (int)dr["thread_id"];
			int priority = (int) dr["priority"];
            ThreadIdentifier ti = new ThreadIdentifier(appid, threadId,priority);
			
			dr = null;
			ds.Dispose();

			logger.Debug("Schedule dedicated. app_id="+appid+",threadID="+threadId+", executor-id="+executorId);

            return new DedicatedSchedule(ti, executorId);
        }
    }
}