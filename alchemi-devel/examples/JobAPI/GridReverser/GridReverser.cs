using System;
using System.IO;
using Alchemi.Core;
using Alchemi.Core.Owner;

namespace Alchemi.Examples.CrossPlatformDemo
{
    class GridReverser
    {
        static GApplication ga;
        
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Press [enter] to start ...");
            Console.ReadLine();

            try
            {
                ga = new GApplication(GConnection.FromConsole("localhost", "9000", "user", "user"));
                ga.ApplicationName = "Grid Reverser - Alchemi sample";

                ga.ThreadFinish += new GThreadFinish(JobFinished);
                ga.ApplicationFinish += new GApplicationFinish(ApplicationFinished);
                
                ga.Manifest.Add(new EmbeddedFileDependency("Reverse.exe", @"..\..\..\Reverse\bin\Debug\Reverse.exe"));

                for (int jobNum=0; jobNum<2; jobNum++)
                {
                    GJob job = new GJob();
                    string inputFileName = string.Format("input{0}.txt", jobNum);
                    string outputFileName = string.Format("output{0}.txt", jobNum);

                    job.InputFiles.Add(new EmbeddedFileDependency(inputFileName, @"..\..\" + inputFileName));
                    job.RunCommand = string.Format("Reverse {0} > {1}", inputFileName, outputFileName);
                    job.OutputFiles.Add(new EmbeddedFileDependency(outputFileName));

                    ga.Threads.Add(job);
                }

                ga.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType() + " : " + e.Message);
                Console.ReadLine();
                return;
            }
            Console.WriteLine("Started .. Waiting for jobs to finish ..\n");
            Console.ReadLine();
        }

        public static void JobFinished(GThread thread)
        {
            GJob job = (GJob) thread;
            Console.WriteLine("Finished job {0}", job.Id);

            foreach (FileDependency fd in job.OutputFiles)
            {
            	Directory.CreateDirectory("job_" + job.Id);
                fd.UnPack(Path.Combine("job_" + job.Id, fd.FileName));
                Console.WriteLine("Unpacked file {0} for job {1}", fd.FileName, job.Id);
            }
            Console.WriteLine();
        }

        public static void ApplicationFinished()
        {
            ga.Stop();
        }
    }
}
