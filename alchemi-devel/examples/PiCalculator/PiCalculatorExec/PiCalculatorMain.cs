using System;
using System.Text;
using System.Configuration;
using Alchemi.Core;
using Alchemi.Core.Utility;

namespace Alchemi.Examples.PiCalculator
{
    class PiCalculatorMain
    {
        // the product of these two numbers determines the number of digits of pi calculated
        static int NumThreads = 10;
        static int DigitsPerThread = 10;
        
        static DateTime StartTime;
        static GApplication App;

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("[Pi Calculator Grid Application]\n--------------------------------\n");

            Console.WriteLine("Press <enter> to start ...");
            Console.ReadLine();

            try
            {
                // get settings from user
                GConnection gc = GConnection.FromConsole("localhost", "9000", "user", "user");

                StartTiming();
                
                // create a new grid application
                App = new GApplication(gc);

                // add the module containing PiCalcGridThread to the application manifest        
                App.Manifest.Add(new ModuleDependency(typeof(PiCalculator.PiCalcGridThread).Module));

                // create and add the required number of grid threads
                for (int i = 0; i < NumThreads; i++)
                {
                    int StartDigitNum = 1 + (i*DigitsPerThread);
          
                    Console.WriteLine(
                        "starting a thread to calculate the digits of pi from {0} to {1}",
                        StartDigitNum,
                        StartDigitNum + DigitsPerThread - 1);
          
                    PiCalcGridThread thread = new PiCalcGridThread(
                        StartDigitNum,
                        DigitsPerThread
                        );

                    App.Threads.Add(thread);
                }

                // subcribe to events
                App.ThreadFinish += new GThreadFinish(ThreadFinished);
                App.ApplicationFinish += new GApplicationFinish(ApplicationFinished); 
        
                // start the grid application
                App.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: {0}", e.Message);
            }

            Console.ReadLine();
        }

        static void StartTiming()
        {
            StartTime = DateTime.Now;
        }

        static void ThreadFinished(GThread thread)
        {
            Console.WriteLine("grid thread # {0} finished executing", thread.Id);
        }

        static void ApplicationFinished()
        {
            StringBuilder result = new StringBuilder();
            for (int i=0; i<App.Threads.Count; i++)
            {
                PiCalcGridThread pcgt = (PiCalcGridThread) App.Threads[i];
                result.Append(pcgt.Result);
            }
            
            Console.WriteLine(
                "===\nThe value of Pi to {0} digits is:\n3.{1}\n===\nTotal time taken = {2}\n===",
                NumThreads * DigitsPerThread,
                result,
                DateTime.Now - StartTime);
        }
    }
}
