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

namespace Alchemi.Core
{
	/// <summary>
	/// Represents a remote end point.
	/// </summary>
    [Serializable]
    public class RemoteEndPoint : OwnEndPoint
    {
        private string _Host;
        //private int _Port;
        //private RemotingMechanism _RemotingMechanism;
    
        //-----------------------------------------------------------------------------------------------    
    
		/// <summary>
		/// Gets or sets the hostname of the remote end point
		/// </summary>
        public new string Host
        {
            get { return _Host; } 
            set { _Host = value; }
        }

//        public int Port
//        {
//            get { return _Port; }
//            set { _Port = value; }
//        }
//
//        public RemotingMechanism RemotingMechanism
//        {
//            get { return _RemotingMechanism; }
//            set { _RemotingMechanism = value; }
//        }

        //-----------------------------------------------------------------------------------------------    
		
		/// <summary>
		/// Creates an instance of the RemoteEndPoint class
		/// </summary>
		/// <param name="host"></param>
		/// <param name="port"></param>
		/// <param name="remotingMechanism"></param>
        public RemoteEndPoint(string host, int port, RemotingMechanism remotingMechanism) : base(port,remotingMechanism)
        {
            _Host = host;
            //_Port = port;
            //_RemotingMechanism = remotingMechanism;
        }
    }
}
