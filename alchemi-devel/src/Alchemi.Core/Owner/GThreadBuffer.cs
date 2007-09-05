#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   GThreadBuffer.cs
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
using System.Collections;
using System.Runtime.Serialization;

// 2.8.06 MDV
// TODO: Can m_cThreads be moved from an ArrayList to a List<>?
// TODO: Get rid of the Hungarian notation?

namespace Alchemi.Core.Owner
{
    /// <summary>
    /// FullEventHandler delegate represents the Full event handler.
    /// </summary>
    public delegate void FullEventHandler(object sender, EventArgs args);


	/// <summary>
	/// GThreadBuffer class represents a thread buffer that holds many threads that can be
    /// executed by an executor as one thread. It is used primarily by GApplicationBuffered
    /// to improve performance when executing many threads with short execution times.
	/// </summary>
	[Serializable]
	public class GThreadBuffer : GThread, ICollection
	{
		private const int DefaultCapacity = 8;

		private IList m_cThreads = new ArrayList();
		private Hashtable m_cThreadIdException = new Hashtable();


        #region Event - Full
        private event FullEventHandler _Full;
        public event FullEventHandler Full
        {
            add { _Full += value; }
            remove { _Full -= value; }
        }

        /// <summary>
        /// Fires the full event.
        /// </summary>
        protected virtual void OnFull()
        {
            if (_Full != null)
            {
                _Full(this, new EventArgs());
            }
        }
        #endregion



        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public GThreadBuffer()
            : base()
        {
        }

        /// <summary>
        /// Constructor that takes the capacity.
        /// </summary>
        /// <param name="nCapacity">capacity</param>
        public GThreadBuffer(int nCapacity)
            : base()
        {
            if (nCapacity < 1)
            {
                throw new ArgumentOutOfRangeException("nCapacity", nCapacity, "0 < nCapacity <= Int32.MaxValue");
            }
            m_nCapacity = nCapacity;
        } 
        #endregion



        #region Property - Capacity
        private int m_nCapacity = DefaultCapacity;
        /// <summary>
        /// Capacity property represents the maximum the number of threads that can be held in the buffer.
        /// </summary>
        public int Capacity
        {
            get { return m_nCapacity; }
        } 
        #endregion


        #region Property - Count
        /// <summary>
        /// Count property represents the number of threads in the buffer.
        /// </summary>
        public int Count
        {
            get { return m_cThreads.Count; }
        } 
        #endregion


        #region Property - IsFull
        /// <summary>
        /// Determines whether the thread buffer is full.
        /// </summary>
        /// <value><c>true</c> if the GThreadBuffer is full; otherwise, <c>false</c>.</value>
        /// <returns>whether it is full</returns>
        public bool IsFull
        {
            get { return (m_cThreads.Count == m_nCapacity); }
        } 
        #endregion


		/// <summary>
		/// Adds a thread to the buffer.
		/// </summary>
		/// <param name="thread">thread</param>
		public void Add(GThread oThread) 
		{
			if (this.IsFull) 
			{
				throw new ThreadBufferFullException("Attempting to add a thread to a full thread buffer.");
			}
			
			m_cThreads.Add(oThread);

			if (this.IsFull) 
			{
				OnFull();
			}
		}


		/// <summary>
		/// Starts the thread buffer by starting each thread in the buffer.
		/// </summary>
		public override void Start()
		{
			foreach (GThread oThread in m_cThreads) 
			{
				try
				{
					oThread.StartThread();
				}
				catch (Exception oException) 
				{
					m_cThreadIdException[oThread.Id] = oException;
				}
			}
		}


		/// <summary>
		/// Gets the thread exception for the given thread id.
		/// </summary>
		/// <param name="nThreadId">thread id</param>
		/// <returns>thread exception</returns>
		public Exception GetException(int nThreadId) 
		{
			return (Exception) m_cThreadIdException[nThreadId];
		}


		#region ICollection Members

		/// <summary>
		/// Determines whether the threads are synchronized.
		/// </summary>
		public bool IsSynchronized
		{
			get
			{
				return m_cThreads.IsSynchronized;
			}
		}

		/// <summary>
		/// Copies the threads to the given array.
		/// </summary>
		/// <param name="oArray">array</param>
		/// <param name="nIndex">index</param>
		public void CopyTo(Array oArray, int nIndex)
		{
			m_cThreads.CopyTo(oArray, nIndex);
		}

		/// <summary>
		/// Gets the threads sync root.
		/// </summary>
		public object SyncRoot
		{
			get
			{
				return m_cThreads.SyncRoot;
			}
		}

		#endregion


		#region IEnumerable Members

		/// <summary>
		/// Gets the threads enumerator.
		/// </summary>
		/// <returns>enumerator</returns>
		public IEnumerator GetEnumerator()
		{
			return m_cThreads.GetEnumerator();
		}

		#endregion
	}


}