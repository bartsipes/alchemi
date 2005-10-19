//
// Alchemi.Core.Manager.Storage.Executor.cs
//
// Author:
//   Tibor Biro (tb@tbiro.com)
//
// Copyright (C) 2005 Tibor Biro (tb@tbiro.com)
//
// This program is free software; you can redistribute it and/or modify 
// it under the terms of the GNU General Public License as published by 
// the Free Software Foundation; either version 2 of the License, or 
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of 
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the 
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License 
// along with this program; if not, write to the Free Software 
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

using System;

namespace Alchemi.Core.Manager.Storage
{
	/// <summary>
	/// Storage view of an executor object. 
	/// Used to pass group related data to and from the storage layer 
	/// </summary>
	public class ExecutorStorageView
	{
		#region "Private variables"
		
		private String m_executorId;
		private bool m_dedicated;
		private bool m_connected;
		private DateTime m_pingTime;
		private String m_hostname;
		private Int32 m_port;
		private String m_username;
		private Int32 m_maxCpu;
		private Int32 m_cpuUsage;
		private Int32 m_availableCpu;
		private float m_totalCpuUsage;

		#endregion

		#region "Properties"
		public String ExecutorId
		{
			get
			{
				return m_executorId;
			}
			set
			{
				m_executorId = value;
			}
		}

		public bool Dedicated
		{
			get
			{
				return m_dedicated;
			}
		}

		public bool Connected
		{
			get
			{
				return m_connected;
			}
		}

		public DateTime PingTime
		{
			get
			{
				return m_pingTime;
			}
		}

		public String HostName
		{
			get
			{
				return m_hostname;
			}
		}

		public Int32 Port
		{
			get
			{
				return m_port;
			}
		}

		public String Username
		{
			get
			{
				return m_username;
			}
		}

		public Int32 MaxCpu
		{
			get
			{
				return m_maxCpu;
			}
		}

		public Int32 CpuUsage
		{
			get
			{
				return m_cpuUsage;
			}
		}

		public Int32 AvailableCpu
		{
			get
			{
				return m_availableCpu;
			}
		}

		public float TotalCpuUsage
		{
			get
			{
				return m_totalCpuUsage;
			}
		}

		#endregion

		public ExecutorStorageView(
				bool dedicated,
				bool connected,
				DateTime pingTime,
				String hostname,
				Int32 port,
				String username,
				Int32 maxCpu,
				Int32 cpuUsage,
				Int32 availableCpu,
				float totalCpuUsage
			) : this(
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

		public ExecutorStorageView(
			String executorId,
			bool dedicated,
			bool connected,
			DateTime pingTime,
			String hostname,
			Int32 port,
			String username,
			Int32 maxCpu,
			Int32 cpuUsage,
			Int32 availableCpu,
			float totalCpuUsage
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
		}

	
	}
}
