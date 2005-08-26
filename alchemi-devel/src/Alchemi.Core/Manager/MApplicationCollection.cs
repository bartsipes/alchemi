#region Alchemi copyright notice
/*
  Alchemi [.NET Grid Computing Framework]
  http://www.alchemi.net
  
  Copyright (c)  Akshay Luther (2002-2004) & Rajkumar Buyya (2003-to-date), 
  GRIDS Lab, The University of Melbourne, Australia.
  
  Maintained and Updated by: Krishna Nadiminti (2005-to-date)
---------------------------------------------------------------------------

  This program is free software; you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation; either version 2 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program; if not, write to the Free Software
  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
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