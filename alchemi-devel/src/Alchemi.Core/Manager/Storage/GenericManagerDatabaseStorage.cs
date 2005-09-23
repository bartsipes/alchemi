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
//		protected object RunSpReturnScalar(String storedProcedure, params SqlParameter[] paramArray)
//		{
//			AdpConnection connection = new AdpConnection(m_connectionString);
//			AdpCommand command = new AdpCommand();
//
//			command.Connection = connection;
//			command.CommandText = storedProcedure;
//			command.CommandType = CommandType.StoredProcedure;
//
//			foreach (SqlParameter objParam in paramArray)
//			{
//				AdpParameter param = command.CreateParameter(objParam.ParameterName, objParam.Value);
//				//AdpParameter param = new AdpParameter();
//				
//				param.DbType = DbType.String;
//				param.Value = objParam.Value;
//				param.ParameterName = objParam.ParameterName;
//				param.Direction = ParameterDirection.Input;
//				
//				command.Parameters.Add(param);
//			}
//		
//			return command.ExecuteScalar();
//		}

		protected void RunSql(String sqlQuery)
		{
			AdpConnection connection = new AdpConnection(m_connectionString);
			AdpCommand command = new AdpCommand();

			command.Connection = connection;
			command.CommandText = sqlQuery;
			command.CommandType = CommandType.Text;
		
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
		public void AddUsers(User[] users)
		{
			if (users == null)
			{
				return;
			}

			foreach (User user in users)
			{
				String sqlQuery;
				
				sqlQuery = String.Format("insert usr values('{0}', '{1}', {2})", 
					user.Username.Replace("'", "''"), 
					user.Password.Replace("'", "''"), 
					user.GroupId);
				
				RunSql(sqlQuery);
			}
		}

		public void UpdateUsers(User[] updates)
		{
			if (updates == null)
			{
				return;
			}

			foreach (User user in updates)
			{
				String sqlQuery;
				
				sqlQuery = String.Format("update usr set password='{1}', grp_id={2} where usr_name='{0}'", 
					user.Username.Replace("'", "''"), 
					user.Password.Replace("'", "''"), 
					user.GroupId);
				
				RunSql(sqlQuery);
			}
		}

		public User[] GetUserList()
		{
			ArrayList userList = new ArrayList();

			using(AdpDataReader dataReader = RunSqlReturnDataReader("select usr_name, password, grp_id from usr"))
			{
				while(dataReader.Read())
				{
					String username = dataReader.GetString(dataReader.GetOrdinal("usr_name"));
					String password = dataReader.GetString(dataReader.GetOrdinal("password"));
					Int32 groupId = dataReader.GetInt32(dataReader.GetOrdinal("grp_id"));

					User user = new User(username, password, groupId);

					userList.Add(user);
				}
			}

			return (User[])userList.ToArray(typeof(User));
		}

		public void AddGroups(Group[] groups)
		{
			if (groups == null)
			{
				return;
			}

			foreach (Group group in groups)
			{
				String sqlQuery;
				
				sqlQuery = String.Format("insert grp values({0}, '{1}')", 
					group.GroupId,
					group.GroupName.Replace("'", "''"));
				
				RunSql(sqlQuery);
			}
		}
		
		public Group[] GetGroups()
		{
			ArrayList groupList = new ArrayList();

			using(AdpDataReader dataReader = RunSqlReturnDataReader("select grp_id, grp_name from grp"))
			{
				while(dataReader.Read())
				{
					Int32 groupId = dataReader.GetInt32(dataReader.GetOrdinal("grp_id"));
					String groupName = dataReader.GetString(dataReader.GetOrdinal("grp_name"));

					Group group = new Group(groupId, groupName);

					groupList.Add(group);
				}
			}

			return (Group[])groupList.ToArray(typeof(Group));
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

		#endregion

	}
}
