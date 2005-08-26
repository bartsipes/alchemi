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

namespace Alchemi.Core
{
	/// <summary>
	/// Defines the functions to be provided by a cross-platform webservices manager
	/// </summary>
    public interface ICrossPlatformManager
    {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
        string /* taskId */ CreateTask(string username, string password);
        
		/// <summary>
		/// 
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <param name="taskXml"></param>
		/// <returns></returns>
		string /* taskId */ SubmitTask(string username, string password, string taskXml);
        
		/// <summary>
		/// Add a job to the manager with the given credentials, task and jobID, priority and XML description
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <param name="taskId"></param>
		/// <param name="jobId"></param>
		/// <param name="priority"></param>
		/// <param name="jobXml"></param>
		void AddJob(string username, string password, string taskId, int jobId, int priority, string jobXml);
        
		/// <summary>
		/// Gets the XML description of the finished jobs for the given application/task id
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <param name="taskId">task / application id</param>
		/// <returns></returns>
		string /* taskXml */ GetFinishedJobs(string username, string password, string taskId);
        
		/// <summary>
		/// Gets the status of the job with the given id and task/application id.
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <param name="taskId">application / task id</param>
		/// <param name="jobId"></param>
		/// <returns></returns>
		int GetJobState(string username, string password, string taskId, int jobId);
    }
}
