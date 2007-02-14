#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
* Title         :  ManagerStorageFactory.cs
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
using System.Configuration;

using Alchemi.Core.Manager.Storage;

namespace Alchemi.Manager.Storage
{
	/// <summary>
	/// Takes care of choosing the right storage.
	/// </summary>
	public class ManagerStorageFactory
	{
		private static IManagerStorage m_managerStorage = null;
		private static object m_inMemoryManagerStorageLock = new object();

		public ManagerStorageFactory()
		{
		}

		public static IManagerStorage ManagerStorage()
		{
			if (m_managerStorage == null)
			{
				CreateManagerStorage(null);
			}

			return m_managerStorage;
		}

		/// <summary>
		/// Set the manager storage.
		/// Used to control the current storage from the caller.
		/// </summary>
		/// <param name="managerStorage"></param>
		public static void SetManagerStorage(IManagerStorage managerStorage)
		{
			m_managerStorage = managerStorage;
		}

		/// <summary>
		/// Create the right manager storage object based on the storage type in the configuration file.
		/// This will set the default database to the one specified in the configuration file
		/// </summary>
		/// <param name="config"></param>
		/// <returns></returns>
		public static IManagerStorage CreateManagerStorage(Configuration config)
		{
			return CreateManagerStorage(config, true);
		}

		/// <summary>
		/// Create the right manager storage object based on the storage type in the configuration file.
		/// </summary>
		/// <param name="config"></param>
		/// <param name="overwriteDefaultDatabase">Specify if the default catalog is set to the database name in the config file.</param>
		/// <returns></returns>
		public static IManagerStorage CreateManagerStorage(Configuration config, bool overwriteDefaultDatabase)
		{
			Configuration configuration = config;
			
			if (configuration==null)
			{
				configuration = Configuration.GetConfiguration();
			}

			string sqlConnStr;

			switch(configuration.DbType)
			{
					case ManagerStorageEnum.SqlServer:

					if (overwriteDefaultDatabase)
					{ 
						// build sql server configuration string
						sqlConnStr = string.Format(
							"user id={1};password={2};initial catalog={3};data source={0};Connect Timeout={4}; Max Pool Size={5}; Min Pool Size={6}",
							configuration.DbServer,
							configuration.DbUsername,
							configuration.DbPassword,
							configuration.DbName,
                            configuration.DbConnectTimeout,
                            configuration.DbMaxPoolSize,
                            configuration.DbMinPoolSize
							);
					}
					else
					{
						sqlConnStr = string.Format(
							"user id={1};password={2};data source={0};Connect Timeout={3}; Max Pool Size={4}; Min Pool Size={5}",
							configuration.DbServer,
							configuration.DbUsername,
							configuration.DbPassword,
                            configuration.DbConnectTimeout,
                            configuration.DbMaxPoolSize,
                            configuration.DbMinPoolSize
							);
					}

					m_managerStorage = new SqlServerManagerDatabaseStorage(sqlConnStr);
					break;

				case ManagerStorageEnum.MySql:

					if (overwriteDefaultDatabase)
					{ 
						// build sql server configuration string
						sqlConnStr = string.Format(
							"user id={1};password={2};database={3};data source={0};Connect Timeout={4}; Max Pool Size={5}; Min Pool Size={6}",
							configuration.DbServer,
							configuration.DbUsername,
							configuration.DbPassword,
							configuration.DbName,
                            configuration.DbConnectTimeout,
                            configuration.DbMaxPoolSize,
                            configuration.DbMinPoolSize
							);
					}
					else
					{
						sqlConnStr = string.Format(
							"user id={1};password={2};data source={0};Connect Timeout={3}; Max Pool Size={4}; Min Pool Size={5}",
							configuration.DbServer,
							configuration.DbUsername,
							configuration.DbPassword,
                            configuration.DbConnectTimeout,
                            configuration.DbMaxPoolSize,
                            configuration.DbMinPoolSize
							);						
					}

					m_managerStorage = new MySqlManagerDatabaseStorage(sqlConnStr);
					break;
                case ManagerStorageEnum.Postgresql:

                    /// Valid values are:
        /// Server:                     Address/Name of Postgresql Server;
        /// Port:                       Port to connect to;
        /// Protocol:                   Protocol version to use, instead of automatic; Integer 2 or 3;
        /// Database:                   Database name. Defaults to user name if not specified;
        /// User Id:                    User name;
        /// Password:                   Password for clear text authentication;
        /// SSL:                        True or False. Controls whether to attempt a secure connection. Default = False;
        /// Pooling:                    True or False. Controls whether connection pooling is used. Default = True;
        /// MinPoolSize:                Min size of connection pool. Default: 1;
        /// MaxPoolSize:                Max size of connection pool. Default: 20;
        /// Encoding:                   Encoding to be used; Can be ASCII or UNICODE. Default is ASCII. Use UNICODE if you are having problems with accents.
        /// Timeout:                    Time to wait for connection open in seconds. Default is 15.
        /// CommandTimeout:             Time to wait for command to finish execution before throw an exception. In seconds. Default is 20.
        /// Sslmode:                    Mode for ssl connection control. 
        /// ConnectionLifeTime:         Time to wait before closing unused connections in the pool in seconds. Default is 15.
        /// SyncNotification:           Specifies if Npgsql should use synchronous notifications
        //Encoding can be ASCII or UNICODE. If your application uses characters with accents and with default settings it doesn't work, try changing that.
        //Min pool size when specified will make NpgsqlConnection pre allocates this number of connections with the server.
        //Sslmode can be one of the following values: 
        //    Prefer - If it is possible to connect using ssl, it will be used.
        //    Require - If an ssl connection cannot be made, an exception is thrown.
        //    Allow - Not supported yet, just connects without ssl.
        //    Disable - No ssl connection is done.
        //    Default is Disable.

                    if (overwriteDefaultDatabase)
                    {
                        // build configuration string
                        sqlConnStr = string.Format(
                            "user id={1};password={2};database={3};port=5432;server={0};Connect Timeout={4}; Max Pool Size={5}; Min Pool Size={6}",
                            configuration.DbServer,
                            configuration.DbUsername,
                            configuration.DbPassword,
                            configuration.DbName,
                            configuration.DbConnectTimeout,
                            configuration.DbMaxPoolSize,
                            configuration.DbMinPoolSize
                            );
                    }
                    else
                    {
                        sqlConnStr = string.Format(
                            "user id={1};password={2};database=postgres;server={0};port=5432;Connect Timeout={3}; Max Pool Size={4}; Min Pool Size={5}",
                            configuration.DbServer,
                            configuration.DbUsername,
                            configuration.DbPassword,
                            configuration.DbConnectTimeout,
                            configuration.DbMaxPoolSize,
                            configuration.DbMinPoolSize
                            );
                    }

                    m_managerStorage = new PostgresqlManagerDatabaseStorage(sqlConnStr);
                    break;
				case ManagerStorageEnum.InMemory:
					/// in memory storage is volatile so we always return the same object
					if (m_managerStorage == null)
					{
						lock (m_inMemoryManagerStorageLock)
						{
							// make sure that when we got the lock we are still uninitialized
							if (m_managerStorage == null)
							{
								InMemoryManagerStorage inMemoryStorage = new InMemoryManagerStorage();

								// volatile storage, we must initialize the data here
								inMemoryStorage.InitializeStorageData();

								m_managerStorage = inMemoryStorage;
							}
						}
					}

					break;

				default:
                    throw new System.Configuration.ConfigurationErrorsException(
                        string.Format("Unknown storage type: {0}", configuration.DbType));
			}

			return m_managerStorage;
		}

	}
}
