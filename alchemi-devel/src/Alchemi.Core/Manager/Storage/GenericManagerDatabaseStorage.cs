#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
* Title         :  GenericManagerDatabaseStorage.cs
* Project       :  Alchemi.Core.Manager.Storage
* Created on    :  30 August 2005
* Copyright     :  Copyright © 2005 The University of Melbourne
*                    This technology has been developed with the support of
*                    the Australian Research Council and the University of Melbourne
*                    research grants as part of the Gridbus Project
*                    within GRIDS Laboratory at the University of Melbourne, Australia.
* Author        :  Tibor Biro (tb@tbiro.com)
* License       :  GPL
*                    This program is free software; you can redistribute it and/or
*                    modify it under the terms of the GNU General Public
*                    License as published by the Free Software Foundation;
*                    See the GNU General Public License
*                    (http://www.gnu.org/copyleft/gpl.html) for more 
details.
*
*/
#endregion

using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

using Advanced.Data.Provider;

using Alchemi.Core.Owner;

namespace Alchemi.Core.Manager.Storage
{
	/// <summary>
	/// Implement generic relational database storage here
	/// This class should not be directly instantiated because it only contains a partial implementation
	/// </summary>
	public abstract class GenericManagerDatabaseStorage : IManagerStorage
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
			return RunSqlReturnScalar(sqlQuery, null);
		}

		protected object RunSqlReturnScalar(String sqlQuery, params SqlParameter[] parameters)
		{
			using(AdpConnection connection = new AdpConnection(m_connectionString))
			{
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

		public String AddApplication(ApplicationStorageView application)
		{
			if (application == null)
			{
				return null;
			}

			String applicationId = Guid.NewGuid().ToString();

			SqlParameter dateTimeParameter = new SqlParameter("@time_created", application.TimeCreated);

			RunSql(String.Format("insert into application(application_id, state, time_created, is_primary, usr_name) values ('{0}', {1}, @time_created, '{2}', '{3}')",
				applicationId,
				application.State,
				Convert.ToInt16(application.Primary),
				application.Username.Replace("'", "''")
				), 
				dateTimeParameter);

			return applicationId;
		}

		public void UpdateApplication(ApplicationStorageView updatedApplication)
		{
			if (updatedApplication == null || updatedApplication.ApplicationId == null || updatedApplication.ApplicationId.Length == 0)
			{
				return;
			}

			SqlParameter dateTimeParameter = new SqlParameter("@time_created", updatedApplication.TimeCreated);

			RunSql(String.Format("update application set state = {1}, time_created = @time_created, is_primary = '{2}', usr_name = '{3}' where application_id = '{0}'",
				updatedApplication.ApplicationId,
				updatedApplication.State,
				Convert.ToInt16(updatedApplication.Primary),
				updatedApplication.Username.Replace("'", "''")
				), 
				dateTimeParameter);
		}

		public ApplicationStorageView[] GetApplications()
		{
			return GetApplications(false);
		}

		public ApplicationStorageView[] GetApplications(bool populateThreadCount)
		{
			ArrayList applications = new ArrayList();

			using(AdpDataReader dataReader = RunSqlReturnDataReader(String.Format("select application_id, state, time_created, is_primary, usr_name from application")))
			{
				while(dataReader.Read())
				{
					// in SQL the application ID is stored as a GUID so we use GetValue instead of GetString in order to maximize compatibility with other databases
					String applicationId = dataReader.GetValue(dataReader.GetOrdinal("application_id")).ToString(); 

					Int32 state = dataReader.GetInt32(dataReader.GetOrdinal("state"));
					DateTime timeCreated = dataReader.GetDateTime(dataReader.GetOrdinal("time_created"));
					bool primary = dataReader.GetBoolean(dataReader.GetOrdinal("is_primary"));
					String username = dataReader.GetString(dataReader.GetOrdinal("usr_name"));

					ApplicationStorageView application = new ApplicationStorageView(
						applicationId,
						state,
						timeCreated,
						primary,
						username
						);

					if (populateThreadCount)
					{
						Int32 totalThreads;
						Int32 unfinishedThreads;

						GetApplicationThreadCount(application.ApplicationId, out totalThreads, out unfinishedThreads);

						application.TotalThreads = totalThreads;
						application.UnfinishedThreads = unfinishedThreads;
					}

					applications.Add(application);
				}
			}

			return (ApplicationStorageView[])applications.ToArray(typeof(ApplicationStorageView));
		}

		public ApplicationStorageView[] GetApplications(String userName, bool populateThreadCount)
		{
			ArrayList applications = new ArrayList();

			using(AdpDataReader dataReader = RunSqlReturnDataReader(String.Format("select application_id, state, time_created, is_primary, usr_name from application where usr_name = '{0}'",
					  userName.Replace("'", "''"))))
			{
				while(dataReader.Read())
				{
					// in SQL the application ID is stored as a GUID so we use GetValue instead of GetString in order to maximize compatibility with other databases
					String applicationId = dataReader.GetValue(dataReader.GetOrdinal("application_id")).ToString(); 

					Int32 state = dataReader.GetInt32(dataReader.GetOrdinal("state"));
					DateTime timeCreated = dataReader.GetDateTime(dataReader.GetOrdinal("time_created"));
					bool primary = dataReader.GetBoolean(dataReader.GetOrdinal("is_primary"));
					String username = dataReader.GetString(dataReader.GetOrdinal("usr_name"));

					ApplicationStorageView application = new ApplicationStorageView(
						applicationId,
						state,
						timeCreated,
						primary,
						username
						);

					if (populateThreadCount)
					{
						Int32 totalThreads;
						Int32 unfinishedThreads;

						GetApplicationThreadCount(application.ApplicationId, out totalThreads, out unfinishedThreads);

						application.TotalThreads = totalThreads;
						application.UnfinishedThreads = unfinishedThreads;
					}

					applications.Add(application);
				}
			}

			return (ApplicationStorageView[])applications.ToArray(typeof(ApplicationStorageView));
		}

		public ApplicationStorageView GetApplication(String applicationId)
		{
			using(AdpDataReader dataReader = RunSqlReturnDataReader(String.Format("select application_id, state, time_created, is_primary, usr_name from application where application_id='{0}'", applicationId)))
			{
				if(dataReader.Read())
				{
					Int32 state = dataReader.GetInt32(dataReader.GetOrdinal("state"));
					DateTime timeCreated = dataReader.GetDateTime(dataReader.GetOrdinal("time_created"));
					bool primary = dataReader.GetBoolean(dataReader.GetOrdinal("is_primary"));
					String username = dataReader.GetString(dataReader.GetOrdinal("usr_name"));

					ApplicationStorageView application = new ApplicationStorageView(
						applicationId,
						state,
						timeCreated,
						primary,
						username
						);

					return application;
				}
				else
				{
					return null;
				}
			}
		}

		public Int32 AddThread(ThreadStorageView thread)
		{
			if (thread == null)
			{
				return -1;
			}

			SqlParameter timeStartedParameter = new SqlParameter("@time_started", thread.TimeStarted);
			SqlParameter timeFinishedParameter = new SqlParameter("@time_finished", thread.TimeFinished);
			SqlParameter executorIdParameter;
			
			if (thread.ExecutorId != null)
			{
				executorIdParameter = new SqlParameter("@executor_id", thread.ExecutorId);
			}
			else
			{
				executorIdParameter = new SqlParameter("@executor_id", DBNull.Value);
			}

			object threadIdObject = RunSqlReturnScalar(String.Format("insert into thread(application_id, executor_id, thread_id, state, time_started, time_finished, priority, failed) values ('{0}', @executor_id, {2}, {3}, @time_started, @time_finished, {4}, '{5}')",
				thread.ApplicationId,
				thread.ExecutorId,
				thread.ThreadId,
				(int)thread.State,
				thread.Priority,
				Convert.ToInt16(thread.Failed)
				), 
				timeStartedParameter, timeFinishedParameter, executorIdParameter);

			return Convert.ToInt32(threadIdObject);
		}

		public void UpdateThread(ThreadStorageView updatedThread)
		{
			if (updatedThread == null)
			{
				return;
			}

			SqlParameter timeStartedParameter = new SqlParameter("@time_started", updatedThread.TimeStarted);
			SqlParameter timeFinishedParameter = new SqlParameter("@time_finished", updatedThread.TimeFinished);

			object threadIdObject = RunSqlReturnScalar(String.Format("update thread set application_id = '{1}', executor_id = '{2}', thread_id = {3}, state = {4}, time_started = @time_started, time_finished = @time_finished, priority = {5}, failed = '{6}' where internal_thread_id = {0}",
				updatedThread.InternalThreadId,
				updatedThread.ApplicationId,
				updatedThread.ExecutorId,
				updatedThread.ThreadId,
				(int)updatedThread.State,
				updatedThread.Priority,
				Convert.ToInt16(updatedThread.Failed)
				), 
				timeStartedParameter, timeFinishedParameter);
		}

		public ThreadStorageView[] GetThreads()
		{
			ArrayList threads = new ArrayList();

			using(AdpDataReader dataReader = RunSqlReturnDataReader(String.Format("select internal_thread_id, application_id, executor_id, thread_id, state, time_started, time_finished, priority, failed from thread")))
			{
				while(dataReader.Read())
				{
					Int32 internalThreadId = dataReader.GetInt32(dataReader.GetOrdinal("internal_thread_id"));

					// in SQL the application ID is stored as a GUID so we use GetValue instead of GetString in order to maximize compatibility with other databases
					String applicationId = dataReader.GetValue(dataReader.GetOrdinal("application_id")).ToString(); 
					String executorId = dataReader.GetValue(dataReader.GetOrdinal("executor_id")).ToString(); 

					Int32 threadId = dataReader.GetInt32(dataReader.GetOrdinal("thread_id"));
					ThreadState state = (ThreadState)dataReader.GetInt32(dataReader.GetOrdinal("state"));
					DateTime timeStarted = dataReader.GetDateTime(dataReader.GetOrdinal("time_started"));
					DateTime timeFinished = dataReader.GetDateTime(dataReader.GetOrdinal("time_finished"));
					Int32 priority = dataReader.GetInt32(dataReader.GetOrdinal("priority"));
					bool failed = dataReader.GetBoolean(dataReader.GetOrdinal("failed"));

					ThreadStorageView thread = new ThreadStorageView(
						internalThreadId,
						applicationId,
						executorId,
						threadId,
						state,
						timeStarted,
						timeFinished,
						priority,
						failed
						);

					threads.Add(thread);
				}
			}

			return (ThreadStorageView[])threads.ToArray(typeof(ThreadStorageView));
		}

		public void GetApplicationThreadCount(String applicationId, out Int32 totalThreads, out Int32 unfinishedThreads)
		{
			totalThreads = unfinishedThreads = 0;

			using(AdpDataReader dataReader = RunSqlReturnDataReader(String.Format("select state from thread where application_id = '{0}'",
					  applicationId)))
			{
				while(dataReader.Read())
				{
					Int32 state = dataReader.GetInt32(dataReader.GetOrdinal("state"));

					totalThreads ++;

					if (state == 0 || state == 1 || state == 2)
					{
						unfinishedThreads ++;
					}

				}
			}

		}

		#endregion

	}
}
