#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	GJob.cs
* Project		:	Alchemi Core
* Created on	:	2003
* Copyright		:	Copyright © 2006 The University of Melbourne
*					This technology has been developed with the support of 
*					the Australian Research Council and the University of Melbourne
*					research grants as part of the Gridbus Project
*					within GRIDS Laboratory at the University of Melbourne, Australia.
* Author         :  Akshay Luther (akshayl@csse.unimelb.edu.au), Rajkumar Buyya (raj@csse.unimelb.edu.au), and Krishna Nadiminti (kna@csse.unimelb.edu.au)
* License        :  GPL
*					This program is free software; you can redistribute it and/or 
*					modify it under the terms of the GNU General Public
*					License as published by the Free Software Foundation;
*					See the GNU General Public License 
*					(http://www.gnu.org/copyleft/gpl.html) for more details.
*
*/ 
#endregion
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace Alchemi.Core.Owner
{
    public struct JobVariables
    {
        public static string WorkingDirectory = "${WorkingDirectory}";
    }

	/// <summary>
	/// Represents a coarse unit of work on the grid. This class extends the GThread to enable legacy applications to 
	/// run on the grid. A GJob is associated with file dependencies which are the inputs and outputs of the job
	/// and a compiled binary (the legacy application) which is the work unit of the job.
	/// </summary>
    [Serializable]
    public class GJob : GThread
    {
		private FileDependencyCollection _InputFiles = new FileDependencyCollection();
        private FileDependencyCollection _OutputFiles = new FileDependencyCollection();
        private string _RunCommand;

        private string _Stdout;
        private string _Stderr;
        private string _Log;

        [NonSerialized]
        private StringBuilder output;
        [NonSerialized]
        private StringBuilder error;
        [NonSerialized]
        private StringBuilder log;
        [NonSerialized]
        private Process process = null;

        /// <summary>
        /// Gets the alchemi log messages, that were output during the job execution.
        /// </summary>
        public string Log
        {
            get
            {
                return _Log;
            }
        }

        /// <summary>
        /// Gets the entire standard output text of the job.
        /// </summary>
        public string Stdout
        {
            get
            {
                return _Stdout;
            }
        }
        /// <summary>
        /// Gets the entire standard error text of the job.
        /// </summary>
        public string Stderr
        {
            get
            {
                return _Stderr;
            }
        }

        //-----------------------------------------------------------------------------------------------    

		/// <summary>
		/// Gets the collection of input files for this job
		/// </summary>
        public FileDependencyCollection InputFiles
        {
            get { return _InputFiles; }
        }

		/// <summary>
		/// Gets the collection of output files for this job
		/// </summary>
        public FileDependencyCollection OutputFiles
        {
            get { return _OutputFiles; }
        }

		/// <summary>
		/// Gets or sets the work unit, or the command that is to be executed when this job runs on the executor
		/// </summary>
        public string RunCommand
        {
            get { return _RunCommand; }
            set { _RunCommand = value; }
        }

        private string SubstituteVariables(string input)
        {
            string output = null;
            if (!string.IsNullOrEmpty(input))
            {
                output = input.Replace(JobVariables.WorkingDirectory, WorkingDirectory);
            }
            return output;
        }

        //-----------------------------------------------------------------------------------------------    

		/// <summary>
		/// Runs the executable specified in the RunCommand of the GJob. This happens on the executor node.
		/// </summary>
        public override void Start()
        {
            try
            {
                output = new StringBuilder();
                error = new StringBuilder();
                log = new StringBuilder();

                log.AppendLine("Starting Job ... ");
                foreach (FileDependency dep in _InputFiles)
                {
                    dep.UnpackToFolder(WorkingDirectory);
                    log.AppendFormat("Unpacking input file: {0}", dep.FileName).AppendLine();
                }

                process = new Process();
                process.StartInfo.WorkingDirectory = WorkingDirectory;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.EnableRaisingEvents = true;

                process.OutputDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);
                process.ErrorDataReceived += new DataReceivedEventHandler(process_ErrorDataReceived);
                process.Exited += new EventHandler(process_Exited);
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;

                process.StartInfo.FileName = "cmd"; //_RunCommand; //
                process.StartInfo.Arguments = "/C " + SubstituteVariables(_RunCommand);

                log.Append("Forking process: ").AppendLine(process.StartInfo.FileName);
                log.Append("Arguments: ").AppendLine(process.StartInfo.Arguments);
                log.Append("WorkingDirectory: ").AppendLine(process.StartInfo.WorkingDirectory);

                process.Start();
                process.WaitForExit();

                foreach (EmbeddedFileDependency dep in _OutputFiles)
                {
                    //handle errors connected to missing output files.
                    try
                    {
                        dep.Pack(Path.Combine(WorkingDirectory, dep.FileName));
                        log.AppendFormat("Packed output file: {0}", dep.FileName).AppendLine();
                        // cleanup
                        File.Delete(Path.Combine(WorkingDirectory, dep.FileName));
                    }
                    catch (Exception ex)
                    {
                        dep.Base64EncodedContents = "";
                        log.AppendFormat("Error packing file {0}", dep.FileName).AppendLine();
                        log.AppendLine(ex.ToString()).AppendLine("Continuing with other files ...");
                    }
                }

                //let us not clean up input files - because some may be some of them
                //are actually readonly files elsewhere on the file system! 
                //the working dir would be clean up by the Executor anyway.

                log.AppendFormat("Job {0} complete.", Id).AppendLine();

                output.AppendLine(process.StandardOutput.ReadToEnd().ToString());
                error.AppendLine(process.StandardError.ReadToEnd().ToString());
            }
            finally
            {
                CloseProcess(process);
                _Stderr = error.ToString();
                _Stdout = output.ToString();
                _Log = log.ToString();
            }
        }

        private void CloseProcess(Process process)
        {
            try
            {
                if (process != null)
                {
                    if (!process.HasExited)
                    {
                        process.Kill();
                    }
                    process.Close();
                    process.Dispose();
                    process = null;
                }
            }
            catch (Exception ex)
            {
                log.Append("Error shutting down process: ")
                    .AppendLine(ex.ToString()); 
            }
        }

        #region Process Events
        void process_Exited(object sender, EventArgs e)
        {
            try
            {
                if (process != null)
                {
                    log.AppendFormat("Process {0} has exited at {1} with exit code {2}.",
                        process.Id,
                        process.ExitTime.ToUniversalTime(),
                        process.ExitCode).AppendLine();
                }
            }
            catch (Exception ex)
            {
                //ignore
            }
        }

        void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            lock (error)
            {
                error.AppendLine(e.Data);
            }
        }

        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            lock (output)
            {
                output.AppendLine(e.Data);
            }
        }
        #endregion
    }
}
