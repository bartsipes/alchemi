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
using Alchemi.Core.Utility;

namespace Alchemi.Core.Executor
{
	/// <summary>
	/// This class executes the thread on the executor in a seperate application domain.
	/// </summary>
    public class AppDomainExecutor : MarshalByRefObject
    {
		// Create a logger for use in this class
		private static readonly Logger logger = new Logger();

		/// <summary>
		/// Creates a instance of the AppDomainExecutor class.
		/// </summary>
        public AppDomainExecutor() {}

        //----------------------------------------------------------------------------------------------- 

		/// <summary>
		/// Executes a grid thread in its own Application Domain on the executor node.
		/// Returns the gridThread as a serialized byte array after execution. This byte array includes the results of execution of the GThread.
		/// </summary>
		/// <param name="thread">A byte array representing a serialized gridThread to be executed.</param>
		/// <returns>A byte array representing a serialized gridThread, after the execution is complete.</returns>
        public byte[] ExecuteThread(byte[] thread)
        {
			
			GThread gridThread = (GThread) Utils.DeserializeFromByteArray(thread);
			logger.Debug("Executor running GThread: "+gridThread.Id);
			logger.Debug("Working dir="+AppDomain.CurrentDomain.SetupInformation.PrivateBinPath);

            gridThread.SetWorkingDirectory(AppDomain.CurrentDomain.SetupInformation.PrivateBinPath);
            
            gridThread.Start();

			logger.Debug("GThread "+gridThread.Id+" done. Serializing it to send back to the manager...");

            return Utils.SerializeToByteArray(gridThread);
        }
        
        //----------------------------------------------------------------------------------------------- 
        
		/// <summary>
		/// Overrides the InitializeLifetimeService and return null, to provide for "Infinite" lifetime
		/// </summary>
		/// <returns>null</returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}


