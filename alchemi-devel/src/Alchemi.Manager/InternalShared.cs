#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	InternalShared.cs
* Project		:	Alchemi Core
* Created on	:	2003
* Copyright		:	Copyright � 2006 The University of Melbourne
*					This technology has been developed with the support of 
*					the Australian Research Council and the University of Melbourne
*					research grants as part of the Gridbus Project
*					within GRIDS Laboratory at the University of Melbourne, Australia.
* Author         :  Akshay Luther (akshayl@csse.unimelb.edu.au) and Rajkumar Buyya (raj@csse.unimelb.edu.au)
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
using System.Threading;
using Alchemi.Core.Utility;

namespace Alchemi.Manager
{
	/// <summary>
	/// This class has some static methods which are used internally in Alchemi by various other classes.
	/// These mainly deal with communicating with the scheduler.
	/// This class, therefore is like a container for the common objects.
	/// </summary>
    internal class InternalShared
    {	
		/// <summary>
		/// The location of the working directory of the manager.
		/// This property is readonly.
		/// </summary>
        internal readonly string DataRootDirectory;

		/// <summary>
		/// This property is used to synchronize access to the scheduler by multiple threads at once.
		/// </summary>
        internal ManualResetEvent DedicatedSchedulerActive;

        internal readonly IScheduler Scheduler;

        private InternalShared(Configuration config)
        {
            DataRootDirectory = Utils.GetFilePath("dat", AlchemiRole.Manager, true); 
            DedicatedSchedulerActive = new ManualResetEvent(true);
            Scheduler = (new SchedulerFactory()).CreateScheduler(config);
        }

		/// <summary>
		/// Represents the static instance of this class
		/// </summary>
        internal static InternalShared Instance;
        
		/// <summary>
		/// Gets an instance of this class (creates it, the first time).
		/// </summary>
		/// <returns></returns>
        internal static InternalShared GetInstance(Configuration config)
        {
            if (Instance == null)
            {
                Instance = new InternalShared(config);
            }
            return Instance;
        }

        internal string GetApplicationDirectory(string applicationId)
        {
            return Path.Combine(DataRootDirectory, string.Format("application_{0}", applicationId));
        }
    }
}
