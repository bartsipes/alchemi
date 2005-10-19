//
// Alchemi.Tester.Manager.Storage.SqlServerManagerDatabaseStorageTester.cs
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
using System.Configuration;

using Alchemi.Core;
using Alchemi.Core.Manager.Storage;

using NUnit.Framework;

namespace Alchemi.Tester.Manager.Storage
{
	/// <summary>
	/// SQL Server specific tests
	/// Replace the managerStorage object with a SQL Server storage and 
	/// inherit all tests from the ManagerStorageTester
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
