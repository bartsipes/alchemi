//
// Alchemi.Core.Manager.Storage.IManagerStorage.cs
//
// Author:
//   Tibor Biro (tb@tbiro.com)
//
// Copyright (C) Tibor Biro, 2005
//

//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

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

		object GetUserList(SecurityCredentials sc);

		object GetGroups(SecurityCredentials sc);

		object UpdateUsers(SecurityCredentials sc, object updates);

		object AddUsers(SecurityCredentials sc, object users);


		object GetExecutors(SecurityCredentials sc);

		/// <summary>
		/// Check if a permisison is set.
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
		*/

		/// <summary>
		/// Get system summary information from the storage.
		/// </summary>
		/// <returns>
		/// An object with the summary information or null if the storage does not implement system summary.
		/// </returns>
		SystemSummary GetSystemSummary();

	}
}
