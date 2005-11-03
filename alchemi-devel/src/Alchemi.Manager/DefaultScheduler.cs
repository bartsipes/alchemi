#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	DefaultScheduler.cs
* Project		:	Alchemi Core
* Created on	:	2003
* Copyright		:	Copyright © 2005 The University of Melbourne
*					This technology has been developed with the support of 
*					the Australian Research Council and the University of Melbourne
*					research grants as part of the Gridbus Project
*					within GRIDS Laboratory at the University of Melbourne, Australia.
* Author         :  Akshay Luther (akshayl@cs.mu.oz.au) and Rajkumar Buyya (raj@cs.mu.oz.au)
* License        :  GPL
*					This program is free software; you can redistribute it and/or 
*					modify it under the terms of the GNU General Public
*					License as published by the Free Software Foundation;
*					See the GNU General Public License 
*					(http://www.gnu.org/copyleft/gpl.html) for more details.
*
*/ 
#endregion


using System;
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
			DedicatedSchedule dsched = null;
			DataSet ds = InternalShared.Instance.Database.ExecSql_DataSet("Thread_Schedule null");
            if (ds.Tables.Count != 0)
            {
				DataRow dr = ds.Tables[0].Rows[0];
				string executorId = null;
				if (dr["executor_id"] != DBNull.Value)
				{
					executorId = dr["executor_id"].ToString();
				}
					
				string appid = null;
				if (dr["application_id"] != DBNull.Value)
				{
					appid = dr["application_id"].ToString();
				}
					
				int threadId = -1;
				if (dr["thread_id"]!=DBNull.Value)
				{
					threadId = (int)dr["thread_id"];
				}
					
				int priority = -1;
				if (dr["priority"]!=DBNull.Value)
				{
					priority = (int) dr["priority"];
				}
				else
				{
					priority = 5; //DEFAULT PRIORITY - TODO: have to put this in some Constants.cs file or something...
				}
				dr = null;
				
				if (threadId!=-1 && appid!=null && executorId!=null)
				{
					ThreadIdentifier ti= new ThreadIdentifier(appid, threadId,priority);
					logger.Debug("Schedule dedicated. app_id="+appid+",threadID="+threadId+", executor-id="+executorId);
					dsched = new DedicatedSchedule(ti, executorId);
				}
            }
			ds.Dispose();

            return dsched;
        }

		 
		/*
		 * //non-optimised code for demonstration on how a custom scheduler might be written
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
    }
}