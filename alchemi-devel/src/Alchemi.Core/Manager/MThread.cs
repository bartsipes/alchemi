#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	MThread.cs
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
using System.IO;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Remoting;
using System.Threading;
using Alchemi.Core;
using Alchemi.Core.Owner;
using Alchemi.Core.Utility;
using ThreadState = Alchemi.Core.Owner.ThreadState;

namespace Alchemi.Core.Manager
{
	/// <summary>
	/// Represents a thread on the manager node.
	/// </summary>
    public class MThread
    {
        private string _AppId;
        private int _Id;
        
		/// <summary>
		/// Creates a new instance of an MThread
		/// </summary>
		/// <param name="appId">id of the application, this thread belongs to</param>
		/// <param name="id">id of this thread</param>
        public MThread(string appId, int id)
        {
            _AppId = appId;
            _Id = id;
        }
        
		/// <summary>
		/// Creates a new instance of an MThread
		/// </summary>
		/// <param name="ti">ThreadIdentifier for this thread</param>
        public MThread(ThreadIdentifier ti)
        {
            _AppId = ti.ApplicationId;
            _Id = ti.ThreadId;
        }

		/// <summary>
		/// Initializes this thread.
		/// </summary>
		/// <param name="primary">specifies if this thread is a primary thread. A thread is primary if it is created and scheduled by the same manager</param>
        public void Init(bool primary)
        {
            if (primary)
            {
                InternalShared.Instance.Database.ExecSql(
                    "Thread_Insert '{0}', {1}", _AppId, _Id);
            }
            else
            {
                InternalShared.Instance.Database.ExecSql(
                    "Thread_InsertNonPrimary '{0}', {1}, {2}", _AppId, _Id);
            }
        }
        
		/// <summary>
		/// Gets or sets the byte array representing the serialized thread
		/// </summary>
        public byte[] Value
        {
            set
            {
                Utils.WriteByteArrayToFile(DataFile, value);
            }
            get
            {
                return Utils.ReadByteArrayFromFile(DataFile);
            }
        }

		/// <summary>
		/// Gets or sets the exception that occurred during the thread execution
		/// </summary>
        public Exception FailedThreadException
        {
            set
            {
                Utils.SerializeToFile(value, ExceptionFile);
            }
            get
            {
                try
                {
                    return (Exception) Utils.DeserializeFromFile(ExceptionFile);
                }
                catch
                {
                    return null;
                }
            }
        }

		/// <summary>
		/// Gets or sets the thread priority
		/// </summary>
        public int Priority
        {
            set 
            {
                InternalShared.Instance.Database.ExecSql(string.Format(
                    "update thread set priority = {0} where application_id = '{1}' and thread_id = {2}", value, _AppId, _Id
                    ));

                string foo = string.Format("select priority from thread where application_id = '{0}' and thread_id = {1}", _AppId, _Id).ToString(); // InternalShared.Instance.Database.ExecSql_Scalar
            }
        }

        private string DataFile
        {
            get 
            {
                return Path.Combine(InternalShared.Instance.DataRootDirectory,
                    Path.Combine("application_" + _AppId, "thread_" + _Id + ".dat"));
            }
        }

        private string ExceptionFile
        {
            get
            {
                return Path.Combine(InternalShared.Instance.DataRootDirectory,
                    Path.Combine("application_" + _AppId, "thread_" + _Id + "_exception.dat"));
            }
        }

		/// <summary>
		/// Gets or sets the thread state
		/// </summary>
        public ThreadState State
        {
            get 
            {
                int state = (int) InternalShared.Instance.Database.ExecSql_Scalar(
                    string.Format("Thread_SelectState '{0}', {1}", _AppId, _Id)
                    );
                // TODO: if state can be verfified on the executor, verify it
                return (ThreadState) state;
            }
            set
            {
                // optional special things to do
                switch (value)
                {
                    case ThreadState.Started:
                        InternalShared.Instance.Database.ExecSql(
                            string.Format("update thread set time_started = getdate() where application_id = '{0}' and thread_id = {1}", _AppId, _Id)
                            );
                        break;

                    case ThreadState.Finished:
                        InternalShared.Instance.Database.ExecSql(
                            string.Format("update thread set time_finished = getdate() where application_id = '{0}' and thread_id = {1}", _AppId, _Id)
                            );
                        break;
                }

                // update state in db
                InternalShared.Instance.Database.ExecSql(
                    string.Format("Thread_UpdateState '{0}', {1}, {2}", _AppId, _Id, (int) value)
                    );
            }
        }

		/// <summary>
		/// Resets this MThread so it can be rescheduled.
		/// </summary>
        public void Reset()
        {
			//the reset stored-procedure takes care that only threads that are not already aborted are reset.
            InternalShared.Instance.Database.ExecSql(
                string.Format("Thread_Reset '{0}', {1}", _AppId, _Id)
                );
        }

		/// <summary>
		/// Gets or sets the id of the executor
		/// </summary>
        public string CurrentExecutorId
        {
            set 
            {
                InternalShared.Instance.Database.ExecSql(
                    "update thread set executor_id = '{2}' where thread.application_id = '{0}' and thread.thread_id = {1}", _AppId, _Id, value);                
            }
            
            get
            {
                DataTable dt = InternalShared.Instance.Database.ExecSql_DataTable(string.Format("select executor.executor_id, executor.is_dedicated from executor inner join thread on thread.executor_id = executor.executor_id where thread.application_id = '{0}' and thread.thread_id = {1}", _AppId, _Id));
                if (dt.Rows.Count != 0)
                {
                    DataRow executor = dt.Rows[0];
                    return executor["executor_id"].ToString();
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
