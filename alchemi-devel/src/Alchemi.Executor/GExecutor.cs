#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	GExecutor.cs
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
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Security;
using System.Security.Policy;
using System.Threading;
using System.Configuration;

using Alchemi.Core;
using Alchemi.Core.Owner;
using Alchemi.Core.Executor;

using Microsoft.Win32;
using System.Collections.Generic;
using Alchemi.Core.Utility;
using Alchemi.Executor.Sandbox;

namespace Alchemi.Executor
{
    public delegate void NonDedicatedExecutingStatusChangedEventHandler();
    public delegate void GotDisconnectedEventHandler();
 
	/// <summary>
	/// The GExecutor class is an implementation of the IExecutor interface and represents an Executor node.
	/// </summary>
    public class GExecutor : GNode, IExecutor, IOwner
    {        
		// Create a logger for use in this class
		private static readonly Logger logger = new	Logger();

        private string _Id;
        private bool _Dedicated;
        private bool _AutoRevertToNDE;

        //kna: July 25, 06: 
        /*
         * Modified GExecutor : split into a number of seperate worker classes,
         * running on seperate threads.
         * Also added ability to run multiple threads at the same time.
         */
        private IDictionary <ThreadIdentifier, ExecutorWorker> _ActiveWorkers;
        private HeartbeatWorker _HeartbeatWorker;
        private NonDedicatedExecutorWorker _NDEWorker;

        internal IDictionary<string, GridAppDomain> _GridAppDomains;
        
		/// <summary>
		/// Raised when the connection status of a non-dedicated Executor is changed.
		/// </summary>
        public event NonDedicatedExecutingStatusChangedEventHandler NonDedicatedExecutingStatusChanged;
		
		/// <summary>
		/// This event is raised only when a Executor loses connection to the Manager.
		/// (This can happen in both non-dedicated and dedicated modes.
		/// </summary>
        public event GotDisconnectedEventHandler GotDisconnected;

		/// <summary>
		/// Gets the executor id
		/// </summary>
        public string Id
        {
            get { return _Id; }
        }

		/// <summary>
		/// Gets whether the executor is dedicated
		/// </summary>
        public bool Dedicated
        {
            get { return _Dedicated; }
        }

		/// <summary>
		/// Gets whether the executor is currently running a grid thread in non-dedicated mode.
		/// </summary>
        public bool ExecutingNonDedicated
        {
            get 
            {
                return (_NDEWorker == null ? false : _NDEWorker.ExecutingNonDedicated);
            }
        }

        public int HeartBeatInterval
        {
            get
            {
                return this._HeartbeatWorker.Interval;
            }
            set
            {
                this._HeartbeatWorker.Interval = value;
            }
        }

        private ExecutorInfo Info
        {
            get 
            {
				//TODO need to see how executor info. is passed to manager, when and how things are updated.
				//TODO need to discover/report these properly
                ExecutorInfo info = new ExecutorInfo();
				//info.Dedicated = this._Dedicated;
				info.Hostname = this.OwnEP.Host;
				info.OS = Environment.OSVersion.ToString();
				info.Number_of_CPUs = 1; //default for now
				info.MaxDiskSpace = 0; //need to fix
				info.MaxMemory = 0; //need to fix

				//here we can better catch the error, since it is not a show-stopper. just informational.
				try
				{
					//need to find a better way to do these things.
					RegistryKey hklm = Registry.LocalMachine;
					hklm = hklm.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0");
					info.MaxCpuPower = int.Parse(hklm.GetValue("~MHz").ToString());
					info.Architecture = hklm.GetValue("Identifier","x86").ToString(); //CPU arch.
					hklm.Close();
				}catch (Exception e)
				{
					logger.Debug("Error getting executorInfo. Continuing...",e);
				}

                return info;
            }
        }

        //----------------------------------------------------------------------------------------------- 
        // constructors
        //----------------------------------------------------------------------------------------------- 
        
		/// <summary>
		/// Creates an instance of the GExecutor with the given end points 
		/// (one for itself, and one for the manager), credentials and other options.
		/// </summary>
		/// <param name="managerEP">Manager end point</param>
		/// <param name="ownEP">Own end point</param>
		/// <param name="id">executor id</param>
		/// <param name="dedicated">Specifies whether the executor is dedicated</param>
		/// <param name="sc">Security credentials</param>
		/// <param name="baseDir">Working directory for the executor</param>
        public GExecutor(EndPoint managerEP, EndPoint ownEP, string id, bool dedicated, bool autoRevertToNDE, SecurityCredentials sc, string baseDir) : base(managerEP, ownEP, sc)
        {
            _AutoRevertToNDE = autoRevertToNDE;
            _Dedicated = dedicated;
            _Id = id;

            if (_Id == null || _Id == "")
            {
				logger.Info("Registering new executor");
                _Id = Manager.Executor_RegisterNewExecutor(Credentials, null, Info);
				logger.Info("Successfully Registered new executor:"+_Id);
            }

            _GridAppDomains = new System.Collections.Generic.Dictionary<string, GridAppDomain>();
            _ActiveWorkers = new System.Collections.Generic.Dictionary<ThreadIdentifier, ExecutorWorker>();

			//handle exception since we want to connect to the manager 
			//even if it doesnt succeed the first time.
			//that is, we need to handle InvalidExecutor and ConnectBack Exceptions.
            try 
            {
                try
                {
                    ConnectToManager();
                }
                catch (InvalidExecutorException)
                {
					logger.Info("Invalid executor! Registering new executor again...");

                    _Id = Manager.Executor_RegisterNewExecutor(Credentials, null, Info);
                    
					logger.Info("New ExecutorID = " + _Id);
                    ConnectToManager();
                }
            }
            catch (ConnectBackException) 
            {
                if (_AutoRevertToNDE)
                {
                    logger.Warn("Couldn't connect as dedicated executor. Reverting to non-dedicated executor. ConnectBackException");
                    _Dedicated = false;
                    ConnectToManager();
                }
            }

			//for non-dedicated mode, the heart-beat thread will be started when execution is started
            if (_Dedicated)
            {
				logger.Debug("Dedicated mode: starting heart-beat thread");
				StartHeartBeatThread(HeartbeatWorker.DEFAULT_INTERVAL);
            }
        }

		private void StartHeartBeatThread(int interval)
		{
            _HeartbeatWorker= new HeartbeatWorker(this);
            _HeartbeatWorker.Interval = interval;
            _HeartbeatWorker.Start();
		}

        private void StopHeartBeat()
        {
			logger.Debug("Stopping heartbeat thread...");
            _HeartbeatWorker.Stop();
            _HeartbeatWorker = null;
			logger.Debug("HeartBeat stopped.");
        }

        //----------------------------------------------------------------------------------------------- 
        // public methods
        //----------------------------------------------------------------------------------------------- 

		/// <summary>
		/// Abort all running threads and Disconnect from the Manager. 
		/// </summary>
        public void Disconnect()
        {
            if (!_Dedicated)
                StopNonDedicatedExecuting(); //also stops the heart-beat
            else
                StopHeartBeat();

			//handle disconnection error, since we dont want that to hold up the disconnect process.
			try
			{
				Manager.Executor_DisconnectExecutor(Credentials, _Id);
				logger.Debug("Disconnected executor.");
			}
			catch (SocketException se)
			{
				logger.Debug("Error trying to disconnect from Manager. Continuing disconnection process...",se);
			}
			catch (System.Runtime.Remoting.RemotingException re)
			{
				logger.Debug("Error trying to disconnect from Manager. Continuing disconnection process...",re);
			}
			catch (Exception ex)
			{
				logger.Debug("Error trying to disconnect from Manager. Continuing disconnection process...",ex);
			}

            RelinquishIncompleteThreads();
            UnRemoteSelf();

			logger.Debug("Unloading AppDomains on this executor...");
            lock (_GridAppDomains)
            {
                foreach (object gad in _GridAppDomains.Values)
                {
                    //handle error while unloading appDomain and continue...
                    try
                    {
                        AppDomain.Unload(((GridAppDomain)gad).Domain);
                    }
                    catch (Exception e)
                    {
                        logger.Error("Error unloading appDomain. Continuing disconnection process...", e);
                    }
                }
                _GridAppDomains.Clear();
            }
            //TODO: file clean up via garbage-collector type thread.
        }

        //-----------------------------------------------------------------------------------------------    

		/// <summary>
		/// StartApplication execution in non-dedicated mode.
		/// </summary>
		/// <param name="emptyThreadInterval">Interval to wait in between attempts to get a thread from the manager</param>
        public void StartNonDedicatedExecuting(int emptyThreadInterval)
        {
            if (!_Dedicated & !ExecutingNonDedicated)
            {
				logger.Debug("Starting Non-dedicatedMonitor thread");
                _NDEWorker = new NonDedicatedExecutorWorker(this);
                _NDEWorker.Start();

                //raise the nde status change event.
                OnNonDedicatedExecutingStatusChanged();

				//start the heartbeat thread.
				logger.Debug("Starting heart-beat thread: non-dedicated mode");
				StartHeartBeatThread(HeartbeatWorker.DEFAULT_INTERVAL);
			}
        }

        //-----------------------------------------------------------------------------------------------    

		/// <summary>
		/// Stops execution in non-dedicated mode.
		/// </summary>
        public void StopNonDedicatedExecuting()
        {
            if (!_Dedicated & ExecutingNonDedicated)
            {
				logger.Debug("Stopping Non-dedicated execution monitor thread...");
                this._NDEWorker.Stop();
                _NDEWorker = null;
				
				logger.Debug("Raising event: NonDedicatedExecutingStatusChanged");
                OnNonDedicatedExecutingStatusChanged();
            }
        }

        internal void OnNonDedicatedExecutingStatusChanged()
        {
            try
            {
                if (NonDedicatedExecutingStatusChanged != null)
                    NonDedicatedExecutingStatusChanged();
            }
            catch (Exception ex)
            {
                logger.Debug("Error in NonDedicatedExecutingStatusChanged event-handler: " + ex.ToString());
            }
        }

        //-----------------------------------------------------------------------------------------------    

		/// <summary>
		/// Pings the executor node. If this method runs successfully, it means that the remoting set up 
		/// between the manager and executor is working.
		/// </summary>
        public void PingExecutor()
        {
            // for testing communication
			logger.Debug("Executor pinged successfully");
        }
    
        //-----------------------------------------------------------------------------------------------    

		/// <summary>
		/// Executes the given thread
		/// </summary>
		/// <param name="ti">ThreadIdentifier representing the GridThread to be executed on this node.</param>
        public void Manager_ExecuteThread(ThreadIdentifier ti)
        {
            lock (_ActiveWorkers)
            {
                ExecutorWorker worker = null;
                if (_ActiveWorkers.ContainsKey(ti))
                {
                    //stop any existing instances of the user's thread
                    worker = _ActiveWorkers[ti];
                    worker.Stop();
                }

                worker = new ExecutorWorker(this, ti);
                _ActiveWorkers[ti] = worker;
                worker.Start();
            }
        }

		/// <summary>
		/// Cleans up all the application related files on the executor.
		/// </summary>
		/// <param name="appid">Application Id</param>
		public void Manager_CleanupApplication(string appid)
		{
			try
			{
				//unload the app domain and clean up all the files here, for this application.
				try
				{
                    if (_GridAppDomains != null)
                    {
                        lock (_GridAppDomains)
                        {
                            if (_GridAppDomains.ContainsKey(appid))
                            {
                                GridAppDomain gad = _GridAppDomains[appid];
                                if (gad != null)
                                {
                                    logger.Debug("Unloading AppDomain for application:" + appid);
                                    AppDomain.Unload(gad.Domain);
                                    _GridAppDomains.Remove(appid);
                                }
                            }
                            else
                            {
                                logger.Debug("Appdomain not found in collection - " + appid);
                            }
                        }
                    }
				}
				catch (Exception e)
				{
					logger.Debug("Error unloading appdomain:",e);
				}
                Cleanup(appid);
			}
			catch (Exception e)
			{
				//just debug info. to see why clean up failed.
				logger.Debug("Clean up app error: "+e.Message);
			}
		}

        //----------------------------------------------------------------------------------------------- 
        // private methods
        //----------------------------------------------------------------------------------------------- 

        private void Cleanup(string appId)
        {
            Directory.Delete(ExecutorUtil.GetApplicationDirectory(appId), true);
        }

        private void ConnectToManager()
        {
            if (_Dedicated)
            {
				logger.Debug("Connecting to Manager dedicated...");
                Manager.Executor_ConnectDedicatedExecutor(Credentials, _Id, OwnEP);
            }
            else
            {
				logger.Debug("Connecting to Manager NON-dedicated...");
                Manager.Executor_ConnectNonDedicatedExecutor(Credentials, _Id, OwnEP);
            }
        }

        private void RelinquishIncompleteThreads()
        {
            if (_ActiveWorkers != null)
            {
                ThreadIdentifier[] keys = null;
                lock (_ActiveWorkers)
                {
                    keys = new ThreadIdentifier[_ActiveWorkers.Keys.Count];
                    //since we remove it from the list in this method!
                    //copy active worker 'keys' into another collection
                    //to avoid concurrent modification exception
                    _ActiveWorkers.Keys.CopyTo(keys, 0);
                }
                if (keys != null)
                {
                    foreach (ThreadIdentifier ti in keys)
                    {
                        if (ti != null)
                        {
                            try
                            {
                                logger.Debug("Relinquishing incomplete thread:" + ti.UniqueId);
                                Manager_AbortThread(ti);
                                Manager.Executor_RelinquishThread(Credentials, ti);
                            }
                            catch (Exception ex)
                            {
                                logger.Warn("Error relinquishing thread : " + ti.UniqueId + ", " + ex.Message);
                            }
                        }
                    }
                }
            }
        }

		/// <summary>
		/// Abort the given thread.
		/// </summary>
		/// <param name="ti">ThreadIdentifier object representing the GridThread to be aborted</param>
        public void Manager_AbortThread(ThreadIdentifier ti)
        {
            if (_ActiveWorkers != null)
            {
                lock (_ActiveWorkers)
                {
                    if (_ActiveWorkers.ContainsKey(ti))
                    {
                        _ActiveWorkers[ti].Stop();
                        _ActiveWorkers.Remove(ti);
                    }
                }
            }
		}

        internal void DisconnectNDE()
        {
            UnRemoteSelf();
        }

        internal void OnGotDisconnected()
        {
            try
            {
                if (GotDisconnected != null)
                {
                    logger.Debug("Raising event: Executor GotDisconnected.");
                    GotDisconnected();
                }
            }
            catch { } //it is always better to catch exceptions on eventhandlers, because we don't know what may be in it.
        }
    }
}

