using System;
using System.Reflection;
using Alchemi.Core;
using Alchemi.Core.Owner;
using log4net;

namespace Tutorial
{
    [Serializable]
    class PrimeNumberChecker : GThread
    {
        public readonly int Candidate;
        public int Factors = 0;

        public PrimeNumberChecker(int candidate)
        {
            Candidate = candidate;
        }

        public override void Start()
        {
            // count the number of factors of the number from 1 to the number itself
            for (int d=1; d<=Candidate; d++)
            {
                if (Candidate%d == 0) Factors++;
            }
        }
    }

    class PrimeNumberGenerator
    {
		// Create a logger for use in this class
		private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static GApplication App = new GApplication();
		static DateTime StartTime;
		static int max = 1000000;
		static int primesFound = 0;

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

        [STAThread]
        static void Main(string[] args)
        {
			Logger.LogHandler += new LogEventHandler(LogHandler);

			Console.WriteLine("[PrimeNumber Checker Grid Application]\n--------------------------------\n");
			Console.Write("Enter a maximum limit for Prime Number checking [default=1000000] :");
			string input = Console.ReadLine();
		
			if (input!=null || input.Equals(""))
			{
				try
				{
					max = Int32.Parse(input);
				}catch{}
			}

			Console.WriteLine("Connecting to Alchemi Grid...");
            // initialise application
            Init();

			// create grid threads to check if some randomly generated large numbers are prime
            Random rnd = new Random();
            for (int i=0; i<10; i++)
            {
				int candidate = rnd.Next(max);
				Console.WriteLine("Creating a grid thread to check if {0} is prime...",candidate);
                App.Threads.Add(new PrimeNumberChecker(candidate));
            }


            // start the application
            App.Start();

			Console.WriteLine("Prime Number Generator completed.") ;
            Console.ReadLine();

            // stop the application
			try
			{
				App.Stop();
			}catch {}
        }

        private static void Init()
        {
			try
			{
				// get settings from user
				GConnection gc = GConnection.FromConsole("localhost", "9000", "user", "user");
				StartTime = DateTime.Now;
				App.Connection = gc;

				// grid thread needs to
				App.Manifest.Add(new ModuleDependency(typeof(PrimeNumberChecker).Module));

				// subscribe to ThreadFinish event
				App.ThreadFinish += new GThreadFinish(App_ThreadFinish);
				App.ApplicationFinish += new GApplicationFinish(App_ApplicationFinish);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: "+ex.Message);
				logger.Error("ERROR: ",ex);	
			}
        }

        private static void App_ThreadFinish(GThread thread)
        {
            // cast the supplied GThread back to PrimeNumberChecker
            PrimeNumberChecker pnc = (PrimeNumberChecker) thread;

            // check whether the candidate is prime or not
            bool prime = false;
            if (pnc.Factors == 2) prime = true;

            // display results
            Console.WriteLine("{0} is prime? {1} ({2} factors)", pnc.Candidate, prime, pnc.Factors);

			if (prime)
				primesFound++;
		}

		private static void App_ApplicationFinish()
		{
			Console.WriteLine("Application finished. \nRandom primes found: {0}. Total time taken : {1}", primesFound, DateTime.Now - StartTime);
		}
	}
}


