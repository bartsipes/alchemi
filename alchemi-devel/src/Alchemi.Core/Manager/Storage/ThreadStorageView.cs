#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   ThreadStorageView.cs
 * Project      :   Alchemi.Core.Manager.Storage
 * Created on   :   19 October 2005
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

using Alchemi.Core.Owner;

namespace Alchemi.Core.Manager.Storage
{
	/// <summary>
	/// Storage view of an thread object. 
	/// Used to pass thread related data to and from the storage layer.
	/// </summary>
	[Serializable]
	public class ThreadStorageView
	{
		private static DateTime c_noTimeSet = DateTime.MinValue;


        #region Property - InternalThreadId
        private int m_internalThreadId;
        /// <summary>
        /// Internal thread Id.
        /// </summary>
        public int InternalThreadId
        {
            get { return m_internalThreadId; }
            set { m_internalThreadId = value; }
        } 
        #endregion


        #region Property - ApplicationId
        private string m_applicationId;
        /// <summary>
        /// The application Id to which this thread belongs to.
        /// </summary>
        public string ApplicationId
        {
            get { return m_applicationId; }
            set { m_applicationId = value; }
        } 
        #endregion


        #region Property - ExecutorId
        private string m_executorId;
        /// <summary>
        /// The executor Id of the Executor on which this thread was executed.
        /// </summary>
        public string ExecutorId
        {
            get { return m_executorId; }
            set { m_executorId = value; }
        } 
        #endregion


        #region Property - ThreadId
        private int m_threadId;
        /// <summary>
        /// The thread Id.
        /// </summary>
        public int ThreadId
        {
            get { return m_threadId; }
            set { m_threadId = value; }
        } 
        #endregion


        #region Property - State
        private ThreadState m_state;
        /// <summary>
        /// The thread state.
        /// <seealso cref="ThreadState"/>
        /// </summary>
        public ThreadState State
        {
            get { return m_state; }
            set { m_state = value; }
        } 
        #endregion


        #region Property - TimeStarted
        private DateTime m_timeStarted;
        /// <summary>
        /// The time this thread was started.
        /// <seealso cref="TimeStartedSet"/>
        /// </summary>
        public DateTime TimeStarted
        {
            get { return m_timeStarted; }
            set { m_timeStarted = value; }
        } 
        #endregion


        #region Property - TimeStartedSet
        /// <summary>
        /// Gets a name indicating whether the TimeStarted property is set.
        /// <seealso cref="TimeStarted"/>
        /// </summary>
        public bool TimeStartedSet
        {
            get { return (m_timeStarted != c_noTimeSet); }
        }
        #endregion


        #region Property - TimeFinished
        private DateTime m_timeFinished;
        /// <summary>
        /// The time this thread was finished.
        /// <seealso cref="TimeFinishedSet"/>
        /// </summary>
        public DateTime TimeFinished
        {
            get { return m_timeFinished; }
            set { m_timeFinished = value; }
        } 
        #endregion


        #region Property - TimeFinishedSet
        /// <summary>
        /// Gets a name indicating whether the TimeFinished property is set.
        /// </summary>
        public bool TimeFinishedSet
        {
            get { return (m_timeFinished != c_noTimeSet); }
        } 
        #endregion


        #region Property - Priority
        private int m_priority;
        /// <summary>
        /// The thread priority.
        /// </summary>
        public int Priority
        {
            get { return m_priority; }
            set { m_priority = value; }
        } 
        #endregion


        #region Property - Failed
        private bool m_failed;
        /// <summary>
        /// Gets a name indicating whether this thread failed running.
        /// </summary>
        public bool Failed
        {
            get { return m_failed; }
        } 
        #endregion



        #region Method - ResetTimeStarted
        /// <summary>
        /// Reset the TimeStarted property to the default name.
        /// </summary>
        public void ResetTimeStarted()
        {
            m_timeStarted = c_noTimeSet;
        } 
        #endregion


        #region Method - ResetTimeFinished
        /// <summary>
        /// Reset the TimeFinished property to the default name.
        /// </summary>
        public void ResetTimeFinished()
        {
            m_timeFinished = c_noTimeSet;
        } 
        #endregion



        #region Constructors
        /// <summary>
        /// ThreadStorageView constructor.
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="threadId"></param>
        public ThreadStorageView(
            string applicationId,
            int threadId
            )
            : this(
            applicationId,
            threadId,
            ThreadState.Unknown
            )
        {
        }

        /// <summary>
        /// ThreadStorageView constructor.
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="threadId"></param>
        /// <param name="state"></param>
        public ThreadStorageView(
            string applicationId,
            int threadId,
            ThreadState state
            )
            : this(
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

        /// <summary>
        /// ThreadStorageView constructor.
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="executorId"></param>
        /// <param name="threadId"></param>
        /// <param name="state"></param>
        /// <param name="timeStarted"></param>
        /// <param name="timeFinished"></param>
        /// <param name="priority"></param>
        /// <param name="failed"></param>
        public ThreadStorageView(
                string applicationId,
                string executorId,
                int threadId,
                ThreadState state,
                DateTime timeStarted,
                DateTime timeFinished,
                int priority,
                bool failed
            )
            : this(
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

        /// <summary>
        /// ThreadStorageView constructor.
        /// </summary>
        /// <param name="internalThreadId"></param>
        /// <param name="applicationId"></param>
        /// <param name="executorId"></param>
        /// <param name="threadId"></param>
        /// <param name="state"></param>
        /// <param name="timeStarted"></param>
        /// <param name="timeFinished"></param>
        /// <param name="priority"></param>
        /// <param name="failed"></param>
        public ThreadStorageView(
                int internalThreadId,
                string applicationId,
                string executorId,
                int threadId,
                ThreadState state,
                DateTime timeStarted,
                DateTime timeFinished,
                int priority,
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
        #endregion


		/// <summary>
		/// Gets a hunam readable description of the State property.
		/// <seealso cref="State"/>
		/// </summary>
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
