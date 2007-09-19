#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   GroupStorageView.cs
 * Project      :   Alchemi.Core.Manager.Storage
 * Created on   :   22 September 2005
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

namespace Alchemi.Core.Manager.Storage
{
	/// <summary>
	/// Storage view of a group object. 
	/// Used to pass group related data to and from the storage layer.
	/// </summary>
	[Serializable]
	public class GroupStorageView
	{

        #region Property - IsSystem
        private bool _isSystem;
        /// <summary>
        /// Gets or sets a name indicating whether this Group is a system group.
        /// </summary>
        public bool IsSystem
        {
            get { return _isSystem; }
            set { _isSystem = value; }
        } 
        #endregion


        #region Property - GroupName
        private string _groupName;
        /// <summary>
        /// The group name.
        /// </summary>
        public string GroupName
        {
            get { return _groupName; }
        } 
        #endregion


        #region Property - GroupId
        private int _groupId;
        /// <summary>
        /// The group Id.
        /// </summary>
        public int GroupId
        {
            get { return _groupId; }
        } 
        #endregion


        #region Property - Description
        private string _description;
        /// <summary>
        /// A human readable description for this group.
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        } 
        #endregion


        #region Constructors
        /// <summary>
        /// GroupStorageView constructor.
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="groupName"></param>
        public GroupStorageView(int groupId, string groupName)
        {
            _groupId = groupId;
            _groupName = groupName;
        }

        /// <summary>
        /// GroupStorageView constructor.
        /// Initializes an empty object.
        /// </summary>
        /// <param name="groupId"></param>
        public GroupStorageView(int groupId)
            : this(groupId, null)
        {
        } 
        #endregion
	}
}
