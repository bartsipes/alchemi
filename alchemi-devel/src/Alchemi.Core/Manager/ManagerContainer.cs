#region Alchemi copyright notice
/*
  Alchemi [.NET Grid Computing Framework]
  http://www.alchemi.net
  
  Copyright (c) 2002-2004 Akshay Luther & 2003-2004 Rajkumar Buyya 
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
using System.IO;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Runtime.Remoting;
using System.Threading;
using Alchemi.Core;
using Alchemi.Core.Utility;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;


namespace Alchemi.Core.Manager
{
    public delegate void LogEventHandler(string s);

    public class ManagerContainer
    {
        //----------------------------------------------------------------------------------------------- 
        // member variables
        //----------------------------------------------------------------------------------------------- 

        //bool _Initted;
        TcpChannel _Chnl;

        Thread _DedicatedSchedulerThread;
        Thread _WatchDogThread;

        MExecutorCollection _Executors = new MExecutorCollection();
        MApplicationCollection _Applications = new MApplicationCollection();

        //----------------------------------------------------------------------------------------------- 
        // properties
        //----------------------------------------------------------------------------------------------- 

        public static event LogEventHandler Log;
    
        //----------------------------------------------------------------------------------------------- 
        // constructors
        //----------------------------------------------------------------------------------------------- 

        public ManagerContainer(RemoteEndPoint managerEP, OwnEndPoint ownEP, string id, bool dedicated, string sqlConnStr)
        {
            try
            {
                RemotingConfiguration.Configure("Alchemi.Manager.exe.config");

                try
                {
                    _Chnl = new TcpChannel(ownEP.Port);
                    ChannelServices.RegisterChannel(_Chnl);
                }
                catch {}

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
      
                string datDir = string.Format("{0}\\dat", Environment.CurrentDirectory);
                
                if (!Directory.Exists(datDir))
                {
                    Directory.CreateDirectory(datDir);
                }

                IScheduler scheduler = new DefaultScheduler();
                scheduler.Executors = _Executors;
                scheduler.Applications = _Applications;

                InternalShared common = InternalShared.GetInstance(
                    new SqlServer(sqlConnStr),
                    string.Format("{0}\\dat", Environment.CurrentDirectory),
                    scheduler
                    );

                _Executors.Init();
                
                _DedicatedSchedulerThread = new Thread(new ThreadStart(ScheduleDedicated));
                _DedicatedSchedulerThread.Start();
                _WatchDogThread = new Thread(new ThreadStart(Watchdog));
                _WatchDogThread.Start();
            }
            catch (Exception e)
            {
                Stop();
                throw e;
            }
        }

        //----------------------------------------------------------------------------------------------- 
        // public methods
        //----------------------------------------------------------------------------------------------- 

        public void Stop()
        {
            if (_DedicatedSchedulerThread != null)
            {
                _DedicatedSchedulerThread.Abort();
                _DedicatedSchedulerThread.Join();
            }
            if (_WatchDogThread != null)
            {
                _WatchDogThread.Abort();
                _WatchDogThread.Join();
            }

            ChannelServices.UnregisterChannel(_Chnl);
        }

        private void ScheduleDedicated()
        {
            try
            {
                // TODO: allow scheduling of multiple threads in one go
                while (true)
                {
                    InternalShared.Instance.DedicatedSchedulerActive.WaitOne(1000, false);

                    DedicatedSchedule ds = InternalShared.Instance.Scheduler.ScheduleDedicated();

                    if (ds == null)
                    {
                        InternalShared.Instance.DedicatedSchedulerActive.Reset();
                        continue;
                    }

                    MExecutor me = _Executors[ds.ExecutorId];
                    MThread mt = new MThread(ds.TI);
                    
                    try          
                    {
                        // update thread state
                        mt.CurrentExecutorId = ds.ExecutorId;
                        mt.State = ThreadState.Scheduled;
                        // dispatch thread
                        me.RemoteRef.Manager_ExecuteThread(ds.TI);
                    }
                    catch
                    {
                        // remove executor and reset thread so it can be rescheduled
                        me.Disconnect();
                        mt.Reset(); // this should happen as part of the the disconnection
                    }
                }
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
        }

        private void Watchdog()
        {
            try
            {
                while (true)
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
                    // TODO: make time interval configurable ...................................V
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
                Thread.ResetAbort();
            }
        }
    }
}

