#region Alchemi copyright notice
/*
  Alchemi [.NET Grid Computing Framework]
  Copyright (c) 2002-2004 Akshay Luther
  http://www.alchemi.net
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
using System.Threading;
using System.IO;
using System.Collections;
using System.Security; 
using System.Security.Policy; 
using System.Security.Permissions; 
using System.Diagnostics;
using Microsoft.Win32;
using Alchemi.Core;

namespace Alchemi.Core.Executor
{
    public delegate void LogEventHandler(string s);
    public delegate void NonDedicatedExecutingStatusChangedEventHandler();
    public delegate void GotDisconnectedEventHandler();
 
    public class GExecutor : GNode, IExecutor
    {
        //----------------------------------------------------------------------------------------------- 
        // member variables
        //----------------------------------------------------------------------------------------------- 
        
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
        
        private int _HeartbeatInterval = 2; // TODO: make heartbeat time configurable

        public static event LogEventHandler Log;
        public static event NonDedicatedExecutingStatusChangedEventHandler NonDedicatedExecutingStatusChanged;
        public static event GotDisconnectedEventHandler GotDisconnected;

        //----------------------------------------------------------------------------------------------- 
        // properties
        //----------------------------------------------------------------------------------------------- 

        public string Id
        {
            get { return _Id; }
        }

        public bool Dedicated
        {
            get { return _Dedicated; }
        }

        public bool ExecutingNonDedicated
        {
            get { return _ExecutingNonDedicated; }
        }

        private ExecutorInfo Info
        {
            get 
            {
                ExecutorInfo info = new ExecutorInfo();
                
                RegistryKey hklm = Registry.LocalMachine;
                hklm = hklm.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0");
                info.MaxCpuPower = int.Parse(hklm.GetValue("~MHz").ToString());

                return info;
            }
        }

        //----------------------------------------------------------------------------------------------- 
        // constructors
        //----------------------------------------------------------------------------------------------- 
        
        public GExecutor(RemoteEndPoint managerEP, OwnEndPoint ownEP, string id, bool dedicated, SecurityCredentials sc) : base(managerEP, ownEP, sc)
        {
            string datDir = string.Format("{0}\\dat", Environment.CurrentDirectory);
            if (!Directory.Exists(datDir))
            {
                Directory.CreateDirectory(datDir);
            }

            _GridAppDomains = new Hashtable();
      
            _Dedicated = dedicated;
            _Id = id;

            if (_Id == "")
            {
                Log("Registering new executor ...");
                _Id = Manager.Executor_RegisterNewExecutor(Credentials, Info);
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
                    _Id = Manager.Executor_RegisterNewExecutor(Credentials, Info);
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

            if (_Dedicated)
            {
                _HeartbeatThread = new Thread(new ThreadStart(Heartbeat));
                _HeartbeatThread.Start();
            }
        }


        //----------------------------------------------------------------------------------------------- 
        // public methods
        //----------------------------------------------------------------------------------------------- 

        public void Disconnect()
        {
            StopNonDedicatedExecuting();
            
            if (_Dedicated)
            {
                _HeartbeatThread.Abort();
                _HeartbeatThread.Join();
            }

            try
            {
                Manager.Executor_DisconnectExecutor(Credentials, _Id);
            }
            catch (System.Net.Sockets.SocketException) {}
            catch (System.Runtime.Remoting.RemotingException) {}
            
            UnRemoteSelf();

            RelinquishIncompleteThreads();
            
            foreach (object gad in _GridAppDomains.Values)
            {
                AppDomain.Unload(((GridAppDomain) gad).Domain);
            }
            _GridAppDomains.Clear();
        }

        //-----------------------------------------------------------------------------------------------    

        public void StartNonDedicatedExecuting(int emptyThreadInterval)
        {
            if (!_Dedicated & !_ExecutingNonDedicated)
            {
                _EmptyThreadInterval = emptyThreadInterval;
                _NonDedicatedMonitorThread = new Thread(new ThreadStart(NonDedicatedMonitor));
                _NonDedicatedMonitorThread.Start();

                _ExecutingNonDedicated = true;
                NonDedicatedExecutingStatusChanged();

                _HeartbeatThread = new Thread(new ThreadStart(Heartbeat));
                _HeartbeatThread.Start();
            }
        }

        //-----------------------------------------------------------------------------------------------    

        public void StopNonDedicatedExecuting()
        {
            if (!_Dedicated & _ExecutingNonDedicated)
            {
                _NonDedicatedMonitorThread.Abort();
                _NonDedicatedMonitorThread.Join();

                _ExecutingNonDedicated = false;
                NonDedicatedExecutingStatusChanged();

                _HeartbeatThread.Abort();
                _HeartbeatThread.Join();
            }
        }

        //-----------------------------------------------------------------------------------------------    

        public void PingExecutor()
        {
            // for testing communication
        }
    
        //-----------------------------------------------------------------------------------------------    

        public void Manager_ExecuteThread(ThreadIdentifier ti)
        {
            _ReadyToExecute.WaitOne();
            _ReadyToExecute.Reset();
            _CurTi = ti;
            _ThreadExecutorThread = new Thread(new ThreadStart(ExecuteThreadInAppDomain));
            _ThreadExecutorThread.Priority = ThreadPriority.Lowest;
            _ThreadExecutorThread.Start();
        }

        //----------------------------------------------------------------------------------------------- 
        // private methods
        //----------------------------------------------------------------------------------------------- 

        private void ConnectToManager()
        {
            if (_Dedicated)
            {
                Manager.Executor_ConnectDedicatedExecutor(Credentials, _Id, OwnEP.ToRemoteEndPoint());
            }
            else
            {
                Manager.Executor_ConnectNonDedicatedExecutor(Credentials, _Id);
            }
        }

        //-----------------------------------------------------------------------------------------------    
    
        private void NonDedicatedMonitor()
        {
            bool gotDisconnected = false;
            try
            {
                _ReadyToExecute.WaitOne();
                while (!gotDisconnected)
                {
                    try
                    {
                        ThreadIdentifier ti = Manager.Executor_GetNextScheduledThreadIdentifier(Credentials, _Id);

                        if (ti == null)
                        {
                            Random rnd = new Random();
                            System.Threading.Thread.Sleep(rnd.Next(_EmptyThreadInterval, _EmptyThreadInterval * 2));
                        }
                        else
                        {
                            Manager_ExecuteThread(ti);
                        }
                    }
                    catch (System.Net.Sockets.SocketException)
                    {
                        gotDisconnected = true;
                    }
                    catch (System.Runtime.Remoting.RemotingException)
                    {
                        gotDisconnected = true;
                    }
                }

                // got disconnected
                _ExecutingNonDedicated = false;
                NonDedicatedExecutingStatusChanged();
                UnRemoteSelf();
                GotDisconnected();
            }
            catch (ThreadAbortException)
            {
                _ExecutingNonDedicated = false;
                Thread.ResetAbort();
            }
        }

        //-----------------------------------------------------------------------------------------------    

        private void ExecuteThreadInAppDomain()
        {
            try
            {
                Log(string.Format("executing grid thread # {0}.{1}", _CurTi.ApplicationId, _CurTi.ThreadId));

                string appDir = string.Format("{0}\\dat\\application_{1}", Environment.CurrentDirectory, _CurTi.ApplicationId);

                if (!_GridAppDomains.Contains(_CurTi.ApplicationId))
                {
                    // create application domain for newly encountered grid application
                    Directory.CreateDirectory(appDir);
      
                    FileDependencyCollection manifest = Manager.Executor_GetApplicationManifest(Credentials, _CurTi.ApplicationId);
                    if (manifest != null)
                    {
                        foreach (FileDependency dep in manifest)
                        {
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

                    AppDomainExecutor executor = (AppDomainExecutor) domain.CreateInstanceFromAndUnwrap("Alchemi.Core.dll", "Alchemi.Core.Executor.AppDomainExecutor");

                    _GridAppDomains.Add(
                        _CurTi.ApplicationId,
                        new GridAppDomain(domain, executor)
                        );
                }

                byte[] rawThread = Manager.Executor_GetThread(Credentials, _CurTi);
                GridAppDomain gad = (GridAppDomain) _GridAppDomains[_CurTi.ApplicationId];
                try
                {
                    byte[] finishedThread = gad.Executor.ExecuteThread(rawThread);
                    Manager.Executor_SetFinishedThread(Credentials, _CurTi, finishedThread, null);
                    Log(string.Format("finished executing grid thread # {0}.{1}", _CurTi.ApplicationId, _CurTi.ThreadId));
                }
                catch (Exception e)
                {
                    Manager.Executor_SetFinishedThread(Credentials, _CurTi, rawThread, e);
                    Log(string.Format("grid thread # {0}.{1} failed", _CurTi.ApplicationId, _CurTi.ThreadId));
                }
                
                _CurTi = null;
                _ReadyToExecute.Set();
            }
            catch (ThreadAbortException)
            {
                Log(string.Format("aborted grid thread # {0}.{1}", _CurTi.ApplicationId, _CurTi.ThreadId));
                _CurTi = null;
                _ReadyToExecute.Set();
                Thread.ResetAbort();
            }
        }

        //-----------------------------------------------------------------------------------------------    

        private void RelinquishIncompleteThreads()
        {
            if (_CurTi != null)
            {
                ThreadIdentifier ti = _CurTi;
                Manager_AbortThread(ti);
                Manager.Executor_RelinquishThread(Credentials, ti);
            }
        }
        
        //-----------------------------------------------------------------------------------------------    

        private void Heartbeat()
        {
            HeartbeatInfo info = new HeartbeatInfo();
            info.Interval = _HeartbeatInterval;
            
            // init for cpu usage
            TimeSpan oldTime = System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime;
            DateTime timeMeasured = DateTime.Now;
            TimeSpan newTime = new TimeSpan(0);

            // init for cpu avail
            PerformanceCounter cpuCounter = new PerformanceCounter(); 
            cpuCounter.CategoryName = "Processor"; 
            cpuCounter.CounterName = "% Processor Time"; 
            cpuCounter.InstanceName = "_Total"; 

            try
            {
                while (true)
                {
                    // calculate usage
                    newTime = System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime;
                    TimeSpan absUsage = newTime - oldTime;
                    float flUsage = ((float) absUsage.Ticks / (DateTime.Now - timeMeasured).Ticks) * 100;
                    info.PercentUsedCpuPower = (int) flUsage > 100 ? 100 : (int) flUsage;
                    info.PercentUsedCpuPower = (int) flUsage < 0 ? 0 : (int) flUsage;
                    timeMeasured = DateTime.Now;
                    oldTime = newTime;

                    // calculate avail
                    info.PercentAvailCpuPower = 100 - (int) cpuCounter.NextValue() + info.PercentUsedCpuPower;
                    info.PercentAvailCpuPower = (int) info.PercentAvailCpuPower > 100 ? 100 : info.PercentAvailCpuPower;
                    info.PercentAvailCpuPower = (int) info.PercentAvailCpuPower < 0 ? 0 : info.PercentAvailCpuPower;

                    try
                    {
                        Manager.Executor_Heartbeat(Credentials, _Id, info);
                    }
                    catch {}
                    Thread.Sleep(_HeartbeatInterval * 1000);
                }
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
        }

        //-----------------------------------------------------------------------------------------------

        public void Manager_AbortThread(ThreadIdentifier ti)
        {
            _ThreadExecutorThread.Abort();
            _ThreadExecutorThread.Join();
        }
    }
}

