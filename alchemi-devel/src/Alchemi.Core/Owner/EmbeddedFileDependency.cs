#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   EmbeddedFileDependency.cs
 * Project      :   Alchemi.Core.Owner
 * Created on   :   2003
 * Copyright    :   Copyright © 2006 The University of Melbourne
 *                  This technology has been developed with the support of 
 *                  the Australian Research Council and the University of Melbourne
 *                  research grants as part of the Gridbus Project
 *                  within GRIDS Laboratory at the University of Melbourne, Australia.
 * Author       :   Akshay Luther (akshayl@csse.unimelb.edu.au)
 *                  Rajkumar Buyya (raj@csse.unimelb.edu.au)
 * License      :   GPL
 *                  This program is free software; you can redistribute it and/or 
 *                  modify it under the terms of the GNU General Public
 *                  License as published by the Free Software Foundation;
 *                  See the GNU General Public License 
 *                  (http://www.gnu.org/copyleft/gpl.html) for more details.
 * 
 */
#endregion

using System;
using System.Reflection;
using System.IO;
using System.Collections;
using System.Runtime.Serialization;

using Alchemi.Core.Owner;
using Alchemi.Core.Utility;
using System.Collections.Generic;

namespace Alchemi.Core.Owner
{
	/// <summary>
	/// The EmbeddedFileDependency Class extends from the FileDependency class
	/// and provides concrete implementation of the methods to pack and unpack files using base64 encoding.
	/// </summary>
    [Serializable]
    [DataContract]
    [KnownType(typeof(Alchemi.Core.Owner.ModuleDependency))]
    public class EmbeddedFileDependency : FileDependency
    {

        #region Property - Base64EncodedContents
        [DataMember]
        private string _base64EncodedContents = String.Empty;
        /// <summary>
        /// Gets or sets the file contents in base64-encoded format
        /// </summary>
        public string Base64EncodedContents
        {
            get { return _base64EncodedContents; }
            set { _base64EncodedContents = value; }
        } 
        #endregion
    


        #region Constructors
        /// <summary>
        /// Creates an instance of an EmbeddedFileDependency
        /// </summary>
        /// <param name="name">file name</param>
        public EmbeddedFileDependency(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Creates an instance of an EmbeddedFileDependency
        /// </summary>
        /// <param name="name">file name</param>
        /// <param name="fileLocation">file location</param>
        public EmbeddedFileDependency(string name, string fileLocation)
            : base(name)
        {
            Pack(fileLocation);
        } 
        #endregion



        #region Method - Pack
        /// <summary>
        /// Reads the file and converts its contents to base64 format
        /// </summary>
        /// <param name="fileLocation">location of the file</param>
        public void Pack(string fileLocation)
        {
            _base64EncodedContents = Utils.ReadBase64EncodedFromFile(fileLocation);
        } 
        #endregion


        #region Method - Unpack
        /// <summary>
        /// Unpacks (writes out) the file to the specified location
        /// </summary>
        /// <param name="fileLocation">file location</param>
        public override void Unpack(string fileLocation)
        {
            Utils.WriteBase64EncodedToFile(fileLocation, _base64EncodedContents);
        } 
        #endregion


        /// <summary>
        /// Create an array of dependencies from the given folder. 
        /// All files under the folder structure will be recursively added to the array.
        /// </summary>
        /// <remarks>
        /// This will preserve the folder structure for any sub-folders. The root folder will not be preserved though
        /// </remarks>
        /// <param name="folderName">The root folder to start from</param>
        /// <returns></returns>
        public static EmbeddedFileDependency[] GetEmbeddedFileDependencyFromFolder(string rootFolderName)
        {
            if (rootFolderName == null || !Directory.Exists(rootFolderName))
            {
                return null;
            }

            List<EmbeddedFileDependency> list = new List<EmbeddedFileDependency>();

            AddFilesToList(list, rootFolderName, "");
            return list.ToArray();
        }


        /// <summary>
        /// Adds the files in folderName to list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="subFolderToAddToFileName">Name of the sub folder to add to file.</param>
        private static void AddFilesToList(List<EmbeddedFileDependency> list, string folderName, string subFolderToAddToFileName)
        {
            foreach (string filePath in Directory.GetFiles(folderName))
            {
                EmbeddedFileDependency fileDep =
                    new EmbeddedFileDependency(
                        Path.Combine(subFolderToAddToFileName, Path.GetFileName(filePath)),
                        filePath);

                list.Add(fileDep);
            }
            
            foreach (string folderPath in Directory.GetDirectories(folderName))
            {
                AddFilesToList(
                    list,
                    folderPath,
                    Path.Combine(subFolderToAddToFileName, Path.GetFileName(folderPath)));
            }
        }

    }
}
