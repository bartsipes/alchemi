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

namespace Alchemi.Core
{
	/// <summary>
	/// Delegate for the Application Finished event
	/// </summary>
    public delegate void GApplicationFinish();
	/// <summary>
	/// Delegare for the ThreadFinish event
	/// </summary>
    public delegate void GThreadFinish(GThread thread);
	/// <summary>
	/// Delegate for the ThreadFailed event
	/// </summary>
    public delegate void GThreadFailed(GThread thread, Exception e);

	/// <summary>
	/// Delegate for the log event
	/// </summary>
	public delegate void LogEventHandler(object sender, LogEventArgs e);
}