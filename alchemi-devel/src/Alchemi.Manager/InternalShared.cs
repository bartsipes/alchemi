#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	InternalShared.cs
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
using System.Threading;

namespace Alchemi.Core.Manager
{
	/// <summary>
	/// This class has some static methods which are used internally in Alchemi by various other classes.
	/// These mainly deal with communicating with the database and scheduler.
	/// This class, therefore is like a container for the common objects.
	/// </summary>
    public class InternalShared
    {
		/// <summary>
		/// The database interface used to communicate with the SQL server database.
		/// This property is readonly. 
		/// </summary>
        public readonly SqlServer Database;

		/// <summary>
		/// The location of the working directory of the manager.
		/// This property is readonly.
		/// </summary>
        public readonly string DataRootDirectory;

		/// <summary>
		/// This property is used to synchronize access to the scheduler by multiple threads at once.
		/// </summary>
        public ManualResetEvent DedicatedSchedulerActive;

		/// <summary>
		/// The scheduler used by Alchemi
		/// </summary>
        public readonly IScheduler Scheduler; 
		//TODO currently this is readonly. meaning the scheduling algo cant be flipped at runtime.
		//need to change that.

        /*
        public readonly MApplicationCollection Applications;
        public readonly MExecutorCollection Executors;
        */

        private InternalShared(SqlServer database, string dataRootDirectory, IScheduler scheduler/*, MApplicationCollection applications, MExecutorCollection executors*/)
        {
            Database = database;
            DataRootDirectory = dataRootDirectory;
            DedicatedSchedulerActive = new ManualResetEvent(true);
            Scheduler = scheduler;
            /*
            Applications = applications;
            Executors = executors;
            */
        }

		/// <summary>
		/// Represents the static instance of this class
		/// </summary>
        public static InternalShared Instance;
        
		/// <summary>
		/// Gets an instance of this class (creates it, the first time).
		/// </summary>
		/// <param name="database"></param>
		/// <param name="dataRootDirectory"></param>
		/// <param name="scheduler"></param>
		/// <returns></returns>
        public static InternalShared GetInstance(SqlServer database, string dataRootDirectory, IScheduler scheduler/*, MApplicationCollection applications, MExecutorCollection executors*/)
        {
            if (Instance == null)
            {
                if (!Directory.Exists(dataRootDirectory))
                {
                    Directory.CreateDirectory(dataRootDirectory);
                }
                Instance = new InternalShared(database, dataRootDirectory, scheduler/*, applications, executors*/);
            }
            return Instance;
        }
    }
}
