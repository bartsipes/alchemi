#region Alchemi copyright notice
/*
  Alchemi [.NET Grid Computing Framework]
  http://www.alchemi.net
  
  Copyright (c) 2002-2004 Akshay Luther & 2003-2004 Rajkumar Buyya 
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
using System.Collections;
using System.Collections.Specialized;

namespace Alchemi.Core
{
    [Serializable]
    public class FileDependencyCollection : System.Collections.ReadOnlyCollectionBase
    {
        public FileDependency this[int index]
        {
            get 
            { 
                return (FileDependency) InnerList[index]; 
            }
        }
    
        //-----------------------------------------------------------------------------------------------    
    
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