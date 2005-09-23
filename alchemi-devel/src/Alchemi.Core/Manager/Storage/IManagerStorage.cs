//
// Alchemi.Core.Manager.Storage.IManagerStorage.cs
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
		/*
		object GetLiveApplicationList(SecurityCredentials sc);

		object GetUserApplicationList(SecurityCredentials sc);

		object GetThreadList(SecurityCredentials sc, string appId);

		object GetThreadList(SecurityCredentials sc, string appId, ThreadState status);

		object GetExecutors(SecurityCredentials sc);

		/// <summary>
		/// Check if a permisison is set.
		/// </summary>
		/// <param name="sc">Security credentials to use in the check.</param>
		/// <param name="perm">Permission to check for</param>
		/// <returns>true if the permission is set, false otherwise</returns>
		bool CheckPermission(SecurityCredentials sc, Permission perm);

		*/

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
		void AddUsers(User[] users);

		void UpdateUsers(User[] updates);

		/// <summary>
		/// Get an array with all the users found in the current storage
		/// </summary>
		/// <returns></returns>
		User[] GetUserList();

		void AddGroups(Group[] groups);
		
		Group[] GetGroups();

		/// <summary>
		/// Get system summary information from the storage.
		/// </summary>
		/// <returns>
		/// An object with the summary information or null if the storage does not implement system summary.
		/// </returns>
		SystemSummary GetSystemSummary();

	}
}
