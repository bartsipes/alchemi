using System;
using System.IO;
using System.Text;

namespace Alchemi.Examples.CrossPlatformDemo
{
    class Reverse
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                string input = new StreamReader(args[0]).ReadToEnd();
                char[] output = new char[input.Length];
                for (int i=0; i<input.Length; i++)
                {
                    output[input.Length - (i+1)] = input[i];
                }
                Console.WriteLine(output);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType() + " : " + e.Message);
            }
        }
    }
}
