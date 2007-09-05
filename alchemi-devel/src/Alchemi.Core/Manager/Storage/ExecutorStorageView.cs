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
        private string m_architecture;
        /// <summary>
        /// Executor architecture.
        /// </summary>
        public string Architecture
        {
            get { return m_architecture; }
            set { m_architecture = value; }
        } 
        #endregion


        #region Property - OS
        private string m_os;
        /// <summary>
        /// Executor Operating System.
        /// </summary>
        public string OS
        {
            get { return m_os; }
            set { m_os = value; }
        } 
        #endregion


        #region Property - NumberOfCpu
        private int m_numberOfCpu;
        /// <summary>
        /// The number of CPUs on this Executor.
        /// </summary>
        public int NumberOfCpu
        {
            get { return m_numberOfCpu; }
            set { m_numberOfCpu = value; }
        } 
        #endregion


        #region Property - MaxDisk
        private float m_maxDisk;
        /// <summary>
        /// The maximum disk space available on this Executor.
        /// </summary>
        public float MaxDisk
        {
            get { return m_maxDisk; }
            set { m_maxDisk = value; }
        } 
        #endregion


        #region Property - MaxMemory
        private float m_maxMemory;
        /// <summary>
        /// The maximum memory available on this Executor.
        /// </summary>
        public float MaxMemory
        {
            get { return m_maxMemory; }
            set { m_maxMemory = value; }
        } 
        #endregion


        #region Property - ExecutorId
        private string m_executorId;
        /// <summary>
        /// Executor Id.
        /// </summary>
        public string ExecutorId
        {
            get { return m_executorId; }
            set { m_executorId = value; }
        } 
        #endregion


        #region Property - Dedicated
        private bool m_dedicated;
        /// <summary>
        /// Gets or sets a name indicating whether the Executor is dedicated or not.
        /// </summary>
        public bool Dedicated
        {
            get { return m_dedicated; }
            set { m_dedicated = value; }
        } 
        #endregion


        #region Property - Connected
        private bool m_connected;
        /// <summary>
        /// Gets or sets a name indicating whether the Executor is connected or not.
        /// </summary>
        public bool Connected
        {
            get { return m_connected; }
            set { m_connected = value; }
        } 
        #endregion


        #region Property - PingTime
        private DateTime m_pingTime;
        /// <summary>
        /// The last time this Executor was pinged
        /// </summary>
        public DateTime PingTime
        {
            get { return m_pingTime; }
            set { m_pingTime = value; }
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
                return (m_pingTime != c_noTimeSet);
            }
        } 
        #endregion


        #region Property - HostName
        private string m_hostname;
        /// <summary>
        /// The Executor's host name.
        /// </summary>
        public string HostName
        {
            get { return m_hostname; }
            set { m_hostname = value; }
        } 
        #endregion


        #region Property - Port
        private int m_port;
        /// <summary>
        /// The Executor's port.
        /// </summary>
        public int Port
        {
            get { return m_port; }
            set { m_port = value; }
        } 
        #endregion


        #region Property - Username
        private string m_username;
        /// <summary>
        /// The Executor's username.
        /// </summary>
        public string Username
        {
            get { return m_username; }
        } 
        #endregion


        #region Property - MaxCpu
        private int m_maxCpu;
        /// <summary>
        /// The maximum CPU of the Executor.
        /// </summary>
        public int MaxCpu
        {
            get { return m_maxCpu; }
        } 
        #endregion


        #region Property - CpuUsage
        private int m_cpuUsage;
        /// <summary>
        /// The CPU usage for this Executor.
        /// </summary>
        public int CpuUsage
        {
            get { return m_cpuUsage; }
            set { m_cpuUsage = value; }
        } 
        #endregion


        #region Property - AvailableCpu
        private int m_availableCpu;
        /// <summary>
        /// The available CPU power for this Executor.
        /// </summary>
        public int AvailableCpu
        {
            get { return m_availableCpu; }
            set { m_availableCpu = value; }
        } 
        #endregion


        #region Property - TotalCpuUsage
        private float m_totalCpuUsage;
        /// <summary>
        /// The total CPU usage for this Executor.
        /// </summary>
        public float TotalCpuUsage
        {
            get { return m_totalCpuUsage; }
            set { m_totalCpuUsage = value; }
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
            m_executorId = executorId;
            m_dedicated = dedicated;
            m_connected = connected;
            m_pingTime = pingTime;
            m_hostname = hostname;
            m_port = port;
            m_username = username;
            m_maxCpu = maxCpu;
            m_cpuUsage = cpuUsage;
            m_availableCpu = availableCpu;
            m_totalCpuUsage = totalCpuUsage;
            MaxMemory = maxMemory;
            MaxDisk = maxDisk;
            NumberOfCpu = numberOfCpu;
            OS = os;
            Architecture = architecture;
        } 
        #endregion
	
	}
}
