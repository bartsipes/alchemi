#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   GConnection.cs
 * Project      :   Alchemi.Core.Owner
 * Created on   :   2003
 * Copyright    :   Copyright © 2006 The University of Melbourne
 *                  This technology has been developed with the support of 
 *                  the Australian Research Council and the University of Melbourne
 *                  research grants as part of the Gridbus Project
 *                  within GRIDS Laboratory at the University of Melbourne, Australia.
 * Author       :   Akshay Luther (akshayl@csse.unimelb.edu.au)
 *                  Rajkumar Buyya (raj@csse.unimelb.edu.au)
 *                  Krishna Nadiminti (kna@csse.unimelb.edu.au)
 * License      :   GPL
 *                  This program is free software; you can redistribute it and/or 
 *                  modify it under the terms of the GNU General Public
 *                  License as published by the Free Software Foundation;
 *                  See the GNU General Public License 
 *                  (http://www.gnu.org/copyleft/gpl.html) for more details.
 *
 */
#endregion

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Alchemi.Core.Utility;

namespace Alchemi.Core.Owner
{
    /// <summary>
    /// The GConnection class represents a connection to a Alchemi manager.
    /// It is a container for the properties such as host, port, securityCredentials etc... used to connect to the manager.
    /// </summary>
    [Serializable]
    public partial class GConnection : Component
    {
        #region Constructors
        public GConnection()
        {
            InitializeComponent();
        }

        public GConnection(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// Creates a new GConnection
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public GConnection(string host, int port, string username, string password)
        {
            _Host = host;
            _Port = port;
            _Username = username;
            _Password = password;
        } 
        #endregion



        #region Property - Host
        private string _Host = "localhost";
        /// <summary>
        /// Gets or sets the host name of the Alchemi manager
        /// </summary>
        public string Host
        {
            get { return _Host; }
            set { _Host = value; }
        } 
        #endregion


        #region Property - Port
        private int _Port = 9000;
        /// <summary>
        /// Gets or sets the port number of the Alchemi manager
        /// </summary>
        public int Port
        {
            get { return _Port; }
            set { _Port = value; }
        } 
        #endregion


        #region Property - Username
        private string _Username = "";
        /// <summary>
        /// Gets or sets the username used to connect to the manager
        /// </summary>
        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        } 
        #endregion


        #region Property - Password
        private string _Password = "";
        /// <summary>
        /// Gets or sets the password used to connect to the manager
        /// </summary>
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        } 
        #endregion


        #region Property - RemoteEP
        /// <summary>
        /// Gets the EndPoint object associated with this GConnection.
        /// </summary>
        public EndPoint RemoteEP
        {
            get
            {
                return new EndPoint(_Host, _Port, RemotingMechanism.TcpBinary);
            }
        } 
        #endregion


        #region Property - Credentials
        /// <summary>
        /// Gets the SecurityCredentials object associated with this GConnection.
        /// </summary>
        public SecurityCredentials Credentials
        {
            get
            {
                return new SecurityCredentials(_Username, _Password);
            }
        } 
        #endregion



        /// <summary>
        /// Gets an instance of the GConnection class, from values input through the console.
        /// </summary>
        /// <param name="defaultHost">The default host.</param>
        /// <param name="defaultPort">The default port.</param>
        /// <param name="defaultUsername">The default username.</param>
        /// <param name="defaultPassword">The default password.</param>
        /// <returns>GConnection</returns>
        public static GConnection FromConsole(string defaultHost, string defaultPort, string defaultUsername, string defaultPassword)
        {
            string host = Utils.ValueFromConsole("Host", defaultHost);
            string port = Utils.ValueFromConsole("Port", defaultPort);
            string username = Utils.ValueFromConsole("Username", defaultUsername);
            string password = Utils.ValueFromConsole("Password", defaultPassword);

            Console.WriteLine();

            return new GConnection(host, int.Parse(port), username, password);
        }
    }
}
