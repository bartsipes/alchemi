#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
* Title         :  UserStorageView.cs
* Project       :  Alchemi.Core.Manager.Storage
* Created on    :  21 September 2005
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
	/// Storage view of a user object. 
	/// Used to pass user related data to and from the storage layer 
	/// </summary>
	public class UserStorageView
	{
		#region "Private variables"
		
		private String m_username;
		private String m_password;
		private Int32 m_groupId; 

		#endregion

		#region "Properties"
		public String Username
		{
			get
			{
				return m_username;
			}
		}
		
		public String Password
		{
			get
			{
				return m_password;
			}
			set
			{
				m_password = value;
			}
		}

		public Int32 GroupId
		{
			get
			{
				return m_groupId;
			}
			set
			{
				m_groupId = value;
			}
		}

		#endregion

		public UserStorageView(String username, String password, Int32 groupId)
		{
			m_username = username;
			m_password = password;
			m_groupId = groupId; 
		}
	}
}
