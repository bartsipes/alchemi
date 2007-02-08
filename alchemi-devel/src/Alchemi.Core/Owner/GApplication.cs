#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	GApplication.cs
* Project		:	Alchemi Core
* Created on	:	2003
* Copyright		:	Copyright © 2006 The University of Melbourne
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

using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;
using Alchemi.Core.Utility;
using System.Collections.Generic;

// 2.8.07 MDV
// TODO: Add blocking Start() method. 
// That is, a call to Start() doesn't return until the entire GApplication is finished.

// TODO: Maybee we need not have a seperate "state" for the application.
// If all threads are dead, app should have state=stopped as well isnt it?
// How do we handle multi-use apps then?

namespace Alchemi.Core.Owner
{
	/// <summary>
	/// Represents a grid application
	/// </summary>
	public class GApplication : GNode
	{
		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion

		//----------------------------------------------------------------------------------------------- 
		// member variables
		//----------------------------------------------------------------------------------------------- 

		// Create a logger for use in this class
		private static readonly Logger logger = new Logger();

		private Container components = null;
       
		private FileDependencyCollection _Manifest = new FileDependencyCollection();
		private ThreadCollection _Threads = new ThreadCollection();
		private string _Id = "";
		private int _LastThreadId = -1;
        private String _ApplicationName;

		private int _NumThreadsFinished = 0;
		private Thread _GetFinishedThreadsThread;

        private Thread _StartAppThread;

		private bool _Running = false;
		private bool _Initted = false;
		private bool _MultiUse = false;

		//to prevent starting of already started app. also to prevent re-using single-use apps.
		private bool firstuse = true;

		private bool _StopGetFinished = false;


        #region Event - ThreadFinish
        private event GThreadFinish _ThreadFinish;
        /// <summary>
        /// ThreadFinish event: is raised when the thread has completed execution successfully.
        /// </summary>		
        public event GThreadFinish ThreadFinish
        {
            add { _ThreadFinish += value; }
            remove { _ThreadFinish -= value; }
        }
        #endregion


        #region Event - ThreadFailed
        private event GThreadFailed _ThreadFailed;
        /// <summary>
        /// ThreadFailed event: is raised when the thread has completed execution and failed.
        /// </summary>		
        public event GThreadFailed ThreadFailed
        {
            add { _ThreadFailed += value; }
            remove { _ThreadFailed -= value; }
        }
        #endregion


        #region Event - ApplicationFinish
        private event GApplicationFinish _ApplicationFinish;
        /// <summary>
        /// ApplicationFinish event: is raised when all the threads in the application have completed execution, i.e finished/failed.
        /// This event is NOT raised when the GApplication is declared as "multi-use".
        /// </summary>
        public event GApplicationFinish ApplicationFinish
        {
            add { _ApplicationFinish += value; }
            remove { _ApplicationFinish -= value; }
        } 
        #endregion


		//----------------------------------------------------------------------------------------------- 
		// properties
		//----------------------------------------------------------------------------------------------- 

		/// <summary>
		/// Gets the application manifest (a manifest is a collection file dependencies)
		/// </summary>
		public FileDependencyCollection Manifest
		{
			get { return _Manifest; }
		}

		/// <summary>
		/// Gets the collection  threads in the application
		/// </summary>
		public ThreadCollection Threads
		{
			get { return _Threads; }
		}

		/// <summary>
		/// Gets the application id
		/// </summary>
		public string Id
		{
			get { return _Id; }
		}

        /// <summary>
        /// Gets or Sets the application name
        /// </summary>
        /// <param name="name"></param>
        public string ApplicationName
        {
            get { return _ApplicationName; }
            set { _ApplicationName = value; }
        }

		/// <summary>
		/// Gets a name indicating whether the application is currently running
		/// </summary>
		public bool Running
		{
			get { return _Running; }
		}

		/// <summary>
		/// Gets the state of the given GThread
		/// </summary>
		/// <param name="thread"></param>
		/// <returns>ThreadState indicating its status</returns>
		internal ThreadState GetThreadState(GThread thread)
		{
			if (_Running)
			{
				return Manager.Owner_GetThreadState(Credentials, new ThreadIdentifier(_Id, thread.Id));			
			}
			else
			{
				return ThreadState.Unknown;
			}
		}

		//----------------------------------------------------------------------------------------------- 
		// constructors and disposal
		//----------------------------------------------------------------------------------------------- 

		/// <summary>
		/// Creates an instance of the GApplication
		/// </summary>
		/// <param name="container"></param>
		public GApplication(IContainer container)
		{
			container.Add(this);
			InitializeComponent();
		}

		/// <summary>
		/// Creates an instance of the GApplication
		/// </summary>
		public GApplication()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Creates an instance of the GApplication
		/// </summary>
		/// <param name="connection"></param>
		public GApplication(GConnection connection) : base(connection) 
		{
			InitializeComponent();
		}

		/// <summary>
		/// Creates an instance of the GApplication
		/// </summary>
		/// <param name="multiUse">specifies if the GApplication instance is re-usable</param>
		public GApplication(bool multiUse)
		{
			InitializeComponent();
			_MultiUse = multiUse;
		}

		/// <summary>
		/// Disposes the GApplication object and performs clean up operations.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				try
				{
                    Stop();
					Manager.Owner_CleanupApplication(Credentials,_Id);
				}
				catch (Exception ex)
				{
					logger.Debug("Error while cleanUp: Dispose: ",ex);
				}

				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		//----------------------------------------------------------------------------------------------- 
		// public methods
		//----------------------------------------------------------------------------------------------- 
        public void Start()
        {
            this.AddModuleDependencies();

            //start the app on a new thread
            _StartAppThread = new Thread(new ThreadStart(StartApplication));
            _StartAppThread.Name = "GAppStarterThread";
            _StartAppThread.Start();
        }

        private void AddModuleDependencies()
        {
            List<Type> types = new List<Type>();

            // add the moduledependencies for the threads to the manifest
            foreach( GThread gThread in this.Threads )
            {
                // if we've already processed this gThread type, then don't process it again
                if( !types.Contains( gThread.GetType() ) )
                {
                    this.RecursivelyAddModuleDependencies( gThread.GetType().Module, new List<System.Reflection.Module>() );
                    types.Add( gThread.GetType() );
                }
            }
        }

        private void RecursivelyAddModuleDependencies( System.Reflection.Module module, List<System.Reflection.Module> addedModules )
        {
            // if it's in the GAC, don't add it
            if( module.Assembly.GlobalAssemblyCache ) return;

            // if it's already been added, don't add it
            if( addedModules.Contains( module ) ) return;

            // if it was manually added, don't add it
            if( this.Manifest.Contains( new ModuleDependency( module ) ) ) return;

            // otherwise, add it to the manifest and to the collection
            this.Manifest.Add( new ModuleDependency( module ) );
            addedModules.Add( module );

            // and add all referenced modules
            foreach( System.Reflection.AssemblyName assemblyName in module.Assembly.GetReferencedAssemblies() )
            {
                System.Reflection.Assembly referencedAssembly = System.Reflection.Assembly.Load( assemblyName );

                foreach( System.Reflection.Module referencedModule in referencedAssembly.GetModules() )
                {
                    this.RecursivelyAddModuleDependencies( referencedModule, addedModules );
                }
            }
        }


        //TODO: Add blocking Start() method here


		/// <summary>
		/// Starts the grid application
		/// </summary>
        protected internal virtual void StartApplication()
		{
            try
            {
                if (_Running)
                {
                    logger.Info("Application is already running...");
                    return;
                }

                if (!_MultiUse)
                {
                    logger.Debug("This is not a multi-use GApplication");
                    if (!firstuse)
                        throw new InvalidOperationException("Cannot re-use a single-use GApplication.");
                }

                Init();
                logger.Debug("StartApplication GApp..." + _Id + " with " + _Threads.Count + " threads.");

                //lock it to make doubly sure that the collection enumeration is sync-ed.
                List<ThreadIdentifier> thIds = new List<ThreadIdentifier>();
                List<byte[]> serializedThreads = new List<byte[]>();
                lock (_Threads)
                {
                    logger.Debug("Enter Thread Lock to SetThreads On Manager...GApp " + _Id);
                    
                    foreach (GThread thread in _Threads)
                    {
                        if (thread.Id == -1) //only send threads which are not yet started.
                        {
                            thread.SetId(++_LastThreadId);
                            thread.SetApplication(this);

                            //create the lists to be sent out.
                            thIds.Add(new ThreadIdentifier(_Id, thread.Id));
                            serializedThreads.Add(Utils.SerializeToByteArray(thread));
                        }
                    }
                }
                //send all threads in one call.
                SendThreadsToManager(thIds, serializedThreads);

                logger.Debug("Exit Thread Lock. Finished setting threads on Manager...GApp " + _Id);

                StartGetFinishedThreads();

                firstuse = false; //this should be the only place this name should be set. to make sure the first use is over
            }
            catch (ThreadAbortException)
            {
                logger.Info("Aborting application start...");
                Thread.ResetAbort();
            }
		}

        private void SendThreadsToManager(List<ThreadIdentifier> thIds, List<byte[]> serializedThreads)
        {
            ThreadIdentifier[] threadIds = thIds.ToArray();
            byte[][] serializedThs = serializedThreads.ToArray();
            Manager.Owner_SetThreads(Credentials, threadIds, serializedThs);
        }
        
		//----------------------------------------------------------------------------------------------- 
        
		/// <summary>
		/// Starts the given thread
		/// </summary>
		/// <param name="thread">thread to start</param>
		public virtual void StartThread(GThread thread)
		{
            if (thread == null)
                throw new ArgumentNullException("thread", "GThread cannot be null.");

            /// May 10, 2006 michael@meadows.force9.co.uk: Fix for bug 1482578
            /// Prevents the client from executing StartThread on single-use 
            /// applications. StartThread should only be called for starting 
            /// on-the-fly threads to multi-use applications.
            if (!_MultiUse)
            {
                throw new InvalidOperationException("Cannot use StartThread with single-use GApplication objects.");
            }

			Init();
			Threads.Add(thread);
			SetThreadOnManager(thread);
			StartGetFinishedThreads();
		}
        
		//----------------------------------------------------------------------------------------------- 

        //stop the local thread-monitor
        private void StopApplication()
        {
            if (_Running)
            {
                //first check if app is starting.
                if (_StartAppThread != null && _StartAppThread.IsAlive)
                {
                    _StartAppThread.Abort();
                    _StartAppThread.Join();
                }

                if (_GetFinishedThreadsThread != null && _GetFinishedThreadsThread.IsAlive)
                {
                    _StopGetFinished = true;
                    _GetFinishedThreadsThread.Join();
                }
            }
            _Running = false;
        }

		/// <summary>
		/// Stops the grid application. This will also send a message to the manager to stop all
        /// threads on remote machines.
		/// </summary>
		public virtual void Stop()
		{
            StopApplication();

            Manager.Owner_StopApplication(Credentials, this._Id);
			//TODO: may be we need not have a seperate "state" for the application. if all threads are dead, app should have state: stopped as well isnt it?
			//how do we handle multi-use apps then?
		}
        
		//----------------------------------------------------------------------------------------------- 
        
		/// <summary>
		/// Aborts the given thread
		/// </summary>
		/// <param name="thread">thread to abort</param>
		internal void AbortThread(GThread thread)
		{
			if (_Running)
			{
				Manager.Owner_AbortThread(Credentials, new ThreadIdentifier(_Id, thread.Id));
			}
		}
        
		//----------------------------------------------------------------------------------------------- 
		// private methods
		//----------------------------------------------------------------------------------------------- 

		//initialises the GApplication. overrides the base class init
		new private void Init()
		{
			base.Init();

			logger.Debug(string.Format("GApp credentials appId: {0}, username: {1}", _Id, Credentials.Username));
			if (!_Initted)
			{
				logger.Debug("Not initted. Initting GApp..."+_Id);
				if (Connection == null)
				{
					throw new InvalidOperationException("No connection specified.");
				}

				_Id = Manager.Owner_CreateApplication(Credentials);
				Manager.Owner_SetApplicationManifest(Credentials, _Id, _Manifest);
				_Initted = true;
			}
			else
			{
				//the app seems to be already initt-ed
				//verify anyway. - in case of apps re-started etc.

				logger.Debug("Already initted. GApp..."+_Id);
				if (!Manager.Owner_VerifyApplication(Credentials,_Id))
				{
					logger.Debug("Couldnot verify application setup. Creating a new id for GApp..."+_Id);
					_Id = Manager.Owner_CreateApplication(Credentials);
					logger.Debug("newId for GApp..."+_Id);
				}

				logger.Debug("Checking for manifest...GApp id= " + _Id);

                if (!Manager.Owner_HasApplicationManifest(Credentials, _Id))
                {
                    //app manifest needs to be set again, since the manager doesnt have it.
                    logger.Debug("Setting manifest...GApp id= " + _Id);
                    Manager.Owner_SetApplicationManifest(Credentials, _Id, _Manifest);
                }
				
                logger.Debug("Manifest set up...GApp id= " + _Id);
			}
        
            Manager.Owner_SetApplicationName(Credentials, Id, _ApplicationName);
        }

		//----------------------------------------------------------------------------------------------- 
		
        //initialises one thread on the manager
        private void SetThreadOnManager(GThread thread)
        {
            thread.SetId(++_LastThreadId);
            thread.SetApplication(this);

            Manager.Owner_SetThread(
                Credentials,
                new ThreadIdentifier(_Id, thread.Id, thread.Priority),
                Utils.SerializeToByteArray(thread));
        }

		//----------------------------------------------------------------------------------------------- 
        
		//gets the finished threads
		private void GetFinishedThreads()
		{
			bool appCleanedup = false;
			logger.Info("GetFinishedThreads thread started.");
			try
			{
                int logCounter = 0;
				while (!_StopGetFinished)
				{
					try
					{
                        //a couple of potential issues here:
                        //1. the first and second calls may return different thread counts.
                        //since more threads may finish meanwhile.
                        //2. this sleeps for 700 ms ... which means really quick threads can't 
                        //be retrieved before 700ms.
						Thread.Sleep(700);
        
						byte[][] FinishedThreads = Manager.Owner_GetFinishedThreads(Credentials, _Id);
						_NumThreadsFinished = Manager.Owner_GetFinishedThreadCount(Credentials,_Id);

                        if (logCounter > 20 || FinishedThreads.Length > 0)
                        {
                            //print log only once in a while
                            logger.Debug("Threads finished this poll..." + FinishedThreads.Length);
                            logger.Debug("Total Threads finished so far..." + _NumThreadsFinished);
                            logCounter = 0;
                        }
                        logCounter++;

						for (int i=0; i<FinishedThreads.Length; i++)
						{
							GThread th = (GThread) Utils.DeserializeFromByteArray(FinishedThreads[i]);
							// assign [NonSerialized] members from the old local copy
							th.SetApplication(this);
							// HACK: need to change this if the user is allowed to set the id
							_Threads[th.Id] = th;
							Exception ex = Manager.Owner_GetFailedThreadException(Credentials, new ThreadIdentifier(_Id, th.Id));
                        
							if (ex == null)
							{
                                logger.Debug("Thread completed successfully:" + th.Id);
                                //raise the thread finish event
                                OnThreadFinish(th);
							}
							else
							{
                                logger.Debug("Thread failed:" + th.Id);
                                //raise the thread failed event
                                OnThreadFailed(th, ex);
							}
						}

                        // May 10, 2006 michael@meadows.force9.co.uk: Fix for bug 1485426
						if ((!_MultiUse) && (_NumThreadsFinished == Threads.Count))
						{
							logger.Debug("Application finished!"+_Id);

							if (!appCleanedup)
							{
								//clean up manager,executor.
								logger.Debug("SingleUse-Application finished cleaning up..."+_Id);
								Manager.Owner_CleanupApplication(Credentials,_Id);
								appCleanedup = true;
							}
							if (_ApplicationFinish != null)
							{
								/// January 25, 2006 tb@tbiro.com: Fix for bug 1410797 
								/// Mark the application as stopped in the database
								/// This relies on the client to mark the application as stopped on the server, 
								/// maybe not the best approach
								Manager.Owner_StopApplication(Credentials, _Id);

								logger.Debug("Raising AppFinish event (for single-use app)..."+_Id);
								_Running = false;
								try
								{
									_ApplicationFinish.BeginInvoke(null, null);
								}
								catch (Exception ex)
								{
									logger.Debug("ApplicationFinish Event-handler error: "+ex.ToString());
								}
							}
							break;
							//we break here since there is no point raising events mutliple times.!!! :kna: dec 3, 2006.
							//logger.Debug("App finished, but still looping since some-one might subscribe to this event, and we can send it to them now.");
						}
					}
					catch (SocketException se)
					{
						// lost connection to Manager
						logger.Error("Lost connection to manager. Stopping GetFinishedThreads...",se);
						break;
					}
					catch (RemotingException re)
					{
						// lost connection to Manager
						logger.Error("Lost connection to manager. Stopping GetFinishedThreads...",re);
						break;
					}
					catch (Exception e)
					{
						logger.Error("Error in GetFinishedThreads. Continuing to poll for finished threads...",e);
					}
				}
			}
			catch (ThreadAbortException)
			{
				logger.Debug("GetFinishedThreads Thread aborted.");
				Thread.ResetAbort();
			}
			catch (Exception e)
			{
				logger.Error("Error in GetFinishedThreads. GetFinishedThreads thread stopping...",e);
			}

			logger.Info("GetFinishedThreads thread exited...");
		}

        /// <summary>
        /// Fires the thread finish event.
        /// </summary>
        /// <param name="th">thread</param>
        protected virtual void OnThreadFinish(GThread thread)
        {
            if (_ThreadFinish != null)
            {
                logger.Debug("Raising ThreadFinish event...");
                try
                {
                    _ThreadFinish(thread); //TODO: Need to see the effect of calling it async. 
                }
                catch (Exception eventhandlerEx)
                {
                    // TODO: Figure out a way to not eat this exception!
                    logger.Debug("Error in ThreadFinish event handler: " + eventhandlerEx);
                }
            }
        }

        /// <summary>
        /// Fires the ThreadFailed event.
        /// </summary>
        /// <param name="th">thread</param>
        /// <param name="ex">ex</param>
        protected virtual void OnThreadFailed(GThread thread, Exception ex)
        {
            if (_ThreadFailed != null)
            {
                logger.Debug("Raising ThreadFailed event...");
                try
                {
                    _ThreadFailed(thread, ex); //TODO: Need to see the effect of calling it async. 
                }
                catch (Exception eventhandlerEx)
                {
                    // TODO: Figure out a way to not eat this exception!
                    logger.Debug("Error in ThreadFailed Event handler: " + eventhandlerEx);
                }
            }
        }

		//----------------------------------------------------------------------------------------------- 
        
		//start seperate threads to get the finished grid-threads.
		private void StartGetFinishedThreads()
		{
			if (!_Running)
			{
				logger.Debug("Starting a thread to get finished threads...");
				_StopGetFinished = false;
				_GetFinishedThreadsThread = new Thread(new ThreadStart(GetFinishedThreads));
				_GetFinishedThreadsThread.Name = "MonitorThread";
                _GetFinishedThreadsThread.Start();
			}

			_Running = true;
		}
	}
}
