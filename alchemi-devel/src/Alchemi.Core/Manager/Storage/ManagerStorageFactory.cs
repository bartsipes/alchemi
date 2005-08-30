//
// Alchemi.Core.Manager.Storage.ManagerStorageFactory.cs
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

namespace Alchemi.Core.Manager.Storage
{
	/// <summary>
	/// Takes care of choosing the right storage.
	/// </summary>
	public class ManagerStorageFactory
	{
		private static IManagerStorage m_managerStorage = null;

		public ManagerStorageFactory()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static IManagerStorage ManagerStorage()
		{
			if (m_managerStorage == null)
			{
				m_managerStorage = GetManagerStorage();
			}

			return m_managerStorage;
		}

		/// <summary>
		/// Create the right manager storage object.
		/// </summary>
		/// <returns></returns>
		private static IManagerStorage GetManagerStorage()
		{
			// TODO: implement different storages based on the current configuration file

			Configuration configuration = Configuration.GetConfiguration();

			String connectionString = ""; // TODO: create this

			return new SqlServerManagerDatabaseStorage(connectionString);
		}
	}
}
