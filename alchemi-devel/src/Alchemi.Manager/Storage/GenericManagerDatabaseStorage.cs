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
* Author        :  Tibor Biro (tb@tbiro.com), Krishna Nadiminti (kna@csse.unimelb.edu.au)
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
using System.Data.OleDb;
using System.IO;
using System.Reflection;
using System.Text;
using Alchemi.Core;
using Alchemi.Core.Manager;
using Alchemi.Core.Manager.Storage;
using Alchemi.Core.Owner;
using Alchemi.Core.Utility;

namespace Alchemi.Manager.Storage
{
	/// <summary>
	/// Implement generic relational database storage here
	/// This class should not be directly instantiated because it only contains a partial implementation
	/// 
	/// TODO: The Executors are updated very often so database updates will probably be very expensive. Change the update functions to only update data that actually changed since the last load.
	/// </summary>
	public abstract class GenericManagerDatabaseStorage : ManagerStorageBase, IManagerStorage, IManagerStorageSetup 
	{
		// Create a logger for use in this class
		protected static readonly Logger logger = new Logger();

		protected string ConnectionString;

		protected virtual string IsNullOperator
		{
			get
			{
				return "IsNull";
			}
		}
		

		public GenericManagerDatabaseStorage(string connectionString)
		{
			ConnectionString = connectionString;
		}

		#region IManagerStorageSetup Members

		protected virtual string GetSetupFileLocation()
		{
			return "SqlServer";
		}


		public void CreateStorage(string databaseName)
		{
			string sqlScript = GetStringFromEmbededScriptFile(GetSetupFileLocation(), "Alchemi_database.sql");

			/// The database name is unknown at compilation time so we have to replace it now
			sqlScript = sqlScript.Replace("DATABASE_NAME_TOKEN", databaseName);

			RunSql(sqlScript);
		}

		/// <summary>
		/// Create the tables, stored procedures and other structures needed by this storage.
		/// </summary>
		public void SetUpStorage()
		{
			string sqlScript = GetStringFromEmbededScriptFile(GetSetupFileLocation(), "Alchemi_structure.sql");

			RunSql(sqlScript);			
		}

		public void InitializeStorageData()
		{
			string sqlScript = GetStringFromEmbededScriptFile(GetSetupFileLocation(), "Alchemi_data.sql");

			RunSql(sqlScript);

			CreateDefaultObjects(this);
		}

		public void TearDownStorage()
		{
			string sqlScript = GetStringFromEmbededScriptFile(GetSetupFileLocation(), "Alchemi_structure_drop.sql");

			RunSql(sqlScript);
		}

		#endregion

		#region IManagerStorage Members

		public bool VerifyConnection()
		{
			bool isValid = false;
			
            try
			{
				RunSql("SELECT 1;");
				isValid = true;
			}
			catch (Exception ex)
			{
				logger.Warn("VerifyConnection error: ", ex);
			}

			return isValid;
		}

        protected virtual string GetPowerUsageSqlQuery()
        {
            return String.Format(
                "select count(*) as total_executors, " +
                "{0}(sum(cpu_max), 0) as max_power," +
                "{0}(avg(cpu_usage), 0) as power_usage, {0}(avg(cpu_avail), 0) as power_avail," +
                "{0}(sum(cpu_totalusage * cpu_max / (3600 * 1000)), 0) as power_totalusage " +
                "from executor where is_connected = 1 ", IsNullOperator );
        }

        protected virtual string GetUnfinishedThreadCountSqlQuery()
        {
            return "select count(*) as unfinished_threads from thread where state not in (3, 4)"; ;
        }

        protected virtual string GetUnfinishedApplicationCountSqlQuery()
        {
            return  "select count(*) as unfinished_apps " +
                    "from application " +
                    "where state not in (2) ";
        }

        protected virtual string GetUserCountSqlQuery( SecurityCredentials sc )
        {
            return String.Format(
                "select count(*) as authenticated from usr where usr_name = '{0}' and password = '{1}'"
				, Utils.MakeSqlSafe(sc.Username)
				, Utils.MakeSqlSafe(sc.Password) 
                );
        }

		/// <summary>
		/// GetSystemSummary implementation for RDBMS.
		/// </summary>
		/// <returns></returns>
		public SystemSummary  GetSystemSummary()
		{
			//build the System_Summary SQLs

            string powerUsageSqlQuery = this.GetPowerUsageSqlQuery();
            string unfinishedThreadCountSqlQuery = this.GetUnfinishedThreadCountSqlQuery();
            string unfinishedApplicationCountSqlQuery = this.GetUnfinishedApplicationCountSqlQuery();
			
			SystemSummary summary = null;
			string maxPower= null;
			int totalExecutors = 0;
			int powerUsage = 0;
			int powerAvailable = 0;
			string powerTotalUsage = null;
			int unfinishedApps = 0;
			int unfinishedThreads = 0;
			
			try
			{
				using (IDataReader dataReader = RunSqlReturnDataReader(powerUsageSqlQuery))
				{
					if (dataReader.Read())
					{
						maxPower = String.Format("{0} GHz", 
							(double)dataReader.GetInt32(dataReader.GetOrdinal("max_power")) / 1000);
                        
						totalExecutors = dataReader.GetInt32(dataReader.GetOrdinal("total_executors"));
						powerUsage = dataReader.GetInt32(dataReader.GetOrdinal("power_usage"));
						powerAvailable = dataReader.GetInt32(dataReader.GetOrdinal("power_avail"));
						powerTotalUsage = String.Format("{0} GHz*Hr", 
							Math.Round(dataReader.GetDouble(dataReader.GetOrdinal("power_totalusage")), 6));
					}

					dataReader.Close();
				}
			
				unfinishedThreads = Convert.ToInt32(RunSqlReturnScalar(unfinishedThreadCountSqlQuery));

				unfinishedApps = Convert.ToInt32(RunSqlReturnScalar(unfinishedApplicationCountSqlQuery));
			}
			catch (Exception ex)
			{
				logger.Debug("Error getting system summary:",ex);
			}

			summary = new SystemSummary(
				maxPower, 
				totalExecutors,
				powerUsage,
				powerAvailable,
				powerTotalUsage,
				unfinishedApps,
				unfinishedThreads);

			return summary;
		}

		protected DataSet RunSqlReturnDataSet(string query)
		{
			DataSet result = null;
			using (IDbConnection connection = GetConnection(ConnectionString))
			{
				IDbCommand command = GetCommand();
				command.Connection = connection;
				command.CommandText = query;
				command.CommandType = CommandType.Text;
			
				connection.Open();
				IDataAdapter da = GetDataAdapter(command);
				result = new DataSet();
				da.Fill(result);
			}

			return result;
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
				string sqlQuery;
				
//				sqlQuery = String.Format("insert usr(usr_id, usr_name, password) values({0}, '{1}', '{2}')",
//					user.UserId,
//					Utils.MakeSqlSafe(user.Username), 
//					Utils.MakeSqlSafe(user.Password), 
//					);

				sqlQuery = String.Format("insert into usr(usr_name, password, grp_id, is_system) values('{0}', '{1}', {2}, {3})",
					Utils.MakeSqlSafe(user.Username), 
					Utils.MakeSqlSafe(user.PasswordMd5Hash), 
					user.GroupId,
					user.IsSystem ? 1 : 0);
				
				RunSql(sqlQuery);
			}
		}

//		public void UpdateGroupMembership(GroupStorageView group, UserStorageView[] users)
//		{
//			//todo : usr_grp //put this in the parent interface also
//			//delete all existing members, and add these members.
//		}

		public void UpdateUsers(UserStorageView[] updates)
		{
			if (updates == null)
			{
				return;
			}

			foreach (UserStorageView user in updates)
			{
				string sqlQuery;
				
				if (user.Password != null && user.Password != "")
				{
					logger.Debug("Updating password AND group id...." + user.PasswordMd5Hash);

					sqlQuery = String.Format("update usr set password='{1}', grp_id={2} where usr_name='{0}'", 
						Utils.MakeSqlSafe(user.Username), 
						Utils.MakeSqlSafe(user.PasswordMd5Hash), 
						user.GroupId);
				}
				else
				{
					logger.Debug("Updating only group id....");
					//just change only the group. dont touch the password.	
					sqlQuery = String.Format("update usr set grp_id={1} where usr_name='{0}'", 
						Utils.MakeSqlSafe(user.Username), 
						user.GroupId);
				}
				
				RunSql(sqlQuery);
			}
		}

		public UserStorageView[] GetUsers()
		{
			ArrayList userList = new ArrayList();

			using(IDataReader dataReader = RunSqlReturnDataReader("select usr_name, password, grp_id, is_system from usr"))
			{
				while(dataReader.Read())
				{
					string username = dataReader.GetString(dataReader.GetOrdinal("usr_name"));
					string password = dataReader.GetString(dataReader.GetOrdinal("password"));
					int groupId = dataReader.GetInt32(dataReader.GetOrdinal("grp_id"));
					bool isSystem = false;

					if (!dataReader.IsDBNull(dataReader.GetOrdinal("is_system")))
					{
						isSystem = dataReader.GetBoolean(dataReader.GetOrdinal("is_system"));
					}

					UserStorageView user = new UserStorageView(username);
					user.PasswordMd5Hash = password;
					user.GroupId = groupId;
					user.IsSystem = isSystem;
					userList.Add(user);
				}

				dataReader.Close();
			}

			return (UserStorageView[])userList.ToArray(typeof(UserStorageView));
		}
        public UserStorageView GetUser(string username)
        {
            ArrayList userList = new ArrayList();
            UserStorageView user = null;

            string sqlQuery = String.Format("select usr_name, password, grp_id, is_system from usr where usr_name = '{0}'",
                        Utils.MakeSqlSafe(username));

            using (IDataReader dataReader = RunSqlReturnDataReader(sqlQuery))
            {
                if (dataReader.Read())
                {
                    string password = dataReader.GetString(dataReader.GetOrdinal("password"));
                    int groupId = dataReader.GetInt32(dataReader.GetOrdinal("grp_id"));
                    bool isSystem = false;

                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("is_system")))
                    {
                        isSystem = dataReader.GetBoolean(dataReader.GetOrdinal("is_system"));
                    }

                    user = new UserStorageView(username);
                    user.PasswordMd5Hash = password;
                    user.GroupId = groupId;
                    user.IsSystem = isSystem;
                }

                dataReader.Close();
            }

            return (user);
        }

		public void DeleteUser(UserStorageView userToDelete)
		{
			if (userToDelete == null)
			{
				return;
			}

			string sqlQuery;
			
			sqlQuery = String.Format("delete from usr where usr_name='{0}'", 
				Utils.MakeSqlSafe(userToDelete.Username));
			
			logger.Debug(String.Format("Deleting user {0}", userToDelete.Username));

			RunSql(sqlQuery);
		}


		/// <summary>
		/// Authenticate a user's security credentials
		/// </summary>
		/// <param name="sc">Security credentials to authenticate</param>
		/// <returns>True if the authentication is successful, false otherwise.</returns>		
		public bool AuthenticateUser(SecurityCredentials sc)
		{
			if (sc == null || sc.Username == null || sc.Password == null)
			{
				return false;
			}

			object userCount = RunSqlReturnScalar( this.GetUserCountSqlQuery( sc ) );

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
				string sqlQuery;
				
				sqlQuery = String.Format("insert into grp(grp_id, grp_name, is_system) values({0}, '{1}', {2})", 
					group.GroupId,
					Utils.MakeSqlSafe(group.GroupName),
					Utils.BoolToSqlBit(group.IsSystem)
					);
				
				RunSql(sqlQuery);
			}
		}
		
		public GroupStorageView[] GetGroups()
		{
			ArrayList groupList = new ArrayList();

			using(IDataReader dataReader = RunSqlReturnDataReader("select grp_id, grp_name, is_system from grp"))
			{
				while(dataReader.Read())
				{
					int groupId = dataReader.GetInt32(dataReader.GetOrdinal("grp_id"));
					string groupName = dataReader.GetString(dataReader.GetOrdinal("grp_name"));
					bool isSystem = false;
					if (!dataReader.IsDBNull(dataReader.GetOrdinal("is_system")))
					{
						isSystem = dataReader.GetBoolean(dataReader.GetOrdinal("is_system"));
					}
					GroupStorageView group = new GroupStorageView(groupId, groupName);
					group.IsSystem = isSystem;

					groupList.Add(group);
				}

				dataReader.Close();
			}

			return (GroupStorageView[])groupList.ToArray(typeof(GroupStorageView));
		}

		public GroupStorageView GetGroup(int groupId)
		{
			using(IDataReader dataReader = RunSqlReturnDataReader(String.Format("select grp_id, grp_name, is_system from grp where grp_id={0}", groupId)))
			{
				if(dataReader.Read())
				{
					string groupName = dataReader.GetString(dataReader.GetOrdinal("grp_name"));
					bool isSystem = false;
					if (!dataReader.IsDBNull(dataReader.GetOrdinal("is_system")))
					{
						isSystem = dataReader.GetBoolean(dataReader.GetOrdinal("is_system"));
					}

					GroupStorageView group = new GroupStorageView(groupId, groupName);
					group.IsSystem = isSystem;

					dataReader.Close();
					return group;
				}
				else
				{
					dataReader.Close();
					return null;
				}

			}
		}

		public void AddGroupPermission(int groupId, Permission permission)
		{
			string sqlQuery;
			
			// in case there is a duplicate remove the permission first
			sqlQuery = String.Format("delete from grp_prm where grp_id={0} and prm_id={1}", 
				groupId,
				(Int32)permission);

			RunSql(sqlQuery);

			sqlQuery = String.Format("insert into grp_prm(grp_id, prm_id) values({0}, {1})", 
				groupId,
				(Int32)permission);
				
			RunSql(sqlQuery);
		}

		/// <summary>
		/// Returns the Group permissions read from a SQL server database
		/// </summary>
		/// <param name="groupId"></param>
		/// <returns></returns>
		public Permission[] GetGroupPermissions(int groupId)
		{
			ArrayList permissions = new ArrayList();

			using(IDataReader dataReader = RunSqlReturnDataReader(String.Format("select prm_id from grp_prm where grp_id={0}", groupId)))
			{
				while(dataReader.Read())
				{
					Permission permission = (Permission)dataReader.GetInt32(dataReader.GetOrdinal("prm_id"));

					permissions.Add(permission);
				}

				dataReader.Close();
			}

			return (Permission[])permissions.ToArray(typeof(Permission));
		}

		public PermissionStorageView[] GetGroupPermissionStorageView(int groupId)
		{
			return PermissionStorageView.GetPermissionStorageView(GetGroupPermissions(groupId));
		}

		public void DeleteGroup(GroupStorageView groupToDelete)
		{
			if (groupToDelete == null)
			{
				return;
			}

			string sqlQuery;
			
			sqlQuery = String.Format("DELETE FROM usr WHERE grp_id='{0}'; DELETE FROM grp WHERE grp_id='{0}';", 
				groupToDelete.GroupId);
			
			logger.Debug(String.Format("Deleting group {0} and all users associated with it.", groupToDelete.GroupId));

			RunSql(sqlQuery);
		}

		public UserStorageView[] GetGroupUsers(int groupId)
		{
			ArrayList userList = new ArrayList();

			using(IDataReader dataReader = RunSqlReturnDataReader(String.Format("select usr_name, password, grp_id, is_system from usr where grp_id={0}", groupId)))
			{
				while(dataReader.Read())
				{
					string username = dataReader.GetString(dataReader.GetOrdinal("usr_name"));
					string password = dataReader.GetString(dataReader.GetOrdinal("password"));
					bool isSystem = false;

					if (!dataReader.IsDBNull(dataReader.GetOrdinal("is_system")))
					{
						isSystem = dataReader.GetBoolean(dataReader.GetOrdinal("is_system"));
					}

					UserStorageView user = new UserStorageView(username, password, groupId);
					user.IsSystem = isSystem;
					userList.Add(user);
				}

				dataReader.Close();
			}

			return (UserStorageView[])userList.ToArray(typeof(UserStorageView));
		}


		/// <summary>
		/// Check if a permisson is set.
		/// </summary>
		/// <param name="sc">Security credentials to use in the check.</param>
		/// <param name="perm">Permission to check for</param>
		/// <returns>true if the permission is set, false otherwise</returns>
		public bool CheckPermission(SecurityCredentials sc, Permission perm)
		{
			string query = String.Format("select count(*) as permitted from usr inner join grp on grp.grp_id = usr.grp_id inner join grp_prm on grp_prm.grp_id = grp.grp_id inner join prm on prm.prm_id = grp_prm.prm_id where usr.usr_name = '{0}' and prm.prm_id >= {1}", 
				Utils.MakeSqlSafe(sc.Username),
				(int)perm);

			return Convert.ToBoolean(RunSqlReturnScalar(query));
		}

		public string AddExecutor(ExecutorStorageView executor)
		{
			if (executor == null)
			{
				return null;
			}

			string executorId;
			if (executor.ExecutorId == null)
			{
				executorId = Guid.NewGuid().ToString();
			}
			else
			{
				executorId = executor.ExecutorId;
			}

			RunSql(String.Format("insert into executor(executor_id, is_dedicated, is_connected, usr_name) values ('{0}', {1}, {2}, '{3}')",
				executorId,
				Convert.ToInt16(executor.Dedicated),
				Convert.ToInt16(executor.Connected),
				Utils.MakeSqlSafe(executor.Username)
				));

			UpdateExecutorPingTime(executorId, executor.PingTime);

			UpdateExecutorHostAddress(executorId, executor.HostName, executor.Port);

			UpdateExecutorCpuUsage(executorId, executor.MaxCpu, executor.CpuUsage, executor.AvailableCpu, executor.TotalCpuUsage);

			UpdateExecutorAdditionalInformation(executorId, executor.MaxMemory, executor.MaxDisk, executor.NumberOfCpu, executor.OS, executor.Architecture);

			return executorId;
		}

		protected void UpdateExecutorPingTime(string executorId, DateTime pingTime)
		{
			IDataParameter dateTimeParameter = GetParameter(DatabaseParameterDecoration() + "ping_time", pingTime, DbType.DateTime);
			
			if (pingTime != DateTime.MinValue)
			{
				RunSql(String.Format("update executor set ping_time={0}ping_time where executor_id='{1}'", 
					DatabaseParameterDecoration(),
					executorId), 
					dateTimeParameter);
			}
			else
			{
				RunSql(String.Format("update executor set ping_time=null where executor_id='{0}'", executorId));
			}
		}

		protected void UpdateExecutorHostAddress(string executorId, string hostName, int port)
		{
			RunSql(String.Format("update executor set host='{1}', port={2} where executor_id='{0}'",
				executorId,
				Utils.MakeSqlSafe(hostName),
				port
				));
		}

		protected void UpdateExecutorCpuUsage(string executorId, int maxCpu, int cpuUsage, int availableCpu, float totalCpuUsage)
		{
			IDataParameter totalCpuUsageParameter = GetParameter(DatabaseParameterDecoration() + "cpu_totaluse", totalCpuUsage, DbType.Double);

			RunSql(String.Format("update executor set cpu_max={2}, cpu_usage={3}, cpu_avail={4}, cpu_totalusage={0}cpu_totaluse where executor_id='{1}'",
				DatabaseParameterDecoration(),
				executorId,
				maxCpu,
				cpuUsage,
				availableCpu,
				totalCpuUsage
				),
				totalCpuUsageParameter);
		}

		protected void UpdateExecutorDetails(string executorId, bool dedicated, bool connected, string userName)
		{
			RunSql(String.Format("update executor set is_dedicated={1}, is_connected={2}, usr_name='{3}' where executor_id='{0}'",
				executorId,
				Convert.ToInt16(dedicated),
				Convert.ToInt16(connected),
				Utils.MakeSqlSafe(userName)
				));
		}
		
		protected void UpdateExecutorAdditionalInformation(string executorId, float maxMemory, float maxDisk, int numberOfCpu, string os, string architecture)
		{
			IDataParameter maxMemoryParameter = GetParameter(DatabaseParameterDecoration() + "max_memory", maxMemory, DbType.Double);
			IDataParameter maxDiskParameter = GetParameter(DatabaseParameterDecoration() + "max_disk", maxDisk, DbType.Double);

			RunSql(String.Format("update executor set mem_max = {0}max_memory, disk_max = {0}max_disk, num_cpus = {4}, os = '{5}', arch = '{6}' where executor_id='{1}'",
				DatabaseParameterDecoration(),
				executorId,
				maxMemory, 
				maxDisk, 
				numberOfCpu, 
				Utils.MakeSqlSafe(os), 
				Utils.MakeSqlSafe(architecture)
				),
				maxMemoryParameter, maxDiskParameter);
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

        public void DeleteExecutor(ExecutorStorageView executorToDelete)
        {
            if (executorToDelete == null)
            {
                return;
            }

            string sqlQuery;

            sqlQuery = string.Format("DELETE FROM executor WHERE executor_id='{0}'",
                executorToDelete.ExecutorId);

            logger.Debug(String.Format("Deleting executor {0}.", executorToDelete.ExecutorId));

            RunSql(sqlQuery);
        }

		public ExecutorStorageView[] GetExecutors()
		{
			return GetExecutors(TriStateBoolean.Undefined);
		}

		public ExecutorStorageView[] GetExecutors(TriStateBoolean dedicated)
		{
			return GetExecutors(dedicated, TriStateBoolean.Undefined);
		}

		public ExecutorStorageView[] GetExecutors(TriStateBoolean dedicated, TriStateBoolean connected)
		{
			StringBuilder query = new StringBuilder();
			bool whereSet = false;

			query.Append("select executor_id, is_dedicated, is_connected, ping_time, host, port, usr_name, cpu_max, cpu_usage, cpu_avail, cpu_totalusage, mem_max, disk_max, num_cpus, os, arch from executor");
			
			if(dedicated != TriStateBoolean.Undefined)
			{
				if (!whereSet)
				{
					query.Append(" where ");
					whereSet = true;
				}
				else
				{
					query.Append(" and ");
				}

				query.AppendFormat(" is_dedicated = {0}", dedicated == TriStateBoolean.True ? 1 : 0);
			}

			if(connected != TriStateBoolean.Undefined)
			{
				if (!whereSet)
				{
					query.Append(" where ");
					whereSet = true;
				}
				else
				{
					query.Append(" and ");
				}

				query.AppendFormat(" is_connected = {0}", connected == TriStateBoolean.True ? 1 : 0);
			}
		
			using(IDataReader dataReader = RunSqlReturnDataReader(query.ToString()))
			{
				return DecodeExecutorFromDataReader(dataReader);
			}
		}

		public ExecutorStorageView GetExecutor(string executorId)
		{
			if (executorId == null)
			{
				return null;
			}

			using(IDataReader dataReader = RunSqlReturnDataReader(String.Format("select executor_id, is_dedicated, is_connected, ping_time, host, port, usr_name, cpu_max, cpu_usage, cpu_avail, cpu_totalusage, mem_max, disk_max, num_cpus, os, arch from executor where executor_id='{0}'",
					  executorId)))
			{
				ExecutorStorageView[] executors = DecodeExecutorFromDataReader(dataReader);

				if (executors == null || executors.Length == 0)
				{
					dataReader.Close();
					return null;
				}
				else
				{
					dataReader.Close();
					return executors[0];
				}

			}
		}

		private ExecutorStorageView[] DecodeExecutorFromDataReader(IDataReader dataReader)
		{
			ArrayList executors = new ArrayList();

			using(dataReader)
			{
				while(dataReader.Read())
				{
					// in SQL the executor ID is stored as a GUID so we use GetValue instead of GetString in order to maximize compatibility with other databases
					string executorId = dataReader.GetValue(dataReader.GetOrdinal("executor_id")).ToString(); 

					bool dedicated = dataReader.GetBoolean(dataReader.GetOrdinal("is_dedicated"));
					bool connected = dataReader.GetBoolean(dataReader.GetOrdinal("is_connected"));
					DateTime pingTime = GetDateTime(dataReader, "ping_time");
					string hostname = dataReader.GetString(dataReader.GetOrdinal("host"));
					int port = dataReader.IsDBNull(dataReader.GetOrdinal("port")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("port"));
					string username = dataReader.GetString(dataReader.GetOrdinal("usr_name"));
					int maxCpu = dataReader.IsDBNull(dataReader.GetOrdinal("cpu_max")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("cpu_max"));
					int cpuUsage = dataReader.IsDBNull(dataReader.GetOrdinal("cpu_usage")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("cpu_usage"));
					int availableCpu = dataReader.IsDBNull(dataReader.GetOrdinal("cpu_avail")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("cpu_avail"));
					float totalCpuUsage = dataReader.IsDBNull(dataReader.GetOrdinal("cpu_totalusage")) ? 0 : (float)dataReader.GetDouble(dataReader.GetOrdinal("cpu_totalusage"));

					float maxMemory = dataReader.IsDBNull(dataReader.GetOrdinal("mem_max")) ? 0 : (float)dataReader.GetDouble(dataReader.GetOrdinal("mem_max"));;
					float maxDisk = dataReader.IsDBNull(dataReader.GetOrdinal("disk_max")) ? 0 : (float)dataReader.GetDouble(dataReader.GetOrdinal("disk_max"));
					int numberOfCpu = dataReader.IsDBNull(dataReader.GetOrdinal("num_cpus")) ? 0 : dataReader.GetInt32(dataReader.GetOrdinal("num_cpus"));
					string os = dataReader.IsDBNull(dataReader.GetOrdinal("os")) ? "" : dataReader.GetString(dataReader.GetOrdinal("os"));
					string architecture = dataReader.IsDBNull(dataReader.GetOrdinal("arch")) ? "" : dataReader.GetString(dataReader.GetOrdinal("arch"));

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
						totalCpuUsage,
						maxMemory,
						maxDisk,
						numberOfCpu,
						os,
						architecture
						);

					executors.Add(executor);
				}

				dataReader.Close();
			}

			return (ExecutorStorageView[])executors.ToArray(typeof(ExecutorStorageView));
		}

		public string AddApplication(ApplicationStorageView application)
		{
			if (application == null)
			{
				return null;
			}

			string applicationId = Guid.NewGuid().ToString();

            IDataParameter timeCreatedParameter = GetParameter(DatabaseParameterDecoration() + "time_created", application.TimeCreated, DbType.DateTime);
            if (!application.TimeCreatedSet)
            {
                timeCreatedParameter.Value = DBNull.Value;
            }

            IDataParameter timeCompletedParameter = GetParameter(DatabaseParameterDecoration() + "time_completed", application.TimeCompleted, DbType.DateTime);
            if (!application.TimeCompletedSet)
            {
                timeCompletedParameter.Value = DBNull.Value;
            }

            RunSql(String.Format("insert into application(application_id, state, time_created, time_completed, is_primary, usr_name) values ('{1}', {2}, {0}time_created,{0}time_completed, {3}, '{4}')",
				DatabaseParameterDecoration(),
				applicationId,
				(int)application.State,
				Convert.ToInt16(application.Primary),
				Utils.MakeSqlSafe(application.Username)
				),
                timeCreatedParameter,
                timeCompletedParameter);

            application.ApplicationId = applicationId;

            UpdateApplicationName(application);

			return applicationId;
		}

        private void UpdateApplicationName(ApplicationStorageView application)
        {
            RunSql(String.Format("update application set application_name='{2}' where application_id = '{1}'",
                DatabaseParameterDecoration(),
                application.ApplicationId,
                Utils.MakeSqlSafe(application.ApplicationName)
                ));
        }

		public void UpdateApplication(ApplicationStorageView updatedApplication)
		{
			if (updatedApplication == null || updatedApplication.ApplicationId == null || updatedApplication.ApplicationId.Length == 0)
			{
				return;
			}

			IDataParameter timeCreatedParameter = GetParameter(DatabaseParameterDecoration() + "time_created", updatedApplication.TimeCreated, DbType.DateTime);
            if (!updatedApplication.TimeCreatedSet)
            {
                timeCreatedParameter.Value = DBNull.Value;
            }

            IDataParameter timeCompletedParameter = GetParameter(DatabaseParameterDecoration() + "time_completed", updatedApplication.TimeCompleted, DbType.DateTime);
            if (!updatedApplication.TimeCompletedSet)
            {
                timeCompletedParameter.Value = DBNull.Value;
            }

            RunSql(String.Format("update application set state = {2}, time_created = {0}time_created, time_completed = {0}time_completed, is_primary = {3}, usr_name = '{4}' where application_id = '{1}'",
				DatabaseParameterDecoration(),
				updatedApplication.ApplicationId,
				(int)updatedApplication.State,
				Convert.ToInt16(updatedApplication.Primary),
                Utils.MakeSqlSafe(updatedApplication.Username)
                ),
                timeCreatedParameter,
                timeCompletedParameter);

            UpdateApplicationName(updatedApplication);
		}

		public ApplicationStorageView[] GetApplications()
		{
			return GetApplications(false);
		}

		public ApplicationStorageView[] GetApplications(bool populateThreadCount)
		{
			ArrayList applications = new ArrayList();

			string sql = string.Format("select application_id, state, time_created, is_primary, usr_name, application_name, time_completed from application");

			using(IDataReader dataReader = RunSqlReturnDataReader(sql))
			{
				while(dataReader.Read())
				{
					// in SQL the application ID is stored as a GUID so we use GetValue instead of GetString in order to maximize compatibility with other databases
					string applicationId = dataReader.GetValue(dataReader.GetOrdinal("application_id")).ToString(); 

					ApplicationState state = (ApplicationState)dataReader.GetInt32(dataReader.GetOrdinal("state"));
					DateTime timeCreated = GetDateTime(dataReader, "time_created");
					bool primary = dataReader.GetBoolean(dataReader.GetOrdinal("is_primary"));
					string username = dataReader.GetString(dataReader.GetOrdinal("usr_name"));

					ApplicationStorageView application = new ApplicationStorageView(
						applicationId,
						state,
						timeCreated,
						primary,
						username
						);

                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("application_name")))
                    {
                        application.ApplicationName = dataReader.GetString(dataReader.GetOrdinal("application_name"));
                    }
                    else
                    {
                        application.ApplicationName = String.Empty;
                    }

					application.TimeCompleted = GetDateTime(dataReader, "time_completed");

					if (populateThreadCount)
					{
						int totalThreads;
						int unfinishedThreads;

						GetApplicationThreadCount(application.ApplicationId, out totalThreads, out unfinishedThreads);

						application.TotalThreads = totalThreads;
						application.UnfinishedThreads = unfinishedThreads;
					}

					applications.Add(application);
				}

				dataReader.Close();
			}

			return (ApplicationStorageView[])applications.ToArray(typeof(ApplicationStorageView));
		}

		public ApplicationStorageView[] GetApplications(string userName, bool populateThreadCount)
		{
			ArrayList applications = new ArrayList();

			string sql = string.Format("select application_id, state, time_created, is_primary, usr_name, application_name, time_completed from application where usr_name = '{0}'", Utils.MakeSqlSafe(userName));

			using(IDataReader dataReader = RunSqlReturnDataReader(sql))
			{
				while(dataReader.Read())
				{
					// in SQL the application ID is stored as a GUID so we use GetValue instead of GetString in order to maximize compatibility with other databases
					string applicationId = dataReader.GetValue(dataReader.GetOrdinal("application_id")).ToString(); 

					ApplicationState state = (ApplicationState)dataReader.GetInt32(dataReader.GetOrdinal("state"));
					DateTime timeCreated = GetDateTime(dataReader, "time_created");
					bool primary = dataReader.GetBoolean(dataReader.GetOrdinal("is_primary"));
					string username = dataReader.GetString(dataReader.GetOrdinal("usr_name"));

					ApplicationStorageView application = new ApplicationStorageView(
						applicationId,
						state,
						timeCreated,
						primary,
						username
						);

					application.ApplicationName = dataReader.IsDBNull(dataReader.GetOrdinal("application_name")) ? "" : dataReader.GetString(dataReader.GetOrdinal("application_name"));
					application.TimeCompleted = GetDateTime(dataReader, "time_completed");

					if (populateThreadCount)
					{
						int totalThreads;
						int unfinishedThreads;

						GetApplicationThreadCount(application.ApplicationId, out totalThreads, out unfinishedThreads);

						application.TotalThreads = totalThreads;
						application.UnfinishedThreads = unfinishedThreads;
					}

					applications.Add(application);
				}
			
				dataReader.Close();
			}

			return (ApplicationStorageView[])applications.ToArray(typeof(ApplicationStorageView));
		}

		public ApplicationStorageView GetApplication(string applicationId)
		{
            using (IDataReader dataReader = RunSqlReturnDataReader(String.Format("select application_id, application_name, state, time_created, is_primary, usr_name, time_completed from application where application_id='{0}'", applicationId)))
			{
				if(dataReader.Read())
				{
					ApplicationState state = (ApplicationState)dataReader.GetInt32(dataReader.GetOrdinal("state"));
                    DateTime timeCreated = GetDateTime(dataReader, "time_created");
					bool primary = dataReader.GetBoolean(dataReader.GetOrdinal("is_primary"));
					string username = dataReader.GetString(dataReader.GetOrdinal("usr_name"));

					ApplicationStorageView application = new ApplicationStorageView(
						applicationId,
						state,
						timeCreated,
						primary,
						username
						);

                    application.ApplicationName = dataReader.IsDBNull(dataReader.GetOrdinal("application_name")) ? "" : dataReader.GetString(dataReader.GetOrdinal("application_name")); 
                    application.TimeCompleted = GetDateTime(dataReader, "time_completed");

					dataReader.Close();
					return application;
				}
				else
				{
					dataReader.Close();
					return null;
				}
			
			}
		}

		public void DeleteApplication(ApplicationStorageView applicationToDelete)
		{
			if (applicationToDelete == null)
			{
				return;
			}

			string sqlQuery;
			
			sqlQuery = string.Format("DELETE FROM application WHERE application_id='{0}'; DELETE FROM thread WHERE application_id='{0}';",
				applicationToDelete.ApplicationId);
			
			logger.Debug(String.Format("Deleting application {0} and all threads associated with it.", applicationToDelete.ApplicationId));

			RunSql(sqlQuery);
		}

		public int AddThread(ThreadStorageView thread)
		{
			if (thread == null)
			{
				return -1;
			}

			IDataParameter timeStartedParameter = GetParameter(DatabaseParameterDecoration() + "time_started", thread.TimeStarted, DbType.DateTime);
			if (!thread.TimeStartedSet)
			{
				timeStartedParameter.Value = DBNull.Value;
			}

			IDataParameter timeFinishedParameter = GetParameter(DatabaseParameterDecoration() + "time_finished", thread.TimeFinished, DbType.DateTime);
			if (!thread.TimeFinishedSet)
			{
				timeFinishedParameter.Value = DBNull.Value;
			}

			IDataParameter executorIdParameter;
			
			if (thread.ExecutorId != null)
			{
				executorIdParameter = GetParameter(DatabaseParameterDecoration() + "executor_id", thread.ExecutorId, DbType.Guid);
			}
			else
			{
				executorIdParameter = GetParameter(DatabaseParameterDecoration() + "executor_id", DBNull.Value, DbType.Guid);
			}

			object threadIdObject = RunSqlReturnScalar(String.Format("insert into thread(application_id, executor_id, thread_id, state, time_started, time_finished, priority, failed) values ('{1}', {0}executor_id, {3}, {4}, {0}time_started, {0}time_finished, {5}, {6})",
				DatabaseParameterDecoration(),
				thread.ApplicationId,
				thread.ExecutorId,
				thread.ThreadId,
				(int)thread.State,
				thread.Priority,
				Convert.ToInt16(thread.Failed)
				), 
				executorIdParameter, timeStartedParameter, timeFinishedParameter);

			return Convert.ToInt32(threadIdObject);
		}

		public void UpdateThread(ThreadStorageView updatedThread)
		{
			if (updatedThread == null)
			{
				return;
			}

			IDataParameter timeStartedParameter = GetParameter(DatabaseParameterDecoration() + "time_started", updatedThread.TimeStarted, DbType.DateTime);
			if (!updatedThread.TimeStartedSet)
			{
				timeStartedParameter.Value = DBNull.Value;
			}

			IDataParameter timeFinishedParameter = GetParameter(DatabaseParameterDecoration() + "time_finished", updatedThread.TimeFinished, DbType.DateTime);
			if (!updatedThread.TimeFinishedSet)
			{
				timeFinishedParameter.Value = DBNull.Value;
			}

			IDataParameter executorIdParameter;
			
			if (updatedThread.ExecutorId != null)
			{
				executorIdParameter = GetParameter(DatabaseParameterDecoration() + "executor_id", updatedThread.ExecutorId, DbType.Guid);
			}
			else
			{
				executorIdParameter = GetParameter(DatabaseParameterDecoration() + "executor_id", DBNull.Value, DbType.Guid);
			}

			RunSql(String.Format("update thread set application_id = '{2}', executor_id = {0}executor_id, thread_id = {4}, state = {5}, time_started = {0}time_started, time_finished = {0}time_finished, priority = {6}, failed = {7} where internal_thread_id = {1}",
				DatabaseParameterDecoration(),
				updatedThread.InternalThreadId,
				updatedThread.ApplicationId,
				updatedThread.ExecutorId,
				updatedThread.ThreadId,
				(int)updatedThread.State,
				updatedThread.Priority,
				Convert.ToInt16(updatedThread.Failed)
				), 
				executorIdParameter, timeStartedParameter, timeFinishedParameter);
		}

		public ThreadStorageView GetThread(string applicationId, int threadId)
		{
			StringBuilder query = new StringBuilder();

			query.AppendFormat("select internal_thread_id, application_id, executor_id, thread_id, state, time_started, time_finished, priority, failed from thread where application_id='{0}' and thread_id={1}",
				applicationId,
				threadId);

			using(IDataReader dataReader = RunSqlReturnDataReader(query.ToString()))
			{
				ThreadStorageView[] threads = DecodeThreadFromDataReader(dataReader);

				if (threads.Length > 0)
				{
					dataReader.Close();
					return threads[0];
				}
				else
				{
					dataReader.Close();
					return null;
				}
			}
		}

		public ThreadStorageView[] GetThreads(ApplicationState appState, params ThreadState[] findStates)
		{
			StringBuilder query = new StringBuilder();

			query.AppendFormat("select internal_thread_id, thread.application_id, executor_id, thread_id, thread.state, time_started, time_finished, priority, failed from thread inner join application on (thread.application_id = application.application_id) where application.state={0}", 
				(int)appState);

			if (findStates != null && findStates.Length > 0)
			{
				query.Append(" and ");

				query.Append(" thread.state in ");
				query.Append("(");

				for(int index = 0; index < findStates.Length; index++)
				{
					ThreadState state = findStates[index];

					if (index > 0)
					{
						query.Append(",");
					}
					query.Append((int)state);
				}

				query.Append(")");
			}
			

			using(IDataReader dataReader = RunSqlReturnDataReader(query.ToString()))
			{
				return DecodeThreadFromDataReader(dataReader);
			}
		}

		public ThreadStorageView[] GetThreads(params ThreadState[] findStates)
		{
			return GetThreads(null, findStates);
		}

		public ThreadStorageView[] GetThreads(string findApplicationId, params ThreadState[] findStates)
		{
			StringBuilder query = new StringBuilder();

			query.AppendFormat("select internal_thread_id, application_id, executor_id, thread_id, state, time_started, time_finished, priority, failed from thread");

			if (findApplicationId != null || (findStates != null && findStates.Length > 0))
			{
				query.Append(" where ");
			}

			// build the query based on the passed in variables
			if (findApplicationId != null)
			{
				query.AppendFormat("application_id='{0}'",
					findApplicationId);

			}

			if (findStates != null && findStates.Length > 0)
			{
				if (findApplicationId != null)
				{
					query.Append(" and ");
				}

				query.Append(" state in ");
				query.Append("(");

				for(int index = 0; index < findStates.Length; index++)
				{
					ThreadState state = findStates[index];

					if (index > 0)
					{
						query.Append(",");
					}
					query.Append((int)state);
				}

				query.Append(")");
			}
			

			using(IDataReader dataReader = RunSqlReturnDataReader(query.ToString()))
			{
				return DecodeThreadFromDataReader(dataReader);
			}
		}

		public ThreadStorageView[] GetExecutorThreads(string executorId, params ThreadState[] findStates)
		{
			StringBuilder query = new StringBuilder();

			query.AppendFormat("select internal_thread_id, application_id, executor_id, thread_id, state, time_started, time_finished, priority, failed from thread");

			// build the query based on the passed in variables
			query.AppendFormat(" where executor_id='{0}'",
				executorId);

			if (findStates != null && findStates.Length > 0)
			{
				query.Append(" and state in ");
				query.Append("(");

				for(int index = 0; index < findStates.Length; index++)
				{
					ThreadState state = findStates[index];

					if (index > 0)
					{
						query.Append(",");
					}
					query.Append((int)state);
				}

				query.Append(")");
			}

			using(IDataReader dataReader = RunSqlReturnDataReader(query.ToString()))
			{
				return DecodeThreadFromDataReader(dataReader);
			}
		}

		public ThreadStorageView[] GetExecutorThreads(bool dedicatedExecutor, params ThreadState[] findStates)
		{
			StringBuilder query = new StringBuilder();

			query.AppendFormat("select internal_thread_id, application_id, thread.executor_id, thread_id, state, time_started, time_finished, priority, failed from thread inner join executor on (thread.executor_id = executor.executor_id) where is_dedicated = {0}",
				dedicatedExecutor ? "1" : "0");

			if (findStates != null && findStates.Length > 0)
			{
				query.Append(" and state in ");
				query.Append("(");

				for(int index = 0; index < findStates.Length; index++)
				{
					ThreadState state = findStates[index];

					if (index > 0)
					{
						query.Append(",");
					}
					query.Append((int)state);
				}

				query.Append(")");
			}

			using(IDataReader dataReader = RunSqlReturnDataReader(query.ToString()))
			{
				return DecodeThreadFromDataReader(dataReader);
			}

		}

		public ThreadStorageView[] GetExecutorThreads(bool dedicatedExecutor, bool connectedExecutor, params ThreadState[] findStates)
		{
			StringBuilder query = new StringBuilder();

			query.AppendFormat("select internal_thread_id, application_id, thread.executor_id, thread_id, state, time_started, time_finished, priority, failed from thread inner join executor on (thread.executor_id = executor.executor_id) where is_dedicated = {0} and is_connected = {1}",
				dedicatedExecutor ? "1" : "0",
				connectedExecutor ? "1" : "0");

			if (findStates != null && findStates.Length > 0)
			{
				query.Append(" and state in ");
				query.Append("(");

				for(int index = 0; index < findStates.Length; index++)
				{
					ThreadState state = findStates[index];

					if (index > 0)
					{
						query.Append(",");
					}
					query.Append((int)state);
				}

				query.Append(")");
			}

			using(IDataReader dataReader = RunSqlReturnDataReader(query.ToString()))
			{
				return DecodeThreadFromDataReader(dataReader);
			}
		}

		private ThreadStorageView[] DecodeThreadFromDataReader(IDataReader dataReader)
		{
			if (dataReader == null)
			{
				return new ThreadStorageView[0];
			}

			ArrayList threads = new ArrayList();

			using(dataReader)
			{
				while(dataReader.Read())
				{
					int internalThreadId = dataReader.GetInt32(dataReader.GetOrdinal("internal_thread_id"));

					// in SQL the application ID is stored as a GUID so we use GetValue instead of GetString in order to maximize compatibility with other databases
					string applicationId = dataReader.GetValue(dataReader.GetOrdinal("application_id")).ToString(); 
					string executorId = dataReader.IsDBNull(dataReader.GetOrdinal("executor_id")) ? null : dataReader.GetValue(dataReader.GetOrdinal("executor_id")).ToString();

					int threadId = dataReader.GetInt32(dataReader.GetOrdinal("thread_id"));
					ThreadState state = (ThreadState)dataReader.GetInt32(dataReader.GetOrdinal("state"));

					DateTime timeStarted = GetDateTime(dataReader, "time_started");
					DateTime timeFinished = GetDateTime(dataReader, "time_finished");

					int priority = dataReader.GetInt32(dataReader.GetOrdinal("priority"));
					bool failed = dataReader.IsDBNull(dataReader.GetOrdinal("failed")) ? false : dataReader.GetBoolean(dataReader.GetOrdinal("failed"));

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

				dataReader.Close();
			}

			return (ThreadStorageView[])threads.ToArray(typeof(ThreadStorageView));  
		}


		public void GetApplicationThreadCount(string applicationId, out int totalThreads, out int unfinishedThreads)
		{
			totalThreads = unfinishedThreads = 0;

			using(IDataReader dataReader = RunSqlReturnDataReader(String.Format("select state from thread where application_id = '{0}'",
					  applicationId)))
			{
				while(dataReader.Read())
				{
					int state = dataReader.GetInt32(dataReader.GetOrdinal("state"));

					totalThreads ++;

					if (state == 0 || state == 1 || state == 2)
					{
						unfinishedThreads ++;
					}

				}
			
				dataReader.Close();
			}

		}

		public int GetApplicationThreadCount(string applicationId, ThreadState threadState)
		{
			object threadCount = RunSqlReturnScalar(String.Format("select count(*) from thread where application_id='{0}' and state = {1}",
				applicationId,
				(int)threadState));

			return Convert.ToInt32(threadCount);
		}

		public int GetExecutorThreadCount(string executorId, params ThreadState[] threadState)
		{
			if (executorId == null || threadState == null || threadState.Length == 0)
			{
				return 0;
			}
			
			StringBuilder query = new StringBuilder();

			query.AppendFormat("select count(*) from thread where executor_id='{0}' and state in ", 
				executorId);
			
			query.Append("(");

			for(int index = 0; index < threadState.Length; index++)
			{
				ThreadState state = threadState[index];

				if (index > 0)
				{
					query.Append(",");
				}
				query.Append((int)state);
			}

			query.Append(")");

			object threadCount = RunSqlReturnScalar(query.ToString());

			return Convert.ToInt32(threadCount);

		}

		public void DeleteThread(ThreadStorageView threadToDelete)
		{
			if (threadToDelete == null)
			{
				return;
			}

			string sqlQuery;
			
			sqlQuery = string.Format("DELETE FROM thread WHERE application_id='{1}' and thread_id={0};", 
				threadToDelete.ThreadId,
				threadToDelete.ApplicationId);
			
			logger.Debug(String.Format("Deleting thread {0} in application {1}.", threadToDelete.ThreadId, threadToDelete.ApplicationId));

			RunSql(sqlQuery);
		}

		#endregion

		#region Generic implementation for database-specific objects

		//default is using OleDbConnections.
		protected virtual IDbConnection GetConnection(string connectionString)
		{
			return new OleDbConnection(connectionString);
		}

		//default uses OleDbCommands.
		protected virtual IDbCommand GetCommand()
		{
			return new OleDbCommand();
		}

		protected virtual IDataAdapter GetDataAdapter(IDbCommand command)
		{
			return new OleDbDataAdapter(command as OleDbCommand);
		}

		protected virtual IDataParameter GetParameter(string name, object paramValue, DbType datatype)
		{
			object value = paramValue;
			if (datatype == DbType.Guid)
			{
				value = new Guid(paramValue.ToString());
			}
			OleDbParameter param = new OleDbParameter(name, value);
			param.DbType = datatype;
			return param;
		}

		#endregion

		#region Generic database manipulation routines

		protected virtual string DatabaseParameterDecoration()
		{
			return "@";
		}

		/// <summary>
		/// Run a stored procedure and return a data reader.
		/// The caller is responsible for closing the database connection.
		/// </summary>
		/// <param name="storedProcedure"></param>
		/// <returns></returns>
		protected IDataReader RunSpReturnDataReader(string storedProcedure)
		{
			IDbConnection connection = GetConnection(ConnectionString);
			IDbCommand command = GetCommand();
			command.Connection = connection;
			command.CommandText = storedProcedure;
			command.CommandType = CommandType.StoredProcedure;
		
			connection.Open();

			// the connection must remain open until the reader is closed
			return command.ExecuteReader(CommandBehavior.CloseConnection);
		}

		protected IDataReader RunSqlReturnDataReader(string sqlQuery)
		{
			IDbConnection connection = GetConnection(ConnectionString);
			IDbCommand command = GetCommand();

			command.Connection = connection;
			command.CommandText = sqlQuery;
			command.CommandType = CommandType.Text;
		
			connection.Open();

			// the connection must remain open until the reader is closed
            
            
			return command.ExecuteReader(CommandBehavior.CloseConnection);
		}

		/// <summary>
		/// Run a stored procedure and return a scalar.
		/// </summary>
		/// <param name="storedProcedure"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		protected object RunSpReturnScalar(string storedProcedure, params IDataParameter[] parameters)
		{
			using(IDbConnection connection = GetConnection(ConnectionString))
			{
				IDbCommand command = GetCommand();

				command.Connection = connection;
				command.CommandText = storedProcedure;
				command.CommandType = CommandType.StoredProcedure;

				foreach (IDataParameter parameter in parameters)
				{
					command.Parameters.Add(parameter);
				}
		
				connection.Open();

				object result = command.ExecuteScalar();

				return result;
			}
		}

		protected void RunSql(string sqlQuery)
		{
			RunSql(sqlQuery, null);
		}

		protected void RunSql(string sqlQuery, params IDataParameter[] parameters)
		{
			try
			{
				using(IDbConnection connection = GetConnection(ConnectionString))
				{
					IDbCommand command = GetCommand();

					command.Connection = connection;
					command.CommandText = sqlQuery;
					command.CommandType = CommandType.Text;

					if (parameters != null)
					{
						foreach(IDataParameter parameter in parameters)
						{
							command.Parameters.Add(parameter);
						}
					}
		
					connection.Open();

					command.ExecuteNonQuery();

					connection.Close();
				}
			}
			catch (Exception ex)
			{
				logger.Debug("Exception RunSql:"+sqlQuery, ex);
				throw ex;
			}
		}

		protected object RunSqlReturnScalar(string sqlQuery)
		{
			return RunSqlReturnScalar(sqlQuery, null);
		}

		protected object RunSqlReturnScalar(string sqlQuery, params IDataParameter[] parameters)
		{
			using(IDbConnection connection = GetConnection(ConnectionString))
			{
				IDbCommand command = GetCommand();

				command.Connection = connection;
				command.CommandText = sqlQuery;
				command.CommandType = CommandType.Text;

				if (parameters != null)
				{
					foreach(IDataParameter parameter in parameters)
					{
						command.Parameters.Add(parameter);
					}
				}
		
				connection.Open();

				object result = command.ExecuteScalar();
				
				connection.Close();

				return result;
			}
		}

        /// <summary>
        /// Gets the date time from the given data reader checking for db null. If date time is db null then it returns DateTime.MinValue.
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <param name="strColumnName">column name</param>
        /// <returns>date time</returns>
        private DateTime GetDateTime(IDataReader dataReader, string columnName)
        {
            return GetDateTime(dataReader, columnName, DateTime.MinValue);
        }

        /// <summary>
        /// Gets the date time from the given data reader checking for db null. If date time is db null then it returns the given default date time.
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <param name="columnName">column name</param>
        /// <param name="defaultDateTime">default date time</param>
        /// <returns>date time</returns>
        private DateTime GetDateTime(IDataReader dataReader, string columnName, DateTime defaultDateTime)
        {
            int nOrdinal = dataReader.GetOrdinal(columnName);

            if (dataReader.IsDBNull(nOrdinal))
            {
                return defaultDateTime;
            }

            return dataReader.GetDateTime(nOrdinal);
        }

		#endregion

		private string GetStringFromEmbededScriptFile(string folder, string scriptFileName)
		{
			Stream inputStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(
				String.Format("Alchemi.Manager.Storage.SetupFiles.{0}.{1}", folder, scriptFileName));

			if (inputStream == null)
			{
				throw new ArgumentException(String.Format("Unable to find script file {1} under folder {0}", folder, scriptFileName));
			}

			using(StreamReader reader = new StreamReader(inputStream))
			{
				string data = reader.ReadToEnd();
				
				reader.Close();

				return data;
			}
		}
	}
}
