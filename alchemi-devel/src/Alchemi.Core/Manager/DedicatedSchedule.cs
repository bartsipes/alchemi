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
	/// This class represents a "dedicatedSchedule" which is just a container for the ThreadIdentifier and ExecutorId which refers to
	/// the executor that will run this thread.
	/// </summary>
    public class DedicatedSchedule
    {
		/// <summary>
		/// ThreadIdentifier representing the scheduled application and thread.
		/// </summary>
        public readonly ThreadIdentifier TI;

		/// <summary>
		/// The id of the Executor scheduled to execute this thread
		/// </summary>
        public readonly string ExecutorId;

		/// <summary>
		/// Constructor with a threadIdentifier and executorId
		/// </summary>
		/// <param name="ti">ThreadIdentifier</param>
		/// <param name="executorId">Executor Id</param>
        public DedicatedSchedule(ThreadIdentifier ti, string executorId)
        {
            TI = ti;
            ExecutorId = executorId;
        }
    }
}