using System;

namespace Alchemi.Core.Manager.Storage
{
	/// <summary>
	/// Store all manager information in memory.
	/// 
	/// This type of storage is not persistent but usefull for testing or for running 
	/// lightweight managers.
	/// </summary>
	public class InMemoryManagerStorage : IManagerStorage
	{
		public InMemoryManagerStorage()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#region IManagerStorage Members

		public SystemSummary GetSystemSummary()
		{
			throw new Exception("Not implemented");
			// TODO:  Add InMemoryManagerStorage.GetSystemSummary implementation
			return null;
		}

		#endregion
	}
}
