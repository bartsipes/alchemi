using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using System.Data;
using Alchemi.Core;
using Alchemi.Core.Owner;
using log4net;

// Configure log4net using the .config file
[assembly: log4net.Config.XmlConfigurator(Watch=true)]

namespace Alchemi.Examples.Mandelbrot
{
    public class MandelForm : Form
    {
		// Create a logger for use in this class
		private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private delegate void UpdateHandler(GThread thread);

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Button btZoomIn;
        private System.Windows.Forms.Button btZoomOut;
        private System.Windows.Forms.Button btUp;
        private System.Windows.Forms.Button btDown;
        private System.Windows.Forms.Button btLeft;
        private System.Windows.Forms.Button btRight;
        private System.Windows.Forms.ColorDialog cp;
        private System.Windows.Forms.PictureBox pbColorTwo;
        private System.Windows.Forms.Button btRefresh;
        private System.Windows.Forms.TextBox txHorz;
        private System.Windows.Forms.TextBox txVert;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.SaveFileDialog sfd;
        private System.Windows.Forms.PictureBox pbColorOne;

        bool initted = false;
        bool closing = false;

        int totalHorzMaps;
        int totalVertMaps;
        int width = 500;
        int height = 400;
        int xOffset = -325;
        int yOffset = -200;
        int zoom = 140;
        Bitmap map;
        DateTime startTime;
        private System.Windows.Forms.ProgressBar pb;
        private System.Windows.Forms.StatusBar sb;
    	GApplication ga;

        public MandelForm()
        {
            InitializeComponent();
            map = new Bitmap(500, 400);
            pictureBox1.Image = map;

			Logger.LogHandler += new LogEventHandler(LogHandler);
        }

        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if (components != null) 
                {
                    components.Dispose();
                }
				if (ga!=null)
				{
					ga.Dispose();
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btZoomIn = new System.Windows.Forms.Button();
            this.btZoomOut = new System.Windows.Forms.Button();
            this.btUp = new System.Windows.Forms.Button();
            this.btDown = new System.Windows.Forms.Button();
            this.btLeft = new System.Windows.Forms.Button();
            this.btRight = new System.Windows.Forms.Button();
            this.cp = new System.Windows.Forms.ColorDialog();
            this.pbColorOne = new System.Windows.Forms.PictureBox();
            this.pbColorTwo = new System.Windows.Forms.PictureBox();
            this.btRefresh = new System.Windows.Forms.Button();
            this.txHorz = new System.Windows.Forms.TextBox();
            this.txVert = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btSave = new System.Windows.Forms.Button();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.pb = new System.Windows.Forms.ProgressBar();
            this.sb = new System.Windows.Forms.StatusBar();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(8, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(496, 400);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btZoomIn
            // 
            this.btZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btZoomIn.Location = new System.Drawing.Point(288, 456);
            this.btZoomIn.Name = "btZoomIn";
            this.btZoomIn.Size = new System.Drawing.Size(24, 23);
            this.btZoomIn.TabIndex = 2;
            this.btZoomIn.Text = "+";
            this.btZoomIn.Click += new System.EventHandler(this.btZoomIn_Click);
            // 
            // btZoomOut
            // 
            this.btZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btZoomOut.Location = new System.Drawing.Point(256, 456);
            this.btZoomOut.Name = "btZoomOut";
            this.btZoomOut.Size = new System.Drawing.Size(24, 23);
            this.btZoomOut.TabIndex = 3;
            this.btZoomOut.Text = "-";
            this.btZoomOut.Click += new System.EventHandler(this.btZoomOut_Click);
            // 
            // btUp
            // 
            this.btUp.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btUp.Location = new System.Drawing.Point(272, 424);
            this.btUp.Name = "btUp";
            this.btUp.Size = new System.Drawing.Size(24, 23);
            this.btUp.TabIndex = 4;
            this.btUp.Text = "U";
            this.btUp.Click += new System.EventHandler(this.btUp_Click);
            // 
            // btDown
            // 
            this.btDown.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btDown.Location = new System.Drawing.Point(272, 488);
            this.btDown.Name = "btDown";
            this.btDown.Size = new System.Drawing.Size(24, 23);
            this.btDown.TabIndex = 5;
            this.btDown.Text = "D";
            this.btDown.Click += new System.EventHandler(this.btDown_Click);
            // 
            // btLeft
            // 
            this.btLeft.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btLeft.Location = new System.Drawing.Point(224, 456);
            this.btLeft.Name = "btLeft";
            this.btLeft.Size = new System.Drawing.Size(24, 23);
            this.btLeft.TabIndex = 6;
            this.btLeft.Text = "L";
            this.btLeft.Click += new System.EventHandler(this.btLeft_Click);
            // 
            // btRight
            // 
            this.btRight.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btRight.Location = new System.Drawing.Point(320, 456);
            this.btRight.Name = "btRight";
            this.btRight.Size = new System.Drawing.Size(24, 23);
            this.btRight.TabIndex = 7;
            this.btRight.Text = "R";
            this.btRight.Click += new System.EventHandler(this.btRight_Click);
            // 
            // pbColorOne
            // 
            this.pbColorOne.BackColor = System.Drawing.Color.Yellow;
            this.pbColorOne.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbColorOne.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbColorOne.Location = new System.Drawing.Point(8, 24);
            this.pbColorOne.Name = "pbColorOne";
            this.pbColorOne.Size = new System.Drawing.Size(40, 24);
            this.pbColorOne.TabIndex = 8;
            this.pbColorOne.TabStop = false;
            this.pbColorOne.Click += new System.EventHandler(this.pbColorOne_Click);
            // 
            // pbColorTwo
            // 
            this.pbColorTwo.BackColor = System.Drawing.Color.Red;
            this.pbColorTwo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbColorTwo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbColorTwo.Location = new System.Drawing.Point(8, 56);
            this.pbColorTwo.Name = "pbColorTwo";
            this.pbColorTwo.Size = new System.Drawing.Size(40, 24);
            this.pbColorTwo.TabIndex = 9;
            this.pbColorTwo.TabStop = false;
            this.pbColorTwo.Click += new System.EventHandler(this.pbColorTwo_Click);
            // 
            // btRefresh
            // 
            this.btRefresh.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btRefresh.Location = new System.Drawing.Point(416, 440);
            this.btRefresh.Name = "btRefresh";
            this.btRefresh.Size = new System.Drawing.Size(88, 24);
            this.btRefresh.TabIndex = 10;
            this.btRefresh.Text = "Start / Refresh";
            this.btRefresh.Click += new System.EventHandler(this.btRefresh_Click);
            // 
            // txHorz
            // 
            this.txHorz.Location = new System.Drawing.Point(40, 24);
            this.txHorz.Name = "txHorz";
            this.txHorz.Size = new System.Drawing.Size(24, 20);
            this.txHorz.TabIndex = 11;
            this.txHorz.Text = "5";
            // 
            // txVert
            // 
            this.txVert.Location = new System.Drawing.Point(40, 56);
            this.txVert.Name = "txVert";
            this.txVert.Size = new System.Drawing.Size(24, 20);
            this.txVert.TabIndex = 12;
            this.txVert.Text = "5";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 16);
            this.label3.TabIndex = 15;
            this.label3.Text = "Horz";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 16);
            this.label4.TabIndex = 16;
            this.label4.Text = "Vert";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(16, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 16);
            this.label5.TabIndex = 17;
            this.label5.Text = "X";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pbColorOne);
            this.groupBox1.Controls.Add(this.pbColorTwo);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(8, 424);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(56, 88);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Colors";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txHorz);
            this.groupBox2.Controls.Add(this.txVert);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Location = new System.Drawing.Point(72, 424);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(80, 88);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "No. Cells";
            // 
            // btSave
            // 
            this.btSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btSave.Location = new System.Drawing.Point(416, 472);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(88, 24);
            this.btSave.TabIndex = 20;
            this.btSave.Text = "Save Bitmap";
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // sfd
            // 
            this.sfd.DefaultExt = "bmp";
            this.sfd.Filter = "Bitmap files|*.bmp";
            // 
            // pb
            // 
            this.pb.Location = new System.Drawing.Point(8, 520);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(496, 16);
            this.pb.TabIndex = 21;
            // 
            // sb
            // 
            this.sb.Location = new System.Drawing.Point(0, 544);
            this.sb.Name = "sb";
            this.sb.Size = new System.Drawing.Size(512, 24);
            this.sb.TabIndex = 22;
            // 
            // MandelForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(512, 568);
            this.Controls.Add(this.sb);
            this.Controls.Add(this.pb);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btRefresh);
            this.Controls.Add(this.btRight);
            this.Controls.Add(this.btLeft);
            this.Controls.Add(this.btDown);
            this.Controls.Add(this.btUp);
            this.Controls.Add(this.btZoomOut);
            this.Controls.Add(this.btZoomIn);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MandelForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alchemi Fractal Generator";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MandelForm_Closing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        [STAThread]
        static void Main() 
        {
            Application.EnableVisualStyles();
            Application.Run(new MandelForm());
			
        }

		private static void LogHandler(object sender, LogEventArgs e)
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

        private void Generate()
        {
            try
            {
                totalHorzMaps = int.Parse(txHorz.Text);
                totalVertMaps = int.Parse(txVert.Text);
            }
            catch
            {
                MessageBox.Show("Invalid value(s) for 'No. Cells'.");
                return;
            }
            
            if (!initted)
            {
                GConnectionDialog gcd = new GConnectionDialog();
                if (gcd.ShowDialog() == DialogResult.OK)
                {
                    // initialise application
                    ga = new GApplication(true);
                    ga.ApplicationName = "Alchemi Fractal Generator - Alchemi sample";
					
					ga.Connection = gcd.Connection;
                }
                else
                {
                    return;
                }
                
                // set dependencies
                ga.Manifest.Add(new ModuleDependency(typeof(KarlsTools.Complex).Module));
                ga.Manifest.Add(new ModuleDependency(typeof(MandelThread).Module));
                
                // subscribe to events
                ga.ThreadFinish += new GThreadFinish(UpdateBitmap);
				ga.ApplicationFinish += new GApplicationFinish(AppDone);

                try
                {
                    ga.Start();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }

                initted = true;
            }

            startTime = DateTime.Now;

            for (int mapNumX=0; mapNumX<totalHorzMaps; mapNumX++)
            {
                for (int mapNumY=0; mapNumY<totalVertMaps; mapNumY++)
                {
                    MandelThread mandel = new MandelThread(
                        mapNumX,
                        mapNumY,
                        width/totalHorzMaps,
                        height/totalVertMaps,
                        xOffset + mapNumX * width/totalHorzMaps,
                        yOffset + mapNumY * height/totalVertMaps,
                        zoom,
                        pbColorOne.BackColor,
                        pbColorTwo.BackColor
                        );
                    //ga.Threads.Add(mandel);
                    ga.StartThread(mandel);
                }
            }

            pb.Minimum = 0;
            pb.Value = 0;
            pb.Maximum = totalHorzMaps * totalVertMaps;
        }

		void AppDone()
		{
			MessageBox.Show("Application finished");
		}

        void UpdateBitmap(GThread thread)
        {
            if (!closing)
            {
                this.Invoke(new UpdateHandler(UpdateBitmap0), thread);
            }
        }

        void UpdateBitmap0(GThread thread)
        {
            // update progress bar
            if (pb.Value + 1 <= pb.Maximum)
            {
                pb.Value++;
            }

            // update status bar
            sb.Text = (DateTime.Now - startTime).ToString();

            MandelThread mandel = (MandelThread) thread;
            int startX = mandel.MapNumX * mandel.Width;
            int startY = mandel.MapNumY * mandel.Height;

            for (int x=0; x<mandel.Width; x++)
            {
                for (int y=0; y<mandel.Height; y++)
                {
                    map.SetPixel(x+startX, y+startY, mandel.Map.GetPixel(x, y));
                }
            }
            pictureBox1.Refresh();
        }

        private void btZoomIn_Click(object sender, System.EventArgs e)
        {
            zoom = (int) (zoom * 1.5);
            Generate();
        }

        private void btZoomOut_Click(object sender, System.EventArgs e)
        {
            zoom = (int) (zoom / 1.5);
            Generate();
        }

        private void btUp_Click(object sender, System.EventArgs e)
        {
            yOffset -= 100;
            Generate();
        }

        private void btDown_Click(object sender, System.EventArgs e)
        {
            yOffset += 100;
            Generate();
        }

        private void btRight_Click(object sender, System.EventArgs e)
        {
            xOffset += 125;
            Generate();
        }

        private void btLeft_Click(object sender, System.EventArgs e)
        {
            xOffset -= 125;
            Generate();
        }

        private void pbColorOne_Click(object sender, System.EventArgs e)
        {
            if (cp.ShowDialog() == DialogResult.OK)
            {
                pbColorOne.BackColor = cp.Color;
            }
        }

        private void pbColorTwo_Click(object sender, System.EventArgs e)
        {
            if (cp.ShowDialog() == DialogResult.OK)
            {
                pbColorTwo.BackColor = cp.Color;
            }
        }

        private void btRefresh_Click(object sender, System.EventArgs e)
        {
            Generate();
        }

        private void btSave_Click(object sender, System.EventArgs e)
        {
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                map.Save(sfd.FileName);
            }
        }

        private void MandelForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            closing = true;
            if (ga != null && ga.Running)
            {
				ga.Stop();
            }
        }
    }
}
