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
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Alchemi.Core.Utility
{
    public class Utils
    {
        public static void Trace(string msg)
        {
            StackTrace st = new StackTrace();
            Console.WriteLine("{0}.{1} :: {2}", st.GetFrame(1).GetMethod().ReflectedType, st.GetFrame(1).GetMethod().Name, msg);
        }

        //-----------------------------------------------------------------------------------------------    

        public static byte[] SerializeToByteArray(Object objGraph)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, objGraph);
            return stream.ToArray();
        }

        //-----------------------------------------------------------------------------------------------    

        public static Object DeserializeFromByteArray(byte[] buffer) 
        {
            Stream stream = new MemoryStream(buffer);
            BinaryFormatter formatter = new BinaryFormatter();
            return(formatter.Deserialize(stream));
        }
    
        //-----------------------------------------------------------------------------------------------    
    
        public static void WriteByteArrayToFile(string fileLocation, byte[] byteArray)
        {
            Stream stream = new FileStream (fileLocation, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(byteArray, 0, byteArray.Length);
            writer.Close();
            stream.Close();
        }

        //-----------------------------------------------------------------------------------------------        
    
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

        public static void SerializeToFile(object objGraph, string fileLocation)
        {
            WriteByteArrayToFile(fileLocation, SerializeToByteArray(objGraph));
        }

        //-----------------------------------------------------------------------------------------------    

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

        public static int BoolToSqlBit(bool val)
        {
            return (val ? 1 : 0);
        }

        //-----------------------------------------------------------------------------------------------    


        public static void WriteBase64EncodedToFile(string fileLocation, string base64EncodedData)
        {
            WriteByteArrayToFile(fileLocation, Convert.FromBase64String(base64EncodedData));
        }
    
        //-----------------------------------------------------------------------------------------------    
    
        public static string ReadBase64EncodedFromFile(string fileLocation)
        {
            return Convert.ToBase64String(ReadByteArrayFromFile(fileLocation));
        }

        //-----------------------------------------------------------------------------------------------    

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

        public static string AssemblyVersion
        {
            get
            {
                Version v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                return v.Major + "." + v.Minor + "." + v.Build;
            }
        }

        //-----------------------------------------------------------------------------------------------    

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
