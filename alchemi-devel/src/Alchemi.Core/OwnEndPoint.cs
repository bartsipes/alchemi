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

namespace Alchemi.Core
{
    public class OwnEndPoint
    {
        private int _Port;
        private RemotingMechanism _RemotingMechanism;

        //-----------------------------------------------------------------------------------------------    

        public int Port
        {
            get { return _Port; }
            set { _Port = value; }
        }
    
        public RemotingMechanism RemotingMechanism
        {
            get { return _RemotingMechanism; }
            set { _RemotingMechanism = value; }
        }
    
        //-----------------------------------------------------------------------------------------------    
    
        public OwnEndPoint(int port, RemotingMechanism remotingMechanism)
        {
            _Port = port;
            _RemotingMechanism = remotingMechanism;
        }

        //-----------------------------------------------------------------------------------------------    

        public RemoteEndPoint ToRemoteEndPoint()
        {
            return new RemoteEndPoint(Dns.GetHostName(), this._Port, this._RemotingMechanism);
        }
    }
}
