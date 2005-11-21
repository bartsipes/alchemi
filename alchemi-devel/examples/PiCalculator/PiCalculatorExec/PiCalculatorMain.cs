using System;
using System.Reflection;
using System.Text;
using System.Configuration;
using Alchemi.Core;
using Alchemi.Core.Owner;
using Alchemi.Core.Utility;
using log4net;

// Configure log4net using the .config file
[assembly: log4net.Config.XmlConfigurator(Watch=true)]

namespace Alchemi.Examples.PiCalculator
{
    class PiCalculatorMain
    {
		// Create a logger for use in this class
		private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // the product of these two numbers determines the number of digits of pi calculated
        static int NumThreads = 10;
        static int DigitsPerThread = 10;
        
        static DateTime StartTime;
        static GApplication App;

		static int th = 0;

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("[Pi Calculator Grid Application]\n--------------------------------\n");

            Console.WriteLine("Press <enter> to start ...");
            Console.ReadLine();

			Logger.LogHandler += new LogEventHandler(LogHandler);

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
				
				logger.Debug("PiCalc started.");
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: {0}", e.StackTrace);
            }

            Console.ReadLine();
        }

    	private static void LogHandler(object sender, LogEventArgs e)
    	{
			switch (e.Level)
			{
				case LogLevel.Debug:
					string message = e.Source  + ":" + e.Member + " - " + e.Message;
					logger.Debug(message,e.Exception);
					break;
				case LogLevel.Info:
					logger.Info(e.Message);
					break;
				case LogLevel.Error:
					logger.Error(e.Message,e.Exception);
					break;
				case LogLevel.Warn:
					logger.Warn(e.Message);
					break;
			}
    	}

    	static void StartTiming()
        {
            StartTime = DateTime.Now;
        }

        static void ThreadFinished(GThread thread)
        {
			th++;
            Console.WriteLine("grid thread # {0} finished executing", thread.Id);

//			if (th > 1)
//			{
//				Console.WriteLine("For testing aborting threads beyond th=5");
//				try
//				{
//					Console.WriteLine("Aborting thread th=" + th);
//					thread.Abort();
//					Console.WriteLine("DONE Aborting thread th=" + th);
//				}
//				catch (Exception e)
//				{
//					Console.WriteLine(e.ToString());
//				}
//			}
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

			//Console.WriteLine("Thread finished fired: " + th + " times");
			Console.WriteLine("Application Finished");
        }
    }
}
