#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	ExecutorContainer.cs
* Project		:	Alchemi Core
* Created on	:	Aug 2006
* Copyright		:	Copyright © 2006 The University of Melbourne
*					This technology has been developed with the support of 
*					the Australian Research Council and the University of Melbourne
*					research grants as part of the Gridbus Project
*					within GRIDS Laboratory at the University of Melbourne, Australia.
* Author         :  Krishna Nadiminti (kna@csse.unimelb.edu.au) and Rajkumar Buyya (raj@csse.unimelb.edu.au)  
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
using System.Configuration;
using System.Threading;
using Alchemi.Core;
using Alchemi.Core.Utility;
using System.IO;

namespace Alchemi.Executor
{	
	/// <summary>
	/// Summary description for ExecutorContainer.
	/// </summary>
	public class ExecutorContainer
	{
        // Create a logger for use in this class
        private static readonly Logger logger = new Logger();

        public Configuration _Config = null;
        private GExecutor _Executor = null;

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutorContainer"/> class.
        /// </summary>
        public ExecutorContainer()
        {
        }
        #endregion


        #region Event - GotDisconnected
        private event GotDisconnectedEventHandler _GotDisconnected;
        /// <summary>
        /// Occurs when the Executor becomes disconnected from the Manager.
        /// </summary>
        public event GotDisconnectedEventHandler GotDisconnected
        {
            add { _GotDisconnected += value; }
            remove { _GotDisconnected -= value; }
        }

        protected virtual void OnGotDisconnected()
        {
            if (_GotDisconnected != null)
            {
                _GotDisconnected();
            }
        } 
        #endregion


        #region Event - NonDedicatedExecutingStatusChanged
        private event NonDedicatedExecutingStatusChangedEventHandler _NonDedicatedExecutingStatusChanged;
        public event NonDedicatedExecutingStatusChangedEventHandler NonDedicatedExecutingStatusChanged
        {
            add { _NonDedicatedExecutingStatusChanged += value; }
            remove { _NonDedicatedExecutingStatusChanged -= value; }
        }

        protected virtual void OnNonDedicatedExecutingStatusChanged()
        {
            if (_NonDedicatedExecutingStatusChanged != null)
            {
                _NonDedicatedExecutingStatusChanged();
            }
        } 
        #endregion
		


        #region Property - Connected
        public bool Connected
        {
            get
            {
                return (_Executor == null ? false : true);
            }
        } 
        #endregion


        #region Property - ConnectVerified
        /// <summary>
        /// Returns whether the Connection has been verified previously.
        /// </summary>
        public bool ConnectVerified
        {
            get { return _Config.ConnectVerified; }
        }
        #endregion



        #region Method - Connect
        /// <summary>
        /// Connect to the Manager
        /// </summary>
        public void Connect() 
        {
            //TODO FIX RECONNECT PROPERLY.

            logger.Info("Connecting....");
            EndPoint rep = new EndPoint(
                _Config.ManagerHost,
                _Config.ManagerPort,
                RemotingMechanism.TcpBinary
                );

            logger.Debug("Created remote-end-point");
            EndPoint oep = new EndPoint(_Config.OwnPort, RemotingMechanism.TcpBinary);

            logger.Debug("Created own-end-point");
            // connect to manager
            _Executor = new GExecutor(rep, oep, _Config.Id, _Config.Dedicated, _Config.AutoRevertToNDE,
                new SecurityCredentials(_Config.Username, _Config.Password), ExecutorUtil.BaseDirectory);

            _Config.ConnectVerified = true;
            _Config.Id = _Executor.Id;
            _Config.Dedicated = _Executor.Dedicated;

            _Executor.GotDisconnected += new GotDisconnectedEventHandler(GExecutor_GotDisconnected);
            _Executor.NonDedicatedExecutingStatusChanged += new NonDedicatedExecutingStatusChangedEventHandler(GExecutor_NonDedicatedExecutingStatusChanged);

            _Config.Serialize();

            logger.Info("Connected successfully.");
        } 
        #endregion


        #region Method - Reconnect
        /// <summary>
        /// Reconnect to the Manager.
        /// </summary>
        public void Reconnect()
        {
            Reconnect(_Config.RetryMax, _Config.RetryInterval);
        } 
        #endregion

        #region Method - Reconnect
        /// <summary>
        /// Try to Reconnect to the Manager.
        /// </summary>
        /// <param name="maxRetryCount">Maximum number of times to retry, if connection fails. -1 signifies: try forever.</param>
        /// <param name="retryInterval">Retry connection after every 'retryInterval' seconds.</param>
        public void Reconnect(int maxRetryCount, int retryInterval)
        {
            int retryCount = 0;
            const int DEFAULT_RETRY_INTERVAL = 30; //30 seconds

            //first unregister channel etc... wait for a bit and then start  again.
            try
            {
                if (_Executor != null)
                    _Executor.UnRemoteSelf();
            }
            catch (Exception ex)
            {
                logger.Warn(" Error unremoting self when trying to Re-connect. ", ex);
            }

            while (true)
            {
                //play safe & also wait for a bit first...
                if (retryInterval < 0 || retryInterval > System.Int32.MaxValue)
                    retryInterval = DEFAULT_RETRY_INTERVAL;

                Thread.Sleep(retryInterval * 1000); //convert to milliseconds

                if (maxRetryCount >= 0 && retryCount < maxRetryCount)
                    break;

                logger.Debug("Attempting to reconnect ... attempt: " + (retryCount + 1));
                retryCount++;
                try //handle the error since we want to retry later.
                {
                    Connect();
                }
                catch (Exception ex)
                {
                    //ignore the error. retry later.
                    logger.Debug("Error re-connecting attempt: " + retryCount, ex);
                }

                if (Connected)
                    break;
            }

            //if Executor is null, then it is not Connected. The Connected property actually checks for that.
            if (_Executor != null)
            {
                if (_Executor.Dedicated)
                {
                    logger.Debug("Reconnected successfully.[Dedicated mode.]");
                }
                else //not dedicated...
                {
                    logger.Debug("Reconnected successfully.[Non-dedicated mode.]");
                    _Executor.StartNonDedicatedExecuting(1000);
                }
            }
        } 
        #endregion


        #region Method - Disconnect
        /// <summary>
        /// Disconnects from the Manager
        /// </summary>
        public void Disconnect()
        {
            if (Connected)
            {
                _Executor.Disconnect();
                _Executor = null;
                logger.Info("Disconnected successfully.");
            }
        } 
        #endregion


        #region Method - ReadConfig
        /// <summary>
        /// Read the configuration file.
        /// </summary>
        /// <param name="useDefault"></param>
        public void ReadConfig(bool useDefault)
        {
            if (!useDefault)
            {
                //handle the error and lets use default if the config cannot be found.
                try
                {
                    lock (this) // since we may reload the config dynamically from another thread, if needed.
                    {
                        _Config = Configuration.Deserialize();
                    }
                    logger.Debug("Using configuration from Alchemi.Executor.config.xml ...");
                }
                catch (Exception ex)
                {
                    logger.Debug("Error getting existing config. Continuing with default config...", ex);
                    useDefault = true;
                }
            }

            //this needs to be a seperate if-block, 
            //since we may have a problem getting existing config. then we use default
            if (useDefault)
            {
                _Config = new Configuration();
                logger.Debug("Using default configuration...");
            }
        } 
        #endregion


        #region Method - Start
        /// <summary>
        /// Starts the Executor Container
        /// </summary>
        public void Start()
        {
            logger.Debug("debug mode: curdir env=" + Environment.CurrentDirectory + " app-base=" + AppDomain.CurrentDomain.BaseDirectory);

            ReadConfig(false);

            if (ConnectVerified)
            {
                logger.Info("Using last verified configuration ...");
                Connect();
            }
            else
            {
                if (!Connected)
                    Connect();
            }
        }
        #endregion


        #region Method - Stop
        /// <summary>
        /// Stops the Executor Container
        /// </summary>
        public void Stop()
        {
            if (_Config != null)
            {
                _Config.Serialize();
            }

            Disconnect();

            Cleanup();
        } 
        #endregion


        #region Method - Cleanup
        private void Cleanup()
        {
            //handle errors since clean up shouldnt hold up the other actions.
            try
            {
                logger.Debug("Cleaning up all apps before disconnect...");
                string datDir = ExecutorUtil.DataDirectory;
                string[] dirs = Directory.GetDirectories(datDir);
                foreach (string s in dirs)
                {
                    //handle error since clean up shouldnt hold up the other actions.
                    try
                    {
                        Directory.Delete(s, true);
                        logger.Debug("Deleted directory: " + s);
                    }
                    catch { }
                }
                logger.Debug("Clean up all apps done.");
            }
            catch (Exception e)
            {
                logger.Debug("Clean up error. Continuing...", e);
            }
        } 
        #endregion


        #region Method - UpdateHeartBeatInterval
        public void UpdateHeartBeatInterval(int newInterval)
        {
            if (_Executor != null)
            {
                _Executor.HeartBeatInterval = newInterval;
            }
        }
        #endregion



        #region Method - GExecutor_GotDisconnected
        private void GExecutor_GotDisconnected()
        {
            //always handle errors when raising events
            try
            {
                //bubble the event to whoever handles this.
                OnGotDisconnected();
            }
            catch
            {
            }
        } //TODO REVIEW Catch ALLs everywhere 
        #endregion


        #region Method - GExecutor_NonDedicatedExecutingStatusChanged
        private void GExecutor_NonDedicatedExecutingStatusChanged()
        {
            //always handle errors when raising events
            try
            {
                //bubble the event up
                OnNonDedicatedExecutingStatusChanged();
            }
            catch
            {
            }
        } 
        #endregion

	}
}
