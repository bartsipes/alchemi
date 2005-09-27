using System;
using System.Configuration;

using Alchemi.Core;
using Alchemi.Core.Manager.Storage;

using NUnit.Framework;

namespace Alchemi.Tester.Manager.Storage
{
	/// <summary>
	/// Summary description for SqlServerManagerDatabaseStorageTester.
	/// </summary>
	[TestFixture]
	public class SqlServerManagerDatabaseStorageTester : ManagerStorageTester
	{
		private SqlServerManagerDatabaseStorage m_managerStorage;

		public override IManagerStorage ManagerStorage
		{
			get
			{
				return m_managerStorage;
			}
		}

		[SetUp]
		public void TestStartUp()
		{
			String connectionString = ConfigurationSettings.AppSettings["SqlTesterConnectionString"];
			connectionString = "adpprovider=MsSql;server=localhost;database=AlchemiTester;User ID=alchemi;Password=alchemi";

			m_managerStorage = new SqlServerManagerDatabaseStorage(connectionString);

			m_managerStorage.SetUpStorage();
		}

		[TearDown]
		public void TestShutDown()
		{
			m_managerStorage.TearDownStorage();
		}

		public SqlServerManagerDatabaseStorageTester()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
