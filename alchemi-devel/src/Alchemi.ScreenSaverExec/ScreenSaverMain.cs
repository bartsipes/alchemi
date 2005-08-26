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

// http://www.codeproject.com/csharp/scrframework.asp

using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Alchemi.ScreenSaverExec
{
    public class ScreenSaverMain
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0].ToLower().Trim().Substring(0,2) == "/c")
                {
                    MessageBox.Show("This screen saver has no options you can set.", "Alchemi Screen Saver", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (args[0].ToLower() == "/s")
                {
                    for (int i = Screen.AllScreens.GetLowerBound(0); i <= Screen.AllScreens.GetUpperBound(0); i++) 
                        System.Windows.Forms.Application.Run(new ScreenSaverForm(i));				
                }
            }
            else
            {
                for (int i = Screen.AllScreens.GetLowerBound(0); i <= Screen.AllScreens.GetUpperBound(0); i++)
                {
                    System.Windows.Forms.Application.Run(new ScreenSaverForm(i));				
                }
            }
        }
    }
}
