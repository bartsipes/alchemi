//
// Alchemi.Core.Manager.Storage.InMemoryManagerStorage.cs
//
// Author:
//   Tibor Biro (tb@tbiro.com)
//
// Copyright (C) 2005 Tibor Biro (tb@tbiro.com)
//
// This program is free software; you can redistribute it and/or modify 
// it under the terms of the GNU General Public License as published by 
// the Free Software Foundation; either version 2 of the License, or 
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of 
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the 
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License 
// along with this program; if not, write to the Free Software 
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

using System;
using System.Collections;

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
		private ArrayList m_executors;

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

		public UserStorageView[] GetUserList()
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


		#endregion
	}
}
