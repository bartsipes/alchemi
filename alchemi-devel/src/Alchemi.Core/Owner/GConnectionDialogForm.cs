#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   GConnectionDialogForm.cs
 * Project      :   Alchemi.Core.Owner
 * Created on   :   2003
 * Copyright    :   Copyright © 2006 The University of Melbourne
 *                  This technology has been developed with the support of 
 *                  the Australian Research Council and the University of Melbourne
 *                  research grants as part of the Gridbus Project
 *                  within GRIDS Laboratory at the University of Melbourne, Australia.
 * Author       :   Akshay Luther (akshayl@csse.unimelb.edu.au)
 *                  Rajkumar Buyya (raj@csse.unimelb.edu.au)
 *                  Krishna Nadiminti (kna@csse.unimelb.edu.au)
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Alchemi.Core.Utility;

namespace Alchemi.Core.Owner
{
    public partial class GConnectionDialogForm : Form
    {        
        private GConnectionDialogFormConfig config;

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="GConnectionDialogForm"/> class.
        /// </summary>
        public GConnectionDialogForm()
        {
            InitializeComponent();
        } 
        #endregion


        #region Property - Connection
        private GConnection connection;
        /// <summary>
        /// Gets the GConnection object
        /// </summary>
        public GConnection Connection
        {
            get
            {
                return connection;
            }
        } 
        #endregion

        #region Property - Role
        private AlchemiRole _role = AlchemiRole.Owner;
        /// <summary>
        /// Gets or sets the role of the component that is connecting to manager.
        /// </summary>
        public AlchemiRole Role
        {
            get
            {
                return _role;
            }

            set
            {
                _role = value;
            }
        }
        #endregion


        private void GConnectionDialogForm_Load(object sender, EventArgs e)
        {
            ReadConfig(); 
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            //int port;
            //try
            //{
            //    port = int.Parse(txtPort.Text);
            //}
            //catch (System.FormatException)
            //{
            //    MessageBox.Show("Invalid name for 'Port' field.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            connection = new GConnection();
            //config.Host = connection.Host = txtHost.Text;
            //config.Port = connection.Port = port;
            config.Username = connection.Username = txtUsername.Text;
            config.Password = connection.Password = txtPassword.Text;
            ucEndPointConfig.WriteEndPointConfiguration(config.EndPointConfig);
            connection.RemoteEP = config.EndPointConfig.GetEndPoint();
            config.Write();

            IManager mgr;
            EndPointReference mgrEpr = null;

            try
            {
                mgrEpr = GNode.GetRemoteManagerRef(connection.RemoteEP);
                mgr = (IManager)mgrEpr.Instance;
            }
            catch (RemotingException)
            {
                MessageBox.Show("Could not connect to grid at " + connection.Host + ":" + connection.Port + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                mgr.AuthenticateUser(new SecurityCredentials(config.Username, config.Password));
            }
            catch (AuthenticationException)
            {
                MessageBox.Show("Access denied.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (mgrEpr != null)
            {
                mgrEpr.Dispose();
                mgrEpr = null;
            }

            DialogResult = DialogResult.OK;
        }


        public void ReadConfig()
        {
            config = GConnectionDialogFormConfig.Read(GConnectionDialogFormConfig.Default_Config_File, Role);
            //txtHost.Text = config.Host;
            //txtPort.Text = config.Port.ToString();
            txtUsername.Text = config.Username;
            txtPassword.Text = config.Password;
            ucEndPointConfig.ReadEndPointConfiguration(config.EndPointConfig);
        }
    }
}