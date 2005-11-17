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

using Alchemi.Core;
using Alchemi.Core.Manager.Storage;

namespace Alchemi.Manager.Storage
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
		/// Create the right manager storage object.
		/// </summary>
		/// <returns></returns>
		public static IManagerStorage CreateManagerStorage(Configuration config)
		{
			// TODO: implement different storages based on the current configuration file
			// TODO: currently everything defaults to SQL Server

			Configuration configuration = config;
			
			if (configuration==null)
			{
				configuration = Configuration.GetConfiguration();
			}

			// build sql server configuration string
			string sqlConnStr = string.Format(
				"user id={1};password={2};initial catalog={3};data source={0};Connect Timeout=5; Max Pool Size=5; Min Pool Size=5",
				configuration.DbServer,
				configuration.DbUsername,
				configuration.DbPassword,
				configuration.DbName
				);

//			String connectionString = String.Format("adpprovider=MsSql;server={0};database={1};User ID={2};Password={3}", 
//				configuration.DbServer, 
//				configuration.DbName, 
//				configuration.DbUsername,
//				configuration.DbPassword); 

			m_managerStorage = new SqlServerManagerDatabaseStorage(sqlConnStr);
			return m_managerStorage;
		}

	}
}
