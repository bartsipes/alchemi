using System;
using System.Xml;
using System.IO;

namespace XPManagerClient
{
    class XPManagerClient
    {
        [STAThread]
        static void Main(string[] args)
        {
            string taskXmlDir = Path.GetFullPath("../../taskfiles");
            string taskXmlFile = "reverse.task.xml";

            XmlDocument taskXml = new XmlDocument();
            taskXml.Load(Path.Combine(taskXmlDir, taskXmlFile));

            foreach (XmlNode embeddedFile in taskXml.SelectNodes("//manifest/embedded_file | //input/embedded_file"))
            {
                XmlAttribute location = embeddedFile.Attributes["location"];
                embeddedFile.InnerText = ReadBase64EncodedFromFile(Path.Combine(taskXmlDir, location.Value));
                embeddedFile.Attributes.Remove(location);
            }
            taskXml.Save(Path.Combine(taskXmlDir, "INPUT_" + taskXmlFile));

            string taskId;
            
            AlchemiXPM.CrossPlatformManager proxy = new AlchemiXPM.CrossPlatformManager();
            Console.WriteLine("Submitting...");
            taskId = proxy.SubmitTask("user", "user", taskXml.OuterXml);
            Console.WriteLine("Finished submitting (taskId = '{0}').\nWait a bit and hit <Enter> to get results.", taskId);
            Console.ReadLine();

            Console.WriteLine("Getting results...");
            XmlDocument resultsXml = new XmlDocument();
            resultsXml.LoadXml(proxy.GetFinishedJobs("user", "user", taskId));
            resultsXml.Save(Path.Combine(taskXmlDir, "RESULT_" + taskXmlFile));

            foreach (XmlNode embeddedFile in resultsXml.SelectNodes("//output/embedded_file"))
            {
                WriteBase64EncodedToFile(Path.Combine(taskXmlDir, embeddedFile.Attributes["name"].Value), embeddedFile.InnerText);
                Console.WriteLine("  Wrote output file {0}", embeddedFile.Attributes["name"].Value);
            }

            Console.ReadLine();
        }



        static string ReadBase64EncodedFromFile(string fileLocation)
        {
            return Convert.ToBase64String(ReadByteArrayFromFile(fileLocation));
        }

        static byte[] ReadByteArrayFromFile(string fileLocation)
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

        static void WriteBase64EncodedToFile(string fileLocation, string base64EncodedData)
        {
            WriteByteArrayToFile(fileLocation, Convert.FromBase64String(base64EncodedData));
        }

        static void WriteByteArrayToFile(string fileLocation, byte[] byteArray)
        {
            Stream stream = new FileStream (fileLocation, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(byteArray, 0, byteArray.Length);
            writer.Close();
            stream.Close();
        }



    }
}
