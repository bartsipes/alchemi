#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
* Title         :  GroupStorageView.cs
* Project       :  Alchemi.Core.Manager.Storage
* Created on    :  22 September 2005
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
	/// Storage view of a group object. 
	/// Used to pass group related data to and from the storage layer 
	/// </summary>
	[Serializable]
	public class GroupStorageView
	{

		#region "Private variables"
		
		private Int32 m_groupId; 
		private String m_groupName;
		private String m_description; 
		private bool m_is_system;

		#endregion

		#region "Properties"

		public bool IsSystem
		{
			get
			{
				return m_is_system;
			}
			set
			{
				m_is_system = value;
			}
		}

		public String GroupName
		{
			get
			{
				return m_groupName;
			}
		}

		public Int32 GroupId
		{
			get
			{
				return m_groupId;
			}
		}

		public String Description
		{
			get
			{
				return m_description;
			}
			set
			{
				m_description = value;
			}
		}

		#endregion

		public GroupStorageView(Int32 groupId, String groupName)
		{
			m_groupId = groupId;
			m_groupName = groupName;
		}

		public GroupStorageView(Int32 groupId) : this(groupId, null)
		{
		}
	}
}
