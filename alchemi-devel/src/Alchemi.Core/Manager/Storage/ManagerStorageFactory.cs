using System;

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
				m_managerStorage = new InMemoryManagerStorage();
			}
			return m_managerStorage;
		}
	}
}
