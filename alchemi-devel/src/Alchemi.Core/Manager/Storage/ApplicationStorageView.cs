#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   ApplicationStorageView.cs
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
	/// Storage view of an application object. 
	/// Used to pass application related data to and from the storage layer.
	/// </summary>
	[Serializable]
	public class ApplicationStorageView
	{
		private const int c_valueNotSet = Int32.MaxValue;
		private static DateTime c_noDateTime = DateTime.MinValue;
		

        #region Property - ApplicationName
        private string m_appName;
        /// <summary>
        /// The application name.
        /// </summary>
        public string ApplicationName
        {
            get
            {
                if (m_appName != null && m_appName.Length > 0)
                {
                    return m_appName;
                }
                else
                {
                    return String.Format("Noname: [{0}]", ApplicationId);
                }
            }
            set
            {
                m_appName = value;
            }
        } 
        #endregion


        #region Property - ApplicationId
        private string m_applicationId;
        /// <summary>
        /// The Application Id.
        /// </summary>
        public string ApplicationId
        {
            get { return m_applicationId; }
            set { m_applicationId = value; }
        }
        #endregion


        #region Property - State
        private ApplicationState m_state;
        /// <summary>
        /// The Application state.
        /// <seealso cref="ApplicationState"/>
        /// </summary>
        public ApplicationState State
        {
            get { return m_state; }
            set { m_state = value; }
        } 
        #endregion


        #region Property - StateString
        /// <summary>
        /// Gets a human readable description of the ApplicationState property.
        /// <seealso cref="ApplicationState"/>
        /// </summary>
        public string StateString
        {
            get
            {
                string state = "";
                switch (this.State)
                {
                    case ApplicationState.AwaitingManifest:
                        state = "Awaiting Manifest";
                        break;
                    case ApplicationState.Ready:
                        state = "Running";
                        break;
                    case ApplicationState.Stopped:
                        state = "Finished";
                        break;
                }
                return state;
            }
        } 
        #endregion


        #region Property - TimeCreated
        private DateTime m_timeCreated;
        /// <summary>
        /// The time the application was created.
        /// </summary>
        public DateTime TimeCreated
        {
            get { return m_timeCreated; }
        } 
        #endregion


        #region Property - TimeCreatedSet
        /// <summary>
        /// Gets a name indicating whether the TimeCreated property is set or not.
        /// <seealso cref="TimeCreated"/>
        /// </summary>
        public bool TimeCreatedSet
        {
            get
            {
                return (m_timeCreated != c_noDateTime);
            }
        }
        #endregion


        #region Property - TimeCompleted
        private DateTime m_timeCompleted;
        /// <summary>
        /// The time the application was completed.
        /// </summary>
        public DateTime TimeCompleted
        {
            get { return m_timeCompleted; }
            set { m_timeCompleted = value; }
        } 
        #endregion


        #region Property - TimeCompletedSet
        /// <summary>
        /// Gets a name indicating whether the TimeCompleted property is set or not.
        /// <seealso cref="TimeCompleted"/>
        /// </summary>
        public bool TimeCompletedSet
        {
            get
            {
                return (m_timeCompleted != c_noDateTime);
            }
        } 
        #endregion


        #region Property - Primary
        private bool m_primary;
        /// <summary>
        /// Gets a name indicating whether this is the primary application.
        /// </summary>
        public bool Primary
        {
            get { return m_primary; }
        } 
        #endregion


        #region Property - Username
        private string m_username;
        /// <summary>
        /// The user that created this application.
        /// </summary>
        public string Username
        {
            get { return m_username; }
        } 
        #endregion


        #region Property - TotalThreads
        private int m_totalThreads = c_valueNotSet;
        /// <summary>
        /// The total thread count for this application.
        /// </summary>
        public int TotalThreads
        {
            get
            {
                if (m_totalThreads == c_valueNotSet)
                {
                    throw new Exception("The total thread name is not set for this application object.");
                }
                return m_totalThreads;
            }
            set
            {
                m_totalThreads = value;
            }
        } 
        #endregion


        #region Property - UnfinishedThreads
        private int m_unfinishedThreads = c_valueNotSet;
        /// <summary>
        /// The unfinished thread count for this application.
        /// </summary>
        public int UnfinishedThreads
        {
            get
            {
                if (m_unfinishedThreads == c_valueNotSet)
                {
                    throw new Exception("The unfinished thread name is not set for this application object.");
                }
                return m_unfinishedThreads;
            }
            set
            {
                m_unfinishedThreads = value;
            }
        } 
        #endregion


        #region Constructors
        /// <summary>
        /// ApplicationStorageView constructor.
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="state"></param>
        /// <param name="timeCreated"></param>
        /// <param name="primary"></param>
        /// <param name="username"></param>
        public ApplicationStorageView(
                string applicationId,
                ApplicationState state,
                DateTime timeCreated,
                bool primary,
                string username
        )
        {
            m_applicationId = applicationId;
            m_state = state;
            m_timeCreated = timeCreated;
            m_primary = primary;
            m_username = username;
        }


        /// <summary>
        /// ApplicationStorageView constructor.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="timeCreated"></param>
        /// <param name="primary"></param>
        /// <param name="username"></param>
        public ApplicationStorageView(
                ApplicationState state,
                DateTime timeCreated,
                bool primary,
                string username
            )
            : this(
                null,
                state,
                timeCreated,
                primary,
                username
            )
        {
        }


        /// <summary>
        /// ApplicationStorageView constructor.
        /// Initialize an application with only a username supplied.
        /// </summary>
        /// <param name="username"></param>
        public ApplicationStorageView(
                string username
            )
            : this(
                null,
                ApplicationState.Stopped,
                c_noDateTime,
                true,
                username
            )
        {
        } 
        #endregion

	}
}
