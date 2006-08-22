#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	Configuration.cs
* Project		:	Alchemi Core
* Created on	:	2003
* Copyright		:	Copyright © 2006 The University of Melbourne
*					This technology has been developed with the support of 
*					the Australian Research Council and the University of Melbourne
*					research grants as part of the Gridbus Project
*					within GRIDS Laboratory at the University of Melbourne, Australia.
* Author         :  Akshay Luther (akshayl@csse.unimelb.edu.au) and Rajkumar Buyya (raj@csse.unimelb.edu.au)
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
using Alchemi.Core.Utility;

namespace Alchemi.Executor
{
	/// <summary>
	/// This class stores the configuration information for the Alchemi Executor
	/// This includes information such as the manager host and port to connect to, 
	/// whether this node is dedicated or not. The authentication details to connect
	/// to the manager etc.
	/// </summary>
    public class Configuration
    {
        public const string ConfigFileName = "Alchemi.Executor.config.xml";

		/// <summary>
		/// Executor Id
		/// </summary>
        public string Id;
		/// <summary>
		/// Host of the Manager
		/// </summary>
        public string ManagerHost = "localhost";
		/// <summary>
		/// Port of the Manager
		/// </summary>
        public int ManagerPort = 9000;
		/// <summary>
		/// Specifies whether the Executor is dedicated
		/// </summary>
        public bool Dedicated = true;
		/// <summary>
		/// Specifies the port on which the Executor listens for messages.
		/// </summary>
        public int OwnPort = 9001;
		/// <summary>
		/// Specifies if the Executor connected successfully with the current settings 
        /// for the ManagerHost,ManagerPort
		/// </summary>
        public bool ConnectVerified = false;
		/// <summary>
		/// Username for connection to the Manager
		/// </summary>
        public string Username = "executor";
		/// <summary>
		/// Password for connection to the Manager
		/// </summary>
        public string Password = "executor";

		/// <summary>
		/// Time interval (in seconds) between "heartbeats", ie. pinging the Manager 
        /// to notify that this Executor is alive.
		/// </summary>
		public int HeartBeatInterval = 5; //seconds
		
		/// <summary>
		/// Number of times to retry connecting, if the connection to the Manager is lost
		/// </summary>
		public bool RetryConnect = true; //retry connecting to manager on disconnection

		/// <summary>
		/// Time interval between successive connection retries
		/// </summary>
		public int RetryInterval = 30; //seconds

		/// <summary>
		/// Maximum number of times to retry connecting
		/// </summary>
		public int RetryMax = -1; //try reconnecting forever.

        /// <summary>
        /// Specifies whether to automatically revert to non-dedicated executor mode, 
        /// if the Manager cannot be contacted in dedicated mode.
        /// </summary> 
        public bool AutoRevertToNDE = false;
        /// <summary>
        /// Enforce secure sandboxed execution. Default: false.
        /// Turn this off to allow legacy applications (ie. GJobs)
        /// </summary>
        public bool SecureSandboxedExecution = false;
    
        //-----------------------------------------------------------------------------------------------
    
		/// <summary>
		/// Returns the configuration read from the xml config file.
		/// </summary>
		/// <returns>Configuration object</returns>
		public static Configuration GetConfiguration()
        {
            return (Configuration)DeSlz(typeof(Configuration));
        }
    
		/// <summary>
		/// Creates an instance of the Configuration class.
		/// </summary>
        public Configuration()
        {
        }

        //-----------------------------------------------------------------------------------------------
    
		/// <summary>
		///  Serialises and saves the configuration to the xml config file
		/// </summary>
		public void Slz()
        {
			XmlSerializer xs = new XmlSerializer(this.GetType());
            StreamWriter sw = new StreamWriter(DefaultConfigFile);
            xs.Serialize(sw, this);
            sw.Close();
        }

        //-----------------------------------------------------------------------------------------------

        private static string DefaultConfigFile
        {
            get
            {
                return Utils.GetFilePath(ConfigFileName, AlchemiRole.Executor, true);
            }
        }

		/// <summary>
		/// Deserialises and reads the configuration from the given xml file
		/// </summary>
		/// <returns>Configuration object</returns>
		private static Object DeSlz(Type type)
        {
            XmlSerializer xs = new XmlSerializer(type);
            string configFile = DefaultConfigFile;
            if (!File.Exists(DefaultConfigFile))
            {
                //look in current dir
                configFile = ConfigFileName;
            }
            FileStream fs = new FileStream(configFile, FileMode.Open);
            Object obj = xs.Deserialize(fs);
            fs.Close();
            return obj;
        }
    }
}
