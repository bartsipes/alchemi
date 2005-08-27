using System;
using System.Data;

using Advanced.Data.Provider;

namespace Alchemi.Core.Manager.Storage
{
	/// <summary>
	/// Override some generic database calls with SQL Server specific calls.
	/// This is usually done for performance reasons.
	/// </summary>
	public class SqlServerManagerDatabaseStorage : GenericManagerDatabaseStorage
	{
		private String m_connectionString;

		public SqlServerManagerDatabaseStorage(String connectionString)
		{
			m_connectionString = connectionString;
		}

		#region IManagerStorage Members

		/// <summary>
		/// GetSystemSummary implementation for Sql Server.
		/// This implementation uses a stored procedure to extract the data.
		/// </summary>
		/// <returns></returns>
		public new SystemSummary GetSystemSummary()
		{
			AdpConnection connection = new AdpConnection(m_connectionString);
			AdpCommand command = new AdpCommand();

			command.Connection = connection;
			command.CommandText = "Admon_SystemSummary";
			command.CommandType = CommandType.StoredProcedure;

			using (AdpDataReader dataReader = command.ExecuteReader())
			{
				if (dataReader.Read())
				{
					String maxPower;
					Int32 totalExecutors;
					Int32 powerUsage;
					Int32 powerAvailable;
					String powerTotalUsage;
					Int32 unfinishedThreads;

					maxPower = dataReader.GetString(dataReader.GetOrdinal("max_power"));
					totalExecutors = dataReader.GetInt32(dataReader.GetOrdinal("total_executors"));
					powerUsage = dataReader.GetInt32(dataReader.GetOrdinal("power_usage"));
					powerAvailable = dataReader.GetInt32(dataReader.GetOrdinal("power_avail"));
					powerTotalUsage = dataReader.GetString(dataReader.GetOrdinal("power_totalusage"));
					unfinishedThreads = dataReader.GetInt32(dataReader.GetOrdinal("unfinished_threads"));

					return new SystemSummary(
						maxPower, 
						totalExecutors,
						powerUsage,
						powerAvailable,
						powerTotalUsage,
						unfinishedThreads);
				}
			}

			return null;
		}

		#endregion

	}
}
