#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   SecurityCredentials.cs
 * Project      :   Alchemi.Core
 * Created on   :   1 Aug 2003
 * Copyright    :   Copyright © 2006 The University of Melbourne
 *                  This technology has been developed with the support of 
 *                  the Australian Research Council and the University of Melbourne
 *                  research grants as part of the Gridbus Project
 *                  within GRIDS Laboratory at the University of Melbourne, Australia.
 * Author       :   Akshay Luther (akshayl@csse.unimelb.edu.au)
 *                  Rajkumar Buyya (raj@csse.unimelb.edu.au)              
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

namespace Alchemi.Core
{
	/// <summary>
	/// Represents the credentials required to authenticate to a node
	/// </summary>
	[Serializable]
    public class SecurityCredentials
	{
        #region Property - Username
        private string _Username;
        /// <summary>
        /// Username
        /// </summary>        
        public string Username
        {
            get { return _Username; }
        } 
        #endregion


        #region Property - Password
        private string _Password;
        /// <summary>
        /// Password
        /// </summary>        
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        } 
        #endregion


        #region Constructor
        /// <summary>
        /// Creates an instance of the SecurityCredentials class
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public SecurityCredentials(string username, string password)
        {
            _Username = username;
            _Password = password;
        } 
        #endregion
	}
}
