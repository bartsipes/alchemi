#region Alchemi copyright notice
/*
  Alchemi [.NET Grid Computing Framework]
  http://www.alchemi.net
  
  Copyright (c) 2002-2004 Akshay Luther & 2003-2004 Rajkumar Buyya 
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
    public class RemotingException : ApplicationException
    {
        public RemotingException (string message, Exception innerException) : base(message, innerException) {}
    }
    
    //-----------------------------------------------------------------------------------------------              
    
    [Serializable]
    public class AuthenticationException : ApplicationException
    {
        public AuthenticationException (string message, Exception innerException) : base(message, innerException) {}
        public AuthenticationException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }

    //-----------------------------------------------------------------------------------------------          

    [Serializable]
    public class AuthorizationException : ApplicationException
    {
        public AuthorizationException (string message, Exception innerException) : base(message, innerException) {}
        public AuthorizationException (SerializationInfo info, StreamingContext context) : base(info, context) {}
    }

    //-----------------------------------------------------------------------------------------------          
    
    [Serializable]
    public class InvalidExecutorException : ApplicationException
    {
        public InvalidExecutorException(string message, Exception innerException) : base(message, innerException) {}
        public InvalidExecutorException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }

    //-----------------------------------------------------------------------------------------------          
  
    [Serializable]
    public class ConnectBackException : ApplicationException
    {
        public ConnectBackException (string message, Exception innerException) : base(message, innerException) {}
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
