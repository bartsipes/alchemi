using System;
using Alchemi.Core;
using Alchemi.Core.Owner;

namespace Alchemi.Examples.Tutorial
{
    [Serializable]
    public class MultiplierThread : GThread
    {
        private int _A;
        private int _B;
        private int _Result;

        public int Result
        {
            get { return _Result; }
        }

        public MultiplierThread(int a, int b)
        {
            _A = a;
            _B = b;
        }

        public override void Start()
        {
            _Result = _A * _B;
        }
    }

    class MultiplierApplication
    {
        static GApplication ga;

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("[enter] to start grid application ...");
            Console.ReadLine();

            // create standard grid connection 
            GConnection gc = new GConnection("localhost", 9000, "user", "user");

            // create multi-use grid application
            ga = new GApplication(true);
            ga.ApplicationName = "Tutorial OTF - Alchemi sample";

            // use standard grid connection
            ga.Connection = gc;

            // add GridThread module (this executable) as a dependency
            ga.Manifest.Add(new ModuleDependency(typeof(MultiplierThread).Module));

            // set the thread finish callback method
            ga.ThreadFinish += new GThreadFinish(ThreadFinished);

            // start application
            ga.Start();

            int i = -1;
            string input = "";

            while (input != "x")
            {
                i++;
                // create thread
                MultiplierThread thread = new MultiplierThread(i, i + 1);

                // add thread to application
                ga.StartThread(thread);

                Console.WriteLine("[enter] to start a new thread, [x] + [enter] to stop");
                input = Console.ReadLine();
            }

            ga.Stop();

            ApplicationStopped();
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

        static void ApplicationStopped()
        {
            Console.WriteLine("\napplication stopped");
            Console.WriteLine("\n[enter] to continue ...");
        }
    }
}