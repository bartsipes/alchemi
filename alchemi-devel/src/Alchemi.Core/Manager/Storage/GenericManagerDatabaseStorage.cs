using System;

namespace Alchemi.Core.Manager.Storage
{
	/// <summary>
	/// Summary description for GenericManagerDatabaseStorage.
	/// </summary>
	public class GenericManagerDatabaseStorage : IManagerStorage
	{
		public GenericManagerDatabaseStorage()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		#region IManagerStorage Members

		public SystemSummary GetSystemSummary()
		{
			throw new Exception("Not implemented");
			// TODO:  Add GenericManagerDatabaseStorage.GetSystemSummary implementation
			return null;
		}

		#endregion
	}
}
