#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	AppDomainExecutor.cs
* Project		:	Alchemi Core
* Created on	:	2003
* Copyright		:	Copyright © 2006 The University of Melbourne
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
using Alchemi.Core;
using Alchemi.Core.Owner;
using Alchemi.Core.Utility;

namespace Alchemi.Executor
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


