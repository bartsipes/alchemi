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
using System.Data;
using System.Diagnostics;
using Alchemi.Core;
using Dundas.Charting.WinControl;

namespace Alchemi.Console
{
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.TabControl tabControl1;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGrid dgUsers;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGrid dgExecutors;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
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
        private Dundas.Charting.WinControl.Chart chPower;
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
        private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn1;
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
        
        ConsoleNode console;
        
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
            Dundas.Charting.WinControl.ChartArea chartArea1 = new Dundas.Charting.WinControl.ChartArea();
            Dundas.Charting.WinControl.Series series1 = new Dundas.Charting.WinControl.Series();
            Dundas.Charting.WinControl.Series series2 = new Dundas.Charting.WinControl.Series();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chPower = new Dundas.Charting.WinControl.Chart();
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
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.btStopApps = new System.Windows.Forms.Button();
            this.btLoadApps = new System.Windows.Forms.Button();
            this.dgApps = new System.Windows.Forms.DataGrid();
            this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
            this.dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btLoadExecutors = new System.Windows.Forms.Button();
            this.dgExecutors = new System.Windows.Forms.DataGrid();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btSave = new System.Windows.Forms.Button();
            this.dgUsers = new System.Windows.Forms.DataGrid();
            this.btLoadUsers = new System.Windows.Forms.Button();
            this.tmRefreshSystem = new System.Windows.Forms.Timer(this.components);
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.mmConnect = new System.Windows.Forms.MenuItem();
            this.mmDisconnect = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.mmAbout = new System.Windows.Forms.MenuItem();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chPower)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgApps)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgExecutors)).BeginInit();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgUsers)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(8, 8);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(702, 422);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox2);
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(694, 396);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "System";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.chPower);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Location = new System.Drawing.Point(8, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(488, 374);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            // 
            // chPower
            // 
            this.chPower.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.chPower.BackColor = System.Drawing.SystemColors.Control;
            this.chPower.BorderLineColor = System.Drawing.Color.Black;
            this.chPower.BorderSkin.FrameBackGradientEndColor = System.Drawing.Color.White;
            this.chPower.BorderSkin.PageColor = System.Drawing.SystemColors.Control;
            chartArea1.Area3DStyle.Light = Dundas.Charting.WinControl.LightStyle.Realistic;
            chartArea1.AxisX.LabelStyle.Format = "G0";
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX.MajorGrid.LineStyle = Dundas.Charting.WinControl.ChartDashStyle.Dot;
            chartArea1.AxisX.MajorTickMark.Style = Dundas.Charting.WinControl.TickMarkStyle.Cross;
            chartArea1.AxisX.Margin = false;
            chartArea1.AxisX.Maximum = 0;
            chartArea1.AxisX.Minimum = -60;
            chartArea1.AxisY.LabelStyle.Format = "P0";
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((System.Byte)(172)), ((System.Byte)(168)), ((System.Byte)(153)));
            chartArea1.AxisY.MajorGrid.LineStyle = Dundas.Charting.WinControl.ChartDashStyle.Dot;
            chartArea1.AxisY.MajorTickMark.Style = Dundas.Charting.WinControl.TickMarkStyle.Cross;
            chartArea1.AxisY.Maximum = 100;
            chartArea1.AxisY.Minimum = 0;
            chartArea1.BackColor = System.Drawing.Color.Transparent;
            chartArea1.BorderStyle = Dundas.Charting.WinControl.ChartDashStyle.Solid;
            chartArea1.Name = "Default";
            this.chPower.ChartAreas.Add(chartArea1);
            this.chPower.ChartData = "";
            this.chPower.Legend.BackColor = System.Drawing.Color.White;
            this.chPower.Legend.BorderColor = System.Drawing.Color.Black;
            this.chPower.Legend.Docking = Dundas.Charting.WinControl.LegendDocking.Bottom;
            this.chPower.Legend.LegendStyle = Dundas.Charting.WinControl.LegendStyle.Row;
            this.chPower.Location = new System.Drawing.Point(8, 16);
            this.chPower.Name = "chPower";
            series1.BorderColor = System.Drawing.Color.Black;
            series1.BorderWidth = 2;
            series1.ChartType = "Line";
            series1.Color = System.Drawing.Color.SteelBlue;
            series1.Name = "avail";
            series2.BorderColor = System.Drawing.Color.Black;
            series2.BorderWidth = 2;
            series2.ChartType = "Line";
            series2.Color = System.Drawing.Color.Crimson;
            series2.Name = "usage";
            this.chPower.Series.Add(series1);
            this.chPower.Series.Add(series2);
            this.chPower.Size = new System.Drawing.Size(472, 350);
            this.chPower.TabIndex = 15;
            this.chPower.Title = "CPU Power - Availability & Usage";
            this.chPower.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
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
            this.groupBox1.Location = new System.Drawing.Point(504, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(176, 374);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            // 
            // lbTotalPowerUsage
            // 
            this.lbTotalPowerUsage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.lbTotalPowerUsage.Location = new System.Drawing.Point(16, 96);
            this.lbTotalPowerUsage.Name = "lbTotalPowerUsage";
            this.lbTotalPowerUsage.Size = new System.Drawing.Size(152, 23);
            this.lbTotalPowerUsage.TabIndex = 18;
            this.lbTotalPowerUsage.Text = "_";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(16, 80);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(152, 16);
            this.label6.TabIndex = 17;
            this.label6.Text = "Total Power Usage";
            // 
            // lbTotal
            // 
            this.lbTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.lbTotal.Location = new System.Drawing.Point(16, 40);
            this.lbTotal.Name = "lbTotal";
            this.lbTotal.Size = new System.Drawing.Size(152, 23);
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
            this.lbUsage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.lbUsage.Location = new System.Drawing.Point(16, 264);
            this.lbUsage.Name = "lbUsage";
            this.lbUsage.Size = new System.Drawing.Size(80, 23);
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
            this.lbAvail.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.lbAvail.Location = new System.Drawing.Point(16, 208);
            this.lbAvail.Name = "lbAvail";
            this.lbAvail.Size = new System.Drawing.Size(80, 23);
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
            this.lbNumExec.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.lbNumExec.Location = new System.Drawing.Point(16, 152);
            this.lbNumExec.Name = "lbNumExec";
            this.lbNumExec.Size = new System.Drawing.Size(80, 23);
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
            this.lbUnfinishedThreads.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.lbUnfinishedThreads.Location = new System.Drawing.Point(16, 320);
            this.lbUnfinishedThreads.Name = "lbUnfinishedThreads";
            this.lbUnfinishedThreads.Size = new System.Drawing.Size(80, 23);
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
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.btStopApps);
            this.tabPage4.Controls.Add(this.btLoadApps);
            this.tabPage4.Controls.Add(this.dgApps);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(694, 396);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Applications";
            // 
            // btStopApps
            // 
            this.btStopApps.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btStopApps.Location = new System.Drawing.Point(608, 368);
            this.btStopApps.Name = "btStopApps";
            this.btStopApps.TabIndex = 6;
            this.btStopApps.Text = "Stop";
            this.btStopApps.Click += new System.EventHandler(this.btStopApps_Click);
            // 
            // btLoadApps
            // 
            this.btLoadApps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btLoadApps.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btLoadApps.Location = new System.Drawing.Point(8, 368);
            this.btLoadApps.Name = "btLoadApps";
            this.btLoadApps.Size = new System.Drawing.Size(104, 23);
            this.btLoadApps.TabIndex = 5;
            this.btLoadApps.Text = "Load";
            this.btLoadApps.Click += new System.EventHandler(this.btLoadApps_Click);
            // 
            // dgApps
            // 
            this.dgApps.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.dgApps.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgApps.CaptionText = "Applications";
            this.dgApps.DataMember = "table";
            this.dgApps.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.dgApps.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dgApps.Location = new System.Drawing.Point(8, 8);
            this.dgApps.Name = "dgApps";
            this.dgApps.PreferredColumnWidth = 100;
            this.dgApps.ReadOnly = true;
            this.dgApps.Size = new System.Drawing.Size(678, 350);
            this.dgApps.TabIndex = 4;
            this.dgApps.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
                                                                                               this.dataGridTableStyle1});
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
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btLoadExecutors);
            this.tabPage2.Controls.Add(this.dgExecutors);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(694, 396);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Executors";
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
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btSave);
            this.tabPage1.Controls.Add(this.dgUsers);
            this.tabPage1.Controls.Add(this.btLoadUsers);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(694, 396);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Users";
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
                                                                                      this.mmDisconnect});
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
            this.statusBar.Location = new System.Drawing.Point(0, 433);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(718, 22);
            this.statusBar.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(718, 455);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.tabControl1);
            this.Menu = this.mainMenu1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alchemi Console";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chPower)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgApps)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgExecutors)).EndInit();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgUsers)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            RefreshUI();
        }

        private void btSave_Click(object sender, System.EventArgs e)
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
                MessageBox.Show("An error occured while updating / adding users : " + Environment.NewLine + msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshUsers()
        {
            dgUsers.DataSource = console.Manager.Admon_GetUserList(console.Credentials);
            SizeColumnsToContent(dgUsers, -1);
        }

        ArrayList x1 = new ArrayList();
        ArrayList y1 = new ArrayList();
        ArrayList y2 = new ArrayList();

        double x = -1;

        private void RefreshSystem()
        {
            DataTable summary = console.Manager.Admon_GetSystemSummary(console.Credentials);

            this.lbTotal.Text = summary.Rows[0]["max_power"].ToString();
            this.lbTotalPowerUsage.Text = summary.Rows[0]["power_totalusage"].ToString();
            this.lbNumExec.Text = summary.Rows[0]["total_executors"].ToString();
            this.lbAvail.Text = summary.Rows[0]["power_avail"] + " %";
            this.lbUsage.Text = summary.Rows[0]["power_usage"] + " %";
            this.lbUnfinishedThreads.Text = summary.Rows[0]["unfinished_threads"].ToString();
            
            x++;
            
            // ***
            /*
            if (chPower.Series["avail"].Points.Count > 29)
            {
                this.chPower.Series["avail"].Points.RemoveAt(0);
                //this.chPower.Series["usage"].Points.RemoveAt(0);
            }
            */
            
            x1.Add(x);

            y1.Add(Convert.ToDouble(summary.Rows[0]["power_usage"]));
            y2.Add(Convert.ToDouble(summary.Rows[0]["power_avail"]));

            if (x1.Count > 31)
            {
                x1.RemoveAt(0);
                y1.RemoveAt(0);
                y2.RemoveAt(0);
            }

            chPower.Series["usage"].Points.Clear();
            chPower.Series["avail"].Points.Clear();

            for (int i=0; i<x1.Count; i++)
            {
                int x2 = ((((31 - x1.Count) + i)) * 2) - 60;
                this.chPower.Series["usage"].Points.AddXY(Convert.ToDouble(x2), y1[i]);
                this.chPower.Series["avail"].Points.AddXY(Convert.ToDouble(x2), y2[i]);
            }
        
            this.chPower.Invalidate();
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
            dgApps.DataSource = console.Manager.Admon_GetLiveApplicationList(console.Credentials);
            SizeColumnsToContent(dgApps, -1);
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
                                    iWidth = (int)System.Math.Max(iWidth, iColWidth);
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
                                iWidth = (int)System.Math.Max(iWidth, iColWidth);
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
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                Graphics.Dispose();
            }
        }

        private void btLoadExecutors_Click(object sender, System.EventArgs e)
        {
            dgExecutors.DataSource = console.Manager.Admon_GetExecutors(console.Credentials);
            SizeColumnsToContent(dgExecutors, -1);
        }

        private void mmAbout_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("Alchemi Console v" + Alchemi.Core.Utility.Utils.AssemblyVersion);
        }

        private void btStopApps_Click(object sender, System.EventArgs e)
        {
            ArrayList rows = GetSelectedRows(dgApps);
            string msg = "";
            foreach (object row in rows)
            {
                string appId = dgApps[(int) row, 0].ToString();
                this.console.Manager.Owner_StopApplication(console.Credentials, appId);
                msg += appId + Environment.NewLine;
            }
            if (msg != "")
            {
                msg = "Stopped the following applications:" + Environment.NewLine + msg;
                MessageBox.Show(msg);
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
    }
}
