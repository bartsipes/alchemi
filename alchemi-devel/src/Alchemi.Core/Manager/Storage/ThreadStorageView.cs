//
// Alchemi.Core.Manager.Storage.ThreadStorageView.cs
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
	/// Summary description for ThreadStorageView.
	/// </summary>
	public class ThreadStorageView
	{
		#region "Private variables"
		
		private Int32 m_internalThreadId; // the database identity
		private String m_applicationId;
		private String m_executorId;
		private Int32 m_threadId;
		private Int32 m_state;
		private DateTime m_timeStarted;
		private DateTime m_timeFinished;
		private Int32 m_priority;
		private bool m_failed;

		#endregion

		#region "Properties"
		public Int32 InternalThreadId
		{
			get
			{
				return m_internalThreadId;
			}
			set
			{
				m_internalThreadId = value;
			}
		}

		public String ApplicationId
		{
			get
			{
				return m_applicationId;
			}
			set
			{
				m_applicationId = value;
			}
		}

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

		public Int32 ThreadId
		{
			get
			{
				return m_threadId;
			}
			set
			{
				m_threadId = value;
			}
		}

		public Int32 State
		{
			get
			{
				return m_state;
			}
		}

		public DateTime TimeStarted
		{
			get
			{
				return m_timeStarted;
			}
		}

		public DateTime TimeFinished
		{
			get
			{
				return m_timeFinished;
			}
		}

		public Int32 Priority
		{
			get
			{
				return m_priority;
			}
		}

		public bool Failed
		{
			get
			{
				return m_failed;
			}
		}

		#endregion

		public ThreadStorageView(
				String applicationId,
				String executorId,
				Int32 threadId,
				Int32 state,
				DateTime timeStarted,
				DateTime timeFinished,
				Int32 priority,
				bool failed
			) : this (
				-1,
				applicationId,
				executorId,
				threadId,
				state,
				timeStarted,
				timeFinished,
				priority,
				failed
			)
		{
		}

		public ThreadStorageView(
				Int32 internalThreadId,
				String applicationId,
				String executorId,
				Int32 threadId,
				Int32 state,
				DateTime timeStarted,
				DateTime timeFinished,
				Int32 priority,
				bool failed
			)
		{
			m_internalThreadId = internalThreadId;
			m_applicationId = applicationId;
			m_executorId = executorId;
			m_threadId = threadId;
			m_state = state;
			m_timeStarted = timeStarted;
			m_timeFinished = timeFinished;
			m_priority = priority;
			m_failed = failed;
		}
	}
}
