#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	Configuration.cs
* Project		:	Alchemi Core
* Created on	:	2003
* Copyright		:	Copyright © 2005 The University of Melbourne
*					This technology has been developed with the support of 
*					the Australian Research Council and the University of Melbourne
*					research grants as part of the Gridbus Project
*					within GRIDS Laboratory at the University of Melbourne, Australia.
* Author         :  Akshay Luther (akshayl@cs.mu.oz.au) and Rajkumar Buyya (raj@cs.mu.oz.au)
* License        :  GPL
*					This program is free software; you can redistribute it and/or 
*					modify it under the terms of the GNU General Public
*					License as published by the Free Software Foundation;
*					See the GNU General Public License 
*					(http://www.gnu.org/copyleft/gpl.html) for more details.
*
*/ 
#endregion


using System;
using System.IO;
using System.Xml.Serialization;

namespace Alchemi.Core.Manager
{
	/// <summary>
	/// This class stores the configuration information for the Alchemi Manager
	/// This includes information such as database details, own port,
	/// and in case of a Heirarchical system structure, 
	/// the manager host and port to connect to, whether this node(which is both a manager and executor if in a hierarchy) is dedicated or not.
	/// </summary>
    public class Configuration
    {
        [NonSerialized] private string ConfigFile = "";
        private const string ConfigFileName = "Alchemi.Manager.config.xml";
		
		/// <summary>
		/// Database server host name
		/// </summary>
        public string DbServer = "localhost";
		/// <summary>
		/// Database server username
		/// </summary>
        public string DbUsername = "sa";
		/// <summary>
		/// Database password
		/// </summary>
        public string DbPassword = "xxxx";
		/// <summary>
		/// Database name
		/// </summary>
        public string DbName = "Alchemi";
		/// <summary>
		/// Manager id (valid only if the Manager is also an Executor)
		/// </summary>
        public string Id = "";
		/// <summary>
		/// Host name of the Manager to connect to. (valid only if the Manager is also an Executor)
		/// </summary>
        public string ManagerHost = "";
		/// <summary>
		/// Port of the Manager to connect to. (valid only if the Manager is also an Executor)
		/// </summary>
        public int ManagerPort = 0;
		/// <summary>
		/// Port on which the Manager sends/recieves messages to/from the Executors.
		/// </summary>
        public int OwnPort = 9000;
		/// <summary>
		/// Specifies if the connection parameters have been verified. 
		/// The parameters are verified if the Manager has been able to connect sucessfully using the current parameter values.
		/// </summary>
        public bool ConnectVerified = false;
		/// <summary>
		/// 
		/// </summary>
        public bool InUse = false;
		/// <summary>
		/// Specifies if this Manager is an "intermediate" Manager...ie. if it is performing the role of an Executor also.
		/// </summary>
        public bool Intermediate = false;
		/// <summary>
		/// Specifies if the Manager is Dedicated.(valid only if the Manager is also an Executor)
		/// </summary>
        public bool Dedicated = false;

        //-----------------------------------------------------------------------------------------------
		
		/// <summary>
		/// Returns the configuration read from the xml file: "Alchemi.Manager.config.xml"
		/// </summary>
		/// <returns>Configuration object</returns>
        public static Configuration GetConfiguration()
        {
			return DeSlz(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFileName));
        }

		/// <summary>
		/// Default constructor. ConfigFileName is set to "Alchemi.Manager.config.xml"
		/// </summary>
        public Configuration()
        {
			ConfigFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFileName);
		}

		/// <summary>
		/// This Constructor for the Configuration class sets the ConfigFileName to the given location and "Alchemi.Manager.Congif.xml"
		/// </summary>
		/// <param name="location">Location of the config file</param>
        public Configuration(string location)
        {
            ConfigFile = Path.Combine(location, ConfigFileName);
        }

        //-----------------------------------------------------------------------------------------------    

        /// <summary>
        ///  Serialises and saves the configuration to an xml file
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
            return temp;
        }

    }
}
