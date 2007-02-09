#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	Logger.cs
* Project		:	Alchemi Core
* Created on	:	August 2005
* Copyright		:	Copyright © 2006 The University of Melbourne
*					This technology has been developed with the support of 
*					the Australian Research Council and the University of Melbourne
*					research grants as part of the Gridbus Project
*					within GRIDS Laboratory at the University of Melbourne, Australia.
* Author         :  Krishna Nadiminti (kna@csse.unimelb.edu.au) and Rajkumar Buyya (raj@csse.unimelb.edu.au) 
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
using System.Diagnostics;
using System.IO;

namespace Alchemi.Core
{
    //kna: modified this class to inherit from MarshalByRef so we can use the logger across app-domains

    /// <summary>
    /// The Alchemi logger class raises log events which can be handled by other classes.
    /// This allows to log messages using any logging system the log-event-handler may choose.
    /// </summary>
    public class Logger : MarshalByRefObject
    {
        /// <summary>
        /// Logger Event Handler
        /// </summary>
        public static LogEventHandler LogHandler;

        /// <summary>
        /// Creates an instance of the logger.
        /// </summary>
        public Logger()
        {
        }

        /// <summary>
        /// Raises a log event with the given message and Info level
        /// </summary>
        /// <param name="msg"></param>
        public void Info(string msg)
        {
            RaiseLogEvent(msg, LogLevel.Info, null);
        }

        /// <summary>
        /// Raises a log event with the given message and Debug level
        /// </summary>
        /// <param name="debugMsg"></param>
        public void Debug(string debugMsg)
        {
            RaiseLogEvent(debugMsg, LogLevel.Debug, null);
        }

        /// <summary>
        /// Raises a log event with the given message and Debug level and exception
        /// </summary>
        /// <param name="debugMsg"></param>
        /// <param name="ex"></param>
        public void Debug(string debugMsg, Exception ex)
        {
            RaiseLogEvent(debugMsg, LogLevel.Debug, ex);
        }

        /// <summary>
        /// Raises a log event with the given message and Error level and exception
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public void Error(string msg, Exception ex)
        {
            RaiseLogEvent(msg, LogLevel.Error, ex);
        }

        /// <summary>
        /// Raises a log event with the given message and Warn level
        /// </summary>
        /// <param name="msg"></param>
        public void Warn(string msg)
        {
            RaiseLogEvent(msg, LogLevel.Warn, null);
        }

        /// <summary>
        /// Raises a log event with the given message and Warn level and exception
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public void Warn(string msg, Exception ex)
        {
            RaiseLogEvent(msg, LogLevel.Warn, ex);
        }

        private void RaiseLogEvent(string msg, LogLevel level, Exception ex)
        {
            string source = "?source?";
            string member = "?member?";
            try
            {

                if (LogHandler == null)
                {
                    return;
                }

                //make sure two stackframes above, we have the actually call to the logger! otherwise we get the wrong name!
                //for this, make sure the RaiseLogEvent method is private..and is called by all other logger.XXXX methods
                StackFrame s = new StackFrame(2, true);
                if (s != null)
                {
                    if (s.GetMethod().DeclaringType != null)
                        source = s.GetMethod().DeclaringType.Name;

                    if (s.GetMethod() != null)
                        member = s.GetMethod().Name + "():" + s.GetFileLineNumber();
                }
            }
            catch { }

            RaiseLogEvent(msg, level, ex, source, member);
        }

        private void RaiseLogEvent(string msg, LogLevel level, Exception ex, String source, String member)
        {
            try
            {
                //Raise the log event
                if (LogHandler != null)
                    LogHandler(source, new LogEventArgs(source, member, msg, level, ex));
            }
            catch (Exception) { } //always handle errors when raising events. (since event-handlers are not in our control).

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }




}
