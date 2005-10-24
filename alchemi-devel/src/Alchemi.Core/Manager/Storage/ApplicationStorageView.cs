#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
* Title         :  ApplicationStorageView.cs
* Project       :  Alchemi.Core.Manager.Storage
* Created on    :  19 October 2005
* Copyright     :  Copyright © 2005 The University of Melbourne
*                    This technology has been developed with the support of
*                    the Australian Research Council and the University of Melbourne
*                    research grants as part of the Gridbus Project
*                    within GRIDS Laboratory at the University of Melbourne, Australia.
* Author        :  Tibor Biro (tb@tbiro.com)
* License       :  GPL
*                    This program is free software; you can redistribute it and/or
*                    modify it under the terms of the GNU General Public
*                    License as published by the Free Software Foundation;
*                    See the GNU General Public License
*                    (http://www.gnu.org/copyleft/gpl.html) for more 
details.
*
*/
#endregion

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
