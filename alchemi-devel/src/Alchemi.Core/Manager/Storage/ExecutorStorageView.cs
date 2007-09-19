#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   ExecutorStorageView.cs
 * Project      :   Alchemi.Core.Manager.Storage
 * Created on   :   23 September 2005
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
	/// Storage view of an executor object. 
	/// Used to pass executor related data to and from the storage layer.
	/// </summary>
	[Serializable]
	public class ExecutorStorageView
	{
		private static DateTime c_noTimeSet = DateTime.MinValue;


        #region Property - Architecture
        private string _architecture;
        /// <summary>
        /// Executor architecture.
        /// </summary>
        public string Architecture
        {
            get { return _architecture; }
            set { _architecture = value; }
        } 
        #endregion


        #region Property - OS
        private string _os;
        /// <summary>
        /// Executor Operating System.
        /// </summary>
        public string OS
        {
            get { return _os; }
            set { _os = value; }
        } 
        #endregion


        #region Property - NumberOfCpu
        private int _numberOfCpu;
        /// <summary>
        /// The number of CPUs on this Executor.
        /// </summary>
        public int NumberOfCpu
        {
            get { return _numberOfCpu; }
            set { _numberOfCpu = value; }
        } 
        #endregion


        #region Property - MaxDisk
        private float _maxDisk;
        /// <summary>
        /// The maximum disk space available on this Executor.
        /// </summary>
        public float MaxDisk
        {
            get { return _maxDisk; }
            set { _maxDisk = value; }
        } 
        #endregion


        #region Property - MaxMemory
        private float _maxMemory;
        /// <summary>
        /// The maximum memory available on this Executor.
        /// </summary>
        public float MaxMemory
        {
            get { return _maxMemory; }
            set { _maxMemory = value; }
        } 
        #endregion


        #region Property - ExecutorId
        private string _executorId;
        /// <summary>
        /// Executor Id.
        /// </summary>
        public string ExecutorId
        {
            get { return _executorId; }
            set { _executorId = value; }
        } 
        #endregion


        #region Property - Dedicated
        private bool _dedicated;
        /// <summary>
        /// Gets or sets a name indicating whether the Executor is dedicated or not.
        /// </summary>
        public bool Dedicated
        {
            get { return _dedicated; }
            set { _dedicated = value; }
        } 
        #endregion


        #region Property - Connected
        private bool _connected;
        /// <summary>
        /// Gets or sets a name indicating whether the Executor is connected or not.
        /// </summary>
        public bool Connected
        {
            get { return _connected; }
            set { _connected = value; }
        } 
        #endregion


        #region Property - PingTime
        private DateTime _pingTime;
        /// <summary>
        /// The last time this Executor was pinged
        /// </summary>
        public DateTime PingTime
        {
            get { return _pingTime; }
            set { _pingTime = value; }
        } 
        #endregion


        #region Property - PingTimeSet
        /// <summary>
        /// Gets a name indicating whether the PingTime property is set or not.
        /// </summary>
        public bool PingTimeSet
        {
            get
            {
                return (_pingTime != c_noTimeSet);
            }
        } 
        #endregion


        #region Property - HostName
        private string _hostname;
        /// <summary>
        /// The Executor's host name.
        /// </summary>
        public string HostName
        {
            get { return _hostname; }
            set { _hostname = value; }
        } 
        #endregion


        #region Property - Port
        private int _port;
        /// <summary>
        /// The Executor's port.
        /// </summary>
        public int Port
        {
            get { return _port; }
            set { _port = value; }
        } 
        #endregion


        #region Property - Username
        private string _username;
        /// <summary>
        /// The Executor's username.
        /// </summary>
        public string Username
        {
            get { return _username; }
        } 
        #endregion


        #region Property - MaxCpu
        private int _maxCpu;
        /// <summary>
        /// The maximum CPU of the Executor.
        /// </summary>
        public int MaxCpu
        {
            get { return _maxCpu; }
        } 
        #endregion


        #region Property - CpuUsage
        private int _cpuUsage;
        /// <summary>
        /// The CPU usage for this Executor.
        /// </summary>
        public int CpuUsage
        {
            get { return _cpuUsage; }
            set { _cpuUsage = value; }
        } 
        #endregion


        #region Property - AvailableCpu
        private int _availableCpu;
        /// <summary>
        /// The available CPU power for this Executor.
        /// </summary>
        public int AvailableCpu
        {
            get { return _availableCpu; }
            set { _availableCpu = value; }
        } 
        #endregion


        #region Property - TotalCpuUsage
        private float _totalCpuUsage;
        /// <summary>
        /// The total CPU usage for this Executor.
        /// </summary>
        public float TotalCpuUsage
        {
            get { return _totalCpuUsage; }
            set { _totalCpuUsage = value; }
        } 
        #endregion


        #region Constructors
        /// <summary>
        /// ExecutorStorageView constructor.
        /// </summary>
        /// <param name="dedicated"></param>
        /// <param name="connected"></param>
        /// <param name="pingTime"></param>
        /// <param name="hostname"></param>
        /// <param name="port"></param>
        /// <param name="username"></param>
        /// <param name="maxCpu"></param>
        /// <param name="cpuUsage"></param>
        /// <param name="availableCpu"></param>
        /// <param name="totalCpuUsage"></param>
        public ExecutorStorageView(
                bool dedicated,
                bool connected,
                DateTime pingTime,
                string hostname,
                int port,
                string username,
                int maxCpu,
                int cpuUsage,
                int availableCpu,
                float totalCpuUsage
            )
            : this(
                null,
                dedicated,
                connected,
                pingTime,
                hostname,
                port,
                username,
                maxCpu,
                cpuUsage,
                availableCpu,
                totalCpuUsage
            )
        {
        }

        /// <summary>
        /// ExecutorStorageView constructor.
        /// </summary>
        /// <param name="executorId"></param>
        /// <param name="dedicated"></param>
        /// <param name="connected"></param>
        /// <param name="hostname"></param>
        /// <param name="port"></param>
        /// <param name="username"></param>
        /// <param name="maxCpu"></param>
        /// <param name="cpuUsage"></param>
        /// <param name="availableCpu"></param>
        /// <param name="totalCpuUsage"></param>
        public ExecutorStorageView(
            string executorId,
            bool dedicated,
            bool connected,
            string hostname,
            int port,
            string username,
            int maxCpu,
            int cpuUsage,
            int availableCpu,
            float totalCpuUsage
            )
            : this(
            executorId,
            dedicated,
            connected,
            ExecutorStorageView.c_noTimeSet,
            hostname,
            port,
            username,
            maxCpu,
            cpuUsage,
            availableCpu,
            totalCpuUsage
            )
        {
        }

        /// <summary>
        /// ExecutorStorageView constructor.
        /// </summary>
        /// <param name="executorId"></param>
        /// <param name="dedicated"></param>
        /// <param name="connected"></param>
        /// <param name="pingTime"></param>
        /// <param name="hostname"></param>
        /// <param name="port"></param>
        /// <param name="username"></param>
        /// <param name="maxCpu"></param>
        /// <param name="cpuUsage"></param>
        /// <param name="availableCpu"></param>
        /// <param name="totalCpuUsage"></param>
        public ExecutorStorageView(
            string executorId,
            bool dedicated,
            bool connected,
            DateTime pingTime,
            string hostname,
            int port,
            string username,
            int maxCpu,
            int cpuUsage,
            int availableCpu,
            float totalCpuUsage
            )
            : this(
            executorId,
            dedicated,
            connected,
            pingTime,
            hostname,
            port,
            username,
            maxCpu,
            cpuUsage,
            availableCpu,
            totalCpuUsage,
            0,
            0,
            0,
            "",
            ""
            )
        {
        }

        /// <summary>
        /// ExecutorStorageView constructor.
        /// </summary>
        /// <param name="dedicated"></param>
        /// <param name="connected"></param>
        /// <param name="hostname"></param>
        /// <param name="username"></param>
        /// <param name="maxCpu"></param>
        /// <param name="maxMemory"></param>
        /// <param name="maxDisk"></param>
        /// <param name="numberOfCpu"></param>
        /// <param name="os"></param>
        /// <param name="architecture"></param>
        public ExecutorStorageView(
            bool dedicated,
            bool connected,
            string hostname,
            string username,
            int maxCpu,
            float maxMemory,
            float maxDisk,
            int numberOfCpu,
            string os,
            string architecture
            )
            : this(
            null,
            dedicated,
            connected,
            ExecutorStorageView.c_noTimeSet,
            hostname,
            0,
            username,
            maxCpu,
            0,
            0,
            0,
            maxMemory,
            maxDisk,
            numberOfCpu,
            os,
            architecture
            )
        {
        }

        /// <summary>
        /// ExecutorStorageView constructor.
        /// </summary>
        /// <param name="executorId"></param>
        /// <param name="dedicated"></param>
        /// <param name="connected"></param>
        /// <param name="hostname"></param>
        /// <param name="username"></param>
        /// <param name="maxCpu"></param>
        /// <param name="maxMemory"></param>
        /// <param name="maxDisk"></param>
        /// <param name="numberOfCpu"></param>
        /// <param name="os"></param>
        /// <param name="architecture"></param>
        public ExecutorStorageView(
            string executorId,
            bool dedicated,
            bool connected,
            string hostname,
            string username,
            int maxCpu,
            float maxMemory,
            float maxDisk,
            int numberOfCpu,
            string os,
            string architecture
            )
            : this(
            executorId,
            dedicated,
            connected,
            ExecutorStorageView.c_noTimeSet,
            hostname,
            0,
            username,
            maxCpu,
            0,
            0,
            0,
            maxMemory,
            maxDisk,
            numberOfCpu,
            os,
            architecture
            )
        {
        }

        /// <summary>
        /// ExecutorStorageView constructor.
        /// </summary>
        /// <param name="executorId"></param>
        /// <param name="dedicated"></param>
        /// <param name="connected"></param>
        /// <param name="pingTime"></param>
        /// <param name="hostname"></param>
        /// <param name="port"></param>
        /// <param name="username"></param>
        /// <param name="maxCpu"></param>
        /// <param name="cpuUsage"></param>
        /// <param name="availableCpu"></param>
        /// <param name="totalCpuUsage"></param>
        /// <param name="maxMemory"></param>
        /// <param name="maxDisk"></param>
        /// <param name="numberOfCpu"></param>
        /// <param name="os"></param>
        /// <param name="architecture"></param>
        public ExecutorStorageView(
            string executorId,
            bool dedicated,
            bool connected,
            DateTime pingTime,
            string hostname,
            int port,
            string username,
            int maxCpu,
            int cpuUsage,
            int availableCpu,
            float totalCpuUsage,
            float maxMemory,
            float maxDisk,
            int numberOfCpu,
            string os,
            string architecture
            )
        {
            _executorId = executorId;
            _dedicated = dedicated;
            _connected = connected;
            _pingTime = pingTime;
            _hostname = hostname;
            _port = port;
            _username = username;
            _maxCpu = maxCpu;
            _cpuUsage = cpuUsage;
            _availableCpu = availableCpu;
            _totalCpuUsage = totalCpuUsage;
            MaxMemory = maxMemory;
            MaxDisk = maxDisk;
            NumberOfCpu = numberOfCpu;
            OS = os;
            Architecture = architecture;
        } 
        #endregion
	
	}
}
