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
using System.IO;

namespace Alchemi.Core.Manager
{
    public class InternalShared
    {
        public readonly SqlServer Database;
        public readonly string DataRootDirectory;
        /*
        public readonly MApplicationCollection Applications;
        public readonly MExecutorCollection Executors;
        */

        private InternalShared(SqlServer database, string dataRootDirectory/*, MApplicationCollection applications, MExecutorCollection executors*/)
        {
            Database = database;
            DataRootDirectory = dataRootDirectory;
            /*
            Applications = applications;
            Executors = executors;
            */
        }

        public static InternalShared Instance;
        
        public static InternalShared GetInstance(SqlServer database, string dataRootDirectory/*, MApplicationCollection applications, MExecutorCollection executors*/)
        {
            if (Instance == null)
            {
                if (!Directory.Exists(dataRootDirectory))
                {
                    Directory.CreateDirectory(dataRootDirectory);
                }
                Instance = new InternalShared(database, dataRootDirectory/*, applications, executors*/);
            }
            return Instance;
        }
    }
}
