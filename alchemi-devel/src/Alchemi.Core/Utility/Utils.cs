#region Alchemi copyright notice
/*
  Alchemi [.NET Grid Computing Framework]
  http://www.alchemi.net
  
  Copyright (c)  Akshay Luther (2002-2004) & Rajkumar Buyya (2003-to-date), 
  GRIDS Lab, The University of Melbourne, Australia.
  
  Maintained and Updated by: Krishna Nadiminti (2005-to-date)
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
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Alchemi.Core.Utility
{
	/// <summary>
	/// This class contains some convenient utility function used in various classes in the Alchemi framework
	/// </summary>
    public class Utils
    {
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
		/// <param name="objGraph">The object / object graph to be serialized</param>
		/// <returns>byte-array after serialization</returns>
        public static byte[] SerializeToByteArray(Object objGraph)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, objGraph);
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
            Stream stream = new MemoryStream(buffer);
            BinaryFormatter formatter = new BinaryFormatter();
            return(formatter.Deserialize(stream));
        }
    
        //-----------------------------------------------------------------------------------------------    
    
		/// <summary>
		/// Write the given byte-array to a file
		/// </summary>
		/// <param name="fileLocation">file name to write the data to</param>
		/// <param name="byteArray">byte-array to be written into the file</param>
        public static void WriteByteArrayToFile(string fileLocation, byte[] byteArray)
        {
            Stream stream = new FileStream (fileLocation, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(byteArray, 0, byteArray.Length);
            writer.Close();
            stream.Close();
        }

        //-----------------------------------------------------------------------------------------------        
    
		/// <summary>
		/// Reads the file at the specified location and returns a byte-array.
		/// </summary>
		/// <param name="fileLocation">location of the file to read</param>
		/// <returns>byte-array representing the contents of the file</returns>
        public static byte[] ReadByteArrayFromFile(string fileLocation)
        {
            byte[] Contents;
            FileStream file = new FileStream(fileLocation, FileMode.Open, FileAccess.Read, FileShare.Read);
            int size = (int) file.Length;
            BinaryReader reader = new BinaryReader(file);
            Contents = new byte[size];
            reader.Read(Contents, 0, size);
            reader.Close(); 
            file.Close();
            return Contents;
        }

        //-----------------------------------------------------------------------------------------------    

		/// <summary>
		/// Serializes an object graph to a disk file.
		/// </summary>
		/// <param name="objGraph">The object / objectGraph to be serialized</param>
		/// <param name="fileLocation">filename to store the serialized object graph</param>
        public static void SerializeToFile(object objGraph, string fileLocation)
        {
            WriteByteArrayToFile(fileLocation, SerializeToByteArray(objGraph));
        }

        //-----------------------------------------------------------------------------------------------    

		/// <summary>
		/// Desserializes a file using the binary formatter to return the object after deserialization.
		/// </summary>
		/// <param name="fileLocation">location of file to be deserialized</param>
		/// <returns>result object after deserialization</returns>
        public static object DeserializeFromFile(string fileLocation)
        {
            if (File.Exists(fileLocation))
            {
                return DeserializeFromByteArray(ReadByteArrayFromFile(fileLocation));
            }
            else
            {
                return null;
            }
        }
    
        //-----------------------------------------------------------------------------------------------    
		/// <summary>
		/// Converts the input boolean val to an int value, used in SQL queries.
		/// </summary>
		/// <param name="val">value to be converted to int</param>
		/// <returns></returns>
        public static int BoolToSqlBit(bool val)
        {
            return (val ? 1 : 0);
        }

        //-----------------------------------------------------------------------------------------------    

		/// <summary>
		/// Writes the given base64-data  to a file
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
		/// Prompts the user for a value, reads it from the console and returns it 
		/// </summary>
		/// <param name="prompt">Prompt given to the user</param>
		/// <param name="defaultVal">default value is none is input by the user</param>
		/// <returns></returns>
        public static string ValueFromConsole(string prompt, string defaultVal)
        {
            Console.Write("{0} [default={1}] : ", prompt, defaultVal);
            string val = Console.ReadLine();
            if (val == "")
            {
                return defaultVal;
            }
            else
            {
                return val;
            }
        }

    }
}
