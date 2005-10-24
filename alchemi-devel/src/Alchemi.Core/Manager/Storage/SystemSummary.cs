#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
* Title         :  SystemSummary.cs
* Project       :  Alchemi.Core.Manager.Storage
* Created on    :  30 August 2005
* Copyright     :  Copyright © 2005 The University of Melbourne
*                    This technology has been developed with the support of
*                    the Australian Research Council and the University of Melbourne
*                    research grants as part of the Gridbus Project
*                    within GRIDS Laboratory at the University of Melbourne, Australia.
* Author        :  Tibor Biro (tb@tbiro.com)
* License       :  GPL
*                    This program is free software; you can redistribute it and/or
*                    modify it under the terms of the GNU General Public
*                    License as published by the Free Software Foundation;
*                    See the GNU General Public License
*                    (http://www.gnu.org/copyleft/gpl.html) for more 
details.
*
*/
#endregion

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
