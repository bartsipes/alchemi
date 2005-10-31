#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
* Title         :  InMemoryManagerStorage.cs
* Project       :  Alchemi.Core.Manager.Storage
* Created on    :  30 August 2005
* Copyright     :  Copyright © 2005 The University of Melbourne
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
using System.Collections;

using Alchemi.Core.Owner;

namespace Alchemi.Core.Manager.Storage
{
	/// <summary>
	/// Store all manager information in memory.
	/// 
	/// This type of storage is not persistent but usefull for testing or for running 
	/// lightweight managers.
	/// </summary>
	public class InMemoryManagerStorage : IManagerStorage
	{
		private ArrayList m_users;
		private ArrayList m_groups;
		private Hashtable m_groupPermissions;
		private ArrayList m_executors;
		private ArrayList m_applications;
		private ArrayList m_threads;

		public InMemoryManagerStorage()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		#region IManagerStorage Members

		public SystemSummary GetSystemSummary()
		{
			throw new Exception("Not implemented");
		}

		public void AddUsers(UserStorageView[] users)
		{
			if (users == null)
			{
				return;
			}

			if (m_users == null)
			{
				m_users = new ArrayList();
			}

			m_users.AddRange(users);
		}

		public void UpdateUsers(UserStorageView[] updates)
		{
			if (m_users == null || updates == null)
			{
				return;
			}

			for(int indexInList=0; indexInList<m_users.Count; indexInList++)
			{
				UserStorageView userInList = (UserStorageView)m_users[indexInList];

				foreach(UserStorageView userInUpdates in updates)
				{
					if (userInList.Username == userInUpdates.Username)
					{
						userInList.Password = userInUpdates.Password;
						userInList.GroupId = userInUpdates.GroupId;
					}
				}
			}
		}

		public bool AuthenticateUser(SecurityCredentials sc)
		{
			if (sc == null || m_users == null)
			{
				return false;
			}

			for(int index=0; index<m_users.Count; index++)
			{
				UserStorageView user = (UserStorageView)m_users[index];

				if (user.Username == sc.Username && user.Password == sc.Password)
				{
					return true;
				}
			}

			return false;
		}

		public UserStorageView[] GetUsers()
		{
			if (m_users == null)
			{
				return new UserStorageView[0];
			}
			else
			{
				return (UserStorageView[])m_users.ToArray(typeof(UserStorageView));
			}
		}

		public void AddGroups(GroupStorageView[] groups)
		{
			if (groups == null)
			{
				return;
			}

			if (m_groups == null)
			{
				m_groups = new ArrayList();
			}

			m_groups.AddRange(groups);
		}
		
		public GroupStorageView[] GetGroups()
		{
			if (m_groups == null)
			{
				return new GroupStorageView[0];
			}
			else
			{
				return (GroupStorageView[])m_groups.ToArray(typeof(GroupStorageView));
			}
		}

		public void AddGroupPermission(Int32 groupId, Permission permission)
		{
			if (m_groupPermissions == null)
			{
				m_groupPermissions = new Hashtable();
			}

			ArrayList permissions = null;

			if (m_groupPermissions[groupId] != null)
			{
				permissions = (ArrayList)m_groupPermissions[groupId];
			}
			else
			{
				permissions = new ArrayList();

				m_groupPermissions.Add(groupId, permissions);
			}

			Int32 index = permissions.IndexOf(permission);

			// only add it if it is not already in the array
			if (index < 0)
			{
				permissions.Add(permission);
			}

			m_groupPermissions[groupId] = permissions;
		}

		public Permission[] GetGroupPermissions(Int32 groupId)
		{
			if (m_groupPermissions == null || m_groupPermissions[groupId] == null)
			{
				return new Permission[0];
			}

			ArrayList permissions = (ArrayList)m_groupPermissions[groupId];

			return (Permission[])permissions.ToArray(typeof(Permission));
		}

		public bool CheckPermission(SecurityCredentials sc, Permission perm)
		{
			if (m_users == null || m_groups == null || m_groupPermissions == null)
			{
				return false;
			}

			// get the user's groupId
			Int32 groupId = -1;
			foreach(UserStorageView user in m_users)
			{
				if(String.Compare(user.Username, sc.Username, true) == 0 && user.Password == sc.Password)
				{
					groupId = user.GroupId;
					break;
				}
			}

			if (groupId == -1)
			{
				return false;
			}

			foreach(Permission permission in GetGroupPermissions(groupId))
			{
				// in the SQL implementation the higher leverl permissions are considered to 
				// include the lower leverl permissions
				if ((int)permission >= (int)perm)
				{
					return true;
				}
			}

			return false;
		}

		public String AddExecutor(ExecutorStorageView executor)
		{
			if (executor == null)
			{
				return null;
			}

			if (m_executors == null)
			{
				m_executors = new ArrayList();
			}

			String executorId = Guid.NewGuid().ToString();

			executor.ExecutorId = executorId;

			m_executors.Add(executor);

			return executorId;
		}

		public void UpdateExecutor(ExecutorStorageView updatedExecutor)
		{
			if (m_executors == null || updatedExecutor == null)
			{
				return;
			}

			ArrayList newExecutorList = new ArrayList();

			foreach(ExecutorStorageView executor in m_executors)
			{
				if (executor.ExecutorId == updatedExecutor.ExecutorId)
				{
					newExecutorList.Add(updatedExecutor);
				}
				else
				{
					newExecutorList.Add(executor);
				}
			}

			m_executors = newExecutorList;
		}

		public ExecutorStorageView[] GetExecutors()
		{
			if (m_executors == null)
			{
				return new ExecutorStorageView[0];
			}
			else
			{
				return (ExecutorStorageView[])m_executors.ToArray(typeof(ExecutorStorageView));
			}
		}

		public String AddApplication(ApplicationStorageView application)
		{
			if (application == null)
			{
				return null;
			}

			if (m_applications == null)
			{
				m_applications = new ArrayList();
			}

			String applicationId = Guid.NewGuid().ToString();

			application.ApplicationId = applicationId;

			m_applications.Add(application);

			return applicationId;
		}

		public void UpdateApplication(ApplicationStorageView updatedApplication)
		{
			if (m_applications == null || updatedApplication == null)
			{
				return;
			}

			ArrayList newApplicationList = new ArrayList();

			foreach(ApplicationStorageView application in m_applications)
			{
				if (application.ApplicationId == updatedApplication.ApplicationId)
				{
					newApplicationList.Add(updatedApplication);
				}
				else
				{
					newApplicationList.Add(application);
				}
			}

			m_applications = newApplicationList;
		}

		public ApplicationStorageView[] GetApplications()
		{
			return GetApplications(false);
		}

		public ApplicationStorageView[] GetApplications(bool populateThreadCount)
		{
			if (m_applications == null || m_applications.Count == 0)
			{
				return new ApplicationStorageView[0];
			}

			ArrayList applicationList = new ArrayList();

			foreach(ApplicationStorageView application in m_applications)
			{
				if (populateThreadCount)
				{
					Int32 totalThreads;
					Int32 unfinishedThreads;

					GetApplicationThreadCount(application.ApplicationId, out totalThreads, out unfinishedThreads);

					application.TotalThreads = totalThreads;
					application.UnfinishedThreads = unfinishedThreads;
				}

				applicationList.Add(application);
			}

			return (ApplicationStorageView[])applicationList.ToArray(typeof(ApplicationStorageView));
		}

		public ApplicationStorageView[] GetApplications(String userName, bool populateThreadCount)
		{
			ArrayList applicationList = new ArrayList();

			foreach(ApplicationStorageView application in m_applications)
			{
				if (String.Compare(application.Username, userName, false) == 0)
				{
					if (populateThreadCount)
					{
						Int32 totalThreads;
						Int32 unfinishedThreads;

						GetApplicationThreadCount(application.ApplicationId, out totalThreads, out unfinishedThreads);

						application.TotalThreads = totalThreads;
						application.UnfinishedThreads = unfinishedThreads;
					}

					applicationList.Add(application);
				}
			}

			return (ApplicationStorageView[])applicationList.ToArray(typeof(ApplicationStorageView));
		}

		public ApplicationStorageView GetApplication(String applicationId)
		{
			if (m_applications == null)
			{
				return null;
			}

			IEnumerator enumerator = m_applications.GetEnumerator();

			while(enumerator.MoveNext())
			{
				ApplicationStorageView application = (ApplicationStorageView)enumerator.Current;

				if (application.ApplicationId == applicationId)
				{
					return application;
				}
			}

			// data not found
			return null;

		}


		public Int32 AddThread(ThreadStorageView thread)
		{
			if (thread == null)
			{
				return -1;
			}

			if (m_threads == null)
			{
				m_threads = new ArrayList();
			}

			lock(m_threads)
			{
				// generate the next threadID from the length, this will make sure the thread ID is unique
				// generating from the length also requires thread synchronization code here
				thread.InternalThreadId = m_threads.Count;

				m_threads.Add(thread);
			}

			return thread.InternalThreadId;
		}

		public void UpdateThread(ThreadStorageView updatedThread)
		{
			if (m_threads == null || updatedThread == null)
			{
				return;
			}

			ArrayList newThreadList = new ArrayList();

			foreach(ThreadStorageView thread in m_threads)
			{
				if (thread.InternalThreadId == updatedThread.InternalThreadId)
				{
					newThreadList.Add(updatedThread);
				}
				else
				{
					newThreadList.Add(thread);
				}
			}

			m_threads = newThreadList;
		}

		public ThreadStorageView[] GetThreads()
		{
			if (m_threads == null)
			{
				return new ThreadStorageView[0];
			}
			else
			{
				return (ThreadStorageView[])m_threads.ToArray(typeof(ThreadStorageView));
			}
		}

		public ThreadStorageView[] GetThreads(String applicationId)
		{
			return GetThreads(applicationId, ThreadState.Unknown);
		}

		public ThreadStorageView[] GetThreads(String applicationId, ThreadState state)
		{
			if (m_threads == null)
			{
				return new ThreadStorageView[0];
			}

			ArrayList threadList = new ArrayList();

			foreach(ThreadStorageView thread in m_threads)
			{
				bool threadStateCorrectOrUnknown = thread.State == state || state == ThreadState.Unknown;
				if (thread.ApplicationId == applicationId && threadStateCorrectOrUnknown)
				{
					threadList.Add(thread);
				}
			}

			return (ThreadStorageView[])threadList.ToArray(typeof(ThreadStorageView));
		}

		public void GetApplicationThreadCount(String applicationId, out Int32 totalThreads, out Int32 unfinishedThreads)
		{
			totalThreads = unfinishedThreads = 0;

			if (m_threads == null || m_threads.Count == 0)
			{
				return;
			}

			foreach(ThreadStorageView thread in m_threads)
			{
				if (thread.ApplicationId == applicationId)
				{
					totalThreads ++;

					if (thread.State == ThreadState.Ready || thread.State == ThreadState.Scheduled || thread.State == ThreadState.Started)
					{
						unfinishedThreads ++;
					}
				}
			}
		}

		public Int32 GetThreadCount(String applicationId, ThreadState threadState)
		{
			Int32 threadCount = 0;

			if (m_threads == null || m_threads.Count == 0)
			{
				return threadCount;
			}

			foreach(ThreadStorageView thread in m_threads)
			{
				if (thread.ApplicationId == applicationId && thread.State == threadState)
				{
					threadCount ++;
				}
			}

			return threadCount;
		}

		#endregion
	}
}
