//
// Alchemi.Core.Manager.Storage.User.cs
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
	/// Storage view of a user object. 
	/// Used to pass user related data to and from the storage layer 
	/// </summary>
	public class User
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

		public User(String username, String password, Int32 groupId)
		{
			m_username = username;
			m_password = password;
			m_groupId = groupId; 
		}
	}
}
