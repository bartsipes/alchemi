using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Alchemi.Core.Owner;

namespace CustomGThreadExample
{
    /// <summary>
    /// This program is a simple demonstration of an Alchemi enabled grid application.
    /// 
    /// The program reads an input file block by block and passes the string data
    /// to grid threads. The grid application is then run, and the output from each
    /// thread is added to an array.  When the application has finished, the output of
    /// that array is recombined and written to disk.
    /// 
    /// With minor modifications, this example can be adapted for any task that allows for
    /// parallel processing of an input file separated into blocks that can be later
    /// recombined to an output file.
    /// 
    /// E.g., increasing or decreasing the volume of a WAV file, or encoding WAV to MP3.
    /// </summary>
    class Program
    {
        string[] _outputArray;
        string _inputFileName;
        string _outputFileName;

        static void Main( string[] args )
        {
            Program program = new Program();

            program.Run();

            Console.ReadLine();
        }

        void Run()
        {
            // All grid applications must have a GApplication instance.  The GConnection information
            // specifies the Manager's host name, the port to connect on, and the username and password
            // to use.  A GApplication is created with this connection information.
            GConnection gridConnection = new GConnection( "localhost", 9000, "user", "user" );
            GApplication gridApplication = new GApplication( gridConnection );

            Console.WriteLine( "Enter the input file name: " );
            this._inputFileName = Console.ReadLine();

            Console.WriteLine( "Enter the output file name: " );
            this._outputFileName = Console.ReadLine();

            try
            {
                // Open the input file for reading.
                StreamReader streamReader = new StreamReader( this._inputFileName );

                int blockSize = 100;
                char[] charBuffer = new char[ blockSize ];
                string stringBuffer;
                int currentBlock = 0;

                // In order to ensure that all required DLLs to run the thread exist at each Executor
                // we must add a dependency to the Manifest for each DLL and/or file needed by the GThreads.  
                gridApplication.Manifest.Add( new ModuleDependency( typeof( CustomGThread ).Module ) );

                // Loop through the file reading blocks of size blockSize into the char[] array charBuffer
                // until we hit the end of the file.
                while( streamReader.ReadBlock( charBuffer, 0, blockSize ) != 0 )
                {
                    // ReadBlock returns a char[] buffer so convert it to a string to pass to our custom GThread.
                    stringBuffer = new string( charBuffer );

                    // Instantiate our custom GThread and pass the data we need to process throgh the constructor.
                    // Note: other methods of passing data to GThreads are possible.
                    CustomGThread customGThread = new CustomGThread( currentBlock, stringBuffer );

                    // Add the thread to the GApplication's Threads collection.
                    gridApplication.Threads.Add( customGThread );

                    currentBlock++;
                }

                // Close the input file.
                streamReader.Close();

                // Resize the array that will hold our output so there is one entry per GThread.
                this._outputArray = new string[ currentBlock ];

                // Bind the ThreadFinished and ApplicationFinished events to local event handlers.
                gridApplication.ThreadFinish += new GThreadFinish( gridApp_ThreadFinish );
                gridApplication.ApplicationFinish += new GApplicationFinish( gridApp_ApplicationFinish );

                // Start the GApplication.
                gridApplication.Start();

                Console.WriteLine( "Application Started" );
            }
            catch( Exception ex )
            {
                Console.WriteLine( "An exception occurred: " + ex.Message + "\r\n" + ex.StackTrace );
            }
        }

        /// <summary>
        /// The GApplication.ThreadFinished event fires each time a thread has finished processing.
        /// </summary>
        /// <param name="thread">The thread that finished.</param>
        private void gridApp_ThreadFinish( GThread thread )
        {
            // Cast the GThread to our custom type so that we can access our custom properties.
            CustomGThread customGThread = (CustomGThread) thread;

            Console.WriteLine( "Thread " + customGThread.Index + " finished" );

            // Add the output from the thread to our output array.
            this._outputArray[ customGThread.Index ] = customGThread.Output;
        }

        /// <summary>
        /// The GApplication.ApplicationFinished event fires when all GThreads have finished processing.
        /// </summary>
        private void gridApp_ApplicationFinish()
        {
            // Open the output file for writing.
            StreamWriter streamWriter = new StreamWriter( this._outputFileName );

            // Loop through our output array and write the outupt from each thread to the file.
            foreach( string outputString in this._outputArray )
            {
                streamWriter.Write( outputString );
            }

            // Close the output file to ensure the data is written to disk.
            streamWriter.Close();

            Console.WriteLine( "Application Finished" );
        }
    }
}
