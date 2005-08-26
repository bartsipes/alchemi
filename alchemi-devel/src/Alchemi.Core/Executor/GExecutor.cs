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
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Security;
using System.Security.Policy;
using System.Threading;
using Alchemi.Core.Owner;
using Microsoft.Win32;

namespace Alchemi.Core.Executor
{
    public delegate void NonDedicatedExecutingStatusChangedEventHandler();
    public delegate void GotDisconnectedEventHandler();
 
	/// <summary>
	/// The GExecutor class is an implementation of the IExecutor interface and represents an Executor node.
	/// </summary>
    public class GExecutor : GNode, IExecutor, IOwner
    {
        //----------------------------------------------------------------------------------------------- 
        // member variables
        //----------------------------------------------------------------------------------------------- 
        
		// Create a logger for use in this class
		private static readonly Logger logger = new	Logger();

        private string _Id;
        private bool _Dedicated;
        private Thread _NonDedicatedMonitorThread;
        private Thread _HeartbeatThread;
        private Thread _ThreadExecutorThread;
        private int _EmptyThreadInterval;
        private bool _ExecutingNonDedicated = false;
        private ThreadIdentifier _CurTi;
        private Hashtable _GridAppDomains;
        private ManualResetEvent _ReadyToExecute = new ManualResetEvent(true);
        private string _BaseDir;
        
        private int _HeartbeatInterval = 2;

		private bool _stopHeartBeat = false;
		private bool _stopNonDedicatedMonitor = false;

		private int ca = 0;

		/// <summary>
		/// Raised when the connection status of a non-dedicated Executor is changed.
		/// </summary>
        public static event NonDedicatedExecutingStatusChangedEventHandler NonDedicatedExecutingStatusChanged;
		
		/// <summary>
		/// This event is raised only when a non-dedicated Executor gets disconnected from the Manager.
		/// </summary>
        public static event GotDisconnectedEventHandler GotDisconnected;

        //----------------------------------------------------------------------------------------------- 
        // properties
        //----------------------------------------------------------------------------------------------- 

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
            get { return _ExecutingNonDedicated; }
        }

        private ExecutorInfo Info
        {
            get 
            {
                ExecutorInfo info = new ExecutorInfo();
				try
				{
					RegistryKey hklm = Registry.LocalMachine;
					hklm = hklm.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0");
					info.MaxCpuPower = int.Parse(hklm.GetValue("~MHz").ToString());
				}catch (Exception e)
				{
					logger.Debug("Error getting executorInfo...",e);
				}

                return info;
            }
        }

		/// <summary>
		/// Gets the base directory for the executor
		/// </summary>
        public string BaseDir
        {
            get
            {
                return _BaseDir;
            }
        }

		/// <summary>
		/// Gets or sets the heartbeat interval for the executor (in seconds).
		/// </summary>
		public int HeartBeatInterval
		{
			get 
			{
				return _HeartbeatInterval;
			}
			set
			{
				_HeartbeatInterval = value;
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
        public GExecutor(RemoteEndPoint managerEP, OwnEndPoint ownEP, string id, bool dedicated, SecurityCredentials sc, string baseDir) : base(managerEP, ownEP, sc)
        {
            _BaseDir = baseDir;
            if (_BaseDir == "")
            {
                _BaseDir = AppDomain.CurrentDomain.BaseDirectory;
            }

            string datDir = Path.Combine(_BaseDir,"dat");
            
			logger.Debug("datadir="+datDir);
            if (!Directory.Exists(datDir))
            {
				logger.Debug("Couldnot find datadir. Creating it..."+datDir);
                Directory.CreateDirectory(datDir);
            }

            _GridAppDomains = new Hashtable();
      
            _Dedicated = dedicated;
            _Id = id;

            if (_Id == "")
            {
				logger.Info("Registering new executor");
                _Id = Manager.Executor_RegisterNewExecutor(Credentials, Info);
				logger.Info("Successfully Registered new executor:"+_Id);
            }

            try
            {
                try
                {
                    ConnectToManager();
                }
                catch (InvalidExecutorException)
                {
					logger.Info("Invalid executor! Registering new executor again...");

                    _Id = Manager.Executor_RegisterNewExecutor(Credentials, Info);
                    
					logger.Info("New ExecutorID = " + _Id);
                    ConnectToManager();
                }
            }
            catch (ConnectBackException) 
            {
                logger.Warn("Couldn't connect as dedicated executor. Reverting to non-dedicated executor.");
                _Dedicated = false;
                ConnectToManager();
            }

            if (_Dedicated)
            {
				logger.Debug("Dedicated mode: starting heart-beat thread");
				StartHeartBeatThread();
            }
        }

		private void StartHeartBeatThread()
		{
			_stopHeartBeat = false;
            _HeartbeatThread = new Thread(new ThreadStart(Heartbeat));
            _HeartbeatThread.Start();
		}

		private void CleanUpApps()
		{
			try
			{
				logger.Debug("Cleaning up all apps before disconnect...");
				string datDir = Path.Combine(_BaseDir,"dat");
				string[] dirs = Directory.GetDirectories(datDir);
				foreach(string s in dirs)
				{
					try
					{
						Directory.Delete(s,true);
						logger.Debug("Deleted directory: " + s);
					}catch{}
				}
				logger.Debug("Clean up all apps done.");
			}
			catch (Exception e)
			{
				logger.Debug("Clean up error : " + e.Message);
			}
		}

        //----------------------------------------------------------------------------------------------- 
        // public methods
        //----------------------------------------------------------------------------------------------- 

		/// <summary>
		/// Abort all running threads and Disconnect from the Manager. 
		/// </summary>
        public void Disconnect()
        {
            StopNonDedicatedExecuting();
            
            if (_Dedicated)
            {
				logger.Debug("Stopping heartbeat thread...");
                _stopHeartBeat = true;
				//_HeartbeatThread.Abort();
                _HeartbeatThread.Join();
				logger.Debug("HeartBeat stopped.");
            }

			try
			{
				Manager.Executor_DisconnectExecutor(Credentials, _Id);
			}
			catch (SocketException) {}
			catch (System.Runtime.Remoting.RemotingException) {}
			catch (Exception){}


            UnRemoteSelf();

            RelinquishIncompleteThreads();
            
			//clean up all apps
			CleanUpApps();

			logger.Debug("Unloading AppDomains on this executor...");
            foreach (object gad in _GridAppDomains.Values)
            {
				try
				{
					AppDomain.Unload(((GridAppDomain) gad).Domain);
				}
				catch (Exception e)
				{
					logger.Error("Error unloading appDomain:",e);
				}
            }
            _GridAppDomains.Clear();

			//raise disconnected event
			logger.Debug("Raising event: Executor GotDisconnected.");
			try
			{	//raise the event for non-dedicated mode only.
				if (!Dedicated)
					GotDisconnected();
			}
			catch {} //in case of no subscribers, there is a nullpointerexception
        }

        //-----------------------------------------------------------------------------------------------    

		/// <summary>
		/// Start execution in non-dedicated mode.
		/// </summary>
		/// <param name="emptyThreadInterval">Interval to wait in between attempts to get a thread from the manager</param>
        public void StartNonDedicatedExecuting(int emptyThreadInterval)
        {
            if (!_Dedicated & !_ExecutingNonDedicated)
            {
				logger.Debug("Starting Non-dedicatedMonitor thread");
                _EmptyThreadInterval = emptyThreadInterval;
				_stopNonDedicatedMonitor = false;
                _NonDedicatedMonitorThread = new Thread(new ThreadStart(NonDedicatedMonitor));
                _NonDedicatedMonitorThread.Start();

                _ExecutingNonDedicated = true;
				//raise an event to indicate the status change into non-dedicated execution mode.
				try
				{
					NonDedicatedExecutingStatusChanged();
				}
				catch{}

				//start the heartbeat thread.
				logger.Debug("Starting heart-beat thread: non-dedicated mode");
				StartHeartBeatThread();
			}
        }

        //-----------------------------------------------------------------------------------------------    

		/// <summary>
		/// Stops execution in non-dedicated mode.
		/// </summary>
        public void StopNonDedicatedExecuting()
        {
            if (!_Dedicated & _ExecutingNonDedicated)
            {
				logger.Debug("Stopping Non-dedicated execution monitor thread...");
                _stopNonDedicatedMonitor = true; //clean way to abort thread
                _NonDedicatedMonitorThread.Join(5000);

				try
				{
					if (_NonDedicatedMonitorThread.IsAlive)
					{
						logger.Debug("Nondedicated monitor still alive. forcing abort...");
						//even after 5 seconds if it doesnt stop, forcibly abort.
						_NonDedicatedMonitorThread.Abort();
						_NonDedicatedMonitorThread.Join();
					}
					else
					{
						logger.Debug("Nondedicated monitor thread stopped.");
					}
				}
				catch (Exception e)
				{
					logger.Error("Error trying to abort NonDedicatedMonitor thread.",e);
				}

                _ExecutingNonDedicated = false;
				logger.Debug("Raising event: NonDedicatedExecutingStatusChanged");
				try
				{
					NonDedicatedExecutingStatusChanged();
				}
				catch {}
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
            _ReadyToExecute.WaitOne();
            _ReadyToExecute.Reset();
            _CurTi = ti;
            _ThreadExecutorThread = new Thread(new ThreadStart(ExecuteThreadInAppDomain));
            _ThreadExecutorThread.Priority = ThreadPriority.Lowest;
            _ThreadExecutorThread.Start();
			logger.Debug("Started thread for executing GridThread:"+ti.ThreadId);
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
				ca++;
				logger.Debug("CleanupApp called " + ca + " times");
				try
				{
					logger.Debug("Unloading AppDomain for application:" + appid);
	
					GridAppDomain gad = (GridAppDomain)_GridAppDomains[appid];
					if (gad!=null)
					{
						AppDomain.Unload(gad.Domain);
						_GridAppDomains.Remove(appid);
					}
					else
					{
						logger.Debug("Appdomain not found in collection.");
					}
				}
				catch (Exception e)
				{
					logger.Debug("Error unloading appdomain:",e);
				}

				string appDir = string.Format("{0}\\dat\\application_{1}", _BaseDir, appid);
				logger.Debug("Cleaning up files on executor:"+_Id+" for application:" + appid);
				
				logger.Debug("Deleting: " + appDir);
				Directory.Delete(appDir,true);
				logger.Debug("Clean up finished for app:"+appid);
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

        private void ConnectToManager()
        {
            if (_Dedicated)
            {
				logger.Debug("Connecting to Manager dedicated...");
                Manager.Executor_ConnectDedicatedExecutor(Credentials, _Id, OwnEP.ToRemoteEndPoint());
            }
            else
            {
				logger.Debug("Connecting to Manager NON-dedicated...");
                Manager.Executor_ConnectNonDedicatedExecutor(Credentials, _Id);
            }
        }

        //-----------------------------------------------------------------------------------------------    
    
        private void NonDedicatedMonitor()
        {
            bool gotDisconnected = false;
			try
			{
				while (!gotDisconnected && !_stopNonDedicatedMonitor)
				{
					try
					{
						_ReadyToExecute.WaitOne();
						//get the next thread-id from the manager - only if we can execute something here.
						//in non-dedicated mode...the executor pulls threads instead of the manager giving it threads to execute.
						ThreadIdentifier ti = Manager.Executor_GetNextScheduledThreadIdentifier(Credentials, _Id);

						if (ti == null)
						{
							//if there is no thread to execute, sleep for a random time and ask again later
							Random rnd = new Random();
							Thread.Sleep(rnd.Next(_EmptyThreadInterval, _EmptyThreadInterval * 2));
						}
						else
						{
							logger.Debug("Non-dedicated mode: Calling own method to execute thread");
							Manager_ExecuteThread(ti);
						}
					}
					catch (SocketException se)
					{
						gotDisconnected = true;
						logger.Error("Got disconnected. Non-dedicated mode.",se);
					}
					catch (System.Runtime.Remoting.RemotingException re)
					{
						gotDisconnected = true;
						logger.Error("Got disconnected. Non-dedicated mode.",re);
					}
				}

				// got disconnected
				_ExecutingNonDedicated = false;
				//raise status changed event
				logger.Debug("Raising event: NonDedicatedExecutingStatusChanged");
				try
				{
					NonDedicatedExecutingStatusChanged();
				}
				catch {}

				logger.Debug("Non-dedicated executor: Unremoting self");
				UnRemoteSelf();
				//				//raise disconnected event ..let us do this for both dedicated, and non-dedicated
				//                logger.Debug("Raising event: GotDisconnected");
				//				GotDisconnected();
			}
			catch (ThreadAbortException)
			{
				_ExecutingNonDedicated = false;
				logger.Warn("ThreadAbortException. Non-dedicated executor");
				Thread.ResetAbort();
			}
			catch (Exception e)
			{
				logger.Error("Error in non-dedicated monitor: "+e.Message,e);
			}
        }

        //-----------------------------------------------------------------------------------------------    

        private void ExecuteThreadInAppDomain()
        {
			logger.Info(string.Format("executing grid thread # {0}.{1}", _CurTi.ApplicationId, _CurTi.ThreadId));

            string appDir = Path.Combine(_BaseDir,string.Format("dat\\application_{0}",_CurTi.ApplicationId));
			logger.Debug("AppDir on executor=" + appDir);
            if (!_GridAppDomains.Contains(_CurTi.ApplicationId))
            {
                // create application domain for newly encountered grid application
                Directory.CreateDirectory(appDir);
    
                FileDependencyCollection manifest = Manager.Executor_GetApplicationManifest(Credentials, _CurTi.ApplicationId);
                if (manifest != null)
                {
                    foreach (FileDependency dep in manifest)
                    {
						logger.Debug("Unpacking file: " + dep.FileName + " to " + appDir);
                        dep.UnPack(appDir + "\\" + dep.FileName);
                    }
                }

                AppDomainSetup info = new AppDomainSetup();
                info.PrivateBinPath = appDir;
                AppDomain domain = AppDomain.CreateDomain(_CurTi.ApplicationId, null, info);

                // ***
                // http://www.dotnetthis.com/Articles/DynamicSandboxing.htm
            	PolicyLevel domainPolicy = PolicyLevel.CreateAppDomainLevel(); 
                AllMembershipCondition allCodeMC = new AllMembershipCondition(); 
                // TODO: 'FullTrust' in the following line needs to be replaced with something like 'AlchemiGridThread'
                //        This permission set needs to be defined and set automatically as part of the installation.
            	PermissionSet internetPermissionSet = domainPolicy.GetNamedPermissionSet("FullTrust"); 
                PolicyStatement internetPolicyStatement = new PolicyStatement(internetPermissionSet); 
                CodeGroup allCodeInternetCG = new UnionCodeGroup(allCodeMC, internetPolicyStatement); 
                domainPolicy.RootCodeGroup = allCodeInternetCG; 
                domain.SetAppDomainPolicy(domainPolicy);
                // ***

				/* Log 12/01, 2004
				 * Modifyied by Rodrigo Assirati Dias (rdias@ime.usp.br)
				 * Modified ExecuteThreadInAppDomain function to enable it to create a instance of Alchemi.Core.dll
				 * in the application base directory rather than the running application directory (%WINDOWSDIR%/System32 to services)
				 */
				//Code edited by Rodrigo Assirati Dias
				//Original code was:
				//AppDomainExecutor executor = (AppDomainExecutor) domain.CreateInstanceFromAndUnwrap("Alchemi.Core.dll", "Alchemi.Core.Executor.AppDomainExecutor");
				AppDomainExecutor executor = (AppDomainExecutor) domain.CreateInstanceFromAndUnwrap(AppDomain.CurrentDomain.BaseDirectory + "Alchemi.Core.dll", "Alchemi.Core.Executor.AppDomainExecutor");

                _GridAppDomains.Add(
                    _CurTi.ApplicationId,
                    new GridAppDomain(domain, executor)
                    );

				logger.Debug("-------------- Created app domain, policy, got instance of GridAppDomain and added to hashtable...all done once for this application");
            }

			//get thread from manager
            byte[] rawThread = Manager.Executor_GetThread(Credentials, _CurTi);
            GridAppDomain gad = (GridAppDomain) _GridAppDomains[_CurTi.ApplicationId];
            try
            {
				//execute it
                byte[] finishedThread = gad.Executor.ExecuteThread(rawThread);
				//set its status to finished
                Manager.Executor_SetFinishedThread(Credentials, _CurTi, finishedThread, null);
				logger.Info(string.Format("finished executing grid thread # {0}.{1}", _CurTi.ApplicationId, _CurTi.ThreadId));

            }
            catch (ThreadAbortException)
            {
				logger.Warn(string.Format("aborted grid thread # {0}.{1}", _CurTi.ApplicationId, _CurTi.ThreadId));
                Thread.ResetAbort();
            }
            catch (Exception e)
            {
                Manager.Executor_SetFinishedThread(Credentials, _CurTi, rawThread, e);
				logger.Warn(string.Format("grid thread # {0}.{1} failed ({2})", _CurTi.ApplicationId, _CurTi.ThreadId, e.GetType()));
            }

            _CurTi = null;
            _ReadyToExecute.Set();
        }

        //-----------------------------------------------------------------------------------------------    

        private void RelinquishIncompleteThreads()
        {
			if (_CurTi != null)
			{
				ThreadIdentifier ti = _CurTi;
				logger.Debug("Relinquishing incomplete thread:"+ti.ThreadId);
				Manager_AbortThread(ti);
				Manager.Executor_RelinquishThread(Credentials, ti);
			}
			else
			{
				logger.Debug("No threads to relinquish. _CurTi = null");
			}
        }
        
        //-----------------------------------------------------------------------------------------------    

		//heart beat thread
        private void Heartbeat()
        {
			int pingFailCount=0;

			try
			{
				HeartbeatInfo info = new HeartbeatInfo();
				info.Interval = _HeartbeatInterval;
	            
				// init for cpu usage
				TimeSpan oldTime = Process.GetCurrentProcess().TotalProcessorTime;
				DateTime timeMeasured = DateTime.Now;
				TimeSpan newTime = new TimeSpan(0);

				//TODO: be language neutral here. how??
				// init for cpu avail
				PerformanceCounter cpuCounter = new PerformanceCounter(); 
				cpuCounter.ReadOnly = true;
				cpuCounter.CategoryName = "Processor"; 
				cpuCounter.CounterName = "% Processor Time"; 
				cpuCounter.InstanceName = "_Total"; 

				while (!_stopHeartBeat)
				{
					// calculate usage
					newTime = Process.GetCurrentProcess().TotalProcessorTime;
					TimeSpan absUsage = newTime - oldTime;
					float flUsage = ((float) absUsage.Ticks / (DateTime.Now - timeMeasured).Ticks) * 100;
					info.PercentUsedCpuPower = (int) flUsage > 100 ? 100 : (int) flUsage;
					info.PercentUsedCpuPower = (int) flUsage < 0 ? 0 : (int) flUsage;
					timeMeasured = DateTime.Now;
					oldTime = newTime;

					try
					{
						// calculate avail
						info.PercentAvailCpuPower = 100 - (int) cpuCounter.NextValue() + info.PercentUsedCpuPower;
						info.PercentAvailCpuPower = info.PercentAvailCpuPower > 100 ? 100 : info.PercentAvailCpuPower;
						info.PercentAvailCpuPower = info.PercentAvailCpuPower < 0 ? 0 : info.PercentAvailCpuPower;
					}
					catch (Exception e)
					{
						logger.Debug("HeartBeat thread error: " + e.Message + Environment.NewLine + " Heartbeat continuing after error...");
					}

					//have a seperate try...block since we try 3 times before giving up
					try
					{
						//send a heartbeat to the manager.
						Manager.Executor_Heartbeat(Credentials, _Id, info);
						pingFailCount = 0;
					}
					catch (Exception e)
					{
						if (e is SocketException || e is System.Runtime.Remoting.RemotingException)
						{
							try
							{
								logger.Debug("Error during heartbeat: " +e.Message + " ...trying to PingManager...failCount="+pingFailCount);
								Manager.PingManager();
							}
							catch
							{
								pingFailCount++;
								//we disconnect the executor if the manager cant be pinged three times
								if (pingFailCount == 3)
								{
									logger.Error("Disconnecting from Manager...Cannot contact manager.",e);
									try
									{
										//the disconnect here should be started off on a seperate thread because:
										//disconnect itself waits for HeartBeatThread to stop. If the method call
										//to disconnect from HeartBeat wont return immediately, then there is a deadlock
										//with disconnect waiting for the HeartBeatThread to stop and the HeartBeatThread waiting
										//for the call to disconnect to return.
										new Thread(new ThreadStart(Disconnect)).Start();
									}
									catch{}
								}
							}
						}
					}

					Thread.Sleep(_HeartbeatInterval * 1000);
				}
			}
			catch (ThreadAbortException)
			{
				Thread.ResetAbort();
				logger.Debug("HeartBeat thread aborted.");
			}
			catch (Exception e) 
			{
				logger.Error("HeartBeat Exception : " + e.Message,e);
			}
        }

        //-----------------------------------------------------------------------------------------------

		/// <summary>
		/// Abort the given thread.
		/// </summary>
		/// <param name="ti">ThreadIdentifier object representing the GridThread to be aborted</param>
        public void Manager_AbortThread(ThreadIdentifier ti)
        {
			try
			{
				_ThreadExecutorThread.Abort();
				_ThreadExecutorThread.Join();
				logger.Debug("Aborted Executor Thread: "+ti.ThreadId);
			}
			catch (Exception e)
			{
				logger.Warn("Error aborting thread: " + ti.ThreadId + " Error= " + e.Message);
			}
		}
	}
}

