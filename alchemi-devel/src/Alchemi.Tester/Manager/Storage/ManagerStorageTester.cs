//
// Alchemi.Tester.Manager.Storage.ManagerStorageTester.cs
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

using Alchemi.Core;
using Alchemi.Core.Manager.Storage;

using NUnit.Framework;

namespace Alchemi.Tester.Manager.Storage
{
	/// <summary>
	/// Test ManagerStorage features. 
	/// While this is not a TestFixture all classes that inherit it will automatically get the tests
	/// defined here. So we can easily define storage tests, and test all storage providers against them
	/// to make sure there is consistent behavior.
	/// </summary>
	public abstract class ManagerStorageTester
	{
		public abstract IManagerStorage ManagerStorage
		{
			get;
		}

		private void AddUser(String username, String password)
		{
			AddUser(username, password, 0);
		}

		private void AddUser(String username, String password, Int32 groupId)
		{
			UserStorageView[] users = new UserStorageView[1];

			users[0] = new UserStorageView(username, password, groupId);

			ManagerStorage.AddUsers(users);
		}

		private void AddGroup(Int32 groupId, String groupName)
		{
			GroupStorageView[] groups = new GroupStorageView[1];

			groups[0] = new GroupStorageView(groupId, groupName);

			ManagerStorage.AddGroups(groups);
		}

		private String AddExecutor(
			bool dedicated,
			bool connected,
			DateTime pingTime,
			String hostname,
			Int32 port,
			String username,
			Int32 maxCpu,
			Int32 cpuUsage,
			Int32 availableCpu,
			float totalCpuUsage
		)
		{
			ExecutorStorageView executor = new ExecutorStorageView(
						dedicated,
						connected,
						pingTime,
						hostname,
						port,
						username,
						maxCpu,
						cpuUsage,
						availableCpu,
						totalCpuUsage
					);

			return ManagerStorage.AddExecutor(executor);
		}

		private String AddApplication(
			Int32 state,
			DateTime timeCreated,
			bool primary,
			String username
			)
		{
			ApplicationStorageView application = new ApplicationStorageView(
				state,
				timeCreated,
				primary,
				username
				);

			return ManagerStorage.AddApplication(application);
		}

		private void AddThread(
			String applicationId,
			String executorId,
			Int32 threadId,
			Int32 state,
			DateTime timeStarted,
			DateTime timeFinished,
			Int32 priority,
			bool failed
			)
		{
			ThreadStorageView thread = new ThreadStorageView(
				applicationId,
				executorId,
				threadId,
				state,
				timeStarted,
				timeFinished,
				priority,
				failed
				);

			ManagerStorage.AddThread(thread);
		}


		#region "AddUsers Tests"

		/// <summary>
		/// Add a new user.
		/// Make sure there are no errors.
		/// </summary>
		[Test]
		public void AddUsersTest1()
		{
			AddUser("username", "password");
		}

		/// <summary>
		/// Add a null array.
		/// No errors are expected.
		/// </summary>
		[Test]
		public void AddUsersTest2()
		{
			ManagerStorage.AddUsers(null);
		}

		#endregion

		#region "UpdateUsers Tests"
		
		/// <summary>
		/// Add a new user.
		/// Update the user's password and group.
		/// The updates should stick.
		/// </summary>
		[Test]
		public void UpdateUsersTest1()
		{
			AddUser("username1", "password1", 0);

			UserStorageView[] userUpdates = new UserStorageView[1];

			userUpdates[0] = new UserStorageView("username1", "password2", 1);

			ManagerStorage.UpdateUsers(userUpdates);

			UserStorageView[] users = ManagerStorage.GetUserList();
			
			Assert.AreEqual(1, users.Length);
			Assert.AreEqual("username1", users[0].Username);
			Assert.AreEqual("password2", users[0].Password);
			Assert.AreEqual(1, users[0].GroupId);
		}

		/// <summary>
		/// Add no user
		/// Update the user's password and group.
		/// The user list should be empty, no errors are expected.
		/// </summary>
		[Test]
		public void UpdateUsersTest2()
		{
			UserStorageView[] userUpdates = new UserStorageView[1];

			userUpdates[0] = new UserStorageView("username1", "password2", 1);

			ManagerStorage.UpdateUsers(userUpdates);

			UserStorageView[] users = ManagerStorage.GetUserList();
			
			Assert.AreEqual(0, users.Length);
		}

		/// <summary>
		/// Add a new user
		/// Set the update array to null;
		/// The user list should not be modified, no errors are expected.
		/// </summary>
		[Test]
		public void UpdateUsersTest3()
		{
			AddUser("username1", "password1", 0);

			ManagerStorage.UpdateUsers(null);

			UserStorageView[] users = ManagerStorage.GetUserList();
			
			Assert.AreEqual(1, users.Length);
			Assert.AreEqual("username1", users[0].Username);
			Assert.AreEqual("password1", users[0].Password);
			Assert.AreEqual(0, users[0].GroupId);
		}

		#endregion

		#region "AuthenticateUser Tests"
		
		/// <summary>
		/// Add a new user.
		/// Verify user credentials with valid user.
		/// Should authenticate.
		/// </summary>
		[Test]
		public void AuthenticateUserTest1()
		{
			AddUser("username1", "password1");
			SecurityCredentials sc = new SecurityCredentials("username1", "password1");

			bool result = ManagerStorage.AuthenticateUser(sc);

			Assert.IsTrue(result);
		}

		/// <summary>
		/// Add a new user.
		/// Verify user credentials with invalid user.
		/// Should not authenticate.
		/// </summary>
		[Test]
		public void AuthenticateUserTest2()
		{
			AddUser("username1", "password1");
			SecurityCredentials sc = new SecurityCredentials("username2", "password2");

			bool result = ManagerStorage.AuthenticateUser(sc);

			Assert.IsFalse(result);
		}
	
		/// <summary>
		/// Do not add any user. This will leave the user array empty.
		/// Verify user credentials with valid user.
		/// Should not authenticate.
		/// </summary>
		[Test]
		public void AuthenticateUserTest3()
		{
			SecurityCredentials sc = new SecurityCredentials("username1", "password1");

			bool result = ManagerStorage.AuthenticateUser(sc);

			Assert.IsFalse(result);
		}

		/// <summary>
		/// Add a new user.
		/// Verify user credentials with null values.
		/// Should not authenticate, no errors are expected.
		/// </summary>
		[Test]
		public void AuthenticateUserTest4()
		{
			AddUser("username1", "password1");
			SecurityCredentials sc = new SecurityCredentials(null, null);

			bool result = ManagerStorage.AuthenticateUser(sc);

			Assert.IsFalse(result);
		}
	
		/// <summary>
		/// Add a new user.
		/// Verify user credentials with null credential object.
		/// Should not authenticate, no errors are expected.
		/// </summary>
		[Test]
		public void AuthenticateUserTest5()
		{
			AddUser("username1", "password1");

			bool result = ManagerStorage.AuthenticateUser(null);

			Assert.IsFalse(result);
		}
	
		#endregion

		#region "GetUserList Tests"

		/// <summary>
		/// Add a new user.
		/// Get the users list.
		/// The list should only contain the newly added user.
		/// </summary>
		[Test]
		public void GetUserListTest1()
		{
			AddUser("username1", "password1");
			
			UserStorageView[] users;

			users = ManagerStorage.GetUserList();

			Assert.AreEqual(1, users.Length);
			Assert.AreEqual("username1", users[0].Username);
			Assert.AreEqual("password1", users[0].Password);

		}

		/// <summary>
		/// Add 3 users.
		/// Get the users list.
		/// The list should contain 3 items.
		/// </summary>
		[Test]
		public void GetUserListTest2()
		{
			AddUser("username1", "password1");
			AddUser("username2", "password2");
			AddUser("username3", "password3");
			
			UserStorageView[] users;

			users = ManagerStorage.GetUserList();

			Assert.AreEqual(3, users.Length);
		}

		/// <summary>
		/// Add no users.
		/// Get the users list.
		/// The list should be empty but not null.
		/// </summary>
		[Test]
		public void GetUserListTest3()
		{
			UserStorageView[] users;

			users = ManagerStorage.GetUserList();

			Assert.AreEqual(0, users.Length);
		}


		#endregion
	
		#region "AddGroups Tests"

		/// <summary>
		/// Add a new group.
		/// No errors are expected.
		/// </summary>
		[Test]
		public void AddGroupsTest1()
		{
			AddGroup(0, "group0");
		}

		/// <summary>
		/// Add a null array.
		/// No errors are expected.
		/// </summary>
		[Test]
		public void AddGroupsTest2()
		{
			ManagerStorage.AddGroups(null);
		}

		#endregion

		#region "GetGroups Tests"

		/// <summary>
		/// Add a new group.
		/// Get the group list.
		/// The list should only contain the newly added group.
		/// </summary>
		[Test]
		public void GetGroupsTest1()
		{
			AddGroup(0, "group0");
			
			GroupStorageView[] groups = ManagerStorage.GetGroups();

			Assert.AreEqual(1, groups.Length);
			Assert.AreEqual(0, groups[0].GroupId);
			Assert.AreEqual("group0", groups[0].GroupName);

		}

		/// <summary>
		/// Add 3 groups.
		/// Get the group list.
		/// The list should contain 3 items.
		/// </summary>
		[Test]
		public void GetGroupsTest2()
		{
			AddGroup(0, "group0");
			AddGroup(1, "group1");
			AddGroup(2, "group2");
			
			GroupStorageView[] groups = ManagerStorage.GetGroups();

			Assert.AreEqual(3, groups.Length);
		}

		/// <summary>
		/// Add no groups.
		/// Get the group list.
		/// The list should be empty but not null. No error is expected
		/// </summary>
		[Test]
		public void GetGroupsTest3()
		{
			GroupStorageView[] groups = ManagerStorage.GetGroups();

			Assert.AreEqual(0, groups.Length);
		}


		#endregion

		#region "AddApplication Tests"

		/// <summary>
		/// Add a new application.
		/// Make sure we get back an ID. 
		/// No errors are expected.
		/// </summary>
		[Test]
		public void AddApplicationTest1()
		{
			String applicationId = AddApplication(1, DateTime.Now, false, "test");

			Assert.IsNotNull(applicationId);
			Assert.AreNotEqual("", applicationId);
		}

		/// <summary>
		/// Add a null application.
		/// No errors are expected, nothing should be added. Return value should be null.
		/// </summary>
		[Test]
		public void AddApplicationTest2()
		{
			Assert.IsNull(ManagerStorage.AddApplication(null));
		}

		/// <summary>
		/// create an application object only from the username.
		/// Check the inserted data to see if the right defaults were applied:
		/// state = 0
		/// primary = true
		/// timeCreated is the current time
		/// new application id is generated
		/// </summary>
		[Test]
		public void AddApplicationTestWithUsernameOnly()
		{
			ApplicationStorageView application = new ApplicationStorageView("username3");

			String applicationId = ManagerStorage.AddApplication(application);

			Assert.AreEqual(0, application.State);
			Assert.AreEqual(true, application.Primary);			
			Assert.IsTrue(DateTime.Now.AddHours(-1) < application.TimeCreated && application.TimeCreated < DateTime.Now.AddHours(1), "Time created is not in this hour!");
			Assert.IsTrue(applicationId != null && applicationId.Length > 0, "Invalid ApplicationID!");
		}
		
		#endregion

		#region "UpdateApplication Tests"

		/// <summary>
		/// Add a new application.
		/// Change all values, run update
		/// Get applications, confirm that the update worked.
		/// </summary>
		[Test]
		public void UpdateApplicationTest1()
		{
			// TB: due to rounding errors the milliseconds might be lost in the database storage.
			// TB: I think this is OK so we create a test DateTime without milliseconds
			DateTime now = DateTime.Now;
			DateTime timeCreated1 = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, 0);
			now = now.AddDays(1);
			DateTime timeCreated2 = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, 0);

			String applicationId = AddApplication(12, timeCreated1, false, "test");

			ApplicationStorageView updatedApplication = new ApplicationStorageView(10, timeCreated2, true, "test2");

			updatedApplication.ApplicationId = applicationId;

			ManagerStorage.UpdateApplication(updatedApplication);
			
			ApplicationStorageView[] applications = ManagerStorage.GetApplications();

			Assert.AreEqual(1, applications.Length);
			Assert.AreEqual(10, applications[0].State);
			Assert.AreEqual(timeCreated2, applications[0].TimeCreated);
			Assert.AreEqual(true, applications[0].Primary);
			Assert.AreEqual("test2", applications[0].Username);
		}

		/// <summary>
		/// Run update without any values in there.
		/// The application list should stay empty, no errors are expected
		/// </summary>
		[Test]
		public void UpdateApplicationTest2()
		{
			DateTime timeCreated = DateTime.Now.AddDays(1);

			ApplicationStorageView updatedApplication = new ApplicationStorageView(123, timeCreated, false, "username2");

			updatedApplication.ApplicationId = "";

			ManagerStorage.UpdateApplication(updatedApplication);
			
			ApplicationStorageView[] applications = ManagerStorage.GetApplications();

			Assert.AreEqual(0, applications.Length);
		}

		/// <summary>
		/// Add a new application
		/// Run update with a null application.
		/// The application list should stay empty, no errors are expected
		/// </summary>
		[Test]
		public void UpdateApplicationTest3()
		{
			DateTime timeCreated = DateTime.Now;

			String applicationId = AddApplication(123, timeCreated, true, "username1");

			ManagerStorage.UpdateApplication(null);
			
			ApplicationStorageView[] applications = ManagerStorage.GetApplications();

			Assert.AreEqual(1, applications.Length);
		}

		#endregion

		#region "GetApplications Tests"

		/// <summary>
		/// Add a new application.
		/// Get the application list.
		/// The list should only contain the newly added application.
		/// </summary>
		[Test]
		public void GetApplicationsTest1()
		{
			// TB: due to rounding errors the milliseconds might be lost in the database storage.
			// TB: I think this is OK so we create a test DateTime without milliseconds
			DateTime now = DateTime.Now;
			DateTime timeCreated = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, 0);

			String applicationId = AddApplication(123, timeCreated, true, "username2");
			
			ApplicationStorageView[] applications = ManagerStorage.GetApplications();

			Assert.AreEqual(1, applications.Length);
			Assert.AreEqual(123, applications[0].State);
			Assert.AreEqual(timeCreated, applications[0].TimeCreated);
			Assert.AreEqual(true, applications[0].Primary);
			Assert.AreEqual("username2", applications[0].Username);
		}

		/// <summary>
		/// Add 3 applications.
		/// Get the applications list.
		/// The list should contain 3 items.
		/// </summary>
		[Test]
		public void GetApplicationsTest2()
		{
			DateTime timeCreated = DateTime.Now;

			String applicationId1 = AddApplication(123, timeCreated, true, "username1");
			String applicationId2 = AddApplication(123, timeCreated, true, "username2");
			String applicationId3 = AddApplication(123, timeCreated, true, "username3");
			
			ApplicationStorageView[] applications = ManagerStorage.GetApplications();

			Assert.AreEqual(3, applications.Length);
		}

		/// <summary>
		/// Add no applications.
		/// Get the application list.
		/// The list should be empty but not null. No error is expected
		/// </summary>
		[Test]
		public void GetApplicationsTest3()
		{
			ApplicationStorageView[] applications = ManagerStorage.GetApplications();

			Assert.AreEqual(0, applications.Length);
		}

		/// <summary>
		/// Create an application for username1, add 3 threads
		/// Create an application for username2, add 1 thread
		/// Get the username1 applications, it should have 5 threads, of which 3 are unfinished (status 0, 1 or 2).
		/// </summary>
		[Test]
		public void GetApplicationsTestUserApplications()
		{
			String applicationId1 = AddApplication(0, DateTime.Now, true, "username1");
			String executorId = null;

			// Add a few threads to ths application
			AddThread(applicationId1, executorId, 1, 0, DateTime.Now, DateTime.Now, 0, false);
			AddThread(applicationId1, executorId, 2, 1, DateTime.Now, DateTime.Now, 0, false);
			AddThread(applicationId1, executorId, 3, 2, DateTime.Now, DateTime.Now, 0, false);
			AddThread(applicationId1, executorId, 4, 3, DateTime.Now, DateTime.Now, 0, false);
			AddThread(applicationId1, executorId, 5, 4, DateTime.Now, DateTime.Now, 0, false);

			String applicationId2 = AddApplication(0, DateTime.Now, true, "username2");

			AddThread(applicationId2, executorId, 1, 0, DateTime.Now, DateTime.Now, 0, false);

			ApplicationStorageView[] applications = ManagerStorage.GetApplications("username1", true);

			Assert.AreEqual(1, applications.Length);
			Assert.AreEqual(5, applications[0].TotalThreads);
			Assert.AreEqual(3, applications[0].UnfinishedThreads);
		}

		#endregion

		#region "GetApplication Tests"

		/// <summary>
		/// Add a new application.
		/// Get the application by application ID.
		/// We should find the new application
		/// </summary>
		[Test]
		public void GetApplicationTest1()
		{
			// TB: due to rounding errors the milliseconds might be lost in the database storage.
			// TB: I think this is OK so we create a test DateTime without milliseconds
			DateTime now = DateTime.Now;
			DateTime timeCreated = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, 0);

			String applicationId = AddApplication(123, timeCreated, true, "username2");
			
			ApplicationStorageView application = ManagerStorage.GetApplication(applicationId);

			Assert.IsNotNull(application);
			Assert.AreEqual(123, application.State);
			Assert.AreEqual(timeCreated, application.TimeCreated);
			Assert.AreEqual(true, application.Primary);
			Assert.AreEqual("username2", application.Username);
		}

		/// <summary>
		/// Add 3 applications.
		/// Get the second application.
		/// </summary>
		[Test]
		public void GetApplicationTest2()
		{
			// TB: due to rounding errors the milliseconds might be lost in the database storage.
			// TB: I think this is OK so we create a test DateTime without milliseconds
			DateTime now = DateTime.Now;
			DateTime timeCreated = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, 0);

			String applicationId1 = AddApplication(122, timeCreated, false, "username1");
			String applicationId2 = AddApplication(123, timeCreated, true, "username2");
			String applicationId3 = AddApplication(124, timeCreated, false, "username3");
			
			ApplicationStorageView application = ManagerStorage.GetApplication(applicationId2);

			Assert.IsNotNull(application);
			Assert.AreEqual(123, application.State);
			Assert.AreEqual(timeCreated, application.TimeCreated);
			Assert.AreEqual(true, application.Primary);
			Assert.AreEqual("username2", application.Username);
		}

		/// <summary>
		/// Add no applications.
		/// Get an application.
		/// The object should be null
		/// </summary>
		[Test]
		public void GetApplicationTest3()
		{
			ApplicationStorageView application = ManagerStorage.GetApplication(Guid.NewGuid().ToString());

			Assert.IsNull(application);
		}

		/// <summary>
		/// Add application
		/// Get another application (non-existent)
		/// The object should be null
		/// </summary>
		[Test]
		public void GetApplicationTest4()
		{
			// TB: due to rounding errors the milliseconds might be lost in the database storage.
			// TB: I think this is OK so we create a test DateTime without milliseconds
			DateTime now = DateTime.Now;
			DateTime timeCreated = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, 0);

			String applicationId = AddApplication(123, timeCreated, true, "username2");
			
			ApplicationStorageView application = ManagerStorage.GetApplication(Guid.NewGuid().ToString());

			Assert.IsNull(application);
		}


		#endregion

		#region "AddExecutor Tests"

		/// <summary>
		/// Add a new executor.
		/// Make sure we get back an ID. 
		/// No errors are expected.
		/// </summary>
		[Test]
		public void AddExecutorTest1()
		{
			String executorId = AddExecutor(false, false, DateTime.Now, "", 9000, "username1", 0, 0, 0, 0);

			Assert.IsNotNull(executorId);
			Assert.AreNotEqual("", executorId);
		}

		/// <summary>
		/// Add a null executor.
		/// No errors are expected, nothing should be added. Return value should be null.
		/// </summary>
		[Test]
		public void AddExecutorTest2()
		{
			Assert.IsNull(ManagerStorage.AddExecutor(null));
		}
		
		#endregion

		#region "UpdateExecutor Tests"

		/// <summary>
		/// Add a new executor.
		/// Change all values, run update
		/// Get executors, confirm that the update worked.
		/// </summary>
		[Test]
		public void UpdateExecutorTest1()
		{
			// TB: due to rounding errors the milliseconds might be lost in the database storage.
			// TB: I think this is OK so we create a test DateTime without milliseconds
			DateTime now = DateTime.Now;
			DateTime pingTime1 = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, 0);
			now = now.AddDays(1);
			DateTime pingTime2 = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, 0);

			String executorId = AddExecutor(false, true, pingTime1, "test1", 9999, "username1", 111, 123, 34, (float)3.4);

			ExecutorStorageView updatedExecutor = new ExecutorStorageView(true, false, pingTime2, "test2", 8888, "username2", 222, 456, 56, (float)5.6);

			updatedExecutor.ExecutorId = executorId;

			ManagerStorage.UpdateExecutor(updatedExecutor);
			
			ExecutorStorageView[] executors = ManagerStorage.GetExecutors();

			Assert.AreEqual(1, executors.Length);
			Assert.AreEqual(true, executors[0].Dedicated);
			Assert.AreEqual(false, executors[0].Connected);
			Assert.AreEqual(pingTime2, executors[0].PingTime);
			Assert.AreEqual("test2", executors[0].HostName);
			Assert.AreEqual(8888, executors[0].Port);
			Assert.AreEqual("username2", executors[0].Username);
			Assert.AreEqual(222, executors[0].MaxCpu);
			Assert.AreEqual(456, executors[0].CpuUsage);
			Assert.AreEqual(56, executors[0].AvailableCpu);
			Assert.AreEqual(5.6, executors[0].TotalCpuUsage);
			Assert.AreEqual(executorId, executors[0].ExecutorId);
		}

		/// <summary>
		/// Run update without any values in there.
		/// The executor list should stay empty, no errors are expected
		/// </summary>
		[Test]
		public void UpdateExecutorTest2()
		{
			DateTime pingTime2 = DateTime.Now.AddDays(1);

			ExecutorStorageView updatedExecutor = new ExecutorStorageView(true, false, pingTime2, "test2", 8888, "username2", 222, 456, 56, (float)5.6);

			updatedExecutor.ExecutorId = "";

			ManagerStorage.UpdateExecutor(updatedExecutor);
			
			ExecutorStorageView[] executors = ManagerStorage.GetExecutors();

			Assert.AreEqual(0, executors.Length);
		}

		/// <summary>
		/// Add a new executor
		/// Run update with a null executor.
		/// The executor list should stay empty, no errors are expected
		/// </summary>
		[Test]
		public void UpdateExecutorTest3()
		{
			DateTime pingTime1 = DateTime.Now;

			String executorId = AddExecutor(false, true, pingTime1, "test1", 9999, "username1", 111, 123, 34, (float)3.4);

			ManagerStorage.UpdateExecutor(null);
			
			ExecutorStorageView[] executors = ManagerStorage.GetExecutors();

			Assert.AreEqual(1, executors.Length);
		}

		#endregion

		#region "GetExecutors Tests"

		/// <summary>
		/// Add a new executor.
		/// Get the executor list.
		/// The list should only contain the newly added executor.
		/// </summary>
		[Test]
		public void GetExecutorsTest1()
		{
			// TB: due to rounding errors the milliseconds might be lost in the database storage.
			// TB: I think this is OK so we create a test DateTime without milliseconds
			DateTime now = DateTime.Now;
			DateTime pingTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, 0);

			String executorId = AddExecutor(false, true, pingTime, "test", 9999, "username1", 111, 123, 34, (float)3.4);
			
			ExecutorStorageView[] executors = ManagerStorage.GetExecutors();

			Assert.AreEqual(1, executors.Length);
			Assert.AreEqual(false, executors[0].Dedicated);
			Assert.AreEqual(true, executors[0].Connected);
			Assert.AreEqual(pingTime, executors[0].PingTime);
			Assert.AreEqual("test", executors[0].HostName);
			Assert.AreEqual(9999, executors[0].Port);
			Assert.AreEqual("username1", executors[0].Username);
			Assert.AreEqual(111, executors[0].MaxCpu);
			Assert.AreEqual(123, executors[0].CpuUsage);
			Assert.AreEqual(34, executors[0].AvailableCpu);
			Assert.AreEqual(3.4, executors[0].TotalCpuUsage);
			Assert.AreEqual(executorId, executors[0].ExecutorId);
		}

		/// <summary>
		/// Add 3 executors.
		/// Get the executors list.
		/// The list should contain 3 items.
		/// </summary>
		[Test]
		public void GetExecutorsTest2()
		{
			DateTime pingTime = DateTime.Now;

			String executorId1 = AddExecutor(false, true, pingTime, "test1", 9999, "username1", 111, 123, 34, (float)3.4);
			String executorId2 = AddExecutor(false, true, pingTime, "test2", 9999, "username2", 111, 123, 34, (float)3.4);
			String executorId3 = AddExecutor(false, true, pingTime, "test3", 9999, "username3", 111, 123, 34, (float)3.4);
			
			ExecutorStorageView[] executors = ManagerStorage.GetExecutors();

			Assert.AreEqual(3, executors.Length);
		}

		/// <summary>
		/// Add no executors.
		/// Get the executor list.
		/// The list should be empty but not null. No error is expected
		/// </summary>
		[Test]
		public void GetExecutorsTest3()
		{
			ExecutorStorageView[] executors = ManagerStorage.GetExecutors();

			Assert.AreEqual(0, executors.Length);
		}


		#endregion

		#region "AddThread Tests"

		/// <summary>
		/// Add a new thread.
		/// No errors are expected.
		/// </summary>
		[Test]
		public void AddThreadTest1()
		{
			String applicationId = Guid.NewGuid().ToString();
			String executorId = Guid.NewGuid().ToString();

			AddThread(applicationId, executorId, 1, 2, DateTime.Now, DateTime.Now.AddDays(1), 1, false);
		}

		/// <summary>
		/// Add a null thread.
		/// No errors are expected, nothing should be added.
		/// </summary>
		[Test]
		public void AddThreadTest2()
		{
			ManagerStorage.AddThread(null);
		}
		
		/// <summary>
		/// Add a thread with a null Executor ID.
		/// Note: 
		///		Reproducing a bug on the SQL Server implementation.
		/// </summary>
		[Test]
		public void AddThreadTestNullExecutorId()
		{
			String applicationId = Guid.NewGuid().ToString();
			String executorId = null;

			AddThread(applicationId, executorId, 1, 2, DateTime.Now, DateTime.Now.AddDays(1), 1, false);
		}

		#endregion

		#region "UpdateThread Tests"

		/// <summary>
		/// Add a new thread.
		/// Change all values, run update
		/// Get threads, confirm that the update worked.
		/// </summary>
		[Test]
		public void UpdateThreadTest1()
		{
			String applicationId = Guid.NewGuid().ToString();
			String executorId1 = Guid.NewGuid().ToString();
			String executorId2 = Guid.NewGuid().ToString();

			Int32 threadId = 122;

			// TB: due to rounding errors the milliseconds might be lost in the database storage.
			// TB: I think this is OK so we create a test DateTime without milliseconds
			DateTime now = DateTime.Now;
			DateTime timeStarted1 = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, 0);
			now = now.AddDays(1);
			DateTime timeFinished1 = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, 0);

			now = now.AddDays(1);
			DateTime timeStarted2 = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, 0);
			now = now.AddDays(1);
			DateTime timeFinished2 = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, 0);

			AddThread(applicationId, executorId1, threadId, 2, timeStarted1, timeFinished1, 4, true);

			// retrieve the newly added thread so we have the new internal thread id
			ThreadStorageView[] newThreads = ManagerStorage.GetThreads();

			ThreadStorageView updatedThread = new ThreadStorageView(newThreads[0].InternalThreadId, applicationId, executorId2, threadId, 6, timeStarted2, timeFinished2, 7, false);

			ManagerStorage.UpdateThread(updatedThread);
			
			ThreadStorageView[] threads = ManagerStorage.GetThreads();

			Assert.AreEqual(1, threads.Length);
			Assert.AreEqual(applicationId, threads[0].ApplicationId);
			Assert.AreEqual(executorId2, threads[0].ExecutorId);
			Assert.AreEqual(threadId, threads[0].ThreadId);
			Assert.AreEqual(6, threads[0].State);
			Assert.AreEqual(timeStarted2, threads[0].TimeStarted);
			Assert.AreEqual(timeFinished2, threads[0].TimeFinished);
			Assert.AreEqual(7, threads[0].Priority);
			Assert.AreEqual(false, threads[0].Failed);
		}

		/// <summary>
		/// Run update without any values in there.
		/// The thread list should stay empty, no errors are expected
		/// </summary>
		[Test]
		public void UpdateThreadTest2()
		{
			String applicationId = Guid.NewGuid().ToString();
			String executorId = Guid.NewGuid().ToString();
			Int32 threadId = 122;

			DateTime timeCreated = DateTime.Now.AddDays(1);

			ThreadStorageView updatedThread = new ThreadStorageView(applicationId, executorId, threadId, 6, DateTime.Now, DateTime.Now.AddDays(1), 7, false);

			updatedThread.ThreadId = -1;

			ManagerStorage.UpdateThread(updatedThread);
			
			ThreadStorageView[] threads = ManagerStorage.GetThreads();

			Assert.AreEqual(0, threads.Length);
		}

		/// <summary>
		/// Add a new thread
		/// Run update with a null thread.
		/// The thread list should stay empty, no errors are expected
		/// </summary>
		[Test]
		public void UpdateThreadTest3()
		{
			String applicationId = Guid.NewGuid().ToString();
			String executorId = Guid.NewGuid().ToString();
			Int32 threadId = 122;
			DateTime timeStarted = DateTime.Now;
			DateTime timeFinished = DateTime.Now.AddDays(1);

			AddThread(applicationId, executorId, threadId, 2, timeStarted, timeFinished, 1, false);

			ManagerStorage.UpdateThread(null);
			
			ThreadStorageView[] threads = ManagerStorage.GetThreads();

			Assert.AreEqual(1, threads.Length);
		}

		#endregion

		#region "GetThreads Tests"

		/// <summary>
		/// Add a new thread.
		/// Get the thread list.
		/// The list should only contain the newly added thread.
		/// </summary>
		[Test]
		public void GetThreadsTest1()
		{
			String applicationId = Guid.NewGuid().ToString();
			String executorId = Guid.NewGuid().ToString();
			Int32 threadId = 125;

			// TB: due to rounding errors the milliseconds might be lost in the database storage.
			// TB: I think this is OK so we create a test DateTime without milliseconds
			DateTime now = DateTime.Now;
			DateTime timeStarted = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, 0);
			now.AddDays(1);
			DateTime timeFinished = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, 0);

			AddThread(applicationId, executorId, threadId, 2, timeStarted, timeFinished, 1, true);
			
			ThreadStorageView[] threads = ManagerStorage.GetThreads();

			Assert.AreEqual(1, threads.Length);
			Assert.AreEqual(applicationId, threads[0].ApplicationId);
			Assert.AreEqual(executorId, threads[0].ExecutorId);
			Assert.AreEqual(threadId, threads[0].ThreadId);
			Assert.AreEqual(2, threads[0].State);
			Assert.AreEqual(timeStarted, threads[0].TimeStarted);
			Assert.AreEqual(timeFinished, threads[0].TimeFinished);
			Assert.AreEqual(1, threads[0].Priority);
			Assert.AreEqual(true, threads[0].Failed);
		}

		/// <summary>
		/// Add 3 threads.
		/// Get the threads list.
		/// The list should contain 3 items.
		/// </summary>
		[Test]
		public void GetThreadsTest2()
		{
			String applicationId = Guid.NewGuid().ToString();
			String executorId = Guid.NewGuid().ToString();

			AddThread(applicationId, executorId, 1, 6, DateTime.Now, DateTime.Now.AddDays(1), 7, false);
			AddThread(applicationId, executorId, 2, 6, DateTime.Now, DateTime.Now.AddDays(1), 7, false);
			AddThread(applicationId, executorId, 3, 6, DateTime.Now, DateTime.Now.AddDays(1), 7, false);
			
			ThreadStorageView[] threads = ManagerStorage.GetThreads();

			Assert.AreEqual(3, threads.Length);
		}

		/// <summary>
		/// Add no threads.
		/// Get the thread list.
		/// The list should be empty but not null. No error is expected
		/// </summary>
		[Test]
		public void GetThreadsTest3()
		{
			ThreadStorageView[] threads = ManagerStorage.GetThreads();

			Assert.AreEqual(0, threads.Length);
		}


		#endregion

		#region "GetApplicationThreadInformation Tests"

		/// <summary>
		/// Add no application or thread
		/// Attempt to get the threads, it should return 0
		/// </summary>
		[Test]
		public void GetApplicationThreadCountTestNoThreadInformation()
		{
			String applicationId = Guid.NewGuid().ToString();
			Int32 totalThreads;
			Int32 unfinishedThreads;

			ManagerStorage.GetApplicationThreadCount(applicationId, out totalThreads, out unfinishedThreads);

			Assert.AreEqual(0, totalThreads);
			Assert.AreEqual(0, unfinishedThreads);
		}

		/// <summary>
		/// Add a few threads for an application
		/// Attempt to get the threads, the numbers should be good
		/// </summary>
		[Test]
		public void GetApplicationThreadCountTestSimpleScenario()
		{
			String applicationId = Guid.NewGuid().ToString();
			Int32 totalThreads;
			Int32 unfinishedThreads;

			// add 4 threads, 3 are unfinished
			AddThread(applicationId, null, 1, 0, DateTime.Now, DateTime.Now, 0, false);
			AddThread(applicationId, null, 2, 1, DateTime.Now, DateTime.Now, 0, false);
			AddThread(applicationId, null, 3, 2, DateTime.Now, DateTime.Now, 0, false);
			AddThread(applicationId, null, 4, 3, DateTime.Now, DateTime.Now, 0, false);

			ManagerStorage.GetApplicationThreadCount(applicationId, out totalThreads, out unfinishedThreads);

			Assert.AreEqual(4, totalThreads);
			Assert.AreEqual(3, unfinishedThreads);
		}

		#endregion
	}
}
