#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   SystemSummary.cs
 * Project      :   Alchemi.Core.Manager.Storage
 * Created on   :   30 August 2005
 * Copyright    :   Copyright © 2006 The University of Melbourne
 *                  This technology has been developed with the support of 
 *                  the Australian Research Council and the University of Melbourne
 *                  research grants as part of the Gridbus Project
 *                  within GRIDS Laboratory at the University of Melbourne, Australia.
 * Author       :   Tibor Biro (tb@tbiro.com)
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

namespace Alchemi.Core.Manager.Storage
{
	/// <summary>
	/// Returned by GetSystemSummary.
	/// Contains various information about the application status.
	/// </summary>
	[Serializable]
	public class SystemSummary
	{

        #region Property - MaxPower
        private string _maxPower;
        /// <summary>
        /// Maximum power.
        /// </summary>
        public string MaxPower
        {
            get { return _maxPower; }
        } 
        #endregion


        #region Property - TotalExecutors
        private int _totalExecutors;
        /// <summary>
        /// The total number of Executors.
        /// </summary>
        public int TotalExecutors
        {
            get { return _totalExecutors; }
        } 
        #endregion


        #region Property - PowerUsage
        private int _powerUsage;
        /// <summary>
        /// The power usage.
        /// </summary>
        public int PowerUsage
        {
            get { return _powerUsage; }
        } 
        #endregion


        #region Property - PowerAvailable
        private Int32 _powerAvailable;
        /// <summary>
        /// The available power.
        /// </summary>
        public int PowerAvailable
        {
            get { return _powerAvailable; }
        } 
        #endregion


        #region Property - PowerTotalUsage
        private string _powerTotalUsage;
        /// <summary>
        /// The total power usage.
        /// </summary>
        public string PowerTotalUsage
        {
            get { return _powerTotalUsage; }
        } 
        #endregion


        #region Property - UnfinishedThreads
        private int _unfinishedThreads;
        /// <summary>
        /// The number of unfinished threads.
        /// </summary>
        public int UnfinishedThreads
        {
            get { return _unfinishedThreads; }
        } 
        #endregion


        #region Property - UnfinishedApps
        private int _unfinishedApps;
        /// <summary>
        /// The number of unfinished applications.
        /// </summary>
        public int UnfinishedApps
        {
            get { return _unfinishedApps; }
        } 
        #endregion



        #region Constructor
        /// <summary>
        /// Create the SystemSummary structure
        /// </summary>
        /// <param name="maxPower"></param>
        /// <param name="totalExecutors"></param>
        /// <param name="powerUsage"></param>
        /// <param name="powerAvailable"></param>
        /// <param name="powerTotalUsage"></param>
        /// <param name="unfinishedApps"></param>
        /// <param name="unfinishedThreads"></param>
        public SystemSummary(
            string maxPower,
            int totalExecutors,
            int powerUsage,
            int powerAvailable,
            string powerTotalUsage,
            int unfinishedApps,
            int unfinishedThreads)
        {
            _maxPower = maxPower;
            _totalExecutors = totalExecutors;
            _powerUsage = powerUsage;
            _powerAvailable = powerAvailable;
            _powerTotalUsage = powerTotalUsage;
            _unfinishedApps = unfinishedApps;
            _unfinishedThreads = unfinishedThreads;
        } 
        #endregion
	}
}
