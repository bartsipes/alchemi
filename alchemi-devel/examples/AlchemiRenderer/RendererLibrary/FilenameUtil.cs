using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Alchemi.Examples.Renderer
{
    class FilenameUtil
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetShortPathName(
            [MarshalAs(UnmanagedType.LPTStr)]
            string path,
            [MarshalAs(UnmanagedType.LPTStr)]
            StringBuilder shortPath,
            int shortPathLength
            );

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetLongPathName(
            [MarshalAs(UnmanagedType.LPTStr)]
            string path,
            [MarshalAs(UnmanagedType.LPTStr)]
            StringBuilder longPath,
            int longPathLength
            );

        internal static string GetShortPathName(string longFilename)
        {
            StringBuilder shortPath = new StringBuilder(255);
            GetShortPathName(longFilename, shortPath, shortPath.Capacity);
            return shortPath.ToString();
        }

        internal static string GetLongPathName(string shortFilename)
        {
            StringBuilder longPath = new StringBuilder(255);
            GetShortPathName(shortFilename, longPath, longPath.Capacity);
            return longPath.ToString();
        }
    }
}
