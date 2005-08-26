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

namespace Alchemi.Core
{
	/// <summary>
	/// Represents the credentials required to authenticate to a node
	/// </summary>
	[Serializable]
    public class SecurityCredentials
	{
		/// <summary>
		/// Username
		/// </summary>
		public readonly string Username;
		/// <summary>
		/// Password
		/// </summary>
        public string Password;
        
		/// <summary>
		/// Creates an instance of the SecurityCredentials class
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
        public SecurityCredentials(string username, string password)
        {
            Username = username;
            Password = password;
        }
	}
}
