#region Alchemi copyright notice
/*
  Alchemi [.NET Grid Computing Framework]
  http://www.alchemi.net
  
  Copyright (c)  Akshay Luther (2002-2004) & Rajkumar Buyya (2003-to-date), 
  GRIDS Lab, The University of Melbourne, Australia.
  
  Maintained and Updated by: Krishna Nadiminti (2005-to-date)
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
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Alchemi.Core.Owner
{
	/// <summary>
	/// Represents a coarse unit of work on the grid. This class extends the GThread to enable legacy applications to 
	/// run on the grid. A GJob is associated with file dependencies which are the inputs and outputs of the job
	/// and a compiled binary (the legacy application) which is the work unit of the job.
	/// </summary>
    [Serializable]
    public class GJob : GThread
    {
		private static readonly Logger logger = new Logger();

		private FileDependencyCollection _InputFiles = new FileDependencyCollection();
        private FileDependencyCollection _OutputFiles = new FileDependencyCollection();
        private string _RunCommand;

		//TODO let the user specify what the stdout and stderr are to be called.
		//by default we can call them stdout.txt and stderr.txt

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

        //-----------------------------------------------------------------------------------------------    

		/// <summary>
		/// Runs the executable specified in the RunCommand of the GJob. This happens on the executor node.
		/// </summary>
        public override void Start()
        {
            foreach (FileDependency dep in _InputFiles)
            {
                dep.UnPack(string.Format("{0}\\{1}", WorkingDirectory, dep.FileName));
				logger.Debug("Unpacking input file: "+dep.FileName);
            }
      
			Process process = new Process();
            process.StartInfo.WorkingDirectory = WorkingDirectory;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;

			logger.Debug("Starting a new process...");
            process.StartInfo.FileName = _RunCommand; //"cmd";
            //process.StartInfo.Arguments = "/C " + _RunCommand;
 
            process.Start();
            process.WaitForExit();
      
            foreach (EmbeddedFileDependency dep in _OutputFiles)
            {
                try
                {
                    dep.Pack(string.Format("{0}\\{1}", WorkingDirectory, dep.FileName));
                    // cleanup
                	File.Delete(string.Format("{0}\\{1}", WorkingDirectory, dep.FileName));
					logger.Debug("Packing output file: "+dep.FileName);
                }
                catch (FileNotFoundException)
                {
                    dep.Base64EncodedContents = "";
                }
            }

            foreach (FileDependency dep in _InputFiles)
            {
                // cleanup
                File.Delete(string.Format("{0}\\{1}", WorkingDirectory, dep.FileName));
            }

            AddStandardFile(process.StandardError, "stderr.txt");
            AddStandardFile(process.StandardOutput, "stdout.txt");
			
			logger.Debug("GJob Process complete: "+Id);
        }

        //-----------------------------------------------------------------------------------------------    
    
        private void AddStandardFile(StreamReader reader, string name)
        {
            EmbeddedFileDependency fileDep = new EmbeddedFileDependency(name);
        	UTF8Encoding encoding = new UTF8Encoding();
			fileDep.Base64EncodedContents = Convert.ToBase64String(
                encoding.GetBytes(reader.ReadToEnd())
                );
            _OutputFiles.Add(fileDep);
			logger.Debug("Adding/packing output file: " + name);
        }
    }
}
