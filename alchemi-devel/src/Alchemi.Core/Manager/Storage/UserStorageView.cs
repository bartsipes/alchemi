#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   UserStorageView.cs
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

using Alchemi.Core.Utility;

namespace Alchemi.Core.Manager.Storage
{
	/// <summary>
	/// Storage view of a user object. 
	/// Used to pass user related data to and from the storage layer.
	/// </summary>
	[Serializable]
	public class UserStorageView
	{

        #region Property - IsSystem
        private bool _isSystem;
        /// <summary>
        /// Gets or sets a name indicating whether this user is a system user.
        /// </summary>
        public bool IsSystem
        {
            get { return _isSystem; }
            set { _isSystem = value; }
        } 
        #endregion


        #region Property - Username
        private string _username;
        /// <summary>
        /// The username.
        /// </summary>
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        } 
        #endregion


        #region Property - Password
        private string _password = null;
        /// <summary>
        /// The password. This name is never stored in the database.
        /// This is used to calculate the MD5 hash stored in the database. 
        /// <seealso cref="PasswordMd5Hash"/>
        /// </summary>
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;

                // clean the password hash once the clear-text password was set
                _passwordMd5Hash = null;
            }
        } 
        #endregion


        #region Property - PasswordMd5Hash
        private string _passwordMd5Hash = null;
        /// <summary>
        /// The password's MD5 hash. When validating the user's password only this hash is required.
        /// <seealso cref="Password"/>
        /// </summary>
        public string PasswordMd5Hash
        {
            get
            {
                if (_passwordMd5Hash != null)
                {
                    return _passwordMd5Hash;
                }
                else
                {
                    return HashUtil.GetHash(_password, HashType.MD5);
                }
            }
            set
            {
                _passwordMd5Hash = value;

                // remove the clear-text password
                _password = null;
            }
        } 
        #endregion


        #region Property - GroupId
        private int _groupId;
        /// <summary>
        /// The group id this user belongs to.
        /// </summary>
        public int GroupId
        {
            get { return _groupId; }
            set { _groupId = value; }
        } 
        #endregion



        #region Constructors
        /// <summary>
        /// UserStorageView constructor.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="groupId"></param>
        public UserStorageView(string username, string password, int groupId)
        {
            _username = username;
            _password = password;
            _groupId = groupId;
        }

        /// <summary>
        /// UserStorageView constructor.
        /// </summary>
        /// <param name="username"></param>
        public UserStorageView(string username)
            : this(username, "", -1)
        {
        } 
        #endregion

	}
}
