#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	DBInstaller.cs
* Project		:	Alchemi DB Installer
* Created on	:	2003
* Copyright		:	Copyright � 2005 The University of Melbourne
*					This technology has been developed with the support of 
*					the Australian Research Council and the University of Melbourne
*					research grants as part of the Gridbus Project
*					within GRIDS Laboratory at the University of Melbourne, Australia.
* Author         :  Akshay Luther (akshayl@cs.mu.oz.au), Rajkumar Buyya (raj@cs.mu.oz.au) and Krishna Nadiminti (kna@cs.mu.oz.au)
* License        :  GPL
*					This program is free software; you can redistribute it and/or 
*					modify it under the terms of the GNU General Public
*					License as published by the Free Software Foundation;
*					See the GNU General Public License 
*					(http://www.gnu.org/copyleft/gpl.html) for more details.
*
*/ 
#endregion

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using Alchemi.Core.Manager;

namespace Alchemi.ManagerUtils.DbInstaller
{
    public class DbInstaller : Form
    {
        private Label label1;
        private Label label2;
        private GroupBox groupBox1;
        private TextBox txServer;
        private TextBox txAdminPwd;
        private Button btInstall;
        private Button btFinish;
        private Label label3;
        private TextBox txUsername;
        private GroupBox groupBox2;
        private TextBox txLog;
        private Label label4;
        private PictureBox pictureBox1;
        private Button btSkip;
        private Container components = null;
        private string InstallLocation = "";

        public DbInstaller(string installLocation)
        {
            InitializeComponent();

            btFinish.Enabled = false;
            txServer.Text = Dns.GetHostName();
            if (installLocation == "")
            {
                InstallLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";
            }
            else
            {
				//remove invalid characters from the path
				char[] invalidchars = Path.InvalidPathChars;
				foreach (char c in invalidchars)
				{
					int index = installLocation.IndexOf(c);
					if (index>=0)
					{
						installLocation = installLocation.Remove(index,1);
					}
				}
                InstallLocation = installLocation;
            }
        }

        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if (components != null) 
                {
                    components.Dispose();
                }
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DbInstaller));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txUsername = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txServer = new System.Windows.Forms.TextBox();
			this.txAdminPwd = new System.Windows.Forms.TextBox();
			this.btInstall = new System.Windows.Forms.Button();
			this.btFinish = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.txLog = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.btSkip = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Database Server";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.ImageAlign = System.Drawing.ContentAlignment.TopRight;
			this.label2.Location = new System.Drawing.Point(232, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Password";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.txUsername);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.txServer);
			this.groupBox1.Controls.Add(this.txAdminPwd);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(8, 80);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(488, 80);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Specify Database Server";
			// 
			// txUsername
			// 
			this.txUsername.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txUsername.Location = new System.Drawing.Point(136, 40);
			this.txUsername.Name = "txUsername";
			this.txUsername.Size = new System.Drawing.Size(80, 21);
			this.txUsername.TabIndex = 6;
			this.txUsername.TabStop = false;
			this.txUsername.Text = "sa";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(136, 24);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 16);
			this.label3.TabIndex = 4;
			this.label3.Text = "Username";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txServer
			// 
			this.txServer.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txServer.Location = new System.Drawing.Point(16, 40);
			this.txServer.Name = "txServer";
			this.txServer.TabIndex = 2;
			this.txServer.Text = "";
			// 
			// txAdminPwd
			// 
			this.txAdminPwd.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txAdminPwd.Location = new System.Drawing.Point(232, 40);
			this.txAdminPwd.Name = "txAdminPwd";
			this.txAdminPwd.PasswordChar = '*';
			this.txAdminPwd.TabIndex = 3;
			this.txAdminPwd.Text = "";
			// 
			// btInstall
			// 
			this.btInstall.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btInstall.Location = new System.Drawing.Point(256, 352);
			this.btInstall.Name = "btInstall";
			this.btInstall.Size = new System.Drawing.Size(112, 23);
			this.btInstall.TabIndex = 0;
			this.btInstall.Text = "Install";
			this.btInstall.Click += new System.EventHandler(this.btInstall_Click);
			// 
			// btFinish
			// 
			this.btFinish.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btFinish.Location = new System.Drawing.Point(376, 352);
			this.btFinish.Name = "btFinish";
			this.btFinish.Size = new System.Drawing.Size(112, 23);
			this.btFinish.TabIndex = 1;
			this.btFinish.Text = "Next";
			this.btFinish.Click += new System.EventHandler(this.btFinish_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.txLog);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(8, 168);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(488, 168);
			this.groupBox2.TabIndex = 6;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Activity Log";
			// 
			// txLog
			// 
			this.txLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txLog.Location = new System.Drawing.Point(8, 16);
			this.txLog.Multiline = true;
			this.txLog.Name = "txLog";
			this.txLog.ReadOnly = true;
			this.txLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txLog.Size = new System.Drawing.Size(472, 144);
			this.txLog.TabIndex = 4;
			this.txLog.TabStop = false;
			this.txLog.Text = "";
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.White;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(8, 8);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(392, 23);
			this.label4.TabIndex = 7;
			this.label4.Text = "Install Database";
			// 
			// pictureBox1
			// 
			this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(500, 70);
			this.pictureBox1.TabIndex = 8;
			this.pictureBox1.TabStop = false;
			// 
			// btSkip
			// 
			this.btSkip.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btSkip.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btSkip.Location = new System.Drawing.Point(8, 352);
			this.btSkip.Name = "btSkip";
			this.btSkip.Size = new System.Drawing.Size(112, 23);
			this.btSkip.TabIndex = 9;
			this.btSkip.Text = "Skip";
			this.btSkip.Click += new System.EventHandler(this.btSkip_Click);
			// 
			// DbInstaller
			// 
			this.AcceptButton = this.btInstall;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btSkip;
			this.ClientSize = new System.Drawing.Size(500, 387);
			this.Controls.Add(this.btSkip);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btInstall);
			this.Controls.Add(this.btFinish);
			this.Controls.Add(this.pictureBox1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DbInstaller";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Alchemi Database Installer";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
        #endregion

        [STAThread]
        static void Main(string[] args) 
        {
            string installLocation = "";
            if (args.Length >= 1)
            {
                installLocation = args[0];  
            }
            Application.EnableVisualStyles();
            Application.Run(new DbInstaller(installLocation));
        }

        private void btInstall_Click(object sender, EventArgs e)
        {
        	Process process = new Process();
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = "osql";
            string outputText = "";

            // (re)create database
            txLog.AppendText("[ Creating Database ] ... ");
            try
            {
				string scriptPath = Path.Combine(InstallLocation,"Alchemi_database.sql");
                process.StartInfo.Arguments = string.Format("-S {0} -U {1} -P {2} -i \"{3}\" -n", txUsername.Text, txServer.Text, txAdminPwd.Text, scriptPath);
				process.Start();
                process.WaitForExit();
                outputText = process.StandardOutput.ReadToEnd();
            }
            catch (Exception ex)
            {
                txLog.AppendText("[ Error ]" + Environment.NewLine);
                if (ex.Message == "The system cannot find the file specified")
                {
                    Log("'osql' could not be found. Check that SQL Server 2000 or MSDE is installed and 'osql' is in the path..");
                }
                else
                {
                    Log(ex.Message);
                }
                return;
            }
      
            if (process.ExitCode == 0)
            {
                txLog.AppendText("[ Done ]" + Environment.NewLine);
                Log(outputText);
            }
            else
            {
                txLog.AppendText("[ Error ]" + Environment.NewLine);
                Log(outputText);
                return;
            }

            // create structure
            txLog.AppendText("[ Creating Database Structure ] ... ");
            try
            {
				string scriptPath = Path.Combine(InstallLocation,"Alchemi_structure.sql");
                process.StartInfo.Arguments = string.Format("-S {0} -U {1} -P {2} -d Alchemi -i \"{3}\" -n", txUsername.Text, txServer.Text, txAdminPwd.Text, scriptPath);
                process.Start();
                process.WaitForExit();
                outputText = process.StandardOutput.ReadToEnd();
            }
            catch (Exception ex)
            {
                txLog.AppendText("[ Error ]" + Environment.NewLine);
                Log(ex.Message);
                return;
            }
      
            if (process.ExitCode == 0)
            {
                txLog.AppendText("[ Done ]" + Environment.NewLine);
                Log(outputText);
            }
            else
            {
                txLog.AppendText("[ Error ]" + Environment.NewLine);
                Log(outputText);
                return;
            }

            // insert data
            txLog.AppendText("[ Inserting Default Data ] ... ");
            try
            {
				string scriptPath = Path.Combine(InstallLocation,"Alchemi_data.sql");
                process.StartInfo.Arguments = string.Format("-S {0} -U {1} -P {2} -d Alchemi -i \"{3}\" -n", txUsername.Text, txServer.Text, txAdminPwd.Text, scriptPath);
                process.Start();
                process.WaitForExit();
                outputText = process.StandardOutput.ReadToEnd();
            }
            catch (Exception ex)
            {
                txLog.AppendText("[ Error ]" + Environment.NewLine);
                Log(ex.Message);
                return;
            }
      
            if (process.ExitCode == 0)
            {
                txLog.AppendText("[ Done ]" + Environment.NewLine);
                Log(outputText);
            }
            else
            {
                txLog.AppendText("[ Error ]" + Environment.NewLine);
                Log(outputText);
                return;
            }


            // serialize configuration
            txLog.AppendText("[ Creating Configuration File ] ... ");
            try
            {
				//get the manager install location. it is ../v.v.v
				//string mgrInstallDir = Path.Combine(Directory.GetParent(InstallLocation).FullName,Alchemi.Core.Utility.Utils.AssemblyVersion);
				txLog.AppendText(" Writing Configuration to " + InstallLocation + Environment.NewLine);
            	Configuration config = new Configuration(InstallLocation);
                config.DbServer = txServer.Text;
                config.DbUsername = txUsername.Text;
                config.DbPassword = txAdminPwd.Text;
                config.Slz();
                txLog.AppendText("[ Done ]" + Environment.NewLine);
                Log("wrote configuration file to " + InstallLocation);
            }
            catch (Exception ex)
            {
                txLog.AppendText("[ Error ]" + Environment.NewLine);
                Log(ex.ToString());
            }

            Log("");
            Log("[ Installation Complete! ]");

            btInstall.Enabled = false;
            btFinish.Enabled = true;
        }

        private void Log(string s)
        {
            txLog.AppendText(s + Environment.NewLine);
        }

        private void btFinish_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btSkip_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
