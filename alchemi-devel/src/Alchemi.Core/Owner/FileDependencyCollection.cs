#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	FileDependencyCollection.cs
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
using System.Collections;
using System.Collections.Specialized;
using Alchemi.Core.Owner;

namespace Alchemi.Core.Owner
{
	/// <summary>
	/// Represents a collection of FileDependencies
	/// </summary>
    [Serializable]
    public class FileDependencyCollection : System.Collections.ReadOnlyCollectionBase
    {
		/// <summary>
		/// Gets the FileDependency with the given index
		/// </summary>
        public FileDependency this[int index]
        {
            get 
            { 
                return (FileDependency) InnerList[index]; 
            }
        }
    
        //-----------------------------------------------------------------------------------------------    
    
		/// <summary>
		/// Adds the given FileDependency object to this collection
		/// </summary>
		/// <param name="dependency">file dependency to add</param>
        public void Add(FileDependency dependency)
        {
            foreach (FileDependency fd in InnerList)
            {
                if (fd.FileName == dependency.FileName)
                {
                    throw new InvalidOperationException("A file dependency with the same name already exists.", null);
                }
            }
            InnerList.Add(dependency);
        }
    }
}