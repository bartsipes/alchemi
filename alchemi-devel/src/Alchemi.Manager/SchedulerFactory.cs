#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	SchedulerFactory.cs
* Project		:	Alchemi Manager
* Created on	:	15th July 2006
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
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Alchemi.Core;

namespace Alchemi.Manager
{
    /// <summary>
    /// SchedulerFactory class is responsible for creating the scheduler. 
    /// This factory uses reflection to create a scheduler based upon the scheduler 
    /// assembly name and scheduler type name specified in the manager's configuration file.
    /// If a scheduler has not been specified or if it fails to create the specified 
    /// scheduler then the default scheduler is created.
    /// </summary>
    public class SchedulerFactory
    {
        private static readonly Logger logger = new Logger();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SchedulerFactory()
        {
        }

        /// <summary>
        /// Creates a scheduler based upon the scheduler assembly name and 
        /// scheduler type name specified in the manager's configuration file. 
        /// If it fails to create the specified scheduler then it returns the default scheduler.
        /// </summary>
        /// <returns></returns>
        public IScheduler CreateScheduler()
        {
            Configuration oConfiguration = GetConfiguration();
            if (oConfiguration != null)
            {
                string strSchedulerAssemblyName = oConfiguration.SchedulerAssemblyName;
                if (IsSpecified(strSchedulerAssemblyName))
                {
                    string strSchedulerTypeName = oConfiguration.SchedulerTypeName;
                    if (IsSpecified(strSchedulerTypeName))
                    {
                        IScheduler oScheduler = CreateScheduler(strSchedulerAssemblyName, strSchedulerTypeName);
                        if (oScheduler != null)
                        {
                            return oScheduler;
                        }
                    }
                    else
                    {
                        logger.Debug("Scheduler type name was not specified.");
                    }
                }
                else
                {
                    logger.Debug("Scheduler assembly name was not specified.");
                }
            }

            logger.Debug("Creating default scheduler.");
            return new DefaultScheduler();
        }

        /// <summary>
        /// Creates a scheduler based upon the given scheduler assembly name and scheduler type name. If it fails to create the specified scheduler then it returns null.
        /// </summary>
        /// <param name="strSchedulerAssemblyName">scheduler assembly name</param>
        /// <param name="strSchedulerTypeName">scheduler type name</param>
        /// <returns>scheduler</returns>
        private IScheduler CreateScheduler(string strSchedulerAssemblyName, string strSchedulerTypeName)
        {
            logger.Debug(String.Format("Creating specified scheduler; strSchedulerAssemblyName = '{0}', strSchedulerTypeName = '{1}'", strSchedulerAssemblyName, strSchedulerTypeName));

            try
            {
                Assembly oAssembly = Assembly.Load(strSchedulerAssemblyName);
                object oScheduler = oAssembly.CreateInstance(strSchedulerTypeName);
                if (oScheduler != null)
                {
                    if (oScheduler is IScheduler)
                    {
                        return (IScheduler) oScheduler;
                    }
                    else
                    {
                        logger.Debug("Created scheduler does not implement IScheduler.");
                    }
                }
                else
                {
                    logger.Debug("Created scheduler is null; scheduler type is not found.");
                }
            }
            catch (Exception oException)
            {
                logger.Debug(string.Format("Failed to create specified scheduler; {0}", oException));
            }

            return null;
        }

        /// <summary>
        /// Gets the manager's configuration object. If it fails to get the manager's configuration object then it returns null.
        /// </summary>
        /// <returns>manager's configuration object</returns>
        private Configuration GetConfiguration()
        {
            try
            {
                return Configuration.GetConfiguration();
            }
            catch (Exception oException)
            {
                logger.Debug(string.Format("Failed to read configuration; {0}", oException));
            }

            return null;
        }

        /// <summary>
        /// Determines whether the given name is 'specified'.
        /// </summary>
        /// <param name="strValue">name</param>
        /// <returns>whether it is specified</returns>
        private bool IsSpecified(string strValue)
        {
            return ((strValue != null) && (strValue.Length > 0));
        }
    }
}