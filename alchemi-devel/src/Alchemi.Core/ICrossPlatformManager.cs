#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   ICrossPlatformManager.cs
 * Project      :   Alchemi.Core
 * Created on   :   2003
 * Copyright    :   Copyright © 2006 The University of Melbourne
 *                  This technology has been developed with the support of 
 *                  the Australian Research Council and the University of Melbourne
 *                  research grants as part of the Gridbus Project
 *                  within GRIDS Laboratory at the University of Melbourne, Australia.
 * Author       :   Akshay Luther (akshayl@csse.unimelb.edu.au)
 *                  Rajkumar Buyya (raj@csse.unimelb.edu.au)
 *                  Krishna Nadiminti (kna@csse.unimelb.edu.au)
 * License      :   GPL
 *                  This program is free software; you can redistribute it and/or 
 *                  modify it under the terms of the GNU General Public
 *                  License as published by the Free Software Foundation;
 *                  See the GNU General Public License 
 *                  (http://www.gnu.org/copyleft/gpl.html) for more details.
 *
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
        /// Creates a task.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>The taskId</returns>
        string CreateTask(string username, string password);

        /// <summary>
        /// Submits the task.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="taskXml">The task XML.</param>
        /// <returns>The taskId</returns>
		string SubmitTask(string username, string password, string taskXml);

        /// <summary>
        /// Add a job to the manager with the given credentials, task and jobID, priority and XML description
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="taskId">The taskId.</param>
        /// <param name="jobId">The jobId.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="jobXml">The job XML.</param>
		void AddJob(string username, string password, string taskId, int jobId, int priority, string jobXml);

        /// <summary>
        /// Gets the XML description of the finished jobs for the given application/task id
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="taskId">task/application id</param>
        /// <returns>The task XML</returns>
		string GetFinishedJobs(string username, string password, string taskId);

        /// <summary>
        /// Gets the status of the job with the given id and task/application id.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="taskId">task/application id</param>
        /// <param name="jobId">The job id.</param>
        /// <returns>The job state</returns>
		int GetJobState(string username, string password, string taskId, int jobId);
    }
}
