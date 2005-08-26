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
using Alchemi.Core.Owner;

namespace Alchemi.Core.Owner
{
	/// <summary>
	/// Represents a collection of GThreads.
	/// </summary>
    [Serializable]
    public class ThreadCollection : System.Collections.CollectionBase
    {
		/// <summary>
		/// Gets or sets the GThread at the given index
		/// </summary>
        public GThread this[int index]
        {
            get 
            { 
                return (GThread) InnerList[index]; 
            }

            set 
            {
                InnerList[index] = value;
            }
        }

        //-----------------------------------------------------------------------------------------------    

		/// <summary>
		/// Adds a GThread object to this collection
		/// </summary>
		/// <param name="thread">the grid thread to add</param>
        public void Add(GThread thread)
        {
            InnerList.Add(thread);
        }

		//krishna added - 23May05
		//this gives more freedom to the app developer to remove the threads no longer wanted, for whatever reason
		/// <summary>
		/// Removes a GThread object from this collection IF it is in a dead / finished state.
		/// </summary>
		/// <param name="thread"></param>
		public void Remove(GThread thread)
		{
			lock (InnerList)
			{
				if (thread.State == ThreadState.Dead || thread.State == ThreadState.Finished)
					InnerList.Remove(thread);
			}
		}
    }
}
