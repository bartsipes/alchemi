#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   EndPoint.cs
 * Project      :   Alchemi.Core
 * Created on   :   2003
 * Copyright    :   Copyright © 2006 The University of Melbourne
 *                  This technology has been developed with the support of 
 *                  the Australian Research Council and the University of Melbourne
 *                  research grants as part of the Gridbus Project
 *                  within GRIDS Laboratory at the University of Melbourne, Australia.
 * Author       :   Akshay Luther (akshayl@csse.unimelb.edu.au)
 *                  Krishna Nadiminti (kna@csse.unimelb.edu.au)
 *                  Rajkumar Buyya (raj@csse.unimelb.edu.au)
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
using System.Net;

namespace Alchemi.Core
{
	/// <summary>
	/// Represents the end point of a node
	/// </summary>
	[Serializable]
    public class EndPoint
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="EndPoint"/> class.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <param name="remotingMechanism">The remoting mechanism.</param>
        public EndPoint(int port, RemotingMechanism remotingMechanism)
            : this(Dns.GetHostName(), port, remotingMechanism)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EndPoint"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        /// <param name="remotingMechanism">The remoting mechanism.</param>
        public EndPoint(string host, int port, RemotingMechanism remotingMechanism)
        {
            _Host = host;
            _Port = port;
            _RemotingMechanism = remotingMechanism;
        }
        public EndPoint()
        {
        }
        #endregion


        #region Constants
        //constants for protocol names
        public const string ProtocolNamedPipes = "net.pipe";
        public const string ProtocolHttp = "http";
        public const string ProtocolHttps = "https";
        public const string ProtocolP2P = "net.p2p";
        public const string ProtocolMSMQ = "net.msmq";
        public const string ProtocolTcp = "net.tcp";
        #endregion


        #region Property - Host
        private string _Host;
        /// <summary>
        /// Returns  the host name of this end point.
        /// </summary>
        public string Host
        {
            get { return _Host; }
            set { _Host = value; }
        } 
        #endregion
        

        #region Property - Port
        private int _Port;
        /// <summary>
        /// Gets or sets the port number of the connection to this node
        /// </summary>
        public int Port
        {
            get { return _Port; }
            set { _Port = value; }
        } 
        #endregion


        #region Property - RemotingMechanism
        private RemotingMechanism _RemotingMechanism;
        /// <summary>
        /// Gets or sets the remoting mechanism used to connect to this node
        /// </summary>
        public RemotingMechanism RemotingMechanism
        {
            get { return _RemotingMechanism; }
            set { _RemotingMechanism = value; }
        } 
        #endregion


        #region Property - LocalAddressPart
        private string _LocalAddressPart;
        /// <summary>
        /// Gets or sets the part of the address that is used on a computer to get the specified instance of the manager.
        /// </summary>
        /// <remarks>This value is used only with WCF.</remarks>
        public string LocalAddressPart
        {
            get { return _LocalAddressPart; }
            set { _LocalAddressPart = value; }
        }
        #endregion


        #region Property - Binding
        private WCFBinding _Binding = WCFBinding.None;
        /// <summary>
        /// Gets or sets the WCF binding used.
        /// </summary>
        /// <remarks>This value is used only with WCF.</remarks>
        public WCFBinding Binding
        {
            get { return _Binding; }
            set { _Binding = value; }
        }
        #endregion


        #region Propety - Protocol
        private string _Protocol = string.Empty;
        /// <summary>
        /// Gets or sets the protocol to use.
        /// </summary>
        /// <remarks>This value is used only with WCF.</remarks>
        public string Protocol
        {
            get { return _Protocol; }
            set { _Protocol = value; }
        }
        #endregion


        #region Property - HostNameForPublishing
        private string _HostNameForPublishing = "localhost"; //localhost should allways do, but just in case...
        /// <summary>
        /// Return the host name of this EndPoint, as it should be used for publishing.
        /// </summary>
        public string HostNameForPublishing
        {
            get
            {
                return _HostNameForPublishing;
            }

            set
            {
                _HostNameForPublishing = value;
            }
        }
        #endregion


        #region Property - BindingConfigurationName
        private string _BindingConfigurationName = string.Empty;
        /// <summary>
        /// The name of the binding in configuration file to use for setting the binding of this end point.
        /// </summary>
        public string BindingConfigurationName
        {
            get
            {
                return _BindingConfigurationName;
            }

            set
            {
                _BindingConfigurationName = value;
            }
        }
        #endregion


        #region Property - ServiceConfigurationName
        private string _ServiceConfigurationName = "customAlchemiConfig";
        /// <summary>
        ///  The name of the service in configuration file to use for setting this service.
        /// </summary>
        public string ServiceConfigurationName
        {
            get
            {
                return _ServiceConfigurationName;
            }

            set
            {
                _ServiceConfigurationName = value;
            }
        }
        #endregion


        #region Property - BindingSettingType
        private WCFBindingSettingType _BindingSettingType = WCFBindingSettingType.Default;
        /// <summary>
        /// How to set the Binding needed for accessing or publishing a WCF EndPoint.
        /// </summary>
        public WCFBindingSettingType BindingSettingType
        {
            get
            {
                return _BindingSettingType;
            }

            set
            {
                _BindingSettingType = value;
            }
        }
        #endregion


        #region Property - FullAddress
        /// <summary>
        /// Gets the full url of the end point.
        /// </summary>
        public string FullAddress
        {
            get
            {
                return GetFullAddress(false);
            }
        }
        #endregion


        #region Property - FullPublishingAddress
        /// <summary>
        /// Gets the full url of the end point - using 'localhost' or the value in HostNameForPublishing.
        /// </summary>
        public string FullPublishingAddress
        {
            get
            {
                return GetFullAddress(true);
            }
        }
        #endregion


        #region Method - GetFullAddress
        /// <summary>
        /// Get the Full addres of the node.
        /// </summary>
        /// <param name="forPublish">Should use host name for publishing or host name for accessing the EndPoint.</param>
        /// <returns></returns>
        private string GetFullAddress(bool forPublish)
        {
            string ret = string.Empty;

            string hostName = Host;
            if (forPublish)
                hostName = HostNameForPublishing;
            switch (RemotingMechanism)
            {
                case RemotingMechanism.TcpBinary:
                    {
                        ret = string.Empty;
                        break;
                    }
                case RemotingMechanism.WCFCustom:
                    {
                        return GetCustomWCFAddress(hostName, LocalAddressPart, Port, Protocol, Binding);
                        break;
                    }
                case RemotingMechanism.WCFTcp:
                    {
                        //check if makes sense
                        if (Protocol != ProtocolTcp)
                            throw new InvalidEndPointException(
                                String.Format("Protocol WCF binding Missmatch! Cannot use protocol '{0}' for 'RemotingMechanism.WCFTcp'. Only protocol 'net.tcp' is allowed for that ReotingMechanism.", _Protocol),
                                this
                                );

                        return GetCustomWCFAddress(hostName, LocalAddressPart, Port, ProtocolTcp, Binding);
                        break;
                    }
                case RemotingMechanism.WCFHttp:
                    {
                        //check if makes sense
                        if (Protocol != ProtocolHttp && Protocol != ProtocolHttps)
                            throw new InvalidEndPointException(
                                String.Format("Protocol WCF binding Missmatch! Cannot use protocol '{0}' for 'RemotingMechanism.WCFHttp'. Only protocols 'http' and 'https' are allowed for that ReotingMechanism.", _Protocol),
                                this
                                );

                        return GetCustomWCFAddress(hostName, LocalAddressPart, Port, Protocol, Binding);
                        break;
                    }
                case RemotingMechanism.WCF:
                    {
                        //TODO: check if makes sence

                        return GetCustomWCFAddress(hostName, LocalAddressPart, Port, Protocol, Binding);
                        break;
                    }
            }

            return ret;
        }
        #endregion


        #region Overriden Method - ToString
        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("Alchemi.Core.EndPoint(RemotingMechanism={0}; Protocol={1}; Host={2}; Port={3}; LocalAddresPart={4}; WCFBindingType={5}; FullAddres={6};)",
                this.RemotingMechanism.ToString(), //0
                this.Protocol, //1
                this.Host, //2
                this.Port.ToString(), //3
                this.LocalAddressPart, //4
                this.Binding.ToString(),//5
                this.FullAddress //6
            );
        }
        #endregion


        #region Static Method - GetCustomWCFAddress
        /// <summary>
        /// Gets the full WCF addres from internal data.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="localAddressPart"></param>
        /// <param name="port"></param>
        /// <param name="protocol"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
        public static string GetCustomWCFAddress(string host, string localAddressPart, int port, string protocol, WCFBinding binding)
        {
            return Utility.WCFUtils.ComposeAddress(protocol, host, port, localAddressPart, binding);
        }
        #endregion


        #region Static Method - GetDefaultBindingForProtocol
        /// <summary>
        /// Get the default binding for the protocol.
        /// </summary>
        /// <param name="protocol"></param>
        /// <returns></returns>
        public static WCFBinding GetDefaultBindingForProtocol(string protocol)
        {
            WCFBinding ret = WCFBinding.None;

            switch (protocol)
            {
                case ProtocolHttp:
                    ret = WCFBinding.BasicHttpBinding;
                    break;
                case ProtocolHttps:
                    ret = WCFBinding.BasicHttpBinding;
                    break;
                case ProtocolMSMQ:
                    ret = WCFBinding.NetMsmqBinding;
                    break;
                case ProtocolNamedPipes:
                    ret = WCFBinding.NetNamedPipeBinding;
                    break;
                case ProtocolP2P:
                    ret = WCFBinding.NetPeerTcpBinding;
                    break;
                case ProtocolTcp:
                    ret = WCFBinding.NetTcpBinding;
                    break;
            }

            return ret;
        }
        #endregion


        #region Static Method - GetDefaultProtocolForBinding
        /// <summary>
        /// Gets the default protocol for the binding.
        /// </summary>
        /// <remarks>Some bindings can have multiple protocol http, https. This is just the default protocol for binding.</remarks>
        /// <param name="binding"></param>
        /// <returns>Protocol name that can be used directly in address of the end point</returns>
        public static string GetDefaultProtocolForBinding(WCFBinding binding)
        {
            string ret = string.Empty;
            switch (binding)
            {
                case WCFBinding.BasicHttpBinding:
                    ret = ProtocolHttp;
                    break;
                case WCFBinding.MsmqIntegrationBinding:
                    ret = ProtocolMSMQ;
                    break;
                case WCFBinding.NetMsmqBinding:
                    ret = ProtocolMSMQ;
                    break;
                case WCFBinding.NetNamedPipeBinding:
                    ret = ProtocolNamedPipes;
                    break;
                case WCFBinding.NetPeerTcpBinding:
                    ret = ProtocolP2P;
                    break;
                case WCFBinding.NetTcpBinding:
                    ret = ProtocolTcp;
                    break;
                case WCFBinding.WSDualHttpBinding:
                    ret = ProtocolHttp;
                    break;
                case WCFBinding.WSFederationHttpBinding:
                    ret = ProtocolHttp;
                    break;
                case WCFBinding.WSHttpBinding:
                    ret = ProtocolHttp;
                    break;
            }

            return ret;
        }
        #endregion

    }
}
