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
using System.Runtime.CompilerServices;
using Alchemi.Core;
using Alchemi.Core.Utility;


namespace Alchemi.Core.Manager
{
    public class GManager : MarshalByRefObject, IManager
    {

        MApplicationCollection _Applications = new MApplicationCollection();
        MExecutorCollection _Executors = new MExecutorCollection();
        
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
            t.Init(true);
            t.Priority = ti.Priority;
            
            InternalShared.Instance.DedicatedSchedulerActive.Set();
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
                AbortThread(ti);
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
            a.Stop();
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
                InternalShared.Instance.DedicatedSchedulerActive.Set();
            }
            catch (ExecutorCommException ece)
            {
                throw new ConnectBackException("Couldn't connect back to the supplied Executor", null);
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
            
            // critical section .. don't want to schedule same thread on multiple executors
            Monitor.Enter(InternalShared.Instance);

            // try and get a local thread
            ti = InternalShared.Instance.Scheduler.ScheduleNonDedicated(executorId);
            if (ti != null)
            {
                scheduled = true;
            }
            else
            {
                // no thread, so can release lock immediately
                Monitor.Exit(InternalShared.Instance);
            }
            // TODO: hierarchical grids ignored until after v1.0.0
            /*
            else if ((ti == null) & (Manager != null))
            {
                // no local threads .. request thread from next manager and "simulate" the fact that it was scheduled locally
                ti = Manager.Executor_GetNextScheduledThreadIdentifier(null, _Id);
                
                if (ti != null)
                {
                    scheduled = true;
                    MThread t = new MThread(ti);
                    t.Init(false);
                    t.Priority = ti.Priority + 1;
                }
            }
            */

            if (scheduled)
            {
                MThread t = new MThread(ti);
                t.State = ThreadState.Scheduled;
                t.CurrentExecutorId = executorId;
                // finished scheduling thread, can release lock
                Monitor.Exit(InternalShared.Instance);
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
            InternalShared.Instance.DedicatedSchedulerActive.Set();
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
            InternalShared.Instance.DedicatedSchedulerActive.Set();
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

        internal static void AbortThread(ThreadIdentifier ti, string executorId)
        {
            if (executorId == null)
            {
                // not being executed on any executor
                return;
            }
            MExecutor me = new MExecutor(executorId);
            if (me.RemoteRef == null)
            {
                // not being executed on a dedicated executor .. so can't abort
                return;
            }
            try
            {
                me.RemoteRef.Manager_AbortThread(ti);
            }
            catch (ExecutorCommException)
            {
                me.Disconnect();
            }
        }

        //-----------------------------------------------------------------------------------------------          

        private void AbortThread(ThreadIdentifier ti)
        {
            MThread t = new MThread(ti);
            AbortThread(ti, t.CurrentExecutorId);
        }
    }
}

