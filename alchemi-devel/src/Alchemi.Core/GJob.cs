#region Alchemi copyright notice
/*
  Alchemi [.NET Grid Computing Framework]
  Copyright (c) 2002-2004 Akshay Luther
  http://www.alchemi.net
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
using System.Xml;
using Alchemi.Core.Utility;

namespace Alchemi.Core
{
    [Serializable]
    public class GJob : GThread
    {
        private FileDependencyCollection _InputFiles = new FileDependencyCollection();
        private FileDependencyCollection _OutputFiles = new FileDependencyCollection();
        private string _RunCommand;

        //-----------------------------------------------------------------------------------------------    

        public FileDependencyCollection InputFiles
        {
            get { return _InputFiles; }
        }

        public FileDependencyCollection OutputFiles
        {
            get { return _OutputFiles; }
        }

        public string RunCommand
        {
            get { return _RunCommand; }
            set { _RunCommand = value; }
        }

        //-----------------------------------------------------------------------------------------------    

        public override void Start()
        {
            foreach (FileDependency dep in _InputFiles)
            {
                dep.UnPack(string.Format("{0}\\{1}", WorkingDirectory, dep.FileName));
            }
      
            Process process = new Process();
            process.StartInfo.WorkingDirectory = WorkingDirectory;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;

            process.StartInfo.FileName = "cmd";
            process.StartInfo.Arguments = "/C " + _RunCommand;
 
            process.Start();
            process.WaitForExit();
      
            foreach (EmbeddedFileDependency dep in _OutputFiles)
            {
                try
                {
                    dep.Pack(string.Format("{0}\\{1}", WorkingDirectory, dep.FileName));
                    // cleanup
                    File.Delete(string.Format("{0}\\{1}", WorkingDirectory, dep.FileName));
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

            AddStandardFile(process.StandardError, "StandarError.txt");
            AddStandardFile(process.StandardOutput, "StandardOutput.txt");
        }

        //-----------------------------------------------------------------------------------------------    
    
        private void AddStandardFile(StreamReader reader, string name)
        {
            EmbeddedFileDependency fileDep = new EmbeddedFileDependency(name);
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            fileDep.Base64EncodedContents = Convert.ToBase64String(
                encoding.GetBytes(reader.ReadToEnd())
                );
            _OutputFiles.Add(fileDep);
        }
    }
}
