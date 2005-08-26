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
using System.IO;
using Alchemi.Core.Owner;
using Alchemi.Core.Utility;

namespace Alchemi.Core.Owner
{
	/// <summary>
	/// The EmbeddedFileDependency Class extends from the FileDependency class
	/// and provides concrete implementation of the methods to pack and unpack files using base64 encoding.
	/// </summary>
    [Serializable]
    public class EmbeddedFileDependency : FileDependency
    {
		/// <summary>
		/// Contents of the file representing using base64 encoding.
		/// </summary>
        protected string _Base64EncodedContents = "";

		/// <summary>
		/// Gets or sets the file contents in base64-encoded format
		/// </summary>
        public string Base64EncodedContents
        {
            get { return _Base64EncodedContents; }
            set { _Base64EncodedContents = value; }
        }
    
        //-----------------------------------------------------------------------------------------------        
    
		/// <summary>
		/// Creates an instance of an EmbeddedFileDependency
		/// </summary>
		/// <param name="name">file name</param>
        public EmbeddedFileDependency(string name) : base(name) {}

		/// <summary>
		/// Creates an instance of an EmbeddedFileDependency
		/// </summary>
		/// <param name="name">file name</param>
		/// <param name="fileLocation">file location</param>
        public EmbeddedFileDependency(string name, string fileLocation) : base(name)
        {
            Pack(fileLocation);
        }

        //-----------------------------------------------------------------------------------------------        

		/// <summary>
		/// Reads the file and converts its contents to base64 format
		/// </summary>
		/// <param name="fileLocation">location of the file</param>
        public void Pack(string fileLocation)
        {
            _Base64EncodedContents = Utils.ReadBase64EncodedFromFile(fileLocation);
        }

        //-----------------------------------------------------------------------------------------------    

		/// <summary>
		/// Unpacks (writes out) the file to the specified location
		/// </summary>
		/// <param name="fileLocation">file location</param>
        public override void UnPack(string fileLocation)
        {
            Utils.WriteBase64EncodedToFile(fileLocation, _Base64EncodedContents);
        }
    }
}
