#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   IManager.cs
 * Project      :   Alchemi.Core
 * Created on   :   2003
 * Copyright    :   Copyright © 2006 The University of Melbourne
 *                  This technology has been developed with the support of 
 *                  the Australian Research Council and the University of Melbourne
 *                  research grants as part of the Gridbus Project
 *                  within GRIDS Laboratory at the University of Melbourne, Australia.
 * Author       :   Akshay Luther (akshayl@csse.unimelb.edu.au)
 *                  Rajkumar Buyya (raj@csse.unimelb.edu.au)
 *                  Krishna Nadiminti (kna@csse.unimelb.edu.au)
 * License      :   GPL
 *                  This program is free software; you can redistribute it and/or 
 *                  modify it under the terms of the GNU General Public
 *                  License as published by the Free Software Foundation;
 *                  See the GNU General Public License 
 *                  (http://www.gnu.org/copyleft/gpl.html) for more details.
 *
 */
#endregion

using System;
using System.ServiceModel;

using Alchemi.Core.Executor;
using Alchemi.Core.Manager.Storage;
using Alchemi.Core.Owner;

namespace Alchemi.Core
{
	/// <summary>
	/// Defines the functions/services that need to be provided by a manager implementation
	/// </summary>
    [ServiceContract]
    public interface IManager : IExecutor
    {
		/// <summary>
		/// Pings the manager to verify if it is alive.
		/// </summary>
        [OperationContract]
        void PingManager();

        /// <summary>
        /// Authenticates the user with the given security credentials.
        /// </summary>
        /// <param name="sc">The security credentials.</param>
        [OperationContract]
        void AuthenticateUser(SecurityCredentials sc);


        #region Owner Services
        /// <summary>
        /// Create an application
        /// <br/>(Generally meant to be called by a Owner of an application)
        /// </summary>
        /// <param name="sc"></param>
        /// <returns>Application id</returns>
        [OperationContract]
        string Owner_CreateApplication(SecurityCredentials sc);

        /// <summary>
        /// Set the application human readable name.
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="appId"></param>
        /// <param name="applicationName"></param>
        [OperationContract]
        void Owner_SetApplicationName(SecurityCredentials sc, string appId, string applicationName);

        /// <summary>
        /// Verify if an application exists.
        /// <br/>(Generally meant to be called by a Owner of an application)
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="appId"></param>
        /// <returns>true if the application exists in the Manager database</returns>
        [OperationContract]
        bool Owner_VerifyApplication(SecurityCredentials sc, string appId);

        /// <summary>
        /// Set the application manifest (file dependencies) for the application with the given id.
        /// <br/>(Generally meant to be called by a Owner of an application)
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="appId"></param>
        /// <param name="manifest"></param>
        [OperationContract]
        void Owner_SetApplicationManifest(SecurityCredentials sc, string appId, FileDependencyCollection manifest);


        /// <summary>
        /// Determines whether the manager has the application manifest for the given application id.
        /// <br/>(Generally meant to be called by a Owner of an application)
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        [OperationContract]
        bool Owner_HasApplicationManifest(SecurityCredentials sc, string appId);

        /// <summary>
        /// Set the thread on the manager. i.e provide the manager with a byte array[] representing the thread code.
        /// <br/>(Generally meant to be called by a Owner of an application)
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="ti"></param>
        /// <param name="thread"></param>
        [OperationContract]
        void Owner_SetThread(SecurityCredentials sc, ThreadIdentifier ti, byte[] thread);

        /// <summary>
        /// Set a group of threads on the manager. i.e provide the manager with a byte array[][] representing the threads code.
        /// <br/>(Generally meant to be called by a Owner of an application)
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="ti"></param>
        /// <param name="thread"></param>
        [OperationContract]
        void Owner_SetThreads(SecurityCredentials sc, ThreadIdentifier[] threadIds, byte[][] threads);

        /// <summary>
        /// Retrieve the finished threads for an application with the given id, as a 2-D byte array [][]
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="appId"></param>
        /// <returns>byte array representing all the threads that are finished for the given appication</returns>
        [OperationContract]
        byte[][] Owner_GetFinishedThreads(SecurityCredentials sc, string appId);

        /// <summary>
        /// Gets the state of the thread with the given identifier.
        /// <br/>(Generally meant to be called by a Owner of an application)
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="ti"></param>
        /// <returns>ThreadState</returns>
        [OperationContract]
        ThreadState Owner_GetThreadState(SecurityCredentials sc, ThreadIdentifier ti);

        /// <summary>
        /// Gets the exception, if any, for a thread. If the thread has failed the exception object contains the Exception that 
        /// caused the failure. Otherwise, the return name is null.
        /// <br/>(Generally meant to be called by a Owner of an application)
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="ti"></param>
        /// <returns></returns>
        [OperationContract]
        Exception Owner_GetFailedThreadException(SecurityCredentials sc, ThreadIdentifier ti);

        /// <summary>
        /// Gets the state of the application with the given id.
        /// <br/>(Generally meant to be called by a Owner of an application)
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="appId"></param>
        /// <returns>ApplicationState</returns>
        [OperationContract]
        ApplicationState Owner_GetApplicationState(SecurityCredentials sc, string appId);

        /// <summary>
        /// Aborts the thread with the given identifier.
        /// <br/>(Generally meant to be called by a Owner of an application)
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="ti"></param>
        [OperationContract]
        void Owner_AbortThread(SecurityCredentials sc, ThreadIdentifier ti);

        /// <summary>
        /// Stops the application with the given id.
        /// <br/>(Generally meant to be called by a Owner of an application)
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="appId"></param>
        [OperationContract]
        void Owner_StopApplication(SecurityCredentials sc, string appId);

        /// <summary>
        /// Cleans up the files for an application with the given id.
        /// <br/>(Generally meant to be called by a Owner of an application)
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="appId"></param>
        [OperationContract]
        void Owner_CleanupApplication(SecurityCredentials sc, string appId);

        /// <summary>
        /// Gets the number of threads finished for an application with the given id.
        /// <br/>(Generally meant to be called by a Owner of an application)
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="appId"></param>
        /// <returns>Number of finished threads</returns>
        [OperationContract]
        int Owner_GetFinishedThreadCount(SecurityCredentials sc, string appId); 
        #endregion


        #region Executor Services
        /// <summary>
        /// Registers a new Executor with the Manager.
        /// <br/>(Generally meant to be called by a Executor)
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="existingExecutorId"></param>
        /// <param name="info"></param>
        /// <returns>Executor id</returns>
        [OperationContract]
        string Executor_RegisterNewExecutor(SecurityCredentials sc, string existingExecutorId, ExecutorInfo info);

        /// <summary>
        /// Connects an Executor to the Manager in dedicated mode.
        /// <br/>(Generally meant to be called by a Executor)
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="executorId"></param>
        /// <param name="executorEP"></param>
        [OperationContract]
        void Executor_ConnectDedicatedExecutor(SecurityCredentials sc, string executorId, EndPoint executorEP);

        /// <summary>
        /// Connects an Executor to the Manager in non-dedicated mode.
        /// <br/>(Generally meant to be called by a Executor)
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="executorId"></param>
        /// <param name="executorEP"></param>
        [OperationContract]
        void Executor_ConnectNonDedicatedExecutor(SecurityCredentials sc, string executorId, EndPoint executorEP);

        /// <summary>
        /// Disconnects an Executor from the Manager.
        /// <br/>(Generally meant to be called by a Executor)
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="executorId"></param>
        [OperationContract]
        void Executor_DisconnectExecutor(SecurityCredentials sc, string executorId);

        /// <summary>
        /// Gets the thread-identifier of the next thread scheduled to be executed.
        /// <br/>(Generally meant to be called by a Executor)
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="executorId"></param>
        /// <returns>ThreadIdentifier</returns>
        [OperationContract]
        ThreadIdentifier Executor_GetNextScheduledThreadIdentifier(SecurityCredentials sc, string executorId);

        /// <summary>
        /// Gets the manifest of the application with the given id.
        /// <br/>(Generally meant to be called by a Executor)
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="appId"></param>
        /// <returns>FileDependencyCollection</returns>
        [OperationContract]
        FileDependencyCollection Executor_GetApplicationManifest(SecurityCredentials sc, string appId);

        /// <summary>
        /// Gets the thread with the given id in the form of a byte array. This is the code to be executed on the Executor.
        /// <br/>(Generally meant to be called by a Executor)
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="ti"></param>
        /// <returns>byte array [] representing the thread </returns>
        [OperationContract]
        byte[] Executor_GetThread(SecurityCredentials sc, ThreadIdentifier ti);

        /// <summary>
        /// Notifies the Manager about the status of the Executor with the "heartbeat" information
        /// <br/>(Generally meant to be called by a Executor)
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="executorId"></param>
        /// <param name="info"></param>
        [OperationContract]
        void Executor_Heartbeat(SecurityCredentials sc, string executorId, HeartbeatInfo info);

        /// <summary>
        /// Returns the finished thread to the Manager.
        /// <br/>(Generally meant to be called by a Executor)
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="ti"></param>
        /// <param name="thread"></param>
        /// <param name="e"></param>
        [OperationContract]
        void Executor_SetFinishedThread(SecurityCredentials sc, ThreadIdentifier ti, byte[] thread, Exception e);

        /// <summary>
        /// Informs the manager that the Executor has given up execution of the thread with the given id. 
        /// <br/>(Generally meant to be called by a Executor)
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="ti"></param>
        [OperationContract]
        void Executor_RelinquishThread(SecurityCredentials sc, ThreadIdentifier ti); 
        #endregion
        

        #region Admin/monitoring Services
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
        [OperationContract]
        ApplicationStorageView[] Admon_GetLiveApplicationList(SecurityCredentials sc);

        /// <summary>
        /// Gets the application list for the given user.
        /// 
        /// Updates: 
        /// 
        ///	23 October 2005 - Tibor Biro (tb@tbiro.com) - Changed the Application data from a DataSet DataSet 
        ///		to ApplicationStorageView
        /// </summary>
        /// <param name="sc"></param>
        /// <returns>ApplicationStorageView array with application information</returns>
        [OperationContract]
        ApplicationStorageView[] Admon_GetUserApplicationList(SecurityCredentials sc);

        /// <summary>
        /// Gets the list of thread for the given application
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="appId"></param>
        /// <returns>ThreadStorageView array with thread list</returns>
        [OperationContract]
        ThreadStorageView[] Admon_GetThreadList(SecurityCredentials sc, string appId);

        /// <summary>
        /// Gets the list of threads with a given status.
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="appId"></param>
        /// <param name="status"></param>
        /// <returns>ThreadStorageView array with thread list</returns>
        [OperationContract(Name = "Admon_GetThreadListByThreadState")]
        ThreadStorageView[] Admon_GetThreadList(SecurityCredentials sc, string appId, ThreadState status);

        /// <summary>
        /// Gets the list of users.
        /// </summary>
        /// <param name="sc"></param>
        /// <returns>DataTabke with user information</returns>
        [OperationContract]
        UserStorageView[] Admon_GetUserList(SecurityCredentials sc);

        /// <summary>
        /// Gets the list of groups
        /// </summary>
        /// <param name="sc"></param>
        /// <returns>DataTable with group information</returns>
        [OperationContract]
        GroupStorageView[] Admon_GetGroups(SecurityCredentials sc);

        /// <summary>
        /// Gets group details.
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="groupId"></param>
        /// <returns>Group details</returns>
        [OperationContract]
        GroupStorageView Admon_GetGroup(SecurityCredentials sc, int groupId);

        /// <summary>
        /// Delete a group and all the associated users.
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="groupToDelete"></param>
        [OperationContract]
        void Admon_DeleteGroup(SecurityCredentials sc, GroupStorageView groupToDelete);

        /// <summary>
        /// Get the users associated with a group.
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [OperationContract]
        UserStorageView[] Admon_GetGroupUsers(SecurityCredentials sc, int groupId);

        /// <summary>
        /// Updates the Manager database with the given table of users.
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="updates"></param>
        [OperationContract]
        void Admon_UpdateUsers(SecurityCredentials sc, UserStorageView[] updates);

        /// <summary>
        /// Adds all the users in the given table to the Manager database
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="users"></param>
        [OperationContract]
        void Admon_AddUsers(SecurityCredentials sc, UserStorageView[] users);

        /// <summary>
        /// Remove a user from the database;
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="userToDelete"></param>
        [OperationContract]
        void Admon_DeleteUser(SecurityCredentials sc, UserStorageView userToDelete);

        /// <summary>
        /// Gets the system summary information
        /// </summary>
        /// <param name="sc"></param>
        /// <returns></returns>
        [OperationContract]
        SystemSummary Admon_GetSystemSummary(SecurityCredentials sc);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sc"></param>
        /// <returns></returns>
        [OperationContract]
        ExecutorStorageView[] Admon_GetExecutors(SecurityCredentials sc);

        /// <summary>
        /// Get executor details
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="executorId"></param>
        /// <returns></returns>
        [OperationContract]
        ExecutorStorageView Admon_GetExecutor(SecurityCredentials sc, string executorId);

        /// <summary>
        /// Delete the given thread form the database.
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="threadToDelete"></param>
        [OperationContract]
        void Admon_DeleteThread(SecurityCredentials sc, ThreadStorageView threadToDelete);

        /// <summary>
        /// Delete an application and alal associated threads.
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="applicationToDelete"></param>
        [OperationContract]
        void Admon_DeleteApplication(SecurityCredentials sc, ApplicationStorageView applicationToDelete);

        /// <summary>
        /// Get a list of permissions for a group.
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        [OperationContract]
        PermissionStorageView[] Admon_GetGroupPermissions(SecurityCredentials sc, GroupStorageView group);

        /// <summary>
        /// Get a list of all permissions defined in the application
        /// </summary>
        /// <param name="sc"></param>
        /// <returns></returns>
        [OperationContract]
        PermissionStorageView[] Admon_GetPermissions(SecurityCredentials sc);

        /// <summary>
        /// Performs maintenance on the manager storage using the given parameters.
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="maintenanceParameters"></param>
        [OperationContract]
        void Admon_PerformStorageMaintenance(SecurityCredentials sc, StorageMaintenanceParameters maintenanceParameters);
        #endregion
    }
}