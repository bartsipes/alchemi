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
using Alchemi.Core.Executor;
using Alchemi.Core.Manager;
using Alchemi.Core.Manager.Storage;
using Alchemi.Core.Owner;

namespace Alchemi.Core
{
	/// <summary>
	/// Defines the functions / services that need to be provided by a manager implementation
	/// </summary>
    public interface IManager : IExecutor
    {
		/// <summary>
		/// Pings the manager to verify if it is alive
		/// </summary>
        void PingManager();

		/// <summary>
		/// Authenticates the user with the given security credentials
		/// </summary>
		/// <param name="sc"></param>
        void AuthenticateUser(SecurityCredentials sc);
        
        //
        // owner services
        //
		/// <summary>
		/// Create an application
		/// <br/>(Generally meant to be called by a Owner of an application)
		/// </summary>
		/// <param name="sc"></param>
		/// <returns>Application id</returns>
        string Owner_CreateApplication(SecurityCredentials sc);
		
		/// <summary>
		/// Verify if an application exists.
		/// <br/>(Generally meant to be called by a Owner of an application)
		/// </summary>
		/// <param name="sc"></param>
		/// <param name="appId"></param>
		/// <returns>true if the application exists in the Manager database</returns>
		bool Owner_VerifyApplication(SecurityCredentials sc, string appId);
        
		/// <summary>
		/// Set the application manifest (file dependencies) for the application with the given id.
		/// <br/>(Generally meant to be called by a Owner of an application)
		/// </summary>
		/// <param name="sc"></param>
		/// <param name="appId"></param>
		/// <param name="manifest"></param>
		void Owner_SetApplicationManifest(SecurityCredentials sc, string appId, FileDependencyCollection manifest);
        
		/// <summary>
		/// Set the thread on the manager. i.e provide the manager with a byte array[] representing the thread code.
		/// <br/>(Generally meant to be called by a Owner of an application)
		/// </summary>
		/// <param name="sc"></param>
		/// <param name="ti"></param>
		/// <param name="thread"></param>
		void Owner_SetThread(SecurityCredentials sc, ThreadIdentifier ti, byte[] thread);
        
		/// <summary>
		/// Retrieve the finished threads for an application with the given id, as a 2-D byte array [][]
		/// </summary>
		/// <param name="sc"></param>
		/// <param name="appId"></param>
		/// <returns>byte array representing all the threads that are finished for the given appication</returns>
		byte[][] Owner_GetFinishedThreads(SecurityCredentials sc, string appId);
        
		/// <summary>
		/// Gets the state of the thread with the given identifier.
		/// <br/>(Generally meant to be called by a Owner of an application)
		/// </summary>
		/// <param name="sc"></param>
		/// <param name="ti"></param>
		/// <returns>ThreadState</returns>
		ThreadState Owner_GetThreadState(SecurityCredentials sc, ThreadIdentifier ti);
        
		/// <summary>
		/// Gets the exception, if any, for a thread. If the thread has failed the exception object contains the Exception that 
		/// caused the failure. Otherwise, the return value is null.
		/// <br/>(Generally meant to be called by a Owner of an application)
		/// </summary>
		/// <param name="sc"></param>
		/// <param name="ti"></param>
		/// <returns></returns>
		Exception Owner_GetFailedThreadException(SecurityCredentials sc, ThreadIdentifier ti);
		
		/// <summary>
		/// Gets the state of the application with the given id.
		/// <br/>(Generally meant to be called by a Owner of an application)
		/// </summary>
		/// <param name="sc"></param>
		/// <param name="appId"></param>
		/// <returns>ApplicationState</returns>
		ApplicationState Owner_GetApplicationState(SecurityCredentials sc, string appId);

		/// <summary>
		/// Aborts the thread with the given identifier.
		/// <br/>(Generally meant to be called by a Owner of an application)
		/// </summary>
		/// <param name="sc"></param>
		/// <param name="ti"></param>
        void Owner_AbortThread(SecurityCredentials sc, ThreadIdentifier ti);

		/// <summary>
		/// Stops the application with the given id.
		/// <br/>(Generally meant to be called by a Owner of an application)
		/// </summary>
		/// <param name="sc"></param>
		/// <param name="appId"></param>
        void Owner_StopApplication(SecurityCredentials sc, string appId);

		/// <summary>
		/// Cleans up the files for an application with the given id.
		/// <br/>(Generally meant to be called by a Owner of an application)
		/// </summary>
		/// <param name="sc"></param>
		/// <param name="appId"></param>
		void Owner_CleanupApplication(SecurityCredentials sc, string appId);

		/// <summary>
		/// Gets the number of threads finished for an application with the given id.
		/// <br/>(Generally meant to be called by a Owner of an application)
		/// </summary>
		/// <param name="sc"></param>
		/// <param name="appId"></param>
		/// <returns>Number of finished threads</returns>
		int Owner_GetFinishedThreadCount(SecurityCredentials sc, string appId);

        //
        // executor services
        //

		/// <summary>
		/// Registers a new Executor with the Manager.
		/// <br/>(Generally meant to be called by a Executor)
		/// </summary>
		/// <param name="sc"></param>
		/// <param name="info"></param>
		/// <returns>Executor id</returns>
        string Executor_RegisterNewExecutor(SecurityCredentials sc, ExecutorInfo info);
        
		/// <summary>
		/// Connects an Executor to the Manager in dedicated mode.
		/// <br/>(Generally meant to be called by a Executor)
		/// </summary>
		/// <param name="sc"></param>
		/// <param name="executorId"></param>
		/// <param name="executorEP"></param>
		void Executor_ConnectDedicatedExecutor(SecurityCredentials sc, string executorId, RemoteEndPoint executorEP);
		
		/// <summary>
		/// Connects an Executor to the Manager in non-dedicated mode.
		/// <br/>(Generally meant to be called by a Executor)
		/// </summary>
		/// <param name="sc"></param>
		/// <param name="executorId"></param>
		void Executor_ConnectNonDedicatedExecutor(SecurityCredentials sc, string executorId, RemoteEndPoint executorEP);
        
		/// <summary>
		/// Disconnects an Executor from the Manager.
		/// <br/>(Generally meant to be called by a Executor)
		/// </summary>
		/// <param name="sc"></param>
		/// <param name="executorId"></param>
		void Executor_DisconnectExecutor(SecurityCredentials sc, string executorId);

		/// <summary>
		/// Gets the thread-identifier of the next thread scheduled to be executed.
		/// <br/>(Generally meant to be called by a Executor)
		/// </summary>
		/// <param name="sc"></param>
		/// <param name="executorId"></param>
		/// <returns>ThreadIdentifier</returns>
        ThreadIdentifier Executor_GetNextScheduledThreadIdentifier(SecurityCredentials sc, string executorId);
        
		/// <summary>
		/// Gets the manifest of the application with the given id.
		/// <br/>(Generally meant to be called by a Executor)
		/// </summary>
		/// <param name="sc"></param>
		/// <param name="appId"></param>
		/// <returns>FileDependencyCollection</returns>
		FileDependencyCollection Executor_GetApplicationManifest(SecurityCredentials sc, string appId);

		/// <summary>
		/// Gets the thread with the given id in the form of a byte array. This is the code to be executed on the Executor.
		/// <br/>(Generally meant to be called by a Executor)
		/// </summary>
		/// <param name="sc"></param>
		/// <param name="ti"></param>
		/// <returns>byte array [] representing the thread </returns>
        byte[] Executor_GetThread(SecurityCredentials sc, ThreadIdentifier ti);

		/// <summary>
		/// Notifies the Manager about the status of the Executor with the "heartbeat" information
		/// <br/>(Generally meant to be called by a Executor)
		/// </summary>
		/// <param name="sc"></param>
		/// <param name="executorId"></param>
		/// <param name="info"></param>
        void Executor_Heartbeat(SecurityCredentials sc, string executorId, HeartbeatInfo info);

		/// <summary>
		/// Returns the finished thread to the Manager.
		/// <br/>(Generally meant to be called by a Executor)
		/// </summary>
		/// <param name="sc"></param>
		/// <param name="ti"></param>
		/// <param name="thread"></param>
		/// <param name="e"></param>
        void Executor_SetFinishedThread(SecurityCredentials sc, ThreadIdentifier ti, byte[] thread, Exception e);

		/// <summary>
		/// Informs the manager that the Executor has given up execution of the thread with the given id. 
		/// <br/>(Generally meant to be called by a Executor)
		/// </summary>
		/// <param name="sc"></param>
		/// <param name="ti"></param>
        void Executor_RelinquishThread(SecurityCredentials sc, ThreadIdentifier ti);
        
        //
        // admin/monitoring services
        //

		/// <summary>
		/// Gets the list of all the applications.
		/// 
		/// Updates: 
		/// 
		///	23 October 2005 - Tibor Biro (tb@tbiro.com) - Changed the Application data from a DataSet 
		///		to ApplicationStorageView
		/// 
		/// </summary>
		/// <param name="sc"></param>
		/// <returns>ApplicationStorageView array with application information</returns>
        ApplicationStorageView[] Admon_GetLiveApplicationList(SecurityCredentials sc);

		/// <summary>
		/// Gets the application list for the given user.
		/// 
		/// Updates: 
		/// 
		///	23 October 2005 - Tibor Biro (tb@tbiro.com) - Changed the Application data from a DataSet 
		///		to ApplicationStorageView
		/// </summary>
		/// <param name="sc"></param>
		/// <returns>ApplicationStorageView array with application information</returns>
		ApplicationStorageView[] Admon_GetUserApplicationList(SecurityCredentials sc);

		/// <summary>
		/// Gets the list of thread for the given application
		/// </summary>
		/// <param name="sc"></param>
		/// <param name="appId"></param>
		/// <returns>Dataset with thread information</returns>
        DataSet Admon_GetThreadList(SecurityCredentials sc, string appId);

		/// <summary>
		/// Gets the list of threads with a given status.
		/// </summary>
		/// <param name="sc"></param>
		/// <param name="appId"></param>
		/// <param name="status"></param>
		/// <returns>Dataset with thread information</returns>
		DataSet Admon_GetThreadList(SecurityCredentials sc, string appId, ThreadState status);

		/// <summary>
		/// Gets the list of users.
		/// </summary>
		/// <param name="sc"></param>
		/// <returns>DataTabke with user information</returns>
        DataTable Admon_GetUserList(SecurityCredentials sc);

		/// <summary>
		/// Gets the list of groups
		/// </summary>
		/// <param name="sc"></param>
		/// <returns>DataTable with group information</returns>
        DataTable Admon_GetGroups(SecurityCredentials sc);

		/// <summary>
		/// Updates the Manager database with the given table of users.
		/// </summary>
		/// <param name="sc"></param>
		/// <param name="updates"></param>
        void Admon_UpdateUsers(SecurityCredentials sc, DataTable updates);

		/// <summary>
		/// Adds all the users in the given table to the Manager database
		/// </summary>
		/// <param name="sc"></param>
		/// <param name="users"></param>
        void Admon_AddUsers(SecurityCredentials sc, DataTable users);

		/// <summary>
		/// Gets the system summary information
		/// </summary>
		/// <param name="sc"></param>
		/// <returns></returns>
        DataTable Admon_GetSystemSummary(SecurityCredentials sc);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sc"></param>
		/// <returns></returns>
        DataTable Admon_GetExecutors(SecurityCredentials sc);

		/// <summary>
		/// Executes an SQL query on the Manager database.
		/// </summary>
		/// <param name="sc"></param>
		/// <param name="perm"></param>
		/// <param name="query"></param>
		/// <returns>Dataset conmtaining the query results</returns>
		DataSet Admon_ExecQuery(SecurityCredentials sc, Permission perm, string query);
    }
}