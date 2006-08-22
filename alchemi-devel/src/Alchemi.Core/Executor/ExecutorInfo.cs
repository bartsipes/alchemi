#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	ExecutorInfo.cs
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

namespace Alchemi.Core.Executor
{
	/// <summary>
	/// Represents the static attributes of an executor.
	/// 
	/// </summary>
    [Serializable]
    public struct ExecutorInfo
    {
        private string _Hostname;
        private int _MaxCpuPower;
        private float _MaxMemory; //in MB
        private float _MaxDiskSpace; // in MB
        private int _Number_of_CPUs;
        private string _OS;
        private string _Architecture;
        private int _CPULimit; //in Ghz * hr
        private float _MemLimit; //in MB
        private float _DiskLimit; //in MB
        private float _CostPerCPUSec;
        private float _CostPerThread;
        private float _CostPerDiskMB;

        /// <summary>
		/// Gets or sets the Hostname of the Executor.
		/// </summary>
        public string Hostname
        {
            get { return _Hostname; }
            set { _Hostname = value; }
        }
		/// <summary>
		/// Gets or sets the maximum CPU power in the Executor hardware. (in Mhz)?Ghz
		/// </summary>
        public int MaxCpuPower
        {
            get { return _MaxCpuPower; }
            set { _MaxCpuPower = value; }
        }
		/// <summary>
		/// Gets or sets the maximum memory (RAM) in the Executor hardware. (in MB)
		/// </summary>
        public float MaxMemory
        {
            get { return _MaxMemory; }
            set { _MaxMemory = value; }
        }
		/// <summary>
		/// Gets or sets the maximum disk space in the Executor hardware. (in MB)
		/// </summary>
        public float MaxDiskSpace
        {
            get { return _MaxDiskSpace; }
            set { _MaxDiskSpace = value; }
        }
		/// <summary>
		/// Gets or sets the total number of CPUs in the Executor hardware.
		/// </summary>
        public int Number_of_CPUs
        {
            get { return _Number_of_CPUs; }
            set { _Number_of_CPUs = value; }
        }
		/// <summary>
		/// Gets or sets the name of Operating system running on the Executor
		/// </summary>
        public string OS
        {
            get { return _OS; }
            set { _OS = value; }
        }
		/// <summary>
		/// Gets or sets the architecture of the processor/machine of the Executor (eg: x86, RISC etc)
		/// </summary>
        public string Architecture
        {
            get { return _Architecture; }
            set { _Architecture = value; }
        }

		//these attributes are the limits set by the owner/administrator of the Executor node
		/// <summary>
		/// 
		/// </summary>
        public int CPULimit
        {
            get { return _CPULimit; }
            set { _CPULimit = value; }
        }
		/// <summary>
		/// 
		/// </summary>
        public float MemLimit
        {
            get { return _MemLimit; }
            set { _MemLimit = value; }
        }
		/// <summary>
		/// 
		/// </summary>
        public float DiskLimit
        {
            get { return _DiskLimit; }
            set { _DiskLimit = value; }
        }

		//Qos stuff
		/// <summary>
		/// 
		/// </summary>
        public float CostPerCPUSec
        {
            get { return _CostPerCPUSec; }
            set { _CostPerCPUSec = value; }
        }
		/// <summary>
		/// 
		/// </summary>
        public float CostPerThread
        {
            get { return _CostPerThread; }
            set { _CostPerThread = value; }
        }
		/// <summary>
		/// 
		/// </summary>
        public float CostPerDiskMB
        {
            get { return _CostPerDiskMB; }
            set { _CostPerDiskMB = value; }
        }

    }
}


