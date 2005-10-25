using System;

using NUnit.Framework;

using Alchemi.Core;
using Alchemi.Core.Manager;
using Alchemi.Core.Manager.Storage;

namespace Alchemi.Tester.Manager
{
	/// <summary>
	/// Testing GManager functionality
	/// These tests use the InMemoryManagerStorage storage, all other storages should 
	/// perform identically as enforced by the storage level tests.
	/// </summary>
	[TestFixture]
	public class GManagerTester : GManager
	{
		private InMemoryManagerStorage m_managerStorage;

		[SetUp]
		public void SetUp()
		{
			m_managerStorage = new InMemoryManagerStorage();
			ManagerStorageFactory.SetManagerStorage(m_managerStorage);
		}

		[TearDown]
		public void TearDown()
		{
			m_managerStorage = null;
			ManagerStorageFactory.SetManagerStorage(null);
		}

		public GManagerTester()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Add an application
		/// test for real creator.
		/// Should return true.
		/// </summary>
		[Test]
		public void IsApplicationCreatorTestRealCreator()
		{
			ApplicationStorageView application = new ApplicationStorageView("username1");

			String applicationId = m_managerStorage.AddApplication(application);

			SecurityCredentials sc = new SecurityCredentials("username1", "password1");
			
			bool result = IsApplicationCreator(sc, applicationId);
			
			Assert.IsTrue(result);
		}

		/// <summary>
		/// Add an application
		/// test for false creator.
		/// Should return false.
		/// </summary>
		[Test]
		public void IsApplicationCreatorTestFalseCreator()
		{
			ApplicationStorageView application = new ApplicationStorageView("username1");

			String applicationId = m_managerStorage.AddApplication(application);

			SecurityCredentials sc = new SecurityCredentials("username2", "password1");
			
			bool result = IsApplicationCreator(sc, applicationId);
			
			Assert.IsFalse(result);
		}

		/// <summary>
		/// Add an application
		/// test for creator for invalid application.
		/// Should return true.
		/// </summary>
		[Test]
		public void IsApplicationCreatorTestInvalidApplication()
		{
			ApplicationStorageView application = new ApplicationStorageView("username1");

			String applicationId = m_managerStorage.AddApplication(application);
			String invalidApplicationId = Guid.NewGuid().ToString();

			SecurityCredentials sc = new SecurityCredentials("username1", "password1");
			
			bool result = IsApplicationCreator(sc, invalidApplicationId);
			
			Assert.IsFalse(result);

		}

		/// <summary>
		/// Add a group, a user and a permissions, make sure the permission check passes.
		/// </summary>
		[Test]
		public void EnsurePermissionTestSimpleScenario()
		{
			Int32 groupId = 12;

			GroupStorageView[] groups = new GroupStorageView[1];

			groups[0] = new GroupStorageView(groupId, "test1");

			UserStorageView[] users = new UserStorageView[1];

			users[0] = new UserStorageView("username1", "password1", groupId);

			m_managerStorage.AddGroups(groups);

			m_managerStorage.AddUsers(users);

			m_managerStorage.AddGroupPermission(groupId, Permission.ExecuteThread);

			SecurityCredentials sc = new SecurityCredentials("username1", "password1");

			EnsurePermission(sc, Permission.ExecuteThread);

			// the above throws an exception if something is wrong so we are doing OK if we get this far
			Assert.IsTrue(true);
		}

		/// <summary>
		/// Add no group or permissions
		/// Check for a permission
		/// It should throw an AuthorizationException 
		/// </summary>
		[Test]
		public void EnsurePermissionTestNoPermission()
		{
			SecurityCredentials sc = new SecurityCredentials("username1", "password1");

			try
			{
				EnsurePermission(sc, Permission.ExecuteThread);
			}
			catch(AuthorizationException)
			{
				Assert.IsTrue(true);
				return;
			}

			Assert.IsFalse(true, "The authorization should fail");			
		}

		/// <summary>
		/// Add a user
		/// Check if the user is authenticated
		/// </summary>
		[Test]
		public void AuthenticateUserTestSimpleScenario()
		{
			Int32 groupId = 12;

			UserStorageView[] users = new UserStorageView[1];

			users[0] = new UserStorageView("username1", "password1", groupId);

			m_managerStorage.AddUsers(users);

			SecurityCredentials sc = new SecurityCredentials("username1", "password1");

			AuthenticateUser(sc);

			// the above throws an exception if something is wrong so we are doing OK if we get this far
			Assert.IsTrue(true);
		}

		/// <summary>
		/// Add no user
		/// Check if the user is authenticated
		/// An AuthenticationException is expected
		/// </summary>
		[Test]
		public void AuthenticateUserTestNoUsers()
		{
			SecurityCredentials sc = new SecurityCredentials("username1", "password1");

			try
			{
				AuthenticateUser(sc);
			}
			catch(AuthenticationException)
			{
				Assert.IsTrue(true);
				return;
			}

			Assert.IsFalse(true, "The authorization should fail");			
		}

//		[Test]
//		public void Admon_GetLiveApplicationListTestSimpleScenario()
//		{
//			Assert.Fail("Test not implemented.");
//		}
//		
//		[Test]
//		public void Admon_GetUserApplicationListTestSimpleScenario()
//		{
//			Assert.Fail("Test not implemented.");
//		}
		


	}
}
