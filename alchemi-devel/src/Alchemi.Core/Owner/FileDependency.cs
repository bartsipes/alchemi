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
using Alchemi.Core.Utility;

namespace Alchemi.Core.Owner
{
	/// <summary>
	/// The FileDependency abstract class defines the members that need to exist in sub classes that are used to implement 
	/// "File" Dependencies. A file dependency represents a single file on which the grid application depends for input.
	/// </summary>
    [Serializable]
    public abstract class FileDependency
    {
		/// <summary>
		/// File name
		/// </summary>
        protected readonly string _FileName;
  
        //-----------------------------------------------------------------------------------------------    
    
		/// <summary>
		/// Creates an instance of the FileDependency
		/// </summary>
        public string FileName
        {
            get { return _FileName; }
        }

        //-----------------------------------------------------------------------------------------------    

		/// <summary>
		/// Creates an instance of the FileDependency
		/// </summary>
		/// <param name="fileName">name of the file</param>
        public FileDependency(string fileName)
        {
            _FileName = fileName;
        }

        //-----------------------------------------------------------------------------------------------    
    
		/// <summary>
		/// Unpacks the file to the specified location
		/// </summary>
		/// <param name="fileLocation">location and file name to unpack the file</param>
        public abstract void UnPack(string fileLocation);
    }
}