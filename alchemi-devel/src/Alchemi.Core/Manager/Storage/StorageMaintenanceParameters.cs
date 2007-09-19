#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   StorageMaintenanceParameters.cs
 * Project      :   Alchemi.Core.Manager.Storage
 * Created on   :   05 May 2006
 * Copyright    :   Copyright © 2006 Tibor Biro (tb@tbiro.com)
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
using System.Collections.Generic;
using System.Collections;
using System.Text;

using Alchemi.Core.Owner;

namespace Alchemi.Core.Manager.Storage
{
    /// <summary>
    /// Parameters passed to the Maintenance class to perform storage maintenance tasks.
    /// </summary>
    [Serializable]
    public class StorageMaintenanceParameters
    {
        // Application maintenance parameters

        #region Property - ApplicationTimeCreatedCutOff
        private TimeSpan _applicationTimeCreatedCutOff;
        /// <summary>
        /// TODO:
        /// </summary>
        public TimeSpan ApplicationTimeCreatedCutOff
        {
            get { return _applicationTimeCreatedCutOff; }
            set
            {
                _applicationTimeCreatedCutOff = value;
                _applicationTimeCreatedCutOffSet = true;
            }
        } 
        #endregion


        #region Property - ApplicationTimeCreatedCutOffSet
        private bool _applicationTimeCreatedCutOffSet = false;
        /// <summary>
        /// TODO:
        /// </summary>
        public bool ApplicationTimeCreatedCutOffSet
        {
            get { return _applicationTimeCreatedCutOffSet; }
        } 
        #endregion


        #region Property - ApplicationTimeCompletedCutOff
        private TimeSpan _applicationTimeCompletedCutOff;
        /// <summary>
        /// TODO:
        /// </summary>
        public TimeSpan ApplicationTimeCompletedCutOff
        {
            get { return _applicationTimeCompletedCutOff; }
            set
            {
                _applicationTimeCompletedCutOff = value;
                _applicationTimeCompletedCutOffSet = true;
            }
        } 
        #endregion


        #region Property - ApplicationTimeCompletedCutOffSet
        private bool _applicationTimeCompletedCutOffSet = false;
        /// <summary>
        /// TODO:
        /// </summary>
        public bool ApplicationTimeCompletedCutOffSet
        {
            get { return _applicationTimeCompletedCutOffSet; }
        } 
        #endregion


        #region Property - ApplicationStatesToRemove
        private List<ApplicationState> _applicationStatesToRemove;
        /// <summary>
        /// TODO:
        /// </summary>
        public ApplicationState[] ApplicationStatesToRemove
        {
            get
            {
                if (_applicationStatesToRemove == null)
                {
                    _applicationStatesToRemove = new List<ApplicationState>();
                }

                return _applicationStatesToRemove.ToArray();
            }
        } 
        #endregion


        #region Property - RemoveAllApplications
        private bool _removeAllApplications = false;
        /// <summary>
        /// TODO:
        /// </summary>
        public bool RemoveAllApplications
        {
            get { return _removeAllApplications; }
            set { _removeAllApplications = value; }
        } 
        #endregion


        // Executor maintenance parameters

        #region Property - ExecutorPingTimeCutOff
        private TimeSpan _executorPingTimeCutOff;
        /// <summary>
        /// TODO:
        /// </summary>
        public TimeSpan ExecutorPingTimeCutOff
        {
            get { return _executorPingTimeCutOff; }
            set
            {
                _executorPingTimeCutOff = value;
                _executorPingTimeCutOffSet = true;
            }
        } 
        #endregion


        #region Property - ExecutorPingTimeCutOffSet
        private bool _executorPingTimeCutOffSet = false;
        /// <summary>
        /// TODO:
        /// </summary>
        public bool ExecutorPingTimeCutOffSet
        {
            get { return _executorPingTimeCutOffSet; }
        } 
        #endregion


        #region Property - RemoveAllExecutors
        private bool _removeAllExecutors = false;
        /// <summary>
        /// TODO:
        /// </summary>
        public bool RemoveAllExecutors
        {
            get { return _removeAllExecutors; }
            set { _removeAllExecutors = value; }
        } 
        #endregion



        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageMaintenanceParameters"/> class.
        /// </summary>
        public StorageMaintenanceParameters()
        {
        } 
        #endregion



        public void AddApplicationStateToRemove(ApplicationState stateToAdd)
        {
            if (_applicationStatesToRemove == null)
            {
                _applicationStatesToRemove = new List<ApplicationState>();
            }

            _applicationStatesToRemove.Add(stateToAdd);
        }

        public void AddApplicationStatesToRemove(IEnumerable<ApplicationState> enumerable)
        {
            if (enumerable == null)
            {
                return;
            }

            IEnumerator<ApplicationState> enumerator = enumerable.GetEnumerator();
            while (enumerator.MoveNext())
            {
                AddApplicationStateToRemove(enumerator.Current);
            }
        }

    }
}
