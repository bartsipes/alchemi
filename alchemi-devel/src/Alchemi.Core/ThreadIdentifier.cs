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

namespace Alchemi.Core
{
    [Serializable]
    public class ThreadIdentifier
    {
        private string _ApplicationId;
        private int _ThreadId;
        private int _Priority;
    
        //-----------------------------------------------------------------------------------------------        
    
        public string ApplicationId
        {
            get { return _ApplicationId; }
        }
    
        public int ThreadId
        {
            get { return _ThreadId; }
        }

        public int Priority
        {
            get { return _Priority; }
        }
    
        //-----------------------------------------------------------------------------------------------    

        public ThreadIdentifier(string applicationId, int threadId)
        {
            _ApplicationId = applicationId;
            _ThreadId = threadId;
            _Priority = -1;
        }

        public ThreadIdentifier(string applicationId, int threadId, int priority)
        {
            _ApplicationId = applicationId;
            _ThreadId = threadId;
            _Priority = priority;
        }
    }
}