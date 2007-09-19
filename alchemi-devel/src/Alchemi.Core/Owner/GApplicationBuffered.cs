#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   GApplicationBuffered.cs
 * Project      :   Alchemi.Core.Owner
 * Created on   :   2003
 * Copyright    :   Copyright © 2006 The University of Melbourne
 *                  This technology has been developed with the support of 
 *                  the Australian Research Council and the University of Melbourne
 *                  research grants as part of the Gridbus Project
 *                  within GRIDS Laboratory at the University of Melbourne, Australia.
 * Author       :   Michael Meadows (michael@meadows.force9.co.uk)
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

namespace Alchemi.Core.Owner
{
    /// <summary>
    /// GApplicationBuffered class represents a buffered multi-use application. 
    /// StartThread method has been overridden such that the thread is not immediately started rather 
    /// it is placed in a thread buffer. When the thread buffer count reaches the thread buffer capacity 
    /// then that thread buffer is sent to the manager as one thread. The GApplicationBuffered class 
    /// should be used when there are many threads with short execution times. Under these conditions, 
    /// using GApplicationBuffered class can significantly improve performance compared to 
    /// using GApplication class.
    /// </summary>
    public class GApplicationBuffered : GApplication
    {
        private const int DefaultThreadBufferCapacity = 8;

        
        private GThreadBuffer _ThreadBuffer;
        private int _InternalThreadId;


        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public GApplicationBuffered()
            : this(DefaultThreadBufferCapacity)
        {
        }

        /// <summary>
        /// Constructor that takes the given connection.
        /// </summary>
        /// <param name="connection">connection to alchemi manager</param>
        public GApplicationBuffered(GConnection connection)
            : this(DefaultThreadBufferCapacity)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection may not be null");
            }

            Connection = connection;
        }

        /// <summary>
        /// Constructor that takes the given connection and the thread buffer capacity.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="threadBufferCapacity"></param>
        public GApplicationBuffered(GConnection connection, int threadBufferCapacity)
            : this(threadBufferCapacity)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            Connection = connection;
        }

        /// <summary>
        /// Constructor that takes the thread buffer capacity.
        /// </summary>
        /// <param name="threadBufferCapacity">thread buffer capacity</param>
        public GApplicationBuffered(int threadBufferCapacity)
            : base(true)
        {
            if (threadBufferCapacity < 1)
            {
                throw new ArgumentOutOfRangeException("threadBufferCapacity", threadBufferCapacity, "0 < threadBufferCapacity <= Int32.MaxValue");
            }

            _ThreadBufferCapacity = threadBufferCapacity;
            _ThreadBuffer = CreateThreadBuffer();
        } 
        #endregion


        #region Property - ThreadBufferCapacity
        private int _ThreadBufferCapacity;
        /// <summary>
        /// ThreadBufferCapacity property represents the thread buffer capacity. 
        /// </summary>
        public int ThreadBufferCapacity
        {
            get { return _ThreadBufferCapacity; }
        } 
        #endregion


        /// <summary>
        /// Starts the given thread indirectly by adding it to the thread buffer. When the thread buffer count reaches the thread buffer capacity then that thread buffer is sent to the manager as one thread.
        /// </summary>
        /// <param name="thread">thread</param>
        public override void StartThread(GThread thread)
        {
            lock (this)
            {
                // assign an internal thread id...
                thread.SetId(_InternalThreadId++);

                // add thread to thread buffer...
                _ThreadBuffer.Add(thread);
            }
        }

        /// <summary>
        /// Handles the thread buffer full event. It flushes the thread buffer.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event args</param>
        private void ThreadBuffer_Full(object sender, EventArgs e)
        {
            FlushThreads();
        }

        /// <summary>
        /// Flushes the thread buffer by sending the thread buffer to the manager as one thread.
        /// </summary>
        public void FlushThreads()
        {
            lock (this)
            {
                // check whether any threads to flush...
                if (_ThreadBuffer.Count > 0)
                {
                    // remove full event handler from thread buffer...
                    _ThreadBuffer.Full -= new FullEventHandler(ThreadBuffer_Full);

                    // start thread buffer thread...
                    base.StartThread(_ThreadBuffer);

                    // create new thread buffer...
                    _ThreadBuffer = CreateThreadBuffer();
                }
            }
        }

        /// <summary>
        /// Creates a thread buffer.
        /// </summary>
        private GThreadBuffer CreateThreadBuffer()
        {
            GThreadBuffer oThreadBuffer = new GThreadBuffer(_ThreadBufferCapacity);
            oThreadBuffer.Full += new FullEventHandler(ThreadBuffer_Full);
            return oThreadBuffer;
        }

        /// <summary>
        /// Fires the thread finish event.
        /// </summary>
        /// <param name="thread">thread</param>
        protected override void OnThreadFinish(GThread thread)
        {
            GThreadBuffer oThreadBuffer = thread as GThreadBuffer;
            if (oThreadBuffer != null)
            {
                foreach (GThread oThread1 in oThreadBuffer)
                {
                    Exception oException = oThreadBuffer.GetException(oThread1.Id);
                    if (oException == null)
                    {
                        base.OnThreadFinish(oThread1);
                    }
                    else
                    {
                        base.OnThreadFailed(oThread1, oException);
                    }
                }
            }
        }

        /// <summary>
        /// Fires the thread failed event.
        /// </summary>
        /// <param name="thread">thread</param>
        /// <param name="ex">exception</param>
        protected override void OnThreadFailed(GThread thread, Exception ex)
        {
            GThreadBuffer oThreadBuffer = thread as GThreadBuffer;
            if (oThreadBuffer != null)
            {
                foreach (GThread oThread1 in oThreadBuffer)
                {
                    base.OnThreadFailed(oThread1, ex);
                }
            }
        }
    }
}