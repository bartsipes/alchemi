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
using Alchemi.Core.Owner;

namespace Alchemi.Core.Owner
{
	/// <summary>
	/// Represents a "thread" that can be run on a remote grid node
	/// </summary>
    [Serializable]
    public abstract class GThread : MarshalByRefObject
    {
        //----------------------------------------------------------------------------------------------- 
        // member variables
        //----------------------------------------------------------------------------------------------- 
        
        int _Id = -1;
        bool _Failed = false;
        [NonSerialized] GApplication _Application = null; // local
        [NonSerialized] int _Priority = 5; // local
        [NonSerialized] string _WorkingDirectory = ""; // remote

        //----------------------------------------------------------------------------------------------- 
        // properties
        //----------------------------------------------------------------------------------------------- 

		/// <summary>
		/// Gets the id of the grid thread
		/// </summary>
        public int Id
        {
            get { return _Id; }
        }

		/// <summary>
		/// Sets the id of the grid thread
		/// </summary>
		/// <param name="id"></param>
        internal void SetId (int id)
        {
            _Id = id;
        }

		/// <summary>
		/// Gets the working directory of the grid thread
		/// </summary>
        protected string WorkingDirectory
        {
            get { return _WorkingDirectory; }
        }

		/// <summary>
		/// Sets the working directory of the grid thread
		/// </summary>
		/// <param name="workingDirectory">the directory name to set as the working directory</param>
        internal void SetWorkingDirectory(string workingDirectory)
        {
            _WorkingDirectory = workingDirectory;
        }

		/// <summary>
		/// Sets the thread state to failed
		/// </summary>
		/// <param name="failed">value indicating whether to set the thread to failed</param>
        internal void SetFailed(bool failed)
        {
            _Failed = failed;
        }

		/// <summary>
		/// Gets the application to which this grid thread belongs
		/// </summary>
        public GApplication Application
        {
            get { return _Application; }
        }

		/// <summary>
		/// Sets the application to which this grid thread belongs
		/// </summary>
		/// <param name="application"></param>
        internal void SetApplication(GApplication application)
        {
            _Application = application;
        }

		/// <summary>
		/// Gets or sets the grid thread priority
		/// </summary>
        public int Priority
        {
            get { return _Priority; }
            set { _Priority = value; }
        }

		/// <summary>
		/// Gets the state of the grid thread
		/// </summary>
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

		/// <summary>
		/// Starts the execution of the thread on the remote node.
		/// This method is to be implemented by subclasses to include code 
		/// which is actually executed on the executor.
		/// </summary>
        public abstract void Start();
        
        //-----------------------------------------------------------------------------------------------    

		/// <summary>
		/// Aborts this grid thread
		/// </summary>
        public void Abort()
        {
            _Application.AbortThread(this);
        }
    }
}

