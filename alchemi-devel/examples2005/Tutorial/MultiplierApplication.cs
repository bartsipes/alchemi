using System;
using System.Collections.Generic;
using System.Text;

using Alchemi.Core.Owner;

namespace Alchemi.Examples.Tutorial
{
    class MultiplierApplication
    {
        static GApplication ga;

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("[enter] to start grid application ...");
            Console.ReadLine();

            // create grid application
            ga = new GApplication(new GConnection("localhost", 9000, "user", "user"));
            ga.ApplicationName = "Alchemi Tutorial - Alchemi sample";

            // add GridThread module (this executable) as a dependency
            ga.Manifest.Add(new ModuleDependency(typeof(MultiplierThread).Module));

            // create and add 10 threads to the application
            for (int i = 0; i < 10; i++)
            {
                // create thread
                MultiplierThread thread = new MultiplierThread(i, i + 1);

                // add thread to application
                ga.Threads.Add(thread);
            }

            // subscribe to events
            ga.ThreadFinish += new GThreadFinish(ThreadFinished);
            ga.ThreadFailed += new GThreadFailed(ThreadFailed);
            ga.ApplicationFinish += new GApplicationFinish(ApplicationFinished);

            // start application
            ga.Start();

            Console.ReadLine();
        }

        static void ThreadFinished(GThread th)
        {
            // cast GThread back to MultiplierThread
            MultiplierThread thread = (MultiplierThread)th;

            Console.WriteLine(
                "thread # {0} finished with result '{1}'",
                thread.Id,
                thread.Result);
        }

        static void ThreadFailed(GThread th, Exception e)
        {
            Console.WriteLine(
                "thread # {0} finished with error '{1}'",
                th.Id,
                e.Message);
        }

        static void ApplicationFinished()
        {
            Console.WriteLine("\napplication finished");
            Console.WriteLine("\n[enter] to continue ...");
        }
    }
}
