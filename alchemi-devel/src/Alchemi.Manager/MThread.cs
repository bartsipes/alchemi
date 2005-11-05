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
using Alchemi.Core.Manager.Storage;
using Alchemi.Core.Owner;
using Alchemi.Core.Utility;
using ThreadState = Alchemi.Core.Owner.ThreadState;

using Alchemi.Manager.Storage;

namespace Alchemi.Manager
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
				ThreadStorageView threadStorage = new ThreadStorageView(_AppId, _Id, ThreadState.Ready);

				ManagerStorageFactory.ManagerStorage().AddThread(threadStorage);
            }
            else
            {
				ApplicationStorageView applicationStorage = ManagerStorageFactory.ManagerStorage().GetApplication(_AppId);

				if (applicationStorage == null)
				{
					applicationStorage = new ApplicationStorageView(
						_AppId, 
						ApplicationState.AwaitingManifest, 
						DateTime.Now, 
						false,
						""/*What's the username here?*/);

					ManagerStorageFactory.ManagerStorage().AddApplication(applicationStorage);
				}

				ThreadStorageView threadStorage = new ThreadStorageView(_AppId, _Id, ThreadState.Ready);

				ManagerStorageFactory.ManagerStorage().AddThread(threadStorage);
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
				ThreadStorageView threadStorage = ManagerStorageFactory.ManagerStorage().GetThread(_AppId, _Id);

				threadStorage.Priority = value;

				ManagerStorageFactory.ManagerStorage().UpdateThread(threadStorage);
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
				ThreadStorageView threadStorage = ManagerStorageFactory.ManagerStorage().GetThread(_AppId, _Id);

                // TODO: if state can be verfified on the executor, verify it
                return threadStorage.State;
            }
            set
            {
				ThreadStorageView threadStorage = ManagerStorageFactory.ManagerStorage().GetThread(_AppId, _Id);
				
				// optional special things to do
                switch (value)
                {
                    case ThreadState.Started:
						threadStorage.TimeStarted = DateTime.Now;
                        break;

                    case ThreadState.Finished:
						threadStorage.TimeFinished = DateTime.Now;
                        break;
                }

				threadStorage.State = value;

				ManagerStorageFactory.ManagerStorage().UpdateThread(threadStorage);
            }
        }

		/// <summary>
		/// Resets this MThread so it can be rescheduled.
		/// </summary>
        public void Reset()
        {
			ThreadStorageView threadStorage = ManagerStorageFactory.ManagerStorage().GetThread(_AppId, _Id);

			//take care that only threads that are not already aborted are reset.
			if (threadStorage.State != ThreadState.Dead)
			{
				threadStorage.State = ThreadState.Ready;
				threadStorage.ExecutorId = null;
				threadStorage.ResetTimeStarted();
				threadStorage.ResetTimeFinished();

				ManagerStorageFactory.ManagerStorage().UpdateThread(threadStorage);
			}
        }

		/// <summary>
		/// Gets or sets the id of the executor
		/// </summary>
        public string CurrentExecutorId
        {            
            get
            {
				ThreadStorageView threadStorage = ManagerStorageFactory.ManagerStorage().GetThread(_AppId, _Id);

				return threadStorage.ExecutorId;
            }
			set 
			{
				ThreadStorageView threadStorage = ManagerStorageFactory.ManagerStorage().GetThread(_AppId, _Id);

				threadStorage.ExecutorId = value;

				ManagerStorageFactory.ManagerStorage().UpdateThread(threadStorage);
			}
		}
    }
}
