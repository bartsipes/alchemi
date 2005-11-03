using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Windows.Forms;
using Alchemi.Core;
using Alchemi.Core.Owner;

namespace AlchemiRenderer
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : Form
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
		private String runId = "";
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

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
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
			this.pictureBox1.Size = new System.Drawing.Size(600, 488);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// render
			// 
			this.render.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.render.Location = new System.Drawing.Point(488, 512);
			this.render.Name = "render";
			this.render.Size = new System.Drawing.Size(112, 32);
			this.render.TabIndex = 5;
			this.render.Text = "render";
			this.render.Click += new System.EventHandler(this.render_Click);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.Location = new System.Drawing.Point(8, 512);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 23);
			this.label1.TabIndex = 6;
			this.label1.Text = "width";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.Location = new System.Drawing.Point(8, 552);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 23);
			this.label2.TabIndex = 7;
			this.label2.Text = "height";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label3.Location = new System.Drawing.Point(176, 512);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 23);
			this.label3.TabIndex = 8;
			this.label3.Text = "columns";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label4.Location = new System.Drawing.Point(176, 552);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 23);
			this.label4.TabIndex = 9;
			this.label4.Text = "rows";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// widthCombo
			// 
			this.widthCombo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.widthCombo.Location = new System.Drawing.Point(72, 512);
			this.widthCombo.Name = "widthCombo";
			this.widthCombo.Size = new System.Drawing.Size(80, 21);
			this.widthCombo.TabIndex = 10;
			this.widthCombo.Text = "width";
			// 
			// heightCombo
			// 
			this.heightCombo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.heightCombo.Location = new System.Drawing.Point(72, 552);
			this.heightCombo.Name = "heightCombo";
			this.heightCombo.Size = new System.Drawing.Size(80, 21);
			this.heightCombo.TabIndex = 11;
			this.heightCombo.Text = "height";
			// 
			// columnsUpDown
			// 
			this.columnsUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.columnsUpDown.Location = new System.Drawing.Point(240, 512);
			this.columnsUpDown.Name = "columnsUpDown";
			this.columnsUpDown.Size = new System.Drawing.Size(56, 20);
			this.columnsUpDown.TabIndex = 12;
			// 
			// rowsUpDown
			// 
			this.rowsUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.rowsUpDown.Location = new System.Drawing.Point(240, 552);
			this.rowsUpDown.Name = "rowsUpDown";
			this.rowsUpDown.Size = new System.Drawing.Size(56, 20);
			this.rowsUpDown.TabIndex = 13;
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label5.Location = new System.Drawing.Point(344, 520);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(48, 23);
			this.label5.TabIndex = 14;
			this.label5.Text = "model";
			// 
			// modelCombo
			// 
			this.modelCombo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.modelCombo.Location = new System.Drawing.Point(336, 552);
			this.modelCombo.Name = "modelCombo";
			this.modelCombo.Size = new System.Drawing.Size(121, 21);
			this.modelCombo.TabIndex = 15;
			// 
			// stop
			// 
			this.stop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.stop.Location = new System.Drawing.Point(488, 552);
			this.stop.Name = "stop";
			this.stop.Size = new System.Drawing.Size(112, 32);
			this.stop.TabIndex = 16;
			this.stop.Text = "stop";
			this.stop.Click += new System.EventHandler(this.stop_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(616, 589);
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
			this.Name = "Form1";
			this.Text = "Alchemi Renderer";
			this.Load += new System.EventHandler(this.Form1_Load);
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
			Application.Run(new Form1());
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			widthCombo.Items.AddRange(new object[] {"100", "200", "240", "320", "480", "600", "640", "800", "1000"});
			heightCombo.Items.AddRange(new object[] {"100", "200", "240", "320", "480", "600", "640", "800", "1000"});
			widthCombo.SelectedIndex = 1;
			heightCombo.SelectedIndex = 1;

			columnsUpDown.Value = 4;
			columnsUpDown.Maximum = 1000;
			columnsUpDown.Minimum = 1;

            rowsUpDown.Value = 4;
			rowsUpDown.Maximum = 1000;
			rowsUpDown.Minimum = 1;

			paths = new String[] {"../scenes/advanced/chess2.pov +L",
											"../scenes/advanced/biscuit.pov +L",
											"../scenes/advanced/abyss.pov +L",
											"../scenes/advanced/wineglass.pov +L",
											"../scenes/advanced/skyvase.pov +L",
											"../scenes/advanced/diffract.pov +L",
											"../scenes/advanced/fish13/fish13.pov +Lc:/povray3.6/scenes/advanced/fish13/"
										  };
			modelCombo.Items.AddRange(new object[] {"chess",
													"biscuit",
													"abyss",
													"wineglass",
													"skyvase",
													"diffract",
													"fish"}
													);
			modelCombo.SelectedIndex = 0;
		}


		private void showSplash() 
		{
			ResourceManager resources = new ResourceManager(typeof(Form1));
			pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
			pictureBox1.Image = (Image)(resources.GetObject("pictureBox1.Image"));
		}

		private void clearImage() 
		{
			if (imageWidth > 0 && imageHeight > 0) 
			{
				composite = new Bitmap(imageWidth,imageHeight);
				Graphics g = Graphics.FromImage(composite);
				pictureBox1.Image = (Image) composite;
			}
		}

		private void displayImage(String filename) 
		{
			if (!drawnFirstSegment) 
			{
				clearImage();
				drawnFirstSegment = true;
			}
				
			//lock(this)
			{
				string delimStr = " _.png";
				char [] delimiter = delimStr.ToCharArray();
				string [] split = null;
				split = filename.Split(delimiter, 6);
				int col = Int32.Parse(split[0]);
				int row = Int32.Parse(split[1]);

				Graphics g = Graphics.FromImage(composite);
				Bitmap segment = null;
				Rectangle sourceRectangle;
				Rectangle destRectangle;
				int x = 0;
				int y = 0;
				x = (col-1)*segmentWidth;
				y = (row-1)*segmentHeight;

				Console.WriteLine("Displaying segment "+col+" "+row);
				try 
				{
					segment = new Bitmap(runId+"/"+col+"_"+row+".png");
					sourceRectangle = new Rectangle(0, 0, segment.Width, segment.Height);
					destRectangle = new Rectangle(x, y, segment.Width, segment.Height);
					g.DrawImage(segment, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
				}
				catch (Exception e)
				{
					Console.WriteLine("!!!ERROR:\n"+e.StackTrace);
				}
				pictureBox1.Image = (Image) composite;
			}
		}
		
		private void displayImages() 
		{
			Graphics g = Graphics.FromImage(composite);

			Bitmap segment = null;
			Rectangle sourceRectangle;
			Rectangle destRectangle;

			int x = 0;
			int y = 0;

			for (int row=0; row<rows; row++)
			{
				for (int col=0; col<columns; col++)
				{
					x = col*segmentWidth;
					y = row*segmentHeight;
					try 
					{
						segment = new Bitmap(runId+"/"+(col+1)+"_"+(row+1)+".png");
						sourceRectangle = new Rectangle(0, 0, segment.Width, segment.Height);
						destRectangle = new Rectangle(x, y, segment.Width, segment.Height);
						g.DrawImage(segment, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
					}
					catch (Exception e)
					{
						Console.WriteLine("!!!ERROR:\n"+e.StackTrace);
					}
				}
			}
			pictureBox1.Image = (Image) composite;
		}

		private void render_Click(object sender, EventArgs e)
		{
			drawnFirstSegment = false;
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

            Console.WriteLine("WIDTH:"+imageWidth);
			Console.WriteLine("HEIGHT:"+imageHeight);
			Console.WriteLine("COLUMNS:"+columns);
			Console.WriteLine("ROWS:"+rows);
			Console.WriteLine(""+modelPath);

			//RUN ALCHEMI
			runId = ""+DateTime.Now.Ticks;

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

				initted = true;
			}
			
			if (ga!=null && ga.Running)
			{
				ga.Stop();
			}

			for (int col=0; col<columns; col++) 
			{
				for (int row=0; row<rows; row++) 
				{
					x = col*segmentWidth;
					y = row*segmentHeight;
//					String cmd = "c:/povray3.6/bin/pvengine /NR /RENDER ../scenes/advanced/chess2.pov +W"+imageWidth+" +H"+imageHeight+" Start_Row="+(y+1)+" End_Row="+(y+segmentHeight)+" Start_Column="+(x+1)+" End_Column="+(x+segmentWidth)+" Output_File_Name=./"+(col+1)+"_"+(row+1)+"_POV.png -D /EXIT";
//					String cmd = "c:/povray3.6/bin/alchemi_pov "+imageWidth+" "+imageHeight+" "+(y+1)+" "+(y+segmentHeight)+" "+(x+1)+" "+(x+segmentWidth)+" "+(col+1)+"_"+(row+1)+"_POV.png "+(col+1)+" "+(row+1)+" "+segmentWidth+" "+segmentHeight;

					// command to execute on alchemi
					String cmd = "c:/povray3.6/bin/alchemi_pov ";
						cmd+=modelPath+" ";
						// width of the final image
						cmd+=imageWidth+" ";
						// height of the final image
						cmd+=imageHeight+" ";
						// start row
						cmd+=(y+1)+" ";
						// end row
						cmd+=(y+segmentHeight)+" ";
						// start column
						cmd+=(x+1)+" ";
						// end column
						cmd+=(x+segmentWidth)+" ";
						// output file from povray -> input to cropper
						cmd+=(col+1)+"_"+(row+1)+"_POV.png ";
						// column number
						cmd+=(col+1)+" ";
						// row number
						cmd+=(row+1)+" ";
						// width of each segment
						cmd+=segmentWidth+" ";
						// height of each segment
						cmd+=segmentHeight;

					Console.WriteLine(cmd);
					GJob gj = new GJob();
					FileDependencyCollection outputs = new FileDependencyCollection();
					gj.OutputFiles.Add(new EmbeddedFileDependency((col+1)+"_"+(row+1)+".png"));
					gj.RunCommand = cmd;

					ga.Threads.Add(gj);
				}
			}

			try 
			{
				Directory.CreateDirectory(runId); 
				ga.Start();
			} 
			catch (Exception ex)
			{
				Console.WriteLine(""+ex.StackTrace);
				MessageBox.Show("Alchemi Rendering Failed!");
			}
		}

		private void ga_ThreadFinish(GThread thread)
		{
			unpackThread(thread);
		}

		private void unpackThread(GThread thread)
		{
			GJob job = (GJob)thread;
			Console.WriteLine("Thread finished for Job : "+job.Id+", # output files="+job.OutputFiles.Count);
			foreach (FileDependency fd in job.OutputFiles) 
			{ 
				Console.WriteLine("Starting to unpack file {0} for job {1}", fd.FileName, job.Id); 
				if (fd.FileName.EndsWith("png"))
				{
					fd.UnPack(Path.Combine(runId, fd.FileName));
					displayImage(fd.FileName);
				} 
				else 
				{
					fd.UnPack(Path.Combine(runId, job.Id+"_"+fd.FileName));
				}
				Console.WriteLine("Unpacked file {0} for job {1}", fd.FileName, job.Id); 
			}
		}

		private void ga_ThreadFailed(GThread thread, Exception e)
		{
			Console.WriteLine("Job {0} failed.",thread.Id);
		}

		private void ga_ApplicationFinish()
		{
			MessageBox.Show("Application Finished");
		}

		private void stop_Click(object sender, EventArgs e)
		{
			if (ga != null && ga.Running)
			{
				ga.Stop();
			}
		}
	}
}
