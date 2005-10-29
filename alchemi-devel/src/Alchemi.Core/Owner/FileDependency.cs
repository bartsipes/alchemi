#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	FileDependency.cs
* Project		:	Alchemi Core
* Created on	:	2003
* Copyright		:	Copyright © 2005 The University of Melbourne
*					This technology has been developed with the support of 
*					the Australian Research Council and the University of Melbourne
*					research grants as part of the Gridbus Project
*					within GRIDS Laboratory at the University of Melbourne, Australia.
* Author         :  Akshay Luther (akshayl@cs.mu.oz.au) and Rajkumar Buyya (raj@cs.mu.oz.au)
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
using Alchemi.Core.Utility;

namespace Alchemi.Core.Owner
{
	/// <summary>
	/// The FileDependency abstract class defines the members that need to exist in sub classes that are used to implement 
	/// "File" Dependencies. A file dependency represents a single file on which the grid application depends for input.
	/// </summary>
    [Serializable]
    public abstract class FileDependency
    {
		/// <summary>
		/// File name
		/// </summary>
        protected readonly string _FileName;
  
        //-----------------------------------------------------------------------------------------------    
    
		/// <summary>
		/// Creates an instance of the FileDependency
		/// </summary>
        public string FileName
        {
            get { return _FileName; }
        }

        //-----------------------------------------------------------------------------------------------    

		/// <summary>
		/// Creates an instance of the FileDependency
		/// </summary>
		/// <param name="fileName">name of the file</param>
        public FileDependency(string fileName)
        {
            _FileName = fileName;
        }

        //-----------------------------------------------------------------------------------------------    
    
		/// <summary>
		/// Unpacks the file to the specified location
		/// </summary>
		/// <param name="fileLocation">location and file name to unpack the file</param>
        public abstract void UnPack(string fileLocation);
    }
}