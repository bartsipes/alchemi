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
			User[] users = new User[1];

			users[0] = new User(username, password, groupId);

			ManagerStorage.AddUsers(users);
		}

		private void AddGroup(Int32 groupId, String groupName)
		{
			Group[] groups = new Group[1];

			groups[0] = new Group(groupId, groupName);

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
			Executor executor = new Executor(
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

			User[] userUpdates = new User[1];

			userUpdates[0] = new User("username1", "password2", 1);

			ManagerStorage.UpdateUsers(userUpdates);

			User[] users = ManagerStorage.GetUserList();
			
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
			User[] userUpdates = new User[1];

			userUpdates[0] = new User("username1", "password2", 1);

			ManagerStorage.UpdateUsers(userUpdates);

			User[] users = ManagerStorage.GetUserList();
			
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

			User[] users = ManagerStorage.GetUserList();
			
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
			
			User[] users;

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
			
			User[] users;

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
			User[] users;

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
			
			Group[] groups = ManagerStorage.GetGroups();

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
			
			Group[] groups = ManagerStorage.GetGroups();

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
			Group[] groups = ManagerStorage.GetGroups();

			Assert.AreEqual(0, groups.Length);
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

	}
}
