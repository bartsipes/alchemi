using System;

namespace Alchemi.Core.Manager.Storage
{
	/// <summary>
	/// Override some generic database calls with SQL Server specific calls.
	/// This is usually done for performance reasons.
	/// </summary>
	public class SqlServerManagerDatabaseStorage : GenericManagerDatabaseStorage
	{
		public SqlServerManagerDatabaseStorage()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
