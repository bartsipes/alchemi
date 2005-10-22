//
// Alchemi.Core.Manager.Storage.ApplicationStorageView.cs
//
// Author:
//   Tibor Biro (tb@tbiro.com)
//
// Copyright (C) 2005 Tibor Biro (tb@tbiro.com)
//
// This program is free software; you can redistribute it and/or modify 
// it under the terms of the GNU General Public License as published by 
// the Free Software Foundation; either version 2 of the License, or 
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of 
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the 
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License 
// along with this program; if not, write to the Free Software 
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

using System;

namespace Alchemi.Core.Manager.Storage
{
	/// <summary>
	/// Summary description for ApplicationStorageView.
	/// </summary>
	public class ApplicationStorageView
	{
		private const Int32 c_valueNotSet = Int32.MaxValue;

		#region "Private variables"
		
		private String m_applicationId;
		private Int32 m_state;
		private DateTime m_timeCreated;
		private bool m_primary;
		private String m_username;

		// these values are set by calculating the number of threads in various states
		private Int32 m_totalThreads = c_valueNotSet;
		private Int32 m_unfinishedThreads = c_valueNotSet;

		#endregion

		#region "Properties"
		public String ApplicationId
		{
			get
			{
				return m_applicationId;
			}
			set
			{
				m_applicationId = value;
			}
		}

		public Int32 State
		{
			get
			{
				return m_state;
			}
		}

		public DateTime TimeCreated
		{
			get
			{
				return m_timeCreated;
			}
		}

		public bool Primary
		{
			get
			{
				return m_primary;
			}
		}

		public String Username
		{
			get
			{
				return m_username;
			}
		}

		public Int32 TotalThreads
		{
			get
			{
				if (m_totalThreads == c_valueNotSet)
				{
					throw new Exception("The total thread value is not set for this application object.");
				}

				return m_totalThreads;
			}
			set
			{
				m_totalThreads = value;
			}
		}

		public Int32 UnfinishedThreads
		{
			get
			{
				if (m_unfinishedThreads == c_valueNotSet)
				{
					throw new Exception("The unfinished thread value is not set for this application object.");
				}

				return m_unfinishedThreads;
			}
			set
			{
				m_unfinishedThreads = value;
			}
		}

		#endregion

		public ApplicationStorageView(
				Int32 state,
				DateTime timeCreated,
				bool primary,
				String username
			) : this (
				null,
				state,
				timeCreated,
				primary,
				username
			)
		{
		
		}

		public ApplicationStorageView(
				String applicationId,
				Int32 state,
				DateTime timeCreated,
				bool primary,
				String username
		)
		{
			m_applicationId = applicationId;
			m_state = state;
			m_timeCreated = timeCreated;
			m_primary = primary;
			m_username = username;
		}

		// initialize an application with only a username supplied
		public ApplicationStorageView(
				String username
			) : this (
				null,
				0,
				DateTime.Now,
				true,
				username
			)
		{
		
		}
	}
}
