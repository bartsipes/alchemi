using System;

using Alchemi.Core;
using Alchemi.Core.Manager.Storage;

using NUnit.Framework;

namespace Alchemi.Tester.Manager.Storage
{
	/// <summary>
	/// Summary description for InMemoryManagerStorage.
	/// </summary>
	[TestFixture]
	public class InMemoryManagerStorageTester : ManagerStorageTester
	{
		private InMemoryManagerStorage m_managerStorage;

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
			m_managerStorage = new InMemoryManagerStorage();
		}

		public InMemoryManagerStorageTester()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
