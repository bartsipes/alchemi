#region Alchemi copyright notice
/*
  Alchemi [.NET Grid Computing Framework]
  http://www.alchemi.net
  
  Copyright (c) 2002-2004 Akshay Luther & 2003-2004 Rajkumar Buyya 
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Threading;
using System.Diagnostics;
using Microsoft.Win32;
using Alchemi.Core;
using Alchemi.Core.Executor;

namespace Alchemi.ExecutorExec
{
    public class ExecutorMainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.TextBox txLog;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.StatusBar statusBar1;
        private System.Windows.Forms.MainMenu MainMenu;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem mmExit;
        private System.Windows.Forms.NotifyIcon TrayIcon;
        private System.Windows.Forms.ContextMenu TrayMenu;
        private System.Windows.Forms.MenuItem tmExit;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem mmAbout;
        private System.Windows.Forms.Timer HideTimer;
        private Thread ReconnectThread = null;
        private Configuration Config;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btReset;
        private System.Windows.Forms.Button btDisconnect;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txMgrPort;
        private System.Windows.Forms.TextBox txMgrHost;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txOwnPort;
        private System.Windows.Forms.CheckBox cbDedicated;
        private System.Windows.Forms.TextBox txId;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btConnect;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btStopExec;
        private System.Windows.Forms.Button btStartExec;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txPassword;
        private System.Windows.Forms.TextBox txUsername;
        private GExecutor Executor = null;

        public ExecutorMainForm()
        {
            InitializeComponent();
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
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.TrayMenu = new System.Windows.Forms.ContextMenu();
            this.tmExit = new System.Windows.Forms.MenuItem();
            this.MainMenu = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.mmExit = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
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
            this.groupBox4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage2.SuspendLayout();
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
            this.txLog.Size = new System.Drawing.Size(424, 80);
            this.txLog.TabIndex = 1;
            this.txLog.Text = "";
            this.txLog.DoubleClick += new System.EventHandler(this.txLog_DoubleClick);
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 517);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(458, 22);
            this.statusBar1.TabIndex = 2;
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
                                                                                      this.mmAbout});
            this.menuItem2.Text = "Help";
            // 
            // mmAbout
            // 
            this.mmAbout.Index = 0;
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
            this.groupBox4.Size = new System.Drawing.Size(440, 104);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.ItemSize = new System.Drawing.Size(97, 18);
            this.tabControl1.Location = new System.Drawing.Point(8, 8);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(440, 400);
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
            this.tabPage1.Size = new System.Drawing.Size(432, 374);
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
            this.tabPage2.Size = new System.Drawing.Size(432, 374);
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
            // ExecutorMainForm
            // 
            this.AcceptButton = this.btConnect;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(458, 539);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.statusBar1);
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
            this.ResumeLayout(false);

        }
        #endregion

        [STAThread]
        static void Main() 
        {
            Application.EnableVisualStyles();
            Application.Run(new ExecutorMainForm());
        }

        private bool Connected
        {
            get { return (Executor == null? false : true);}
        }

        //-----------------------------------------------------------------------------------------------        
        //
        // Form Event Handlers
        //
        //-----------------------------------------------------------------------------------------------    
    
        private void ExecutorMainForm_Load(object sender, System.EventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(DefaultErrorHandler);
      
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
      
            // subscribe to events
            GExecutor.LogEvent += new LogEventHandler(this.Log);
            GExecutor.NonDedicatedExecutingStatusChanged += new NonDedicatedExecutingStatusChangedEventHandler(this.RefreshUIControls);
            GExecutor.GotDisconnected += new GotDisconnectedEventHandler(this.Executor_GotDisconnected);
      
            ReadConfig(false);
            RefreshUIControls();

            if (Config.ConnectVerified)
            {
                Log("Using last verified configuration ...");
                Connect();
                if (Connected)
                {
                    HideTimer.Enabled = true;
                }
            }
      
        }
    
        //-----------------------------------------------------------------------------------------------    
    
        private void btConnect_Click(object sender, System.EventArgs e)
        {
            Connect();
        }
    
        //-----------------------------------------------------------------------------------------------    
    
        private void mmExit_Click(object sender, System.EventArgs e)
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
    
        private void TrayIcon_Click(object sender, System.EventArgs e)
        {
            Restore();
        }

        //-----------------------------------------------------------------------------------------------    

        private void tmExit_Click(object sender, System.EventArgs e)
        {
            Exit();
        }

        //-----------------------------------------------------------------------------------------------    
    
        private void btStartExec_Click(object sender, System.EventArgs e)
        {
            Executor.StartNonDedicatedExecuting(1000);
        }

        //-----------------------------------------------------------------------------------------------    
    
        private void btStopExec_Click(object sender, System.EventArgs e)
        {
            Executor.StopNonDedicatedExecuting();
        }
    
        //-----------------------------------------------------------------------------------------------    

        private void mmAbout_Click(object sender, System.EventArgs e)
        {
            ShowSplashScreen();
        }

        //-----------------------------------------------------------------------------------------------    

        static void DefaultErrorHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception) args.ExceptionObject;
            if (e.GetType() == typeof(AppDomainUnloadedException))
            {
                // can't figure out why this exception is being thrown .. in any case, seems safe to ignore it
                return;
            }
            MessageBox.Show(e.ToString(), "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }                                   

        //-----------------------------------------------------------------------------------------------    
    
        private void HideTimer_Tick(object sender, System.EventArgs e)
        {
            Hide();
            HideTimer.Enabled = false;
        }

        //-----------------------------------------------------------------------------------------------        
        //
        // Core Methods
        //
        //-----------------------------------------------------------------------------------------------    

        private void ReadConfig(bool useDefault)
        {
            if (!useDefault)
            {
                try
                {
                    Config = Configuration.GetConfiguration();
                }
                catch (System.IO.FileNotFoundException)
                {
                    useDefault = true;
                }
            }

            if (useDefault)
            {
                Config = new Configuration();
            }
        }

        //-----------------------------------------------------------------------------------------------        

        private void Connect()
        {
            statusBar1.Text = "Connecting ...";

            // read config from ui controls
            Config.ManagerHost = txMgrHost.Text;
            Config.ManagerPort = int.Parse(txMgrPort.Text);
            Config.OwnPort = int.Parse(txOwnPort.Text);
            Config.Dedicated = cbDedicated.Checked;

            
            RemoteEndPoint rep = new RemoteEndPoint(
                Config.ManagerHost,
                Config.ManagerPort,
                RemotingMechanism.TcpBinary
                );
            
            OwnEndPoint oep = new OwnEndPoint(
                Config.OwnPort,
                RemotingMechanism.TcpBinary
                );

            try
            {
                // connect to manager
                Executor = new GExecutor(rep, oep, Config.Id, Config.Dedicated, new SecurityCredentials(Config.Username, Config.Password), "");
                Log("Connected successfully.");

                Config.ConnectVerified = true;
                Config.Id = Executor.Id;
                Config.Dedicated = Executor.Dedicated;
                Config.ConnectVerified = true;
            }
            catch (RemotingException e)
            {
                Executor = null;
                Log("Error: " + e.Message);
            }
    
            Config.Slz();
            RefreshUIControls();
        }
    
        //-----------------------------------------------------------------------------------------------                

        private void RefreshUIControls()
        {
            txMgrHost.Text = Config.ManagerHost;
            txMgrPort.Text = Config.ManagerPort.ToString();
            txId.Text = Config.Id;
            txOwnPort.Text = Config.OwnPort.ToString();
            txUsername.Text = Config.Username;
            txPassword.Text = Config.Password;
            cbDedicated.Checked = Config.Dedicated;
      
            btConnect.Enabled = !Connected;
            btReset.Enabled = !Connected;
            btDisconnect.Enabled = !btConnect.Enabled;

            txMgrHost.Enabled = !Connected;
            txMgrPort.Enabled = !Connected;
            txUsername.Enabled = !Connected;
            txPassword.Enabled = !Connected;
            txOwnPort.Enabled = !Connected;
            cbDedicated.Enabled = !Connected;

            bool executing = false;
            if (Connected)
            {
                if (Executor.ExecutingNonDedicated)
                {
                    executing = true;
                }
            }
      
            btStartExec.Enabled = ((!Config.Dedicated) & (Connected) & (!executing));
            btStopExec.Enabled = ((!Config.Dedicated) & (Connected) & (executing));

            string sbAppend = (Config.Dedicated? " (dedicated)" : " (non-dedicated)");
      
            if ((Connected & executing) | (Connected & Config.Dedicated))
            {
                statusBar1.Text = "Executing" + sbAppend;
            }
            else if(Connected)
            {
                statusBar1.Text = "Connected" + sbAppend;
            }
            else
            {
                statusBar1.Text = "Disconnected";
            }
        }
    
        //-----------------------------------------------------------------------------------------------    

        private void Disconnect()
        {
            Executor.Disconnect();
            Executor = null;
            RefreshUIControls();
            Log("Disconnected successfully.");
        }

        //-----------------------------------------------------------------------------------------------            

        private void Log(string s)
        {
            if (txLog != null)
            {
                txLog.AppendText(s + Environment.NewLine);
            }
        }

        private void Log(object sender, string s)
        {
            Log(s);
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
            Config.Slz();
            if (Executor != null)
            {
                Disconnect();
            }
            this.Close();
            Application.Exit();
        }
    
        //-----------------------------------------------------------------------------------------------    

        private void Executor_GotDisconnected()
        {
            Executor = null;
            RefreshUIControls();
            Log("Got disconnected! Starting thread to try and reconnect.");
            ReconnectThread = new Thread(new ThreadStart(Reconnect));
            ReconnectThread.Start();
        }
    
        //-----------------------------------------------------------------------------------------------    
    
        private void btDisconnect_Click(object sender, System.EventArgs e)
        {
            Disconnect();
        }
    
        //-----------------------------------------------------------------------------------------------    

        private void Reconnect()
        {
            while (true)
            {
                Log("Attempting to reconnect ...");
                Connect();
                if (Executor == null)
                {
                    Thread.Sleep(5000);
                }
                else
                {
                    break;
                }
            }
            Log("Reconnected sucessfully.");
            Executor.StartNonDedicatedExecuting(1000);
        }
    
        //-----------------------------------------------------------------------------------------------    

        private void btReset_Click(object sender, System.EventArgs e)
        {
            ReadConfig(true);
            RefreshUIControls();
        }

        //-----------------------------------------------------------------------------------------------    

        private void ShowSplashScreen()
        {
            SplashScreen ss = new SplashScreen();
            ss.ShowDialog();
        }

        private void txLog_DoubleClick(object sender, System.EventArgs e)
        {
            txLog.Clear();
        }
    }
}