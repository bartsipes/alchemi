#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   Exceptions.cs
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
using System.Runtime.Serialization;

namespace Alchemi.Core
{
	/// <summary>
	/// Represents a exception that occured in Remoting
	/// </summary>
    public class RemotingException : Exception
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
    public class AuthenticationException : Exception
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
    public class AuthorizationException : Exception
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
    public class InvalidExecutorException : Exception
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

	/// <summary>
	/// Represents an exception that occurs when the application id is invalid.
	/// </summary>
	[Serializable]
	public class InvalidApplicationException : Exception
	{
		/// <summary>
		/// Creates an instance of the InvalidApplicationException
		/// </summary>
		/// <param name="message">message</param>
		/// <param name="innerException">innerException causing this exception</param>
		public InvalidApplicationException(string message, Exception innerException) : base(message, innerException) {}
		/// <summary>
		/// Creates an instance of the InvalidApplicationException
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public InvalidApplicationException(SerializationInfo info, StreamingContext context) : base(info, context) {}
	}

	/// <summary>
	/// Represents an exception that occurs when the thread id is invalid.
	/// </summary>
	[Serializable]
	public class InvalidThreadException : Exception
	{
		/// <summary>
		/// Creates an instance of the InvalidThreadException
		/// </summary>
		/// <param name="message">message</param>
		/// <param name="innerException">innerException causing this exception</param>
		public InvalidThreadException(string message, Exception innerException) : base(message, innerException) {}
		/// <summary>
		/// Creates an instance of the InvalidThreadException
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public InvalidThreadException(SerializationInfo info, StreamingContext context) : base(info, context) {}
	}

    //-----------------------------------------------------------------------------------------------          
  
	/// <summary>
	/// Represents an exception that occurs when the manager cannot connect back to the executor
	/// </summary>
    [Serializable]
    public class ConnectBackException : Exception
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

	/// <summary>
	/// Represents an exception used to indicate that there is an error related to saving/retrieving the Application Manifest file.
	/// </summary>
	[Serializable]
	public class ManifestFileException : Exception
	{
		/// <summary>
		/// Id of the ApplicationId
		/// </summary>
		public string ApplicationId;

		/// <summary>
		/// Creates an instance of the ManifestException class
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public ManifestFileException(string message, Exception innerException) : base(message, innerException){}
		/// <summary>
		/// Creates an instance of the ManifestFileException
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public ManifestFileException(SerializationInfo info, StreamingContext context) : base(info, context) {}
	}

	/// <summary>
	/// Represents an exception used to indicate that there is an error related to saving/retrieving the Thread Data file.
	/// </summary>
	[Serializable]
	public class ThreadDatFileException : Exception
	{
		/// <summary>
		/// Id of the Application
		/// </summary>
		public string ApplicationId;

		/// <summary>
		/// Id of the Thread
		/// </summary>
		public int ThreadId;

		/// <summary>
		/// Creates an instance of the ThreadDatFileException class
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public ThreadDatFileException(string message, Exception innerException) : base(message, innerException){}
		/// <summary>
		/// Creates an instance of the ThreadDatFileException
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public ThreadDatFileException(SerializationInfo info, StreamingContext context) : base(info, context) {}
	}

    /// <summary>
    /// Represents an exception used to indicate that there is an error communicating with the Executor
    /// </summary>
    [Serializable]
    public class ExecutorCommException : Exception
    {
        /// <summary>
        /// Id of the Executor
        /// </summary>
        public readonly string ExecutorId;

        /// <summary>
        /// Creates an instance of the ExecutorCommException class
        /// </summary>
        /// <param name="executorId"></param>
        /// <param name="innerException"></param>
        public ExecutorCommException(string executorId, Exception innerException)
            : base("", innerException)
        {
            ExecutorId = executorId;
        }

        public ExecutorCommException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ExecutorId = "";
        }
    }

    /// <summary>
    /// Represents an exception used to indicate that the EndPoint instance has properties set that are not compatible.
    /// </summary>
    [Serializable]
    public class InvalidEndPointException : Exception
    {
        /// <summary>
        /// The end point instance.
        /// </summary>
        public EndPoint InvalidEndPointInstance;

        /// <summary>
        /// Creates an instance of the InvalidEndPointException class
        /// </summary>
        /// <param name="message">The message with more details.</param>
        /// <param name="invalidEndPointInstance">An instance of the invalid EndPoint.</param>
        public InvalidEndPointException(string message, EndPoint invalidEndPointInstance)
            : base(message)
        {
            this.InvalidEndPointInstance = invalidEndPointInstance;
        }
    }

    /// <summary>
    /// Represents an exception used to indicate that there are more than one EndPointConfigurations with remoting mechanism set to remoting
    /// </summary>
    [Serializable]
    public class DoubleRemotingEndPointException : Exception
    {
        /// <summary>
        /// Creates an instance of the DoubleRemotingEndPointException class
        /// </summary>
        /// <param name="message">The message with more details.</param>
        public DoubleRemotingEndPointException(string message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Represents an exception used to indicate that there was an error trying to deserialize the exception object sent from another node.
    /// </summary>
    [Serializable]
    public class RemoteExceptionDeserializeException : Exception
    {
        /// <summary>
        /// Creates an instance of the DoubleRemotingEndPointException class
        /// </summary>
        /// <param name="message">The message with more details.</param>
        /// <param name="innerException">The exception holding the details of the desrialilization failure.</param>
        public RemoteExceptionDeserializeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Wraps the exception that ocurred on anothed alchemi node.
    /// <remarks>This is needed to send exceptions betwen alchemi nodes when using WCF communication.</remarks>
    /// </summary>
    [Serializable]
    public class RemoteException : Exception, ISerializable
    {
        private string InternalExceptionDescription = null;
        //{
        //    get
        //    {
        //        return (byte[])Data["InternalExceptionDescription"];
        //    }
        //    set
        //    {
        //        Data["InternalExceptionDescription"] = value;
        //    }
        //}
        //public string Message
        //{
        //    get
        //    {
        //        return (string)Data["Message"];
        //    }
        //    set
        //    {
        //        Data["Message"] = value;
        //    }
        //}

        #region Constructor
        /// <summary>
        /// Creates an instance of the RemoteException class
        /// </summary>
        /// <param name="message">The message with more details.</param>
        /// <param name="innerException">The original exception.</param>
        public RemoteException(string message, Exception innerException)
            : base(message)
        {            
            Serialize(innerException);
        }
        /// <summary>
        /// The empty contructor for serialization.
        /// </summary>
        public RemoteException(): base()
        {
        }
        /// <summary>
        /// Constructor for deserialization with WCF.
        /// </summary>
        /// <param name="info">SerializationInfo</param>
        /// <param name="context">StreamingContext</param>
        public RemoteException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            //info.GetValue("Additional", typeof(byte[]));
            InternalExceptionDescription = info.GetString("Additional");
        }
        #endregion

        #region Property - OriginalRemoteException
        /// <summary>
        /// The original exception that occured on a remote alchemi node.
        /// </summary>
        public Exception OriginalRemoteException
        {
            get
            {
                if (InternalExceptionDescription == null || InternalExceptionDescription == string.Empty)
                    return new RemoteExceptionDeserializeException("No data of the original exception was sent.", null);
                else
                    return Deserialize();
            }
        }
        #endregion

        #region Method - Serialize
        private void Serialize(Exception ex)
        {
            System.IO.MemoryStream ms = null;
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = null;

            try
            {
                ms = new System.IO.MemoryStream();
                bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                bf.Serialize(ms, ex);
                InternalExceptionDescription = Convert.ToBase64String(ms.ToArray());
            }
            finally
            {
                if (ms != null)
                {
                    ms.Close();
                    ms.Dispose();
                    ms = null;
                }
                if (bf != null)
                {
                    bf = null;
                }
            }
        }
        #endregion

        #region Method - Deserialize
        private Exception Deserialize()
        {
            System.IO.MemoryStream ms = null;
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = null;

            try
            {
                ms = new System.IO.MemoryStream(Convert.FromBase64String(InternalExceptionDescription));
                bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                object o = bf.Deserialize(ms);
                return o as Exception;
            }
            catch (Exception ex)
            {
                return new RemoteExceptionDeserializeException("Failed to receive remote exception.", ex);
            }
            finally
            {
                if (ms != null)
                {
                    ms.Close();
                    ms.Dispose();
                    ms = null;
                }
                if (bf != null)
                {
                    bf = null;
                }
            }
        }
        #endregion


        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Additional", InternalExceptionDescription);
        }

        #endregion
    }
}
