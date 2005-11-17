#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	Exceptions.cs
* Project		:	Alchemi Core
* Created on	:	2003
* Copyright		:	Copyright © 2005 The University of Melbourne
*					This technology has been developed with the support of 
*					the Australian Research Council and the University of Melbourne
*					research grants as part of the Gridbus Project
*					within GRIDS Laboratory at the University of Melbourne, Australia.
* Author         :  Akshay Luther (akshayl@csse.unimelb.edu.au), Rajkumar Buyya (raj@csse.unimelb.edu.au), and Krishna Nadiminti (kna@csse.unimelb.edu.au)
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
