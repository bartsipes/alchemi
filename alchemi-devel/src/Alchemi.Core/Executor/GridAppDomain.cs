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
using System.Reflection;

namespace Alchemi.Core.Executor
{
	/// <summary>
	/// This class is a container for the AppDomainExecutor and AppDomain.
	/// </summary>
    public class GridAppDomain
    {
        private AppDomain _Domain;
        private AppDomainExecutor _Executor;

        //----------------------------------------------------------------------------------------------- 
  
		/// <summary>
		/// Gets the AppDomain
		/// </summary>
        public AppDomain Domain
        {
            get { return _Domain; }
        }

		/// <summary>
		/// Gets the AppDommainExecutor
		/// </summary>
        public AppDomainExecutor Executor
        {
            get { return _Executor; }
        }

        //----------------------------------------------------------------------------------------------- 

		/// <summary>
		/// Initialises a new instance of the GridAppDomain with the given AppDomain and AppDomainExecutor
		/// </summary>
		/// <param name="domain"></param>
		/// <param name="executor"></param>
        public GridAppDomain(AppDomain domain, AppDomainExecutor executor)
        {
            _Domain = domain;
            _Executor = executor;
        }
    }
}