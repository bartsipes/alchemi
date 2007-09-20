#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   ExecutorInfo.cs
 * Project      :   Alchemi.Core
 * Created on   :   2003
 * Copyright    :   Copyright © 2006 The University of Melbourne
 *                  This technology has been developed with the support of 
 *                  the Australian Research Council and the University of Melbourne
 *                  research grants as part of the Gridbus Project
 *                  within GRIDS Laboratory at the University of Melbourne, Australia.
 * Author       :   Akshay Luther (akshayl@csse.unimelb.edu.au)
 *                  Krishna Nadiminti (kna@csse.unimelb.edu.au)
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
    /// Represents the static attributes of an Executor.
    /// </summary>
    [Serializable]
    public struct ExecutorInfo
    {

        #region Property - Hostname
        private string _hostname;
        /// <summary>
        /// Gets or sets the Hostname of the Executor.
        /// </summary>
        public string Hostname
        {
            get { return _hostname; }
            set { _hostname = value; }
        } 
        #endregion


        #region Property - MaxCpuPower
        private int _maxCpuPower;
        /// <summary>
        /// Gets or sets the maximum CPU power in the Executor hardware. (in Mhz)
        /// </summary>
        public int MaxCpuPower
        {
            get { return _maxCpuPower; }
            set { _maxCpuPower = value; }
        } 
        #endregion


        #region Property - MaxMemory
        private float _maxMemory;
        /// <summary>
        /// Gets or sets the maximum memory (RAM) in the Executor hardware. (in MB)
        /// </summary>
        public float MaxMemory
        {
            get { return _maxMemory; }
            set { _maxMemory = value; }
        } 
        #endregion


        #region Property - MaxDiskSpace
        private float _maxDiskSpace;
        /// <summary>
        /// Gets or sets the maximum disk space in the Executor hardware. (in MB)
        /// </summary>
        public float MaxDiskSpace
        {
            get { return _maxDiskSpace; }
            set { _maxDiskSpace = value; }
        } 
        #endregion


        #region Property - NumberOfCpus
        private int _numberOfCpus;
        /// <summary>
        /// Gets or sets the total number of CPUs in the Executor hardware.
        /// </summary>
        public int NumberOfCpus
        {
            get { return _numberOfCpus; }
            set { _numberOfCpus = value; }
        } 
        #endregion


        #region Property - OS
        private string _os;
        /// <summary>
        /// Gets or sets the name of operating system running on the Executor
        /// </summary>
        public string OS
        {
            get { return _os; }
            set { _os = value; }
        } 
        #endregion


        #region Property - Architecture
        private string _architecture;
        /// <summary>
        /// Gets or sets the architecture of the processor/machine of the Executor (eg: x86, RISC etc)
        /// </summary>
        public string Architecture
        {
            get { return _architecture; }
            set { _architecture = value; }
        } 
        #endregion


        #region Property - CpuLimit
        private int _cpuLimit;
        /// <summary>
        /// The maximum amount of CPU that the Executor can provide (in GHz*Hr).
        /// This attribute is set by the owner/administrator of the Executor.
        /// </summary>
        public int CpuLimit
        {
            get { return _cpuLimit; }
            set { _cpuLimit = value; }
        } 
        #endregion


        #region Property - MemLimit
        private float _memLimit;
        /// <summary>
        /// The maximum amount of memory (RAM) that the Executor can provide (in MB).
        /// This attribute is set by the owner/administrator of the Executor.
        /// </summary>
        public float MemLimit
        {
            get { return _memLimit; }
            set { _memLimit = value; }
        } 
        #endregion


        #region Property - DiskLimit
        private float _diskLimit;
        /// <summary>
        /// The maximum amount of disk space that the Executor can provide (in MB).
        /// This attribute is set by the owner/administrator of the Executor.
        /// </summary>
        public float DiskLimit
        {
            get { return _diskLimit; }
            set { _diskLimit = value; }
        } 
        #endregion


        //Qos stuff

        #region Property - CostPerCpuSec
        private float _costPerCpuSec;
        /// <summary>
        /// The cost per CPU-seconds.
        /// TODO:
        /// </summary>
        public float CostPerCpuSec
        {
            get { return _costPerCpuSec; }
            set { _costPerCpuSec = value; }
        } 
        #endregion


        #region Property - CostPerThread
        private float _costPerThread;
        /// <summary>
        /// The cost per thread.
        /// TODO:
        /// </summary>
        public float CostPerThread
        {
            get { return _costPerThread; }
            set { _costPerThread = value; }
        } 
        #endregion


        #region Property - CostPerDiskMB
        private float _costPerDiskMB;
        /// <summary>
        /// The cost per MB of disk storage space.
        /// TODO:
        /// </summary>
        public float CostPerDiskMB
        {
            get { return _costPerDiskMB; }
            set { _costPerDiskMB = value; }
        } 
        #endregion

    }
}


