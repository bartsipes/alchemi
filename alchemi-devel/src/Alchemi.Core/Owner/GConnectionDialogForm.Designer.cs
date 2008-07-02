#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   GConnectionDialogForm.Designer.cs
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

namespace Alchemi.Core.Owner
{
    partial class GConnectionDialogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpCredentials = new System.Windows.Forms.GroupBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.grpManager = new System.Windows.Forms.GroupBox();
            this.ucEndPointConfig = new Alchemi.Core.EndPointUtils.EndPointControl();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.grpCredentials.SuspendLayout();
            this.grpManager.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpCredentials
            // 
            this.grpCredentials.Controls.Add( this.lblUsername );
            this.grpCredentials.Controls.Add( this.txtUsername );
            this.grpCredentials.Controls.Add( this.txtPassword );
            this.grpCredentials.Controls.Add( this.lblPassword );
            this.grpCredentials.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpCredentials.Location = new System.Drawing.Point( 8, 205 );
            this.grpCredentials.Name = "grpCredentials";
            this.grpCredentials.Size = new System.Drawing.Size( 232, 64 );
            this.grpCredentials.TabIndex = 15;
            this.grpCredentials.TabStop = false;
            this.grpCredentials.Text = "Credentials";
            // 
            // lblUsername
            // 
            this.lblUsername.Location = new System.Drawing.Point( 8, 16 );
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size( 56, 16 );
            this.lblUsername.TabIndex = 6;
            this.lblUsername.Text = "Username";
            this.lblUsername.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point( 8, 32 );
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size( 104, 20 );
            this.txtUsername.TabIndex = 2;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point( 120, 32 );
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size( 104, 20 );
            this.txtPassword.TabIndex = 3;
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point( 120, 16 );
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size( 72, 16 );
            this.lblPassword.TabIndex = 7;
            this.lblPassword.Text = "Password";
            // 
            // grpManager
            // 
            this.grpManager.Controls.Add( this.ucEndPointConfig );
            this.grpManager.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpManager.Location = new System.Drawing.Point( 8, 8 );
            this.grpManager.Name = "grpManager";
            this.grpManager.Size = new System.Drawing.Size( 232, 191 );
            this.grpManager.TabIndex = 14;
            this.grpManager.TabStop = false;
            this.grpManager.Text = "Manager";
            // 
            // ucEndPointConfig
            // 
            this.ucEndPointConfig.AddressPart = "";
            this.ucEndPointConfig.BindingConfigurationName = "";
            this.ucEndPointConfig.BindingSettingType = Alchemi.Core.WCFBindingSettingType.Default;
            this.ucEndPointConfig.FixedAddressPart = false;
            this.ucEndPointConfig.FullAddress = "http://localhost:0/";
            this.ucEndPointConfig.Host = "localhost";
            this.ucEndPointConfig.HostNameForPublishing = "localhost";
            this.ucEndPointConfig.Location = new System.Drawing.Point( 6, 19 );
            this.ucEndPointConfig.Name = "ucEndPointConfig";
            this.ucEndPointConfig.Port = 0;
            this.ucEndPointConfig.Protocol = "";
            this.ucEndPointConfig.SelectedRemotingMechanism = Alchemi.Core.RemotingMechanism.WCF;
            this.ucEndPointConfig.ServiceConfigurationName = "";
            this.ucEndPointConfig.Size = new System.Drawing.Size( 220, 166 );
            this.ucEndPointConfig.TabIndex = 0;
            this.ucEndPointConfig.WCFBinding = Alchemi.Core.WCFBinding.None;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point( 152, 275 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 88, 23 );
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            // 
            // btnOK
            // 
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point( 56, 275 );
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size( 88, 23 );
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler( this.btnOK_Click );
            // 
            // GConnectionDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 250, 311 );
            this.Controls.Add( this.grpCredentials );
            this.Controls.Add( this.grpManager );
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.btnOK );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GConnectionDialogForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Alchemi Grid Connection";
            this.Load += new System.EventHandler( this.GConnectionDialogForm_Load );
            this.grpCredentials.ResumeLayout( false );
            this.grpCredentials.PerformLayout();
            this.grpManager.ResumeLayout( false );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.GroupBox grpCredentials;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.GroupBox grpManager;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private Alchemi.Core.EndPointUtils.EndPointControl ucEndPointConfig;
    }
}
