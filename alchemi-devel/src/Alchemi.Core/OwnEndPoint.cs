#region Alchemi copyright notice
/*
  Alchemi [.NET Grid Computing Framework]
  http://www.alchemi.net
  
  Copyright (c)  Akshay Luther (2002-2004) & Rajkumar Buyya (2003-to-date), 
  GRIDS Lab, The University of Melbourne, Australia.
  
  Maintained and Updated by: Krishna Nadiminti (2005-to-date)
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
	/// <summary>
	/// Represents the end point of the local node
	/// </summary>
	[Serializable]
    public class OwnEndPoint
    {
        private int _Port;
        private RemotingMechanism _RemotingMechanism;

        //-----------------------------------------------------------------------------------------------    

		/// <summary>
		/// Gets or sets the port number of the connection to this node
		/// </summary>
        public int Port
        {
            get { return _Port; }
            set { _Port = value; }
        }
    
		/// <summary>
		/// Gets or sets the remoting mechanism used to connect to this node
		/// </summary>
        public RemotingMechanism RemotingMechanism
        {
            get { return _RemotingMechanism; }
            set { _RemotingMechanism = value; }
        }
    
        //-----------------------------------------------------------------------------------------------    
    
		/// <summary>
		/// Creates an instance of the OwnEndPoint
		/// </summary>
		/// <param name="port"></param>
		/// <param name="remotingMechanism"></param>
        public OwnEndPoint(int port, RemotingMechanism remotingMechanism)
        {
            _Port = port;
            _RemotingMechanism = remotingMechanism;
        }

        //-----------------------------------------------------------------------------------------------    

		/// <summary>
		/// Creates an instance of the OwnEndPoint
		/// </summary>
        public RemoteEndPoint ToRemoteEndPoint()
        {
            return new RemoteEndPoint(Dns.GetHostName(), this._Port, this._RemotingMechanism);
        }

		/// <summary>
		/// Returns  the host name of this end point.
		/// </summary>
		public string Host
		{
			get
			{
				return Dns.GetHostName();
			}
		}

    }
}
