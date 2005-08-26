using Alchemi.Core.Owner;

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

namespace Alchemi.Core
{
	/// <summary>
	/// Defines the functions / services that should be provided by an executor implementation
	/// </summary>
    public interface IExecutor
    {
		/// <summary>
		/// Ping the executor to check if it is alive
		/// </summary>
        void PingExecutor();
        
		/// <summary>
		/// Executes the thread with the given identifier. 
		/// <br/>(Generally meant to be called by a Manager)
		/// </summary>
		/// <param name="ti"></param>
        void Manager_ExecuteThread(ThreadIdentifier ti);
        
		/// <summary>
		/// Aborts the thread with the given identifier
		/// <br/>(Generally meant to be called by a Manager)
		/// </summary>
		/// <param name="ti"></param>
		void Manager_AbortThread(ThreadIdentifier ti);

		/// <summary>
		/// Cleans up the files related to the application with the given id.
		/// <br/>(Generally meant to be called by a Manager)
		/// </summary>
		/// <param name="appid"></param>
		void Manager_CleanupApplication(string appid);
    }
}
