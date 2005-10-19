//
// Alchemi.Core.Manager.Storage.GenericManagerDatabaseStorage.cs
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
using System.Collections;
using System.Data;
using System.Data.SqlClient;

using Advanced.Data.Provider;

namespace Alchemi.Core.Manager.Storage
{
	/// <summary>
	/// Summary description for GenericManagerDatabaseStorage.
	/// </summary>
	public class GenericManagerDatabaseStorage : IManagerStorage
	{
		private String m_connectionString;

		public GenericManagerDatabaseStorage(String connectionString)
		{
			m_connectionString = connectionString;
		}

		#region "Generic database manipulation routines"
		
		/// <summary>
		/// Run a stored procedure and return a data reader.
		/// The caller is responsible for closing the database connection.
		/// </summary>
		/// <param name="storedProcedure"></param>
		/// <returns></returns>
		protected AdpDataReader RunSpReturnDataReader(String storedProcedure)
		{
			AdpConnection connection = new AdpConnection(m_connectionString);
			AdpCommand command = new AdpCommand();

			command.Connection = connection;
			command.CommandText = storedProcedure;
			command.CommandType = CommandType.StoredProcedure;
		
			return command.ExecuteReader();
		}

		protected AdpDataReader RunSqlReturnDataReader(String sqlQuery)
		{
			AdpConnection connection = new AdpConnection(m_connectionString);
			AdpCommand command = new AdpCommand();

			command.Connection = connection;
			command.CommandText = sqlQuery;
			command.CommandType = CommandType.Text;
		
			return command.ExecuteReader();
		}

		/// <summary>
		/// Run a stored procedure and return a scalar.
		/// </summary>
		/// <param name="storedProcedure"></param>
		/// <param name="paramArray"></param>
		/// <returns></returns>
		protected object RunSpReturnScalar(String storedProcedure, params SqlParameter[] parameters)
		{
			AdpConnection connection = new AdpConnection(m_connectionString);
			AdpCommand command = new AdpCommand();

			command.Connection = connection;
			command.CommandText = storedProcedure;
			command.CommandType = CommandType.StoredProcedure;

			foreach (SqlParameter parameter in parameters)
			{
				command.CreateParameter(parameter.ParameterName, parameter.Value);				
			}
		
			return command.ExecuteScalar();
		}

		protected void RunSql(String sqlQuery)
		{
			RunSql(sqlQuery, null);
		}

		protected void RunSql(String sqlQuery, params SqlParameter[] parameters)
		{
			AdpConnection connection = new AdpConnection(m_connectionString);
			AdpCommand command = new AdpCommand();

			command.Connection = connection;
			command.CommandText = sqlQuery;
			command.CommandType = CommandType.Text;

			if (parameters != null)
			{
				foreach(SqlParameter parameter in parameters)
				{
					command.CreateParameter(parameter.ParameterName, parameter.Value);
				}
			}
		
			command.ExecuteNonQuery();

			connection.Close();
		}

		protected object RunSqlReturnScalar(String sqlQuery)
		{
			using(AdpConnection connection = new AdpConnection(m_connectionString))
			{
				AdpCommand command = new AdpCommand();

				command.Connection = connection;
				command.CommandText = sqlQuery;
				command.CommandType = CommandType.Text;
		
				return command.ExecuteScalar();
			}
		}

		#endregion

		#region IManagerStorage Members

		public SystemSummary GetSystemSummary()
		{
			throw new Exception("Not implemented");
		}

		/// <summary>
		/// Add users to a database
		/// </summary>
		/// <param name="users"></param>
		public void AddUsers(UserStorageView[] users)
		{
			if (users == null)
			{
				return;
			}

			foreach (UserStorageView user in users)
			{
				String sqlQuery;
				
				sqlQuery = String.Format("insert usr values('{0}', '{1}', {2})", 
					user.Username.Replace("'", "''"), 
					user.Password.Replace("'", "''"), 
					user.GroupId);
				
				RunSql(sqlQuery);
			}
		}

		public void UpdateUsers(UserStorageView[] updates)
		{
			if (updates == null)
			{
				return;
			}

			foreach (UserStorageView user in updates)
			{
				String sqlQuery;
				
				sqlQuery = String.Format("update usr set password='{1}', grp_id={2} where usr_name='{0}'", 
					user.Username.Replace("'", "''"), 
					user.Password.Replace("'", "''"), 
					user.GroupId);
				
				RunSql(sqlQuery);
			}
		}

		public UserStorageView[] GetUserList()
		{
			ArrayList userList = new ArrayList();

			using(AdpDataReader dataReader = RunSqlReturnDataReader("select usr_name, password, grp_id from usr"))
			{
				while(dataReader.Read())
				{
					String username = dataReader.GetString(dataReader.GetOrdinal("usr_name"));
					String password = dataReader.GetString(dataReader.GetOrdinal("password"));
					Int32 groupId = dataReader.GetInt32(dataReader.GetOrdinal("grp_id"));

					UserStorageView user = new UserStorageView(username, password, groupId);

					userList.Add(user);
				}
			}

			return (UserStorageView[])userList.ToArray(typeof(UserStorageView));
		}

		public bool AuthenticateUser(SecurityCredentials sc)
		{
			if (sc == null || sc.Username == null || sc.Password == null)
			{
				return false;
			}

			object userCount = RunSqlReturnScalar(String.Format("select count(*) as authenticated from usr where usr_name = '{0}' and password = '{1}'",
				sc.Username.Replace("'", "''"),
				sc.Password.Replace("'", "''")));

			return Convert.ToBoolean(userCount);
		}

		public void AddGroups(GroupStorageView[] groups)
		{
			if (groups == null)
			{
				return;
			}

			foreach (GroupStorageView group in groups)
			{
				String sqlQuery;
				
				sqlQuery = String.Format("insert grp values({0}, '{1}')", 
					group.GroupId,
					group.GroupName.Replace("'", "''"));
				
				RunSql(sqlQuery);
			}
		}
		
		public GroupStorageView[] GetGroups()
		{
			ArrayList groupList = new ArrayList();

			using(AdpDataReader dataReader = RunSqlReturnDataReader("select grp_id, grp_name from grp"))
			{
				while(dataReader.Read())
				{
					Int32 groupId = dataReader.GetInt32(dataReader.GetOrdinal("grp_id"));
					String groupName = dataReader.GetString(dataReader.GetOrdinal("grp_name"));

					GroupStorageView group = new GroupStorageView(groupId, groupName);

					groupList.Add(group);
				}
			}

			return (GroupStorageView[])groupList.ToArray(typeof(GroupStorageView));
		}

		public String AddExecutor(ExecutorStorageView executor)
		{
			if (executor == null)
			{
				return null;
			}

			String executorId = Guid.NewGuid().ToString();

			RunSql(String.Format("insert into executor(executor_id, is_dedicated, is_connected, usr_name) values ('{0}', {1}, {2}, '{3}')",
				executorId,
				Convert.ToInt16(executor.Dedicated),
				Convert.ToInt16(executor.Connected),
				executor.Username.Replace("'", "''")
				));

			UpdateExecutorPingTime(executorId, executor.PingTime);

			UpdateExecutorHostAddress(executorId, executor.HostName, executor.Port);

			UpdateExecutorCpuUsage(executorId, executor.MaxCpu, executor.CpuUsage, executor.AvailableCpu, executor.TotalCpuUsage);

			return executorId;
		}

		private void UpdateExecutorPingTime(String executorId, DateTime pingTime)
		{
			SqlParameter dateTimeParameter = new SqlParameter("@ping_time", pingTime);
			
			RunSql(String.Format("update executor set ping_time=@ping_time where executor_id='{0}'",
				executorId
				), 
				dateTimeParameter);		
		}

		private void UpdateExecutorHostAddress(String executorId, String hostName, Int32 port)
		{
			RunSql(String.Format("update executor set host='{1}', port={2} where executor_id='{0}'",
				executorId,
				hostName.Replace("'", "''"),
				port
				));
		}

		private void UpdateExecutorCpuUsage(String executorId, Int32 maxCpu, Int32 cpuUsage, Int32 availableCpu, float totalCpuUsage)
		{
			RunSql(String.Format("update executor set cpu_max={1}, cpu_usage={2}, cpu_avail={3}, cpu_totalusage={4} where executor_id='{0}'",
				executorId,
				maxCpu,
				cpuUsage,
				availableCpu,
				totalCpuUsage
				));
		}

		private void UpdateExecutorDetails(String executorId, bool dedicated, bool connected, String userName)
		{
			RunSql(String.Format("update executor set is_dedicated='{1}', is_connected='{2}', usr_name='{3}' where executor_id='{0}'",
				executorId,
				Convert.ToInt16(dedicated),
				Convert.ToInt16(connected),
				userName.Replace("'", "''")
				));
		}

		public void UpdateExecutor(ExecutorStorageView executor)
		{
			if (executor == null || executor.ExecutorId == null || executor.ExecutorId.Length == 0)
			{
				return;
			}

			UpdateExecutorDetails(executor.ExecutorId, executor.Dedicated, executor.Connected, executor.Username);

			UpdateExecutorPingTime(executor.ExecutorId, executor.PingTime);

			UpdateExecutorHostAddress(executor.ExecutorId, executor.HostName, executor.Port);

			UpdateExecutorCpuUsage(executor.ExecutorId, executor.MaxCpu, executor.CpuUsage, executor.AvailableCpu, executor.TotalCpuUsage);
		}

		public ExecutorStorageView[] GetExecutors()
		{
			ArrayList executors = new ArrayList();

			using(AdpDataReader dataReader = RunSqlReturnDataReader(String.Format("select executor_id, is_dedicated, is_connected, ping_time, host, port, usr_name, cpu_max, cpu_usage, cpu_avail, cpu_totalusage from executor")))
			{
				while(dataReader.Read())
				{
					// in SQL the executor ID is stored as a GUID so we use GetValue instead of GetString in order to maximize compatibility with other databases
					String executorId = dataReader.GetValue(dataReader.GetOrdinal("executor_id")).ToString(); 

					bool dedicated = dataReader.GetBoolean(dataReader.GetOrdinal("is_dedicated"));
					bool connected = dataReader.GetBoolean(dataReader.GetOrdinal("is_connected"));
					DateTime pingTime = dataReader.GetDateTime(dataReader.GetOrdinal("ping_time"));
					String hostname = dataReader.GetString(dataReader.GetOrdinal("host"));
					Int32 port = dataReader.GetInt32(dataReader.GetOrdinal("port"));
					String username = dataReader.GetString(dataReader.GetOrdinal("usr_name"));
					Int32 maxCpu = dataReader.GetInt32(dataReader.GetOrdinal("cpu_max"));
					Int32 cpuUsage = dataReader.GetInt32(dataReader.GetOrdinal("cpu_usage"));
					Int32 availableCpu = dataReader.GetInt32(dataReader.GetOrdinal("cpu_avail"));
					float totalCpuUsage = (float)dataReader.GetDouble(dataReader.GetOrdinal("cpu_totalusage"));

					ExecutorStorageView executor = new ExecutorStorageView(
						executorId,
						dedicated,
						connected,
						pingTime,
						hostname,
						port,
						username,
						maxCpu,
						cpuUsage,
						availableCpu,
						totalCpuUsage
						);

					executors.Add(executor);
				}
			}

			return (ExecutorStorageView[])executors.ToArray(typeof(ExecutorStorageView));
		}

		#endregion

	}
}
