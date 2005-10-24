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

// Julio Martinez (jumaga2015@users.sourceforge.net)
//  - Using open source "ScPl" charting control instead of "Dundas Chart"

using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using Alchemi.Core;
using Alchemi.Core.Owner;
using Alchemi.Core.Manager.Storage;
using System.Xml;
using System.IO;
using System.Drawing.Drawing2D;
using NPlot;

namespace Alchemi.Console
{
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.TabControl tabControl1;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.DataGrid dgUsers;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.DataGrid dgExecutors;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbTotal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbUsage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbAvail;
        private System.Windows.Forms.Label lbNumExec;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbUnfinishedThreads;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem mmConnect;
        private System.Windows.Forms.Timer tmRefreshSystem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.MenuItem mmDisconnect;
        private System.Windows.Forms.Button btLoadUsers;
        private System.Windows.Forms.DataGrid dgApps;
        private System.Windows.Forms.Button btLoadApps;
        private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
        private System.Windows.Forms.StatusBar statusBar;
        private System.Windows.Forms.Button btLoadExecutors;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbTotalPowerUsage;
        bool connected = false;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem mmAbout;
        private System.Windows.Forms.Button btStopApps;
        private System.Windows.Forms.ToolBarButton toolBarButton1;
        private System.Windows.Forms.ToolBarButton toolBarButton2;
        private scpl.Windows.PlotSurface2D plotSurface;
		private static GApplication ga;

		private delegate void NPlotChartDelegate();
		private NPlotChartDelegate [] PlotRoutines;
		//private int chartID = 0;
        
        ConsoleNode console;

		// Create a logger for use in this class
		private static readonly Alchemi.Core.Logger logger = new Logger();

        private scpl.LinePlot lineAvail = new scpl.LinePlot();
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Button btnExeFile;
		private System.Windows.Forms.TextBox txtExeFile;
		private System.Windows.Forms.Label lblExeFile;
		private System.Windows.Forms.OpenFileDialog ofdExeFile;
		private System.Windows.Forms.Label lblInput;
		private System.Windows.Forms.Label lblOutput;
		private System.Windows.Forms.GroupBox line;
		private System.Windows.Forms.TextBox txtInputFile;
		private System.Windows.Forms.Button btnInputFile;
		private System.Windows.Forms.OpenFileDialog ofdInputFile;
		private System.Windows.Forms.TextBox txtOutputFile;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.DataGrid dgdJobList;
		private System.Data.DataSet dstJobList;
		private System.Data.DataTable dtbJoblist;
		private System.Data.DataColumn colJobID;
		private System.Data.DataColumn colInputFile;
		private System.Data.DataColumn colOutputFile;
		private System.Windows.Forms.TextBox txtRunCommand;
		private System.Windows.Forms.Label lblRunCommand;
		private System.Data.DataColumn colRunCommand;
		private System.Windows.Forms.Label lblJobID;
		private System.Windows.Forms.TextBox txtJobID;
		private System.Windows.Forms.Button btnUpdateJob;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btStopJobs;
        private System.Windows.Forms.DataGrid dgJobs;
		private System.Windows.Forms.Button btnSaveXML;
		private System.Windows.Forms.Button btnDeleteJob;
		private System.Windows.Forms.Button btnLoadXML;
		private System.Windows.Forms.Button btnNewApp;
		private System.Windows.Forms.Button btnSubmit;
		private System.Windows.Forms.OpenFileDialog ofdXMLFile;
		private System.Windows.Forms.Panel palJobSubmit;
		private System.Windows.Forms.Label lblJobSubmit;
		private System.Windows.Forms.ProgressBar prbJobSubmit;
		private System.Windows.Forms.Label lblPercentage;
		private System.Windows.Forms.SaveFileDialog sfdXMLFile;
		private System.Windows.Forms.TabPage tabPage6;
		private NPlot.Windows.PlotSurface2D nplotSurface;
		private System.Windows.Forms.ComboBox cobAppList;
		private System.Windows.Forms.GroupBox groupBox7;
		private System.Windows.Forms.Panel palJobByApp;
		private System.Windows.Forms.ComboBox cobChart;
		private System.Windows.Forms.Label lblChart;
		private System.Windows.Forms.GroupBox groupBox8;
		private System.Windows.Forms.Label lblAppID;
		private System.Windows.Forms.Button btnAppRefresh;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn1;
		private System.Windows.Forms.Label lblMax;
		private System.Windows.Forms.Label lblMin;
		private System.Windows.Forms.Label lblAvg;
		private System.Windows.Forms.Label lblApp;
		private System.Windows.Forms.Label lblJobNum;
		private System.Windows.Forms.Panel palJobByAppExe;
		private System.Windows.Forms.Label lblAppName1;
		private System.Windows.Forms.Label lblAppID1;
		private System.Windows.Forms.ComboBox cobAppID1;
		private System.Windows.Forms.Label lblSchedule;
		private System.Windows.Forms.Label lblReady;
		private System.Windows.Forms.Label lblFinished;
		private System.Windows.Forms.Label lblStarted;
		private System.Windows.Forms.Label lblDead;
		private System.Windows.Forms.Button btnAppLoad;
		private System.Windows.Forms.Panel palJobByExec;
		private System.Windows.Forms.Button btnExecLoad;
		private System.Windows.Forms.Label lblHost;
		private System.Windows.Forms.ComboBox cobExecutorID;
		private System.Windows.Forms.Label lblExecutorID;
		private System.Windows.Forms.Label lblThreadStatus;
		private System.Windows.Forms.Label lblCpuMax;
		private System.Windows.Forms.Label lblCpuUsage;
		private System.Windows.Forms.Label lblCpuAvail;
		private System.Windows.Forms.Label lblCpuTotal;
		private System.Windows.Forms.Label lblCpuStatus;
		private System.Windows.Forms.MenuItem mnuExit;
		private System.Windows.Forms.Button btnCancelAppSubmit;
		private System.Windows.Forms.TabPage tabSystem;
		private System.Windows.Forms.TabPage tabUsers;
		private System.Windows.Forms.TabPage tabExecutors;
		private System.Windows.Forms.TabPage tabAppSubmission;
		private System.Windows.Forms.TabPage tabApplications;
        private scpl.LinePlot lineUsage = new scpl.LinePlot();

        
        public MainForm()
        {
            InitializeComponent();
        }

        [STAThread]
        static void Main() 
        {
            Application.EnableVisualStyles();
            Application.Run(new MainForm());
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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabSystem = new System.Windows.Forms.TabPage();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.plotSurface = new scpl.Windows.PlotSurface2D();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lbTotalPowerUsage = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.lbTotal = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.lbUsage = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.lbAvail = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.lbNumExec = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.lbUnfinishedThreads = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.tabUsers = new System.Windows.Forms.TabPage();
			this.btSave = new System.Windows.Forms.Button();
			this.dgUsers = new System.Windows.Forms.DataGrid();
			this.btLoadUsers = new System.Windows.Forms.Button();
			this.tabExecutors = new System.Windows.Forms.TabPage();
			this.btLoadExecutors = new System.Windows.Forms.Button();
			this.dgExecutors = new System.Windows.Forms.DataGrid();
			this.tabAppSubmission = new System.Windows.Forms.TabPage();
			this.btnCancelAppSubmit = new System.Windows.Forms.Button();
			this.btnNewApp = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.palJobSubmit = new System.Windows.Forms.Panel();
			this.lblPercentage = new System.Windows.Forms.Label();
			this.prbJobSubmit = new System.Windows.Forms.ProgressBar();
			this.lblJobSubmit = new System.Windows.Forms.Label();
			this.btnSubmit = new System.Windows.Forms.Button();
			this.btnDeleteJob = new System.Windows.Forms.Button();
			this.btnSaveXML = new System.Windows.Forms.Button();
			this.btnUpdateJob = new System.Windows.Forms.Button();
			this.txtJobID = new System.Windows.Forms.TextBox();
			this.lblJobID = new System.Windows.Forms.Label();
			this.txtRunCommand = new System.Windows.Forms.TextBox();
			this.lblRunCommand = new System.Windows.Forms.Label();
			this.dgdJobList = new System.Windows.Forms.DataGrid();
			this.dstJobList = new System.Data.DataSet();
			this.dtbJoblist = new System.Data.DataTable();
			this.colJobID = new System.Data.DataColumn();
			this.colInputFile = new System.Data.DataColumn();
			this.colOutputFile = new System.Data.DataColumn();
			this.colRunCommand = new System.Data.DataColumn();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.btnAdd = new System.Windows.Forms.Button();
			this.txtOutputFile = new System.Windows.Forms.TextBox();
			this.btnInputFile = new System.Windows.Forms.Button();
			this.txtInputFile = new System.Windows.Forms.TextBox();
			this.line = new System.Windows.Forms.GroupBox();
			this.lblOutput = new System.Windows.Forms.Label();
			this.lblInput = new System.Windows.Forms.Label();
			this.btnExeFile = new System.Windows.Forms.Button();
			this.txtExeFile = new System.Windows.Forms.TextBox();
			this.lblExeFile = new System.Windows.Forms.Label();
			this.btnLoadXML = new System.Windows.Forms.Button();
			this.tabApplications = new System.Windows.Forms.TabPage();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.btStopJobs = new System.Windows.Forms.Button();
			this.dgJobs = new System.Windows.Forms.DataGrid();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.dgApps = new System.Windows.Forms.DataGrid();
			this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
			this.dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.btLoadApps = new System.Windows.Forms.Button();
			this.btStopApps = new System.Windows.Forms.Button();
			this.tabPage6 = new System.Windows.Forms.TabPage();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.groupBox8 = new System.Windows.Forms.GroupBox();
			this.lblChart = new System.Windows.Forms.Label();
			this.cobChart = new System.Windows.Forms.ComboBox();
			this.palJobByApp = new System.Windows.Forms.Panel();
			this.lblJobNum = new System.Windows.Forms.Label();
			this.lblApp = new System.Windows.Forms.Label();
			this.lblAvg = new System.Windows.Forms.Label();
			this.lblMin = new System.Windows.Forms.Label();
			this.lblMax = new System.Windows.Forms.Label();
			this.btnAppRefresh = new System.Windows.Forms.Button();
			this.lblAppID = new System.Windows.Forms.Label();
			this.cobAppList = new System.Windows.Forms.ComboBox();
			this.palJobByExec = new System.Windows.Forms.Panel();
			this.lblCpuStatus = new System.Windows.Forms.Label();
			this.lblCpuTotal = new System.Windows.Forms.Label();
			this.lblCpuAvail = new System.Windows.Forms.Label();
			this.lblCpuUsage = new System.Windows.Forms.Label();
			this.lblCpuMax = new System.Windows.Forms.Label();
			this.btnExecLoad = new System.Windows.Forms.Button();
			this.lblHost = new System.Windows.Forms.Label();
			this.cobExecutorID = new System.Windows.Forms.ComboBox();
			this.lblExecutorID = new System.Windows.Forms.Label();
			this.palJobByAppExe = new System.Windows.Forms.Panel();
			this.lblThreadStatus = new System.Windows.Forms.Label();
			this.lblDead = new System.Windows.Forms.Label();
			this.lblFinished = new System.Windows.Forms.Label();
			this.lblAppName1 = new System.Windows.Forms.Label();
			this.lblStarted = new System.Windows.Forms.Label();
			this.lblSchedule = new System.Windows.Forms.Label();
			this.lblReady = new System.Windows.Forms.Label();
			this.btnAppLoad = new System.Windows.Forms.Button();
			this.lblAppID1 = new System.Windows.Forms.Label();
			this.cobAppID1 = new System.Windows.Forms.ComboBox();
			this.nplotSurface = new NPlot.Windows.PlotSurface2D();
			this.tmRefreshSystem = new System.Windows.Forms.Timer(this.components);
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.mmConnect = new System.Windows.Forms.MenuItem();
			this.mmDisconnect = new System.Windows.Forms.MenuItem();
			this.mnuExit = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.mmAbout = new System.Windows.Forms.MenuItem();
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
			this.ofdExeFile = new System.Windows.Forms.OpenFileDialog();
			this.ofdInputFile = new System.Windows.Forms.OpenFileDialog();
			this.ofdXMLFile = new System.Windows.Forms.OpenFileDialog();
			this.sfdXMLFile = new System.Windows.Forms.SaveFileDialog();
			this.tabControl1.SuspendLayout();
			this.tabSystem.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabUsers.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgUsers)).BeginInit();
			this.tabExecutors.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgExecutors)).BeginInit();
			this.tabAppSubmission.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.palJobSubmit.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgdJobList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dstJobList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dtbJoblist)).BeginInit();
			this.tabApplications.SuspendLayout();
			this.groupBox6.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgJobs)).BeginInit();
			this.groupBox5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgApps)).BeginInit();
			this.tabPage6.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.palJobByApp.SuspendLayout();
			this.palJobByExec.SuspendLayout();
			this.palJobByAppExe.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabSystem);
			this.tabControl1.Controls.Add(this.tabUsers);
			this.tabControl1.Controls.Add(this.tabExecutors);
			this.tabControl1.Controls.Add(this.tabAppSubmission);
			this.tabControl1.Controls.Add(this.tabApplications);
			this.tabControl1.Controls.Add(this.tabPage6);
			this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.tabControl1.Location = new System.Drawing.Point(8, 8);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(702, 422);
			this.tabControl1.TabIndex = 0;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// tabSystem
			// 
			this.tabSystem.Controls.Add(this.groupBox2);
			this.tabSystem.Controls.Add(this.groupBox1);
			this.tabSystem.Location = new System.Drawing.Point(4, 22);
			this.tabSystem.Name = "tabSystem";
			this.tabSystem.Size = new System.Drawing.Size(694, 396);
			this.tabSystem.TabIndex = 2;
			this.tabSystem.Text = "System";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.plotSurface);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(16, 24);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(480, 350);
			this.groupBox2.TabIndex = 17;
			this.groupBox2.TabStop = false;
			// 
			// plotSurface
			// 
			this.plotSurface.AllowSelection = false;
			this.plotSurface.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.plotSurface.BackColor = System.Drawing.SystemColors.Control;
			this.plotSurface.Legend = null;
			this.plotSurface.Location = new System.Drawing.Point(8, 8);
			this.plotSurface.Name = "plotSurface";
			this.plotSurface.Padding = 10;
			this.plotSurface.PlotBackColor = System.Drawing.SystemColors.Control;
			this.plotSurface.ShowCoordinates = false;
			this.plotSurface.Size = new System.Drawing.Size(464, 332);
			this.plotSurface.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			this.plotSurface.TabIndex = 16;
			this.plotSurface.Title = "";
			this.plotSurface.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
			this.plotSurface.XAxis1 = null;
			this.plotSurface.XAxis2 = null;
			this.plotSurface.YAxis1 = null;
			this.plotSurface.YAxis2 = null;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.lbTotalPowerUsage);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.lbTotal);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.lbUsage);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.lbAvail);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.lbNumExec);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.lbUnfinishedThreads);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(504, 24);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(167, 350);
			this.groupBox1.TabIndex = 16;
			this.groupBox1.TabStop = false;
			// 
			// lbTotalPowerUsage
			// 
			this.lbTotalPowerUsage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lbTotalPowerUsage.Location = new System.Drawing.Point(20, 96);
			this.lbTotalPowerUsage.Name = "lbTotalPowerUsage";
			this.lbTotalPowerUsage.Size = new System.Drawing.Size(132, 23);
			this.lbTotalPowerUsage.TabIndex = 18;
			this.lbTotalPowerUsage.Text = "_";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(13, 80);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(140, 16);
			this.label6.TabIndex = 17;
			this.label6.Text = "Total Power Usage";
			// 
			// lbTotal
			// 
			this.lbTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lbTotal.Location = new System.Drawing.Point(20, 40);
			this.lbTotal.Name = "lbTotal";
			this.lbTotal.Size = new System.Drawing.Size(133, 23);
			this.lbTotal.TabIndex = 4;
			this.lbTotal.Text = "_";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 136);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(128, 16);
			this.label5.TabIndex = 9;
			this.label5.Text = "No. of Executors";
			// 
			// lbUsage
			// 
			this.lbUsage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lbUsage.Location = new System.Drawing.Point(16, 264);
			this.lbUsage.Name = "lbUsage";
			this.lbUsage.Size = new System.Drawing.Size(136, 23);
			this.lbUsage.TabIndex = 8;
			this.lbUsage.Text = "_";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 248);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(128, 16);
			this.label4.TabIndex = 7;
			this.label4.Text = "Current Power Usage";
			// 
			// lbAvail
			// 
			this.lbAvail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lbAvail.Location = new System.Drawing.Point(16, 208);
			this.lbAvail.Name = "lbAvail";
			this.lbAvail.Size = new System.Drawing.Size(136, 23);
			this.lbAvail.TabIndex = 6;
			this.lbAvail.Text = "_";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 192);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(128, 16);
			this.label3.TabIndex = 5;
			this.label3.Text = "Current Power Available";
			// 
			// lbNumExec
			// 
			this.lbNumExec.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lbNumExec.Location = new System.Drawing.Point(16, 152);
			this.lbNumExec.Name = "lbNumExec";
			this.lbNumExec.Size = new System.Drawing.Size(136, 23);
			this.lbNumExec.TabIndex = 10;
			this.lbNumExec.Text = "_";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(128, 16);
			this.label1.TabIndex = 3;
			this.label1.Text = "Max Power Available";
			// 
			// lbUnfinishedThreads
			// 
			this.lbUnfinishedThreads.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lbUnfinishedThreads.Location = new System.Drawing.Point(16, 320);
			this.lbUnfinishedThreads.Name = "lbUnfinishedThreads";
			this.lbUnfinishedThreads.Size = new System.Drawing.Size(136, 23);
			this.lbUnfinishedThreads.TabIndex = 14;
			this.lbUnfinishedThreads.Text = "_";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(16, 304);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(128, 16);
			this.label8.TabIndex = 13;
			this.label8.Text = "Unfinished Threads";
			// 
			// tabUsers
			// 
			this.tabUsers.Controls.Add(this.btSave);
			this.tabUsers.Controls.Add(this.dgUsers);
			this.tabUsers.Controls.Add(this.btLoadUsers);
			this.tabUsers.Location = new System.Drawing.Point(4, 22);
			this.tabUsers.Name = "tabUsers";
			this.tabUsers.Size = new System.Drawing.Size(694, 396);
			this.tabUsers.TabIndex = 0;
			this.tabUsers.Text = "Users";
			// 
			// btSave
			// 
			this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btSave.Location = new System.Drawing.Point(582, 366);
			this.btSave.Name = "btSave";
			this.btSave.Size = new System.Drawing.Size(104, 23);
			this.btSave.TabIndex = 3;
			this.btSave.Text = "Save";
			this.btSave.Click += new System.EventHandler(this.btSave_Click);
			// 
			// dgUsers
			// 
			this.dgUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.dgUsers.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.dgUsers.CaptionText = "Users";
			this.dgUsers.DataMember = "";
			this.dgUsers.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dgUsers.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgUsers.Location = new System.Drawing.Point(8, 8);
			this.dgUsers.Name = "dgUsers";
			this.dgUsers.PreferredColumnWidth = 100;
			this.dgUsers.Size = new System.Drawing.Size(678, 350);
			this.dgUsers.TabIndex = 2;
			// 
			// btLoadUsers
			// 
			this.btLoadUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btLoadUsers.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btLoadUsers.Location = new System.Drawing.Point(8, 366);
			this.btLoadUsers.Name = "btLoadUsers";
			this.btLoadUsers.Size = new System.Drawing.Size(104, 23);
			this.btLoadUsers.TabIndex = 1;
			this.btLoadUsers.Text = "Load";
			this.btLoadUsers.Click += new System.EventHandler(this.btLoadUsers_Click);
			// 
			// tabExecutors
			// 
			this.tabExecutors.Controls.Add(this.btLoadExecutors);
			this.tabExecutors.Controls.Add(this.dgExecutors);
			this.tabExecutors.Location = new System.Drawing.Point(4, 22);
			this.tabExecutors.Name = "tabExecutors";
			this.tabExecutors.Size = new System.Drawing.Size(694, 396);
			this.tabExecutors.TabIndex = 1;
			this.tabExecutors.Text = "Executors";
			// 
			// btLoadExecutors
			// 
			this.btLoadExecutors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btLoadExecutors.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btLoadExecutors.Location = new System.Drawing.Point(8, 366);
			this.btLoadExecutors.Name = "btLoadExecutors";
			this.btLoadExecutors.Size = new System.Drawing.Size(104, 23);
			this.btLoadExecutors.TabIndex = 4;
			this.btLoadExecutors.Text = "Load";
			this.btLoadExecutors.Click += new System.EventHandler(this.btLoadExecutors_Click);
			// 
			// dgExecutors
			// 
			this.dgExecutors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.dgExecutors.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.dgExecutors.CaptionText = "Executors";
			this.dgExecutors.DataMember = "";
			this.dgExecutors.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dgExecutors.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgExecutors.Location = new System.Drawing.Point(8, 8);
			this.dgExecutors.Name = "dgExecutors";
			this.dgExecutors.PreferredColumnWidth = 100;
			this.dgExecutors.ReadOnly = true;
			this.dgExecutors.Size = new System.Drawing.Size(678, 350);
			this.dgExecutors.TabIndex = 3;
			// 
			// tabAppSubmission
			// 
			this.tabAppSubmission.Controls.Add(this.btnCancelAppSubmit);
			this.tabAppSubmission.Controls.Add(this.btnNewApp);
			this.tabAppSubmission.Controls.Add(this.groupBox3);
			this.tabAppSubmission.Controls.Add(this.btnLoadXML);
			this.tabAppSubmission.Location = new System.Drawing.Point(4, 22);
			this.tabAppSubmission.Name = "tabAppSubmission";
			this.tabAppSubmission.Size = new System.Drawing.Size(694, 396);
			this.tabAppSubmission.TabIndex = 4;
			this.tabAppSubmission.Text = "Application Submission";
			// 
			// btnCancelAppSubmit
			// 
			this.btnCancelAppSubmit.Location = new System.Drawing.Point(448, 8);
			this.btnCancelAppSubmit.Name = "btnCancelAppSubmit";
			this.btnCancelAppSubmit.TabIndex = 29;
			this.btnCancelAppSubmit.Text = "Cancel";
			this.btnCancelAppSubmit.Visible = false;
			this.btnCancelAppSubmit.Click += new System.EventHandler(this.btnCancelAppSubmit_Click);
			// 
			// btnNewApp
			// 
			this.btnNewApp.Location = new System.Drawing.Point(24, 8);
			this.btnNewApp.Name = "btnNewApp";
			this.btnNewApp.Size = new System.Drawing.Size(144, 23);
			this.btnNewApp.TabIndex = 28;
			this.btnNewApp.Text = "Create a New Application";
			this.btnNewApp.Click += new System.EventHandler(this.btnNewApp_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.palJobSubmit);
			this.groupBox3.Controls.Add(this.btnSubmit);
			this.groupBox3.Controls.Add(this.btnDeleteJob);
			this.groupBox3.Controls.Add(this.btnSaveXML);
			this.groupBox3.Controls.Add(this.btnUpdateJob);
			this.groupBox3.Controls.Add(this.txtJobID);
			this.groupBox3.Controls.Add(this.lblJobID);
			this.groupBox3.Controls.Add(this.txtRunCommand);
			this.groupBox3.Controls.Add(this.lblRunCommand);
			this.groupBox3.Controls.Add(this.dgdJobList);
			this.groupBox3.Controls.Add(this.groupBox4);
			this.groupBox3.Controls.Add(this.btnAdd);
			this.groupBox3.Controls.Add(this.txtOutputFile);
			this.groupBox3.Controls.Add(this.btnInputFile);
			this.groupBox3.Controls.Add(this.txtInputFile);
			this.groupBox3.Controls.Add(this.line);
			this.groupBox3.Controls.Add(this.lblOutput);
			this.groupBox3.Controls.Add(this.lblInput);
			this.groupBox3.Controls.Add(this.btnExeFile);
			this.groupBox3.Controls.Add(this.txtExeFile);
			this.groupBox3.Controls.Add(this.lblExeFile);
			this.groupBox3.Enabled = false;
			this.groupBox3.Location = new System.Drawing.Point(8, 40);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(680, 352);
			this.groupBox3.TabIndex = 0;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "App. Parameters";
			// 
			// palJobSubmit
			// 
			this.palJobSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.palJobSubmit.Controls.Add(this.lblPercentage);
			this.palJobSubmit.Controls.Add(this.prbJobSubmit);
			this.palJobSubmit.Controls.Add(this.lblJobSubmit);
			this.palJobSubmit.Location = new System.Drawing.Point(152, 96);
			this.palJobSubmit.Name = "palJobSubmit";
			this.palJobSubmit.Size = new System.Drawing.Size(392, 32);
			this.palJobSubmit.TabIndex = 28;
			this.palJobSubmit.Visible = false;
			// 
			// lblPercentage
			// 
			this.lblPercentage.Location = new System.Drawing.Point(344, 8);
			this.lblPercentage.Name = "lblPercentage";
			this.lblPercentage.Size = new System.Drawing.Size(40, 16);
			this.lblPercentage.TabIndex = 2;
			this.lblPercentage.Text = "0%";
			this.lblPercentage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// prbJobSubmit
			// 
			this.prbJobSubmit.Location = new System.Drawing.Point(80, 8);
			this.prbJobSubmit.Name = "prbJobSubmit";
			this.prbJobSubmit.Size = new System.Drawing.Size(264, 16);
			this.prbJobSubmit.TabIndex = 1;
			// 
			// lblJobSubmit
			// 
			this.lblJobSubmit.Location = new System.Drawing.Point(8, 8);
			this.lblJobSubmit.Name = "lblJobSubmit";
			this.lblJobSubmit.Size = new System.Drawing.Size(64, 16);
			this.lblJobSubmit.TabIndex = 0;
			this.lblJobSubmit.Text = "Job Submit:";
			this.lblJobSubmit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// btnSubmit
			// 
			this.btnSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSubmit.Location = new System.Drawing.Point(16, 320);
			this.btnSubmit.Name = "btnSubmit";
			this.btnSubmit.Size = new System.Drawing.Size(80, 23);
			this.btnSubmit.TabIndex = 27;
			this.btnSubmit.Text = "Submit";
			this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
			// 
			// btnDeleteJob
			// 
			this.btnDeleteJob.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.btnDeleteJob.Location = new System.Drawing.Point(440, 168);
			this.btnDeleteJob.Name = "btnDeleteJob";
			this.btnDeleteJob.Size = new System.Drawing.Size(72, 23);
			this.btnDeleteJob.TabIndex = 26;
			this.btnDeleteJob.Text = "Delete Job";
			this.btnDeleteJob.Click += new System.EventHandler(this.btnDeleteJob_Click);
			// 
			// btnSaveXML
			// 
			this.btnSaveXML.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.btnSaveXML.Location = new System.Drawing.Point(176, 168);
			this.btnSaveXML.Name = "btnSaveXML";
			this.btnSaveXML.Size = new System.Drawing.Size(72, 23);
			this.btnSaveXML.TabIndex = 25;
			this.btnSaveXML.Text = "Save XML";
			this.btnSaveXML.Click += new System.EventHandler(this.btnSaveXML_Click);
			// 
			// btnUpdateJob
			// 
			this.btnUpdateJob.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.btnUpdateJob.Location = new System.Drawing.Point(352, 168);
			this.btnUpdateJob.Name = "btnUpdateJob";
			this.btnUpdateJob.Size = new System.Drawing.Size(72, 23);
			this.btnUpdateJob.TabIndex = 24;
			this.btnUpdateJob.Text = "Update Job";
			this.btnUpdateJob.Click += new System.EventHandler(this.btnUpdateJob_Click);
			// 
			// txtJobID
			// 
			this.txtJobID.Location = new System.Drawing.Point(104, 64);
			this.txtJobID.Name = "txtJobID";
			this.txtJobID.ReadOnly = true;
			this.txtJobID.Size = new System.Drawing.Size(32, 20);
			this.txtJobID.TabIndex = 23;
			this.txtJobID.Text = "";
			this.txtJobID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// lblJobID
			// 
			this.lblJobID.Location = new System.Drawing.Point(8, 64);
			this.lblJobID.Name = "lblJobID";
			this.lblJobID.Size = new System.Drawing.Size(88, 16);
			this.lblJobID.TabIndex = 22;
			this.lblJobID.Text = "Job Number";
			this.lblJobID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtRunCommand
			// 
			this.txtRunCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtRunCommand.Location = new System.Drawing.Point(104, 88);
			this.txtRunCommand.Name = "txtRunCommand";
			this.txtRunCommand.Size = new System.Drawing.Size(496, 20);
			this.txtRunCommand.TabIndex = 21;
			this.txtRunCommand.Text = "";
			// 
			// lblRunCommand
			// 
			this.lblRunCommand.Location = new System.Drawing.Point(8, 88);
			this.lblRunCommand.Name = "lblRunCommand";
			this.lblRunCommand.Size = new System.Drawing.Size(88, 16);
			this.lblRunCommand.TabIndex = 20;
			this.lblRunCommand.Text = "Run Command";
			this.lblRunCommand.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// dgdJobList
			// 
			this.dgdJobList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.dgdJobList.CaptionText = "Job List";
			this.dgdJobList.CaptionVisible = false;
			this.dgdJobList.DataMember = "JobList";
			this.dgdJobList.DataSource = this.dstJobList;
			this.dgdJobList.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgdJobList.Location = new System.Drawing.Point(16, 208);
			this.dgdJobList.Name = "dgdJobList";
			this.dgdJobList.PreferredColumnWidth = 100;
			this.dgdJobList.ReadOnly = true;
			this.dgdJobList.RowHeaderWidth = 20;
			this.dgdJobList.Size = new System.Drawing.Size(648, 104);
			this.dgdJobList.TabIndex = 19;
			this.dgdJobList.CurrentCellChanged += new System.EventHandler(this.dgdJobList_CurrentCellChanged);
			// 
			// dstJobList
			// 
			this.dstJobList.DataSetName = "dstJobList";
			this.dstJobList.Locale = new System.Globalization.CultureInfo("en-US");
			this.dstJobList.Tables.AddRange(new System.Data.DataTable[] {
																			this.dtbJoblist});
			// 
			// dtbJoblist
			// 
			this.dtbJoblist.Columns.AddRange(new System.Data.DataColumn[] {
																			  this.colJobID,
																			  this.colInputFile,
																			  this.colOutputFile,
																			  this.colRunCommand});
			this.dtbJoblist.Constraints.AddRange(new System.Data.Constraint[] {
																				  new System.Data.UniqueConstraint("Constraint1", new string[] {
																																				   "Job Number"}, true)});
			this.dtbJoblist.PrimaryKey = new System.Data.DataColumn[] {
																		  this.colJobID};
			this.dtbJoblist.TableName = "JobList";
			// 
			// colJobID
			// 
			this.colJobID.AllowDBNull = false;
			this.colJobID.Caption = "Job Number";
			this.colJobID.ColumnName = "Job Number";
			this.colJobID.DataType = typeof(short);
			// 
			// colInputFile
			// 
			this.colInputFile.ColumnName = "Input File";
			// 
			// colOutputFile
			// 
			this.colOutputFile.ColumnName = "Output File";
			// 
			// colRunCommand
			// 
			this.colRunCommand.ColumnName = "Run Command";
			// 
			// groupBox4
			// 
			this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox4.Location = new System.Drawing.Point(8, 192);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(664, 8);
			this.groupBox4.TabIndex = 18;
			this.groupBox4.TabStop = false;
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.btnAdd.Location = new System.Drawing.Point(264, 168);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(72, 23);
			this.btnAdd.TabIndex = 16;
			this.btnAdd.Text = "Add Job";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// txtOutputFile
			// 
			this.txtOutputFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtOutputFile.Location = new System.Drawing.Point(104, 136);
			this.txtOutputFile.Name = "txtOutputFile";
			this.txtOutputFile.Size = new System.Drawing.Size(496, 20);
			this.txtOutputFile.TabIndex = 15;
			this.txtOutputFile.Text = "";
			// 
			// btnInputFile
			// 
			this.btnInputFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnInputFile.Location = new System.Drawing.Point(608, 112);
			this.btnInputFile.Name = "btnInputFile";
			this.btnInputFile.Size = new System.Drawing.Size(56, 23);
			this.btnInputFile.TabIndex = 14;
			this.btnInputFile.Text = "Browse";
			this.btnInputFile.Click += new System.EventHandler(this.btnInputFile_Click);
			// 
			// txtInputFile
			// 
			this.txtInputFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtInputFile.Location = new System.Drawing.Point(104, 112);
			this.txtInputFile.Name = "txtInputFile";
			this.txtInputFile.Size = new System.Drawing.Size(496, 20);
			this.txtInputFile.TabIndex = 13;
			this.txtInputFile.Text = "";
			// 
			// line
			// 
			this.line.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.line.Location = new System.Drawing.Point(8, 48);
			this.line.Name = "line";
			this.line.Size = new System.Drawing.Size(664, 8);
			this.line.TabIndex = 12;
			this.line.TabStop = false;
			// 
			// lblOutput
			// 
			this.lblOutput.Location = new System.Drawing.Point(8, 136);
			this.lblOutput.Name = "lblOutput";
			this.lblOutput.Size = new System.Drawing.Size(88, 16);
			this.lblOutput.TabIndex = 11;
			this.lblOutput.Text = "Job Output File";
			this.lblOutput.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblInput
			// 
			this.lblInput.Location = new System.Drawing.Point(8, 112);
			this.lblInput.Name = "lblInput";
			this.lblInput.Size = new System.Drawing.Size(88, 16);
			this.lblInput.TabIndex = 10;
			this.lblInput.Text = "Job Input File";
			this.lblInput.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// btnExeFile
			// 
			this.btnExeFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExeFile.Location = new System.Drawing.Point(608, 24);
			this.btnExeFile.Name = "btnExeFile";
			this.btnExeFile.Size = new System.Drawing.Size(56, 23);
			this.btnExeFile.TabIndex = 7;
			this.btnExeFile.Text = "Browse";
			this.btnExeFile.Click += new System.EventHandler(this.btnExeFile_Click);
			// 
			// txtExeFile
			// 
			this.txtExeFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtExeFile.Location = new System.Drawing.Point(104, 24);
			this.txtExeFile.Name = "txtExeFile";
			this.txtExeFile.Size = new System.Drawing.Size(496, 20);
			this.txtExeFile.TabIndex = 6;
			this.txtExeFile.Text = "";
			// 
			// lblExeFile
			// 
			this.lblExeFile.Location = new System.Drawing.Point(8, 24);
			this.lblExeFile.Name = "lblExeFile";
			this.lblExeFile.Size = new System.Drawing.Size(88, 16);
			this.lblExeFile.TabIndex = 5;
			this.lblExeFile.Text = "Executable File";
			this.lblExeFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// btnLoadXML
			// 
			this.btnLoadXML.Location = new System.Drawing.Point(184, 8);
			this.btnLoadXML.Name = "btnLoadXML";
			this.btnLoadXML.Size = new System.Drawing.Size(248, 23);
			this.btnLoadXML.TabIndex = 27;
			this.btnLoadXML.Text = "Load existing XML Application Description file";
			this.btnLoadXML.Click += new System.EventHandler(this.btnLoadXML_Click);
			// 
			// tabApplications
			// 
			this.tabApplications.Controls.Add(this.groupBox6);
			this.tabApplications.Controls.Add(this.groupBox5);
			this.tabApplications.Location = new System.Drawing.Point(4, 22);
			this.tabApplications.Name = "tabApplications";
			this.tabApplications.Size = new System.Drawing.Size(694, 396);
			this.tabApplications.TabIndex = 3;
			this.tabApplications.Text = "Applications";
			// 
			// groupBox6
			// 
			this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox6.Controls.Add(this.btStopJobs);
			this.groupBox6.Controls.Add(this.dgJobs);
			this.groupBox6.Location = new System.Drawing.Point(7, 201);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(686, 193);
			this.groupBox6.TabIndex = 8;
			this.groupBox6.TabStop = false;
			// 
			// btStopJobs
			// 
			this.btStopJobs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btStopJobs.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btStopJobs.Location = new System.Drawing.Point(573, 163);
			this.btStopJobs.Name = "btStopJobs";
			this.btStopJobs.Size = new System.Drawing.Size(75, 24);
			this.btStopJobs.TabIndex = 7;
			this.btStopJobs.Text = "Stop";
			this.btStopJobs.Click += new System.EventHandler(this.btStopJobs_Click);
			// 
			// dgJobs
			// 
			this.dgJobs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.dgJobs.CaptionText = "Jobs";
			this.dgJobs.DataMember = "table";
			this.dgJobs.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgJobs.Location = new System.Drawing.Point(7, 12);
			this.dgJobs.Name = "dgJobs";
			this.dgJobs.ReadOnly = true;
			this.dgJobs.Size = new System.Drawing.Size(673, 142);
			this.dgJobs.TabIndex = 0;
			// 
			// groupBox5
			// 
			this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox5.Controls.Add(this.dgApps);
			this.groupBox5.Controls.Add(this.btLoadApps);
			this.groupBox5.Controls.Add(this.btStopApps);
			this.groupBox5.Location = new System.Drawing.Point(7, 0);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(686, 208);
			this.groupBox5.TabIndex = 7;
			this.groupBox5.TabStop = false;
			// 
			// dgApps
			// 
			this.dgApps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.dgApps.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.dgApps.CaptionText = "Applications";
			this.dgApps.DataMember = "table";
			this.dgApps.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dgApps.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgApps.Location = new System.Drawing.Point(7, 12);
			this.dgApps.Name = "dgApps";
			this.dgApps.PreferredColumnWidth = 100;
			this.dgApps.ReadOnly = true;
			this.dgApps.Size = new System.Drawing.Size(673, 159);
			this.dgApps.TabIndex = 4;
			this.dgApps.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
																							   this.dataGridTableStyle1});
			this.dgApps.CurrentCellChanged += new System.EventHandler(this.dgApps_CurrentCellChanged);
			// 
			// dataGridTableStyle1
			// 
			this.dataGridTableStyle1.DataGrid = this.dgApps;
			this.dataGridTableStyle1.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
																												  this.dataGridTextBoxColumn1});
			this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGridTableStyle1.MappingName = "";
			// 
			// dataGridTextBoxColumn1
			// 
			this.dataGridTextBoxColumn1.Format = "";
			this.dataGridTextBoxColumn1.FormatInfo = null;
			this.dataGridTextBoxColumn1.HeaderText = "Application ID";
			this.dataGridTextBoxColumn1.MappingName = "application_id";
			this.dataGridTextBoxColumn1.Width = 150;
			// 
			// btLoadApps
			// 
			this.btLoadApps.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btLoadApps.Location = new System.Drawing.Point(13, 178);
			this.btLoadApps.Name = "btLoadApps";
			this.btLoadApps.Size = new System.Drawing.Size(104, 23);
			this.btLoadApps.TabIndex = 5;
			this.btLoadApps.Text = "Load";
			this.btLoadApps.Click += new System.EventHandler(this.btLoadApps_Click);
			// 
			// btStopApps
			// 
			this.btStopApps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btStopApps.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btStopApps.Location = new System.Drawing.Point(573, 178);
			this.btStopApps.Name = "btStopApps";
			this.btStopApps.TabIndex = 6;
			this.btStopApps.Text = "Stop";
			this.btStopApps.Click += new System.EventHandler(this.btStopApps_Click);
			// 
			// tabPage6
			// 
			this.tabPage6.Controls.Add(this.groupBox7);
			this.tabPage6.Controls.Add(this.nplotSurface);
			this.tabPage6.Location = new System.Drawing.Point(4, 22);
			this.tabPage6.Name = "tabPage6";
			this.tabPage6.Size = new System.Drawing.Size(694, 396);
			this.tabPage6.TabIndex = 5;
			this.tabPage6.Text = "Charts";
			// 
			// groupBox7
			// 
			this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox7.Controls.Add(this.groupBox8);
			this.groupBox7.Controls.Add(this.lblChart);
			this.groupBox7.Controls.Add(this.cobChart);
			this.groupBox7.Controls.Add(this.palJobByApp);
			this.groupBox7.Controls.Add(this.palJobByExec);
			this.groupBox7.Controls.Add(this.palJobByAppExe);
			this.groupBox7.Location = new System.Drawing.Point(544, 16);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(144, 368);
			this.groupBox7.TabIndex = 17;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "Chart Information";
			// 
			// groupBox8
			// 
			this.groupBox8.Location = new System.Drawing.Point(8, 80);
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.Size = new System.Drawing.Size(128, 8);
			this.groupBox8.TabIndex = 20;
			this.groupBox8.TabStop = false;
			// 
			// lblChart
			// 
			this.lblChart.Location = new System.Drawing.Point(8, 24);
			this.lblChart.Name = "lblChart";
			this.lblChart.Size = new System.Drawing.Size(56, 16);
			this.lblChart.TabIndex = 19;
			this.lblChart.Text = "Catalogue";
			// 
			// cobChart
			// 
			this.cobChart.Items.AddRange(new object[] {
														  "Performance of Apps",
														  "Jobs on Executors",
														  "Jobs of Apps on Exec"});
			this.cobChart.Location = new System.Drawing.Point(8, 48);
			this.cobChart.Name = "cobChart";
			this.cobChart.Size = new System.Drawing.Size(128, 21);
			this.cobChart.TabIndex = 18;
			this.cobChart.SelectedIndexChanged += new System.EventHandler(this.cobChart_SelectedIndexChanged);
			// 
			// palJobByApp
			// 
			this.palJobByApp.Controls.Add(this.lblJobNum);
			this.palJobByApp.Controls.Add(this.lblApp);
			this.palJobByApp.Controls.Add(this.lblAvg);
			this.palJobByApp.Controls.Add(this.lblMin);
			this.palJobByApp.Controls.Add(this.lblMax);
			this.palJobByApp.Controls.Add(this.btnAppRefresh);
			this.palJobByApp.Controls.Add(this.lblAppID);
			this.palJobByApp.Controls.Add(this.cobAppList);
			this.palJobByApp.Location = new System.Drawing.Point(8, 96);
			this.palJobByApp.Name = "palJobByApp";
			this.palJobByApp.Size = new System.Drawing.Size(128, 264);
			this.palJobByApp.TabIndex = 17;
			this.palJobByApp.Visible = false;
			// 
			// lblJobNum
			// 
			this.lblJobNum.Location = new System.Drawing.Point(8, 192);
			this.lblJobNum.Name = "lblJobNum";
			this.lblJobNum.Size = new System.Drawing.Size(112, 16);
			this.lblJobNum.TabIndex = 23;
			this.lblJobNum.Text = "Job Num: ";
			// 
			// lblApp
			// 
			this.lblApp.Location = new System.Drawing.Point(8, 64);
			this.lblApp.Name = "lblApp";
			this.lblApp.Size = new System.Drawing.Size(112, 48);
			this.lblApp.TabIndex = 22;
			this.lblApp.Text = "Name:";
			// 
			// lblAvg
			// 
			this.lblAvg.Location = new System.Drawing.Point(8, 168);
			this.lblAvg.Name = "lblAvg";
			this.lblAvg.Size = new System.Drawing.Size(112, 16);
			this.lblAvg.TabIndex = 21;
			this.lblAvg.Text = "Avg Time: ";
			// 
			// lblMin
			// 
			this.lblMin.Location = new System.Drawing.Point(8, 144);
			this.lblMin.Name = "lblMin";
			this.lblMin.Size = new System.Drawing.Size(112, 16);
			this.lblMin.TabIndex = 20;
			this.lblMin.Text = "Min Time: ";
			// 
			// lblMax
			// 
			this.lblMax.Location = new System.Drawing.Point(8, 120);
			this.lblMax.Name = "lblMax";
			this.lblMax.Size = new System.Drawing.Size(112, 16);
			this.lblMax.TabIndex = 19;
			this.lblMax.Text = "Max Time: ";
			// 
			// btnAppRefresh
			// 
			this.btnAppRefresh.Location = new System.Drawing.Point(16, 224);
			this.btnAppRefresh.Name = "btnAppRefresh";
			this.btnAppRefresh.Size = new System.Drawing.Size(96, 24);
			this.btnAppRefresh.TabIndex = 18;
			this.btnAppRefresh.Text = "Load";
			this.btnAppRefresh.Click += new System.EventHandler(this.btnAppRefresh_Click);
			// 
			// lblAppID
			// 
			this.lblAppID.Location = new System.Drawing.Point(8, 8);
			this.lblAppID.Name = "lblAppID";
			this.lblAppID.Size = new System.Drawing.Size(80, 16);
			this.lblAppID.TabIndex = 17;
			this.lblAppID.Text = "Application ID";
			// 
			// cobAppList
			// 
			this.cobAppList.Location = new System.Drawing.Point(8, 32);
			this.cobAppList.Name = "cobAppList";
			this.cobAppList.Size = new System.Drawing.Size(112, 21);
			this.cobAppList.TabIndex = 15;
			this.cobAppList.SelectedIndexChanged += new System.EventHandler(this.cobAppList_SelectedIndexChanged);
			// 
			// palJobByExec
			// 
			this.palJobByExec.Controls.Add(this.lblCpuStatus);
			this.palJobByExec.Controls.Add(this.lblCpuTotal);
			this.palJobByExec.Controls.Add(this.lblCpuAvail);
			this.palJobByExec.Controls.Add(this.lblCpuUsage);
			this.palJobByExec.Controls.Add(this.lblCpuMax);
			this.palJobByExec.Controls.Add(this.btnExecLoad);
			this.palJobByExec.Controls.Add(this.lblHost);
			this.palJobByExec.Controls.Add(this.cobExecutorID);
			this.palJobByExec.Controls.Add(this.lblExecutorID);
			this.palJobByExec.Location = new System.Drawing.Point(8, 96);
			this.palJobByExec.Name = "palJobByExec";
			this.palJobByExec.Size = new System.Drawing.Size(128, 264);
			this.palJobByExec.TabIndex = 25;
			this.palJobByExec.Visible = false;
			// 
			// lblCpuStatus
			// 
			this.lblCpuStatus.Location = new System.Drawing.Point(8, 88);
			this.lblCpuStatus.Name = "lblCpuStatus";
			this.lblCpuStatus.Size = new System.Drawing.Size(112, 16);
			this.lblCpuStatus.TabIndex = 26;
			this.lblCpuStatus.Text = "CPU Status";
			this.lblCpuStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblCpuTotal
			// 
			this.lblCpuTotal.Location = new System.Drawing.Point(8, 184);
			this.lblCpuTotal.Name = "lblCpuTotal";
			this.lblCpuTotal.Size = new System.Drawing.Size(112, 16);
			this.lblCpuTotal.TabIndex = 25;
			this.lblCpuTotal.Text = "Total: ";
			// 
			// lblCpuAvail
			// 
			this.lblCpuAvail.Location = new System.Drawing.Point(8, 160);
			this.lblCpuAvail.Name = "lblCpuAvail";
			this.lblCpuAvail.Size = new System.Drawing.Size(112, 16);
			this.lblCpuAvail.TabIndex = 24;
			this.lblCpuAvail.Text = "Avail: ";
			// 
			// lblCpuUsage
			// 
			this.lblCpuUsage.Location = new System.Drawing.Point(8, 136);
			this.lblCpuUsage.Name = "lblCpuUsage";
			this.lblCpuUsage.Size = new System.Drawing.Size(112, 16);
			this.lblCpuUsage.TabIndex = 23;
			this.lblCpuUsage.Text = "Usage: ";
			// 
			// lblCpuMax
			// 
			this.lblCpuMax.Location = new System.Drawing.Point(8, 112);
			this.lblCpuMax.Name = "lblCpuMax";
			this.lblCpuMax.Size = new System.Drawing.Size(112, 16);
			this.lblCpuMax.TabIndex = 22;
			this.lblCpuMax.Text = "Max: ";
			// 
			// btnExecLoad
			// 
			this.btnExecLoad.Location = new System.Drawing.Point(16, 224);
			this.btnExecLoad.Name = "btnExecLoad";
			this.btnExecLoad.Size = new System.Drawing.Size(96, 24);
			this.btnExecLoad.TabIndex = 21;
			this.btnExecLoad.Text = "Load";
			// 
			// lblHost
			// 
			this.lblHost.Location = new System.Drawing.Point(8, 64);
			this.lblHost.Name = "lblHost";
			this.lblHost.Size = new System.Drawing.Size(112, 16);
			this.lblHost.TabIndex = 20;
			this.lblHost.Text = "Host: ";
			// 
			// cobExecutorID
			// 
			this.cobExecutorID.Location = new System.Drawing.Point(8, 32);
			this.cobExecutorID.Name = "cobExecutorID";
			this.cobExecutorID.Size = new System.Drawing.Size(112, 21);
			this.cobExecutorID.TabIndex = 16;
			// 
			// lblExecutorID
			// 
			this.lblExecutorID.Location = new System.Drawing.Point(8, 8);
			this.lblExecutorID.Name = "lblExecutorID";
			this.lblExecutorID.Size = new System.Drawing.Size(64, 16);
			this.lblExecutorID.TabIndex = 0;
			this.lblExecutorID.Text = "Executor ID";
			// 
			// palJobByAppExe
			// 
			this.palJobByAppExe.Controls.Add(this.lblThreadStatus);
			this.palJobByAppExe.Controls.Add(this.lblDead);
			this.palJobByAppExe.Controls.Add(this.lblFinished);
			this.palJobByAppExe.Controls.Add(this.lblAppName1);
			this.palJobByAppExe.Controls.Add(this.lblStarted);
			this.palJobByAppExe.Controls.Add(this.lblSchedule);
			this.palJobByAppExe.Controls.Add(this.lblReady);
			this.palJobByAppExe.Controls.Add(this.btnAppLoad);
			this.palJobByAppExe.Controls.Add(this.lblAppID1);
			this.palJobByAppExe.Controls.Add(this.cobAppID1);
			this.palJobByAppExe.Location = new System.Drawing.Point(8, 96);
			this.palJobByAppExe.Name = "palJobByAppExe";
			this.palJobByAppExe.Size = new System.Drawing.Size(128, 264);
			this.palJobByAppExe.TabIndex = 24;
			this.palJobByAppExe.Visible = false;
			// 
			// lblThreadStatus
			// 
			this.lblThreadStatus.Location = new System.Drawing.Point(8, 88);
			this.lblThreadStatus.Name = "lblThreadStatus";
			this.lblThreadStatus.Size = new System.Drawing.Size(112, 16);
			this.lblThreadStatus.TabIndex = 25;
			this.lblThreadStatus.Text = "Thread Status";
			this.lblThreadStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDead
			// 
			this.lblDead.Location = new System.Drawing.Point(8, 208);
			this.lblDead.Name = "lblDead";
			this.lblDead.Size = new System.Drawing.Size(112, 16);
			this.lblDead.TabIndex = 24;
			this.lblDead.Text = "Dead:";
			// 
			// lblFinished
			// 
			this.lblFinished.Location = new System.Drawing.Point(8, 184);
			this.lblFinished.Name = "lblFinished";
			this.lblFinished.Size = new System.Drawing.Size(112, 16);
			this.lblFinished.TabIndex = 23;
			this.lblFinished.Text = "Finished:";
			// 
			// lblAppName1
			// 
			this.lblAppName1.Location = new System.Drawing.Point(8, 64);
			this.lblAppName1.Name = "lblAppName1";
			this.lblAppName1.Size = new System.Drawing.Size(112, 16);
			this.lblAppName1.TabIndex = 22;
			this.lblAppName1.Text = "Name:";
			// 
			// lblStarted
			// 
			this.lblStarted.Location = new System.Drawing.Point(8, 160);
			this.lblStarted.Name = "lblStarted";
			this.lblStarted.Size = new System.Drawing.Size(112, 16);
			this.lblStarted.TabIndex = 21;
			this.lblStarted.Text = "Started:";
			// 
			// lblSchedule
			// 
			this.lblSchedule.Location = new System.Drawing.Point(8, 136);
			this.lblSchedule.Name = "lblSchedule";
			this.lblSchedule.Size = new System.Drawing.Size(112, 16);
			this.lblSchedule.TabIndex = 20;
			this.lblSchedule.Text = "Scheduled:";
			// 
			// lblReady
			// 
			this.lblReady.Location = new System.Drawing.Point(8, 112);
			this.lblReady.Name = "lblReady";
			this.lblReady.Size = new System.Drawing.Size(112, 16);
			this.lblReady.TabIndex = 19;
			this.lblReady.Text = "Ready: ";
			// 
			// btnAppLoad
			// 
			this.btnAppLoad.Location = new System.Drawing.Point(16, 232);
			this.btnAppLoad.Name = "btnAppLoad";
			this.btnAppLoad.Size = new System.Drawing.Size(96, 24);
			this.btnAppLoad.TabIndex = 18;
			this.btnAppLoad.Text = "Load";
			// 
			// lblAppID1
			// 
			this.lblAppID1.Location = new System.Drawing.Point(8, 8);
			this.lblAppID1.Name = "lblAppID1";
			this.lblAppID1.Size = new System.Drawing.Size(80, 16);
			this.lblAppID1.TabIndex = 17;
			this.lblAppID1.Text = "Application ID";
			// 
			// cobAppID1
			// 
			this.cobAppID1.Location = new System.Drawing.Point(8, 32);
			this.cobAppID1.Name = "cobAppID1";
			this.cobAppID1.Size = new System.Drawing.Size(112, 21);
			this.cobAppID1.TabIndex = 15;
			// 
			// nplotSurface
			// 
			this.nplotSurface.AutoScaleAutoGeneratedAxes = false;
			this.nplotSurface.AutoScaleTitle = false;
			this.nplotSurface.BackColor = System.Drawing.SystemColors.Control;
			this.nplotSurface.DateTimeToolTip = false;
			this.nplotSurface.Legend = null;
			this.nplotSurface.LegendZOrder = -1;
			this.nplotSurface.Location = new System.Drawing.Point(16, 16);
			this.nplotSurface.Name = "nplotSurface";
			this.nplotSurface.Padding = 10;
			this.nplotSurface.RightMenu = null;
			this.nplotSurface.ShowCoordinates = false;
			this.nplotSurface.Size = new System.Drawing.Size(520, 368);
			this.nplotSurface.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
			this.nplotSurface.TabIndex = 14;
			this.nplotSurface.Title = "";
			this.nplotSurface.TitleFont = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.nplotSurface.XAxis1 = null;
			this.nplotSurface.XAxis2 = null;
			this.nplotSurface.YAxis1 = null;
			this.nplotSurface.YAxis2 = null;
			// 
			// tmRefreshSystem
			// 
			this.tmRefreshSystem.Enabled = true;
			this.tmRefreshSystem.Interval = 2000;
			this.tmRefreshSystem.Tick += new System.EventHandler(this.tmRefreshGraph_Tick);
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1,
																					  this.menuItem2});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mmConnect,
																					  this.mmDisconnect,
																					  this.mnuExit});
			this.menuItem1.Text = "Grid";
			// 
			// mmConnect
			// 
			this.mmConnect.Index = 0;
			this.mmConnect.Text = "Connect...";
			this.mmConnect.Click += new System.EventHandler(this.mmConnect_Click);
			// 
			// mmDisconnect
			// 
			this.mmDisconnect.Index = 1;
			this.mmDisconnect.Text = "Disconnect";
			this.mmDisconnect.Click += new System.EventHandler(this.mmDisconnect_Click);
			// 
			// mnuExit
			// 
			this.mnuExit.Index = 2;
			this.mnuExit.Text = "Exit";
			this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
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
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 432);
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new System.Drawing.Size(717, 22);
			this.statusBar.TabIndex = 1;
			// 
			// ofdExeFile
			// 
			this.ofdExeFile.FileOk += new System.ComponentModel.CancelEventHandler(this.ofdExeFile_FileOk);
			// 
			// ofdInputFile
			// 
			this.ofdInputFile.FileOk += new System.ComponentModel.CancelEventHandler(this.ofdInputFile_FileOk);
			// 
			// ofdXMLFile
			// 
			this.ofdXMLFile.Filter = "XML File|*.xml";
			this.ofdXMLFile.Title = "Open XML File";
			this.ofdXMLFile.FileOk += new System.ComponentModel.CancelEventHandler(this.ofdXMLFile_FileOk);
			// 
			// sfdXMLFile
			// 
			this.sfdXMLFile.CheckPathExists = false;
			this.sfdXMLFile.CreatePrompt = true;
			this.sfdXMLFile.DefaultExt = "xml";
			this.sfdXMLFile.Filter = "XML File|*.xml";
			this.sfdXMLFile.Title = "Save XML File";
			this.sfdXMLFile.FileOk += new System.ComponentModel.CancelEventHandler(this.sfdXMLFile_FileOk);
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(717, 454);
			this.Controls.Add(this.statusBar);
			this.Controls.Add(this.tabControl1);
			this.Menu = this.mainMenu1;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Alchemi Console";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabSystem.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.tabUsers.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgUsers)).EndInit();
			this.tabExecutors.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgExecutors)).EndInit();
			this.tabAppSubmission.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.palJobSubmit.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgdJobList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dstJobList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dtbJoblist)).EndInit();
			this.tabApplications.ResumeLayout(false);
			this.groupBox6.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgJobs)).EndInit();
			this.groupBox5.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgApps)).EndInit();
			this.tabPage6.ResumeLayout(false);
			this.groupBox7.ResumeLayout(false);
			this.palJobByApp.ResumeLayout(false);
			this.palJobByExec.ResumeLayout(false);
			this.palJobByAppExe.ResumeLayout(false);
			this.ResumeLayout(false);

		}
        #endregion

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            InitializePlotSurface();
			InitializeNPlotSurface();
            RefreshUI();
        }

        private void InitializePlotSurface()
        {
            plotSurface.Clear();

            plotSurface.Add( lineAvail );
            plotSurface.Add( lineUsage );

            plotSurface.PlotBackColor = plotSurface.BackColor;

            plotSurface.Title = "CPU Power - Availability & Usage";
            plotSurface.TitleFont = new Font(new FontFamily("Microsoft Sans Serif" ), 9.75f, FontStyle.Bold);

            plotSurface.XAxis1.WorldMin = -60.0f;
            plotSurface.XAxis1.WorldMax = 0.0f;
            plotSurface.XAxis1.Label = "Seconds";
            plotSurface.XAxis1.LabelFont = new Font(new FontFamily("Microsoft Sans Serif" ), 9.75f, FontStyle.Bold);
            plotSurface.XAxis1.TickTextFont = new Font(new FontFamily("Microsoft Sans Serif" ), 9.75f, FontStyle.Bold);

            plotSurface.YAxis1.WorldMin = 0.0;
            plotSurface.YAxis1.WorldMax= 100.0;
            plotSurface.YAxis1.Label = "Power [%]";
            plotSurface.YAxis1.LabelFont = new Font(new FontFamily("Microsoft Sans Serif" ), 9.75f, FontStyle.Bold);
            plotSurface.YAxis1.TickTextFont = new Font(new FontFamily("Microsoft Sans Serif" ), 9.75f, FontStyle.Bold);

            scpl.Grid gridPlotSurface = new scpl.Grid();
            gridPlotSurface.HorizontalGridType = scpl.Grid.GridType.None;
            gridPlotSurface.VerticalGridType = scpl.Grid.GridType.Fine;
            gridPlotSurface.MajorGridPen.Color = Color.DarkGray;
            plotSurface.Add(gridPlotSurface);

            plotSurface.Legend = new scpl.Legend();
            plotSurface.Legend.NeverShiftAxes = false;
            plotSurface.Legend.AttachTo( scpl.PlotSurface2D.XAxisPosition.Bottom , scpl.PlotSurface2D.YAxisPosition.Left);
            plotSurface.Legend.HorizontalEdgePlacement = scpl.Legend.Placement.Inside;
            plotSurface.Legend.VerticalEdgePlacement = scpl.Legend.Placement.Inside;

            lineAvail.Label = "usage";
            lineAvail.Pen   = new Pen(Color.Crimson, 2.0f);

            lineUsage.Label = "avail";
            lineUsage.Pen   = new Pen(Color.SteelBlue, 2.0f);
        }

        private void btSave_Click(object sender, System.EventArgs e)
        {
			try
			{
				DataTable src = ((DataTable) dgUsers.DataSource);
				DataTable updates = src.Clone();
				DataTable adds = src.Clone();

				bool updatePassword = false;
				string newPassword = "";

				foreach (DataRow user in src.Rows)
				{
					if (user.RowState == DataRowState.Modified)
					{
						updates.ImportRow(user);

						// check if the user is update his own password
						if (user["usr_name"].ToString() == console.Credentials.Username)
						{
							updatePassword = true;
							newPassword = user["password"].ToString();
						}
					}
					else if (user.RowState == DataRowState.Added)
					{
						adds.ImportRow(user);
					}
				}

				bool refresh = false;
				string msg = "";

				if (adds.Rows.Count > 0)
				{
					try
					{
						console.Manager.Admon_AddUsers(console.Credentials, adds);
						refresh = true;
					}
					catch (Exception ex)
					{
						msg = ex.Message;
					}
				}

				if (updates.Rows.Count > 0)
				{
					try
					{
						console.Manager.Admon_UpdateUsers(console.Credentials, updates);
						refresh = true;
					}
					catch (Exception ex)
					{
						msg = ex.Message;
					}
				}

				if (updatePassword)
				{
					console.Credentials.Password = newPassword;
				}
            
				if (refresh)
				{
					RefreshUsers();
				}

				if (msg != "")
				{
					MessageBox.Show("An error occured while updating / adding users : " + Environment.NewLine + msg, "Console Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Could not save users. Error: "+ex.Message,"Console Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				statusBar.Text = "Couldnot get save users. Error: " + ex.Message;
			}

        }

        private void RefreshUsers()
        {
			try
			{
				dgUsers.DataSource = console.Manager.Admon_GetUserList(console.Credentials);
				SizeColumnsToContent(dgUsers, -1);
			}			
			catch (Exception e)
			{
				MessageBox.Show("Could not get list of users. Error: "+e.Message,"Console Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				statusBar.Text = "Couldnot get list of users. Error: " + e.Message;
			}

        }

        ArrayList x1 = new ArrayList();
        ArrayList y1 = new ArrayList();
        ArrayList y2 = new ArrayList();

        double x = -1;

        private void RefreshSystem()
        {
			try
			{

				DataTable summary = console.Manager.Admon_GetSystemSummary(console.Credentials);

				this.lbTotal.Text = summary.Rows[0]["max_power"].ToString();
				this.lbTotalPowerUsage.Text = summary.Rows[0]["power_totalusage"].ToString();
				this.lbNumExec.Text = summary.Rows[0]["total_executors"].ToString();
				this.lbAvail.Text = summary.Rows[0]["power_avail"] + " %";
				this.lbUsage.Text = summary.Rows[0]["power_usage"] + " %";
				this.lbUnfinishedThreads.Text = summary.Rows[0]["unfinished_threads"].ToString();
            
				x++;
           
				x1.Add(x);

				y1.Add(Convert.ToDouble(summary.Rows[0]["power_usage"]));
				y2.Add(Convert.ToDouble(summary.Rows[0]["power_avail"]));

				if (x1.Count > 31)
				{
					x1.RemoveAt(0);
					y1.RemoveAt(0);
					y2.RemoveAt(0);
				}
          
        
				int npt=31;
				int []xTime  = new int[npt];
				double []yAvail = new double[npt];
				double []yUsage = new double[npt];

				for (int i=0; i<x1.Count; i++)
				{
					int x2 = ((((31 - x1.Count) + i)) * 2) - 60;
					xTime[i] = x2;
					yAvail[i] = (double) y1[i];
					yUsage[i] = (double) y2[i];
				}

				lineAvail.AbscissaData = xTime;
				lineAvail.ValueData = yAvail;

				lineUsage.AbscissaData = xTime;
				lineUsage.ValueData = yUsage;

				plotSurface.Refresh();
			}
			catch (System.Net.Sockets.SocketException se)
			{
				statusBar.Text = "Could not refresh system. Error: " + se.Message;
				disconnect();
				MessageBox.Show("Could not refresh system. Error: "+se.Message,"Console Error",MessageBoxButtons.OK,MessageBoxIcon.Error );
			}
			catch (Exception e)
			{
				statusBar.Text = "Could not refresh system. Error: " + e.Message; 
			}
        }

        private void tmRefreshGraph_Tick(object sender, System.EventArgs e)
        {
            if (connected)
            {
                RefreshSystem();
                RefreshUI();
            }
        }

        private void mmConnect_Click(object sender, System.EventArgs e)
        {
			
			logger.Debug("Showing connection dialog...");
            GConnectionDialog gcd = new GConnectionDialog();
            if (gcd.ShowDialog() == DialogResult.OK)
            {
                console = new ConsoleNode(gcd.Connection);
                connected = true;
                RefreshUI();
            }
        }

        private void mmDisconnect_Click(object sender, System.EventArgs e)
        {
			disconnect();
        }

		private void disconnect()
		{
			console = null;
			connected = false;
			RefreshUI();
		}

        private void RefreshUI()
        {
            this.tabControl1.Enabled = connected;
            this.mmConnect.Enabled = !connected;
            this.mmDisconnect.Enabled = connected;

            if (connected)
            {
                statusBar.Text = string.Format("Connected to grid at {0}:{1}.", console.Connection.Host, console.Connection.Port);
            }
            else
            {
                statusBar.Text = "Not connected.";
            }
        }

        private void btLoadUsers_Click(object sender, System.EventArgs e)
        {
            RefreshUsers();
        }

        private void btLoadApps_Click(object sender, System.EventArgs e)
        {
			RefreshApps();
        }

		private void RefreshApps()
		{
			try
			{
				string state;
				DataSet ds = new DataSet();
				ds = console.Manager.Admon_ExecQuery(console.Credentials,Alchemi.Core.Manager.Permission.ManageOwnApp,"select * from application");
				//ds = console.Manager.Admon_GetLiveApplicationList(console.Credentials);
				ds.Tables[0].Columns.Add("Status", typeof(System.String));
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					switch (ds.Tables[0].Rows[i]["state"].ToString())
					{
						case "0":
							state = "Awaiting Manifest";
							break;
						case "1":
							state = "Ready";
							break;
						case "2":
							state = "Stopped";
							break;
						default:
							state = "Unknown";
							break;
					}
					ds.Tables[0].Rows[i]["Status"] = state;
				}
				ds.Tables[0].Columns.Remove("state");
				dgApps.DataSource = ds;

				//dgApps.DataSource = console.Manager.Admon_GetLiveApplicationList(console.Credentials);
				SizeColumnsToContent(dgApps, -1);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Couldnot get list of applications. Error: "+ex.Message,"Console Error",MessageBoxButtons.OK,MessageBoxIcon.Error );
				statusBar.Text = "Couldnot get list of applications. Error: " + ex.Message;
			}			
		}


        /// http://www.codeguru.com/Csharp/Csharp/cs_controls/datagrid/comments.php/c4787/?thread=1505
        /// 
        /// Given a DataGrid and an int X, this function will size any table columns contained in that DataGrid based on a best-fit size for the first X elements of that column
        /// 
        /// the DataGrid whose tables need sizing
        /// the number of elements to analyze for sizing starting from the top, -1 for all
        public void SizeColumnsToContent(DataGrid dataGrid, int nRowsToScan)
        {
            //first check to make sure the DataGrid has a valid datasource
            if (dataGrid.DataSource == null)
            {
                //it does not
                return;
            }

            // Create graphics object for measuring widths.
            Graphics Graphics = dataGrid.CreateGraphics();

            // Define new table style.
            DataGridTableStyle tableStyle;

            //necessary b/c of the DataSet looping
            int nRowsToScanOriginal = nRowsToScan;
            bool scanAllRows;
            if(-1 == nRowsToScan)
                scanAllRows = true;
            else
                scanAllRows = false;

            try
            {
                if (dataGrid.DataSource.GetType() == typeof(DataSet))
                {
                    DataSet dataSet = (DataSet)dataGrid.DataSource;
                    if(dataSet.Tables.Count == 0)
                    {
                        //if the DataSet it empty, nothing to do
                        return;
                    }
                    // Clear any existing table styles.
                    dataGrid.TableStyles.Clear();
                    foreach(DataTable dataTable in dataSet.Tables)
                    {
                        if (scanAllRows)
                        {
                            nRowsToScan = dataTable.Rows.Count;
                        }
                        else
                        {
                            // Can only scan rows if they exist.
                            nRowsToScan = System.Math.Min(nRowsToScanOriginal,
                                dataTable.Rows.Count);
                        }

                        // Use mapping name that is defined in the data source.
                        tableStyle = new DataGridTableStyle();
                        tableStyle.MappingName = dataTable.TableName;

                        // Now create the column styles within the table style.
                        DataGridTextBoxColumn columnStyle;
                        int iWidth;

                        for (int iCurrCol = 0;
                            iCurrCol < dataTable.Columns.Count; iCurrCol++)
                        {
                            DataColumn dataColumn = dataTable.Columns[iCurrCol];

                            columnStyle = new DataGridTextBoxColumn();

                            columnStyle.TextBox.Enabled = true;
                            if(dataColumn.Caption != "")
                            {
                                columnStyle.HeaderText = dataColumn.Caption;
                            }
                            else
                            {
                                columnStyle.HeaderText = dataColumn.Caption;
                            }
                            columnStyle.MappingName = dataColumn.ColumnName;

                            // Set width to header text width.
                            iWidth = (int)(Graphics.MeasureString
                                (columnStyle.HeaderText,
                                dataGrid.Font).Width);

                            // Change width, if data width is
                            // wider than header text width.
                            // Check the width of the data in the first X rows.
                            DataRow dataRow;
                            for (int iRow = 0; iRow < nRowsToScan; iRow++)
                            {
                                dataRow = dataTable.Rows[iRow];

                                if (null != dataRow[dataColumn.ColumnName])
                                {
                                    int iColWidth = (int)(Graphics.MeasureString
                                        (dataRow.ItemArray[iCurrCol].ToString(),
                                        dataGrid.Font).Width);
                                    iWidth = System.Math.Max(iWidth, iColWidth);
                                }
                            }
                            columnStyle.Width = iWidth + 4;

                            // Add the new column style to the table style.
                            tableStyle.GridColumnStyles.Add(columnStyle);
                        }
                        // Add the new table style to the data grid.
                        dataGrid.TableStyles.Add(tableStyle);
                    }
                }
                else if(dataGrid.DataSource.GetType() == typeof(DataTable)) //the datagrid just has a DataTable
                {               
                    tableStyle = new DataGridTableStyle();
                    DataTable dataTable = (DataTable)dataGrid.DataSource;

                    if (-1 == nRowsToScan)
                    {
                        nRowsToScan = dataTable.Rows.Count;
                    }
                    else
                    {
                        // Can only scan rows if they exist.
                        nRowsToScan = System.Math.Min(nRowsToScan,
                            dataTable.Rows.Count);
                    }

                    // Clear any existing table styles.
                    dataGrid.TableStyles.Clear();

                    // Use mapping name that is defined in the data source.
                    tableStyle.MappingName = dataTable.TableName;

                    // Now create the column styles within the table style.
                    DataGridTextBoxColumn columnStyle;
                    int iWidth;

                    for (int iCurrCol = 0;
                        iCurrCol < dataTable.Columns.Count; iCurrCol++)
                    {
                        DataColumn dataColumn = dataTable.Columns[iCurrCol];

                        columnStyle = new DataGridTextBoxColumn();

                        columnStyle.TextBox.Enabled = true;
                        if(dataColumn.Caption != "")
                        {
                            columnStyle.HeaderText = dataColumn.Caption;
                        }
                        else
                        {
                            columnStyle.HeaderText = dataColumn.ColumnName;
                        }
                        columnStyle.MappingName = dataColumn.ColumnName;

                        // Set width to header text width.
                        iWidth = (int)(Graphics.MeasureString
                            (columnStyle.HeaderText,
                            dataGrid.Font).Width);

                        // Change width, if data width is
                        // wider than header text width.
                        // Check the width of the data in the first X rows.
                        DataRow dataRow;
                        for (int iRow = 0; iRow < nRowsToScan; iRow++)
                        {
                            dataRow = dataTable.Rows[iRow];

                            if (null != dataRow[dataColumn.ColumnName])
                            {
                                int iColWidth = (int)(Graphics.MeasureString
                                    (dataRow.ItemArray[iCurrCol].ToString(),
                                    dataGrid.Font).Width);
                                iWidth = System.Math.Max(iWidth, iColWidth);
                            }
                        }
                        columnStyle.Width = iWidth + 4;

                        // Add the new column style to the table style.
                        tableStyle.GridColumnStyles.Add(columnStyle);
                    }
                    // Add the new table style to the data grid.
                    dataGrid.TableStyles.Add(tableStyle);
                }
            }
            catch(Exception ex)
            {
				MessageBox.Show("Could not size columns.. Error: "+ex.Message,"Console Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            finally
            {
                Graphics.Dispose();
            }
        }

        private void btLoadExecutors_Click(object sender, System.EventArgs e)
        {
			RefreshExecutors();
        }

		private void RefreshExecutors()
		{
			try
			{
				dgExecutors.DataSource = console.Manager.Admon_GetExecutors(console.Credentials);
				SizeColumnsToContent(dgExecutors, -1);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Could not get list of executors. Error: "+ex.Message,"Console Error",MessageBoxButtons.OK,MessageBoxIcon.Error );
				statusBar.Text = "Couldnot get list of executors. Error: " + ex.Message;
			}
		}

        private void mmAbout_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("Alchemi Console v" + Alchemi.Core.Utility.Utils.AssemblyVersion,"About Alchemi Console",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void btStopApps_Click(object sender, System.EventArgs e)
        {
			try
			{
				ArrayList rows = GetSelectedRows(dgApps);
				string msg = "";
				if (MessageBox.Show("Stop selected applications?","Console Message",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
				{
					foreach (object row in rows)
					{
						string appId = dgApps[(int) row, 0].ToString();
						this.console.Manager.Owner_StopApplication(console.Credentials, appId);
						msg += appId + Environment.NewLine;
					}
					if (msg != "")
					{
						msg = "Stopped the following applications:" + Environment.NewLine + msg;
						MessageBox.Show(msg,"Stop Applications",MessageBoxButtons.OK,MessageBoxIcon.Information);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Could not stop applications. Error: "+ex.Message,"Console Error",MessageBoxButtons.OK,MessageBoxIcon.Error );
				statusBar.Text = "Couldnot stop applications. Error: " + ex.Message;
			}

        }

        private ArrayList GetSelectedRows(DataGrid dg) 
        { 
            ArrayList al = new ArrayList(); 
 
            CurrencyManager cm = (CurrencyManager)this.BindingContext[dg.DataSource, dg.DataMember]; 
            DataView dv = (DataView)cm.List; 
            for(int i = 0; i < dv.Count; ++i) 
            { 
                if (dg.IsSelected(i)) al.Add(i); 
            }
            return al; 
        }

		private void btnExeFile_Click(object sender, System.EventArgs e)
		{
			ofdExeFile.ShowDialog();
		}

		private void ofdExeFile_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			txtExeFile.Text = ofdExeFile.FileName;
		}

		private void btnInputFile_Click(object sender, System.EventArgs e)
		{
			ofdInputFile.ShowDialog();
		}

		private void ofdInputFile_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			txtInputFile.Text = ofdInputFile.FileName;
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			try
			{
				string inputFile, outputFile,runCommand,errorMsg;

				inputFile = txtInputFile.Text.Trim();
				outputFile = txtOutputFile.Text.Trim();
				runCommand = txtRunCommand.Text.Trim();
				errorMsg = "Input file name, output file name and run command cannot be empty!";

				if (inputFile.Equals("") || outputFile.Equals("") || runCommand.Equals(""))
					MessageBox.Show(errorMsg,"Console Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				else
				{
					addJob(inputFile, outputFile, runCommand);
					SizeColumnsToContent(dgdJobList,-1);
					dgdJobList.Refresh();
					resetAppSubmitGroupBox();
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show("Couldnot add the current job. Error: "+ex.Message,"Console Error",MessageBoxButtons.OK,MessageBoxIcon.Error );
			}
		}

		private void addJob(string inputFile, string outputFile, string runCommand)
		{
			DataRow jobRow;
				
			jobRow = dstJobList.Tables["JobList"].NewRow();
			jobRow[0] = dstJobList.Tables["JobList"].Rows.Count + 1;
			jobRow[1] = inputFile;
			jobRow[2] = outputFile;
			jobRow[3] = runCommand;
			dstJobList.Tables["JobList"].Rows.Add(jobRow);		
		}

		private void btnSubmit_Click(object sender, System.EventArgs e)
		{
			if (appCheck())
			{
				if (MessageBox.Show("Submit the current application?","Console Message",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
				{
					string s = "Job submitting...";
					setStatusBar(s);
					enableAppSubmitGroupBox(false);

					s = JobSubmit();

					if (s.Trim().Equals(""))
					{
						MessageBox.Show("Job submit finished.","Console Message",MessageBoxButtons.OK,MessageBoxIcon.Information);
						s = "Job submitted. Start to execute the application...";
						dstJobList.Tables["JobList"].Clear();
						palJobSubmit.Visible = false;
						setStatusBar(s);
						txtExeFile.Text = "";
						resetAppSubmitGroupBox();
					}
					else
						MessageBox.Show(s,"Console Error",MessageBoxButtons.OK,MessageBoxIcon.Error );
				}
			}
		}

		private bool appCheck()
		{
			string errorMsg = "";
			if (txtExeFile.Text.Trim().Equals(""))
			{
				errorMsg = "Executable file cannot be empty!";
				MessageBox.Show(errorMsg,"Console Error",MessageBoxButtons.OK,MessageBoxIcon.Error );
				return false;
			}
			else if (dstJobList.Tables["JobList"].Rows.Count <= 0)
			{
				errorMsg = "Job list cannot be empty!";
				MessageBox.Show(errorMsg,"Console Error",MessageBoxButtons.OK,MessageBoxIcon.Error );
				return false;
			}
			return true;
		}

		private string JobSubmit()
		{
			try
			{
				string exeFile, exeFilePath, inputFile, inputFilePath, outputFile, runCommand;
				int index, percentage;
				DataRow jobRow;

				exeFilePath = txtExeFile.Text.Trim().ToString();
				index = exeFilePath.LastIndexOf("\\");
				exeFile = exeFilePath.Substring(index + 1, exeFilePath.Length - index - 1 );

				palJobSubmit.Visible = true;
				prbJobSubmit.Value = 0;

				ga = new GApplication(console.Connection);

				ga.ThreadFinish += new GThreadFinish(JobFinished);
				ga.ApplicationFinish += new GApplicationFinish(ApplicationFinished);
                
				ga.Manifest.Add(new EmbeddedFileDependency(exeFile, exeFilePath));

				prbJobSubmit.Value = 5;
				lblPercentage.Text = "5%";
				lblPercentage.Refresh();

				for (int jobNum=0; jobNum<dstJobList.Tables["JobList"].Rows.Count; jobNum++)
				{
					GJob job = new GJob();

					jobRow = dstJobList.Tables["JobList"].Rows[jobNum];
					inputFilePath = jobRow[1].ToString();
					index = inputFilePath.LastIndexOf("\\");
					inputFile = inputFilePath.Substring(index + 1, inputFilePath.Length - index - 1);
					outputFile = jobRow[2].ToString();
					runCommand = jobRow[3].ToString();

					job.InputFiles.Add(new EmbeddedFileDependency(inputFile, inputFilePath));
					job.RunCommand = runCommand;
					job.OutputFiles.Add(new EmbeddedFileDependency(outputFile));

					ga.Threads.Add(job);

					percentage = (jobNum + 1) * 90 / dstJobList.Tables["JobList"].Rows.Count;
					prbJobSubmit.Value = percentage;
					lblPercentage.Text = percentage + "%";
					lblPercentage.Refresh();
				}
				ga.Start();	
				palJobSubmit.Visible = false;

				prbJobSubmit.Value = 100;
				lblPercentage.Text = "100%";
				lblPercentage.Refresh();

				return "";
			}
			catch (Exception e)
			{
				MessageBox.Show("Could not submit jobs. Error: "+e.Message,"Console Error",MessageBoxButtons.OK,MessageBoxIcon.Error );
				return "Could not submit jobs. Error: " + e.Message;			
			}
		}

		public static void JobFinished(GThread thread)
		{
			GJob job = (GJob) thread;

			foreach (FileDependency fd in job.OutputFiles)
			{
				Directory.CreateDirectory("job_" + job.Id);
				fd.UnPack(Path.Combine("job_" + job.Id, fd.FileName));
			}
		}

		public static void ApplicationFinished()
		{
			string s = "Application execution finished.";
			StatusBar staBar =  new StatusBar();
			staBar.Text  = s;
			ga.Stop();
		}

		private void setStatusBar(string s)
		{
			statusBar.Text = s;
		}

        private string SelectedAppID = "";
        private void dgApps_CurrentCellChanged(object sender, System.EventArgs ne)
        {
            if (dgApps.DataSource != null)
            {
                DataSet dsApps = (DataSet)dgApps.DataSource; 
                int RowIndex = dgApps.CurrentRowIndex;
                DataRow AppRow;
                string AppID = "";
      
                foreach (DataTable jobTable in dsApps.Tables)
                {
                    AppRow = jobTable.Rows[RowIndex];
                    AppID = AppRow["application_id"].ToString();
                    SelectedAppID = AppID;
                }
                try
                {
					string state;
					DataSet ds = new DataSet();
					ds = console.Manager.Admon_GetThreadList(console.Credentials,AppID);
					ds.Tables[0].Columns.Add("Status", typeof(System.String));
					for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
					{
						switch (ds.Tables[0].Rows[i]["state"].ToString())
						{
							case "0":
								state = "Ready";
								break;
							case "1":
								state = "Scheduled";
								break;
							case "2":
								state = "Started";
								break;
							case "3":
								state = "Finished";
								break;
							case "4":
								state = "Dead";
								break;
							default:
								state = "Unknown";
								break;
						}
						ds.Tables[0].Rows[i]["Status"] = state;
					}
					ds.Tables[0].Columns.Remove("state");
					dgJobs.DataSource = ds;

                    //dgJobs.DataSource = console.Manager.Admon_GetThreadList(console.Credentials,AppID);
                    SizeColumnsToContent(dgJobs,-1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Can't get list of Jobs. Error:" + ex.Message,"Console Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    statusBar.Text = "Can't get list of Jobs, Error:" + ex.Message;
                }
            }
            else 
            {
                statusBar.Text = "Please select validated Application";
                return;
            }
        }

        private void btStopJobs_Click(object sender, System.EventArgs e)
        {
            try
            {
                ArrayList rows = GetSelectedRows(dgJobs);
                string msg = "";
				if (MessageBox.Show("Stop selected jobs?","Console Message",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
				{
					foreach (object row in rows)
					{
						ThreadIdentifier JobID = new ThreadIdentifier(SelectedAppID, (int) dgJobs[(int) row, 0]);
						this.console.Manager.Owner_AbortThread(console.Credentials, JobID);
						msg += JobID + Environment.NewLine;
					}
					if (msg != "")
					{
						msg = "Stopped the following Job(s):" + Environment.NewLine + msg;
						MessageBox.Show(msg,"Stop Job(s)",MessageBoxButtons.OK,MessageBoxIcon.Information);
					}
				}
            }
            catch (Exception ex)
            {
                statusBar.Text = "Could not stop Job(s). Error: " + ex.Message;
                MessageBox.Show("Could not stop Job(s). Error: "+ex.Message,"Console Error",MessageBoxButtons.OK,MessageBoxIcon.Error );
            }

        }

		private void dgdJobList_CurrentCellChanged(object sender, System.EventArgs ne)
		{
				int rowIndex = dgdJobList.CurrentRowIndex;
				DataRow jobRow;
      
				jobRow = dstJobList.Tables["JobList"].Rows[rowIndex];
				txtRunCommand.Text = jobRow[3].ToString();
				txtInputFile.Text = jobRow[1].ToString();
				txtOutputFile.Text = jobRow[2].ToString();
				txtJobID.Text = jobRow[0].ToString();
		}

		private void btnUpdateJob_Click(object sender, System.EventArgs e)
		{
			try
			{
				string inputFile, outputFile,runCommand,errorMsg;
				int rowIndex = dgdJobList.CurrentRowIndex;

				inputFile = txtInputFile.Text.Trim();
				outputFile = txtOutputFile.Text.Trim();
				runCommand = txtRunCommand.Text.Trim();

				if (inputFile.Equals("") || outputFile.Equals("") || runCommand.Equals(""))
				{
					errorMsg = "Input file name, output file name and run command cannot be empty!";
					MessageBox.Show(errorMsg,"Console Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				}
				else if (rowIndex < 0)
				{
					errorMsg = "Please select a job to modify!";
					MessageBox.Show(errorMsg,"Console Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				}
				else
				{
					errorMsg = "Update the current job?";
					if (MessageBox.Show(errorMsg,"Console Message",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
					{
						DataRow jobRow;
				
						jobRow = dstJobList.Tables["JobList"].Rows[rowIndex];
						jobRow[1] = inputFile;
						jobRow[2] = outputFile;
						jobRow[3] = runCommand;
						SizeColumnsToContent(dgdJobList,-1);
						dgdJobList.Refresh();	
						resetAppSubmitGroupBox();
					}
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show("Couldnot update the current job. Error: "+ex.Message,"Console Error",MessageBoxButtons.OK,MessageBoxIcon.Error );
			}
		}

		private void resetAppSubmitGroupBox()
		{
			txtExeFile.Text = "";
			txtJobID.Text = "";
			txtRunCommand.Text = "";
			txtInputFile.Text = "";
			txtOutputFile.Text = "";
		}

		private void enableAppSubmitGroupBox(Boolean enable)
		{
			groupBox3.Enabled = enable;
			btnCancelAppSubmit.Visible = enable;
		}

		private void btnDeleteJob_Click(object sender, System.EventArgs e)
		{
			try
			{
				string errorMsg;
				int jobID;
				int rowIndex = dgdJobList.CurrentRowIndex;

				errorMsg = "Please select a job to delete!";

				if (rowIndex < 0)
					MessageBox.Show(errorMsg,"Console Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				else
				{
					errorMsg = "Delete the  job " + dstJobList.Tables["JobList"].Rows[rowIndex][0].ToString()  + " ?";
					if (MessageBox.Show(errorMsg,"Console Message",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
					{
						dstJobList.Tables["JobList"].Rows[rowIndex].Delete();
						for(jobID = rowIndex ; jobID  <  dstJobList.Tables["JobList"].Rows.Count; jobID++)
							dstJobList.Tables["JobList"].Rows[jobID][0] = jobID + 1;
						SizeColumnsToContent(dgdJobList,-1);
						dgdJobList.Refresh();	
						resetAppSubmitGroupBox();
					}
				}	
			}
			catch(Exception ex)
			{
				MessageBox.Show("Couldnot delete the selected job. Error: "+ex.Message,"Console Error",MessageBoxButtons.OK,MessageBoxIcon.Error );
			}
		}

		private void btnSaveXML_Click(object sender, System.EventArgs e)
		{
			if (appCheck())
				sfdXMLFile.ShowDialog();
		}

		private void btnLoadXML_Click(object sender, System.EventArgs e)
		{
			ofdXMLFile.ShowDialog();
		}

		private void loadXMLFile(string fileName)
		{
			try
			{
				string inputFile, outputFile, runCommand;

				dstJobList.Tables["JobList"].Clear();
				XmlTextReader reader = null;
				reader = new XmlTextReader (fileName);

				while (reader.Read())
					if (reader.NodeType == XmlNodeType.Element)
					{
						if (reader.Name.Equals("manifest"))
						{
							//manifest can be empty if there is no file being sent over.
							while(!reader.Name.Equals("embedded_file"))
							{
								reader.Read();
								if (reader.Value==null)
									break;
							}
								
							if (reader.HasAttributes)
								txtExeFile.Text = reader["location"].ToString();
						}
						else if (reader.Name.Equals("job"))
						{
							// Read input file location and name
							while (!reader.Name.Equals("embedded_file"))
								reader.Read();
							inputFile = reader["location"].ToString();
							// Read run command
							while (!reader.Name.Equals("work"))
								reader.Read();
							runCommand = reader["run_command"].ToString();
							// Read output file location and name
							while (!reader.Name.Equals("embedded_file"))
								reader.Read();
							outputFile = reader["name"].ToString();
							addJob(inputFile, outputFile, runCommand);
						}
					}

				reader.Close();
				SizeColumnsToContent(dgdJobList,-1);
				enableAppSubmitGroupBox(true);
			}
			catch(Exception ex)
			{
				MessageBox.Show("Couldnot read XML file. Error: "+ex.Message,"Console Error",MessageBoxButtons.OK,MessageBoxIcon.Error );
			}
		}

		private void btnNewApp_Click(object sender, System.EventArgs e)
		{
			dstJobList.Tables["JobList"].Clear();
			enableAppSubmitGroupBox(true);
			txtExeFile.Text = "";
			resetAppSubmitGroupBox();
		}

		private void ofdXMLFile_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			loadXMLFile(ofdXMLFile.FileName);
		}

		private void sfdXMLFile_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			string fileName;

			try
			{
				fileName = sfdXMLFile.FileName;
				writeXMLFile(fileName);
				MessageBox.Show("XML file saved!", "Console Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Could not save file! Error: "+ex.Message,"Console Error",MessageBoxButtons.OK,MessageBoxIcon.Error );
			}
		}

		private void writeXMLFile(string fileName)
		{
			string exeFile, exeFilePath, inputFile, inputFilePath, outputFile, runCommand;
			int index;
			DataRow jobRow;

			XmlTextWriter myXmlTextWriter = null;
			myXmlTextWriter = new XmlTextWriter(fileName, null);

			myXmlTextWriter.Formatting = Formatting.Indented;
			myXmlTextWriter.WriteStartElement("task");

			// Write manifest element
			exeFilePath = txtExeFile.Text.Trim().ToString();
			index = exeFilePath.LastIndexOf("\\");
			exeFile = exeFilePath.Substring(index + 1, exeFilePath.Length - index - 1 );
			myXmlTextWriter.WriteStartElement("manifest",null);
			myXmlTextWriter.WriteStartElement("embedded_file",null);
			myXmlTextWriter.WriteAttributeString("name",exeFile);
			myXmlTextWriter.WriteAttributeString("location",exeFilePath);
			myXmlTextWriter.WriteEndElement();
			myXmlTextWriter.WriteEndElement();

			// Write job element
			for (int i = 0; i < dstJobList.Tables["JobList"].Rows.Count; i++)
			{
				int jobID = i + 1;
				jobRow = dstJobList.Tables["JobList"].Rows[i];
				inputFilePath = jobRow[1].ToString();
				index = inputFilePath.LastIndexOf("\\");
				inputFile = inputFilePath.Substring(index + 1, inputFilePath.Length - index - 1);
				outputFile = jobRow[2].ToString();
				runCommand = jobRow[3].ToString();

				myXmlTextWriter.WriteStartElement("job", null);
				myXmlTextWriter.WriteAttributeString("id", jobID.ToString());

				// Write input file
				myXmlTextWriter.WriteStartElement("input", null);
				myXmlTextWriter.WriteStartElement("embedded_file",null);
				myXmlTextWriter.WriteAttributeString("name",inputFile);
				myXmlTextWriter.WriteAttributeString("location",inputFilePath);
				myXmlTextWriter.WriteEndElement();
				myXmlTextWriter.WriteEndElement();

				// Write run command
				myXmlTextWriter.WriteStartElement("work", null);
				myXmlTextWriter.WriteAttributeString("run_command",runCommand);
				myXmlTextWriter.WriteEndElement();

				// Write output file
				myXmlTextWriter.WriteStartElement("output", null);
				myXmlTextWriter.WriteStartElement("embedded_file",null);
				myXmlTextWriter.WriteAttributeString("name",outputFile);
				myXmlTextWriter.WriteEndElement();
				myXmlTextWriter.WriteEndElement();

				myXmlTextWriter.WriteEndElement();
			}

			myXmlTextWriter.WriteEndElement();

			//Write the XML to file and close the writer
			myXmlTextWriter.Flush();
			myXmlTextWriter.Close();		
		}

		public void InitializeNPlotSurface()
		{
			try
			{
				this.nplotSurface.Anchor = 
					System.Windows.Forms.AnchorStyles.Left |
					System.Windows.Forms.AnchorStyles.Right |
					System.Windows.Forms.AnchorStyles.Top |
					System.Windows.Forms.AnchorStyles.Bottom;

				// List here the plot routines that you want to be accessed
				PlotRoutines = new NPlotChartDelegate [] {	
															 new NPlotChartDelegate(JobsPerformanceByApp)
														 };

				this.nplotSurface.RightMenu = NPlot.Windows.PlotSurface2D.DefaultContextMenu;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Couldnot initialize graph. Error: "+ex.Message,"Console Error",MessageBoxButtons.OK,MessageBoxIcon.Error );
			}
		}

		public void JobsPerformanceByApp()
		{
			try
			{
				string appID;
				TimeSpan duration = new TimeSpan();
				appID = cobAppList.Items[cobAppList.SelectedIndex].ToString();

				DataSet ds = new DataSet();
				ds = console.Manager.Admon_GetThreadList(console.Credentials,appID);	

				nplotSurface.Clear();

				NPlot.Grid mygrid = new NPlot.Grid();
				mygrid.VerticalGridType = NPlot.Grid.GridType.Coarse;
				Pen majorGridPen = new Pen( Color.LightGray );
				float[] pattern = { 1.0f, 2.0f };
				majorGridPen.DashPattern = pattern;
				mygrid.MajorGridPen = majorGridPen;
				nplotSurface.Add( mygrid );

				float[] js = new float[ds.Tables[0].Rows.Count];
				float max = 0, min = 0, sum = 0;
				int avg;
				int jobNum;
				jobNum = ds.Tables[0].Rows.Count;
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					duration = (DateTime)ds.Tables[0].Rows[i]["time_finished"] -  (DateTime)ds.Tables[0].Rows[i]["time_started"];
					js[i] = (float)duration.TotalMilliseconds;
					sum = sum + js[i];
					if (i == 0)
					{
						max = js[i];
						min = js[i];
					}
					else
					{
						if (js[i] > max)
							max = js[i];
						if (js[i] < min)
							min = js[i];
					}
					//MessageBox.Show(js[i].ToString(),"Console Error",MessageBoxButtons.OK,MessageBoxIcon.Error );
				}

				NPlot.HistogramPlot jobTime = new NPlot.HistogramPlot();
				jobTime.DataSource = js;
				jobTime.BaseWidth = 0.6f;
				jobTime.RectangleBrush = RectangleBrushes.Vertical.FaintBlueFade;
				jobTime.Filled = true;
				jobTime.Label = "Execution Time";
		
				nplotSurface.Add(jobTime);
		
				nplotSurface.Legend = new NPlot.Legend();

				NPlot.LabelAxis la = new NPlot.LabelAxis( nplotSurface.XAxis1 );

				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
					la.AddLabel( "Job" + i.ToString(), (float)i );
				la.Label = "Jobs";
				la.TickTextFont = new Font( "Courier New", 8 );
				la.TicksBetweenText = true;

				nplotSurface.XAxis1 = la;
				if (ds.Tables[0].Rows.Count >= 10)
					nplotSurface.XAxis1.WorldMax =10.0;
				nplotSurface.YAxis1.WorldMin = 0.0;
				nplotSurface.YAxis1.Label = "Time(ms)";
				((NPlot.LinearAxis)nplotSurface.YAxis1).NumberOfSmallTicks = 1;

				nplotSurface.AddInteraction(new NPlot.Windows.PlotSurface2D.Interactions.HorizontalDrag());
				nplotSurface.AddInteraction(new NPlot.Windows.PlotSurface2D.Interactions.VerticalDrag());
				nplotSurface.AddInteraction(new NPlot.Windows.PlotSurface2D.Interactions.AxisDrag(true));

				nplotSurface.Title = "Performance of " + appID;

				nplotSurface.XAxis1.TicksLabelAngle = 30.0f;

				nplotSurface.PlotBackBrush = RectangleBrushes.Vertical.FaintRedFade;
				nplotSurface.Refresh();

				// Assign time lable
				avg = (int) sum / jobNum;
				lblMax.Text = "Max Time: " + max.ToString() + "ms";
				lblMin.Text = "Min Time: " + min.ToString() + "ms";
				lblAvg.Text = "Avg Time: " + avg.ToString() + "ms";
				lblJobNum.Text = "Job Num: " + jobNum.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Couldnot generate graph. Error: "+ex.Message,"Console Error",MessageBoxButtons.OK,MessageBoxIcon.Error );
			}	
		}

		private void cobAppList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			JobsPerformanceByApp();
		}

		private void cobChart_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			switch (cobChart.SelectedIndex)
			{
				case 0:
					palJobByApp.Visible = true;
					palJobByExec.Visible = false;
					palJobByAppExe.Visible = false;
					showJobsPerformanceByApp(false);
					break;
				case 1:
					palJobByApp.Visible = false;
					palJobByExec.Visible = true;
					palJobByAppExe.Visible = false;
					showJobsOnExecutors();
					break;
				case 2:
					palJobByApp.Visible = false;
					palJobByExec.Visible = false;
					palJobByAppExe.Visible = true;
					showJobsOfAppExe();
					break;
				default:
					nplotSurface.Clear();
					palJobByApp.Visible = false;
					palJobByExec.Visible = false;
					palJobByAppExe.Visible = false;
					break;
			}
		}
		
		/// <summary>
		/// Updates: 
		/// 
		///	23 October 2005 - Tibor Biro (tb@tbiro.com) - Changed the Application data from a DataSet 
		///		to ApplicationStorageView
		///		
		/// </summary>
		/// <param name="appRefresh"></param>
		private void showJobsPerformanceByApp(bool appRefresh)
		{
			try
			{
				if (appRefresh == true)
				{
					if (cobAppList.Items.Count > 0)
						cobAppList.Items.Clear();
//					DataSet ds = new DataSet();
//					ds = console.Manager.Admon_GetLiveApplicationList(console.Credentials);
//					for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
//						cobAppList.Items.Add(ds.Tables[0].Rows[i][0]);
					
					ApplicationStorageView[] applications = console.Manager.Admon_GetLiveApplicationList(console.Credentials);
					
					foreach(ApplicationStorageView application in applications)
					{
						cobAppList.Items.Add(application.ApplicationId);
					}

					cobAppList.SelectedIndex = 0;
				}
				else	if (cobAppList.Items.Count > 0)
					JobsPerformanceByApp();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Couldnot get list of applications. Error: "+ex.Message,"Console Error",MessageBoxButtons.OK,MessageBoxIcon.Error );
				statusBar.Text = "Couldnot get list of applications. Error: " + ex.Message;
			}	
		}

		private void showJobsOnExecutors()
		{
			nplotSurface.Clear();
			nplotSurface.Refresh();
			//MessageBox.Show("To be developed","Console Message",MessageBoxButtons.OK,MessageBoxIcon.Information);
		}

		private void showJobsOfAppExe()
		{
			nplotSurface.Clear();
			nplotSurface.Refresh();
			//MessageBox.Show("To be developed","Console Message",MessageBoxButtons.OK,MessageBoxIcon.Information);
		}

		private void btnAppRefresh_Click(object sender, System.EventArgs e)
		{
			showJobsPerformanceByApp(true);
		}

		private void mnuExit_Click(object sender, System.EventArgs e)
		{
			disconnect();
			Application.Exit();
		}

		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			TabPage t = tabControl1.SelectedTab;
			if (t == null)
				return;

			switch (t.Name)
			{
				case "tabSystem":
					RefreshSystem();
					break;
				case "tabUsers":
					RefreshUsers();
					break;
				case "tabApplications":
					RefreshApps();
					break;
				case "tabExecutors":
					RefreshExecutors();
					break;
			}
			
		}

		private void btnCancelAppSubmit_Click(object sender, System.EventArgs e)
		{
			resetAppSubmitGroupBox();
			enableAppSubmitGroupBox(false);
		}
	}
}
