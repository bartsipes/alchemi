#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
* Title         :  GManagerTester.cs
* Project       :  Alchemi.Tester.Manager
* Created on    :  22 October 2005
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

		private void SetupApplicationsGroupsAndUsers(Permission permission)
		{
			// add permissions
			Int32 groupId = 12;

			GroupStorageView[] groups = new GroupStorageView[1];

			groups[0] = new GroupStorageView(groupId, "test1");

			UserStorageView[] users = new UserStorageView[1];

			users[0] = new UserStorageView("username1", "password1", groupId);

			m_managerStorage.AddGroups(groups);

			m_managerStorage.AddUsers(users);

			m_managerStorage.AddGroupPermission(groupId, permission);

			SecurityCredentials sc = new SecurityCredentials("username1", "password1");

			// add applications, only one assigned to this user

			m_managerStorage.AddApplication(new ApplicationStorageView("username1"));
			m_managerStorage.AddApplication(new ApplicationStorageView("username2"));
			m_managerStorage.AddApplication(new ApplicationStorageView("username3"));		
		}

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

		/// <summary>
		/// Add permissions and group membership so that EnsurePermission(sc,Permission.ManageOwnApp) does not throw an exception
		/// </summary>
		[Test]
		public void Admon_GetLiveApplicationListTestSimpleScenario()
		{
			SetupApplicationsGroupsAndUsers(Permission.ManageOwnApp);

			SecurityCredentials sc = new SecurityCredentials("username1", "password1");

			ApplicationStorageView[] result = Admon_GetUserApplicationList(sc);

			Assert.AreEqual(1, result.Length);
		}
		
		[Test]
		public void Admon_GetUserApplicationListTestSimpleScenario()
		{
			SetupApplicationsGroupsAndUsers(Permission.ManageAllApps);

			SecurityCredentials sc = new SecurityCredentials("username1", "password1");

			ApplicationStorageView[] result = Admon_GetLiveApplicationList(sc);

			Assert.AreEqual(3, result.Length);
		}
		
		/// <summary>
		/// Setup applications but user has only rights to see own apps
		/// Should return oly one application
		/// </summary>
		[Test]
		public void Admon_GetUserApplicationListTestNotEnoughPermissions()
		{
			SetupApplicationsGroupsAndUsers(Permission.ManageOwnApp);

			SecurityCredentials sc = new SecurityCredentials("username1", "password1");

			ApplicationStorageView[] result = Admon_GetLiveApplicationList(sc);

			Assert.AreEqual(1, result.Length);
		}


	}
}
