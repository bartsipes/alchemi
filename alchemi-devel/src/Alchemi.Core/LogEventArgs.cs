#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   LogEventArgs.cs
 * Project      :   Alchemi.Core
 * Created on   :   August 2005
 * Copyright    :   Copyright © 2006 The University of Melbourne
 *                  This technology has been developed with the support of 
 *                  the Australian Research Council and the University of Melbourne
 *                  research grants as part of the Gridbus Project
 *                  within GRIDS Laboratory at the University of Melbourne, Australia.
 * Author       :   Krishna Nadiminti (kna@csse.unimelb.edu.au)
 *                  Rajkumar Buyya (raj@csse.unimelb.edu.au)
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
using System.Text;

namespace Alchemi.Core
{
    /// <summary>
    /// Delegate for the log event
    /// </summary>
    public delegate void LogEventHandler(object sender, LogEventArgs e);


    /// <summary>
    /// The arguments passed when raising a log event.
    /// </summary>
    public class LogEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LogEventArgs"/> class.
        /// </summary>
        public LogEventArgs()
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LogEventArgs"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="sourceMember">The source member.</param>
        /// <param name="message">The message.</param>
        /// <param name="level">The log level.</param>
        /// <param name="ex">The exception.</param>
        public LogEventArgs(string source, string sourceMember, string message, LogLevel level, Exception ex)
        {
            this._Source = source;
            this._Member = sourceMember;
            this._Message = message;
            this._Level = level;
            this._Exception = ex;
        } 
        #endregion



        #region Property - Source
        private string _Source;
        /// <summary>
        /// Gets the source 
        /// </summary>
        public string Source
        {
            get { return _Source; }
        }
        #endregion


        #region Property - Member
        private string _Member;
        /// <summary>
        /// Gets the member of the source class that raised the log event
        /// </summary>
        public string Member
        {
            get { return _Member; }
        }
        #endregion


        #region Property - Message
        private string _Message;
        /// <summary>
        /// Getsthe log message
        /// </summary>
        public string Message
        {
            get { return _Message; }
        }
        #endregion


        #region Property - Level
        private LogLevel _Level = LogLevel.Info;
        /// <summary>
        /// Gets the level of the log message
        /// </summary>
        public LogLevel Level
        {
            get { return _Level; }
        } 
        #endregion


        #region Property - Exception
        private Exception _Exception = null;
        /// <summary>
        /// Gets the exception for the log event
        /// </summary>
        public Exception Exception
        {
            get { return _Exception; }
        } 
        #endregion

    }
}
