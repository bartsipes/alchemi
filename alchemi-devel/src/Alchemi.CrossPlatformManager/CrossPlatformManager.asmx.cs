#region Alchemi copyright notice
/*
  Alchemi [.NET Grid Computing Framework]
  http://www.alchemi.net
  
  Copyright (c) 2002-2004 Akshay Luther & 2003-2004 Rajkumar Buyya 
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
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Configuration;
using Alchemi.Core;
using Alchemi.Core.Utility;

namespace Alchemi.Manager
{
    [WebService(Namespace="http://www.alchemi.net")]
    public class CrossPlatformManager : WebService, ICrossPlatformManager
    {
        #region Component Designer generated code
		
        //Required by the Web Services Designer 
        private IContainer components = null;
				
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if(disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);		
        }
		
        #endregion

        private IManager Manager;
    
        //-----------------------------------------------------------------------------------------------          
    
        public CrossPlatformManager()
        {
            Manager = (IManager) Activator.GetObject(
                typeof(IManager),
                ConfigurationSettings.AppSettings["ManagerUri"]
                );

            InitializeComponent();
        }

        //-----------------------------------------------------------------------------------------------          

        [WebMethod]
        public string CreateTask(string username, string password)
        {
            return CrossPlatformHelper.CreateTask(Manager, new SecurityCredentials(username, password));
        }
        
        //-----------------------------------------------------------------------------------------------                       
        
        [WebMethod]
        public string SubmitTask(string username, string password, string taskXml)
        {
            return CrossPlatformHelper.CreateTask(Manager, new SecurityCredentials(username, password), taskXml);
        }
        
        //-----------------------------------------------------------------------------------------------       
        
        [WebMethod]
        public void AddJob(string username, string password, string taskId, int jobId, int priority, string jobXml)
        {
            CrossPlatformHelper.AddJob(Manager, new SecurityCredentials(username, password), taskId, jobId, priority, jobXml);
        }

        //-----------------------------------------------------------------------------------------------       
    
        [WebMethod]
        public string GetFinishedJobs(string username, string password, string taskId)
        {
            return CrossPlatformHelper.GetFinishedJobs(Manager, new SecurityCredentials(username, password), taskId);
        }
    
        //-----------------------------------------------------------------------------------------------       

        [WebMethod]
        public int GetJobState(string username, string password, string taskId, int jobId)
        {
            return CrossPlatformHelper.GetJobState(Manager, new SecurityCredentials(username, password), taskId, jobId);
        }

        //-----------------------------------------------------------------------------------------------       

        [WebMethod]
        public void Ping()
        {
            Manager.PingManager();
        }
        
        //-----------------------------------------------------------------------------------------------       
        
        [WebMethod]
        public DataSet ListLiveApps()
        {
            return null;
        }

    }
}
