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
    public abstract class GThread : MarshalByRefObject
    {
        //----------------------------------------------------------------------------------------------- 
        // member variables
        //----------------------------------------------------------------------------------------------- 
        
        int _Id = -1;
        bool _Failed = false;
        [NonSerialized] GApplication _Application = null; // local
        [NonSerialized] int _Priority = 0; // local
        [NonSerialized] string _WorkingDirectory = ""; // remote
        //Exception _RemoteExecutionException = null;

        //----------------------------------------------------------------------------------------------- 
        // properties
        //----------------------------------------------------------------------------------------------- 

        public int Id
        {
            get { return _Id; }
        }

        internal void SetId (int id)
        {
            _Id = id;
        }

        protected string WorkingDirectory
        {
            get { return _WorkingDirectory; }
        }

        internal void SetWorkingDirectory(string workingDirectory)
        {
            _WorkingDirectory = workingDirectory;
        }

        internal void SetFailed(bool failed)
        {
            _Failed = failed;
        }

        public GApplication Application
        {
            get { return _Application; }
        }

        internal void SetApplication(GApplication application)
        {
            _Application = application;
        }

        public int Priority
        {
            get { return _Priority; }
            set { _Priority = value; }
        }

        public ThreadState State
        {
            get { return _Application.GetThreadState(this); }
        }

        /*
        public Exception RemoteExecutionException
        {
            get { return _RemoteExecutionException; }
            set { _RemoteExecutionException = value; }
        }
        */

        //----------------------------------------------------------------------------------------------- 
        // public methods
        //----------------------------------------------------------------------------------------------- 

        public abstract void Start();
        
        //-----------------------------------------------------------------------------------------------    

        public void Abort()
        {
            _Application.AbortThread(this);
        }
    }
}

