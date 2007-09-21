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
using System.Data;
using System.Xml;

using Alchemi.Core;
using Alchemi.Core.Owner;
using Alchemi.Core.Manager;
using Alchemi.Core.Manager.Storage;
using Alchemi.Core.Utility;

namespace Alchemi.Manager.Storage
{
	/// <summary>
	/// Store all manager information in memory.
	/// 
	/// This type of storage is not persistent but usefull for testing or for running 
	/// lightweight managers.
	/// </summary>
	public class InMemoryManagerStorage : ManagerStorageBase, IManagerStorage, IManagerStorageSetup
	{
		private ArrayList _users = new ArrayList();
		private ArrayList _groups = new ArrayList();
		private Hashtable _groupPermissions = new Hashtable();
		private ArrayList _executors = new ArrayList();
		private ArrayList _applications = new ArrayList();
		private ArrayList _threads = new ArrayList();

		public InMemoryManagerStorage()
		{
		}

		#region IManagerStorage Members

		public bool VerifyConnection()
		{
			return true; //for a In-memory storage, the connection is always alive and valid.
		}

        public SystemSummary GetSystemSummary()
        {
            // calculate total number of executors
            int totalExecutors = _executors != null ? _executors.Count : 0;

            // calculate number of unfinished applications
            int unfinishedApps = 0;
            foreach( ApplicationStorageView application in _applications )
            {
                if( application.State == ApplicationState.AwaitingManifest || application.State == ApplicationState.Ready )
                {
                    unfinishedApps++;
                }
            }

            int unfinishedThreads = 0;
            foreach( ThreadStorageView thread in _threads )
            {
                if( thread.State != ThreadState.Dead && thread.State != ThreadState.Finished )
                {
                    unfinishedThreads++;
                }
            }

            float maxPowerValue = 0;
            int powerUsage = 0;
            int powerAvailable = 0;
            float totalUsageValue = 0;

            int connectedExecutorCount = 0;

            foreach( ExecutorStorageView executor in _executors )
            {
                if( executor.Connected )
                {
                    connectedExecutorCount++;
                    maxPowerValue += executor.MaxCpu;
                    powerAvailable += executor.AvailableCpu;
                    powerUsage += executor.CpuUsage;
                    totalUsageValue += executor.TotalCpuUsage * executor.MaxCpu / ( 3600 * 1000 );
                }
            }

            if (connectedExecutorCount != 0)
            {
                powerAvailable /= connectedExecutorCount;
                powerUsage /= connectedExecutorCount;
            }

            string powerTotalUsage = String.Format( "{0} GHz*Hr", Math.Round( totalUsageValue, 6 ) );
            string maxPower = String.Format( "{0} GHz", Math.Round( maxPowerValue / 1000, 6 ) );

            SystemSummary summary = new SystemSummary(
                maxPower,
                totalExecutors,
                powerUsage,
                powerAvailable,
                powerTotalUsage,
                unfinishedApps,
                unfinishedThreads );

            return summary;
        }

		public DataSet RunSqlReturnDataSet(string query)
		{
			//throw new NotImplementedException();
			return null;
		}//TODO: need to get rid of this from the interface
		public void RunSql(string sqlQuery)
		{
			//throw new NotImplementedException();
		} //TODO: need to clean this up, and get rid of this from the interface

		public void AddUsers(UserStorageView[] users)
		{
            if (users == null)
                return;

			_users.AddRange(users);
		}

		public void UpdateUsers(UserStorageView[] updates)
		{
			if (updates == null)
			{
				return;
			}

			for(int indexInList=0; indexInList<_users.Count; indexInList++)
			{
				UserStorageView userInList = (UserStorageView)_users[indexInList];

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

		public void DeleteUser(UserStorageView userToDelete)
		{
			if (userToDelete == null)
			{
				return;
			}

			ArrayList remainingUsers = new ArrayList();

			for(int indexInList=0; indexInList<_users.Count; indexInList++)
			{
				UserStorageView userInList = (UserStorageView)_users[indexInList];

				if (userInList.Username != userToDelete.Username)
				{
					remainingUsers.Add(userInList);
				}
			}

			_users = remainingUsers;
		}

		public bool AuthenticateUser(SecurityCredentials sc)
		{
			if (sc == null)
			{
				return false;
			}

			for(int index=0; index<_users.Count; index++)
			{
				UserStorageView user = (UserStorageView)_users[index];

				if (user.Username == sc.Username && user.PasswordMd5Hash == sc.Password)
				{
					return true;
				}
			}

			return false;
		}

        public UserStorageView[] GetUsers()
        {
            return (UserStorageView[]) _users.ToArray( typeof( UserStorageView ) );
        }
        public UserStorageView GetUser(string username)
        {
            foreach (UserStorageView user in _users)
            {
                if (username == user.Username)
                    return user;
            }
            return null;
        }

		public void AddGroups(GroupStorageView[] groups)
		{
            if (groups == null)
                return;
			_groups.AddRange(groups);
		}
		
		public GroupStorageView[] GetGroups()
		{
			return (GroupStorageView[])_groups.ToArray(typeof(GroupStorageView));
		}

		public GroupStorageView GetGroup(int groupId)
		{
			foreach (GroupStorageView group in _groups)
			{
				if (group.GroupId == groupId)
				{
					return group;
				}
			}

			return null;
		}

		public void AddGroupPermission(int groupId, Permission permission)
		{
			ArrayList permissions = null;

			if (_groupPermissions[groupId] != null)
			{
				permissions = (ArrayList)_groupPermissions[groupId];
			}
			else
			{
				permissions = new ArrayList();

				_groupPermissions.Add(groupId, permissions);
			}

			int index = permissions.IndexOf(permission);

			// only add it if it is not already in the array
			if (index < 0)
			{
				permissions.Add(permission);
			}

			_groupPermissions[groupId] = permissions;
		}

		public Permission[] GetGroupPermissions(int groupId)
		{
			if (_groupPermissions[groupId] == null)
			{
				return new Permission[0];
			}

			ArrayList permissions = (ArrayList)_groupPermissions[groupId];

			return (Permission[])permissions.ToArray(typeof(Permission));
		}

		public PermissionStorageView[] GetGroupPermissionStorageView(int groupId)
		{
			return PermissionStorageView.GetPermissionStorageView(GetGroupPermissions(groupId));
		}

		public void DeleteGroup(GroupStorageView groupToDelete)
		{
			if( groupToDelete == null )
			{
				return;
			}

			ArrayList remainingGroups = new ArrayList();
			ArrayList remainingUsers = new ArrayList();

			foreach (UserStorageView user in _users)
			{
				if (user.GroupId != groupToDelete.GroupId)
				{
					remainingUsers.Add(user);
				}
			}

			foreach (GroupStorageView group in _groups)
			{
				if (group.GroupId != groupToDelete.GroupId)
				{
					remainingGroups.Add(group);
				}
			}

			_groups = remainingGroups;
			_users = remainingUsers;
		}

		public UserStorageView[] GetGroupUsers(int groupId)
		{
			ArrayList result = new ArrayList();

			foreach (UserStorageView user in _users)
			{
				if (user.GroupId == groupId)
				{
					result.Add(user);
				}
			}

			return (UserStorageView[])result.ToArray(typeof(UserStorageView));
		}

		public bool CheckPermission(SecurityCredentials sc, Permission perm)
		{
			// get the user's groupId
			int groupId = -1;
			foreach(UserStorageView user in _users)
			{
				if(String.Compare(user.Username, sc.Username, true) == 0 && user.PasswordMd5Hash == sc.Password)
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

		public string AddExecutor(ExecutorStorageView executor)
		{
			if (executor == null)
			{
				return null;
			}

			string executorId;
			if (executor.ExecutorId == null)
			{
				executorId = Guid.NewGuid().ToString();
			}
			else
			{
				executorId = executor.ExecutorId;
			}

			executor.ExecutorId = executorId;

			_executors.Add(executor);

			return executorId;
		}

		public void UpdateExecutor(ExecutorStorageView updatedExecutor)
		{
            if( updatedExecutor == null )
            {
                return;
            }

			ArrayList newExecutorList = new ArrayList();

			foreach(ExecutorStorageView executor in _executors)
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

			_executors = newExecutorList;
		}

        public void DeleteExecutor(ExecutorStorageView executorToDelete)
        {
            if (executorToDelete == null)
            {
                return;
            }

            ArrayList newExecutorList = new ArrayList();

            foreach (ExecutorStorageView executor in _executors)
            {
                if (executor.ExecutorId != executorToDelete.ExecutorId)
                {
                    newExecutorList.Add(executor);
                }
            }

            _executors = newExecutorList;
        }

		public ExecutorStorageView[] GetExecutors()
		{
			return (ExecutorStorageView[])_executors.ToArray(typeof(ExecutorStorageView));
		}

		public ExecutorStorageView[] GetExecutors(TriStateBoolean dedicated)
		{
			return GetExecutors(dedicated, TriStateBoolean.Undefined);
		}

		public ExecutorStorageView[] GetExecutors(TriStateBoolean dedicated, TriStateBoolean connected)
		{
			ArrayList executorList = new ArrayList();

			foreach(ExecutorStorageView executor in _executors)
			{
				bool onlyLookingForDedicatedAndExecutorIsDedicated = (dedicated == TriStateBoolean.True && executor.Dedicated);
				bool dedicatedTestPassed = (onlyLookingForDedicatedAndExecutorIsDedicated || dedicated == TriStateBoolean.Undefined);

				bool onlyLookingForConnectedAndExecutorIsConnected = (connected == TriStateBoolean.True && executor.Connected);
				bool connectedTestPassed = (onlyLookingForConnectedAndExecutorIsConnected || connected == TriStateBoolean.Undefined);
				
				if (dedicatedTestPassed && connectedTestPassed)
				{
					executorList.Add(executor);
				}
			}

			return (ExecutorStorageView[])executorList.ToArray(typeof(ExecutorStorageView));
		}

		public ExecutorStorageView GetExecutor(string executorId)
		{
			foreach(ExecutorStorageView executor in _executors)
			{
				if (executor.ExecutorId == executorId)
				{
					return executor;
				}
			}

			return null;
		}


		public string AddApplication(ApplicationStorageView application)
		{
			if (application == null)
			{
				return null;
			}

			string applicationId = Guid.NewGuid().ToString();

			application.ApplicationId = applicationId;

			_applications.Add(application);

			return applicationId;
		}

		public void UpdateApplication(ApplicationStorageView updatedApplication)
		{
			if (updatedApplication == null)
			{
				return;
			}

			ArrayList newApplicationList = new ArrayList();

			foreach(ApplicationStorageView application in _applications)
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

			_applications = newApplicationList;
		}

		public ApplicationStorageView[] GetApplications()
		{
			return GetApplications(false);
		}

		public ApplicationStorageView[] GetApplications(bool populateThreadCount)
		{
			ArrayList applicationList = new ArrayList();

			foreach(ApplicationStorageView application in _applications)
			{
				if (populateThreadCount)
				{
					int totalThreads;
					int unfinishedThreads;

					GetApplicationThreadCount(application.ApplicationId, out totalThreads, out unfinishedThreads);

					application.TotalThreads = totalThreads;
					application.UnfinishedThreads = unfinishedThreads;
				}

				applicationList.Add(application);
			}

			return (ApplicationStorageView[])applicationList.ToArray(typeof(ApplicationStorageView));
		}

		public ApplicationStorageView[] GetApplications(string userName, bool populateThreadCount)
		{
			ArrayList applicationList = new ArrayList();

			foreach(ApplicationStorageView application in _applications)
			{
				if (String.Compare(application.Username, userName, false) == 0)
				{
					if (populateThreadCount)
					{
						int totalThreads;
						int unfinishedThreads;

						GetApplicationThreadCount(application.ApplicationId, out totalThreads, out unfinishedThreads);

						application.TotalThreads = totalThreads;
						application.UnfinishedThreads = unfinishedThreads;
					}

					applicationList.Add(application);
				}
			}

			return (ApplicationStorageView[])applicationList.ToArray(typeof(ApplicationStorageView));
		}

		public ApplicationStorageView GetApplication(string applicationId)
		{
			IEnumerator enumerator = _applications.GetEnumerator();

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

        public void DeleteApplication( ApplicationStorageView applicationToDelete )
        {
            if( applicationToDelete == null )
            {
                return;
            }

            ArrayList remainingApplications = new ArrayList();
            ArrayList remainingThreads = new ArrayList();

            foreach( ThreadStorageView thread in _threads )
            {
                if( thread.ApplicationId != applicationToDelete.ApplicationId )
                {
                    remainingThreads.Add( thread );
                }
            }

            foreach( ApplicationStorageView application in _applications )
            {
                if( application.ApplicationId != applicationToDelete.ApplicationId )
                {
                    remainingApplications.Add( application );
                }
            }

            _threads = remainingThreads;
            _applications = remainingApplications;
        }
        
		public int AddThread(ThreadStorageView thread)
		{
			if (thread == null)
			{
				return -1;
			}

			lock(_threads)
			{
				// generate the next threadID from the length, this will make sure the thread ID is unique
				// generating from the length also requires thread synchronization code here
				thread.InternalThreadId = _threads.Count;

				_threads.Add(thread);
			}

			return thread.InternalThreadId;
		}

		public void UpdateThread(ThreadStorageView updatedThread)
		{
			if (updatedThread == null)
			{
				return;
			}

			ArrayList newThreadList = new ArrayList();

			foreach(ThreadStorageView thread in _threads)
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

			_threads = newThreadList;
		}

		public ThreadStorageView GetThread(string applicationId, int threadId)
		{
			foreach(ThreadStorageView thread in _threads)
			{
				if (thread.ApplicationId == applicationId && thread.ThreadId == threadId)
				{
					return thread;
				}
			}

			return null;
		}

		public ThreadStorageView[] GetThreads(ApplicationState appState, params ThreadState[] threadStates)
		{
			ArrayList threadList = new ArrayList();

			foreach(ApplicationStorageView application in _applications)
			{
				if (application.State == appState)
				{
					foreach (ThreadStorageView thread in GetThreads(application.ApplicationId, threadStates))
					{
						threadList.Add(thread);
					}
				}
			}

			return (ThreadStorageView[])threadList.ToArray(typeof(ThreadStorageView));
		}

		public ThreadStorageView[] GetThreads(params ThreadState[] state)
		{
			return GetThreads(null, state);
		}

		public ThreadStorageView[] GetThreads(string applicationId, params ThreadState[] threadStates)
		{
			ArrayList threadList = new ArrayList();

			foreach(ThreadStorageView thread in _threads)
			{
				if (thread.ApplicationId == applicationId || applicationId == null)
				{
					bool threadStateCorrect = false;

					if (threadStates == null || threadStates.Length == 0)
					{
						threadStateCorrect = true;
					}
					else
					{
						foreach(ThreadState state in threadStates)
						{
							if (state == thread.State)
							{
								threadStateCorrect = true;
								break;
							}
						}
					}

					if (threadStateCorrect)
					{
						threadList.Add(thread);
					}
				}
			}

			return (ThreadStorageView[])threadList.ToArray(typeof(ThreadStorageView));
		}

		public ThreadStorageView[] GetExecutorThreads(string executorId, params ThreadState[] threadStates)
		{
			ArrayList threadList = new ArrayList();

			foreach(ThreadStorageView thread in _threads)
			{
				if (thread.ExecutorId == executorId)
				{
					bool threadStateCorrect = false;

					if (threadStates == null || threadStates.Length == 0)
					{
						threadStateCorrect = true;
					}
					else
					{
						foreach(ThreadState state in threadStates)
						{
							if (state == thread.State)
							{
								threadStateCorrect = true;
								break;
							}
						}
					}

					if (threadStateCorrect)
					{
						threadList.Add(thread);
					}
				}
			}

			return (ThreadStorageView[])threadList.ToArray(typeof(ThreadStorageView));
		}

		public ThreadStorageView[] GetExecutorThreads(bool dedicatedExecutor, params ThreadState[] state)
		{
			ArrayList threadList = new ArrayList();

			foreach(ExecutorStorageView executor in _executors)
			{
				if (executor.Dedicated == dedicatedExecutor)
				{
					threadList.AddRange(GetExecutorThreads(executor.ExecutorId, state));
				}
			}

			return (ThreadStorageView[])threadList.ToArray(typeof(ThreadStorageView));
		}

		public ThreadStorageView[] GetExecutorThreads(bool dedicatedExecutor, bool connectedExecutor, params ThreadState[] state)
		{
            ArrayList threadList = new ArrayList();

			foreach(ExecutorStorageView executor in _executors)
			{
				if (executor.Dedicated == dedicatedExecutor && executor.Connected == connectedExecutor)
				{
					threadList.AddRange(GetExecutorThreads(executor.ExecutorId, state));
				}
			}

			return (ThreadStorageView[])threadList.ToArray(typeof(ThreadStorageView));
		}
        
		public void GetApplicationThreadCount(string applicationId, out int totalThreads, out int unfinishedThreads)
		{
			totalThreads = unfinishedThreads = 0;

			foreach(ThreadStorageView thread in _threads)
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

		public int GetApplicationThreadCount(string applicationId, ThreadState threadState)
		{
			int threadCount = 0;

			foreach(ThreadStorageView thread in _threads)
			{
				if (thread.ApplicationId == applicationId && thread.State == threadState)
				{
					threadCount ++;
				}
			}

			return threadCount;
		}

		public int GetExecutorThreadCount(string executorId, params ThreadState[] threadState)
		{
			int threadCount = 0;

			if (threadState == null && threadState.Length == 0)
			{
				return threadCount;
			}

			foreach(ThreadStorageView thread in _threads)
			{
				if (thread.ExecutorId != null && thread.ExecutorId == executorId)
				{
					foreach(ThreadState state in threadState)
					{
						if (thread.State == state)
						{
							threadCount ++;
							break; // no point in continuing since there is only one state for a thread
						}
					}
				}
			}

			return threadCount;
		}

		public void DeleteThread(ThreadStorageView threadToDelete)
		{
			if (threadToDelete == null)
			{
				return;
			}

			ArrayList remainingThreads = new ArrayList();

			foreach (ThreadStorageView thread in _threads)
			{
				if (thread.ApplicationId != threadToDelete.ApplicationId || thread.ThreadId != threadToDelete.ThreadId)
				{
					remainingThreads.Add(thread);
				}
			}

			_threads = remainingThreads;
		}


		#endregion

		#region "XML storage persistence implementation - incomplete"
		/// THIS FUNCTIONALITY IS NOT FULLY IMPLEMENTED AND IT MIGHT BE DISCARDED ALTOGETHER
		/// 
		/// Loading from an XML file is the perfect tool for complex storage setups which would be useful for more in-depth unit testing
		/// Saving to an XML file could be used to dump the current storage state for troubleshooting, for example to receive faulty storages from the field.


		/// <summary>
		/// Save the current storage state into an XML file.
		/// It is important that the file format is easily editable by humans so test cases can easily be maintained.
		/// For this reason we do not use the build-in persistence modules.
		/// </summary>
		/// <param name="filename"></param>
		public void SaveToHumanReadableXmlFile(string filename)
		{
			const string storageDocumentTemplate = "<storage><users/><groups/><group_permissions/><executors/><applications/><threads/></storage>";
			XmlDocument storageDocument = new XmlDocument();

			storageDocument.LoadXml(storageDocumentTemplate);

			XmlNode usersNode = storageDocument.SelectSingleNode("/storage/users");
			XmlNode groupsNode = storageDocument.SelectSingleNode("/storage/groups");
			//XmlNode groupPermissionsNode = storageDocument.SelectSingleNode("/storage/group_permissions");
			XmlNode executorsNode = storageDocument.SelectSingleNode("/storage/executors");
			XmlNode applicationsNode = storageDocument.SelectSingleNode("/storage/applications");
			XmlNode threadsNode = storageDocument.SelectSingleNode("/storage/threads");

			if (_users != null)
			{
				IEnumerator usersEnumerator = _users.GetEnumerator();

				while(usersEnumerator.MoveNext())
				{
					UserStorageView user = usersEnumerator.Current as UserStorageView;

					XmlElement userElement = storageDocument.CreateElement("user");

					userElement.SetAttribute("username", user.Username);
					userElement.SetAttribute("password", user.Password);
					userElement.SetAttribute("groupid", user.GroupId.ToString());

					usersNode.AppendChild(userElement);
				}
			}

			if (_groups != null)
			{
				IEnumerator groupsEnumerator = _groups.GetEnumerator();

				while(groupsEnumerator.MoveNext())
				{
					GroupStorageView group = groupsEnumerator.Current as GroupStorageView;

					XmlElement groupElement = storageDocument.CreateElement("group");

					groupElement.SetAttribute("groupname", group.GroupName);
					groupElement.SetAttribute("groupid", group.GroupId.ToString());

					groupsNode.AppendChild(groupElement);
				}
			}

			//		private Hashtable m_groupPermissions;
//			if (m_groupPermissions != null)
//			{
//				IEnumerator groupPermissionsEnumerator = m_groupPermissions.GetEnumerator();
//
//				while(groupPermissionsEnumerator.MoveNext())
//				{
//					GroupPermissionStorageView group = groupPermissionsEnumerator.Current as GroupStorageView;
//
//					XmlElement groupElement = storageDocument.CreateElement("group");
//
//					groupElement.SetAttribute("groupname", group.GroupName);
//					groupElement.SetAttribute("groupid", group.GroupId.ToString());
//
//					groupsNode.AppendChild(groupElement);
//				}
//			}


			if (_executors != null)
			{
				IEnumerator executorsEnumerator = _executors.GetEnumerator();

				while(executorsEnumerator.MoveNext())
				{
					ExecutorStorageView executor = executorsEnumerator.Current as ExecutorStorageView;

					XmlElement executorElement = storageDocument.CreateElement("executor");

					executorElement.SetAttribute("executorid", executor.ExecutorId);
					executorElement.SetAttribute("dedicated", executor.Dedicated.ToString());
					executorElement.SetAttribute("connected", executor.Connected.ToString());
					executorElement.SetAttribute("pingtime", executor.PingTime.ToString());
					executorElement.SetAttribute("hostname", executor.HostName);
					executorElement.SetAttribute("port", executor.Port.ToString());
					executorElement.SetAttribute("username", executor.Username);
					executorElement.SetAttribute("maxcpu", executor.MaxCpu.ToString());
					executorElement.SetAttribute("cpuusage", executor.CpuUsage.ToString());
					executorElement.SetAttribute("availablecpu", executor.AvailableCpu.ToString());
					executorElement.SetAttribute("totalcpuusage", executor.TotalCpuUsage.ToString());
					executorElement.SetAttribute("maxmemory", executor.MaxMemory.ToString());
					executorElement.SetAttribute("maxdisk", executor.MaxDisk.ToString());
					executorElement.SetAttribute("numberofcpu", executor.NumberOfCpu.ToString());
					executorElement.SetAttribute("os", executor.OS);
					executorElement.SetAttribute("architecture", executor.Architecture);

					executorsNode.AppendChild(executorElement);
				}
			}

			if (_applications != null)
			{
				IEnumerator applicationsEnumerator = _applications.GetEnumerator();

				while(applicationsEnumerator.MoveNext())
				{
					ApplicationStorageView application = applicationsEnumerator.Current as ApplicationStorageView;

					XmlElement applicationElement = storageDocument.CreateElement("application");

					applicationElement.SetAttribute("applicationid", application.ApplicationId);
					applicationElement.SetAttribute("state", application.State.ToString());
					applicationElement.SetAttribute("timecreated", application.TimeCreated.ToString());
					applicationElement.SetAttribute("primary", application.Primary.ToString());
					applicationElement.SetAttribute("username", application.Username.ToString());
					applicationElement.SetAttribute("totalthreads", application.TotalThreads.ToString());
					applicationElement.SetAttribute("unfinishedthreads", application.UnfinishedThreads.ToString());

					applicationsNode.AppendChild(applicationElement);
				}
			}

			if (_threads != null)
			{
				IEnumerator threadsEnumerator = _threads.GetEnumerator();

				while(threadsEnumerator.MoveNext())
				{
					ThreadStorageView thread = threadsEnumerator.Current as ThreadStorageView;

					XmlElement threadElement = storageDocument.CreateElement("thread");

					threadElement.SetAttribute("internalthreadid", thread.InternalThreadId.ToString());
					threadElement.SetAttribute("applicationid", thread.ApplicationId);
					threadElement.SetAttribute("executorid", thread.ExecutorId);
					threadElement.SetAttribute("threadid", thread.ThreadId.ToString());
					threadElement.SetAttribute("state", thread.State.ToString());
					threadElement.SetAttribute("timestarted", thread.TimeStarted.ToString());
					threadElement.SetAttribute("timefinished", thread.TimeFinished.ToString());
					threadElement.SetAttribute("priority", thread.Priority.ToString());
					threadElement.SetAttribute("failed", thread.Failed.ToString());

					threadsNode.AppendChild(threadElement);
				}
			}

			storageDocument.Save(filename);
		}

		/// <summary>
		/// Load the storage information from an XML file.
		/// </summary>
		/// <param name="filename"></param>
		public void LoadFromHumanReadableXmlFile(string filename)
		{
			
		}
		#endregion

		#region IManagerStorageSetup Members

		public void TearDownStorage()
		{
		}

		public void CreateStorage(string databaseName)
		{
		}

		public void InitializeStorageData()
		{
			CreateDefaultObjects(this);
		}

		public void SetUpStorage()
		{
		}

		#endregion
	}
}
