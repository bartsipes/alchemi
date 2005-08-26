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
using System.Xml;
using System.IO;

namespace Alchemi.Core.Utility
{
	/// <summary>
	/// This class is used to read in / write out XML data from / in memory.
	/// </summary>
    public class XmlStringWriter
    {
        private MemoryStream _Ms;
        private XmlTextWriter _Writer;

        //-----------------------------------------------------------------------------------------------    

		/// <summary>
		/// Creates an instance of the XML writer capable of writing text
		/// </summary>
        public XmlTextWriter Writer
        {
            get { return _Writer; } 
        }

        //-----------------------------------------------------------------------------------------------    

		/// <summary>
		/// Creates an instance of an XML writer capable of writing ASCII text with indented format.
		/// </summary>
        public XmlStringWriter()
        {
            _Ms = new MemoryStream();
            _Writer = new XmlTextWriter(_Ms, System.Text.Encoding.ASCII);
            _Writer.Formatting = Formatting.Indented;
            _Writer.Indentation = 2;
        }
    
        //-----------------------------------------------------------------------------------------------    
    
		/// <summary>
		///	Returns the XML written to memory (so far) by the writer.
		/// </summary>
		/// <returns></returns>
        public string GetXmlString()
        {
            _Writer.Flush();
            _Ms.Position = 0;

            return (new StreamReader(_Ms)).ReadToEnd();
        }
    }
}
