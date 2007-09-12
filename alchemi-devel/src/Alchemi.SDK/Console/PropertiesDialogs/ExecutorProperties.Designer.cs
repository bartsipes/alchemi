namespace Alchemi.Console.PropertiesDialogs
{
    partial class ExecutorProperties
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExecutorProperties));
            this.label10 = new System.Windows.Forms.Label();
            this.txPingTime = new System.Windows.Forms.TextBox();
            this.chkDedicated = new System.Windows.Forms.CheckBox();
            this.chkConnected = new System.Windows.Forms.CheckBox();
            this.txPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txId = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txUsername = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabAdvanced = new System.Windows.Forms.TabPage();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txOS = new System.Windows.Forms.TextBox();
            this.txArch = new System.Windows.Forms.TextBox();
            this.txMaxCPU = new System.Windows.Forms.TextBox();
            this.txNumCPUs = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txMaxDisk = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPerf = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.plotSurface = new NPlot.Windows.PlotSurface2D();
            this.tmRefreshSystem = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).BeginInit();
            this.tabs.SuspendLayout();
            this.tabAdvanced.SuspendLayout();
            this.tabPerf.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // imgListSmall
            // 
            this.imgListSmall.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListSmall.ImageStream")));
            this.imgListSmall.Images.SetKeyName(0, "");
            this.imgListSmall.Images.SetKeyName(1, "");
            this.imgListSmall.Images.SetKeyName(2, "");
            this.imgListSmall.Images.SetKeyName(3, "");
            this.imgListSmall.Images.SetKeyName(4, "");
            this.imgListSmall.Images.SetKeyName(5, "");
            this.imgListSmall.Images.SetKeyName(6, "");
            this.imgListSmall.Images.SetKeyName(7, "");
            this.imgListSmall.Images.SetKeyName(8, "");
            this.imgListSmall.Images.SetKeyName(9, "");
            this.imgListSmall.Images.SetKeyName(10, "");
            this.imgListSmall.Images.SetKeyName(11, "");
            this.imgListSmall.Images.SetKeyName(12, "");
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.label10);
            this.tabGeneral.Controls.Add(this.txPingTime);
            this.tabGeneral.Controls.Add(this.chkDedicated);
            this.tabGeneral.Controls.Add(this.chkConnected);
            this.tabGeneral.Controls.Add(this.txPort);
            this.tabGeneral.Controls.Add(this.label2);
            this.tabGeneral.Controls.Add(this.txId);
            this.tabGeneral.Controls.Add(this.label9);
            this.tabGeneral.Controls.Add(this.txUsername);
            this.tabGeneral.Controls.Add(this.label3);
            this.tabGeneral.Controls.SetChildIndex(this.iconBox, 0);
            this.tabGeneral.Controls.SetChildIndex(this.lbName, 0);
            this.tabGeneral.Controls.SetChildIndex(this.lineLabel, 0);
            this.tabGeneral.Controls.SetChildIndex(this.label3, 0);
            this.tabGeneral.Controls.SetChildIndex(this.txUsername, 0);
            this.tabGeneral.Controls.SetChildIndex(this.label9, 0);
            this.tabGeneral.Controls.SetChildIndex(this.txId, 0);
            this.tabGeneral.Controls.SetChildIndex(this.label2, 0);
            this.tabGeneral.Controls.SetChildIndex(this.txPort, 0);
            this.tabGeneral.Controls.SetChildIndex(this.chkConnected, 0);
            this.tabGeneral.Controls.SetChildIndex(this.chkDedicated, 0);
            this.tabGeneral.Controls.SetChildIndex(this.txPingTime, 0);
            this.tabGeneral.Controls.SetChildIndex(this.label10, 0);
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tabAdvanced);
            this.tabs.Controls.Add(this.tabPerf);
            this.tabs.Controls.SetChildIndex(this.tabPerf, 0);
            this.tabs.Controls.SetChildIndex(this.tabAdvanced, 0);
            this.tabs.Controls.SetChildIndex(this.tabGeneral, 0);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 176);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(75, 13);
            this.label10.TabIndex = 42;
            this.label10.Text = "Last ping time:";
            // 
            // txPingTime
            // 
            this.txPingTime.BackColor = System.Drawing.Color.White;
            this.txPingTime.Location = new System.Drawing.Point(96, 176);
            this.txPingTime.Name = "txPingTime";
            this.txPingTime.ReadOnly = true;
            this.txPingTime.Size = new System.Drawing.Size(144, 20);
            this.txPingTime.TabIndex = 41;
            this.txPingTime.Text = "txPingTime";
            // 
            // chkDedicated
            // 
            this.chkDedicated.AutoCheck = false;
            this.chkDedicated.Location = new System.Drawing.Point(16, 240);
            this.chkDedicated.Name = "chkDedicated";
            this.chkDedicated.Size = new System.Drawing.Size(80, 24);
            this.chkDedicated.TabIndex = 40;
            this.chkDedicated.Text = "Dedicated";
            // 
            // chkConnected
            // 
            this.chkConnected.AutoCheck = false;
            this.chkConnected.Location = new System.Drawing.Point(16, 208);
            this.chkConnected.Name = "chkConnected";
            this.chkConnected.Size = new System.Drawing.Size(80, 24);
            this.chkConnected.TabIndex = 39;
            this.chkConnected.Text = "Connected";
            // 
            // txPort
            // 
            this.txPort.BackColor = System.Drawing.Color.White;
            this.txPort.Location = new System.Drawing.Point(96, 112);
            this.txPort.Name = "txPort";
            this.txPort.ReadOnly = true;
            this.txPort.Size = new System.Drawing.Size(48, 20);
            this.txPort.TabIndex = 38;
            this.txPort.Text = "txPort";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 37;
            this.label2.Text = "Port:";
            // 
            // txId
            // 
            this.txId.BackColor = System.Drawing.Color.White;
            this.txId.Location = new System.Drawing.Point(96, 80);
            this.txId.Name = "txId";
            this.txId.ReadOnly = true;
            this.txId.Size = new System.Drawing.Size(216, 20);
            this.txId.TabIndex = 36;
            this.txId.Text = "txId";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 82);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(19, 13);
            this.label9.TabIndex = 35;
            this.label9.Text = "Id:";
            // 
            // txUsername
            // 
            this.txUsername.BackColor = System.Drawing.Color.White;
            this.txUsername.Location = new System.Drawing.Point(96, 144);
            this.txUsername.Name = "txUsername";
            this.txUsername.ReadOnly = true;
            this.txUsername.Size = new System.Drawing.Size(144, 20);
            this.txUsername.TabIndex = 34;
            this.txUsername.Text = "txUsername";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 33;
            this.label3.Text = "Username:";
            // 
            // tabAdvanced
            // 
            this.tabAdvanced.Controls.Add(this.label12);
            this.tabAdvanced.Controls.Add(this.label11);
            this.tabAdvanced.Controls.Add(this.label6);
            this.tabAdvanced.Controls.Add(this.txOS);
            this.tabAdvanced.Controls.Add(this.txArch);
            this.tabAdvanced.Controls.Add(this.txMaxCPU);
            this.tabAdvanced.Controls.Add(this.txNumCPUs);
            this.tabAdvanced.Controls.Add(this.label8);
            this.tabAdvanced.Controls.Add(this.txMaxDisk);
            this.tabAdvanced.Controls.Add(this.label7);
            this.tabAdvanced.Controls.Add(this.label4);
            this.tabAdvanced.Controls.Add(this.label5);
            this.tabAdvanced.Location = new System.Drawing.Point(4, 22);
            this.tabAdvanced.Name = "tabAdvanced";
            this.tabAdvanced.Size = new System.Drawing.Size(328, 318);
            this.tabAdvanced.TabIndex = 2;
            this.tabAdvanced.Text = "Advanced";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(248, 114);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(26, 13);
            this.label12.TabIndex = 35;
            this.label12.Text = "MB.";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(248, 82);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(30, 13);
            this.label11.TabIndex = 34;
            this.label11.Text = "Mhz.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 80);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 29;
            this.label6.Text = "Max. CPU:";
            // 
            // txOS
            // 
            this.txOS.BackColor = System.Drawing.Color.White;
            this.txOS.Location = new System.Drawing.Point(112, 48);
            this.txOS.Name = "txOS";
            this.txOS.ReadOnly = true;
            this.txOS.Size = new System.Drawing.Size(200, 20);
            this.txOS.TabIndex = 26;
            this.txOS.Text = "txOS";
            // 
            // txArch
            // 
            this.txArch.BackColor = System.Drawing.Color.White;
            this.txArch.Location = new System.Drawing.Point(112, 16);
            this.txArch.Name = "txArch";
            this.txArch.ReadOnly = true;
            this.txArch.Size = new System.Drawing.Size(200, 20);
            this.txArch.TabIndex = 24;
            this.txArch.Text = "txArch";
            // 
            // txMaxCPU
            // 
            this.txMaxCPU.BackColor = System.Drawing.Color.White;
            this.txMaxCPU.Location = new System.Drawing.Point(112, 80);
            this.txMaxCPU.Name = "txMaxCPU";
            this.txMaxCPU.ReadOnly = true;
            this.txMaxCPU.Size = new System.Drawing.Size(128, 20);
            this.txMaxCPU.TabIndex = 28;
            this.txMaxCPU.Text = "txMaxCPU";
            // 
            // txNumCPUs
            // 
            this.txNumCPUs.BackColor = System.Drawing.Color.White;
            this.txNumCPUs.Location = new System.Drawing.Point(112, 144);
            this.txNumCPUs.Name = "txNumCPUs";
            this.txNumCPUs.ReadOnly = true;
            this.txNumCPUs.Size = new System.Drawing.Size(40, 20);
            this.txNumCPUs.TabIndex = 32;
            this.txNumCPUs.Text = "txNumCPUs";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 144);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 13);
            this.label8.TabIndex = 33;
            this.label8.Text = "# of CPUs:";
            // 
            // txMaxDisk
            // 
            this.txMaxDisk.BackColor = System.Drawing.Color.White;
            this.txMaxDisk.Location = new System.Drawing.Point(112, 112);
            this.txMaxDisk.Name = "txMaxDisk";
            this.txMaxDisk.ReadOnly = true;
            this.txMaxDisk.Size = new System.Drawing.Size(128, 20);
            this.txMaxDisk.TabIndex = 30;
            this.txMaxDisk.Text = "txMaxDisk";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 112);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 13);
            this.label7.TabIndex = 31;
            this.label7.Text = "Max. Storage:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "Architecture:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 13);
            this.label5.TabIndex = 27;
            this.label5.Text = "Operating System:";
            // 
            // tabPerf
            // 
            this.tabPerf.Controls.Add(this.panel1);
            this.tabPerf.Location = new System.Drawing.Point(4, 22);
            this.tabPerf.Name = "tabPerf";
            this.tabPerf.Size = new System.Drawing.Size(328, 318);
            this.tabPerf.TabIndex = 3;
            this.tabPerf.Text = "Performance";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.plotSurface);
            this.panel1.Location = new System.Drawing.Point(8, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(312, 304);
            this.panel1.TabIndex = 28;
            // 
            // plotSurface
            // 
            this.plotSurface.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.plotSurface.AutoScaleAutoGeneratedAxes = false;
            this.plotSurface.AutoScaleTitle = false;
            this.plotSurface.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.plotSurface.DateTimeToolTip = false;
            this.plotSurface.Legend = null;
            this.plotSurface.LegendZOrder = -1;
            this.plotSurface.Location = new System.Drawing.Point(2, 2);
            this.plotSurface.Name = "plotSurface";
            this.plotSurface.RightMenu = null;
            this.plotSurface.ShowCoordinates = true;
            this.plotSurface.Size = new System.Drawing.Size(304, 296);
            this.plotSurface.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            this.plotSurface.TabIndex = 0;
            this.plotSurface.Text = "plotSurface2D1";
            this.plotSurface.Title = "";
            this.plotSurface.TitleFont = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.plotSurface.XAxis1 = null;
            this.plotSurface.XAxis2 = null;
            this.plotSurface.YAxis1 = null;
            this.plotSurface.YAxis2 = null;
            // 
            // tmRefreshSystem
            // 
            this.tmRefreshSystem.Interval = 2000;
            this.tmRefreshSystem.Tick += new System.EventHandler(this.tmRefreshSystem_Tick);
            // 
            // ExecutorProperties2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 389);
            this.Name = "ExecutorProperties2";
            this.Text = "Executor Properties";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).EndInit();
            this.tabs.ResumeLayout(false);
            this.tabAdvanced.ResumeLayout(false);
            this.tabAdvanced.PerformLayout();
            this.tabPerf.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txPingTime;
        private System.Windows.Forms.CheckBox chkDedicated;
        private System.Windows.Forms.CheckBox chkConnected;
        private System.Windows.Forms.TextBox txPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txId;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txUsername;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabAdvanced;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txOS;
        private System.Windows.Forms.TextBox txArch;
        private System.Windows.Forms.TextBox txMaxCPU;
        private System.Windows.Forms.TextBox txNumCPUs;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txMaxDisk;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tabPerf;        
        private System.Windows.Forms.Panel panel1;
        private NPlot.Windows.PlotSurface2D plotSurface;
        private System.Windows.Forms.Timer tmRefreshSystem;
    }
}