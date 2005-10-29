#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	MApplicationCollection.cs
* Project		:	Alchemi Core
* Created on	:	2003
* Copyright		:	Copyright © 2005 The University of Melbourne
*					This technology has been developed with the support of 
*					the Australian Research Council and the University of Melbourne
*					research grants as part of the Gridbus Project
*					within GRIDS Laboratory at the University of Melbourne, Australia.
* Author         :  Akshay Luther (akshayl@cs.mu.oz.au) and Rajkumar Buyya (raj@cs.mu.oz.au)
* License        :  GPL
*					This program is free software; you can redistribute it and/or 
*					modify it under the terms of the GNU General Public
*					License as published by the Free Software Foundation;
*					See the GNU General Public License 
*					(http://www.gnu.org/copyleft/gpl.html) for more details.
*
*/ 
#endregion


using System;
using System.IO;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Remoting;
using System.Threading;
using Alchemi.Core;
using Alchemi.Core.Owner;
using Alchemi.Core.Utility;

namespace Alchemi.Core.Manager
{
    /// <summary>
    /// Represents a collection of grid applications. A particular application is referred to by its ID via the indexer.
    /// </summary>
    public class MApplicationCollection
    {
		/// <summary>
		/// Gets the MApplication with the given id
		/// </summary>
        public MApplication this[string applicationId]
        {
            get
            {
                return new MApplication(applicationId);
            }
        }
        
		/// <summary>
		/// Creates a new application
		/// </summary>
		/// <param name="username">the user associated with the application</param>
		/// <returns>Id of the newly created application</returns>
        public string CreateNew(string username)
        {
            string appId = InternalShared.Instance.Database.ExecSql_Scalar("Application_Insert '{0}'", username).ToString();
            this[appId].CreateDataDirectory();
            return appId;
        }

		/// <summary>
		/// Verify if the database is properly set up.
		/// </summary>
		/// <param name="id">application id</param>
		/// <returns>true if the application is set up in the database</returns>
		public bool VerifyApp(string id)
		{
			bool appSetup = true;

			//create directory can be repeated, and wont throw an error even if the dir already exists.
			this[id].CreateDataDirectory();

			object appId = InternalShared.Instance.Database.ExecSql_Scalar("SELECT application_id FROM application WHERE application_id='{0}'", id);
			if (appId==null)
			{
				appSetup = false;
			}
			return appSetup;
		}

		/// <summary>
		/// Gets the list of applications.
		/// </summary>
        public DataSet LiveList
        {
            get 
            {
                DataSet applications = InternalShared.Instance.Database.ExecSql_DataSet(string.Format("Admon_Applications"));
                return applications;
            }
        }

		/// <summary>
		/// Gets the list of applications for the given user.
		/// </summary>
		/// <param name="user_name"></param>
		/// <returns></returns>
		public DataSet GetApplicationList(string user_name)
		{
			DataSet applications = InternalShared.Instance.Database.ExecSql_DataSet(string.Format("Admon_UserApplications '{0}'",user_name));
			return applications;			
		}

		/// <summary>
		/// Gets the list of threads which are ready to be scheduled.
		/// </summary>
        public DataTable ReadyThreads
        {
            get 
            {
                return InternalShared.Instance.Database.ExecSql_DataTable("Thread_SelectReady");
            }
        }

    }
}