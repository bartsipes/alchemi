//
// Alchemi.Tester.Manager.Storage.InMemoryManagerStorageTester.cs
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
