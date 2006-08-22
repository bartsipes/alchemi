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
		public event GotDisconnectedEventHandler GotDisconnected;
		public event NonDedicatedExecutingStatusChangedEventHandler NonDedicatedExecutingStatusChanged;
		
		public Configuration Config=null;
		private GExecutor _Executor = null;

		// Create a logger for use in this class
		private static readonly Logger logger = new Logger();

		public ExecutorContainer()
		{
		}

		public bool Connected
		{
			get { return (_Executor == null? false : true);}
		}

		public void Connect() //TODO FIX RECONNECT PROPERLY.
		{
			logger.Info("Connecting....");
			EndPoint rep = new EndPoint(
				Config.ManagerHost,
				Config.ManagerPort,
				RemotingMechanism.TcpBinary
				);

			logger.Debug("Created remote-end-point");
			EndPoint oep = new EndPoint(Config.OwnPort,RemotingMechanism.TcpBinary);

			logger.Debug("Created own-end-point");
			// connect to manager
			_Executor = new GExecutor(rep, oep, Config.Id, Config.Dedicated, Config.AutoRevertToNDE, 
                new SecurityCredentials(Config.Username, Config.Password), ExecutorUtil.BaseDirectory);

			Config.ConnectVerified = true;
			Config.Id = _Executor.Id;
			Config.Dedicated = _Executor.Dedicated;

			_Executor.GotDisconnected += new GotDisconnectedEventHandler(GExecutor_GotDisconnected);
			_Executor.NonDedicatedExecutingStatusChanged += new NonDedicatedExecutingStatusChangedEventHandler(GExecutor_NonDedicatedExecutingStatusChanged);

			Config.Slz();

			logger.Info("Connected successfully.");
		}

		/// <summary>
		/// Reconnect to the Manager.
		/// </summary>
		public void Reconnect()
		{
			Reconnect(Config.RetryMax ,Config.RetryInterval);
		}

		/// <summary>
		/// Try to Reconnect to the Manager.
		///
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

				logger.Debug ("Attempting to reconnect ... attempt: "+(retryCount+1));
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
			if (_Executor!=null)
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

		public void Disconnect()
		{
			if (Connected)
			{
                _Executor.Disconnect();
				_Executor = null;
				logger.Info("Disconnected successfully.");
			}
		}

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
						Config = Configuration.GetConfiguration();
					}
					logger.Debug("Using configuration from Alchemi.Executor.config.xml ...");
				}
				catch (Exception ex)
				{
					logger.Debug("Error getting existing config. Continuing with default config...",ex);
					useDefault = true;
				}
			}

			//this needs to be a seperate if-block, 
            //since we may have a problem getting existing config. then we use default
			if (useDefault)
			{
				Config = new Configuration();
				logger.Debug("Using default configuration...");
			}
		}

		/// <summary>
		/// Returns a name specifying whether the Connection has been verified previously.
		/// </summary>
		public bool ConnectVerified
		{
			get { return Config.ConnectVerified; }
		}

		/// <summary>
		/// Stops the Executor Container
		/// </summary>
		public void Stop()
		{
			if (Config!=null)
			{
				Config.Slz();
			}
			
			Disconnect();

            Cleanup();
		}

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

		/// <summary>
		/// Starts the Executor Container
		/// </summary>
		public void Start()
		{
			logger.Debug("debug mode: curdir env="+Environment.CurrentDirectory + " app-base="+AppDomain.CurrentDomain.BaseDirectory);

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

		private void GExecutor_GotDisconnected()
		{
			//always handle errors when raising events
			try
			{
				//bubble the event to whoever handles this.
				if (GotDisconnected!=null)
					GotDisconnected();
			}catch {}
		} //TODO REVIEW Catch ALLs everywhere

		private void GExecutor_NonDedicatedExecutingStatusChanged()
		{
			//always handle errors when raising events
			try
			{
				//bubble the event up
				if (NonDedicatedExecutingStatusChanged!=null)
					NonDedicatedExecutingStatusChanged();
			}catch {}
		}

		public void UpdateHeartBeatBInterval(int newHBInterval)
		{
			if (_Executor != null)
			{
	            _Executor.HeartBeatInterval = newHBInterval;
			}
		}

	}
}
