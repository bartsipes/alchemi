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

using Alchemi.Manager.Storage;
using Alchemi.Core.Utility;

namespace Alchemi.Manager
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
        private string _DbServer = "localhost";

        public string DbServer
        {
            get { return _DbServer; }
            set { _DbServer = value; }
        }

		/// <summary>
		/// Database server username
		/// </summary>
        private string _DbUsername = "sa";

        public string DbUsername
        {
            get { return _DbUsername; }
            set { _DbUsername = value; }
        }

		/// <summary>
		/// Database password
		/// </summary>
        private string _DbPassword = "xxxx";

        public string DbPassword
        {
            get { return _DbPassword; }
            set { _DbPassword = value; }
        }

		/// <summary>
		/// Database name
		/// </summary>
        private string _DbName = "Alchemi";

        public string DbName
        {
            get { return _DbName; }
            set { _DbName = value; }
        }

        /// <summary>
        /// Database connect timeout
        /// </summary>
        private int _DbConnectTimeout = 5;

        public int DbConnectTimeout
        {
            get { return _DbConnectTimeout; }
            set { _DbConnectTimeout = value; }
        }

        /// <summary>
        /// Database max pool size
        /// </summary>
        private int _DbMaxPoolSize = 5;

        public int DbMaxPoolSize
        {
            get { return _DbMaxPoolSize; }
            set { _DbMaxPoolSize = value; }
        }

        /// <summary>
        /// Database min pool size
        /// </summary>
        private int _DbMinPoolSize = 5;

        public int DbMinPoolSize
        {
            get { return _DbMinPoolSize; }
            set { _DbMinPoolSize = value; }
        }

        /// <summary>
        /// Default filename for file-based db storage
        /// </summary>
        private string _DbFilePath = "Alchemi.db";

        public string DbFilePath
        {
            get { return _DbFilePath; }
            set { _DbFilePath = value; }
        }

		/// <summary>
		/// The storage used by this Manager.
		/// Defaults to In-memory.
		/// </summary>
        private ManagerStorageEnum _DbType = ManagerStorageEnum.InMemory;

        public ManagerStorageEnum DbType
        {
            get { return _DbType; }
            set { _DbType = value; }
        }

		/// <summary>
		/// Manager id (valid only if the Manager is also an Executor)
		/// </summary>
        private string _Id = "";

        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

		/// <summary>
		/// Host name of the Manager to connect to. (valid only if the Manager is also an Executor)
		/// </summary>
        private string _ManagerHost = "";

        public string ManagerHost
        {
            get { return _ManagerHost; }
            set { _ManagerHost = value; }
        }

		/// <summary>
		/// Port of the Manager to connect to. (valid only if the Manager is also an Executor)
		/// </summary>
        private int _ManagerPort = 0;

        public int ManagerPort
        {
            get { return _ManagerPort; }
            set { _ManagerPort = value; }
        }

		/// <summary>
		/// Port on which the Manager sends/recieves messages to/from the Executors.
		/// </summary>
        private int _OwnPort = 9000;

        public int OwnPort
        {
            get { return _OwnPort; }
            set { _OwnPort = value; }
        }

        private Alchemi.Core.EndPointUtils.EndPointConfigurationCollection _EndPoints;
        /// <summary>
        /// Collection of end points on whitch the manager is published.
        /// </summary>
        public Alchemi.Core.EndPointUtils.EndPointConfigurationCollection EndPoints
        {
            get
            {
                if (_EndPoints == null)
                    _EndPoints = new Alchemi.Core.EndPointUtils.EndPointConfigurationCollection();

                return _EndPoints;
            }
            set { _EndPoints = value; }
        }

		/// <summary>
		/// Specifies if the connection parameters have been verified. 
		/// The parameters are verified if the Manager has been able to connect sucessfully using the current parameter values.
		/// </summary>
        private bool _ConnectVerified = false;

        public bool ConnectVerified
        {
            get { return _ConnectVerified; }
            set { _ConnectVerified = value; }
        }

		/// <summary>
		/// Specifies if this Manager is an "intermediate" Manager...ie. if it is performing the role of an Executor also.
		/// </summary>
        private bool _Intermediate = false;

        public bool Intermediate
        {
            get { return _Intermediate; }
            set { _Intermediate = value; }
        }

		/// <summary>
		/// Specifies if the Manager is Dedicated.(valid only if the Manager is also an Executor)
		/// </summary>
        private bool _Dedicated = false;

        public bool Dedicated
        {
            get { return _Dedicated; }
            set { _Dedicated = value; }
        }

        /// <summary>
        /// Specifies the scheduler assembly name; null or empty indicates the default scheduler assembly name.
        /// </summary>
        private string _SchedulerAssemblyName = "";

        public string SchedulerAssemblyName
        {
            get { return _SchedulerAssemblyName; }
            set { _SchedulerAssemblyName = value; }
        }

        /// <summary>
        /// Specifies the scheduler type name; null or empty indicates the default scheduler type name.
        /// </summary>
        private string _SchedulerTypeName = "";

        public string SchedulerTypeName
        {
            get { return _SchedulerTypeName; }
            set { _SchedulerTypeName = value; }
        }


        //-----------------------------------------------------------------------------------------------
		
		/// <summary>
		/// Returns the configuration read from the xml file: "Alchemi.Manager.config.xml"
		/// </summary>
		/// <returns>Configuration object</returns>
        public static Configuration GetConfiguration()
        {
            return Deserialize(Utils.GetFilePath(ConfigFileName, AlchemiRole.Manager, true));
        }

		/// <summary>
		/// Default constructor. ConfigFileName is set to "Alchemi.Manager.config.xml"
		/// </summary>
        public Configuration()
        {
            ConfigFile = Utils.GetFilePath(ConfigFileName, AlchemiRole.Manager, true);
		}

        //-----------------------------------------------------------------------------------------------    

        /// <summary>
        ///  Serialises and saves the configuration to an xml file
        /// </summary>
        public void Serialize()
        {
            XmlSerializer xs = new XmlSerializer(typeof(Configuration));
            StreamWriter sw = new StreamWriter(ConfigFile);
            xs.Serialize(sw, this);
            sw.Close();
        }

        //-----------------------------------------------------------------------------------------------

        /// <summary>
        /// Deserialises and reads the configuration from the given xml file
        /// 
        /// Updates:
        /// Jan 18, 2006 - tb@tbiro.com
        ///		Saved the file used to load into ConfigFile so serializing puts the file back to the original location.
        /// </summary>
        /// <param name="file">Name of the config file</param>
        /// <returns>Configuration object</returns>
        private static Configuration Deserialize(string file)
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
