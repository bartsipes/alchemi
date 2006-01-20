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
// Configure log4net using the .config file
[assembly: XmlConfigurator(Watch=true)]
namespace Alchemi.Examples.Renderer
{
	/// <summary>
	/// Summary description for RendererForm.
	/// </summary>
	public class RendererForm : Form
	{
		private PictureBox pictureBox1;
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

		//private String runId = ""; //not used anymore. we directly read/write from thread objects, not files

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
		private CheckBox stretchCheckBox;

		// Create a logger for use in this class
		private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private ProgressBar pbar;
		private Label lbProgress;

		private string basepath;

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(RendererForm));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
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
			this.stretchCheckBox = new System.Windows.Forms.CheckBox();
			this.pbar = new System.Windows.Forms.ProgressBar();
			this.lbProgress = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.columnsUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.rowsUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox1.BackColor = System.Drawing.Color.Black;
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(8, 8);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(608, 480);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// render
			// 
			this.render.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.render.Location = new System.Drawing.Point(488, 536);
			this.render.Name = "render";
			this.render.Size = new System.Drawing.Size(120, 32);
			this.render.TabIndex = 5;
			this.render.Text = "render";
			this.render.Click += new System.EventHandler(this.render_Click);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.Location = new System.Drawing.Point(8, 536);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 23);
			this.label1.TabIndex = 6;
			this.label1.Text = "width";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.Location = new System.Drawing.Point(8, 576);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 23);
			this.label2.TabIndex = 7;
			this.label2.Text = "height";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label3.Location = new System.Drawing.Point(176, 536);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 23);
			this.label3.TabIndex = 8;
			this.label3.Text = "columns";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label4.Location = new System.Drawing.Point(176, 576);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 23);
			this.label4.TabIndex = 9;
			this.label4.Text = "rows";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// widthCombo
			// 
			this.widthCombo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.widthCombo.Location = new System.Drawing.Point(72, 536);
			this.widthCombo.Name = "widthCombo";
			this.widthCombo.Size = new System.Drawing.Size(80, 21);
			this.widthCombo.TabIndex = 10;
			this.widthCombo.Text = "width";
			// 
			// heightCombo
			// 
			this.heightCombo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.heightCombo.Location = new System.Drawing.Point(72, 576);
			this.heightCombo.Name = "heightCombo";
			this.heightCombo.Size = new System.Drawing.Size(80, 21);
			this.heightCombo.TabIndex = 11;
			this.heightCombo.Text = "height";
			// 
			// columnsUpDown
			// 
			this.columnsUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.columnsUpDown.Location = new System.Drawing.Point(240, 536);
			this.columnsUpDown.Name = "columnsUpDown";
			this.columnsUpDown.Size = new System.Drawing.Size(56, 20);
			this.columnsUpDown.TabIndex = 12;
			// 
			// rowsUpDown
			// 
			this.rowsUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.rowsUpDown.Location = new System.Drawing.Point(240, 576);
			this.rowsUpDown.Name = "rowsUpDown";
			this.rowsUpDown.Size = new System.Drawing.Size(56, 20);
			this.rowsUpDown.TabIndex = 13;
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label5.Location = new System.Drawing.Point(320, 536);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(48, 23);
			this.label5.TabIndex = 14;
			this.label5.Text = "model";
			// 
			// modelCombo
			// 
			this.modelCombo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.modelCombo.Location = new System.Drawing.Point(360, 536);
			this.modelCombo.Name = "modelCombo";
			this.modelCombo.Size = new System.Drawing.Size(121, 21);
			this.modelCombo.TabIndex = 15;
			// 
			// stop
			// 
			this.stop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.stop.Enabled = false;
			this.stop.Location = new System.Drawing.Point(488, 576);
			this.stop.Name = "stop";
			this.stop.Size = new System.Drawing.Size(120, 32);
			this.stop.TabIndex = 16;
			this.stop.Text = "stop";
			this.stop.Click += new System.EventHandler(this.stop_Click);
			// 
			// stretchCheckBox
			// 
			this.stretchCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.stretchCheckBox.Location = new System.Drawing.Point(352, 576);
			this.stretchCheckBox.Name = "stretchCheckBox";
			this.stretchCheckBox.TabIndex = 18;
			this.stretchCheckBox.Text = "stretch image";
			this.stretchCheckBox.CheckedChanged += new System.EventHandler(this.stretchCheckBox_CheckedChanged);
			// 
			// pbar
			// 
			this.pbar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pbar.Location = new System.Drawing.Point(192, 496);
			this.pbar.Name = "pbar";
			this.pbar.Size = new System.Drawing.Size(424, 23);
			this.pbar.TabIndex = 19;
			// 
			// lbProgress
			// 
			this.lbProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lbProgress.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lbProgress.Location = new System.Drawing.Point(8, 496);
			this.lbProgress.Name = "lbProgress";
			this.lbProgress.Size = new System.Drawing.Size(176, 23);
			this.lbProgress.TabIndex = 20;
			this.lbProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// RendererForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(624, 613);
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
			this.Controls.Add(this.pictureBox1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "RendererForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Alchemi Renderer";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.RendererForm_Closing);
			this.Load += new System.EventHandler(this.RenderForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.columnsUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.rowsUpDown)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new RendererForm());
		}

		private void RenderForm_Load(object sender, EventArgs e)
		{
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(DefaultErrorHandler);
			
			//for windows forms apps unhandled exceptions on the main thread
			Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
   
			widthCombo.Items.AddRange(new object[] {"100", "160", "200", "240", "320", "480", "512", "600", "640", "800", "1024", "1280"});
			heightCombo.Items.AddRange(new object[] {"100", "120", "200", "240", "320", "384", "480", "600", "640", "768", "800", "1024"});
			widthCombo.SelectedIndex = 4;
			heightCombo.SelectedIndex = 3;

			columnsUpDown.Value = 4;
			columnsUpDown.Maximum = 1000;
			columnsUpDown.Minimum = 1;

            rowsUpDown.Value = 4;
			rowsUpDown.Maximum = 1000;
			rowsUpDown.Minimum = 1;

			basepath = "C:/povray3.6";

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
			modelCombo.SelectedIndex = 0;
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
			pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
			pictureBox1.Image = (Image)(resources.GetObject("pictureBox1.Image"));
		}

		private void clearImage() 
		{
			if (imageWidth > 0 && imageHeight > 0) 
			{
				composite = new Bitmap(imageWidth,imageHeight);
				//Graphics g = Graphics.FromImage(composite);
				if (stretchCheckBox.Checked) 
				{
					pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
				} 
				else 
				{
					pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
				}
				pictureBox1.Image = composite;
			}
		}

		private void displayImage(Bitmap segment, int col, int row)
		{
			if (!drawnFirstSegment) 
			{
				clearImage();
				drawnFirstSegment = true;
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

		#region old code
//		private void displayImageOLD(String filename, int col, int row) 
//		{
//			try
//			{
//				Bitmap segment = new Bitmap(Path.Combine(runId, filename));
//				displayImage(segment, col, row);
//			}
//			catch (Exception ex)
//			{
//				logger.Debug(ex.ToString());
//			}
//		}
//		
//		private void displayImagesOLD() 
//		{
//			Graphics g = Graphics.FromImage(composite);
//
//			Bitmap segment = null;
//			Rectangle sourceRectangle;
//			Rectangle destRectangle;
//
//			int x = 0;
//			int y = 0;
//
//			for (int row=0; row<rows; row++)
//			{
//				for (int col=0; col<columns; col++)
//				{
//					x = col*segmentWidth;
//					y = row*segmentHeight;
//					try 
//					{
//						segment = new Bitmap(runId+"/"+(col+1)+"_"+(row+1)+".png");
//						sourceRectangle = new Rectangle(0, 0, segment.Width, segment.Height);
//						destRectangle = new Rectangle(x, y, segment.Width, segment.Height);
//						g.DrawImage(segment, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
//					}
//					catch (Exception e)
//					{
//						logger.Debug("!!!ERROR:\n"+e.StackTrace);
//					}
//				}
//			}
//			pictureBox1.Image = composite;
//		}

		#endregion

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

			//RUN ALCHEMI
			//runId = ""+DateTime.Now.Ticks;

			// reset the display
			clearImage();
			
			if (!initted)
			{
				GConnectionDialog gcd = new GConnectionDialog();
				gcd.ShowDialog();

				ga = new GApplication(true);
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
			lbProgress.Text = "";

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
				//Directory.CreateDirectory(runId); 
				ga.Start();
			} 
			catch (Exception ex)
			{
				Console.WriteLine(""+ex.StackTrace);
				MessageBox.Show("Alchemi Rendering Failed!"+ex.ToString());
			}
		}

		private void ga_ThreadFinish(GThread thread)
		{
			logger.Debug("Thread finished: "+thread.Id);
			UpdateStatus();
			unpackThread(thread);
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
					//				if (logger.IsDebugEnabled)
					//				{
					//					string filename = Path.Combine(runId, rth.TempFile); 
					//					bit.Save(filename, ImageFormat.Png);
					//				}
				}
				else
				{
					logger.Debug ("bit is null! " + thread.Id );
				}


//				if (bit==null)
//				{
//					logger.Error("SERIOUS ERROR: bit is null!");
//					StreamWriter sw = new StreamWriter(filename);
//					sw.Write(rth.BitmapContent);
//					sw.Flush();
//					sw.Close();
//					displayImage(filename, rth.Col,  rth.Row);
//				}
//				logger.Debug(" filename="+filename);

//				logger.Debug("stdout from thread: "+rth.stdout);
//				logger.Debug("stderr from thread: "+rth.stderr);
			}
		}

		private void ga_ThreadFailed(GThread thread, Exception e)
		{
			logger.Debug("Thread failed: "+thread.Id + "\n"+e.ToString());
			UpdateStatus();
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

		private void stop_Click(object sender, EventArgs e)
		{
			StopApp();
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

	}
}
