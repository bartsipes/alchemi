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

        #region Property -Id
        /// <summary>
		/// Executor Id
		/// </summary>
        private string _Id;

        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        #endregion

        #region Property -ManagerHost
        /// <summary>
		/// Host of the Manager
		/// </summary>
        private string _ManagerHost = "localhost";

        public string ManagerHost
        {
            get { return _ManagerHost; }
            set { _ManagerHost = value; }
        }
        #endregion

        #region Property -ManagerPort
        /// <summary>
		/// Port of the Manager
		/// </summary>
        private int _ManagerPort = 9000;

        public int ManagerPort
        {
            get { return _ManagerPort; }
            set { _ManagerPort = value; }
        }
        #endregion

        #region Property -Dedicated
        /// <summary>
		/// Specifies whether the Executor is dedicated
		/// </summary>
        private bool _Dedicated = true;

        public bool Dedicated
        {
            get { return _Dedicated; }
            set { _Dedicated = value; }
        }
        #endregion

        #region Property -OwnPort
        /// <summary>
		/// Specifies the port on which the Executor listens for messages.
		/// </summary>
        private int _OwnPort = 9001;

        public int OwnPort
        {
            get { return _OwnPort; }
            set { _OwnPort = value; }
        }
        #endregion

        #region Property -ConnectVerified
        /// <summary>
		/// Specifies if the Executor connected successfully with the current settings 
        /// for the ManagerHost,ManagerPort
		/// </summary>
        private bool _ConnectVerified = false;

        public bool ConnectVerified
        {
            get { return _ConnectVerified; }
            set { _ConnectVerified = value; }
        }
        #endregion

        #region Property -Username
        /// <summary>
		/// Username for connection to the Manager
		/// </summary>
        private string _Username = "executor";

        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }
        #endregion

        #region Property -Password
        /// <summary>
		/// Password for connection to the Manager
		/// </summary>
        private string _Password = "executor";

        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }
        #endregion

        #region Property -HeartBeatInterval
        /// <summary>
		/// Time interval (in seconds) between "heartbeats", ie. pinging the Manager 
        /// to notify that this Executor is alive.
		/// </summary>
        private int _HeartBeatInterval = 5; //seconds

        public int HeartBeatInterval
        {
            get { return _HeartBeatInterval; }
            set { _HeartBeatInterval = value; }
        }
        #endregion

        #region Property -RetryConnect
        /// <summary>
		/// Number of times to retry connecting, if the connection to the Manager is lost
		/// </summary>
        private bool _RetryConnect = true; //retry connecting to manager on disconnection

        public bool RetryConnect
        {
            get { return _RetryConnect; }
            set { _RetryConnect = value; }
        }
        #endregion

        #region Property -RetryInterval
        /// <summary>
		/// Time interval between successive connection retries
		/// </summary>
        private int _RetryInterval = 30; //seconds

        public int RetryInterval
        {
            get { return _RetryInterval; }
            set { _RetryInterval = value; }
        }
        #endregion

        #region Property -RetryMax
        /// <summary>
		/// Maximum number of times to retry connecting
		/// </summary>
        private int _RetryMax = -1; //try reconnecting forever.

        public int RetryMax
        {
            get { return _RetryMax; }
            set { _RetryMax = value; }
        }
        #endregion

        #region Property -AutoRevertToNDE
        /// <summary>
        /// Specifies whether to automatically revert to non-dedicated executor mode, 
        /// if the Manager cannot be contacted in dedicated mode.
        /// </summary> 
        private bool _AutoRevertToNDE = false;

        public bool AutoRevertToNDE
        {
            get { return _AutoRevertToNDE; }
            set { _AutoRevertToNDE = value; }
        }
        #endregion

        #region Property -SecureSandboxedExecution
        /// <summary>
        /// Enforce secure sandboxed execution. Default: false.
        /// Turn this off to allow legacy applications (ie. GJobs)
        /// </summary>
        private bool _SecureSandboxedExecution = false;

        public bool SecureSandboxedExecution
        {
            get { return _SecureSandboxedExecution; }
            set { _SecureSandboxedExecution = value; }
        }
        #endregion



        #region Constructor
        /// <summary>
        /// Creates an instance of the Configuration class.
        /// </summary>
        public Configuration()
        {
        } 
        #endregion



        #region Property - DefaultConfigFile
        private static string DefaultConfigFile
        {
            get
            {
                return Utils.GetFilePath(ConfigFileName, AlchemiRole.Executor, true);
            }
        }
        #endregion



        #region Method - Serialize
        /// <summary>
        ///  Serialises and saves the configuration to the xml config file
        /// </summary>
        public void Serialize()
        {
            XmlSerializer xs = new XmlSerializer(typeof(Configuration));
            StreamWriter sw = new StreamWriter(Configuration.DefaultConfigFile);
            xs.Serialize(sw, this);
            sw.Close();
        } 
        #endregion


        #region Method - Deserialize
        /// <summary>
        /// Reads the configuration from the xml file
        /// </summary>
        /// <returns></returns>
        public static Configuration Deserialize()
        {
            XmlSerializer xs = new XmlSerializer(typeof(Configuration));
            string configFile = DefaultConfigFile;

            if (!File.Exists(DefaultConfigFile))
            {
                //look in current dir
                configFile = ConfigFileName;
            }
            FileStream fs = new FileStream(configFile, FileMode.Open);
            Configuration obj = (Configuration)xs.Deserialize(fs);
            fs.Close();
            return obj;
        } 
        #endregion

    }
}
