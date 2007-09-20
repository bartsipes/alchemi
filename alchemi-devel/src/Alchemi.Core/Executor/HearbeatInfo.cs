#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   HeartbeatInfo.cs
 * Project      :   Alchemi.Core
 * Created on   :   2003
 * Copyright    :   Copyright © 2006 The University of Melbourne
 *                  This technology has been developed with the support of 
 *                  the Australian Research Council and the University of Melbourne
 *                  research grants as part of the Gridbus Project
 *                  within GRIDS Laboratory at the University of Melbourne, Australia.
 * Author       :   Akshay Luther (akshayl@csse.unimelb.edu.au)
 *                  Rajkumar Buyya (raj@csse.unimelb.edu.au)
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

namespace Alchemi.Core.Executor
{
	/// <summary>
	/// This structure is a container for all the information sent in a heartbeat update.
	/// This primarily consists of dynamic information about an Executor, such as current load conditions etc...
	/// </summary>
    [Serializable]
    public struct HeartbeatInfo
    {
        #region Property - Interval
        private int _interval;
        /// <summary>
        /// Heartbeat interval (in seconds)
        /// </summary>
        public int Interval
        {
            get { return _interval; }
            set { _interval = value; }
        } 
        #endregion


        #region Property - PercentUsedCpuPower
        private int _percentUsedCpuPower;
        /// <summary>
        /// Percent of CPU power currently being used.
        /// </summary>
        public int PercentUsedCpuPower
        {
            get { return _percentUsedCpuPower; }
            set { _percentUsedCpuPower = value; }
        } 
        #endregion


        #region Property - PercentAvailCpuPower
        private int _percentAvailCpuPower;
        /// <summary>
        /// Percent of CPU power that is currently available.
        /// </summary>
        public int PercentAvailCpuPower
        {
            get { return _percentAvailCpuPower; }
            set { _percentAvailCpuPower = value; }
        } 
        #endregion



        #region Constructor
        /// <summary>
        /// Creates an instance of the HeartBeatInfo object with the given interval, used, and available CPU power.
        /// </summary>
        /// <param name="interval">The heartbeat interval (seconds).</param>
        /// <param name="used">The CPU power currently being used.</param>
        /// <param name="avail">The CPU power currently available.</param>
        public HeartbeatInfo(int interval, int used, int avail)
        {
            _interval = interval;
            _percentUsedCpuPower = used;
            _percentAvailCpuPower = avail;
        } 
        #endregion
    }
}
