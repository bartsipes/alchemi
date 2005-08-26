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
using Alchemi.Core;
using Alchemi.Core.Owner;

namespace Alchemi.Core.Manager
{
	/// <summary>
	/// Represents scheduler.
	/// This interface defines the basic members that need to exist in any scheduler implementation.
	/// </summary>
    public interface IScheduler
    {
		/// <summary>
		/// Sets the collection Applications
		/// </summary>
        MApplicationCollection Applications { set; }

		/// <summary>
		/// Sets the collection of Executors.
		/// </summary>
        MExecutorCollection Executors { set; }

		/// <summary>
		/// Returns a thread-identifier representing the next thread scheduled to the given executor.
		/// This represents a non-dedicated schedule, since the Executor would ask for this  thread-identifier.
		/// </summary>
		/// <param name="executorId"></param>
		/// <returns></returns>
		ThreadIdentifier ScheduleNonDedicated(string executorId);

		/// <summary>
		/// Returns the next available dedicated-schedule: containing the application,thread and executor id.
		/// </summary>
		/// <returns></returns>
        DedicatedSchedule ScheduleDedicated();
    }
}
