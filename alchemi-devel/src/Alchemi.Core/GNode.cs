#region Alchemi copyright notice
/*
  Alchemi [.NET Grid Computing Framework]
  Copyright (c) 2002-2004 Akshay Luther
  http://www.alchemi.net
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
using System.Net;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Lifetime;

namespace Alchemi.Core
{
    public abstract class GNode : System.ComponentModel.Component
    {
        //----------------------------------------------------------------------------------------------- 
        // member variables
        //----------------------------------------------------------------------------------------------- 
        
        private const string RemoteObjPrefix = "Alchemi_Node";
        private bool _ChannelRegistered = false;
        private IManager _Manager = null;
        private RemoteEndPoint _ManagerEP = null;
        private OwnEndPoint _OwnEP = null;
        private TcpChannel _Channel = null;
        private SecurityCredentials _Credentials;
        private bool _Initted = false;
        private GConnection _Connection;

        //----------------------------------------------------------------------------------------------- 
        // properties
        //----------------------------------------------------------------------------------------------- 
    
        public IManager Manager
        {
            get { return _Manager; }
        }
    
        public RemoteEndPoint ManagerEP
        {
            get { return _ManagerEP; }
        }

        public OwnEndPoint OwnEP
        {
            get { return _OwnEP; }
        }

        public SecurityCredentials Credentials
        {
            get { return _Credentials; }
        }

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
                    _ManagerEP = new RemoteEndPoint(value.Host, value.Port, RemotingMechanism.TcpBinary);
                }
            }
        }

        //----------------------------------------------------------------------------------------------- 
        // constructors
        //----------------------------------------------------------------------------------------------- 

        public GNode() {}
        
        public GNode(RemoteEndPoint managerEP, OwnEndPoint ownEP, SecurityCredentials credentials)
        {
            _OwnEP = ownEP;
            _Credentials = credentials;
            _ManagerEP = managerEP;
            Init();
        }

        public GNode(GConnection connection)
        {
            Connection = connection;
            Init();
        }
        
        //----------------------------------------------------------------------------------------------- 
        // public methods
        //----------------------------------------------------------------------------------------------- 

        protected void Init()
        {
            if (_Initted)
            {
                return;
                //throw new InvalidOperationException("Node has already been initialised.");
            }
            
            if (_Connection != null)
            {
                _Credentials = new SecurityCredentials(_Connection.Username, _Connection.Password);
                _ManagerEP = new RemoteEndPoint(_Connection.Host, _Connection.Port, RemotingMechanism.TcpBinary);
            }
            GetManagerRef();
            RemoteSelf();
            _Initted = true;
        }
        
        //----------------------------------------------------------------------------------------------- 
        
        public static GNode GetRemoteRef(RemoteEndPoint remoteEP)
        {
            switch (remoteEP.RemotingMechanism)
            {
                case RemotingMechanism.TcpBinary:
                    string uri = "tcp://" + remoteEP.Host + ":" + remoteEP.Port + "/" + RemoteObjPrefix;
                    return (GNode) Activator.GetObject(typeof(GNode), uri);
                default:
                    return null;
            }
        }
        
        //-----------------------------------------------------------------------------------------------          
        
        public static IManager GetRemoteManagerRef(RemoteEndPoint remoteEP)
        {
            IManager manager;

            try
            {
                manager = (IManager) GetRemoteRef(remoteEP);
                manager.PingManager();
            }
            catch (Exception e)
            {
                throw new RemotingException("Could not connect to Manager.", e);
            }

            return manager;
        }

        //-----------------------------------------------------------------------------------------------          
        
        public override object InitializeLifetimeService()
        {
            return null;
        }

        //----------------------------------------------------------------------------------------------- 
        // private methods
        //----------------------------------------------------------------------------------------------- 

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
                                ChannelServices.RegisterChannel(_Channel);
                                _ChannelRegistered = true;
                            }
                            catch (Exception e)
                            {
                                if ((e.Message == "The channel tcp is already registered.") || (e.Message == "Only one usage of each socket address (protocol/network address/port) is normally permitted"))
                                {
                                    _ChannelRegistered = true;
                                }
                                else
                                {
                                    UnRemoteSelf();
                                    throw new RemotingException("Could not register channel while trying to remote self.", e);
                                }
                            }
                        }
            
                        if (_ChannelRegistered)
                        {
                            try
                            {
                                RemotingServices.Marshal(this, RemoteObjPrefix);
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

        //-----------------------------------------------------------------------------------------------          
    
        private void GetManagerRef()
        {
            if (_ManagerEP != null)
            {
                _Manager = GetRemoteManagerRef(_ManagerEP);
            }
        }

        //-----------------------------------------------------------------------------------------------          

        protected void UnRemoteSelf()
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
                        catch {}
                        RemotingServices.Disconnect(this);
                        break;
                }
            }
        }
    }
}