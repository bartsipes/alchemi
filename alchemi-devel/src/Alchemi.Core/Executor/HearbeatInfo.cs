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

namespace Alchemi.Core.Executor
{
	/// <summary>
	/// This structure is a container for all the information sent in a heartbeat update.
	/// This primarily consists of dynamic information about an Executor, such as current load conditions etc...
	/// </summary>
    [Serializable]
    public struct HeartbeatInfo
    {
		/// <summary>
		/// Heartbeat interval
		/// </summary>
        public int Interval;
		/// <summary>
		/// PercentUsedCpuPower
		/// </summary>
        public int PercentUsedCpuPower;
		/// <summary>
		/// PercentAvailCpuPower
		/// </summary>
        public int PercentAvailCpuPower;

		/// <summary>
		/// Creates an instance of the HeartBeat object with the given interval, used, and available CPU power.
		/// </summary>
		/// <param name="interval"></param>
		/// <param name="used"></param>
		/// <param name="avail"></param>
        public HeartbeatInfo(int interval, int used, int avail)
        {
            this.Interval = interval;
            this.PercentUsedCpuPower = used;
            this.PercentAvailCpuPower = avail;
        }
    }
}
