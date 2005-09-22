#region Alchemi copyright notice
/*
  Alchemi [.NET Grid Computing Framework]
  http://www.alchemi.net
  
  Copyright (c)  Akshay Luther (2002-2004) & Rajkumar Buyya (2003-to-date), 
  GRIDS Lab, The University of Melbourne, Australia.
  
  Maintained and Updated by: Krishna Nadiminti (2005-to-date)
---------------------------------------------------------------------------

  This program is free software; you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation; either version 2 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program; if not, write to the Free Software
  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/
#endregion

using System;
using System.ComponentModel;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using Alchemi.Core;
using Alchemi.Core.Executor;
using Alchemi.Updater;
using log4net;
using Timer = System.Windows.Forms.Timer;

// Configure log4net using the .config file
[assembly: log4net.Config.XmlConfigurator(Watch=true)]

namespace Alchemi.ExecutorExec
{
    public class ExecutorMainForm : Form
    {
        private TextBox txLog;
        private IContainer components;
        private MainMenu MainMenu;
        private MenuItem menuItem1;
        private MenuItem mmExit;
        private NotifyIcon TrayIcon;
        private ContextMenu TrayMenu;
        private MenuItem tmExit;
        private MenuItem menuItem2;
        private MenuItem mmAbout;
        private Timer HideTimer;
        
        private Configuration Config;
		private ExecutorContainer _container = null;

        private GroupBox groupBox4;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private Button btReset;
        private Button btDisconnect;
        private GroupBox groupBox1;
        private Label label2;
        private Label label1;
        private TextBox txMgrPort;
        private TextBox txMgrHost;
        private GroupBox groupBox2;
        private Label label3;
        private TextBox txOwnPort;
        private CheckBox cbDedicated;
        private TextBox txId;
        private Label label4;
        private Button btConnect;
        private TabPage tabPage2;
        private Button btStopExec;
        private Button btStartExec;
        private GroupBox groupBox3;
        private Label label5;
        private Label label6;
        private TextBox txPassword;
        private TextBox txUsername;
		private TabPage tabPage3;
		private NumericUpDown udHBInterval;
		private Label lblHBInt;

		// Create a logger for use in this class
		private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private Label label7;
		private Label label8;
		private CheckBox chkRetryConnect;
		private Label label9;
		private Label label10;
		private NumericUpDown udMaxRetry;
		private Label label11;
		private NumericUpDown udReconnectInterval;
		private ProgressBar pbar;
		private MenuItem mnuUpdates;
		private System.Windows.Forms.StatusBar statusBar;

		private static bool silentMode = true;

		//service stuff
		private bool isService = false;
    	private const string serviceName = "Alchemi Executor Service";

		//updater stuff
		private AppUpdater updater = null;
		private const string updateExecURL = "http://www.alchemi.net/updates/executor/exec/ExecutorExecManifest.xml";
		private const string updateServiceURL = "http://www.alchemi.net/updates/executor/service/ExecutorServiceManifest.xml";
		private bool updating = false;
    	public int returnCode = 0;

    	public ExecutorMainForm(): this(false)
        {
        }

		public ExecutorMainForm(bool isService)
		{
            InitializeComponent();
			this.isService = isService;
			string updateURL;
			if (isService)
			{
				this.Text = "Alchemi Executor Service Controller";
				updateURL = updateServiceURL;
			}
			else
			{
				this.Text = "Alchemi Executor";
				updateURL = updateExecURL;
			}

			// 
			// updater
			// 
			updater = new AppUpdater(updateURL,ChangeDetectionModes.ServerManifestCheck,false,false,false);
			
			//need this only for a custom update detection system
			//updater.OnCheckForUpdate += new AppUpdater.CheckForUpdateEventHandler(this.updater_OnCheckForUpdate);
			updater.OnUpdateDetected += new AppUpdater.UpdateDetectedEventHandler(this.updater_OnUpdateDetected);
			updater.OnUpdateComplete += new AppUpdater.UpdateCompleteEventHandler(this.updater_OnUpdateComplete);
			updater.OnDownloadProgress += new Alchemi.Updater.AppUpdater.DownloadProgressEventHandler(updater_OnDownloadProgress);
		}

        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(components != null)
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ExecutorMainForm));
			this.txLog = new System.Windows.Forms.TextBox();
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.TrayMenu = new System.Windows.Forms.ContextMenu();
			this.tmExit = new System.Windows.Forms.MenuItem();
			this.MainMenu = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.mmExit = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.mnuUpdates = new System.Windows.Forms.MenuItem();
			this.mmAbout = new System.Windows.Forms.MenuItem();
			this.HideTimer = new System.Windows.Forms.Timer(this.components);
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.txPassword = new System.Windows.Forms.TextBox();
			this.txUsername = new System.Windows.Forms.TextBox();
			this.btReset = new System.Windows.Forms.Button();
			this.btDisconnect = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txMgrPort = new System.Windows.Forms.TextBox();
			this.txMgrHost = new System.Windows.Forms.TextBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txOwnPort = new System.Windows.Forms.TextBox();
			this.cbDedicated = new System.Windows.Forms.CheckBox();
			this.txId = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.btConnect = new System.Windows.Forms.Button();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.btStopExec = new System.Windows.Forms.Button();
			this.btStartExec = new System.Windows.Forms.Button();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.label11 = new System.Windows.Forms.Label();
			this.udMaxRetry = new System.Windows.Forms.NumericUpDown();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.chkRetryConnect = new System.Windows.Forms.CheckBox();
			this.udReconnectInterval = new System.Windows.Forms.NumericUpDown();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.lblHBInt = new System.Windows.Forms.Label();
			this.udHBInterval = new System.Windows.Forms.NumericUpDown();
			this.pbar = new System.Windows.Forms.ProgressBar();
			this.groupBox4.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udMaxRetry)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udReconnectInterval)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.udHBInterval)).BeginInit();
			this.SuspendLayout();
			// 
			// txLog
			// 
			this.txLog.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txLog.Location = new System.Drawing.Point(8, 16);
			this.txLog.Multiline = true;
			this.txLog.Name = "txLog";
			this.txLog.ReadOnly = true;
			this.txLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txLog.Size = new System.Drawing.Size(424, 96);
			this.txLog.TabIndex = 1;
			this.txLog.Text = "";
			this.txLog.DoubleClick += new System.EventHandler(this.txLog_DoubleClick);
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 541);
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new System.Drawing.Size(458, 22);
			this.statusBar.TabIndex = 2;
			// 
			// TrayIcon
			// 
			this.TrayIcon.ContextMenu = this.TrayMenu;
			this.TrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("TrayIcon.Icon")));
			this.TrayIcon.Text = "Alchemi Executor";
			this.TrayIcon.Visible = true;
			this.TrayIcon.Click += new System.EventHandler(this.TrayIcon_Click);
			// 
			// TrayMenu
			// 
			this.TrayMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.tmExit});
			// 
			// tmExit
			// 
			this.tmExit.Index = 0;
			this.tmExit.Text = "Exit";
			this.tmExit.Click += new System.EventHandler(this.tmExit_Click);
			// 
			// MainMenu
			// 
			this.MainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.menuItem1,
																					 this.menuItem2});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mmExit});
			this.menuItem1.Text = "Executor";
			// 
			// mmExit
			// 
			this.mmExit.Index = 0;
			this.mmExit.Text = "Exit";
			this.mmExit.Click += new System.EventHandler(this.mmExit_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuUpdates,
																					  this.mmAbout});
			this.menuItem2.Text = "Help";
			// 
			// mnuUpdates
			// 
			this.mnuUpdates.Index = 0;
			this.mnuUpdates.Text = "Check for Updates...";
			this.mnuUpdates.Click += new System.EventHandler(this.mnuUpdates_Click);
			// 
			// mmAbout
			// 
			this.mmAbout.Index = 1;
			this.mmAbout.Text = "About";
			this.mmAbout.Click += new System.EventHandler(this.mmAbout_Click);
			// 
			// HideTimer
			// 
			this.HideTimer.Interval = 1000;
			this.HideTimer.Tick += new System.EventHandler(this.HideTimer_Tick);
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.txLog);
			this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox4.Location = new System.Drawing.Point(8, 408);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(440, 120);
			this.groupBox4.TabIndex = 8;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Log messages";
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.ItemSize = new System.Drawing.Size(97, 18);
			this.tabControl1.Location = new System.Drawing.Point(8, 8);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(440, 392);
			this.tabControl1.TabIndex = 6;
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage1.Controls.Add(this.groupBox3);
			this.tabPage1.Controls.Add(this.btReset);
			this.tabPage1.Controls.Add(this.btDisconnect);
			this.tabPage1.Controls.Add(this.groupBox1);
			this.tabPage1.Controls.Add(this.groupBox2);
			this.tabPage1.Controls.Add(this.btConnect);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(432, 366);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Setup Connection";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.label5);
			this.groupBox3.Controls.Add(this.label6);
			this.groupBox3.Controls.Add(this.txPassword);
			this.groupBox3.Controls.Add(this.txUsername);
			this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox3.Location = new System.Drawing.Point(96, 96);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(224, 72);
			this.groupBox3.TabIndex = 5;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Credentials";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(120, 24);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(64, 16);
			this.label5.TabIndex = 3;
			this.label5.Text = "Password";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(16, 24);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(96, 16);
			this.label6.TabIndex = 2;
			this.label6.Text = "Username";
			// 
			// txPassword
			// 
			this.txPassword.Location = new System.Drawing.Point(120, 40);
			this.txPassword.Name = "txPassword";
			this.txPassword.PasswordChar = '*';
			this.txPassword.Size = new System.Drawing.Size(88, 20);
			this.txPassword.TabIndex = 4;
			this.txPassword.Text = "";
			// 
			// txUsername
			// 
			this.txUsername.Location = new System.Drawing.Point(16, 40);
			this.txUsername.Name = "txUsername";
			this.txUsername.Size = new System.Drawing.Size(88, 20);
			this.txUsername.TabIndex = 3;
			this.txUsername.Text = "";
			// 
			// btReset
			// 
			this.btReset.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btReset.Location = new System.Drawing.Point(96, 296);
			this.btReset.Name = "btReset";
			this.btReset.Size = new System.Drawing.Size(224, 23);
			this.btReset.TabIndex = 2;
			this.btReset.Text = "Reset";
			this.btReset.Click += new System.EventHandler(this.btReset_Click);
			// 
			// btDisconnect
			// 
			this.btDisconnect.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btDisconnect.Location = new System.Drawing.Point(208, 328);
			this.btDisconnect.Name = "btDisconnect";
			this.btDisconnect.Size = new System.Drawing.Size(112, 23);
			this.btDisconnect.TabIndex = 1;
			this.btDisconnect.Text = "Disconnect";
			this.btDisconnect.Click += new System.EventHandler(this.btDisconnect_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.txMgrPort);
			this.groupBox1.Controls.Add(this.txMgrHost);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(96, 16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(224, 72);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Manager Node";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(144, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(32, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Port";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(96, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Host / IP Address";
			// 
			// txMgrPort
			// 
			this.txMgrPort.Location = new System.Drawing.Point(144, 40);
			this.txMgrPort.Name = "txMgrPort";
			this.txMgrPort.Size = new System.Drawing.Size(64, 20);
			this.txMgrPort.TabIndex = 4;
			this.txMgrPort.Text = "";
			// 
			// txMgrHost
			// 
			this.txMgrHost.Location = new System.Drawing.Point(16, 40);
			this.txMgrHost.Name = "txMgrHost";
			this.txMgrHost.TabIndex = 3;
			this.txMgrHost.Text = "";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.txOwnPort);
			this.groupBox2.Controls.Add(this.cbDedicated);
			this.groupBox2.Controls.Add(this.txId);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(96, 176);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(224, 112);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Own Node";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(144, 64);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(32, 16);
			this.label3.TabIndex = 3;
			this.label3.Text = "Port";
			// 
			// txOwnPort
			// 
			this.txOwnPort.Location = new System.Drawing.Point(144, 80);
			this.txOwnPort.Name = "txOwnPort";
			this.txOwnPort.Size = new System.Drawing.Size(64, 20);
			this.txOwnPort.TabIndex = 7;
			this.txOwnPort.Text = "";
			// 
			// cbDedicated
			// 
			this.cbDedicated.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cbDedicated.Location = new System.Drawing.Point(16, 72);
			this.cbDedicated.Name = "cbDedicated";
			this.cbDedicated.Size = new System.Drawing.Size(88, 32);
			this.cbDedicated.TabIndex = 6;
			this.cbDedicated.Text = "Dedicated?";
			// 
			// txId
			// 
			this.txId.Enabled = false;
			this.txId.Location = new System.Drawing.Point(16, 40);
			this.txId.Name = "txId";
			this.txId.ReadOnly = true;
			this.txId.Size = new System.Drawing.Size(192, 20);
			this.txId.TabIndex = 5;
			this.txId.Text = "";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 24);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(96, 16);
			this.label4.TabIndex = 6;
			this.label4.Text = "Id";
			// 
			// btConnect
			// 
			this.btConnect.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btConnect.Location = new System.Drawing.Point(96, 328);
			this.btConnect.Name = "btConnect";
			this.btConnect.Size = new System.Drawing.Size(104, 23);
			this.btConnect.TabIndex = 0;
			this.btConnect.Text = "Connect";
			this.btConnect.Click += new System.EventHandler(this.btConnect_Click);
			// 
			// tabPage2
			// 
			this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage2.Controls.Add(this.btStopExec);
			this.tabPage2.Controls.Add(this.btStartExec);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(432, 366);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Manage Execution";
			// 
			// btStopExec
			// 
			this.btStopExec.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btStopExec.Location = new System.Drawing.Point(120, 152);
			this.btStopExec.Name = "btStopExec";
			this.btStopExec.Size = new System.Drawing.Size(192, 23);
			this.btStopExec.TabIndex = 1;
			this.btStopExec.Text = "Stop Executing";
			this.btStopExec.Click += new System.EventHandler(this.btStopExec_Click);
			// 
			// btStartExec
			// 
			this.btStartExec.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btStartExec.Location = new System.Drawing.Point(120, 112);
			this.btStartExec.Name = "btStartExec";
			this.btStartExec.Size = new System.Drawing.Size(192, 23);
			this.btStartExec.TabIndex = 0;
			this.btStartExec.Text = "Start Executing";
			this.btStartExec.Click += new System.EventHandler(this.btStartExec_Click);
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.label11);
			this.tabPage3.Controls.Add(this.udMaxRetry);
			this.tabPage3.Controls.Add(this.label10);
			this.tabPage3.Controls.Add(this.label9);
			this.tabPage3.Controls.Add(this.chkRetryConnect);
			this.tabPage3.Controls.Add(this.udReconnectInterval);
			this.tabPage3.Controls.Add(this.label8);
			this.tabPage3.Controls.Add(this.label7);
			this.tabPage3.Controls.Add(this.lblHBInt);
			this.tabPage3.Controls.Add(this.udHBInterval);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(432, 366);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Options";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(183, 126);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(88, 16);
			this.label11.TabIndex = 9;
			this.label11.Text = "times at most.";
			// 
			// udMaxRetry
			// 
			this.udMaxRetry.Location = new System.Drawing.Point(118, 124);
			this.udMaxRetry.Minimum = new System.Decimal(new int[] {
																	   1,
																	   0,
																	   0,
																	   -2147483648});
			this.udMaxRetry.Name = "udMaxRetry";
			this.udMaxRetry.Size = new System.Drawing.Size(52, 20);
			this.udMaxRetry.TabIndex = 8;
			this.udMaxRetry.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.udMaxRetry.Value = new System.Decimal(new int[] {
																	 3,
																	 0,
																	 0,
																	 0});
			this.udMaxRetry.ValueChanged += new System.EventHandler(this.udMaxRetry_ValueChanged);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(24, 126);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(88, 16);
			this.label10.TabIndex = 7;
			this.label10.Text = "Try to reconnect ";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(24, 99);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(176, 16);
			this.label9.TabIndex = 6;
			this.label9.Text = "Continue to try and connect every";
			// 
			// chkRetryConnect
			// 
			this.chkRetryConnect.Checked = true;
			this.chkRetryConnect.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkRetryConnect.Location = new System.Drawing.Point(24, 72);
			this.chkRetryConnect.Name = "chkRetryConnect";
			this.chkRetryConnect.Size = new System.Drawing.Size(267, 17);
			this.chkRetryConnect.TabIndex = 5;
			this.chkRetryConnect.Text = "Retry connecting to Manager on disconnection.";
			this.chkRetryConnect.CheckedChanged += new System.EventHandler(this.chkRetryConnect_CheckedChanged);
			// 
			// udReconnectInterval
			// 
			this.udReconnectInterval.Location = new System.Drawing.Point(200, 97);
			this.udReconnectInterval.Maximum = new System.Decimal(new int[] {
																				10000,
																				0,
																				0,
																				0});
			this.udReconnectInterval.Minimum = new System.Decimal(new int[] {
																				2,
																				0,
																				0,
																				0});
			this.udReconnectInterval.Name = "udReconnectInterval";
			this.udReconnectInterval.Size = new System.Drawing.Size(72, 20);
			this.udReconnectInterval.TabIndex = 4;
			this.udReconnectInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.udReconnectInterval.Value = new System.Decimal(new int[] {
																			  30,
																			  0,
																			  0,
																			  0});
			this.udReconnectInterval.ValueChanged += new System.EventHandler(this.udReconnectInterval_ValueChanged);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(280, 99);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(56, 16);
			this.label8.TabIndex = 3;
			this.label8.Text = "seconds.";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(216, 24);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(56, 16);
			this.label7.TabIndex = 2;
			this.label7.Text = "seconds.";
			// 
			// lblHBInt
			// 
			this.lblHBInt.Location = new System.Drawing.Point(24, 23);
			this.lblHBInt.Name = "lblHBInt";
			this.lblHBInt.Size = new System.Drawing.Size(100, 16);
			this.lblHBInt.TabIndex = 1;
			this.lblHBInt.Text = "Heartbeat interval";
			// 
			// udHBInterval
			// 
			this.udHBInterval.Location = new System.Drawing.Point(133, 20);
			this.udHBInterval.Maximum = new System.Decimal(new int[] {
																		 10000,
																		 0,
																		 0,
																		 0});
			this.udHBInterval.Minimum = new System.Decimal(new int[] {
																		 2,
																		 0,
																		 0,
																		 0});
			this.udHBInterval.Name = "udHBInterval";
			this.udHBInterval.Size = new System.Drawing.Size(72, 20);
			this.udHBInterval.TabIndex = 0;
			this.udHBInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.udHBInterval.Value = new System.Decimal(new int[] {
																	   5,
																	   0,
																	   0,
																	   0});
			this.udHBInterval.ValueChanged += new System.EventHandler(this.udHBInterval_ValueChanged);
			// 
			// pbar
			// 
			this.pbar.Location = new System.Drawing.Point(200, 548);
			this.pbar.Maximum = 5;
			this.pbar.Name = "pbar";
			this.pbar.Size = new System.Drawing.Size(240, 8);
			this.pbar.Step = 1;
			this.pbar.TabIndex = 12;
			this.pbar.Visible = false;
			// 
			// ExecutorMainForm
			// 
			this.AcceptButton = this.btConnect;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(458, 563);
			this.Controls.Add(this.pbar);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.statusBar);
			this.Controls.Add(this.tabControl1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Menu = this.MainMenu;
			this.Name = "ExecutorMainForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Alchemi Executor";
			this.Load += new System.EventHandler(this.ExecutorMainForm_Load);
			this.groupBox4.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udMaxRetry)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udReconnectInterval)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.udHBInterval)).EndInit();
			this.ResumeLayout(false);

		}
        #endregion

        //-----------------------------------------------------------------------------------------------        
        //
        // Form Event Handlers
        //
        //-----------------------------------------------------------------------------------------------    
    
        private void ExecutorMainForm_Load(object sender, EventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(DefaultErrorHandler);
			
			//for windows forms apps unhandled exceptions on the main thread
			Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            
			// avoid multiple instances
			/*
            bool isOnlyInstance = false;
            Mutex mtx = new Mutex(true, "AlchemiExecutor_Mutex", out isOnlyInstance);
            if(!isOnlyInstance)
            {
                MessageBox.Show("An instance of this application is already running. The program will now exit.", "Alchemi Executor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Application.Exit();
            }
            */

			Logger.LogHandler += new LogEventHandler(this.LogHandler);

			ExecutorContainer.NonDedicatedExecutingStatusChanged += new NonDedicatedExecutingStatusChangedEventHandler(this.RefreshUIControls);
            ExecutorContainer.GotDisconnected += new GotDisconnectedEventHandler(this.Executor_GotDisconnected);
			ExecutorContainer.ExecConnectEvent += new ExecutorConnectStatusEventHandler(this.ExecutorConnect_Status);


			if (isService)
			{
				//this is a service. just read the config.
				ExecutorContainer ec = new ExecutorContainer();
				ec.ReadConfig(false);
				Config = ec.Config;
				ec = null;

				this.btConnect.Text = "Start";
				this.btDisconnect.Text = "Stop";
			}
			else
			{	//not a service. normal exec startup mode.
				_container = new ExecutorContainer();
				_container.ReadConfig(false);
				Config = _container.Config;

				this.btConnect.Text = "Connect";
				this.btDisconnect.Text = "Disconnect";

				ConnectOnStartup();
			}

			RefreshUIControls();
			btConnect.Focus();

        }
    
        //-----------------------------------------------------------------------------------------------    
    
        private void btConnect_Click(object sender, EventArgs e)
        {
			if (isService)
			{
				StartExecutorService();
			}else
			{
				ConnectExecutor();
			}
        }

		private void ConnectOnStartup()
		{
			try
			{
				//we dont call start here, because we are doing the same thing here
				_container.ReadConfig(false);
				Config = _container.Config;
				RefreshUIControls();
				if (Config.ConnectVerified)
				{
					Log("Using last verified configuration ...");
					ConnectExecutor();
					HideTimer.Enabled = true;
				}
			}
			catch (Exception ex)
			{
				logger.Error("Error connecting to manager",ex);
				Log("Error connecting to manager. " + ex.Message);
			}
		}

    	private void ConnectExecutor()
    	{
			//if the update process is going on, avoid starting the manager.
			if (updating)
			{
				MessageBox.Show(this,"The manager is checking for updates. It can be started only after the updates are completed.","Update Alchemi Manager",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				return;
			}

			if (Started)
			{
				RefreshUIControls();
				return;
			}

			try
			{
				Log("Attempting to connect to Manager...");
				GetConfigFromUI();
				_container.Config = Config;
				_container.Connect();
				Log("Connected to Manager.");
			}
			catch (Exception ex)
			{
				logger.Error("Error connecting to manager.",ex);
				Log("Error connecting to manager.");
			}
			RefreshUIControls();
    	}

    	private void GetConfigFromUI()
    	{
			if (Config == null)
			{
				Config = new Configuration();
			}
			// read config from ui controls
			Config.ManagerHost = txMgrHost.Text;
			Config.ManagerPort = int.Parse(txMgrPort.Text);
			Config.OwnPort = int.Parse(txOwnPort.Text);
			Config.Dedicated = cbDedicated.Checked;
			Config.HeartBeatInterval = (int)udHBInterval.Value;
			Config.RetryConnect = chkRetryConnect.Checked;
			Config.RetryInterval = (int)udReconnectInterval.Value;
			Config.RetryMax = (int)udMaxRetry.Value;
		}

    	private void StartExecutorService()
		{
			//if the update process is going on, avoid starting the executor.
			if (updating)
			{
				MessageBox.Show(this,"The executor is checking for updates. It can be started only after the updates are completed.","Update Alchemi Executor",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				return;
			}

			if (Started)
			{
				Log("Executor Service is already started.");
				RefreshUIControls();
				return;
			}

			try
			{
				//to avoid people from clicking this again
				btConnect.Enabled = false;
				statusBar.Text = "Starting Executor Service...";
				Log("Attempting to start Executor Service...");

				ServiceController sc = new ServiceController(serviceName);
				if (sc.Status != ServiceControllerStatus.Running && sc.Status != ServiceControllerStatus.StartPending)
				{
					//get latest config and serialize the  object, so that the service uses the latest config.
					if (Config!=null)
					{
						GetConfigFromUI();
						Config.Slz();
					}
					sc.Start();
					sc.WaitForStatus(ServiceControllerStatus.Running,new TimeSpan(0,0,28));
					Log("Executor Service started.");
				}
			}
			catch (TimeoutException)
			{
				Log("Timeout expired trying to start Executor Service.");
			}
			catch (Exception ex)
			{
				Log("Error starting ExecutorService");
				logger.Error("Error starting ExecutorService",ex);
				StopExecutorService();
			}
			RefreshUIControls();
		}

    	//-----------------------------------------------------------------------------------------------    
    
        private void mmExit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        //-----------------------------------------------------------------------------------------------    

        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_CLOSE = 0xF060;
            if (m.Msg == WM_SYSCOMMAND & (int) m.WParam == SC_CLOSE)
            {
                // 'X' button clicked .. minimise to system tray
                Hide();
                return;
            }
            base.WndProc(ref m);
        }
    
        //-----------------------------------------------------------------------------------------------    
    
        private void TrayIcon_Click(object sender, EventArgs e)
        {
            Restore();
        }

        //-----------------------------------------------------------------------------------------------    

        private void tmExit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        //-----------------------------------------------------------------------------------------------    
    
        private void btStartExec_Click(object sender, EventArgs e)
        {
			try
			{
				_container.Executor.StartNonDedicatedExecuting(1000);
			}
			catch (Exception ex)
			{
				logger.Error("Error starting non-dedication execution.",ex) ;
				Log("Error starting non-dedication execution.");
			}
			RefreshUIControls();
        }

        //-----------------------------------------------------------------------------------------------    
    
        private void btStopExec_Click(object sender, EventArgs e)
        {
			try
			{
				_container.Executor.StopNonDedicatedExecuting();
			}
			catch (Exception ex)
			{
				logger.Error("Error stopping non-dedication execution.",ex) ;
				Log("Error stopping non-dedication execution.");
			}
			RefreshUIControls();
        }
    
        //-----------------------------------------------------------------------------------------------    

        private void mmAbout_Click(object sender, EventArgs e)
        {
            ShowSplashScreen();
        }

        //-----------------------------------------------------------------------------------------------    

        static void DefaultErrorHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception) args.ExceptionObject;
//            if (e.GetType() == typeof(AppDomainUnloadedException))
//            {
//                // can't figure out why this exception is being thrown .. in any case, seems safe to ignore it
//                //return;
//            }
			HandleAllUnknownErrors(sender.ToString(),e);
        }                                   

        //-----------------------------------------------------------------------------------------------    
    
        private void HideTimer_Tick(object sender, EventArgs e)
        {
            Hide();
            HideTimer.Enabled = false;
        }

        //-----------------------------------------------------------------------------------------------        
        //
        // Core Methods
        //
        //-----------------------------------------------------------------------------------------------    

        private void RefreshUIControls()
        {
			//dont need to keep calling the getter of the property, since it keeps querying the service
			bool connected = Started;

            txMgrHost.Text = Config.ManagerHost;
            txMgrPort.Text = Config.ManagerPort.ToString();
            txId.Text = Config.Id;
            txOwnPort.Text = Config.OwnPort.ToString();
            txUsername.Text = Config.Username;
            txPassword.Text = Config.Password;
            cbDedicated.Checked = Config.Dedicated;
      
            btConnect.Enabled = !connected;
            btReset.Enabled = !connected;
            btDisconnect.Enabled = !btConnect.Enabled;

            txMgrHost.Enabled = !connected;
            txMgrPort.Enabled = !connected;
            txUsername.Enabled = !connected;
            txPassword.Enabled = !connected;
            txOwnPort.Enabled = !connected;
            cbDedicated.Enabled = !connected;

			udHBInterval.Value = (decimal)Config.HeartBeatInterval;
			udMaxRetry.Value = (decimal)Config.RetryMax;

			chkRetryConnect.Checked = Config.RetryConnect;
			udReconnectInterval.Value = (decimal)Config.RetryInterval;

			pbar.Visible = false;
			pbar.Value = 0;

			if (isService)
			{
				//hide the non-dedicated mode controls
				tabControl1.TabPages[1].Visible = false;
				cbDedicated.Visible = false;

				if (connected)
				{
					statusBar.Text = "Executor Started.";
				}
				else
				{
					statusBar.Text = "Executor Stopped.";
				}
			}
			else
			{           
				bool executing = false;
				if (_container!=null && _container.Executor!=null && _container.Executor.ExecutingNonDedicated)
                {
                    executing = true;
                }
				btStartExec.Enabled = ((!Config.Dedicated) && (connected) && (!executing));
				btStopExec.Enabled = ((!Config.Dedicated) && (connected) && (executing));

				string sbAppend = (Config.Dedicated? " (dedicated)" : " (non-dedicated)");
				if ((connected && executing) || (connected && Config.Dedicated))
				{
					statusBar.Text = "Executing" + sbAppend;
				}
				else if(connected)
				{
					statusBar.Text = "Connected" + sbAppend;
				}
				else
				{
					statusBar.Text = "Disconnected";
				}
			}
        }

        //-----------------------------------------------------------------------------------------------            

        private void Log(string s)
        {
            if (txLog != null)
            {
				if (txLog.Text.Length + s.Length >= txLog.MaxLength)
				{
					//remove all old stuff except the last 10 lines.
					string[] s1 = new string[10];
					for (int i = 0 ; i < 10 ; i++)
					{
						s1[9-i]=txLog.Lines[txLog.Lines.Length-1-i];
					}
					txLog.Lines = s1;
				}
				txLog.AppendText(s + Environment.NewLine);
            }
        }

        //-----------------------------------------------------------------------------------------------            

        private void Restore()
        {
            this.WindowState = FormWindowState.Normal;
            this.Show();
            this.Activate();
        }

        //-----------------------------------------------------------------------------------------------            
   
        private void Exit()
        {
			if (!isService)
			{
				DisconnectExecutor();
			}
			//we dont stop the service just because the serviceController is closed.

			this.Close();
			Application.Exit();

        }
    
        //-----------------------------------------------------------------------------------------------    

		//happens only for non-dedicated mode.
        private void Executor_GotDisconnected()
        {
			try
			{
				_container.Executor = null;
				RefreshUIControls();

				//just double check.
				//flag to find out if the disconnect was a manual operation by the user.
				if (btDisconnect.Tag == null)
				{
					Log("Got disconnected!");
					logger.Debug ("Got disconnected!");
					if (chkRetryConnect.Checked)
					{
						Log("Trying to reconnect...");
						logger.Debug("Trying to reconnect...");
						_container.Reconnect((int)udMaxRetry.Value,(int)udReconnectInterval.Value);
					}
				}
				btDisconnect.Tag = null; //reset it here
			}
			catch (Exception e)
			{
				logger.Error("Error Executor_GotDisconnected ",e) ;
				RefreshUIControls();
			}
        }

        //-----------------------------------------------------------------------------------------------    
    
        private void btDisconnect_Click(object sender, EventArgs e)
        {
			if (isService)
			{
				StopExecutorService();
			}
			else
			{
				DisconnectExecutor();
			}
        }

    	private void StopExecutorService()
    	{
			if (!Started)
			{
				Log("The Executor Service is already stopped.");
				RefreshUIControls();
				return;
			}

			try
			{
				statusBar.Text = "Stopping Executor Service...";
				Log("Stopping Executor Service...");

				btDisconnect.Enabled = false; //to avoid clicking on this again.
				ServiceController sc = new ServiceController(serviceName);
				if (sc.CanStop)
				{
					sc.Stop();
					sc.WaitForStatus(ServiceControllerStatus.Stopped,new TimeSpan(0,0,28));
					Log("Executor Service stopped.");
				}
				else
				{
					logger.Debug("Couldnot stop service: CanStop = false");	
				}
			}
			catch (TimeoutException)
			{
				Log("Timeout expired trying to stop Executor Service.");
			}
			catch (Exception ex)
			{
				Log("Error stopping Executor Service");
				logger.Error("Error stopping Executor Service",ex);
			}
			RefreshUIControls();
    	}

		/// <summary>
		/// Specifies whether the Executor is Connected / ExecutorService is Started
		/// </summary>
		private bool Started
		{
			get
			{
				bool started = false;
				if (isService)
				{
					try
					{
						ServiceController sc = new ServiceController(serviceName);
						if (sc.Status == ServiceControllerStatus.Running || sc.Status == ServiceControllerStatus.StartPending)
						{
							started = true;
						}
						sc = null;
					}
					catch (Exception ex)
					{
						logger.Error("Error trying to determine service status",ex);
					}
				}
				else
				{
					started =_container==null ? false : _container.Connected;
				}
				return started;
			}
		}

    	private void DisconnectExecutor()
    	{
			if (!Started)
			{
				RefreshUIControls();
				return;
			}

			try
			{
				pbar.Visible = true;
				pbar.Value = pbar.Maximum / 2;

				//set a flag to show that this is a manual disconnect,
				//so should not reconnect.
				btDisconnect.Tag = "true";
				_container.Disconnect();
				Log("Disconnected successfully.");
				pbar.Value = pbar.Maximum;
			}
			catch (Exception ex)
			{
				logger.Debug("Error disconnecting from Manager.",ex);
				Log("Error disconnecting from Manager." + ex.Message);
			}
			RefreshUIControls();
    	}


    	//-----------------------------------------------------------------------------------------------    

        private void btReset_Click(object sender, EventArgs e)
        {
			//in case it is a service, the container would be null since we dont need it really.
			//but we still need to get the config from it, so create a new one and read the config.
			if (isService)
			{
				ExecutorContainer ec = new ExecutorContainer();
				ec.ReadConfig(true);
				Config = ec.Config;
				ec = null;
			}
			else
			{
				_container.ReadConfig(true);
				Config = _container.Config;
			}
			RefreshUIControls();
        }

        //-----------------------------------------------------------------------------------------------    

        private void ShowSplashScreen()
        {
            SplashScreen ss = new SplashScreen();
            ss.ShowDialog();
        }

        private void txLog_DoubleClick(object sender, EventArgs e)
        {
            txLog.Clear();
        }

		private void chkRetryConnect_CheckedChanged(object sender, EventArgs e)
		{
			udReconnectInterval.Enabled = chkRetryConnect.Checked;
			udMaxRetry.Enabled = chkRetryConnect.Checked;
			Config.RetryConnect = chkRetryConnect.Checked;
		}

		private void udHBInterval_ValueChanged(object sender, EventArgs e)
		{
			if (_container.Executor != null)
				_container.Executor.HeartBeatInterval = (int)udHBInterval.Value;
			Config.HeartBeatInterval = (int) udHBInterval.Value;
		}

		private void udReconnectInterval_ValueChanged(object sender, EventArgs e)
		{
			Config.RetryInterval = (int)udReconnectInterval.Value;
		}

		private void udMaxRetry_ValueChanged(object sender, EventArgs e)
		{
			Config.RetryMax = (int)udMaxRetry.Value;
		}

		private void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			HandleAllUnknownErrors(sender.ToString(),e.Exception);
		}

		static void HandleAllUnknownErrors(string sender, Exception e)
		{
			logger.Error("Unknown Error from: " + sender,e);
			if (!silentMode)
			{
				MessageBox.Show(e.ToString(), "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		//just for showing the progress bar.
    	public void ExecutorConnect_Status(string msg, int percentDone)
    	{
			statusBar.Text = msg;
			Log(msg);
			logger.Info("Status message from ExecutorContainer: "+msg);
			if (percentDone==0)
			{
				pbar.Value = 0;
				pbar.Visible = true;
			}
			else if (percentDone >= 100)
			{
				pbar.Value = pbar.Maximum;
				pbar.Visible = false;
			}
			else
			{
				if ((percentDone + pbar.Value) <= pbar.Maximum){
					pbar.Value = percentDone;
				}else
				{
					pbar.Value = pbar.Maximum;
				}
    		}
    	}

		private void LogHandler(object sender, LogEventArgs e)
		{
			switch (e.Level)
			{
				case LogLevel.Debug:
					string message = e.Source  + ":" + e.Member + " - " + e.Message;
					logger.Debug(message,e.Exception);
					break;
				case LogLevel.Info:
					logger.Info(e.Message);
					break;
				case LogLevel.Error:
					logger.Error(e.Message,e.Exception);
					break;
				case LogLevel.Warn:
					logger.Warn(e.Message);
					break;
			}
		}

		private void mnuUpdates_Click(object sender, EventArgs e)
		{
			updateExecutor();
		}

		#region "Updater Stuff"

		//**************************************************************
		// This event is called everytime the appupdater attempts to check for 
		// an update.  You can hook this event and perform your own update check.
		// Return a boolean value indicating whether an update was found.  
		// Notes:  
		//   * This event fires on the poller thread, so you can make network requests 
		//     to check for updates & it will be done asyncronously.
		//   * Because this event has a non void return value, it can only
		//     be handled by one event handler.  It can not be multi-cast.
		//**************************************************************
		//		public bool updater_OnCheckForUpdate(object sender, EventArgs e)
		//		{
		//			//TODO : can implement an updates webservice... later on
		//			return true;
		//		}

		//**************************************************************
		// This event if fired whenever a new update is detected.
		// The default behavior is to start the update download immediatly.
		// However, in this case we have disabled that feature by setting
		// the DownloadOnDetection property to false. In this case we'll ask 
		// the user if they want to download the update & then use the ApplyUpdate()
		// method if they wish too.  Note that this event will fire on the main
		// UI thread to allow interaction with the app UI.  
		//**************************************************************
		public void updater_OnUpdateDetected(object sender, UpdateDetectedEventArgs e)
		{
			try
			{
				if (e.UpdateDetected)
				{
					Log("Executor updater: Found new updates...");
				
					DialogResult res = MessageBox.Show(this,"New updates were found. Click Yes to proceed with downloading the updates, No to cancel the download","Alchemi Executor",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
					if (res == DialogResult.Yes)
					{
						//async method, starts the download 
						updater.DownloadUpdate();
					}
					else
					{ //user selected NO downloads...
						updating = false;
					}
				}
				else
				{
					Log("Executor updater: No new updates...update complete.");
					MessageBox.Show(this,"No new updates found.","Alchemi Executor",MessageBoxButtons.OK,MessageBoxIcon.Information);
					updating = false;
				}
			}
			catch (Exception ex)
			{
				handleUpdaterError("Error in DoUpdateDetectedAction: update found = "+ e.UpdateDetected ,ex);
			}
		}

		//**************************************************************
		// This event if fired whenever a new update is complete.
		// You could do whatever shut down logic your app needs to run
		// here (ex. saving files).  The Exit() method will close
		// the current app.
		//**************************************************************		
		public void updater_OnUpdateComplete(object sender, UpdateCompleteEventArgs e)
		{
			//If the udpate succeeded...
			if ( e.UpdateSucceeded)
			{
				string message = "The updates were successfully completed. Do you want to start using the new udpate now?";		
				if (MessageBox.Show(this,message,"Update Complete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					//The return code must be set to tell appstart to restart the app
					//The app must shut itself down. Note:  Don't use methods like Environment.Exit that
					//will shut down the entire process, when using appstart in appdomain mode.
					this.returnCode = AppUpdater.RestartAppReturnValue;
					Exit();
				}
			} 
			else //If the update failed....
			{
				handleUpdaterError(e.ErrorMessage,e.FailureException);
			}		
		}

		private void updateExecutor()
		{
			if (updating)
			{
				MessageBox.Show(this,"Another update process is already in progress","Alchemi Executor", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				return;
			}

			try
			{
				if (Started)
				{ //if executor is started then stop it
					DialogResult res = MessageBox.Show(this,"The executor needs to be stopped before checking for updates. Click Yes to proceed, No to cancel checking for updates","Alchemi Executor",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
					if (res == DialogResult.No)
					{
						return;
					}
					if (isService)
					{
						StopExecutorService();
					}
					else
					{
						DisconnectExecutor();
					}
				}
				updating = true;

				Log("Executor updater: Checking for updates...");

				updater.CheckForUpdates();
			}
			catch (Exception ex)
			{
				handleUpdaterError("Error updating executor...",ex);
			}
		}
		
		private void updater_OnDownloadProgress(object sender, DownloadProgressEventArgs e)
		{
			try
			{
				pbar.Visible = true;
				pbar.Value = (int)e.CurrentState;
				statusBar.Text = "Downloading " + e.CurrentState + " %";
			}
			catch {}
		}


		private void handleUpdaterError(string msg, Exception ex)
		{
			updating = false;
			Log(msg + Environment.NewLine + ex.Message);
			logger.Error("Error Updating Executor ",ex) ;	
			MessageBox.Show(this,"Error updating executor: " + ex.Message,"Alchemi Executor",MessageBoxButtons.OK,MessageBoxIcon.Error);
			RefreshUIControls();
		}

    	#endregion

    }
}
