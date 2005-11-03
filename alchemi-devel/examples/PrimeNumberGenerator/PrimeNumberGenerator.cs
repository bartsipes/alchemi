using System;
using Alchemi.Core;
using Alchemi.Core.Owner;

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
        public static GApplication App = new GApplication();

        [STAThread]
        static void Main(string[] args)
        {
            // create grid threads to check if some randomly generated large numbers are prime
            Random rnd = new Random();
            for (int i=0; i<10; i++)
            {
                App.Threads.Add(new PrimeNumberChecker(rnd.Next(1000)));
            }

            // initialise application
            Init();

            // start the application
            App.Start();

            Console.ReadLine();

            // stop the application
            App.Stop();
        }

        private static void Init()
        {
            // specify connection properties
            App.Connection = new GConnection("localhost", 9000, "user", "user");

            // grid thread needs to
            App.Manifest.Add(new ModuleDependency(typeof(PrimeNumberChecker).Module));

            // subscribe to ThreadFinish event
            App.ThreadFinish += new GThreadFinish(App_ThreadFinish);
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
        }

    }
}


