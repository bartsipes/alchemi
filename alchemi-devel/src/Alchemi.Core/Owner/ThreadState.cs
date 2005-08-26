using System;

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

namespace Alchemi.Core.Owner
{
	/// <summary>
	/// List of possible thread states
	/// Ready = 0, // ready to execute <br />
	/// Scheduled = 1, // executor has id <br/>
	/// Started = 2, // executor has thread and is executing <br />
	/// Finished = 3, // executor has returned the finished thread<br />
	/// Dead = 4, // returned to owner OR aborted
	/// </summary>
	[Serializable]
    public enum ThreadState
    {
        Ready = 0, // ready to execute
        Scheduled = 1, // executor has id
        Started = 2, // executor has thread and is executing
        Finished = 3, // executor has returned the finished thread
        Dead = 4, // returned to owner OR aborted
    }
}