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
