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
        private string _appName;
        /// <summary>
        /// The application name.
        /// </summary>
        public string ApplicationName
        {
            get
            {
                if (_appName != null && _appName.Length > 0)
                {
                    return _appName;
                }
                else
                {
                    return String.Format("Noname: [{0}]", ApplicationId);
                }
            }
            set
            {
                _appName = value;
            }
        } 
        #endregion


        #region Property - ApplicationId
        private string _applicationId;
        /// <summary>
        /// The Application Id.
        /// </summary>
        public string ApplicationId
        {
            get { return _applicationId; }
            set { _applicationId = value; }
        }
        #endregion


        #region Property - State
        private ApplicationState _state;
        /// <summary>
        /// The Application state.
        /// <seealso cref="ApplicationState"/>
        /// </summary>
        public ApplicationState State
        {
            get { return _state; }
            set { _state = value; }
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
        private DateTime _timeCreated;
        /// <summary>
        /// The time the application was created.
        /// </summary>
        public DateTime TimeCreated
        {
            get { return _timeCreated; }
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
                return (_timeCreated != c_noDateTime);
            }
        }
        #endregion


        #region Property - TimeCompleted
        private DateTime _timeCompleted;
        /// <summary>
        /// The time the application was completed.
        /// </summary>
        public DateTime TimeCompleted
        {
            get { return _timeCompleted; }
            set { _timeCompleted = value; }
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
                return (_timeCompleted != c_noDateTime);
            }
        } 
        #endregion


        #region Property - Primary
        private bool _primary;
        /// <summary>
        /// Gets a name indicating whether this is the primary application.
        /// </summary>
        public bool Primary
        {
            get { return _primary; }
        } 
        #endregion


        #region Property - Username
        private string _username;
        /// <summary>
        /// The user that created this application.
        /// </summary>
        public string Username
        {
            get { return _username; }
        } 
        #endregion


        #region Property - TotalThreads
        private int _totalThreads = c_valueNotSet;
        /// <summary>
        /// The total thread count for this application.
        /// </summary>
        public int TotalThreads
        {
            get
            {
                if (_totalThreads == c_valueNotSet)
                {
                    throw new Exception("The total thread name is not set for this application object.");
                }
                return _totalThreads;
            }
            set
            {
                _totalThreads = value;
            }
        } 
        #endregion


        #region Property - UnfinishedThreads
        private int _unfinishedThreads = c_valueNotSet;
        /// <summary>
        /// The unfinished thread count for this application.
        /// </summary>
        public int UnfinishedThreads
        {
            get
            {
                if (_unfinishedThreads == c_valueNotSet)
                {
                    throw new Exception("The unfinished thread name is not set for this application object.");
                }
                return _unfinishedThreads;
            }
            set
            {
                _unfinishedThreads = value;
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
            _applicationId = applicationId;
            _state = state;
            _timeCreated = timeCreated;
            _primary = primary;
            _username = username;
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
