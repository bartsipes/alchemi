#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   ModuleDependency.cs
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

namespace Alchemi.Core.Owner
{
	/// <summary>
	/// Represents a dependency which is a .NET module
	/// </summary>
    [Serializable]
    public class ModuleDependency : EmbeddedFileDependency
    {
        /// <summary>
        /// Creates an instance of the ModuleDependency class
        /// </summary>
        /// <param name="module">The module.</param>
        public ModuleDependency(Module module) : base(module.Name, module.FullyQualifiedName) {}
    }
}
