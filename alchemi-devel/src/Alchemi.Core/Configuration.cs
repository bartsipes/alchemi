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
using System.IO;
using System.Xml.Serialization;

namespace Alchemi.Core
{
	/// <summary>
	/// This class stores the configuration information for the Alchemi component using this class.
	/// </summary>
    public abstract class Configuration
    {
        [NonSerialized] protected string ConfigFile = "";
        private const string ConfigFileName = "Alchemi.config.xml";
    
        //-----------------------------------------------------------------------------------------------
    
		/// <summary>
		/// Returns the configuration read from the xml file: "Alchemi.config.xml"
		/// </summary>
		/// <returns>Configuration object</returns>
		public static Configuration GetConfiguration()
        {
            return DeSlz(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFileName));
        }

		/// <summary>
		/// Returns the configuration read from the xml file ("Alchemi.config.xml") at the given location
		/// </summary>
		/// <param name="location">Location of the config file</param>
		/// <returns>Configuration object</returns>
        public static Configuration GetConfiguration(string location)
        {
            return DeSlz(Path.Combine(location, ConfigFileName));
        }
    
		/// <summary>
		/// Creates an instance of the Configuration class.
		/// </summary>
        public Configuration()
        {
            ConfigFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFileName);
        }

		//Code added by Rodrigo Assirati Dias
		/* Log 12/03, 2004
		 * Modifyied by Rodrigo Assirati Dias (rdias@ime.usp.br)
		 * Created additional constructor for Configuration class, to enable a windows service to create
		 * a configuration file in other directory than the running application directory (%WINDOWSDIR%/System32 to services)
		 */
		/// <summary>
		/// Creates an instance of the Configuration class.
		/// </summary>
		/// <param name="location"></param>
		public Configuration(string location)
		{
			ConfigFile = location + ConfigFileName;
		}

        //-----------------------------------------------------------------------------------------------
    
		/// <summary>
		///  Serialises and saves the configuration to the xml file: "Alchemi.Executor.config.xml"
		/// </summary>
		public void Slz()
        {
			XmlSerializer xs = new XmlSerializer(typeof(Configuration));
            StreamWriter sw = new StreamWriter(ConfigFile);
            xs.Serialize(sw, this);
            sw.Close();
        }

        //-----------------------------------------------------------------------------------------------

		/// <summary>
		/// Deserialises and reads the configuration from the given xml file
		/// </summary>
		/// <param name="file">Name of the config file</param>
		/// <returns>Configuration object</returns>
		private static Configuration DeSlz(string file)
        {
            XmlSerializer xs = new XmlSerializer(typeof(Configuration));
            FileStream fs = new FileStream(file, FileMode.Open);
			Configuration temp = (Configuration) xs.Deserialize(fs);
            fs.Close();
            temp.ConfigFile = file;
            return temp;
        }

    }
}
