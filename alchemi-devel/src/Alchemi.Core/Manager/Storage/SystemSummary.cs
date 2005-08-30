//
// Alchemi.Core.Manager.Storage.SystemSummary.cs
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
	/// Returned by GetSystemSummary.
	/// Contains various information about the application status
	/// </summary>
	public class SystemSummary
	{
		#region "Private variables"
		
		private String m_maxPower;
		private Int32 m_totalExecutors;
		private Int32 m_powerUsage;
		private Int32 m_powerAvailable;
		private String m_powerTotalUsage;
		private Int32 m_unfinishedThreads;

		#endregion

		#region "Properties"
		public String MaxPower
		{
			get
			{
				return m_maxPower;
			}
		}

		public Int32 TotalExecutors
		{
			get
			{
				return m_totalExecutors;
			}
		}

		public Int32 PowerUsage
		{
			get
			{
				return m_powerUsage;
			}
		}

		public Int32 PowerAvailable
		{
			get
			{
				return m_powerAvailable;
			}
		}

		public String PowerTotalUsage
		{
			get
			{
				return m_powerTotalUsage;
			}
		}

		public Int32 UnfinishedThreads
		{
			get
			{
				return m_unfinishedThreads;
			}
		}

		#endregion

		/// <summary>
		/// Create the SystemSummary structure
		/// </summary>
		/// <param name="maxPower"></param>
		/// <param name="totalExecutors"></param>
		/// <param name="powerUsage"></param>
		/// <param name="powerAvailable"></param>
		/// <param name="m_powerTotalUsage"></param>
		/// <param name="m_unfinishedThreads"></param>
		public SystemSummary(
			String maxPower, 
			Int32 totalExecutors, 
			Int32 powerUsage,
			Int32 powerAvailable,
			String powerTotalUsage,
			Int32 unfinishedThreads)
		{
			m_maxPower = maxPower;
			m_totalExecutors = totalExecutors;
			m_powerUsage = powerUsage;
			m_powerAvailable = powerAvailable;
			m_powerTotalUsage = powerTotalUsage;
			m_unfinishedThreads = unfinishedThreads;
		}
	}
}
