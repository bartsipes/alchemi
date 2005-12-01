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
	/// Storage view of a Permission object. 
	/// Used to pass permission related data to and from the storage layer 
	/// </summary>
	[Serializable]
	public class PermissionStorageView
	{
		#region "Private variables"
		
		private String m_prmname;
		private Int32 m_prmId; 

		#endregion

		#region "Properties"

		public String PermissionName
		{
			get
			{
				return m_prmname;
			}
			set
			{
				m_prmname = value;
			}
		}
		
		public Int32 PermissionId
		{
			get
			{
				return m_prmId;
			}
			set
			{
				m_prmId = value;
			}
		}


		#endregion

		public PermissionStorageView(Int32 id, String name)
		{
			m_prmId = id;
			m_prmname = name;
		}
	}
}
