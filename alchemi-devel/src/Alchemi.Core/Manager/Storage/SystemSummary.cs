//
// Alchemi.Core.Manager.Storage.SystemSummary.cs
//
// Author:
//   Tibor Biro (tb@tbiro.com)
//
// Copyright (C) Tibor Biro, 2005
//

//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

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
