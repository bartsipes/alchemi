using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Alchemi.Core.Owner;

namespace Alchemi.Examples.Renderer
{
	/// <summary>
	/// Summary description for RenderThread.
	/// </summary>
	[Serializable]
	public class RenderThread : GThread
	{
		private int _startRowPixel;
		private int _startColPixel;
		private int _endRowPixel;
		private int _endColPixel;

		private string _inputFile;
		private string _megaPOV_Options;

		private int _imageWidth;
		private int _imageHeight;

		private int _segWidth;
		private int _segHeight;

		private int _col; //column of the big image
		private int _row; //row of the big image

		private Bitmap crop;

		//private string _stdout;
		//private string _stderr;

		//private string _bitmapContent;

		private string _basePath; 

		public int Row
		{
			get
			{
				return _row;	
			}
			set
			{
				_row = value;
			}
		}

		public int Col
		{
			get
			{
				return _col;
			}
			set
			{
				_col = value;
			}
		}

		public string BasePath
		{
			get
			{
				return _basePath;
			}
			set
			{
				_basePath = value;
			}
		}

//		public string stdout
//		{
//			get
//			{
//				return _stdout;
//			}
//		}
//
//		public string stderr
//		{
//			get
//			{
//				return _stderr;
//			}
//		}

		public Bitmap RenderedImageSegment
		{
			get
			{
				return crop;
			}
		}

		public string TempFile
		{
			get
			{
				string fileToSave = Col+"_"+Row+".png";
				return fileToSave;
			}
		}

//		public string BitmapContent
//		{
//			get
//			{
//				return _bitmapContent;	
//			}
//		}

		public RenderThread(string InputFile, int ImageWidth, int ImageHeight, int SegmentWidth, int SegmentHeight, int StartRow, int EndRow, int StartCol, int EndCol, string MegaPOV_Options)
		{
			this._inputFile = InputFile;
			this._imageWidth = ImageWidth;
			this._imageHeight = ImageHeight;
			this._segWidth = SegmentWidth;
			this._segHeight = SegmentHeight;
			this._startRowPixel = StartRow;
			this._endRowPixel = EndRow;
			this._startColPixel = StartCol;
			this._endColPixel = EndCol;
			this._megaPOV_Options = MegaPOV_Options;
		}

		public override void Start()
		{
			//do all the rendering by calling the povray stuff, and then crop it and send it back.
			//first call megapov, and render the scence.
			//direct it to save to some filename.

			string tempDir = Path.Combine(_basePath, Path.GetFileName(WorkingDirectory));
			if (!Directory.Exists(tempDir))
				Directory.CreateDirectory(tempDir);
			StreamWriter log = File.CreateText(tempDir+"/tempLog_"+Col+"_"+Row+".txt");

			log.WriteLine("Working dir is "+WorkingDirectory);
			log.WriteLine("TempdIr is "+tempDir);

			string megaPOV_outputFilename = Path.Combine(tempDir, Col+"_"+Row+"_tempPOV.png");
			string cmd = "cmd";
			string args = "/C " + Path.Combine(_basePath,"bin/megapov.exe") +
				string.Format(" +I{0} +O{1} +H{2} +W{3} +SR{4} +ER{5} +SC{6} +EC{7} +FN16 {8}",
					_inputFile, megaPOV_outputFilename,
					_imageHeight, _imageWidth,
					_startRowPixel, _endRowPixel,
					_startColPixel, _endColPixel,
					_megaPOV_Options
				);

			log.WriteLine("Cmd for process is :"+cmd);
			log.WriteLine("Args for process is :"+args);

			Process megapov = new Process();
			megapov.StartInfo.FileName = cmd;
			megapov.StartInfo.Arguments = args;
			megapov.StartInfo.WorkingDirectory = WorkingDirectory;
			megapov.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			megapov.StartInfo.UseShellExecute = true;
			megapov.StartInfo.CreateNoWindow = true;

			megapov.StartInfo.RedirectStandardError = false;
			megapov.StartInfo.RedirectStandardOutput = false;

			megapov.Start();
			megapov.WaitForExit();

			//_stdout = ReadStream(megapov.StandardOutput);
			//_stderr = ReadStream(megapov.StandardError);
			
			//then crop the image produced by megapov, and get it back.
			int x = (Col-1)*_segWidth;

			if (File.Exists(megaPOV_outputFilename))
			{
				Bitmap im = new Bitmap(megaPOV_outputFilename);
				crop = new Bitmap(_segWidth,_segHeight);
				Graphics g = Graphics.FromImage(crop);

				Rectangle sourceRectangle = new Rectangle(x, 0, _segWidth, _segHeight);
				Rectangle destRectangle = new Rectangle(0, 0, _segWidth, _segHeight);
				g.DrawImage(im,destRectangle,sourceRectangle,GraphicsUnit.Pixel);

//				crop.Save(Path.Combine(tempDir,TempFile),ImageFormat.Png);

//				_bitmapContent = ReadFile(Path.Combine(tempDir,TempFile));
			}

			log.Flush();
			log.Close();
			log = null;
		}

//		private string ReadFile(string filename)
//		{
//			string result = null;
//			StreamReader sr =  new StreamReader(filename);
//			result = sr.ReadToEnd();
//			return result;
//		}
//
//		private string ReadStream(System.IO.StreamReader sr)
//		{
//			string result = null;
//			result = sr.ReadToEnd();
//			return result;
//		}
	}
}
