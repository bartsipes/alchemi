#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   PermissionStorageView.cs
 * Project      :   Alchemi.Core.Manager.Storage
 * Created on   :   21 September 2005
 * Copyright    :   Copyright © 2006 The University of Melbourne
 *                  This technology has been developed with the support of 
 *                  the Australian Research Council and the University of Melbourne
 *                  research grants as part of the Gridbus Project
 *                  within GRIDS Laboratory at the University of Melbourne, Australia.
 * Author       :   Tibor Biro (tb@tbiro.com)
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

namespace Alchemi.Core.Manager.Storage
{
	/// <summary>
	/// Storage view of a Permission object. 
	/// Used to pass permission related data to and from the storage layer.
	/// </summary>
	[Serializable]
	public class PermissionStorageView
	{

        #region Property - PermissionName
        private string m_prmname;
        /// <summary>
        /// A human readable permission name.
        /// </summary>
        public string PermissionName
        {
            get { return m_prmname; }
            set { m_prmname = value; }
        } 
        #endregion


        #region Property - PermissionId
        private int m_prmId;
        /// <summary>
        /// Permission Id.
        /// </summary>
        public int PermissionId
        {
            get { return m_prmId; }
            set { m_prmId = value; }
        } 
        #endregion



        #region Constructors
        /// <summary>
        /// PermissionStorageView constructor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public PermissionStorageView(int id, string name)
        {
            m_prmId = id;
            m_prmname = name;
        }

        /// <summary>
        /// PermissionStorageView constructor.
        /// </summary>
        /// <param name="perm"></param>
        public PermissionStorageView(Permission perm)
        {
            m_prmId = (int)perm;
            m_prmname = perm.ToString();
        } 
        #endregion



		/// <summary>
		/// Convert a Permission array into a PermissionStorageView array.
		/// <seealso cref="Permission"/>
		/// </summary>
		/// <param name="permissions">The Permission array to be converted</param>
		/// <returns>A new array of PermissionStorageView values.</returns>
		public static PermissionStorageView[] GetPermissionStorageView(Permission[] permissions)
		{
			ArrayList result = new ArrayList();

			foreach(Permission permission in permissions)
			{
				PermissionStorageView storageView = new PermissionStorageView(permission);
				result.Add(storageView);
			}

			return (PermissionStorageView[])result.ToArray(typeof(PermissionStorageView));			
		}
	}
}
