#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	DedicateSchedule.cs
* Project		:	Alchemi Core
* Created on	:	2003
* Copyright		:	Copyright © 2006 The University of Melbourne
*					This technology has been developed with the support of 
*					the Australian Research Council and the University of Melbourne
*					research grants as part of the Gridbus Project
*					within GRIDS Laboratory at the University of Melbourne, Australia.
* Author         :  Akshay Luther (akshayl@csse.unimelb.edu.au) and Rajkumar Buyya (raj@csse.unimelb.edu.au)
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

namespace Alchemi.Manager
{
	/// <summary>
	/// This class represents a "dedicatedSchedule" which is just a container for the ThreadIdentifier and ExecutorId which refers to
	/// the executor that will run this thread.
	/// </summary>
    public class DedicatedSchedule
    {
        #region Constructor
        /// <summary>
        /// Constructor with a threadIdentifier and executorId
        /// </summary>
        /// <param name="ti">ThreadIdentifier</param>
        /// <param name="executorId">Executor Id</param>
        public DedicatedSchedule(ThreadIdentifier ti, string executorId)
        {
            _ti = ti;
            _ExecutorId = executorId;
        }
        #endregion



        #region Property - TI
        private ThreadIdentifier _ti;
        /// <summary>
        /// ThreadIdentifier representing the scheduled application and thread.
        /// </summary>      
        public ThreadIdentifier TI
        {
            get { return _ti; }
        } 
        #endregion


        #region Property - ExecutorId
        private string _ExecutorId;
        /// <summary>
        /// The id of the Executor scheduled to execute this thread
        /// </summary>
        public string ExecutorId
        {
            get { return _ExecutorId; }
        } 
        #endregion

    }
}