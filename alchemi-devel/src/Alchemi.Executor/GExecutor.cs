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
using System.Management;

namespace Alchemi.Executor
{
    public delegate void NonDedicatedExecutingStatusChangedEventHandler();
    public delegate void GotDisconnectedEventHandler();

    //kna: July 25, 06: 
    /*
     * Modified GExecutor : split into a number of seperate worker classes,
     * running on seperate threads.
     * Also added ability to run multiple threads at the same time.
     */
	/// <summary>
	/// The GExecutor class is an implementation of the IExecutor interface and represents an Executor node.
	/// </summary>
    [System.ServiceModel.ServiceBehavior(ConcurrencyMode = System.ServiceModel.ConcurrencyMode.Multiple, InstanceContextMode = System.ServiceModel.InstanceContextMode.Single)]
    public class GExecutor : GNode, IExecutor, IOwner
    {        
		// Create a logger for use in this class
		private static readonly Logger logger = new	Logger();

        private bool _AutoRevertToNDE;
        private IDictionary <ThreadIdentifier, ExecutorWorker> _ActiveWorkers;
        private HeartbeatWorker _HeartbeatWorker;
        private NonDedicatedExecutorWorker _NDEWorker;

        internal IDictionary<string, GridAppDomain> _GridAppDomains;

        private Exception _ConnectionException;


        #region Constructor
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
        public GExecutor(EndPoint managerEP, EndPoint ownEP, string id, bool dedicated, bool autoRevertToNDE, SecurityCredentials sc, string baseDir)
            : base(managerEP, ownEP, sc)
        {
            _AutoRevertToNDE = autoRevertToNDE;
            _Dedicated = dedicated;
            _Id = id;

            if (String.IsNullOrEmpty(_Id))
            {
                logger.Info("Registering new executor");
                _Id = Manager.Executor_RegisterNewExecutor(this.Credentials, null, this.Info);
                logger.Info("Successfully Registered new executor:" + _Id);
            }

            _GridAppDomains = new Dictionary<string, GridAppDomain>();
            _ActiveWorkers = new Dictionary<ThreadIdentifier, ExecutorWorker>();

            //handle exception since we want to connect to the manager 
            //even if it doesnt succeed the first time.
            //that is, we need to handle InvalidExecutor and ConnectBack Exceptions.
            //WCF requires this to execute in another thread.
            Thread t = new Thread(new ThreadStart(DoConnectThread));
            t.Start();            
        }
        #endregion


        
        #region Property - Id
        private string _Id;
        /// <summary>
        /// Gets the executor id
        /// </summary>
        public string Id
        {
            get { return _Id; }
        } 
        #endregion


        #region Property - Dedicated
        private bool _Dedicated;
        /// <summary>
        /// Gets whether the executor is dedicated
        /// </summary>
        public bool Dedicated
        {
            get { return _Dedicated; }
        } 
        #endregion


        #region Property - ExecutingNonDedicated
        /// <summary>
        /// Gets whether the executor is currently running a grid thread in non-dedicated mode.
        /// </summary>
        public bool ExecutingNonDedicated
        {
            get
            {
                if (_NDEWorker == null)
                {
                    return false;
                }
                return _NDEWorker.ExecutingNonDedicated;
            }
        } 
        #endregion


        #region Property - HeartBeatInterval
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
        #endregion


        #region Property - Info
        private ExecutorInfo Info
        {
            get
            {
                //TODO need to see how executor info. is passed to manager, when and how things are updated.
                //TODO need to discover/report these properly
                ExecutorInfo info = new ExecutorInfo();
                //info.Dedicated = this._Dedicated;
                info.Hostname = OwnEP.Host;
                info.OS = Environment.OSVersion.ToString();
                info.NumberOfCpus = Environment.ProcessorCount;

                //here we can better catch the error, since it is not a show-stopper. just informational.
                try
                {
                    //Maybe better to use only ManagementObjects? We get much more info like number of cores, etc...
                    //and also we get EVERYTHING!
                    //ManagementObject processor = new
                    //    ManagementObject("Win32_Processor");
                    //processor.Get();

                    //Probably useful here are : MaxClockSpeed/CurrentClockSpeed, 
                    //NumberOfCores/NumberOfLogicalProcessors, Architecture (but this is an int16 :-(, so can't replace just yet).

                    //this is probably broken for network shares...
                    ManagementObject disk = new
                        ManagementObject("win32_logicaldisk.deviceid='" +
                            Path.GetPathRoot(ExecutorUtil.BaseDirectory).Substring(0, 2) + "'");
                    disk.Get();
                    // free disk space [bytes]
                    info.MaxDiskSpace = Convert.ToSingle(disk["FreeSpace"]);
                    disk.Dispose(); //needed?

                    // the total available memory in the executor machine [bytes]
                    ManagementClass mc = new ManagementClass("Win32_PhysicalMemory");
                    ManagementObjectCollection col = mc.GetInstances();
                    float maxMemory = 0.0f;
                    foreach(ManagementObject mo in col)
                    {
                        foreach(PropertyData pd in mo.Properties)
                        {
                            if(pd.Name == "Capacity")
                            {
                                maxMemory += Convert.ToSingle(pd.Value);
                            }
                        }
                    }
                    info.MaxMemory = maxMemory;

                    //need to find a better way to do these things.
                    RegistryKey hklm = Registry.LocalMachine;
                    hklm = hklm.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0");
                    int cpuPower = int.Parse(hklm.GetValue("~MHz").ToString());
                    info.MaxCpuPower = cpuPower * info.NumberOfCpus;
                    info.Architecture = hklm.GetValue("Identifier", "x86").ToString(); //CPU arch.
                    hklm.Close();
                }
                catch (Exception e)
                {
                    logger.Debug("Error getting executorInfo. Continuing...", e);
                }

                return info;
            }
        } 
        #endregion



        #region Method - StartHeartBeat
        private void StartHeartBeat(int interval)
        {
            _HeartbeatWorker = new HeartbeatWorker(this);
            _HeartbeatWorker.Interval = interval;
            _HeartbeatWorker.Start();
        } 
        #endregion


        #region Method - StopHeartBeat
        private void StopHeartBeat()
        {
            logger.Debug("Stopping heartbeat thread...");
            _HeartbeatWorker.Stop();
            _HeartbeatWorker = null;
            logger.Debug("HeartBeat stopped.");
        } 
        #endregion


        #region Method - Disconnect
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
                logger.Debug("Error trying to disconnect from Manager. Continuing disconnection process...", se);
            }
            catch (RemotingException re)
            {
                logger.Debug("Error trying to disconnect from Manager. Continuing disconnection process...", re);
            }
            catch (Exception ex)
            {
                logger.Debug("Error trying to disconnect from Manager. Continuing disconnection process...", ex);
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

        #endregion


        #region Method - StartNonDedicatedExecuting
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
                StartHeartBeat(HeartbeatWorker.DEFAULT_INTERVAL);
            }
        } 
        #endregion


        #region Method - StopNonDedicatedExecuting
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
        #endregion



        #region Method - PingExecutor
        /// <summary>
        /// Pings the executor node. If this method runs successfully, it means that the remoting set up 
        /// between the manager and executor is working.
        /// </summary>
        public void PingExecutor()
        {
            // for testing communication
            logger.Debug("Executor pinged successfully");
        } 
        #endregion


        #region Method - Manager_ExecuteThread
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
        #endregion


        #region Method - Manager_CleanupApplication
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
                    logger.Debug("Error unloading appdomain:", e);
                }
                Cleanup(appid);
            }
            catch (Exception e)
            {
                //just debug info. to see why clean up failed.
                logger.Debug("Clean up app error: " + e.Message);
            }
        } 
        #endregion


        #region Method - Manager_AbortThread
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
        #endregion


        #region Method - DisconnectNDE
        internal void DisconnectNDE()
        {
            UnRemoteSelf();
        } 
        #endregion



        #region Method - Cleanup
        private void Cleanup(string appId)
        {
            Directory.Delete(ExecutorUtil.GetApplicationDirectory(appId), true);
        } 
        #endregion


        #region Method - ConnectToManager
        private Exception ConnectToManager()
        {
            Exception ret = null;
            if (_Dedicated)
            {
                logger.Debug("Connecting to Manager dedicated...");
                ret = Manager.Executor_ConnectDedicatedExecutor(Credentials, _Id, OwnEP);
                
            }
            else
            {
                logger.Debug("Connecting to Manager NON-dedicated...");
                ret = Manager.Executor_ConnectNonDedicatedExecutor(Credentials, _Id, OwnEP);
            }

            if (ret != null && ret is RemoteException)
                ret = (ret as RemoteException).OriginalRemoteException;

            return ret;
        }
        #endregion

        #region Method - DoConnectThread
        private void DoConnectThread()
        {
            Exception ex = ConnectToManager();

            if (ex != null || ex is InvalidExecutorException)
            {
                logger.Info("Invalid executor! Registering new executor again...");

                _Id = Manager.Executor_RegisterNewExecutor(Credentials, null, Info);

                logger.Info("New ExecutorID = " + _Id);
                ex = null;
                ex = ConnectToManager();
            }

            if (ex != null || ex is ConnectBackException)
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
                StartHeartBeat(HeartbeatWorker.DEFAULT_INTERVAL);
            }
        }
        #endregion

        #region Method - RelinquishIncompleteThreads
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
        #endregion



        #region Event - NonDedicatedExecutingStatusChanged
        private event NonDedicatedExecutingStatusChangedEventHandler _NonDedicatedExecutingStatusChanged;
        /// <summary>
        /// Raised when the connection status of a non-dedicated Executor is changed.
        /// </summary>
        public event NonDedicatedExecutingStatusChangedEventHandler NonDedicatedExecutingStatusChanged
        {
            add { _NonDedicatedExecutingStatusChanged += value; }
            remove { _NonDedicatedExecutingStatusChanged -= value; }
        }

        /// <summary>
        /// Raises the NonDedicatedExecutingStatusChanged event
        /// </summary>
        internal void OnNonDedicatedExecutingStatusChanged()
        {
            try
            {
                if (_NonDedicatedExecutingStatusChanged != null)
                {
                    _NonDedicatedExecutingStatusChanged();
                }
            }
            catch (Exception ex)
            {
                logger.Debug("Error in NonDedicatedExecutingStatusChanged event-handler: " + ex.ToString());
            }
        }
        #endregion


        #region Event - GotDisconnected
        private event GotDisconnectedEventHandler _GotDisconnected;
        /// <summary>
        /// This event is raised only when a Executor loses connection to the Manager.
        /// (This can happen in both non-dedicated and dedicated modes.
        /// </summary>
        public event GotDisconnectedEventHandler GotDisconnected
        {
            add { _GotDisconnected += value; }
            remove { _GotDisconnected -= value; }
        }

        internal void OnGotDisconnected()
        {
            try
            {
                if (_GotDisconnected != null)
                {
                    logger.Debug("Raising event: Executor GotDisconnected.");
                    _GotDisconnected();
                }
            }
            catch
            {
                // it is always better to catch exceptions on eventhandlers,
                // because we don't know what may be in it.
            }
        } 
        #endregion
    }
}

