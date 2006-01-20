#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
* Title         :  ThreadStorageView.cs
* Project       :  Alchemi.Core.Manager.Storage
* Created on    :  19 October 2005
* Copyright     :  Copyright © 2006 The University of Melbourne
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

using Alchemi.Core.Owner;

namespace Alchemi.Core.Manager.Storage
{
	/// <summary>
	/// Summary description for ThreadStorageView.
	/// </summary>
	[Serializable]
	public class ThreadStorageView
	{
		private static DateTime c_noTimeSet = DateTime.MinValue;

		#region "Private variables"
		
		private Int32 m_internalThreadId; // the database identity
		private String m_applicationId;
		private String m_executorId;
		private Int32 m_threadId;
		private ThreadState m_state;
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

		public ThreadState State
		{
			get
			{
				return m_state;
			}
			set
			{
				m_state = value;
			}
		}

		public DateTime TimeStarted
		{
			get
			{
				return m_timeStarted;
			}
			set
			{
				m_timeStarted = value;
			}
		}

		public DateTime TimeFinished
		{
			get
			{
				return m_timeFinished;
			}
			set
			{
				m_timeFinished = value;
			}
		}

		public bool TimeStartedSet
		{
			get
			{
				return m_timeStarted != c_noTimeSet;
			}
		}

		public bool TimeFinishedSet
		{
			get
			{
				return m_timeFinished != c_noTimeSet;
			}
		}

		public Int32 Priority
		{
			get
			{
				return m_priority;
			}
			set
			{
				m_priority = value;
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

		public void ResetTimeStarted()
		{
			m_timeStarted = c_noTimeSet;
		}

		public void ResetTimeFinished()
		{
			m_timeFinished = c_noTimeSet;
		}

		public ThreadStorageView(
			String applicationId,
			Int32 threadId
			) : this (
			applicationId,
			threadId,
			ThreadState.Unknown
			)
		{
		}

		public ThreadStorageView(
			String applicationId,
			Int32 threadId,
			ThreadState state
			) : this (
			-1,
			applicationId,
			null,
			threadId,
			state,
			c_noTimeSet,
			c_noTimeSet,
			0,
			false
			)
		{
		}

		public ThreadStorageView(
				String applicationId,
				String executorId,
				Int32 threadId,
				ThreadState state,
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
				ThreadState state,
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

		public string StateString
		{
			get
			{
				string state = "Unknown";
				switch (this.State)
				{
					case ThreadState.Dead:
						state = "Dead";
						break;
					case ThreadState.Ready:
						state = "Ready";
						break;
					case ThreadState.Finished:
						state = "Finished";
						break;
					case ThreadState.Scheduled:
						state = "Scheduled";
						break;
					case ThreadState.Started:
						state = "Started";
						break;
				}
				return state;
			}
		}
	}
}
