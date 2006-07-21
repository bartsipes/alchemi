#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	MappingScheduler.cs
* Project		:	Alchemi Manager
* Created on	:	16th July 2006
* Copyright		:	Copyright © 2006 The University of Melbourne
*					This technology has been developed with the support of 
*					the Australian Research Council and the University of Melbourne
*					research grants as part of the Gridbus Project
*					within GRIDS Laboratory at the University of Melbourne, Australia.
* Author         :  Michael Meadows (michael@meadows.force9.co.uk)
* License        :  GPL
*					This program is free software; you can redistribute it and/or 
*					modify it under the terms of the GNU General Public
*					License as published by the Free Software Foundation;
*					See the GNU General Public License 
*					(http://www.gnu.org/copyleft/gpl.html) for more details.
*
*/

#endregion

using System;
using System.Collections;

using Alchemi.Core;
using Alchemi.Core.Manager.Storage;
using Alchemi.Core.Owner;
using Alchemi.Core.Utility;
using Alchemi.Manager.Storage;

namespace Alchemi.Manager
{
    /// <summary>
    /// MappingScheduler class represents a scheduler where applications are mapped to executors. The term 'mapping' is used to try and avoid confusion with 'assigning' where a thread is assigned to an executor. A 'mapped' executor receives threads from the corresponding 'mapped' application. An application is mapped to an executor by an algorithm that takes the applications and distributes them evenly across the executors. If there are more executors than applications then an application may be mapped to more than one executor. Conversely, if there are more applications than executors then an executor may be mapped to more than one application. 
    /// </summary>
    public class MappingScheduler : IScheduler
    {
        private static readonly Logger logger = new Logger();

        private Mapping m_oMapping;

        /// <summary>
        /// Default constructor required by scheduler factory.
        /// </summary>
        public MappingScheduler()
        {
            m_oMapping = new Mapping();
        }

        /// <summary>
        /// Schedules a thread for the given executor id.
        /// </summary>
        /// <param name="strExecutorId">executor id</param>
        /// <returns>thread identifier</returns>
        public ThreadIdentifier ScheduleNonDedicated(string strExecutorId)
        {
            lock(this)
            {
                m_oMapping.Update();

                IList cApplicationIds = m_oMapping.GetApplicationIds(strExecutorId);
                ThreadStorageView oThreadStorage = GetNextThread(cApplicationIds);
                if (oThreadStorage != null)
                {
                    logger.Debug(String.Format("ScheduleNonDedicated; ApplicationId = {0}, ThreadId = {1}, ExecutorId = {2}", oThreadStorage.ApplicationId, oThreadStorage.ThreadId, strExecutorId));

                    string strApplicationId = oThreadStorage.ApplicationId;
                    int nThreadId = oThreadStorage.ThreadId;
                    int nPriority = oThreadStorage.Priority;

                    return new ThreadIdentifier(strApplicationId, nThreadId, nPriority);
                }
            }

            return null;
        }

        /// <summary>
        /// Get the next thread from the given application ids with the highest priority.
        /// </summary>
        /// <param name="cApplicationIds">application ids</param>
        /// <returns>next thread</returns>
        private ThreadStorageView GetNextThread(IList cApplicationIds)
        {
            IList cThreads = GetThreads(cApplicationIds);

            if (cThreads.Count != 0)
            {
                return GetHighestPriorityThread(cThreads);
            }
            
            return null;
        }

        /// <summary>
        /// Gets all the threads from the given application ids.
        /// </summary>
        /// <param name="cApplicationIds">application ids</param>
        /// <returns>all threads from application ids</returns>
        private IList GetThreads(IList cApplicationIds)
        {
            ArrayList cThreads = new ArrayList();
            foreach (string strApplicationId in cApplicationIds)
            {
                cThreads.AddRange(ManagerStorageFactory.ManagerStorage().GetThreads(strApplicationId, ThreadState.Ready));
            }
            return cThreads;
        }

        /// <summary>
        /// Gets the hightest priority thread from the given threads.
        /// </summary>
        /// <param name="cThreads">threads</param>
        /// <returns>highest priority thread</returns>
        private ThreadStorageView GetHighestPriorityThread(IList cThreads)
        {
            ThreadStorageView oHighestPriorityThread = (ThreadStorageView ) cThreads[0];
            foreach (ThreadStorageView oThread in cThreads)
            {
                if (oThread.Priority > oHighestPriorityThread.Priority)
                {
                    oHighestPriorityThread = oThread;
                }
            }
            return oHighestPriorityThread;
        }

        /// <summary>
        /// Schedules a thread for the dedicated executors.
        /// </summary>
        /// <returns>thread identifier</returns>
        public DedicatedSchedule ScheduleDedicated()
        {
            lock(this)
            {
                m_oMapping.Update();

                ThreadStorageView oThreadStorage = GetNextThread();
                if (oThreadStorage != null)
                {
                    string strApplicationId = oThreadStorage.ApplicationId;
                    string strExecutorId = GetNextExecutor(strApplicationId);
                    if (strExecutorId != null)
                    {
                        int nThreadId = oThreadStorage.ThreadId;
                        int nPriority = oThreadStorage.Priority;

                        logger.Debug(String.Format("ScheduleDedicated; ApplicationId = {0}, ThreadId = {1}, ExecutorId = {2}", strApplicationId, nThreadId, strExecutorId));

                        ThreadIdentifier oThreadIdentifier = new ThreadIdentifier(strApplicationId, nThreadId, nPriority);
                        return new DedicatedSchedule(oThreadIdentifier, strExecutorId);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the next thread with the highest priority.
        /// </summary>
        /// <returns>next thread</returns>
        private ThreadStorageView GetNextThread()
        {
            IList cThreads = GetThreads();

            if (cThreads.Count != 0)
            {
                return GetHighestPriorityThread(cThreads);
            }

            return null;
        }

        /// <summary>
        /// Gets the threads.
        /// </summary>
        /// <returns></returns>
        private IList GetThreads()
        {
            return new ArrayList(ManagerStorageFactory.ManagerStorage().GetThreads(ApplicationState.Ready, ThreadState.Ready));
        }

        /// <summary>
        /// Gets the next executor for the given application id.
        /// </summary>
        /// <param name="strApplicationId">application id</param>
        /// <returns>next executor for application</returns>
        private string GetNextExecutor(string strApplicationId)
        {
            IList cExecutorIds = m_oMapping.GetExecutorIds(strApplicationId);
            foreach (string strExecutorId in cExecutorIds)
            {
                int nThreadCount = ManagerStorageFactory.ManagerStorage().GetExecutorThreadCount(strExecutorId, ThreadState.Ready, ThreadState.Scheduled, ThreadState.Started);
                if (nThreadCount == 0)
                {
                    return strExecutorId;
                }
            }

            return null;
        }

        #region Applications and Executors properties

        // TODO: Applications and Executors properties are not used in DefaultScheduler or ExecutorScheduler probably because ManagerStorage is used instead. Consider changing IScheduler interface to reflect this.

        private MApplicationCollection m_cApplications;
        private MExecutorCollection m_cExecutors;

        /// <summary>
        /// Applications property represents the applications.
        /// </summary>
        public MApplicationCollection Applications
        {
            set
            {
                m_cApplications = value;
            }
        }

        /// <summary>
        /// Executors property represents the executors.
        /// </summary>
        public MExecutorCollection Executors
        {
            set
            {
                m_cExecutors = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// Mapping class is responsible for the application to executor mapping.
    /// </summary>
    public class Mapping
    {
        private static readonly Logger logger = new Logger();

        private Hashtable m_cApplicationExecutor;
        private Hashtable m_cExecutorApplication;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Mapping()
        {
            m_cApplicationExecutor = new Hashtable();
            m_cExecutorApplication = new Hashtable();
        }

        /// <summary>
        /// Gets the application ids mapped to the given executor id.
        /// </summary>
        /// <param name="strExecutorId">executor id</param>
        /// <returns>application ids</returns>
        public IList GetApplicationIds(string strExecutorId)
        {
            IList cList = (IList) m_cExecutorApplication[strExecutorId];
            return (cList != null) ? cList : new ArrayList();
        }

        /// <summary>
        /// Gets the executors ids mapped to the given application id.
        /// </summary>
        /// <param name="strApplicationId">application id</param>
        /// <returns>executor ids</returns>
        public IList GetExecutorIds(string strApplicationId)
        {
            IList cList = (IList) m_cApplicationExecutor[strApplicationId];
            return (cList != null) ? cList : new ArrayList();
        }

        /// <summary>
        /// Updates the application executor mappings by distributing the applications evenly across the executors. This naive implementation should be changed to improve performance and take into account the application priorities.
        /// </summary>
        public void Update()
        {
            Clear();
            
            IList cActiveApplicationIds = GetActiveApplicationIds();
            IList cActiveExecutorIds = GetActiveExecutorIds();

            if ((cActiveApplicationIds.Count > 0) && (cActiveExecutorIds.Count > 0)) 
            {
                bool bResetApplicationIndex = false;
                bool bResetExecutorIndex = false;

                int nApplicationIndex = 0;
                int nExecutorIndex = 0;
                while ((!bResetApplicationIndex) || (!bResetExecutorIndex))
                {
                    string strApplicationId = (string) cActiveApplicationIds[nApplicationIndex];
                    string strExecutorId = (string) cActiveExecutorIds[nExecutorIndex];
                    Map(strApplicationId, strExecutorId);

                    nApplicationIndex++;
                    nExecutorIndex++;

                    if (nApplicationIndex >= cActiveApplicationIds.Count)
                    {
                        nApplicationIndex = 0;
                        bResetApplicationIndex = true;
                    }

                    if (nExecutorIndex >= cActiveExecutorIds.Count)
                    {
                        nExecutorIndex = 0;
                        bResetExecutorIndex = true;
                    }
                }
            }
        }

        /// <summary>
        /// Clears the mappings.
        /// </summary>
        private void Clear()
        {
            m_cApplicationExecutor.Clear();
            m_cExecutorApplication.Clear();
        }

        /// <summary>
        /// Gets the active application ids.
        /// </summary>
        /// <returns></returns>
        private IList GetActiveApplicationIds()
        {
            IList cActiveApplicationIds = new ArrayList();

            ApplicationStorageView[] cApplications = ManagerStorageFactory.ManagerStorage().GetApplications();
            foreach (ApplicationStorageView oApplication in cApplications)
            {
                if (oApplication.State == ApplicationState.Ready)
                {
                    cActiveApplicationIds.Add(oApplication.ApplicationId);
                }
            }

            return cActiveApplicationIds;
        }

        /// <summary>
        /// Gets the active executor ids.
        /// </summary>
        /// <returns></returns>
        private IList GetActiveExecutorIds()
        {
            IList cActiveExecutorIds = new ArrayList();

            ExecutorStorageView[] cExecutors = ManagerStorageFactory.ManagerStorage().GetExecutors();
            foreach (ExecutorStorageView oExecutor in cExecutors)
            {
                if (oExecutor.Connected)
                {
                    cActiveExecutorIds.Add(oExecutor.ExecutorId);
                }
            }

            return cActiveExecutorIds;
        }

        /// <summary>
        /// Maps the given application id to the given executor id.
        /// </summary>
        /// <param name="strApplicationId">application id</param>
        /// <param name="strExecutorId">executor id</param>
        private void Map(string strApplicationId, string strExecutorId)
        {
            AddToList(strApplicationId, strExecutorId, m_cApplicationExecutor);
            AddToList(strExecutorId, strApplicationId, m_cExecutorApplication);
        }

        /// <summary>
        /// Adds a value to a list in the hashtable.
        /// </summary>
        /// <param name="strKey">key</param>
        /// <param name="strValue">value</param>
        /// <param name="cHashtable">hashtable</param>
        private void AddToList(string strKey, string strValue, Hashtable cHashtable)
        {
            IList cList = (IList) cHashtable[strKey];
            if (cList == null)
            {
                cList = new ArrayList();
                cHashtable[strKey] = cList;
            }
            cList.Add(strValue);
        }
    }
}