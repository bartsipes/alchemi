#region Alchemi copyright notice
/*
  Alchemi [.NET Grid Computing Framework]
  http://www.alchemi.net
  
  Copyright (c) 2002-2004 Akshay Luther & 2003-2004 Rajkumar Buyya 
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
using Alchemi.Core.Utility;

namespace Alchemi.Core
{
    [Serializable]
    public class EmbeddedFileDependency : FileDependency
    {
        protected string _Base64EncodedContents = "";

        public string Base64EncodedContents
        {
            get { return _Base64EncodedContents; }
            set { _Base64EncodedContents = value; }
        }
    
        //-----------------------------------------------------------------------------------------------        
    
        public EmbeddedFileDependency(string name) : base(name) {}

        public EmbeddedFileDependency(string name, string fileLocation) : base(name)
        {
            Pack(fileLocation);
        }

        //-----------------------------------------------------------------------------------------------        

        public void Pack(string fileLocation)
        {
            _Base64EncodedContents = Utils.ReadBase64EncodedFromFile(fileLocation);
        }

        //-----------------------------------------------------------------------------------------------    

        public override void UnPack(string fileLocation)
        {
            Utils.WriteBase64EncodedToFile(fileLocation, _Base64EncodedContents);
        }
    }
}
