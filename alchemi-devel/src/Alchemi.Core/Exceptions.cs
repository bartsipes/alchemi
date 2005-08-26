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
using System.Runtime.Serialization;

namespace Alchemi.Core
{
	/// <summary>
	/// Represents a exception that occured in Remoting
	/// </summary>
    public class RemotingException : ApplicationException
    {
		/// <summary>
		/// Creates an instance of the RemotingException
		/// </summary>
		/// <param name="message">error message</param>
		/// <param name="innerException">innerException causing this exception</param>
        public RemotingException (string message, Exception innerException) : base(message, innerException) {}
    }
    
    //-----------------------------------------------------------------------------------------------              
    
	/// <summary>
	/// Represents an exception that occured during Authentication
	/// </summary>
    [Serializable]
    public class AuthenticationException : ApplicationException
    {
		/// <summary>
		///  Creates an instance of the AuthenticationException
		/// </summary>
		/// <param name="message">error message</param>
		/// <param name="innerException">innerException causing this exception</param>
        public AuthenticationException (string message, Exception innerException) : base(message, innerException) {}
        /// <summary>
        /// Creates an instance of the AuthenticationException
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
		public AuthenticationException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }

    //-----------------------------------------------------------------------------------------------          

	/// <summary>
	/// Represents an exception that occured during authorization (checking user permissions).
	/// </summary>
    [Serializable]
    public class AuthorizationException : ApplicationException
    {
		/// <summary>
		/// Creates an instance of the AuthorizationException
		/// </summary>
		/// <param name="message">error message</param>
		/// <param name="innerException">innerException causing this exception</param>
        public AuthorizationException (string message, Exception innerException) : base(message, innerException) {}
        /// <summary>
        /// Creates an instance of the AuthorizationException
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
		public AuthorizationException (SerializationInfo info, StreamingContext context) : base(info, context) {}
    }

    //-----------------------------------------------------------------------------------------------          
    
	/// <summary>
	/// Represents an exception that occurs when the executor id is invalid.
	/// </summary>
    [Serializable]
    public class InvalidExecutorException : ApplicationException
    {
		/// <summary>
		/// Creates an instance of the InvalidExecutorException
		/// </summary>
		/// <param name="message">error message</param>
		/// <param name="innerException">innerException causing this exception</param>
        public InvalidExecutorException(string message, Exception innerException) : base(message, innerException) {}
        /// <summary>
        /// Creates an instance of the InvalidExecutorException
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
		public InvalidExecutorException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }

    //-----------------------------------------------------------------------------------------------          
  
	/// <summary>
	/// Represents an exception that occurs when the manager cannot connect back to the executor
	/// </summary>
    [Serializable]
    public class ConnectBackException : ApplicationException
    {
		/// <summary>
		/// Creates an instance of the ConnectBackException
		/// </summary>
		/// <param name="message">error message</param>
		/// <param name="innerException">innerException causing this exception</param>
		public ConnectBackException (string message, Exception innerException) : base(message, innerException) {}
        /// <summary>
        /// Creates an instance of the ConnectBackException
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
		public ConnectBackException (SerializationInfo info, StreamingContext context) : base(info, context) {}
    }

    //-----------------------------------------------------------------------------------------------          

    /*
    [Serializable]
    public class FrameworkException : ApplicationException
    {
        public FrameworkException (string message, Exception innerException) : base(message, innerException) {}
        public FrameworkException (SerializationInfo info, StreamingContext context) : base(info, context) {}
    }
    */
}
