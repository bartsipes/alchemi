#region Creative Commons License
// Copyright (c) 2007 Matt Valerio
//
// In summary,
// You are free:
//  - To share (copy, distribute, and transmit) this work
//  - To remix (adapt) this work
// Provided that:
//  - Attribution of the work is given (but not in any way that suggests endorsement)
//
// This work is licensed under the Creative Commons Attribution 3.0 Unported License.
//
// To view a copy of this license, visit http://creativecommons.org/licenses/by/3.0/ 
// or send a letter to:
// Creative Commons, 171 Second Street, Suite 300, San Francisco, California, 94105, USA.
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Alchemi.Tasks
{
    /// <summary>
    /// A task that injects the contents of one file into another to produce an output file.
    /// </summary>
    public class InjectFile : Task
    {
        #region Property - TemplateFile
        private ITaskItem m_TemplateFile;
        /// <summary>
        /// [Required] The file to use as the template. 
        /// This file must contain the string specified in the Placeholder property.
        /// </summary>
        [Required]
        public ITaskItem TemplateFile
        {
            get { return m_TemplateFile; }
            set { m_TemplateFile = value; }
        } 
        #endregion


        #region Property - DataFiles
        private ITaskItem[] m_DataFiles;
        /// <summary>
        /// [Required] The data files. 
        /// All text in these files will be injected into the TemplateFile where the Placeholder is.
        /// </summary>
        [Required]
        public ITaskItem[] DataFiles
        {
            get { return m_DataFiles; }
            set { m_DataFiles = value; }
        } 
        #endregion


        #region Property - Placeholder
        private string m_Placeholder;
        /// <summary>
        /// [Required] The placeholder text where the entire contents of DataFile gets
        /// injected into the TemplateFile.
        /// </summary>
        [Required]
        public string Placeholder
        {
            get { return m_Placeholder; }
            set { m_Placeholder = value; }
        } 
        #endregion


        #region Property - OutputFiles
        private ITaskItem[] m_OutputFiles;
        /// <summary>
        /// [Required] After the DataFile text has been inserted into the TemplateFile where
        /// the Placeholder is, the result is written to the OutputFile.
        /// </summary>
        [Required]
        public ITaskItem[] OutputFiles
        {
            get { return m_OutputFiles; }
            set { m_OutputFiles = value; }
        } 
        #endregion



        #region Method Override - Execute
        /// <summary>
        /// When overridden in a derived class, executes the task.
        /// </summary>
        /// <returns>
        /// true if the task successfully executed; otherwise, false.
        /// </returns>
        public override bool Execute()
        {
            if (this.DataFiles == null || this.DataFiles.Length < 1)
            {
                throw new Exception("There must be at least 1 DataFile");
            }
            if (this.OutputFiles == null || this.OutputFiles.Length < 1)
            {
                throw new Exception("There must be at least 1 OutputFile");
            }
            if (this.DataFiles.Length != this.OutputFiles.Length)
            {
                throw new InvalidOperationException("There must be the same number of DataFiles and OutputFiles");
            }


            try
            {
                string templateFilename = this.TemplateFile.ItemSpec;
                string template = File.ReadAllText(templateFilename);

                int length = this.DataFiles.Length;

                for (int i = 0; i < this.DataFiles.Length; i++)
                {
                    string dataFilename = this.DataFiles[i].ItemSpec;
                    string outputFilename = this.OutputFiles[i].ItemSpec;

                    Log.LogMessage(MessageImportance.High, "InjectFile:");
                    Log.LogMessage(MessageImportance.High, " Template: {0}", templateFilename);
                    Log.LogMessage(MessageImportance.High, " Data: {0}", dataFilename);
                    Log.LogMessage(MessageImportance.High, " Placeholder: {0}", this.Placeholder);
                    Log.LogMessage(MessageImportance.High, " Output: {0}", outputFilename);

                    string data = File.ReadAllText(dataFilename);
                    string output = template.Replace(this.Placeholder, data);
                    File.WriteAllText(outputFilename, output);
                }
            }
            catch (Exception ex)
            {
                Log.LogErrorFromException(ex);
                return false;
            }
            return true;
        } 
        #endregion
    }
}
