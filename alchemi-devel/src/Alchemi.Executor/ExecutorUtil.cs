using System;
using System.Collections.Generic;
using System.Text;
using Alchemi.Core.Utility;
using System.IO;

namespace Alchemi.Executor
{
    internal sealed class ExecutorUtil
    {
        internal const string DATADIR = "dat";

        internal static string BaseDirectory
        {
            get
            {
                return Utils.GetFilePath(string.Empty, AlchemiRole.Executor, false); 
            }
        }
        
        internal static string DataDirectory
        {
            get
            {
                return Path.Combine(BaseDirectory, DATADIR);
            }
        }
        
        internal static string GetApplicationDirectory(string applicationId)
        {
            return Path.Combine(DataDirectory, string.Format("application_{0}", applicationId));
        }

    }
}
