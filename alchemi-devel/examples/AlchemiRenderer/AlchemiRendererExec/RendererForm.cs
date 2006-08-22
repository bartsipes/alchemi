using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using Alchemi.Core;
using Alchemi.Core.Owner;
using log4net;
using log4net.Config;
using AlchemiRenderer;
// Configure log4net using the .config file
[assembly: XmlConfigurator(Watch=true)]
namespace Alchemi.Examples.Renderer
{
	/// <summary>
	/// Summary description for RendererForm.
	/// </summary>
	public class RendererForm : Form
    {
		private Button render;
		private Label label1;
		private Label label2;
		private Label label3;
		private Label label4;

		private int imageWidth = 0;
		private int imageHeight = 0;
		private int columns = 0;
		private int rows = 0;
		private int segmentWidth = 0;
		private int segmentHeight = 0;

		private GApplication ga = null;
		private bool initted = false;

		private Bitmap composite = null;
		private String modelPath = "";
		private String[] paths = null;
		private bool drawnFirstSegment = false;

		private ComboBox widthCombo;
		private ComboBox heightCombo;
		private NumericUpDown columnsUpDown;
		private NumericUpDown rowsUpDown;
		private Label label5;
		private ComboBox modelCombo;
        private Button stop;

		// Create a logger for use in this class
		private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private ProgressBar pbar;
		private Label lbProgress;

        private string basepath = "%POVRAY_HOME%";

        private MenuStrip menuStrip1;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;

        private object threadCompleteLock = new object();
        private CheckBox stretchCheckBox;

        private Image busyGif;
        private EventHandler busyHandler;
        private Panel panel1;
        private PictureBox pictureBox1;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		public RendererForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			Logger.LogHandler += new LogEventHandler(LogHandler);
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
					logger.Warn(e.Message, e.Exception);
					break;
			}
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RendererForm));
            this.render = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.widthCombo = new System.Windows.Forms.ComboBox();
            this.heightCombo = new System.Windows.Forms.ComboBox();
            this.columnsUpDown = new System.Windows.Forms.NumericUpDown();
            this.rowsUpDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.modelCombo = new System.Windows.Forms.ComboBox();
            this.stop = new System.Windows.Forms.Button();
            this.pbar = new System.Windows.Forms.ProgressBar();
            this.lbProgress = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stretchCheckBox = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.columnsUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rowsUpDown)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // render
            // 
            this.render.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.render.Location = new System.Drawing.Point(513, 560);
            this.render.Name = "render";
            this.render.Size = new System.Drawing.Size(100, 30);
            this.render.TabIndex = 5;
            this.render.Text = "render";
            this.render.Click += new System.EventHandler(this.render_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 569);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "width";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 599);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "height";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(143, 569);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "columns";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(160, 599);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "rows";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // widthCombo
            // 
            this.widthCombo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.widthCombo.Location = new System.Drawing.Point(57, 566);
            this.widthCombo.Name = "widthCombo";
            this.widthCombo.Size = new System.Drawing.Size(80, 21);
            this.widthCombo.TabIndex = 10;
            this.widthCombo.Text = "width";
            // 
            // heightCombo
            // 
            this.heightCombo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.heightCombo.Location = new System.Drawing.Point(57, 596);
            this.heightCombo.Name = "heightCombo";
            this.heightCombo.Size = new System.Drawing.Size(80, 21);
            this.heightCombo.TabIndex = 11;
            this.heightCombo.Text = "height";
            // 
            // columnsUpDown
            // 
            this.columnsUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.columnsUpDown.Location = new System.Drawing.Point(195, 567);
            this.columnsUpDown.Name = "columnsUpDown";
            this.columnsUpDown.Size = new System.Drawing.Size(56, 20);
            this.columnsUpDown.TabIndex = 12;
            // 
            // rowsUpDown
            // 
            this.rowsUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rowsUpDown.Location = new System.Drawing.Point(195, 597);
            this.rowsUpDown.Name = "rowsUpDown";
            this.rowsUpDown.Size = new System.Drawing.Size(56, 20);
            this.rowsUpDown.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(279, 569);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "model";
            // 
            // modelCombo
            // 
            this.modelCombo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.modelCombo.Location = new System.Drawing.Point(320, 566);
            this.modelCombo.Name = "modelCombo";
            this.modelCombo.Size = new System.Drawing.Size(177, 21);
            this.modelCombo.TabIndex = 15;
            // 
            // stop
            // 
            this.stop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.stop.Enabled = false;
            this.stop.Location = new System.Drawing.Point(513, 598);
            this.stop.Name = "stop";
            this.stop.Size = new System.Drawing.Size(100, 30);
            this.stop.TabIndex = 16;
            this.stop.Text = "stop";
            this.stop.Click += new System.EventHandler(this.stop_Click);
            // 
            // pbar
            // 
            this.pbar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pbar.Location = new System.Drawing.Point(4, 537);
            this.pbar.Name = "pbar";
            this.pbar.Size = new System.Drawing.Size(614, 12);
            this.pbar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbar.TabIndex = 19;
            // 
            // lbProgress
            // 
            this.lbProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbProgress.AutoSize = true;
            this.lbProgress.Location = new System.Drawing.Point(6, 519);
            this.lbProgress.Name = "lbProgress";
            this.lbProgress.Size = new System.Drawing.Size(79, 13);
            this.lbProgress.TabIndex = 20;
            this.lbProgress.Text = "Not Rendering.";
            this.lbProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(624, 24);
            this.menuStrip1.TabIndex = 21;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(35, 20);
            this.toolStripMenuItem1.Text = "&File";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.optionsToolStripMenuItem.Text = "&Options...";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // stretchCheckBox
            // 
            this.stretchCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.stretchCheckBox.AutoSize = true;
            this.stretchCheckBox.Location = new System.Drawing.Point(282, 596);
            this.stretchCheckBox.Name = "stretchCheckBox";
            this.stretchCheckBox.Size = new System.Drawing.Size(89, 17);
            this.stretchCheckBox.TabIndex = 18;
            this.stretchCheckBox.Text = "stretch image";
            this.stretchCheckBox.Visible = false;
            this.stretchCheckBox.CheckedChanged += new System.EventHandler(this.stretchCheckBox_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.AutoScrollMargin = new System.Drawing.Size(2, 2);
            this.panel1.AutoScrollMinSize = new System.Drawing.Size(512, 480);
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(4, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(616, 488);
            this.panel1.TabIndex = 22;
            this.panel1.Resize += new System.EventHandler(this.panel1_Resize);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(127, 69);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(382, 289);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // RendererForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.AutoScrollMargin = new System.Drawing.Size(4, 4);
            this.ClientSize = new System.Drawing.Size(624, 635);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lbProgress);
            this.Controls.Add(this.pbar);
            this.Controls.Add(this.stretchCheckBox);
            this.Controls.Add(this.stop);
            this.Controls.Add(this.modelCombo);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.rowsUpDown);
            this.Controls.Add(this.columnsUpDown);
            this.Controls.Add(this.heightCombo);
            this.Controls.Add(this.widthCombo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.render);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(632, 662);
            this.Name = "RendererForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alchemi Renderer";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.RendererForm_Closing);
            this.Load += new System.EventHandler(this.RenderForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.columnsUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rowsUpDown)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void RenderForm_Load(object sender, EventArgs e)
		{
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(DefaultErrorHandler);
			
			//for windows forms apps unhandled exceptions on the main thread
			Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
   
			widthCombo.Items.AddRange(new object[] {"100", "160", "200", "240", "320", "480", "512", "600", "640", "800", "1024", "1280"});
			heightCombo.Items.AddRange(new object[] {"100", "120", "200", "240", "320", "384", "480", "600", "640", "768", "800", "1024"});
			widthCombo.SelectedIndex = 9;
			heightCombo.SelectedIndex = 8;

			columnsUpDown.Value = 6;
			columnsUpDown.Maximum = 100;
			columnsUpDown.Minimum = 1;

            rowsUpDown.Value = 5;
			rowsUpDown.Maximum = 100;
			rowsUpDown.Minimum = 1;

            busyGif = Image.FromFile("status_anim.gif");
            busyHandler = new EventHandler(this.OnFrameChanged);

			paths = new String[] {basepath+"/scenes/advanced/chess2.pov +L"+basepath+"/include",
											basepath+"/scenes/advanced/isocacti.pov +L"+basepath+"/include",
											basepath+"/scenes/advanced/glasschess/glasschess.pov +L"+basepath+"/scenes/advanced/glasschess/",
											basepath+"/scenes/advanced/biscuit.pov +L"+basepath+"/include",
											basepath+"/scenes/advanced/landscape.pov +L"+basepath+"/include",
											basepath+"/scenes/advanced/mediasky.pov +L"+basepath+"/include",
											basepath+"/scenes/advanced/abyss.pov +L"+basepath+"/include",
											basepath+"/scenes/advanced/wineglass.pov +L"+basepath+"/include",
											basepath+"/scenes/advanced/skyvase.pov +L"+basepath+"/include",
											basepath+"/scenes/advanced/newdiffract.pov +L"+basepath+"/include",
											basepath+"/scenes/advanced/quilt1.pov +L"+basepath+"/include"//,
											//basepath+"/scenes/advanced/fish13/fish13.pov +L"+basepath+"scenes/advanced/fish13/"
										  };
			modelCombo.Items.AddRange(new object[] {"chess",
												    "cacti",
												    "glass chess",
													"biscuits",
												    "landscape",
												    "mediasky",
													"abyss",
													"wineglass",
													"skyvase",
													"diffract",
												    "ball"//,
													//"fish"
													}
													);
			modelCombo.SelectedIndex = 5;
		}

		private void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			HandleAllUnknownErrors(sender.ToString(),e.Exception);
		}

		private void DefaultErrorHandler(object sender, UnhandledExceptionEventArgs args)
		{
			Exception e = (Exception) args.ExceptionObject;
			HandleAllUnknownErrors(sender.ToString(),e);
		}

		private void HandleAllUnknownErrors(string sender, Exception e)
		{
			logger.Error("Unknown Error from: " + sender,e);
			MessageBox.Show(e.ToString(), "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private void showSplash() 
		{
			ResourceManager resources = new ResourceManager(typeof(RendererForm));
			pictureBox1.Image = (Image)(resources.GetObject("pictureBox1.Image"));
		}
		private void clearImage() 
		{
			if (imageWidth > 0 && imageHeight > 0) 
			{
				composite = new Bitmap(imageWidth,imageHeight);
				pictureBox1.Image = composite;
			}
		}
		private void displayImage(Bitmap segment, int col, int row)
		{
			if (!drawnFirstSegment) 
			{
				clearImage();
				drawnFirstSegment = true;
                //auto-size picture box to image size.
                //if it is bigger
                pictureBox1.Size = new Size(imageWidth, imageHeight);
                panel1.AutoScrollMinSize = new Size(imageWidth, imageHeight);
			}

			Graphics g = Graphics.FromImage(composite);
			Rectangle sourceRectangle;
			Rectangle destRectangle;
			int x = 0;
			int y = 0;
			x = (col-1)*segmentWidth;
			y = (row-1)*segmentHeight;

			logger.Debug("Displaying segment c, r: "+col+", "+row);
			try 
			{
				sourceRectangle = new Rectangle(0, 0, segment.Width, segment.Height);
				destRectangle = new Rectangle(x, y, segment.Width, segment.Height);
				g.DrawImage(segment, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
			}
			catch (Exception e)
			{
				logger.Debug("!!!ERROR:\n"+e.StackTrace);
			}
			pictureBox1.Image = composite;

		}
        private void printStdFiles(RenderThread thread)
        {
            try
            {
                logger.Info("Output : " + thread.Stdout);
                logger.Info("Error : " + thread.Stderr);
            }
            catch (Exception ex)
            {
                logger.Error("Error getting stdfiles from thread", ex);
            }
        }
		private void unpackThread(GThread thread)
		{
			RenderThread rth = (RenderThread)thread;
			if (rth!=null)
			{
				Bitmap bit = rth.RenderedImageSegment;
				if (bit!=null)
				{
					logger.Debug("Loading from bitmap");
					displayImage(bit, rth.Col, rth.Row);
				}
				else
				{
					logger.Debug ("bit is null! " + thread.Id );
				}
			}
		}
		private void UpdateStatus()
		{
			if (pbar.Value==pbar.Maximum)
			{
				lbProgress.Text = string.Format("All {0} threads completed.",pbar.Maximum);
			}
			else if (pbar.Value < pbar.Maximum)
			{
				pbar.Increment(1);
				lbProgress.Text = string.Format("Thread {0} of {1} completed.",pbar.Value, pbar.Maximum);			
			}
		}
		private void StopApp()
		{
			try
			{
				if (ga != null && ga.Running)
				{
					ga.Stop();
					logger.Debug("Application stopped.");
				}
				else
				{
					if (ga == null)
					{
						logger.Debug("ga is null");
					}
					else
					{
						logger.Debug("ga running returned false...");
					}
				}

				stop.Enabled = false;
				render.Enabled = !stop.Enabled;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error stopping application: "+ex.Message);
			}
        }

        #region BusyGIFAnimation
        private void ShowBusyGif()
        {
            pictureBox1.Image = busyGif;
            ImageAnimator.Animate(busyGif, busyHandler);
        }
        //private void StopBusyGif()
        //{
        //    ImageAnimator.StopAnimate(busyGif, busyHandler);
        //}
        private void OnFrameChanged(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!drawnFirstSegment)
            {
                //Get the next frame ready for rendering.
                ImageAnimator.UpdateFrames();

                //Draw the next frame in the animation.
                //pictureBox1.Image = busyGif;
                //Graphics g = pictureBox1.CreateGraphics();
                //g.DrawImage(busyGif, );
            }
        }
        #endregion

        #region Alchemi Grid Events
        private void ga_ThreadFinish(GThread thread)
		{
            //to prevent multiple events from over-writing each other
            lock (threadCompleteLock)
            {
                logger.Debug("Thread finished: " + thread.Id);
                UpdateStatus();
                unpackThread(thread);
                printStdFiles(thread as RenderThread);
            }
		}
		private void ga_ThreadFailed(GThread thread, Exception e)
		{
            //to prevent multiple events from over-writing each other
            lock (threadCompleteLock)
            {
                logger.Debug("Thread failed: " + thread.Id + "\n" + e.ToString());
                UpdateStatus();
                printStdFiles(thread as RenderThread);
            }
		}
		private void ga_ApplicationFinish()
		{
			//initted = false;
			UpdateStatus();
			logger.Debug("Application Finished");
			//displayImages();
			MessageBox.Show("Rendering Finished");
			//displayImages();

			stop.Enabled = false;
			render.Enabled = !stop.Enabled;

        }
        #endregion

        #region Form Events


		private void render_Click(object sender, EventArgs e)
		{
			stop.Enabled = true;
			render.Enabled = !stop.Enabled;
			
			drawnFirstSegment = false;
			showSplash();

			// model path
			modelPath = paths[modelCombo.SelectedIndex];
			// get width and height from combo box
			imageWidth = Int32.Parse(widthCombo.SelectedItem.ToString());
			imageHeight = Int32.Parse(heightCombo.SelectedItem.ToString());

			// get cols and rows from up downs
			columns = Decimal.ToInt32(columnsUpDown.Value);
			rows = Decimal.ToInt32(rowsUpDown.Value);

			segmentWidth = imageWidth/columns;
			segmentHeight = imageHeight/rows;

			int x = 0;
			int y = 0;

            logger.Debug("WIDTH:"+imageWidth);
			logger.Debug("HEIGHT:"+imageHeight);
			logger.Debug("COLUMNS:"+columns);
			logger.Debug("ROWS:"+rows);
			logger.Debug(""+modelPath);

			// reset the display
			clearImage();
			
			if (!initted)
			{
				GConnectionDialog gcd = new GConnectionDialog();
				gcd.ShowDialog();

				ga = new GApplication(true);
                ga.ApplicationName = "Alchemi POV-Ray Renderer - Alchemi sample";
				ga.Connection = gcd.Connection;
				ga.ThreadFinish += new GThreadFinish(ga_ThreadFinish);
				ga.ThreadFailed += new GThreadFailed(ga_ThreadFailed);
				ga.ApplicationFinish += new GApplicationFinish(ga_ApplicationFinish);

				ga.Manifest.Add(new ModuleDependency(typeof(RenderThread).Module));

				initted = true;
			}
			
			if (ga!=null && ga.Running)
			{
				ga.Stop();
			}

			pbar.Maximum = columns*rows;
			pbar.Minimum = 0;
			pbar.Value = 0;
			lbProgress.Text = "Starting to render image ... ";

			for (int col=0; col<columns; col++) 
			{
				for (int row=0; row<rows; row++) 
				{
					x = col*segmentWidth;
					y = row*segmentHeight;

					int startRowPixel = y + 1;
					int endRowPixel = y + segmentHeight;
					int startColPixel = x + 1;
					int endColPixel = x + segmentWidth;

					RenderThread rth = new RenderThread(modelPath, 
						imageWidth, imageHeight, 
						segmentWidth, segmentHeight, 
						startRowPixel, endRowPixel, 
						startColPixel, endColPixel, 
						"");

					rth.BasePath = this.basepath;
					rth.Col = col+1;
					rth.Row = row+1;

					ga.Threads.Add(rth);

				}
			}

			try 
			{
				ga.Start();
			} 
			catch (Exception ex)
			{
				Console.WriteLine(""+ex.StackTrace);
				MessageBox.Show("Alchemi Rendering Failed!"+ex.ToString());
			}

            lbProgress.Text = "Rendering image ... ";
            ShowBusyGif();
        }
        
        private void stop_Click(object sender, EventArgs e)
		{
			StopApp();
		}

		private void stretchCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (stretchCheckBox.Checked) 
			{
				pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
			} 
			else 
			{
				pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
			}
		}

		private void RendererForm_Closing(object sender, CancelEventArgs e)
		{
			StopApp();
		}

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options opt = new Options();
            opt.txPath.Text = basepath;
            opt.ShowDialog(this);
            basepath = opt.txPath.Text;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopApp();
            this.Close();
        }
        
        private void panel1_Resize(object sender, EventArgs e)
        {
            //set the pic box to the middle of the panel.
            int x = (panel1.Width - pictureBox1.Width) / 2;
            int y = (panel1.Height - pictureBox1.Height) / 2;
            pictureBox1.Location = new Point(x, y);
        }
        #endregion


    }
}
