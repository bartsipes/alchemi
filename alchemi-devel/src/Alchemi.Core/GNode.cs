#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   GNode.cs
 * Project      :   Alchemi.Core
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
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

using Alchemi.Core.Owner;

namespace Alchemi.Core
{
    /// <summary>
    /// Represents a grid node
    /// </summary>
    public abstract partial class GNode : Component
    {
        // Create a logger for use in this class
        private static readonly Logger logger = new Logger();

        private const string DefaultRemoteObjectPrefix = "Alchemi_Node";
        private string _RemoteObjPrefix = DefaultRemoteObjectPrefix;
        private bool _ChannelRegistered = false;
        private TcpChannel _Channel = null;
        private bool _Initted = false;
        

        #region Constructors
        /// <summary>
        /// Creates a new instance of the GNode class
        /// </summary>
        protected GNode()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GNode"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        protected GNode(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// Creates a new instance of the GNode class
        /// </summary>
        /// <param name="managerEP">Manager end point</param>
        /// <param name="ownEP">Own end point</param>
        /// <param name="credentials">The credentials.</param>
        protected GNode(EndPoint managerEP, EndPoint ownEP, SecurityCredentials credentials)
            : this(managerEP, ownEP, credentials, DefaultRemoteObjectPrefix)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GNode"/> class.
        /// </summary>
        /// <param name="managerEP">The manager EP.</param>
        /// <param name="ownEP">The own EP.</param>
        /// <param name="credentials">The credentials.</param>
        /// <param name="remoteObjectPrefix">The remote object prefix.</param>
        protected GNode(EndPoint managerEP, EndPoint ownEP, SecurityCredentials credentials, string remoteObjectPrefix)
        {
            _OwnEP = ownEP;
            _Credentials = credentials;
            _ManagerEP = managerEP;
            _RemoteObjPrefix = remoteObjectPrefix;
            Init();
        }

        /// <summary>
        /// Creates a new instance of the GConnection class
        /// </summary>
        /// <param name="connection">The connection.</param>
        protected GNode(GConnection connection)
        {
            Connection = connection;
            Init();
        } 
        #endregion



        #region Property - Manager
        private IManager _Manager = null;
        /// <summary>
        /// Gets the manager
        /// </summary>
        public IManager Manager
        {
            get { return _Manager; }
        } 
        #endregion


        #region Property - ManagerEP
        private EndPoint _ManagerEP = null;
        /// <summary>
        /// Gets the manager end point
        /// </summary>
        public EndPoint ManagerEP
        {
            get { return _ManagerEP; }
        } 
        #endregion


        #region Property - OwnEP
        private EndPoint _OwnEP = null;
        /// <summary>
        /// Gets the node end point
        /// </summary>
        public EndPoint OwnEP
        {
            get { return _OwnEP; }
        } 
        #endregion


        #region Property - Credentials
        private SecurityCredentials _Credentials;
        /// <summary>
        /// Gets the security credentials
        /// </summary>
        public SecurityCredentials Credentials
        {
            get { return _Credentials; }
        } 
        #endregion


        #region Property - Connection
        private GConnection _Connection;
        /// <summary>
        /// Gets or sets the GConnection
        /// </summary>
        public GConnection Connection
        {
            get
            {
                return _Connection;
            }
            set
            {
                _Connection = value;
                _OwnEP = null;
                if (value != null)
                {
                    _Credentials = new SecurityCredentials(value.Username, value.Password);
                    _ManagerEP = new EndPoint(value.Host, value.Port, RemotingMechanism.TcpBinary);
                }
            }
        } 
        #endregion



        #region Method - Init
        /// <summary>
        /// Initialised the remoted "node"
        /// </summary>
        protected void Init()
        {
            if (_Initted)
            {
                return;
            }

            if (_Connection != null)
            {
                _Credentials = new SecurityCredentials(_Connection.Username, _Connection.Password);
                _ManagerEP = new EndPoint(_Connection.Host, _Connection.Port, RemotingMechanism.TcpBinary);
            }
            GetManagerRef();
            RemoteSelf();
            _Initted = true;
        } 
        #endregion



        #region Method - GetRemoteRef
        /// <summary>
        /// Gets the reference to the remote node
        /// </summary>
        /// <param name="remoteEP">end point of the remote node</param>
        /// <returns>Node reference</returns>
        public static GNode GetRemoteRef(EndPoint remoteEP)
        {
            return GetRemoteRef(remoteEP, DefaultRemoteObjectPrefix);
        } 
        #endregion


        #region Method - GetRemoteRef
        /// <summary>
        /// Gets the remote ref.
        /// </summary>
        /// <param name="remoteEP">The remote endpoint.</param>
        /// <param name="remoteObjectPrefix">The remote object prefix.</param>
        /// <returns></returns>
        public static GNode GetRemoteRef(EndPoint remoteEP, string remoteObjectPrefix)
        {
            switch (remoteEP.RemotingMechanism)
            {
                case RemotingMechanism.TcpBinary:
                    string uri = "tcp://" + remoteEP.Host + ":" + remoteEP.Port + "/" + remoteObjectPrefix;
                    return (GNode)Activator.GetObject(typeof(GNode), uri);
                default:
                    return null;
            }
        } 
        #endregion


        #region Method - GetRemoteManagerRef
        /// <summary>
        /// Gets the reference to a remote manager
        /// </summary>
        /// <param name="remoteEP">end point of the remote manager</param>
        /// <returns>Manager reference</returns>
        public static IManager GetRemoteManagerRef(EndPoint remoteEP)
        {
            IManager manager;

            try
            {
                manager = (IManager)GetRemoteRef(remoteEP);
                manager.PingManager();
            }
            catch (Exception e)
            {
                throw new RemotingException("Could not connect to Manager.", e);
            }

            return manager;
        } 
        #endregion



        #region Method Override - InitializeLifetimeService
        /// <summary>
        /// Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns>
        /// An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease"/> used to control the lifetime policy for this instance. This is the current lifetime service object for this instance if one exists; otherwise, a new lifetime service object initialized to the value of the <see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime"/> property.
        /// </returns>
        /// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
        /// <PermissionSet>
        /// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure"/>
        /// </PermissionSet>
        public override object InitializeLifetimeService()
        {
            return null;
        } 
        #endregion



        #region Method - GetManagerRef
        private void GetManagerRef()
        {
            if (_ManagerEP != null)
            {
                _Manager = GetRemoteManagerRef(_ManagerEP);
            }
        }
        #endregion



        #region Method - RemoteSelf
        /// <summary>
        /// Remotes this instance across a channel
        /// - if the own end point is valid: 
        ///  - register a new channel, if not already done.
        ///  - if successful, then marshal this GNode object over the remoting channel
        /// </summary>
        private void RemoteSelf()
        {
            if (_OwnEP != null)
            {
                switch (_OwnEP.RemotingMechanism)
                {
                    case (RemotingMechanism.TcpBinary):
                        if (!_ChannelRegistered)
                        {
                            try
                            {
                                _Channel = new TcpChannel(_OwnEP.Port);
                                //Hashtable properties = new Hashtable();

                                //// the name must be Empty in order to allow multiple TCP channels
                                //properties.Add("name", String.Empty);
                                //properties.Add("port", _OwnEP.Port);

                                //_Channel = new TcpChannel(
                                //    properties, 
                                //    new BinaryClientFormatterSinkProvider(), 
                                //    new BinaryServerFormatterSinkProvider());

                                ChannelServices.RegisterChannel(_Channel, false);
                                _ChannelRegistered = true;
                            }
                            catch (Exception e)
                            {
                                if (
                                    object.ReferenceEquals(e.GetType(), typeof(System.Runtime.Remoting.RemotingException)) /* assuming: "The channel tcp is already registered." */
                                    |
                                    object.ReferenceEquals(e.GetType(), typeof(SocketException)) /* assuming: "Only one usage of each socket address (protocol/network address/port) is normally permitted" */
                                    )
                                {
                                    _ChannelRegistered = true;
                                }
                                else
                                {
                                    UnRemoteSelf();
                                    throw new RemotingException("Could not register channel while trying to remote self: " + e.Message, e);
                                }
                            }
                        }

                        if (_ChannelRegistered)
                        {
                            try
                            {
                                logger.Info("Trying to publish a GNode at : " + _RemoteObjPrefix);
                                RemotingServices.Marshal(this, _RemoteObjPrefix);
                                logger.Info("GetObjectURI from remoting services : " + RemotingServices.GetObjectUri(this));
                                logger.Info("Server object type: " + RemotingServices.GetServerTypeForUri(RemotingServices.GetObjectUri(this)).FullName);
                            }
                            catch (Exception e)
                            {
                                UnRemoteSelf();
                                throw new RemotingException("Could not remote self.", e);
                            }
                        }
                        break;
                }
            }
        } 
        #endregion


        #region Method - UnRemoteSelf
        /// <summary>
        /// Unregister channel and disconnect remoting
        /// </summary>
        public void UnRemoteSelf() //TODO check if we need this protection level
        {
            if (_OwnEP != null)
            {
                switch (_OwnEP.RemotingMechanism)
                {
                    case RemotingMechanism.TcpBinary:
                        try
                        {
                            ChannelServices.UnregisterChannel(_Channel);
                        }
                        catch { }
                        RemotingServices.Disconnect(this);
                        break;
                }
                logger.Debug("Unremoting self...done");
            }
        } 
        #endregion
    }
}
