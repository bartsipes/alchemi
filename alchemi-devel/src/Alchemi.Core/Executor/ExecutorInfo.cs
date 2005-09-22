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
	/// Represents the static attributes of an executor.
	/// 
	/// </summary>
    [Serializable]
    public struct ExecutorInfo
    {
		/// <summary>
		/// Gets or sets the Hostname of the Executor.
		/// </summary>
		public string Hostname;
		/// <summary>
		/// Gets or sets the maximum CPU power in the Executor hardware. (in Mhz)?Ghz
		/// </summary>
        public int MaxCpuPower;
		/// <summary>
		/// Gets or sets the maximum memory (RAM) in the Executor hardware. (in MB)
		/// </summary>
		public float MaxMemory; //in MB
		/// <summary>
		/// Gets or sets the maximum disk space in the Executor hardware. (in MB)
		/// </summary>
		public float MaxDiskSpace; // in MB
		/// <summary>
		/// Gets or sets the total number of CPUs in the Executor hardware.
		/// </summary>
		public int Number_of_CPUs;
		/// <summary>
		/// Gets or sets the name of Operating system running on the Executor
		/// </summary>
		public string OS;
		/// <summary>
		/// Gets or sets the architecture of the processor/machine of the Executor (eg: x86, RISC etc)
		/// </summary>
		public string Architecture;

		//these attributes are the limits set by the owner/administrator of the Executor node
		/// <summary>
		/// 
		/// </summary>
		public int CPULimit; //in Ghz * hr
		/// <summary>
		/// 
		/// </summary>
		public float memLimit; //in MB
		/// <summary>
		/// 
		/// </summary>
		public float diskLimit; //in MB

		//Qos stuff
		/// <summary>
		/// 
		/// </summary>
		public float CostPerCPUSec;
		/// <summary>
		/// 
		/// </summary>
		public float CostPerThread;
		/// <summary>
		/// 
		/// </summary>
		public float CostPerDiskMB;
    }
}


