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
using System.Data;

namespace Alchemi.Core
{
    public interface IManager : IExecutor
    {
        void PingManager();
        void AuthenticateUser(SecurityCredentials sc);
        
        //
        // owner services
        //
        string Owner_CreateApplication(SecurityCredentials sc);
        void Owner_SetApplicationManifest(SecurityCredentials sc, string appId, FileDependencyCollection manifest);
        void Owner_SetThread(SecurityCredentials sc, ThreadIdentifier ti, byte[] thread);
        byte[][] Owner_GetFinishedThreads(SecurityCredentials sc, string appId);
        ThreadState Owner_GetThreadState(SecurityCredentials sc, ThreadIdentifier ti);
        Exception Owner_GetFailedThreadException(SecurityCredentials sc, ThreadIdentifier ti);
        ApplicationState Owner_GetApplicationState(SecurityCredentials sc, string appId);
        void Owner_AbortThread(SecurityCredentials sc, ThreadIdentifier ti);
        void Owner_StopApplication(SecurityCredentials sc, string appId);

        //
        // executor services
        //
        string Executor_RegisterNewExecutor(SecurityCredentials sc, ExecutorInfo info);
        void Executor_ConnectDedicatedExecutor(SecurityCredentials sc, string executorId, RemoteEndPoint executorEP);
        void Executor_ConnectNonDedicatedExecutor(SecurityCredentials sc, string executorId);
        void Executor_DisconnectExecutor(SecurityCredentials sc, string executorId);
        ThreadIdentifier Executor_GetNextScheduledThreadIdentifier(SecurityCredentials sc, string executorId);
        FileDependencyCollection Executor_GetApplicationManifest(SecurityCredentials sc, string appId);
        byte[] Executor_GetThread(SecurityCredentials sc, ThreadIdentifier ti);
        void Executor_Heartbeat(SecurityCredentials sc, string executorId, HeartbeatInfo info);
        void Executor_SetFinishedThread(SecurityCredentials sc, ThreadIdentifier ti, byte[] thread, Exception e);
        void Executor_RelinquishThread(SecurityCredentials sc, ThreadIdentifier ti);
        
        //
        // admin/monitoring services
        //
        DataSet Admon_GetLiveApplicationList(SecurityCredentials sc);
        DataSet Admon_GetThreadList(SecurityCredentials sc, string appId);
        DataTable Admon_GetUserList(SecurityCredentials sc);
        DataTable Admon_GetGroups(SecurityCredentials sc);
        void Admon_UpdateUsers(SecurityCredentials sc, DataTable updates);
        void Admon_AddUsers(SecurityCredentials sc, DataTable users);
        DataTable Admon_GetSystemSummary(SecurityCredentials sc);
        DataTable Admon_GetExecutors(SecurityCredentials sc);
    }
}