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
        private TimeSpan m_applicationTimeCreatedCutOff;
        /// <summary>
        /// TODO:
        /// </summary>
        public TimeSpan ApplicationTimeCreatedCutOff
        {
            get { return m_applicationTimeCreatedCutOff; }
            set
            {
                m_applicationTimeCreatedCutOff = value;
                m_applicationTimeCreatedCutOffSet = true;
            }
        } 
        #endregion


        #region Property - ApplicationTimeCreatedCutOffSet
        private bool m_applicationTimeCreatedCutOffSet = false;
        /// <summary>
        /// TODO:
        /// </summary>
        public bool ApplicationTimeCreatedCutOffSet
        {
            get { return m_applicationTimeCreatedCutOffSet; }
        } 
        #endregion


        #region Property - ApplicationTimeCompletedCutOff
        private TimeSpan m_applicationTimeCompletedCutOff;
        /// <summary>
        /// TODO:
        /// </summary>
        public TimeSpan ApplicationTimeCompletedCutOff
        {
            get { return m_applicationTimeCompletedCutOff; }
            set
            {
                m_applicationTimeCompletedCutOff = value;
                m_applicationTimeCompletedCutOffSet = true;
            }
        } 
        #endregion


        #region Property - ApplicationTimeCompletedCutOffSet
        private bool m_applicationTimeCompletedCutOffSet = false;
        /// <summary>
        /// TODO:
        /// </summary>
        public bool ApplicationTimeCompletedCutOffSet
        {
            get { return m_applicationTimeCompletedCutOffSet; }
        } 
        #endregion


        #region Property - ApplicationStatesToRemove
        private ArrayList m_applicationStatesToRemove;
        /// <summary>
        /// TODO:
        /// </summary>
        public ApplicationState[] ApplicationStatesToRemove
        {
            get
            {
                if (m_applicationStatesToRemove == null)
                {
                    m_applicationStatesToRemove = new ArrayList();
                }

                return (ApplicationState[])m_applicationStatesToRemove.ToArray(typeof(ApplicationState));
            }
        } 
        #endregion


        #region Property - RemoveAllApplications
        private bool m_removeAllApplications = false;
        /// <summary>
        /// TODO:
        /// </summary>
        public bool RemoveAllApplications
        {
            get { return m_removeAllApplications; }
            set { m_removeAllApplications = value; }
        } 
        #endregion


        // Executor maintenance parameters

        #region Property - ExecutorPingTimeCutOff
        private TimeSpan m_executorPingTimeCutOff;
        /// <summary>
        /// TODO:
        /// </summary>
        public TimeSpan ExecutorPingTimeCutOff
        {
            get { return m_executorPingTimeCutOff; }
            set
            {
                m_executorPingTimeCutOff = value;
                m_executorPingTimeCutOffSet = true;
            }
        } 
        #endregion


        #region Property - ExecutorPingTimeCutOffSet
        private bool m_executorPingTimeCutOffSet = false;
        /// <summary>
        /// TODO:
        /// </summary>
        public bool ExecutorPingTimeCutOffSet
        {
            get { return m_executorPingTimeCutOffSet; }
        } 
        #endregion


        #region Property - RemoveAllExecutors
        private bool m_removeAllExecutors = false;
        /// <summary>
        /// TODO:
        /// </summary>
        public bool RemoveAllExecutors
        {
            get { return m_removeAllExecutors; }
            set { m_removeAllExecutors = value; }
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
            if (m_applicationStatesToRemove == null)
            {
                m_applicationStatesToRemove = new ArrayList();
            }

            m_applicationStatesToRemove.Add(stateToAdd);
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
