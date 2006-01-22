#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
* Title         :  IManagerStorage.cs
* Project       :  Alchemi.Core.Manager.Storage
* Created on    :  30 August 2005
* Copyright     :  Copyright © 2006 The University of Melbourne
*                    This technology has been developed with the support of
*                    the Australian Research Council and the University of Melbourne
*                    research grants as part of the Gridbus Project
*                    within GRIDS Laboratory at the University of Melbourne, Australia.
* Author        :  Tibor Biro (tb@tbiro.com)
* License       :  GPL
*                    This program is free software; you can redistribute it and/or
*                    modify it under the terms of the GNU General Public
*                    License as published by the Free Software Foundation;
*                    See the GNU General Public License
*                    (http://www.gnu.org/copyleft/gpl.html) for more 
details.
*
*/
#endregion

using System;
using Alchemi.Core.Owner;
using Alchemi.Core.Utility;

namespace Alchemi.Core.Manager.Storage
{
	/// <summary>
	/// Define the Manager Storage interface.
	/// Contains an entry for each storage operation the manager needs to do
	/// The manager storage is meant to offer a uniform look at the data needed by the manager.
	/// 
	/// Storage guildelines:
	/// Writing and returning data from the storage should not be done with storage specific data structures
	///		such as DataSet, DataTable and so on. Rather, the data should be handled in the format needed by 
	///		the application such as custom objects.
	/// </summary>
	public interface IManagerStorage
	{
		/// <summary>
		/// Verifies if the connection to the back-end storage is alive and valid
		/// </summary>
		/// <returns></returns>
		bool VerifyConnection();

		/// <summary>
		/// Check if a permission is set.
		/// </summary>
		/// <param name="sc">Security credentials to use in the check.</param>
		/// <param name="perm">Permission to check for</param>
		/// <returns>true if the permission is set, false otherwise</returns>
		bool CheckPermission(SecurityCredentials sc, Permission perm);

		/// <summary>
		/// Authenticate a user's security credentials
		/// </summary>
		/// <param name="sc">Security credentials to authenticate</param>
		/// <returns>True if the authentication is successful, false otherwise.</returns>
		bool AuthenticateUser(SecurityCredentials sc);

		/// <summary>
		/// Add a list of users to the storage.
		/// </summary>
		/// <param name="users"></param>
		void AddUsers(UserStorageView[] users);

		void UpdateUsers(UserStorageView[] updates);

		/// <summary>
		/// Get an array with all the users found in the current storage
		/// </summary>
		/// <returns></returns>
		UserStorageView[] GetUsers();

		/// <summary>
		/// Delete the given user.
		/// Only the username has to be set in the UserStorageView structure, all other data is ignored.
		/// </summary>
		/// <param name="userToDelete"></param>
		void DeleteUser(UserStorageView userToDelete);
		
		void AddGroups(GroupStorageView[] groups);
		
		GroupStorageView[] GetGroups();

		GroupStorageView GetGroup(Int32 groupId);

		void AddGroupPermission(Int32 groupId, Permission permission);

		Permission[] GetGroupPermissions(Int32 groupId);

		PermissionStorageView[] GetGroupPermissionStorageView(Int32 groupId);

		/// <summary>
		/// Delete the given group and all the users associated with it.
		/// </summary>
		/// <param name="groupToDelete"></param>
		void DeleteGroup(GroupStorageView groupToDelete);

		UserStorageView[] GetGroupUsers(Int32 groupId);

		String AddExecutor(ExecutorStorageView executor);

		void UpdateExecutor(ExecutorStorageView executor);

		ExecutorStorageView[] GetExecutors();

		ExecutorStorageView[] GetExecutors(TriStateBoolean dedicated);

		ExecutorStorageView[] GetExecutors(TriStateBoolean dedicated, TriStateBoolean connected);

		ExecutorStorageView GetExecutor(String executorId);

		String AddApplication(ApplicationStorageView application);

		void UpdateApplication(ApplicationStorageView updatedApplication);

		ApplicationStorageView[] GetApplications();

		ApplicationStorageView[] GetApplications(bool populateThreadCount);

		/// <summary>
		/// Delete application and all associated threads
		/// </summary>
		/// <param name="applicationToDelete"></param>
		void DeleteApplication(ApplicationStorageView applicationToDelete);

		/// <summary>
		/// Get the user's applications
		/// </summary>
		/// <param name="userName"></param>
		/// <returns></returns>
		ApplicationStorageView[] GetApplications(String userName, bool populateThreadCount);

		ApplicationStorageView GetApplication(String applicationId);


		Int32 AddThread(ThreadStorageView thread);

		void UpdateThread(ThreadStorageView updatedThread);

		ThreadStorageView GetThread(String applicationId, Int32 threadId);
		
		ThreadStorageView[] GetThreads(params ThreadState[] state);

		ThreadStorageView[] GetThreads(String applicationId, params ThreadState[] state);

		ThreadStorageView[] GetExecutorThreads(String executorId, params ThreadState[] state);

		ThreadStorageView[] GetExecutorThreads(bool dedicatedExecutor, params ThreadState[] state);

		ThreadStorageView[] GetExecutorThreads(bool dedicatedExecutor, bool connectedExecutor, params ThreadState[] state);

		void GetApplicationThreadCount(String applicationId, out Int32 totalthreads, out Int32 unfinishedThreads);

		Int32 GetApplicationThreadCount(String applicationId, ThreadState threadState);

		Int32 GetExecutorThreadCount(String executorId, params ThreadState[] threadState);

		void DeleteThread(ThreadStorageView threadToDelete);


		/// <summary>
		/// Get system summary information from the storage.
		/// </summary>
		/// <returns>
		/// An object with the summary information or null if the storage does not implement system summary.
		/// </returns>
		SystemSummary GetSystemSummary();

		//for generic queries
		///	22 January 2006 - Tibor Biro (tb@tbiro.com) - Removed from the IManagerStorage interface.
		//DataSet RunSqlReturnDataSet(string query);

		///	22 January 2006 - Tibor Biro (tb@tbiro.com) - Do not use through this interface!
		///	This will be removed as soon as the DBInstall utility is retired.
		void RunSql(string sqlQuery);
	}
}
