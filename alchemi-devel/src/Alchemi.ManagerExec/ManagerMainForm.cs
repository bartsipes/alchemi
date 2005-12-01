#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	ManagerMainForm.cs
* Project		:	Alchemi Manager Application
* Created on	:	2003
* Copyright		:	Copyright © 2005 The University of Melbourne
*					This technology has been developed with the support of 
*					the Australian Research Council and the University of Melbourne
*					research grants as part of the Gridbus Project
*					within GRIDS Laboratory at the University of Melbourne, Australia.
* Author         :  Akshay Luther (akshayl@csse.unimelb.edu.au), Rajkumar Buyya (raj@csse.unimelb.edu.au), and Krishna Nadiminti (kna@csse.unimelb.edu.au)
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
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using Alchemi.Core;
using Alchemi.Core.Manager;
using Alchemi.Manager;
//using Alchemi.Updater;
using log4net;


// Configure log4net using the .config file
[assembly: log4net.Config.XmlConfigurator(Watch=true)]

namespace Alchemi.ManagerExec
{
	public class ManagerMainForm : Form
	{
		private Label label1;
		private Label label2;
		private Button btStart;
		private GroupBox groupBox1;
		private IContainer components;
		private GroupBox groupBox2;
		private TextBox txLog;
		private Label label3;
		private TextBox txOwnPort;
		private TextBox txManagerHost;
		private TextBox txManagerPort;
		private TextBox txId;
		private CheckBox cbIntermediate;
		private Label label4;
		private Button btStop;
		private Button btReset;
		private CheckBox cbDedicated;
		private NotifyIcon TrayIcon;
		private ContextMenu TrayMenu;
		private MenuItem tmExit;
		private MainMenu MainMenu;
		private MenuItem menuItem1;
		private MenuItem mmExit;
		private Label label5;
		private TextBox txDbPassword;
		private Label label6;
		private TextBox txDbUsername;
		private Label label7;
		private TextBox txDbServer;
		private Label label8;
		private TextBox txDbName;
		private GroupBox groupBox3;
		private GroupBox groupBox4;
		private MenuItem menuItem2;
		private MenuItem mmAbout;

		private ManagerContainer _container = null;
		private Configuration Config;

		// Create a logger for use in this class
		private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private StatusBar statusBar;
		private TabControl tabControl1;
		private TabPage tabPage1;
		private ProgressBar pbar;

//		//updater stuff
//		private AppUpdater updater = null;
//		private const string updateExecURL = "http://www.alchemi.net/updates/manager/exec/ManagerExecManifest.xml";
//		private const string updateServiceURL = "http://www.alchemi.net/updates/manager/service/ManagerServiceManifest.xml";
//		private bool updating = false;
		public int returnCode = 0;

		//field specifying whether the manager-form is a service controller.
		private bool isService = false;
		private const string serviceName = "Alchemi Manager Service";

		public ManagerMainForm(): this(false)
		{
		}

		public ManagerMainForm(bool isService)
		{
			InitializeComponent();
 			this.isService = isService;
     
			//string updateURL;
			if (isService)
			{
				this.Text = "Alchemi Manager Service Controller";
				//updateURL = updateServiceURL;
			}
			else
			{
				this.Text = "Alchemi Manager";
				//updateURL = updateExecURL;
				Logger.LogHandler += new LogEventHandler(LogHandler);
				ManagerContainer.ManagerStartEvent += new ManagerStartStatusEventHandler(this.Manager_StartStatusEvent);
			}

//			// 
//			// updater
//			// 
//			updater = new AppUpdater(updateURL,ChangeDetectionModes.ServerManifestCheck,false,false,false);
//			
//			//need this only for a custom update detection system
//			//updater.OnCheckForUpdate += new AppUpdater.CheckForUpdateEventHandler(this.updater_OnCheckForUpdate);
//			updater.OnUpdateDetected += new AppUpdater.UpdateDetectedEventHandler(this.updater_OnUpdateDetected);
//			updater.OnUpdateComplete += new AppUpdater.UpdateCompleteEventHandler(this.updater_OnUpdateComplete);
//			updater.OnDownloadProgress += new AppUpdater.DownloadProgressEventHandler(updater_OnDownloadProgress);

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ManagerMainForm));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txManagerHost = new System.Windows.Forms.TextBox();
			this.txOwnPort = new System.Windows.Forms.TextBox();
			this.btStart = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cbDedicated = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.cbIntermediate = new System.Windows.Forms.CheckBox();
			this.txId = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txManagerPort = new System.Windows.Forms.TextBox();
			this.btReset = new System.Windows.Forms.Button();
			this.btStop = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.txLog = new System.Windows.Forms.TextBox();
			this.TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.TrayMenu = new System.Windows.Forms.ContextMenu();
			this.tmExit = new System.Windows.Forms.MenuItem();
			this.MainMenu = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.mmExit = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.mmAbout = new System.Windows.Forms.MenuItem();
			this.label5 = new System.Windows.Forms.Label();
			this.txDbPassword = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.txDbUsername = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.txDbServer = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.txDbName = new System.Windows.Forms.TextBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.pbar = new System.Windows.Forms.ProgressBar();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(48, 128);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "ManagerHost";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(72, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 16);
			this.label2.TabIndex = 2;
			this.label2.Text = "OwnPort";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// txManagerHost
			// 
			this.txManagerHost.Location = new System.Drawing.Point(120, 128);
			this.txManagerHost.Name = "txManagerHost";
			this.txManagerHost.Size = new System.Drawing.Size(104, 20);
			this.txManagerHost.TabIndex = 9;
			this.txManagerHost.Text = "";
			// 
			// txOwnPort
			// 
			this.txOwnPort.Location = new System.Drawing.Point(120, 24);
			this.txOwnPort.Name = "txOwnPort";
			this.txOwnPort.Size = new System.Drawing.Size(104, 20);
			this.txOwnPort.TabIndex = 5;
			this.txOwnPort.Text = "";
			// 
			// btStart
			// 
			this.btStart.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btStart.Location = new System.Drawing.Point(88, 50);
			this.btStart.Name = "btStart";
			this.btStart.Size = new System.Drawing.Size(128, 23);
			this.btStart.TabIndex = 12;
			this.btStart.Text = "Start";
			this.btStart.Click += new System.EventHandler(this.btStart_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.cbDedicated);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.cbIntermediate);
			this.groupBox1.Controls.Add(this.txId);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.txManagerPort);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.txOwnPort);
			this.groupBox1.Controls.Add(this.txManagerHost);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(8, 99);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(416, 192);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Node Configuration";
			// 
			// cbDedicated
			// 
			this.cbDedicated.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cbDedicated.Location = new System.Drawing.Point(120, 96);
			this.cbDedicated.Name = "cbDedicated";
			this.cbDedicated.Size = new System.Drawing.Size(88, 24);
			this.cbDedicated.TabIndex = 8;
			this.cbDedicated.Text = "Dedicated";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(96, 72);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(16, 16);
			this.label4.TabIndex = 12;
			this.label4.Text = "Id";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// cbIntermediate
			// 
			this.cbIntermediate.Enabled = false;
			this.cbIntermediate.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cbIntermediate.Location = new System.Drawing.Point(120, 48);
			this.cbIntermediate.Name = "cbIntermediate";
			this.cbIntermediate.Size = new System.Drawing.Size(88, 24);
			this.cbIntermediate.TabIndex = 6;
			this.cbIntermediate.TabStop = false;
			this.cbIntermediate.Text = "Intermediate";
			this.cbIntermediate.CheckedChanged += new System.EventHandler(this.cbIntermediate_CheckedChanged);
			// 
			// txId
			// 
			this.txId.Enabled = false;
			this.txId.Location = new System.Drawing.Point(120, 72);
			this.txId.Name = "txId";
			this.txId.Size = new System.Drawing.Size(240, 20);
			this.txId.TabIndex = 7;
			this.txId.TabStop = false;
			this.txId.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(48, 160);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 16);
			this.label3.TabIndex = 6;
			this.label3.Text = "ManagerPort";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// txManagerPort
			// 
			this.txManagerPort.Location = new System.Drawing.Point(120, 160);
			this.txManagerPort.Name = "txManagerPort";
			this.txManagerPort.Size = new System.Drawing.Size(104, 20);
			this.txManagerPort.TabIndex = 10;
			this.txManagerPort.Text = "";
			// 
			// btReset
			// 
			this.btReset.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btReset.Location = new System.Drawing.Point(88, 20);
			this.btReset.Name = "btReset";
			this.btReset.Size = new System.Drawing.Size(248, 23);
			this.btReset.TabIndex = 11;
			this.btReset.TabStop = false;
			this.btReset.Text = "Reset";
			this.btReset.Click += new System.EventHandler(this.btReset_Click);
			// 
			// btStop
			// 
			this.btStop.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btStop.Location = new System.Drawing.Point(224, 50);
			this.btStop.Name = "btStop";
			this.btStop.Size = new System.Drawing.Size(112, 23);
			this.btStop.TabIndex = 13;
			this.btStop.Text = "Stop";
			this.btStop.Click += new System.EventHandler(this.btStop_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.txLog);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(10, 424);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(432, 120);
			this.groupBox2.TabIndex = 7;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Log Messages";
			// 
			// txLog
			// 
			this.txLog.Location = new System.Drawing.Point(8, 16);
			this.txLog.Multiline = true;
			this.txLog.Name = "txLog";
			this.txLog.ReadOnly = true;
			this.txLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txLog.Size = new System.Drawing.Size(416, 96);
			this.txLog.TabIndex = 14;
			this.txLog.TabStop = false;
			this.txLog.Text = "";
			this.txLog.DoubleClick += new System.EventHandler(this.txLog_DoubleClick);
			// 
			// TrayIcon
			// 
			this.TrayIcon.ContextMenu = this.TrayMenu;
			this.TrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("TrayIcon.Icon")));
			this.TrayIcon.Text = "Alchemi Manager";
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
			this.menuItem1.Text = "Manager";
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
																					  this.mmAbout});
			this.menuItem2.Text = "Help";
			// 
			// mmAbout
			// 
			this.mmAbout.Index = 0;
			this.mmAbout.Text = "About";
			this.mmAbout.Click += new System.EventHandler(this.mmAbout_Click);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(176, 56);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(88, 16);
			this.label5.TabIndex = 13;
			this.label5.Text = "DbPassword";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// txDbPassword
			// 
			this.txDbPassword.Location = new System.Drawing.Point(264, 56);
			this.txDbPassword.Name = "txDbPassword";
			this.txDbPassword.PasswordChar = '*';
			this.txDbPassword.Size = new System.Drawing.Size(104, 20);
			this.txDbPassword.TabIndex = 4;
			this.txDbPassword.Text = "";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(192, 24);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(72, 16);
			this.label6.TabIndex = 15;
			this.label6.Text = "DbUsername";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// txDbUsername
			// 
			this.txDbUsername.Location = new System.Drawing.Point(264, 24);
			this.txDbUsername.Name = "txDbUsername";
			this.txDbUsername.Size = new System.Drawing.Size(104, 20);
			this.txDbUsername.TabIndex = 3;
			this.txDbUsername.Text = "";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(16, 24);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(56, 16);
			this.label7.TabIndex = 17;
			this.label7.Text = "DbServer";
			this.label7.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// txDbServer
			// 
			this.txDbServer.Location = new System.Drawing.Point(72, 24);
			this.txDbServer.Name = "txDbServer";
			this.txDbServer.Size = new System.Drawing.Size(104, 20);
			this.txDbServer.TabIndex = 1;
			this.txDbServer.Text = "";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(16, 56);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(56, 16);
			this.label8.TabIndex = 19;
			this.label8.Text = "DbName";
			this.label8.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// txDbName
			// 
			this.txDbName.Location = new System.Drawing.Point(72, 56);
			this.txDbName.Name = "txDbName";
			this.txDbName.Size = new System.Drawing.Size(104, 20);
			this.txDbName.TabIndex = 2;
			this.txDbName.Text = "";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.txDbPassword);
			this.groupBox3.Controls.Add(this.label5);
			this.groupBox3.Controls.Add(this.txDbName);
			this.groupBox3.Controls.Add(this.txDbUsername);
			this.groupBox3.Controls.Add(this.label8);
			this.groupBox3.Controls.Add(this.label7);
			this.groupBox3.Controls.Add(this.label6);
			this.groupBox3.Controls.Add(this.txDbServer);
			this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox3.Location = new System.Drawing.Point(8, 6);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(416, 88);
			this.groupBox3.TabIndex = 8;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Database Configuration";
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.btReset);
			this.groupBox4.Controls.Add(this.btStop);
			this.groupBox4.Controls.Add(this.btStart);
			this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox4.Location = new System.Drawing.Point(8, 295);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(416, 80);
			this.groupBox4.TabIndex = 9;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Actions";
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 557);
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new System.Drawing.Size(458, 22);
			this.statusBar.TabIndex = 10;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Location = new System.Drawing.Point(8, 8);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(440, 408);
			this.tabControl1.TabIndex = 12;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.groupBox3);
			this.tabPage1.Controls.Add(this.groupBox1);
			this.tabPage1.Controls.Add(this.groupBox4);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(432, 382);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Setup Connection";
			// 
			// pbar
			// 
			this.pbar.Location = new System.Drawing.Point(200, 566);
			this.pbar.Name = "pbar";
			this.pbar.Size = new System.Drawing.Size(240, 8);
			this.pbar.Step = 1;
			this.pbar.TabIndex = 13;
			this.pbar.Visible = false;
			// 
			// ManagerMainForm
			// 
			this.AcceptButton = this.btStart;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(458, 579);
			this.Controls.Add(this.pbar);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.statusBar);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Menu = this.MainMenu;
			this.Name = "ManagerMainForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Alchemi Manager";
			this.Load += new System.EventHandler(this.ManagerMainForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		//-----------------------------------------------------------------------------------------------    
    
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
					if (_container != null && _container.Started)
					{
						started = true;
					}
				}
				return started;
			}
		}

		private void btStart_Click(object sender, EventArgs e)
		{
			if (isService)
			{
				StartManagerService();
			}
			else
			{
				StartManager();
			}
		}

		private void StartManagerService()
		{
//			//if the update process is going on, avoid starting the manager.
//			if (updating)
//			{
//				MessageBox.Show(this,"The manager is checking for updates. It can be started only after the updates are completed.","Update Alchemi Manager",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
//				return;
//			}

			if (Started)
			{
				Log("Manager Service is already started.");
				RefreshUIControls();
				return;
			}

			try
			{
				//to avoid people from clicking this again
				btStart.Enabled = false;
				statusBar.Text = "Starting Manager Service...";

				Log("Attempting to start Manager Service...");

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
					Log("Manager Service started.");
				}
			}
			catch (TimeoutException)
			{
				Log("Timeout expired trying to start Manager Service.");
			}
			catch (Exception ex)
			{
				Log("Error starting ManagerService");
				logger.Error("Error starting ManagerService",ex);
				StopManagerService();
			}
			RefreshUIControls();
		}

		private void StartManager()
		{
//			//if the update process is going on, avoid starting the manager.
//			if (updating)
//			{
//				MessageBox.Show(this,"The manager is checking for updates. It can be started only after the updates are completed.","Update Alchemi Manager",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
//				return;
//			}

			if (Started)
			{
				RefreshUIControls();
				return;
			}

			//to avoid people from clicking this again
			btStart.Enabled = false;
			btStop.Enabled = false;
			statusBar.Text = "Starting Manager...";

			Log("Attempting to start Manager...");

			pbar.Value = 0;
			pbar.Show();

			GetConfigFromUI();

			if (_container == null)
				_container = new ManagerContainer();
			_container.Config = Config;
			_container.RemotingConfigFile = "Alchemi.ManagerExec.exe.config";

			try
			{
				_container.Start();

				Log("Manager started");

				//for heirarchical stuff
				//				if (Config.Intermediate)
				//				{
				//					//Config.Id = Manager.Id;
				//					//Config.Dedicated = Manager.Dedicated;
				//				}

			}
			catch (Exception ex)
			{
				_container = null;
				string errorMsg = string.Format("Could not start Manager. Reason: {0}{1}", Environment.NewLine, ex.Message);
				if (ex.InnerException != null)
				{
					errorMsg += string.Format("{0}", ex.InnerException.Message);
				}
				Log(errorMsg);
				logger.Error(errorMsg,ex);
			}
			RefreshUIControls();
		}

		private void GetConfigFromUI()
		{
			if (Config == null)
			{
				Config = new Configuration();
			}
			Config.DbServer = txDbServer.Text;
			Config.DbUsername = txDbUsername.Text;
			Config.DbPassword = txDbPassword.Text;
			Config.DbName = txDbName.Text;

			Config.OwnPort = int.Parse(txOwnPort.Text);
			Config.ManagerHost = txManagerHost.Text;
			Config.ManagerPort = int.Parse(txManagerPort.Text);
			Config.Intermediate = cbIntermediate.Checked;
			Config.Dedicated = cbDedicated.Checked;
			Config.Id = txId.Text;
		}

		//-----------------------------------------------------------------------------------------------    

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
					logger.Warn(e.Message, e.Exception);
					break;
			}
		}

		void Log(string s)
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
			logger.Info(s);
		}
    
		//-----------------------------------------------------------------------------------------------    
    
		private void Exit()
		{
			if (!isService)
			{
				StopManager();
			}
			//we dont stop the service just because the serviceController is closed.

			this.Close();
			Application.Exit();
		}
    
		//-----------------------------------------------------------------------------------------------    
    
		private void StopManagerService()
		{
			if (!Started)
			{
				Log("The Manager Service is already stopped.");
				RefreshUIControls();
				return;
			}

			try
			{
				statusBar.Text = "Stopping Manager Service...";
				Log("Stopping Manager Service...");

				btStop.Enabled = false; //to avoid clicking on this again.
				ServiceController sc = new ServiceController(serviceName);
				if (sc.CanStop)
				{
					sc.Stop();
					sc.WaitForStatus(ServiceControllerStatus.Stopped,new TimeSpan(0,0,28));
					Log("Manager Service stopped.");
				}
				else
				{
					logger.Debug("Couldnot stop service: CanStop = false");	
				}
			}
			catch (TimeoutException)
			{
				Log("Timeout expired trying to stop Manager Service.");
			}
			catch (Exception ex)
			{
				Log("Error stopping ManagerService");
				logger.Error("Error stopping ManagerService",ex);
			}
			RefreshUIControls();
		}

		private void StopManager()
		{
			if (Started)
			{
				pbar.Value = 0;
				pbar.Show();
				pbar.Value = pbar.Maximum / 2;
				
				statusBar.Text = "Stopping Manager...";
				Log("Stopping Manager...");
				btStop.Enabled = false;
				btStart.Enabled = false;
				try
				{
					_container.Stop();
					_container = null;
				}
				catch (Exception ex)
				{
					logger.Error("Error stopping manager",ex);
				}

				pbar.Value = pbar.Maximum;
	
				Log("Manager stopped.");

			}

//			try
//			{
//				if (updater!=null)
//				{
//					updater.Dispose();	
//				}
//			}catch {}
			
			RefreshUIControls();

		}
    
		//-----------------------------------------------------------------------------------------------    
    
		private void ManagerMainForm_Load(object sender, EventArgs e)
		{
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(DefaultErrorHandler);
			Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

			// avoid multiple instances
			bool isOnlyInstance = false;
			Mutex mtx = new Mutex(true, "AlchemiManager_Mutex", out isOnlyInstance);
			if(!isOnlyInstance)
			{
				MessageBox.Show(this,"An instance of this application is already running. The program will now exit.", "Alchemi Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				Application.Exit();
			}

			if (isService)
			{
				//this is a service. just read the config.
				ManagerContainer mc = new ManagerContainer();
				mc.ReadConfig(false);
				Config = mc.Config;
				mc = null;
			}
			else
			{	//normal startup. not a service
				_container = new  ManagerContainer();
				_container.ReadConfig(false);
				Config = _container.Config;
			}
			RefreshUIControls();
			btStart.Focus();
		}
    
		//-----------------------------------------------------------------------------------------------    

		private void RefreshUIControls()
		{
			txDbServer.Text = Config.DbServer;
			txDbUsername.Text = Config.DbUsername;
			txDbPassword.Text = Config.DbPassword;
			txDbName.Text = Config.DbName;
			txOwnPort.Text = Config.OwnPort.ToString();
			cbDedicated.Checked = Config.Dedicated;
			txManagerHost.Text = Config.ManagerHost;
			txManagerPort.Text = Config.ManagerPort.ToString();

			txId.Text = Config.Id;
			cbIntermediate.Checked = Config.Intermediate;
      		
			//dont need to keep calling the property getter...since it queries the status on each call.
			bool started = this.Started;

			btStart.Enabled = !started;
			btReset.Enabled = !started;
			txDbServer.Enabled = !started;
			txDbUsername.Enabled = !started;
			txDbPassword.Enabled = !started;
			txDbName.Enabled = !started;
			txOwnPort.Enabled = !started;
			cbIntermediate.Enabled = false /* !started */; // <-- hierarchical grid disabled for now
			cbDedicated.Enabled = !started & cbIntermediate.Checked;
			txManagerHost.Enabled = !started & cbIntermediate.Checked;
			txManagerPort.Enabled = !started & cbIntermediate.Checked;
			btStop.Enabled = started;

			if (started)
			{
				statusBar.Text = "Manager Started.";
			}
			else
			{
				statusBar.Text = "Manager Stopped.";
			}

			pbar.Hide();
			pbar.Value = 0;
		}
    
		//-----------------------------------------------------------------------------------------------    
    
		private void btStop_Click(object sender, EventArgs e)
		{
			if (isService)
			{
				StopManagerService();
			}
			else
			{
				StopManager();
			}
		}

		//-----------------------------------------------------------------------------------------------    

		private void btReset_Click(object sender, EventArgs e)
		{
			//in case it is a service, the container would be null since we dont need it really.
			//but we still need to get the config from it, so create a new one and read the config.
			if (isService && _container==null)
			{
				ManagerContainer mc = new ManagerContainer();
				mc.ReadConfig(true);
				Config = mc.Config;
				mc = null;
			}
			else
			{
				_container.ReadConfig(true);
				Config = _container.Config;
			}
			RefreshUIControls();
		}

		//-----------------------------------------------------------------------------------------------    

		private void cbIntermediate_CheckedChanged(object sender, EventArgs e)
		{
			Config.Intermediate = cbIntermediate.Checked;
			_container.Config = Config;
			RefreshUIControls();
		}
    
		//-----------------------------------------------------------------------------------------------    

		private void tmExit_Click(object sender, EventArgs e)
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
				// 'x' button clicked .. minimise to system tray
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
    
		private void Restore()
		{
			this.WindowState = FormWindowState.Normal;
			this.Show();
			this.Activate();
		}

		//-----------------------------------------------------------------------------------------------    

		private void mmExit_Click(object sender, EventArgs e)
		{
			Exit();
		}

		//-----------------------------------------------------------------------------------------------    

		static void DefaultErrorHandler(object sender, UnhandledExceptionEventArgs args)
		{
			Exception e = (Exception) args.ExceptionObject;
			HandleAllUnknownErrors(sender.ToString(),e);
		}

		static void HandleAllUnknownErrors(string sender, Exception e)
		{
			logger.Error("Unknown Error from: " + sender,e);
		}

		//-----------------------------------------------------------------------------------------------    

		private void mmAbout_Click(object sender, EventArgs e)
		{
			new SplashScreen().ShowDialog();
		}

		private void txLog_DoubleClick(object sender, EventArgs e)
		{
			txLog.Clear();
		}

		private void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			HandleAllUnknownErrors(sender.ToString(),e.Exception);
		}

		public void Manager_StartStatusEvent(string msg, int percentDone)
		{
			statusBar.Text = msg;
			Log(msg);
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
				if ((percentDone + pbar.Value) <= pbar.Maximum)
				{
					pbar.Value = percentDone;
				}
				else
				{
					pbar.Value = pbar.Maximum;
				}
			}    	
		}

		#region "Updater Stuff (Commented out - not using it now)"
//		private void mnuUpdates_Click(object sender, EventArgs e)
//		{
//			updateManager();
//		}

//
//		//**************************************************************
//		// This event is called everytime the appupdater attempts to check for 
//		// an update.  You can hook this event and perform your own update check.
//		// Return a boolean value indicating whether an update was found.  
//		// Notes:  
//		//   * This event fires on the poller thread, so you can make network requests 
//		//     to check for updates & it will be done asyncronously.
//		//   * Because this event has a non void return value, it can only
//		//     be handled by one event handler.  It can not be multi-cast.
//		//**************************************************************
////		public bool updater_OnCheckForUpdate(object sender, EventArgs e)
////		{
////			//TODO : can implement an updates webservice... later on
////			return true;
////		}
//
//		//**************************************************************
//		// This event if fired whenever a new update is detected.
//		// The default behavior is to start the update download immediatly.
//		// However, in this case we have disabled that feature by setting
//		// the DownloadOnDetection property to false. In this case we'll ask 
//		// the user if they want to download the update & then use the ApplyUpdate()
//		// method if they wish too.  Note that this event will fire on the main
//		// UI thread to allow interaction with the app UI.  
//		//**************************************************************
//		public void updater_OnUpdateDetected(object sender, UpdateDetectedEventArgs e)
//		{
//			try
//			{
//				if (e.UpdateDetected)
//				{
//					Log("Manager updater: Found new updates...");
//				
//					DialogResult res = MessageBox.Show(this,"New updates were found. Click Yes to proceed with downloading the updates, No to cancel the download","Alchemi Manager",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
//					if (res == DialogResult.Yes)
//					{
//						//async method, starts the download 
//						updater.DownloadUpdate();
//					}
//					else
//					{ //user selected NO downloads...
//						updating = false;
//					}
//				}
//				else
//				{
//					Log("Manager updater: No new updates...update complete.");
//					MessageBox.Show(this,"No new updates found.","Alchemi Manager",MessageBoxButtons.OK,MessageBoxIcon.Information);
//					updating = false;
//				}
//			}
//			catch (Exception ex)
//			{
//				handleUpdaterError("Error in DoUpdateDetectedAction: update found = "+ e.UpdateDetected ,ex);
//			}
//		}
//
//		//**************************************************************
//		// This event if fired whenever a new update is complete.
//		// You could do whatever shut down logic your app needs to run
//		// here (ex. saving files).  The Exit() method will close
//		// the current app.
//		//**************************************************************		
//		public void updater_OnUpdateComplete(object sender, UpdateCompleteEventArgs e)
//		{
//			//If the udpate succeeded...
//			if ( e.UpdateSucceeded)
//			{
//				string message = "The updates were successfully completed. Do you want to start using the new udpate now?";		
//				if (MessageBox.Show(this,message,"Update Complete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
//				{
//					//The return code must be set to tell appstart to restart the app
//					//The app must shut itself down. Note:  Don't use methods like Environment.Exit that
//					//will shut down the entire process, when using appstart in appdomain mode.
//					this.returnCode = AppUpdater.RestartAppReturnValue;
//					Exit();
//				}
//			} 
//			else //If the update failed....
//			{
//				handleUpdaterError(e.ErrorMessage,e.FailureException);
//			}
//		}
//
//		private void updateManager()
//		{
//			if (updating)
//			{
//				MessageBox.Show(this,"Another update process is already in progress","Alchemi Manager", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
//				return;
//			}
//
//			try
//			{
//				if (Started){ //if manager is started then stop it
//					DialogResult res = MessageBox.Show(this,"The manager needs to be stopped before checking for updates. Click Yes to proceed, No to cancel checking for updates","Alchemi Manager",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
//					if (res == DialogResult.No)
//					{
//						return;
//					}
//					if (isService)
//					{
//						StopManagerService();
//					}
//					else
//					{
//						StopManager();
//					}
//				}
//				updating = true;
//
//				Log("Manager updater: Checking for updates...");
//
//				updater.CheckForUpdates();
//			}
//			catch (Exception ex)
//			{
//				handleUpdaterError("Error updating manager...",ex);
//			}
//		}
//		
//		private void updater_OnDownloadProgress(object sender, DownloadProgressEventArgs e)
//		{
//			try
//			{
//				pbar.Visible = true;
//				pbar.Value = (int)e.CurrentState;
//				statusBar.Text = "Downloading " + e.CurrentState + " %";
//			}catch {}
//		}
//
//		private void handleUpdaterError(string msg, Exception ex)
//		{
//			updating = false;
//			Log(msg + Environment.NewLine + ex.Message);
//			logger.Error("Error Updating Manager",ex) ;	
//			MessageBox.Show(this,"Error updating manager: " + ex.Message,"Alchemi Manager",MessageBoxButtons.OK,MessageBoxIcon.Error);
//			RefreshUIControls();
//		}

		#endregion


	}
}
