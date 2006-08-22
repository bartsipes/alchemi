#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	Utils.cs
* Project		:	Alchemi Core
* Created on	:	2003
* Copyright		:	Copyright © 2006 The University of Melbourne
*					This technology has been developed with the support of 
*					the Australian Research Council and the University of Melbourne
*					research grants as part of the Gridbus Project
*					within GRIDS Laboratory at the University of Melbourne, Australia.
* Author         :  Akshay Luther (akshayl@csse.unimelb.edu.au), Rajkumar Buyya (raj@csse.unimelb.edu.au) , and Krishna Nadiminti (kna@csse.unimelb.edu.au) 
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
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Threading;
using System.Security.Principal;

namespace Alchemi.Core.Utility
{
    public enum AlchemiRole
    {
        Manager,
        Executor,
        Owner,
        Admin
    }

    public enum DateTimeInterval
    {
        Tick,
        Millisecond,
        Second,
        Minute,
        Hour,
        Day,
        Week,
        Fortnight,
        Month,
        Quarter,
        Year
    }

    /// <summary>
    /// This class contains some convenient utility function used in various classes in the Alchemi framework
    /// </summary>
    public sealed class Utils
    {
        //We dont want anyone to instantiate this util class.
        private Utils() { }

        /// <summary>
        /// Prints the message with a stack trace to the console.
        /// </summary>
        /// <param name="msg">message to be printed</param>
        public static void Trace(string msg)
        {
            StackTrace st = new StackTrace();
            Console.WriteLine("{0}.{1} :: {2}", st.GetFrame(1).GetMethod().ReflectedType, st.GetFrame(1).GetMethod().Name, msg);
        }

        //-----------------------------------------------------------------------------------------------    

        /// <summary>
        /// Serializes an object graph to an in-memory byte-array using the binary formatter.
        /// </summary>
        /// <param name="graph">The object / object graph to be serialized</param>
        /// <returns>byte-array after serialization</returns>
        public static byte[] SerializeToByteArray(Object graph)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, graph);
            return stream.ToArray();
        }

        //-----------------------------------------------------------------------------------------------    

        /// <summary>
        /// Desserializes a byte array using the binary formatter to return the object after deserialization.
        /// </summary>
        /// <param name="buffer">byte array to be deserialized</param>
        /// <returns>result object after deserialization</returns>
        public static Object DeserializeFromByteArray(byte[] buffer)
        {
            Object o = null;
            if (buffer != null)
            {
                Stream stream = new MemoryStream(buffer);
                BinaryFormatter formatter = new BinaryFormatter();
                o = formatter.Deserialize(stream);
            }
            return o;
        }

        //-----------------------------------------------------------------------------------------------    

        /// <summary>
        /// Write the given byte-array to a file
        /// </summary>
        /// <param name="fileLocation">file name to write the data to</param>
        /// <param name="buffer">byte-array to be written into the file</param>
        public static void WriteByteArrayToFile(string fileLocation, byte[] buffer)
        {
            Stream stream = null;
            BinaryWriter writer = null;
            try
            {
                if (buffer != null && fileLocation != null)
                {
                    stream = new FileStream(fileLocation, FileMode.Create);
                    writer = new BinaryWriter(stream);
                    writer.Write(buffer, 0, buffer.Length);
                }
            }
            finally
            {
                if (writer!=null)
                    writer.Close();
                if (stream!=null)
                    stream.Close();
            }
        }

        //-----------------------------------------------------------------------------------------------        

        /// <summary>
        /// Reads the file at the specified location and returns a byte-array.
        /// </summary>
        /// <param name="fileLocation">location of the file to read</param>
        /// <returns>byte-array representing the contents of the file</returns>
        public static byte[] ReadByteArrayFromFile(string fileLocation)
        {
            byte[] contents;
            FileStream file = null;
            BinaryReader reader = null;
            try
            {
                file = new FileStream(fileLocation, FileMode.Open, FileAccess.Read, FileShare.Read);
                int size = (int)file.Length;
                reader = new BinaryReader(file);
                contents = new byte[size];
                reader.Read(contents, 0, size);
            }
            finally
            {
                if (reader!=null)
                    reader.Close();
                if (file!=null)
                    file.Close();
            }
            return contents;
        }

        //-----------------------------------------------------------------------------------------------    

        /// <summary>
        /// Serializes an object graph to a disk file.
        /// </summary>
        /// <param name="objGraph">The object / graph to be serialized</param>
        /// <param name="fileLocation">filename to store the serialized object graph</param>
        public static void SerializeToFile(object objGraph, string fileLocation)
        {
            WriteByteArrayToFile(fileLocation, SerializeToByteArray(objGraph));
        }

        //-----------------------------------------------------------------------------------------------    

        /// <summary>
        /// Desserializes a file using the binary formatter to return the object after deserialization.
        /// Throws an Exception if the File cannot be found / read.
        /// </summary>
        /// <param name="fileLocation">location of file to be deserialized</param>
        /// <returns>result object after deserialization</returns>
        public static object DeserializeFromFile(string fileLocation)
        {
            return DeserializeFromByteArray(ReadByteArrayFromFile(fileLocation));
        }

        //-----------------------------------------------------------------------------------------------    
        /// <summary>
        /// Converts the input boolean val to an int name, used in SQL queries.
        /// </summary>
        /// <param name="val">name to be converted to int</param>
        /// <returns></returns>
        public static int BoolToSqlBit(bool val)
        {
            return (val ? 1 : 0);
        }

        //-----------------------------------------------------------------------------------------------    

        /// <summary>
        /// Writes the given base64-data  to a file
        /// Throws an Exception if the File cannot written.
        /// </summary>
        /// <param name="fileLocation">filename to write the data to</param>
        /// <param name="base64EncodedData">the base64-encoded data to be written into the file</param>
        public static void WriteBase64EncodedToFile(string fileLocation, string base64EncodedData)
        {
            WriteByteArrayToFile(fileLocation, Convert.FromBase64String(base64EncodedData));
        }

        //-----------------------------------------------------------------------------------------------    

        /// <summary>
        /// Reads a base64-encoded file and returns the contents as a string.
        /// </summary>
        /// <param name="fileLocation">location of the file to read</param>
        /// <returns></returns>
        public static string ReadBase64EncodedFromFile(string fileLocation)
        {
            return Convert.ToBase64String(ReadByteArrayFromFile(fileLocation));
        }

        //-----------------------------------------------------------------------------------------------    

        /// <summary>
        /// Reads a text file and returns the contents as a string
        /// </summary>
        /// <param name="fileLocation">location of the file to read</param>
        /// <returns>string representing the file contents</returns>
        public static string ReadStringFromFile(string fileLocation)
        {
            string contents;
            using (StreamReader sr = new StreamReader(fileLocation))
            {
                contents = sr.ReadToEnd();
            }
            return contents;
        }

        //-----------------------------------------------------------------------------------------------    

        /// <summary>
        /// Gets the version of the current assembly.
        /// </summary>
        public static string AssemblyVersion
        {
            get
            {
                Version v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                return v.Major + "." + v.Minor + "." + v.Build;
            }
        }

        //-----------------------------------------------------------------------------------------------    

        /// <summary>
        /// Prompts the user for a name, reads it from the console and returns it 
        /// </summary>
        /// <param name="prompt">Prompt given to the user</param>
        /// <param name="defaultValue">default name is none is input by the user</param>
        /// <returns></returns>
        public static string ValueFromConsole(string prompt, string defaultValue)
        {
            Console.Write("{0} [default={1}] : ", prompt, defaultValue);
            string val = Console.ReadLine();
            if (String.IsNullOrEmpty(val))
            {
                val = defaultValue;
            }
            return val;
        }


        /// <summary>
        /// same common params similar to the VBScript DateDiff: 
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/script56/html/vsfctdatediff.asp
        ///   /*
        ///    * Sample Code:
        ///    * System.DateTime dt1 = new System.DateTime(1974,12,16);
        ///    * System.DateTime dt2 = new System.DateTime(1973,12,16);
        ///    * double diff = DateDiff(DateTimeInterval.Day, dt1, dt2);
        ///    * 
        ///    */
        /// </summary>
        /// <param name="compareInterval"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        //adapted from http://authors.aspalliance.com/nothingmn/default.aspx?whichpoll=nothingmn_117&aid=117
        public static double DateDiff(DateTimeInterval compareInterval, System.DateTime startDate, System.DateTime endDate)
        {
            double diff = 0;
            System.TimeSpan TS = new System.TimeSpan(startDate.Ticks - endDate.Ticks);
            switch (compareInterval)
            {
                case DateTimeInterval.Tick:
                    diff = Convert.ToDouble(TS.Ticks);
                    break;
                case DateTimeInterval.Millisecond:
                    diff = Convert.ToDouble(TS.TotalMilliseconds);
                    break;
                case DateTimeInterval.Second:
                    diff = Convert.ToDouble(TS.TotalSeconds);
                    break;
                case DateTimeInterval.Minute:
                    diff = Convert.ToDouble(TS.TotalMinutes);
                    break;
                case DateTimeInterval.Hour:
                    diff = Convert.ToDouble(TS.TotalHours);
                    break;
                case DateTimeInterval.Day:
                    diff = Convert.ToDouble(TS.TotalDays);
                    break;
                case DateTimeInterval.Week:
                    diff = Convert.ToDouble(TS.TotalDays / 7);
                    break;
                case DateTimeInterval.Fortnight:
                    diff = Convert.ToDouble(TS.TotalDays / 15);
                    break;
                case DateTimeInterval.Month:
                    diff = Convert.ToDouble((TS.TotalDays / 365) / 12);
                    break;
                case DateTimeInterval.Quarter:
                    diff = Convert.ToDouble((TS.TotalDays / 365) / 4);
                    break;
                case DateTimeInterval.Year:
                    diff = Convert.ToDouble(TS.TotalDays / 365);
                    break;
            }
            return diff;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string MakeSqlSafe(string input)
        {
            string output = null;
            if (input != null)
            {
                output = input.Replace("'", "").Replace("\"", "");
            }
            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsSqlSafe(string text)
        {
            //if the result returned by MakeSqlSafe is same as the original string, then it is safe.
            return (text == MakeSqlSafe(text));
        }

        /// <summary>
        /// Gets the location (including full path) of the given file name.
        /// This returns a path in the users' application-data directory, of the form:
        /// <![CDATA[
        ///     <users-app-data-dir>\Alchemi\<module-name>\filename
        /// ]]>
        /// </summary>
        /// <param name="filename">name of the file or directory whose location is to be resolved</param>
        /// <param name="name">name of the calling Alchemi module</param>
        /// <param name="createIfNeeded">Creates the parent directories if needed.</param>
        /// <returns></returns>
        public static string GetFilePath(string filepath, AlchemiRole name, bool createIfNeeded)
        {
            string dataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string[] paths = new string[] {
                dataDir,
                "Alchemi",
                name.ToString(),
                filepath
                };
            return GetAbsPath(paths, createIfNeeded);
        }
        /// <summary>
        /// Combines the elements in the string[] into a full path.
        /// and creates the directories if needed.
        /// </summary>
        /// <param name="paths"></param>
        /// <param name="createIfNeeded"></param>
        /// <returns></returns>
        private static string GetAbsPath(string[] paths, bool createIfNeeded)
        {
            string absPath = "";
            foreach (string path in paths)
            {
                absPath = Path.Combine(absPath, path);
            }
            if (createIfNeeded)
            {
                //we use the default acl's for the new directory.
                Directory.CreateDirectory(Path.GetDirectoryName(absPath));
            }
            return absPath;
        }

        /// <summary>
        /// Gets a formatted string to print out the current working environment
        /// </summary>
        /// <returns></returns>
        public static string GetEnv()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("\n ************ BEGIN ENV ***************** ");

            sb.Append("\n Thread.CurrentPrincipal.identity name = " + Thread.CurrentPrincipal.Identity.Name);
            sb.Append("\n WindowsIdentity.GetCurrent().Name = " + WindowsIdentity.GetCurrent().Name);

            sb.Append("\n Current thread.managed threadID = " + Thread.CurrentThread.ManagedThreadId);
            sb.Append("\n Thread.Current Context = " + Thread.CurrentContext.ToString());

            sb.Append("\n Current Dir = " + Environment.CurrentDirectory);
            sb.Append("\n TEMP Dir = " + Path.GetTempPath());
            sb.Append("\n UserDomain Name = " + Environment.UserDomainName);
            sb.Append("\n User Name = " + Environment.UserName);
            sb.Append("\n User Interactive = " + Environment.UserInteractive);

            sb.Append("\n Machine Env Vars : ");
            IDictionary env = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Machine);
            foreach (string key in env.Keys)
            {
                sb.Append(string.Format("\n\t {0} = {1} ", key, env[key].ToString()));
            }
            sb.Append("\n Process Env Vars : ");
            env = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process);
            foreach (string key in env.Keys)
            {
                sb.Append(string.Format("\n\t {0} = {1} ", key, env[key].ToString()));
            }
            sb.Append("\n User Env Vars : ");
            env = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.User);
            foreach (string key in env.Keys)
            {
                sb.Append(string.Format("\n\t {0} = {1} ", key, env[key].ToString()));
            }

            sb.Append("\n Special Folders : ");
            sb.Append("\n\t ApplicationData = " + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            sb.Append("\n\t CommonApplicationData = " + Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
            sb.Append("\n\t CommonProgramFiles = " + Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles));
            sb.Append("\n\t Cookies = " + Environment.GetFolderPath(Environment.SpecialFolder.Cookies));
            sb.Append("\n\t Desktop = " + Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            sb.Append("\n\t DesktopDirectory = " + Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
            sb.Append("\n\t Favorites = " + Environment.GetFolderPath(Environment.SpecialFolder.Favorites));
            sb.Append("\n\t History = " + Environment.GetFolderPath(Environment.SpecialFolder.History));
            sb.Append("\n\t InternetCache = " + Environment.GetFolderPath(Environment.SpecialFolder.InternetCache));
            sb.Append("\n\t LocalApplicationData = " + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            sb.Append("\n\t MyComputer = " + Environment.GetFolderPath(Environment.SpecialFolder.MyComputer));
            sb.Append("\n\t MyDocuments = " + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            sb.Append("\n\t MyMusic = " + Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
            sb.Append("\n\t MyPictures = " + Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
            sb.Append("\n\t Personal = " + Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            sb.Append("\n\t ProgramFiles = " + Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
            sb.Append("\n\t Programs = " + Environment.GetFolderPath(Environment.SpecialFolder.Programs));
            sb.Append("\n\t Recent = " + Environment.GetFolderPath(Environment.SpecialFolder.Recent));
            sb.Append("\n\t SendTo = " + Environment.GetFolderPath(Environment.SpecialFolder.SendTo));
            sb.Append("\n\t StartMenu = " + Environment.GetFolderPath(Environment.SpecialFolder.StartMenu));
            sb.Append("\n\t Startup = " + Environment.GetFolderPath(Environment.SpecialFolder.Startup));
            sb.Append("\n\t System = " + Environment.GetFolderPath(Environment.SpecialFolder.System));
            sb.Append("\n\t Templates = " + Environment.GetFolderPath(Environment.SpecialFolder.Templates));

            sb.Append("\n ************ END ENV ***************** ");

            return sb.ToString();
        }
    }
}
