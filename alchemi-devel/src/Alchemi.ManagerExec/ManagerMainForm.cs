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
using System.Windows.Forms;
using System.Configuration;
using Alchemi.Core;
using Alchemi.Core.Manager;

namespace Alchemi.ManagerExec
{
    public class ManagerMainForm : Form
    {
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txLog;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txOwnPort;
        private System.Windows.Forms.TextBox txManagerHost;
        private System.Windows.Forms.TextBox txManagerPort;
        private System.Windows.Forms.TextBox txId;
        private System.Windows.Forms.CheckBox cbIntermediate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btStop;
        private System.Windows.Forms.Button btReset;
        private System.Windows.Forms.CheckBox cbDedicated;
        private System.Windows.Forms.NotifyIcon TrayIcon;
        private System.Windows.Forms.ContextMenu TrayMenu;
        private System.Windows.Forms.MenuItem tmExit;
        private System.Windows.Forms.MainMenu MainMenu;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem mmExit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txDbPassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txDbUsername;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txDbServer;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txDbName;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem mmAbout;
        
        private ManagerContainer _container = null;
        private Configuration Config;

        public ManagerMainForm()
        {
            InitializeComponent();
      
            ManagerContainer.Log += new LogEventHandler(Log);
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
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(48, 136);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "ManagerHost";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(72, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "OwnPort";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // txManagerHost
            // 
            this.txManagerHost.Location = new System.Drawing.Point(120, 136);
            this.txManagerHost.Name = "txManagerHost";
            this.txManagerHost.Size = new System.Drawing.Size(104, 20);
            this.txManagerHost.TabIndex = 8;
            this.txManagerHost.Text = "";
            // 
            // txOwnPort
            // 
            this.txOwnPort.Location = new System.Drawing.Point(120, 32);
            this.txOwnPort.Name = "txOwnPort";
            this.txOwnPort.Size = new System.Drawing.Size(104, 20);
            this.txOwnPort.TabIndex = 30;
            this.txOwnPort.Text = "";
            // 
            // btStart
            // 
            this.btStart.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btStart.Location = new System.Drawing.Point(64, 56);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(128, 23);
            this.btStart.TabIndex = 0;
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
            this.groupBox1.Location = new System.Drawing.Point(8, 104);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(384, 200);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Node Configuration";
            // 
            // cbDedicated
            // 
            this.cbDedicated.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbDedicated.Location = new System.Drawing.Point(120, 104);
            this.cbDedicated.Name = "cbDedicated";
            this.cbDedicated.Size = new System.Drawing.Size(88, 24);
            this.cbDedicated.TabIndex = 7;
            this.cbDedicated.Text = "Dedicated";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(96, 80);
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
            this.cbIntermediate.Location = new System.Drawing.Point(120, 56);
            this.cbIntermediate.Name = "cbIntermediate";
            this.cbIntermediate.Size = new System.Drawing.Size(88, 24);
            this.cbIntermediate.TabIndex = 5;
            this.cbIntermediate.TabStop = false;
            this.cbIntermediate.Text = "Intermediate";
            this.cbIntermediate.CheckedChanged += new System.EventHandler(this.cbIntermediate_CheckedChanged);
            // 
            // txId
            // 
            this.txId.Enabled = false;
            this.txId.Location = new System.Drawing.Point(120, 80);
            this.txId.Name = "txId";
            this.txId.Size = new System.Drawing.Size(240, 20);
            this.txId.TabIndex = 6;
            this.txId.Text = "";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(48, 168);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "ManagerPort";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // txManagerPort
            // 
            this.txManagerPort.Location = new System.Drawing.Point(120, 168);
            this.txManagerPort.Name = "txManagerPort";
            this.txManagerPort.Size = new System.Drawing.Size(104, 20);
            this.txManagerPort.TabIndex = 9;
            this.txManagerPort.Text = "";
            // 
            // btReset
            // 
            this.btReset.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btReset.Location = new System.Drawing.Point(64, 24);
            this.btReset.Name = "btReset";
            this.btReset.Size = new System.Drawing.Size(248, 23);
            this.btReset.TabIndex = 2;
            this.btReset.Text = "Reset";
            this.btReset.Click += new System.EventHandler(this.btReset_Click);
            // 
            // btStop
            // 
            this.btStop.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btStop.Location = new System.Drawing.Point(200, 56);
            this.btStop.Name = "btStop";
            this.btStop.Size = new System.Drawing.Size(112, 23);
            this.btStop.TabIndex = 1;
            this.btStop.Text = "Stop";
            this.btStop.Click += new System.EventHandler(this.btStop_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txLog);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Location = new System.Drawing.Point(8, 408);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(384, 96);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            // 
            // txLog
            // 
            this.txLog.Location = new System.Drawing.Point(8, 16);
            this.txLog.Multiline = true;
            this.txLog.Name = "txLog";
            this.txLog.ReadOnly = true;
            this.txLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txLog.Size = new System.Drawing.Size(368, 72);
            this.txLog.TabIndex = 0;
            this.txLog.Text = "";
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
            this.txDbPassword.TabIndex = 14;
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
            this.txDbUsername.TabIndex = 16;
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
            this.txDbServer.TabIndex = 18;
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
            this.txDbName.TabIndex = 20;
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
            this.groupBox3.Location = new System.Drawing.Point(8, 8);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(384, 88);
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
            this.groupBox4.Location = new System.Drawing.Point(8, 312);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(384, 96);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Actions";
            // 
            // ManagerMainForm
            // 
            this.AcceptButton = this.btStart;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(400, 515);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
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
            this.ResumeLayout(false);

        }
        #endregion

        [STAThread]
        static void Main() 
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.Run(new ManagerMainForm());
        }
    
        //-----------------------------------------------------------------------------------------------    
    
        private void btStart_Click(object sender, System.EventArgs e)
        {
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
      
            OwnEndPoint ownEP = new OwnEndPoint(
                Config.OwnPort,
                RemotingMechanism.TcpBinary
                );

            RemoteEndPoint remoteEP = null;

            if (Config.Intermediate)
            {
                remoteEP = new RemoteEndPoint(
                    Config.ManagerHost, 
                    Config.ManagerPort, 
                    RemotingMechanism.TcpBinary
                    );
            }

            try
            {
                Log("Attempting to start Manager.");
        
                // build sql server configuration string
                string sqlServerConnStr = string.Format(
                    "user id={1};password={2};initial catalog={3};data source={0};Connect Timeout=5; Max Pool Size=5; Min Pool Size=5",
                    Config.DbServer,
                    Config.DbUsername,
                    Config.DbPassword,
                    Config.DbName
                    );

                _container = new ManagerContainer(
                    remoteEP,
                    ownEP,
                    Config.Id,
                    Config.Dedicated,
                    sqlServerConnStr
                    );
                Log("Manager started.");

                if (Config.Intermediate)
                {
                    //Config.Id = Manager.Id;
                    //Config.Dedicated = Manager.Dedicated;
                }
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
            }

      
            RefreshUIControls();
            Config.Slz();
        }
    
        //-----------------------------------------------------------------------------------------------    
    
        void Log(string s)
        {
            txLog.AppendText(s + Environment.NewLine);
        }
    
        //-----------------------------------------------------------------------------------------------    
    
        private void Exit()
        {
            Stop();
            Config.Slz();
            this.Close();
            System.Windows.Forms.Application.Exit();
        }
    
        //-----------------------------------------------------------------------------------------------    
    
        private void Stop()
        {
            if (_container != null)
            {
                _container.Stop();
                _container = null;
                Log("Manager stopped.");
            }
        }
    
        //-----------------------------------------------------------------------------------------------    
    
        private void ManagerMainForm_Load(object sender, System.EventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(DefaultErrorHandler);
      
            ReadConfig(false);
            RefreshUIControls();
            btStart.Focus();
        }
    
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
      
            bool started = _container == null ? false : true;

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
        }
    
        //-----------------------------------------------------------------------------------------------    
    
        private void btStop_Click(object sender, System.EventArgs e)
        {
            Stop();
            _container = null;
            RefreshUIControls();
        }

        //-----------------------------------------------------------------------------------------------    

        private void btReset_Click(object sender, System.EventArgs e)
        {
            ReadConfig(true);
            RefreshUIControls();
        }

        //-----------------------------------------------------------------------------------------------    

        private void cbIntermediate_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.Intermediate = cbIntermediate.Checked;
            RefreshUIControls();
        }
    
        //-----------------------------------------------------------------------------------------------    

        private void tmExit_Click(object sender, System.EventArgs e)
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
    
        private void TrayIcon_Click(object sender, System.EventArgs e)
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

        private void mmExit_Click(object sender, System.EventArgs e)
        {
            Exit();
        }

        //-----------------------------------------------------------------------------------------------    

        static void DefaultErrorHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception) args.ExceptionObject;
            MessageBox.Show(e.ToString(), "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //-----------------------------------------------------------------------------------------------    

        private void mmAbout_Click(object sender, System.EventArgs e)
        {
            new SplashScreen().ShowDialog();
        }

    }
}
