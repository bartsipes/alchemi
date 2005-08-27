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
