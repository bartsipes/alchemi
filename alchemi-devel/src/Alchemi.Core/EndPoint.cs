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
        private string _Host;
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
        public EndPoint(int port, RemotingMechanism remotingMechanism) : this(Dns.GetHostName(), port, remotingMechanism)
        {
        }

        public EndPoint(string host, int port, RemotingMechanism remotingMechanism) 
        {
            _Host = host;
            _Port = port;
            _RemotingMechanism = remotingMechanism;
        }

        //-----------------------------------------------------------------------------------------------    


		/// <summary>
		/// Returns  the host name of this end point.
		/// </summary>
		public string Host
		{
			get
			{
                return _Host;
			}
            set
            {
                _Host = value;
            }
		}

    }
}
