/* 

File: ExecutorService.cs for Alchemi Executor Service
   Written by 
   Rodrigo Assirati Dias (rdias@ime.usp.br) and
   Paula Akemi Nishimoto (pakemi@gmail.com)
   under supervision of Dr. Roberto Hirata Jr. (hirata@ime.usp.br)
   
	Updated: July 2005: Krishna Nadiminti
*/

#region Alchemi copyright notice
/*
  Alchemi [.NET Grid Computing Framework]
  http://www.alchemi.net
  
  Copyright (c)  Akshay Luther (2002-2004) & Rajkumar Buyya (2003-to-date), 
  GRIDS Lab, The University of Melbourne, Australia.
  
  Maintained and Updated by: Krishna Nadiminti (2005-to-date)
---------------------------------------------------------------------------

  This program is free software; you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation; either version 2 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program; if not, write to the Free Software
  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/
#endregion

using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using Alchemi.Core.Executor;
using Alchemi.Core.Utility;
using log4net;

namespace Alchemi.ExecutorService
{
	public class ExecutorService : ServiceBase
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		private ExecutorContainer execContainer = null;

		// Create a logger for use in this class
		private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		//The value that will be returned to appstart
		private static int returnCode = 0;

		public ExecutorService()
		{
			// This call is required by the Windows.Forms Component Designer.
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(DefaultErrorHandler);

			InitializeComponent();
			execContainer = new ExecutorContainer();
			// subscribe to events
			ExecutorContainer.GotDisconnected += new GotDisconnectedEventHandler(this.Executor_GotDisconnected);
		}

		// The main entry point for the process
		[STAThread]
		static int Main(string[] args)
		{
			ServiceBase[] ServicesToRun;

			//the unhandled exception handler is set here as opposed to the constructor, since the Main does a lot more things that 
			//can cause errors.
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(DefaultErrorHandler);

			try
			{
				string opt = null ;
				if ( args.Length > 0)
				{
					opt = args [0];
				}

				if (opt != null && opt.ToLower () == "/install")
				{
					installService();
				}
				else if (opt != null && opt.ToLower () == "/uninstall")
				{
					uninstallService();
				}
				else
				{
					// More than one user Service may run within the same process. To add
					// another service to this process, change the following line to
					// create a second service object. For example,
					//
					//   ServicesToRun = new System.ServiceProcess.ServiceBase[] {new Service1(), new MySecondUserService()};
					//

					ExecutorService es = new ExecutorService();
					if (!ServiceHelper.checkServiceInstallation(es.ServiceName))
					{
						installService();
					}

					ServicesToRun = new ServiceBase[] { es };
					ServiceBase.Run(ServicesToRun);
					es = null;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Unknown error in Alchemi Manager Service: " + ex.ToString());
				HandleAllUnknownErrors("Error in Main: ",ex);
				returnCode = 999; //some error code.
			}

			return returnCode;
		}

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new Container();
			this.ServiceName = "Alchemi Executor Service";
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
				execContainer = null;
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Set things in motion so your service can do its work.
		/// </summary>
		protected override void OnStart(string[] args)
		{
			try
			{
				ThreadStart ts = new ThreadStart(Start);
				Thread t = new Thread(ts);
				t.Start();
			}
			catch (Exception e)
			{
				EventLog.WriteEntry("Error Starting Service " + e,System.Diagnostics.EventLogEntryType.Error);
			}
		}

		private void Start()
		{
			//the executor service is for dedicated machines only
			try
			{
				execContainer.Start();
				logger.Info("Service Started");
			}
			catch (Exception e)
			{
				logger.Error ("Error while starting service:",e) ;
				EventLog.WriteEntry("Error Stopping Service " + e,System.Diagnostics.EventLogEntryType.Error);
				Stop();
			}
		}

		private void Stop()
		{
			try
			{
				execContainer.Stop();
				logger.Info("Service Stopped");
			}
			catch (Exception e)
			{
				logger.Error("Error stopping ExecutorService. ",e);
			}
		}

		private static void DefaultErrorHandler(object sender, UnhandledExceptionEventArgs args)
		{
			Exception e = (Exception) args.ExceptionObject;
			HandleAllUnknownErrors(sender.ToString(),e);
		} 

		//just to follow the same model as the windows forms app
		private static void HandleAllUnknownErrors(string sender, Exception e)
		{
			logger.Error("AlchemiExecutorService Unknown Error: " + sender,e);
			//EventLog.WriteEntry("AlchemiExecutorService Unknown Error: " + e,System.Diagnostics.EventLogEntryType.Error);
		}

		/// <summary>
		/// Stop this service.
		/// </summary>
		protected override void OnStop()
		{
			try
			{
				ThreadStart ts = new ThreadStart(Stop);
				Thread t = new Thread(ts);
				t.Start();
			}
			catch (Exception e)
			{
				EventLog.WriteEntry("Error Stopping Service " + e,System.Diagnostics.EventLogEntryType.Error);
			}
		}

		private void Executor_GotDisconnected()
		{
			try
			{
				logger.Info("Got disconnected! Starting thread to try and reconnect.");
				execContainer.Reconnect();
			}
			catch (Exception e)
			{
				logger.Error("Error in Executor_GotDisconnected, while reconnecting. ",e);
			}
		}

		private static void installService()
		{
			string path = string.Format ("/assemblypath={0}",Assembly.GetExecutingAssembly ().Location);
			ServiceHelper.installService(new ProjectInstaller(),path);

//			TransactedInstaller ti = new TransactedInstaller ();
//			ProjectInstaller pi = new ProjectInstaller ();
//			ti.Installers.Add (pi);
//			string[] cmdline = {path};
//			InstallContext ctx = new InstallContext ("Install.log", cmdline );
//			ti.Context = ctx;
//			ti.Install ( new Hashtable ());
//					
//			//this doesnt seem to appear on the console...why?
//			Console.WriteLine("Alchemi Executor Service installed successfully.");
		}

		private static void uninstallService()
		{
			string path = string.Format ("/assemblypath={0}", Assembly.GetExecutingAssembly ().Location);
			ServiceHelper.uninstallService(new ProjectInstaller(),path);

//			TransactedInstaller ti = new TransactedInstaller ();
//			ProjectInstaller pi = new ProjectInstaller ();
//			ti.Installers.Add (pi);
//			string[] cmdline = {path};
//			InstallContext ctx = new InstallContext ("Uninstall.log", cmdline );
//			ti.Context = ctx;
//			ti.Uninstall ( null );
//			Console.WriteLine("Alchemi Executor Service uninstalled successfully.");
		}
	}
}
