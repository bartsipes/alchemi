#region Alchemi copyright notice
/*
  Alchemi [.NET Grid Computing Framework]
  Copyright (c) 2002-2004 Akshay Luther
  http://www.alchemi.net
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
using Alchemi.Core.Utility;

namespace Alchemi.Core.Manager
{
    /// <summary>
    /// Represents a collection of grid applications. A particular application is referred to by its ID via the indexer.
    /// </summary>
    public class MApplicationCollection
    {
        public MApplication this[string applicationId]
        {
            get
            {
                return new MApplication(applicationId);
            }
        }
        
        public string CreateNew(string username)
        {
            string appId = InternalShared.Instance.Database.ExecSql_Scalar("Application_Insert '{0}'", username).ToString();
            this[appId].CreateDataDirectory();
            return appId;
        }

        public DataSet LiveList
        {
            get 
            {
                DataSet applications = InternalShared.Instance.Database.ExecSql_DataSet(string.Format("Admon_Applications"));
                return applications;
            }
        }

        public DataTable ReadyThreads
        {
            get 
            {
                return InternalShared.Instance.Database.ExecSql_DataTable("Thread_SelectReady");
            }
        }

    }
}