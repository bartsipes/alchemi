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
using System.Xml;
using Alchemi.Core.Utility;

namespace Alchemi.Core
{
    /// <summary>
    /// Provides helper methods for working with tasks and jobs on a Manager. This class is used by the X-Platform Manager
    /// and should be used by any client tools providing task/job support.
    /// </summary>
    public class CrossPlatformHelper
    {
        /// <summary>
        /// Creates an empty task on a Manager and returns its Id.
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        public static string CreateTask(IManager manager, SecurityCredentials sc)
        {
            return manager.Owner_CreateApplication(sc);
        }
        
        //-----------------------------------------------------------------------------------------------            
        
        /// <summary>
        /// Creates a task on the Manager from the provided XML task representation and returns its Id. 
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="taskXml"></param>
        /// <returns></returns>
        public static string CreateTask(IManager manager, SecurityCredentials sc, string taskXml)
        {
            // TODO: validate against schema
            string taskId = manager.Owner_CreateApplication(sc);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(taskXml);
            
            FileDependencyCollection manifest = new FileDependencyCollection();
            foreach (XmlNode manifestFile in doc.SelectNodes("task/manifest/embedded_file"))
            {
                EmbeddedFileDependency dep = new EmbeddedFileDependency(manifestFile.Attributes["name"].Value);
                dep.Base64EncodedContents = manifestFile.InnerText;
                manifest.Add(dep);
            }
            manager.Owner_SetApplicationManifest(sc, taskId, manifest);

            foreach (XmlNode jobXml in doc.SelectNodes("task/job"))
            {
                int jobId = int.Parse(jobXml.Attributes["id"].Value);
                jobXml.Attributes.Remove(jobXml.Attributes["id"]);
                
                // TODO: allow setting of priority in xml file
                AddJob(manager, sc, taskId, jobId, 0, jobXml.OuterXml);
            }

            return taskId;
        }

        //-----------------------------------------------------------------------------------------------            

        public static void AddJob(IManager manager, SecurityCredentials sc, string taskId, int jobId, int priority, string jobXml)
        {
            manager.Owner_SetThread(
                sc,
                new ThreadIdentifier(taskId, jobId, priority),
                Utils.SerializeToByteArray(JobFromXml(jobId, jobXml))
                );
        }

        //-----------------------------------------------------------------------------------------------            

        public static int GetJobState(IManager manager, SecurityCredentials sc, string taskId, int jobId)
        {
            return Convert.ToInt32(manager.Owner_GetThreadState(sc, new ThreadIdentifier(taskId, jobId)));
        }

        //-----------------------------------------------------------------------------------------------            

        public static string GetFinishedJobs(IManager manager, SecurityCredentials sc, string taskId)
        {
            byte[][] FinishedThreads = manager.Owner_GetFinishedThreads(sc, taskId);

            XmlStringWriter xsw = new XmlStringWriter();
            xsw.Writer.WriteStartElement("task");
            xsw.Writer.WriteAttributeString("id", taskId);
      
            for (int i=0; i<FinishedThreads.Length; i++)
            {
                GJob job = (GJob) Utils.DeserializeFromByteArray(FinishedThreads[i]);
                xsw.Writer.WriteRaw("\n" + CrossPlatformHelper.XmlFromJob(job) + "\n");
            }
            xsw.Writer.WriteEndElement(); // close job
      
            return xsw.GetXmlString();
        }

        //-----------------------------------------------------------------------------------------------

        private static GJob JobFromXml(int jobId, string jobXml)
        {
            // TODO: validate against schema
            GJob job = new GJob();

            job.SetId(jobId);
            
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(jobXml);

            foreach (XmlNode inputFile in doc.SelectNodes("job/input/embedded_file"))
            {
                EmbeddedFileDependency dep = new EmbeddedFileDependency(inputFile.Attributes["name"].Value);
                dep.Base64EncodedContents = inputFile.InnerText;
                job.InputFiles.Add(dep);
            }

            job.RunCommand = doc.SelectSingleNode("job/work").Attributes["run_command"].Value;

            foreach (XmlNode outputFile in doc.SelectNodes("job/output/embedded_file"))
            {
                EmbeddedFileDependency dep = new EmbeddedFileDependency(outputFile.Attributes["name"].Value);
                job.OutputFiles.Add(dep);
            }
            
            return job;
        }

        //-----------------------------------------------------------------------------------------------    

        private static string XmlFromJob(GJob job)
        {
            XmlStringWriter xsw = new XmlStringWriter();

            xsw.Writer.WriteStartElement("job");
            xsw.Writer.WriteAttributeString("id", job.Id.ToString());
            xsw.Writer.WriteStartElement("input");
            xsw.Writer.WriteEndElement(); // close input
            xsw.Writer.WriteStartElement("work");
            xsw.Writer.WriteEndElement(); // close work
            xsw.Writer.WriteStartElement("output");
      
            foreach (EmbeddedFileDependency fileDep in job.OutputFiles)
            {
                xsw.Writer.WriteStartElement("embedded_file");
                xsw.Writer.WriteAttributeString("name", fileDep.FileName);
                xsw.Writer.WriteString(fileDep.Base64EncodedContents);
                xsw.Writer.WriteEndElement(); // close file
            }
      
            xsw.Writer.WriteEndElement(); // close output
            xsw.Writer.WriteEndElement(); // close task
      
            return xsw.GetXmlString();
        }
    }
}

