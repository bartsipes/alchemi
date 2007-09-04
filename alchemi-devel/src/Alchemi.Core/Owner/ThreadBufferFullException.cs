using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Alchemi.Core.Owner
{
    /// <summary>
    /// ThreadBufferFullException class represents an exception thrown when attempting to
    /// add a thread to a full thread buffer.
    /// </summary>
    [Serializable]
    public class ThreadBufferFullException : Exception
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ThreadBufferFullException() 
            : base()
        {
        }

        /// <summary>
        /// Constructor that takes the given message.
        /// </summary>
        /// <param name="strMessage">message</param>
        public ThreadBufferFullException(string strMessage)
            : base(strMessage)
        {
        }

        /// <summary>
        /// Constructor that takes the given message and exception.
        /// </summary>
        /// <param name="strMessage">message</param>
        /// <param name="ex">exception</param>
        public ThreadBufferFullException(string strMessage, Exception oException)
            : base(strMessage, oException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadBufferFullException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
        /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
        protected ThreadBufferFullException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
