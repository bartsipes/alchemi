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
using System.IO;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Runtime.Remoting;
using System.Threading;
using Alchemi.Core;
using Alchemi.Core.Utility;

namespace Alchemi.Core.Manager
{
    public delegate void LogEventHandler(string s);

    public class GManager : GNode, IManager
    {
        //----------------------------------------------------------------------------------------------- 
        // member variables
        //----------------------------------------------------------------------------------------------- 

        string _Id;
        bool _Dedicated;

        Thread _DedicatedSchedulerThread;
        Thread _WatchDogThread;
        ManualResetEvent _DedicatedSchedulerActive = new ManualResetEvent(true);

        MExecutorCollection _Executors;
        MApplicationCollection _Applications;

        IScheduler _Scheduler; 

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

        public static event LogEventHandler Log;
    
        //----------------------------------------------------------------------------------------------- 
        // constructors
        //----------------------------------------------------------------------------------------------- 

        public GManager(RemoteEndPoint managerEP, OwnEndPoint ownEP, string id, bool dedicated, string sqlConnStr) : base(managerEP, ownEP, null)
        {
            try
            {
                _Dedicated = dedicated;
                _Id = id;

                // TODO: hierarchical grids ignored until after v1.0.0
                /*
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
                InternalShared common = InternalShared.GetInstance(
                    new SqlServer(sqlConnStr),
                    string.Format("{0}\\dat", Environment.CurrentDirectory)
                    );

                _Applications = new MApplicationCollection();
                _Executors = new MExecutorCollection();
                _Executors.Init();
                _Scheduler = new DefaultScheduler();
                _Scheduler.Executors = _Executors;
                _Scheduler.Applications = _Applications;

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
            UnRemoteSelf();
        }

        //-----------------------------------------------------------------------------------------------          

        public void PingManager()
        {
            // for testing communication
        }

        //-----------------------------------------------------------------------------------------------          

        public void PingExecutor()
        {
            // for testing communication
        }
        
        //-----------------------------------------------------------------------------------------------          

        public void AuthenticateUser(SecurityCredentials sc)
        {
            
            string result = InternalShared.Instance.Database.ExecSql_Scalar("User_Authenticate '{0}', '{1}'", sc.Username, sc.Password).ToString();

            if (result == "0")
            {
                throw new AuthenticationException(string.Format("Authentication failed for user {0}.", sc.Username), null);
            }
        }
        
        //-----------------------------------------------------------------------------------------------          

        public string Owner_CreateApplication(SecurityCredentials sc)
        {
            AuthenticateUser(sc);
            EnsurePermission(sc, Permission.ManageOwnApp);

            return _Applications.CreateNew(sc.Username);
        }

        //-----------------------------------------------------------------------------------------------          

        public void Owner_SetApplicationManifest(SecurityCredentials sc, string appId, FileDependencyCollection manifest)
        {
            AuthenticateUser(sc);
            ApplicationAuthorizationCheck(sc, appId);

            _Applications[appId].Manifest = manifest;
        }

        //-----------------------------------------------------------------------------------------------          

        public void Owner_SetThread(SecurityCredentials sc, ThreadIdentifier ti, byte[] thread)
        {
            AuthenticateUser(sc);
            ApplicationAuthorizationCheck(sc, ti.ApplicationId);

            MThread t = _Applications[ti.ApplicationId][ti.ThreadId];
            t.Value = thread;
            t.Priority = ti.Priority;
            t.Init(true);
            _DedicatedSchedulerActive.Set();
        }

        //-----------------------------------------------------------------------------------------------       

        public void Owner_AbortThread(SecurityCredentials sc, ThreadIdentifier ti)
        {
            AuthenticateUser(sc);
            ApplicationAuthorizationCheck(sc, ti.ApplicationId);

            MThread thread = _Applications[ti.ApplicationId][ti.ThreadId];
            
            thread.State = ThreadState.Dead;
            // if running on an executor, ask it to abort the thread
            if (thread.State == ThreadState.Started | thread.State == ThreadState.Scheduled)
            {
                Log("aborting thread "+ ti.ApplicationId + "."+ ti.ThreadId);
                string executorId = thread.CurrentExecutorId;
                MExecutor me;
                if (executorId == null)
                {
                    // TODO: decide whether we should do anything here, like throwing an exception
                }
                else
                {
                    me = new MExecutor(executorId);
                    if (me.RemoteRef == null)
                    {
                        // TODO: decide if we should do anything here
                    }
                    else
                    {
                        try
                        {
                            me.RemoteRef.Manager_AbortThread(ti);
                            Log("aborted thread "+ ti.ApplicationId + "."+ ti.ThreadId);
                        }
                        catch (ExecutorCommException ece)
                        {
                            _Executors[ece.ExecutorId].Disconnect();
                        }
                    }
                }
            }
        }
        
        //-----------------------------------------------------------------------------------------------          

        public byte[][] Owner_GetFinishedThreads(SecurityCredentials sc, string appId)
        {
            AuthenticateUser(sc);
            ApplicationAuthorizationCheck(sc, appId);

            return _Applications[appId].FinishedThreads;
        }

        //-----------------------------------------------------------------------------------------------          

        public ThreadState Owner_GetThreadState(SecurityCredentials sc, ThreadIdentifier ti)
        {
            AuthenticateUser(sc);
            ApplicationAuthorizationCheck(sc, ti.ApplicationId);

            return _Applications[ti.ApplicationId][ti.ThreadId].State;
        }

        //-----------------------------------------------------------------------------------------------       
        
        public ApplicationState Owner_GetApplicationState(SecurityCredentials sc, string appId)
        {
            AuthenticateUser(sc);
            ApplicationAuthorizationCheck(sc, appId);

            return _Applications[appId].State;
        }

        //-----------------------------------------------------------------------------------------------       
        
        public void Owner_StopApplication(SecurityCredentials sc, string appId)
        {
            AuthenticateUser(sc);
            ApplicationAuthorizationCheck(sc, appId);

            MApplication a = _Applications[appId];
            DataTable dt = a.ThreadList.Tables[0];
            foreach (DataRow thread in dt.Rows)
            {
                Owner_AbortThread(sc, new ThreadIdentifier(appId, int.Parse(thread["thread_id"].ToString())));
            }
            //a.State = ApplicationState.Stopped;
        }
        
        //-----------------------------------------------------------------------------------------------          

        public Exception Owner_GetFailedThreadException(SecurityCredentials sc, ThreadIdentifier ti)
        {
            AuthenticateUser(sc);
            ApplicationAuthorizationCheck(sc, ti.ApplicationId);

            return _Applications[ti.ApplicationId][ti.ThreadId].FailedThreadException;
        }

        //-----------------------------------------------------------------------------------------------       

        public string Executor_RegisterNewExecutor(SecurityCredentials sc, ExecutorInfo info)
        {
            AuthenticateUser(sc);
            EnsurePermission(sc, Permission.ExecuteThread);

            return _Executors.RegisterNew(sc, info.MaxCpuPower);
        }

        //-----------------------------------------------------------------------------------------------          

        public void Executor_ConnectNonDedicatedExecutor(SecurityCredentials sc, string executorId)
        {
            AuthenticateUser(sc);
            EnsurePermission(sc, Permission.ExecuteThread);

            _Executors[executorId].ConnectNonDedicated();
            _Executors[executorId].HeartbeatUpdate(new HeartbeatInfo(0, 0, 0));
        }
    
        //-----------------------------------------------------------------------------------------------          
    
        public void Executor_ConnectDedicatedExecutor(SecurityCredentials sc, string executorId, RemoteEndPoint executorEP)
        {
            AuthenticateUser(sc);
            EnsurePermission(sc, Permission.ExecuteThread);

            try
            {
                _Executors[executorId].ConnectDedicated(executorEP);
                _DedicatedSchedulerActive.Set();
            }
            catch (ExecutorCommException ece)
            {
                throw new ConnectBackException("Couldn't connect back to the supplied Executor", ece);
            }
        }

        //-----------------------------------------------------------------------------------------------          

        public void Executor_DisconnectExecutor(SecurityCredentials sc, string executorId)
        {
            AuthenticateUser(sc);
            EnsurePermission(sc, Permission.ExecuteThread);

            _Executors[executorId].Disconnect();
        }

        //-----------------------------------------------------------------------------------------------          

        public ThreadIdentifier Executor_GetNextScheduledThreadIdentifier(SecurityCredentials sc, string executorId)
        {
            AuthenticateUser(sc);
            EnsurePermission(sc, Permission.ExecuteThread);

            bool scheduled = false;
            ThreadIdentifier ti;
            
            // try and get a local thread
            ti = _Scheduler.ScheduleNonDedicated(executorId);
            if (ti != null)
            {
                scheduled = true;
            }
            else if ((ti == null) & (Manager != null))
            {
                // TODO: hierarchical grids ignored until after v1.0.0
                /*
                // no local threads .. request thread from next manager and "simulate" the fact that it was scheduled locally
                ti = Manager.Executor_GetNextScheduledThreadIdentifier(null, _Id);
                
                if (ti != null)
                {
                    scheduled = true;
                    MThread t = new MThread(ti);
                    t.Init(false);
                    t.Priority = ti.Priority + 1;
                }
                */
            }

            if (scheduled)
            {
                MThread t = new MThread(ti);
                t.State = ThreadState.Scheduled;
                t.CurrentExecutorId = executorId;
            }

            return ti;
        }

        //-----------------------------------------------------------------------------------------------          

        public FileDependencyCollection Executor_GetApplicationManifest(SecurityCredentials sc, string appId)
        {
            AuthenticateUser(sc);
            EnsurePermission(sc, Permission.ExecuteThread);

            MApplication a = _Applications[appId];

            // TODO: hierarchical grids ignored until after v1.0.0
            //if (a.IsPrimary)
            //{
                return a.Manifest;
            //}
            //else
            //{
            //    
            //    //TODO: could copy manifest locally to save bandwidth
            //    return Manager.Executor_GetApplicationManifest(null, appId);
            //    
            //}
        }

        //-----------------------------------------------------------------------------------------------          
    
        public byte[] Executor_GetThread(SecurityCredentials sc, ThreadIdentifier ti)
        {
            AuthenticateUser(sc);
            EnsurePermission(sc, Permission.ExecuteThread);

            byte[] retval;
            MThread t = _Applications[ti.ApplicationId][ti.ThreadId];

            // TODO: hierarchical grids ignored until after v1.0.0
            //if (_Applications[ti.ApplicationId].IsPrimary)
            //{
                retval = t.Value;
            //}
            //else
            //{
            //    retval =  Manager.Executor_GetThread(null, ti);
            //}
            t.State = ThreadState.Started;
            return retval;
        }
    
        //-----------------------------------------------------------------------------------------------          

        public void Executor_SetFinishedThread(SecurityCredentials sc, ThreadIdentifier ti, byte[] thread, Exception e)
        {
            AuthenticateUser(sc);
            EnsurePermission(sc, Permission.ExecuteThread);

            MThread t = _Applications[ti.ApplicationId][ti.ThreadId];

            if (_Applications[ti.ApplicationId].IsPrimary)
            {
                t.Value = thread;
                if (e != null)
                {
                    t.FailedThreadException = e;
                }
            }
            else
            {
                // TODO: hierarchical grids ignored until after v1.0.0
                //Manager.Executor_SetFinishedThread(null, ti, thread, e);
            }

            t.State = ThreadState.Finished;
            _DedicatedSchedulerActive.Set();
        }

        //-----------------------------------------------------------------------------------------------          

        public void Executor_Heartbeat(SecurityCredentials sc, string executorId, HeartbeatInfo info)
        {
            AuthenticateUser(sc);
            EnsurePermission(sc, Permission.ExecuteThread);

            _Executors[executorId].HeartbeatUpdate(info);
        }

        //-----------------------------------------------------------------------------------------------       

        public void Executor_RelinquishThread(SecurityCredentials sc, ThreadIdentifier ti)
        {
            AuthenticateUser(sc);
            EnsurePermission(sc, Permission.ExecuteThread);

            new MThread(ti).Reset();
        }

        //-----------------------------------------------------------------------------------------------          

        public void Manager_ExecuteThread(ThreadIdentifier ti)
        {
            MThread t = _Applications[ti.ApplicationId][ti.ThreadId];
            t.Init(false);
            t.Priority = ti.Priority + 1;
            _DedicatedSchedulerActive.Set();
        }

        //-----------------------------------------------------------------------------------------------          
        
        public void Manager_AbortThread(ThreadIdentifier ti)
        {
            // TODO
        }

        //-----------------------------------------------------------------------------------------------      

        public DataSet Admon_GetLiveApplicationList(SecurityCredentials sc)
        {
            AuthenticateUser(sc);
            
            // TODO: return only owner's apps for 'ManageOwnApp' permission
            EnsurePermission(sc, Permission.ManageAllApps);

            return _Applications.LiveList;
        }

        //-----------------------------------------------------------------------------------------------       
        
        public DataSet Admon_GetThreadList(SecurityCredentials sc, string appId)
        {
            AuthenticateUser(sc);
            ApplicationAuthorizationCheck(sc, appId);

            return _Applications[appId].ThreadList;
        }

        //-----------------------------------------------------------------------------------------------          

        public DataTable Admon_GetUserList(SecurityCredentials sc)
        {
            AuthenticateUser(sc);
            EnsurePermission(sc, Permission.ManageUsers);

            return InternalShared.Instance.Database.ExecSql_DataTable("Admon_GetUserList");
        }
        
        //-----------------------------------------------------------------------------------------------          
        
        public DataTable Admon_GetGroups(SecurityCredentials sc)
        {
            AuthenticateUser(sc);
            EnsurePermission(sc, Permission.ManageUsers);

            return InternalShared.Instance.Database.ExecSql_DataTable("select * from grp");
        }

        //-----------------------------------------------------------------------------------------------          

        public void Admon_UpdateUsers(SecurityCredentials sc, DataTable updates)
        {
            AuthenticateUser(sc);
            EnsurePermission(sc, Permission.ManageUsers);

            foreach (DataRow user in updates.Rows)
            {
                InternalShared.Instance.Database.ExecSql("update usr set password = '{0}', grp_id = '{1}' where usr_name = '{2}'", user["password"].ToString(), (int) user["grp_id"], user["usr_name"].ToString());
            }
        }

        //-----------------------------------------------------------------------------------------------          

        public void Admon_AddUsers(SecurityCredentials sc, DataTable adds)
        {
            AuthenticateUser(sc);
            EnsurePermission(sc, Permission.ManageUsers);

            foreach (DataRow user in adds.Rows)
            {
                InternalShared.Instance.Database.ExecSql("insert usr values('{0}', '{1}', {2})", user["usr_name"].ToString(), user["password"].ToString(), (int) user["grp_id"]);
            }
        }

        //-----------------------------------------------------------------------------------------------          

        public DataTable Admon_GetExecutors(SecurityCredentials sc)
        {
            AuthenticateUser(sc);
            EnsurePermission(sc, Permission.ManageOwnApp);

            return InternalShared.Instance.Database.ExecSql_DataTable("Admon_GetExecutors");
        }

        //----------------------------------------------------------------------------------------------- 

        public DataTable Admon_GetSystemSummary(SecurityCredentials sc)
        {
            AuthenticateUser(sc);
            EnsurePermission(sc, Permission.ManageOwnApp);

            return InternalShared.Instance.Database.ExecSql_DataTable("Admon_SystemSummary");
        }

        //----------------------------------------------------------------------------------------------- 
        // private methods
        //----------------------------------------------------------------------------------------------- 

        private void ConnectToManager()
        {
            // TODO: hierarchical grids ignored until after v1.0.0
            /*
            if (_Dedicated)
            {
                Manager.Executor_ConnectDedicatedExecutor(null, _Id, OwnEP.ToRemoteEndPoint());
            }
            else
            {
                Manager.Executor_ConnectNonDedicatedExecutor(null, _Id);
            }
            */
        }
    
        //-----------------------------------------------------------------------------------------------    
    
        private void ScheduleDedicated()
        {
            try
            {
                // TODO: allow scheduling of multiple threads in one go
                while (true)
                {
                    _DedicatedSchedulerActive.WaitOne(1000, false);

                    DedicatedSchedule ds = _Scheduler.ScheduleDedicated();

                    if (ds == null)
                    {
                        _DedicatedSchedulerActive.Reset();
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

        //-----------------------------------------------------------------------------------------------          

        private bool IsApplicationCreator(SecurityCredentials sc, string appId)
        {
            string creator = InternalShared.Instance.Database.ExecSql_Scalar("User_VerifyApplicationCreator '{0}', '{1}'", sc.Username, appId).ToString();
            return creator == "0" ? false : true;
        }
        
        //-----------------------------------------------------------------------------------------------          
        
        private void ApplicationAuthorizationCheck(SecurityCredentials sc, string appId)
        {
            if (IsApplicationCreator(sc, appId))
            {
                // assume the user has permission 'ManageOwnApp' because this would have been verified during creation
            }
            else
            {
                // ensure permission 'ManageAllApps'
                EnsurePermission(sc, Permission.ManageAllApps);
            }
        }

        //-----------------------------------------------------------------------------------------------          

        private void EnsurePermission(SecurityCredentials sc, Permission perm)
        {
            string result = InternalShared.Instance.Database.ExecSql_Scalar("User_VerifyPermission '{0}', '{1}'", sc.Username, Convert.ToInt32(perm)).ToString();
            if (result == "0")
            {
                throw new AuthorizationException(
                    string.Format("User is not associated with permission '{0}'", perm.ToString()),
                    null
                    );
            }
        }
        
        //-----------------------------------------------------------------------------------------------          

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

