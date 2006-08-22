using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Alchemi.Core.Owner;
using System.Text;
using System.Threading;
using System.Net;

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

		[NonSerialized]private StringBuilder output;
		[NonSerialized]private StringBuilder error;
        [NonSerialized]
        private Process megapov = null;

        private string _stdout;
        private string _stderr;

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
				return Environment.ExpandEnvironmentVariables(_basePath);
			}
			set
			{
				_basePath = value;
			}
		}

        public string Stdout
        {
            get
            {
                return _stdout;
            }
        }

        public string Stderr
        {
            get
            {
                return _stderr;
            }
        }

		public Bitmap RenderedImageSegment
		{
			get
			{
				return crop;
			}
		}

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
            string outfilename = null;
            try
            {
                outfilename = string.Format("{0}_{1}_tempPOV.png", Col, Row);

                string cmd = "cmd";
                string args = "/C " + Path.Combine(BasePath, @"bin\megapov.exe") +
                    string.Format(" +I{0} +O{1} +H{2} +W{3} +SR{4} +ER{5} +SC{6} +EC{7} +FN16 {8}",
                        Environment.ExpandEnvironmentVariables(_inputFile), outfilename,
                        _imageHeight, _imageWidth,
                        _startRowPixel, _endRowPixel,
                        _startColPixel, _endColPixel,
                        _megaPOV_Options
                    );

                output = new StringBuilder();
                error = new StringBuilder();

                //no need to lock output here, since so far there won't be more than one thread
                //using it.
                output.AppendLine("**** BasePath is " + BasePath);
                output.AppendLine("**** Working dir is " + WorkingDirectory);
                output.AppendLine("**** Cmd for process is :" + cmd);
                output.AppendLine("**** Args for process is :" + args);

                megapov = new Process();
                megapov.StartInfo.FileName = cmd;
                megapov.StartInfo.Arguments = args;
                megapov.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                megapov.StartInfo.UseShellExecute = false; //false, since we dont want WorkingDir to be used to find the exe
                megapov.StartInfo.WorkingDirectory = WorkingDirectory;
                megapov.StartInfo.CreateNoWindow = true;
                megapov.EnableRaisingEvents = true;

                megapov.StartInfo.RedirectStandardError = true;
                megapov.StartInfo.RedirectStandardOutput = true;

                megapov.OutputDataReceived += new DataReceivedEventHandler(megapov_OutputDataReceived);
                megapov.ErrorDataReceived += new DataReceivedEventHandler(megapov_ErrorDataReceived);
                megapov.Exited += new EventHandler(megapov_Exited);
                megapov.Start();

                megapov.BeginErrorReadLine();
                megapov.BeginOutputReadLine();

                while (!megapov.HasExited)
                {
                    //since we are getting notified async.
                    megapov.WaitForExit(1000);
                }

                if (megapov.HasExited)
                {
                    LogOutput(
                        string.Format("******** MegaPov Out of wait loop... time: {0}, exit code: {1}",
                            Environment.TickCount,
                            megapov.ExitCode)
                    );
                }
            }
            catch (Exception ex)
            {
                LogError(ex.ToString());
            }
            finally
            {
                CloseProcess();
                try
                {
                    CropImage(Path.Combine(WorkingDirectory, outfilename));
                }
                catch (Exception ex)
                {
                    LogError(ex.ToString());
                }
                _stdout = output.ToString();
                _stderr = error.ToString();
            }
        }

        private void CloseProcess()
        {
            try
            {
                if (megapov != null)
                {
                    //time to kill!
                    if (!megapov.HasExited)
                    {
                        megapov.Kill();
                    }
                    megapov.Close();
                    megapov.Dispose();
                    megapov = null;
                }
            }
            catch { }

        }

        private void CropImage(string imageFile)
        {
            Bitmap im = null;
            try
            {
                //then crop the image produced by megapov, and get it back.
                int x = (Col - 1) * _segWidth;
                im = new Bitmap(imageFile);
                crop = new Bitmap(_segWidth, _segHeight);
                using (Graphics g = Graphics.FromImage(crop))
                {
                    Rectangle sourceRectangle = new Rectangle(x, 0, _segWidth, _segHeight);
                    Rectangle destRectangle = new Rectangle(0, 0, _segWidth, _segHeight);
                    g.DrawImage(im, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
                }

                LogOutput(string.Format("Cropped ImageFile : {0}", imageFile));
            }
            catch (Exception ex)
            {
                LogError("Error cropping file : " + imageFile + "\n" + ex.ToString());
            }
            finally
            {
                try
                {
                    if (im != null)
                    {
                        im.Dispose();
                    }
                }
                catch { }
            }
        }

        private string GetIPAddressString()
        {
            StringBuilder ip = new StringBuilder();
            try
            {
                ip.AppendLine(string.Format("Host : {0}", Dns.GetHostName()));
                IPAddress[] addresses = Dns.GetHostAddresses(Dns.GetHostName());
                foreach (IPAddress ipaddr in addresses)
                {
                    ip.AppendLine(string.Format(", IP : {0}", ipaddr.ToString()));
                }
            }
            catch { }
            return ip.ToString();
        }

        //Thread-safe error-logging.
        private void LogError(string e)
        {
            if (error != null)
            {
                lock (error)
                {
                    error.AppendLine(e);
                }
            }
        }

        //Thread-safe output-logging.
        private void LogOutput(string o)
        {
            if (output != null)
            {
                lock (output)
                {
                    output.AppendLine(o);
                }
            }
        }

        #region MegaPov Process Events
        void megapov_Exited(object sender, EventArgs e)
        {
            try
            {
                LogOutput(string.Format("********** Megapov process has exited at {0}, on {1}",
                            Environment.TickCount,
                            GetIPAddressString()));
            }
            catch { }
        }

        void megapov_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            LogError(e.Data);
        }

        void megapov_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            LogOutput(e.Data);
        }
        #endregion
    }
}
