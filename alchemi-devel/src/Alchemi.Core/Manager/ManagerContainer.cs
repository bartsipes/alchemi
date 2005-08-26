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

using System;
using System.Data;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;
using ThreadState = Alchemi.Core.Owner.ThreadState;

namespace Alchemi.Core.Manager
{
	/// <summary>
	/// Event handler for status changes during Manager startup.
	/// </summary>
	public delegate void ManagerStartStatusEventHandler(string statusMessage, int percentDone);

	/// <summary>
	/// This class acts as a container for the manager and the applications,executors collections.
	/// </summary>
    public class ManagerContainer
    {

		//----------------------------------------------------------------------------------------------- 
        // member variables
        //----------------------------------------------------------------------------------------------- 
		
		/// <summary>
		/// Manager Configuration
		/// </summary>
		public Configuration Config=null;

		/// <summary>
		/// The configuration file used to set remoting parameters for the Manager.
		/// </summary>
		public string RemotingConfigFile = "Alchemi.Manager.exe.config";

		/// <summary>
		/// Specifies if the Manager is started.
		/// </summary>
		public bool Started = false;

		private TcpChannel _Chnl;

		private Thread _DedicatedSchedulerThread;
        private Thread _WatchDogThread;

        private MExecutorCollection _Executors = new MExecutorCollection();
        private MApplicationCollection _Applications = new MApplicationCollection();

		private bool _stopWatchDog = false;
		private bool _stopScheduler = false;

		private static readonly Logger logger = new Logger();

		//----------------------------------------------------------------------------------------------
		// events
		//----------------------------------------------------------------------------------------------

		/// <summary>
		/// This event is raised when the start is called, to notify the progress of the "Start" call.
		/// </summary>
		public static event ManagerStartStatusEventHandler ManagerStartEvent;
    
        //----------------------------------------------------------------------------------------------- 
        // constructors
        //----------------------------------------------------------------------------------------------- 

		/// <summary>
		/// Creates an instance of the ManagerContainer.
		/// </summary>
		public ManagerContainer()
		{
			Started = false;	
		}

		/// <summary>
		/// Starts the Manager
		/// </summary>
        public void Start()
        {
			if (Started)
				return;

            try
            {
				if (Config==null)
				{
					ReadConfig(false);
				}

				OwnEndPoint ownEP = new OwnEndPoint(
					Config.OwnPort,
					RemotingMechanism.TcpBinary
					);

				logger.Debug("Configuring remoting");

				if (ManagerStartEvent!=null)
					ManagerStartEvent("Configuring remoting",10);

            	RemotingConfiguration.Configure(AppDomain.CurrentDomain.BaseDirectory+RemotingConfigFile);

				//TODO: for hierarchical grids
//				RemoteEndPoint managerEP = null;
//				if (Config.Intermediate)
//				{
//					managerEP = new RemoteEndPoint(
//						Config.ManagerHost, 
//						Config.ManagerPort, 
//						RemotingMechanism.TcpBinary
//						);
//				}

				// build sql server configuration string
				string sqlConnStr = string.Format(
					"user id={1};password={2};initial catalog={3};data source={0};Connect Timeout=5; Max Pool Size=5; Min Pool Size=5",
					Config.DbServer,
					Config.DbUsername,
					Config.DbPassword,
					Config.DbName
					);

				logger.Debug("Using SQLConnStr="+sqlConnStr);
				logger.Debug("Registering tcp channel on port: "+ownEP.Port);

                try
                {
                    _Chnl = new TcpChannel(ownEP.Port);
                	ChannelServices.RegisterChannel(_Chnl);
                }
                catch {}

				if (ManagerStartEvent!=null)
					ManagerStartEvent("Registered tcp channel on port: "+ownEP.Port,30);

				//since this is a single call thing, thread safety isnt an issue

				logger.Debug("Registering well known service type");

                RemotingConfiguration.RegisterWellKnownServiceType(
                    typeof(GManager), "Alchemi_Node",
                    WellKnownObjectMode.SingleCall);
                				
				// TODO: hierarchical grids ignored until after v1.0.0
                /*
                _Dedicated = dedicated;
                _Id = id;

                if (Manager != null)
                {
                    if (_Id == "")
                    {
                        Log("Registering new executor ...");
                        _Id = Manager.Executor_RegisterNewExecutor(null, new ExecutorInfo);
                        Log("New ExecutorID = " + _Id);
                    }

                    try
                    {
                        try
                        {
                            ConnectToManager();
                        }
                        catch (InvalidExecutorException)
                        {
                            Log("Invalid executor! Registering new executor ...");
                            _Id = Manager.Executor_RegisterNewExecutor(null, new ExecutorInfo);
                            Log("New ExecutorID = " + _Id);
                            ConnectToManager();
                        }
                    }
                    catch (ConnectBackException)
                    {
                        Log("Couldn't connect as dedicated executor. Reverting to non-dedicated executor.");
                        _Dedicated = false;
                        ConnectToManager();
                    }
                }
                */
      
                string datDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"dat");
                logger.Debug("datadir="+datDir);
                if (!Directory.Exists(datDir))
                {
                    Directory.CreateDirectory(datDir);
					logger.Debug("Data directory not found. Creating a new one:"+datDir);
                }

				if (ManagerStartEvent!=null)
					ManagerStartEvent("Initialising a new scheduler",50);

				logger.Debug("Initialising a new scheduler");
                IScheduler scheduler = new DefaultScheduler();
                scheduler.Executors = _Executors;
                scheduler.Applications = _Applications;

				logger.Debug("Configuring internal shared class...");
                InternalShared common = InternalShared.GetInstance(
                    new SqlServer(sqlConnStr),
                    datDir,
                    scheduler
                    );

				logger.Debug("Initialising scheduler - done");
				logger.Debug("Starting scheduler thread");

				if (ManagerStartEvent!=null)
					ManagerStartEvent("Starting scheduler thread",60);

                _stopScheduler = false;
				_DedicatedSchedulerThread = new Thread(new ThreadStart(ScheduleDedicated));
                _DedicatedSchedulerThread.Start();

				if (ManagerStartEvent!=null)
					ManagerStartEvent("Starting watchdog thread",70);

				logger.Info("Starting watchdog thread");
				
				_stopWatchDog = false;
                _WatchDogThread = new Thread(new ThreadStart(Watchdog));
                _WatchDogThread.Start();

				if (ManagerStartEvent!=null)
					ManagerStartEvent("Updating configuration",80);

				//start a seperate thread to init-known executors, since this may take a while.
				new Thread(new ThreadStart(InitExecutors)).Start();

				Config.Slz();

				if (ManagerStartEvent!=null)
					ManagerStartEvent("Started Manager",100);

				Started = true;

			}
            catch (Exception e)
            {
                Stop();
				logger.Error("Error Starting Manager Container",e);
                throw e;
            }
        }

		/// <summary>
		/// Reads the Manager configuration from the Alchemi.Manager.config.xml file, 
		/// <br /> or gets the default configuration if useDefault = true.
		/// </summary>
		/// <param name="useDefault"></param>
		public void ReadConfig(bool useDefault)
		{
			if (!useDefault)
			{
				try
				{
					Config = Configuration.GetConfiguration();
				}
				catch (Exception)
				{
					useDefault = true;
				}
			}

			if (useDefault)
			{
				Config = new Configuration();
			}
		}

		private void InitExecutors()
		{
			logger.Info("Initialising known executors...");
			lock (_Executors) 
			{
				_Executors.Init();
			}
			logger.Info("Initialising known executors...done");
		}

        //----------------------------------------------------------------------------------------------- 
        // public methods
        //----------------------------------------------------------------------------------------------- 

		/// <summary>
		/// Stop the scheduler and watchdog threads, and shut down the manager.
		/// </summary>
        public void Stop()
        {
			if (!Started)
				return;

            if (_DedicatedSchedulerThread != null)
            {
				logger.Info("Stopping the scheduler thread...");
                //_DedicatedSchedulerThread.Abort();
				_stopScheduler = true; //cleaner way of aborting
                _DedicatedSchedulerThread.Join();
            }
            if (_WatchDogThread != null)
            {
				logger.Info("Stopping the watchdog thread...");
				_stopWatchDog = true; //cleaner way of aborting.
                //_WatchDogThread.Abort();
                _WatchDogThread.Join();
            }

			logger.Info("Cleaning up all apps...");
			CleanUpApps();
			
			logger.Info("Unregistering the remoting channel...");
            ChannelServices.UnregisterChannel(_Chnl);

			Config.Slz();

			Started = false;
        }

		private void CleanUpApps()
		{
			try
			{
				logger.Debug("Cleaning up all apps before disconnect...");
				
				string datDir = string.Format("{0}\\dat", AppDomain.CurrentDomain.BaseDirectory);
				string[] dirs = Directory.GetDirectories(datDir);
				foreach(string s in dirs)
				{
					try
					{
						Directory.Delete(s,true);
						logger.Debug("Deleted directory: " + s);
					}
					catch{}
				}
				logger.Debug("Clean up all apps done.");
				
			}
			catch (Exception e)
			{
				logger.Debug("Clean up error : ",e);
			}
		}

        private void ScheduleDedicated()
        {
			try
			{
				// TODO: allow scheduling of multiple threads in one go
				while (!_stopScheduler)
				{
					//logger.Debug("WaitOne for 1000 millis on DedicatedSchedulerActive");
					InternalShared.Instance.DedicatedSchedulerActive.WaitOne(1000, false);

					//logger.Debug("Getting a dedicated schedule");
					DedicatedSchedule ds = InternalShared.Instance.Scheduler.ScheduleDedicated();

					if (ds == null)
					{
						InternalShared.Instance.DedicatedSchedulerActive.Reset();
						//logger.Debug("Dedicatd schedule is null. Reset the DedicatedSchedulerActive waithandle");
						continue;
					}

					MExecutor me = _Executors[ds.ExecutorId];
					MThread mt = new MThread(ds.TI);
                    
					try          
					{
						logger.Debug("Scheduling thread " + ds.TI.ThreadId + " to executor:"+ds.ExecutorId);
						// update thread state
						mt.CurrentExecutorId = ds.ExecutorId;
						mt.State = ThreadState.Scheduled;
						// dispatch thread
						me.RemoteRef.Manager_ExecuteThread(ds.TI);
					}
					catch (Exception e)
					{
						logger.Error("Some error occured trying to schedule. Disconnecting executor and reset-ing the thread to be scheduled.",e);
						// remove executor and reset thread so it can be rescheduled
						me.Disconnect();
						mt.Reset(); // this should happen as part of the disconnection
					}
				}
			}
			catch (ThreadAbortException)
			{
				logger.Debug("Scheduler Thread aborting...");
				Thread.ResetAbort();
			}
			catch (Exception e)
			{
				logger.Error("ScheduleDedicated thread error",e);
			}
        }

        private void Watchdog()
        {
			try
			{
				while (!_stopWatchDog)
				{
					Thread.Sleep(10000);

					// ping dedicated executors running threads and reset executor and thread if can't ping
					DataTable dt = InternalShared.Instance.Database.ExecSql_DataTable("Executors_SelectDedicatedRunningThreads");
					foreach (DataRow dr in dt.Rows)
					{
						MExecutor me = _Executors[dr["executor_id"].ToString()];
						try
						{
							me.RemoteRef.PingExecutor();
						}
						catch
						{
							me.Disconnect();
							new MThread(dr["application_id"].ToString(), (int) dr["thread_id"]).Reset();
						}
					}

					// disconnect nde if not recd alive notification in the last 10 seconds
					// TODO: make time interval configurable
					InternalShared.Instance.Database.ExecSql("Executors_DiscoverDisconnectedNDE 10");
					// reset threads whose executors have been disconnected
                    
					dt = InternalShared.Instance.Database.ExecSql_DataTable("Threads_SelectLostNDE");
					foreach (DataRow thread in dt.Rows)
					{
						new MThread(thread["application_id"].ToString(), (int) thread["thread_id"]).Reset();
						new MExecutor(thread["executor_id"].ToString()).Disconnect();
					}
				}
			}
			catch (ThreadAbortException)
			{
				logger.Debug("Watchdog thread aborting...");
				Thread.ResetAbort();
			}
			catch (Exception e)
			{
				logger.Error("WatchDog thread error ",e);
			}
        }
    }
}

