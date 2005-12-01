#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	MApplication.cs
* Project		:	Alchemi Core
* Created on	:	2003
* Copyright		:	Copyright © 2005 The University of Melbourne
*					This technology has been developed with the support of 
*					the Australian Research Council and the University of Melbourne
*					research grants as part of the Gridbus Project
*					within GRIDS Laboratory at the University of Melbourne, Australia.
* Author         :  Akshay Luther (akshayl@csse.unimelb.edu.au), Rajkumar Buyya (raj@csse.unimelb.edu.au), and Krishna Nadiminti (kna@csse.unimelb.edu.au)
* License        :  GPL
*					This program is free software; you can redistribute it and/or 
*					modify it under the terms of the GNU General Public
*					License as published by the Free Software Foundation;
*					See the GNU General Public License 
*					(http://www.gnu.org/copyleft/gpl.html) for more details.
*
*/ 
#endregion


using System.Collections;
using System.Data;
using System.IO;
using Alchemi.Core;
using Alchemi.Core.Owner;
using Alchemi.Core.Utility;
using Alchemi.Core.Manager.Storage;
using Alchemi.Manager.Storage;

namespace Alchemi.Manager
{
	/// <summary>
	/// Represents an Application on the manager.
	/// </summary>
    public class MApplication
    {
		// Create a logger for use in this class
		private static readonly Logger logger = new	Logger();

        private string _Id;
        
		/// <summary>
		/// Creates an instance of MApplication
		/// </summary>
		/// <param name="id">id of the application</param>
        public MApplication(string id)
        {
            _Id = id;
        }
        
		/// <summary>
		/// Returns the MThread whose threadId is passed in
		/// </summary> //this is an indexer
        public MThread this[int threadId]
        {
            get
            {
                return new MThread(_Id, threadId);
            }
        }

		/// <summary>
		/// Gets or sets the application manifest which is a collection of fileDependencies
		/// </summary>
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
        
		/// <summary>
		/// Gets the finished threads as a byte array
		/// 
		/// As a side effect, sets the status of the Finished threads to Dead
		/// </summary>
        public byte[][] FinishedThreads
        {
            get
            {
            	ArrayList finishedThreads = new ArrayList();

				ThreadStorageView[] threads = ManagerStorageFactory.ManagerStorage().GetThreads(_Id, ThreadState.Finished);

                foreach (ThreadStorageView thread in threads)
                {
					thread.State = ThreadState.Dead;
					ManagerStorageFactory.ManagerStorage().UpdateThread(thread);

                    finishedThreads.Add(
                        this[thread.ThreadId].Value
                        );
                }
                return (byte[][]) finishedThreads.ToArray(typeof(byte[]));
            }
        }

		/// <summary>
		/// Gets the count threads with the given thread-state.
		/// 
		/// Updates: 
		/// 
		///	27 October 2005 - Tibor Biro (tb@tbiro.com) - Replaced the direct database call with Manager Storage object calls
		///	
		/// </summary>
		/// <param name="ts"></param>
		/// <returns>Thread count</returns>
		public int ThreadCount(ThreadState ts)
		{
			return ManagerStorageFactory.ManagerStorage().GetApplicationThreadCount(_Id, ts);
		}

		/// <summary>
		/// Creates the data directory
		/// </summary>
        public void CreateDataDirectory()
        {
            Directory.CreateDirectory(DataDir);
        }

		/// <summary>
		/// Gets or sets the application state
		/// </summary>
		public ApplicationState State
		{
			get
			{
				ApplicationStorageView application = ManagerStorageFactory.ManagerStorage().GetApplication(_Id);

				return application.State;
			}
			set
			{
				ApplicationStorageView application = ManagerStorageFactory.ManagerStorage().GetApplication(_Id);

				application.State = value;

				ManagerStorageFactory.ManagerStorage().UpdateApplication(application);
			}
		}

		/// <summary>
		/// Gets the list of threads from the database, for this application.
		/// </summary>
        public ThreadStorageView[] ThreadList
        {
            get
            {
				return ManagerStorageFactory.ManagerStorage().GetThreads(_Id);
            }
        }

		/// <summary>
		/// Gets the list of  threads with the given thread-state
		/// </summary>
		/// <param name="status"></param>
		/// <returns>Dataset with thread info.</returns>
		public ThreadStorageView[] GetThreadList(ThreadState status)
		{
			return ManagerStorageFactory.ManagerStorage().GetThreads(_Id, status);
		}

		/// <summary>
		/// Gets a value indicating whether the manager is at the top of the hierarchy.
		/// If true, this node is a primary manager.
		/// </summary>
		public bool IsPrimary
		{
			get 
			{
				ApplicationStorageView application = ManagerStorageFactory.ManagerStorage().GetApplication(_Id);

				return application.Primary;
			}
		}

		/// <summary>
		/// Stops an application.
		/// </summary>
		public void Stop()
		{
			//first check if the application is all finished
			IManagerStorage store = ManagerStorageFactory.ManagerStorage();
			ApplicationStorageView app = store.GetApplication(_Id);

			if (app.State!=ApplicationState.Stopped)
			{
				ThreadStorageView[] threads = store.GetThreads(_Id);

				foreach (ThreadStorageView thread in threads)
				{
					if (thread.State != ThreadState.Dead && thread.State != ThreadState.Finished)
					{
						GManager.AbortThread(new ThreadIdentifier(_Id, thread.ThreadId), thread.ExecutorId);
						thread.State = ThreadState.Dead; //this will update the thread
					}
				}

				//update the application
				app.State = ApplicationState.Stopped;
				store.UpdateApplication(app);

				logger.Debug("Stopped the current application."+_Id);				
			}
			else
			{
				logger.Debug(string.Format("Application {0} already stopped.",_Id));
			}
			
		}
	}
}
