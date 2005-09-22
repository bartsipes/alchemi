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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using Alchemi.Core;
using Alchemi.Core.Executor;
using Alchemi.Core.Utility;
using log4net;
using log4net.Config;

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
		private static ILog logger;

		public ExecutorService()
		{
			InitializeComponent();
			execContainer = new ExecutorContainer();
			// subscribe to events
			ExecutorContainer.GotDisconnected += new GotDisconnectedEventHandler(this.Executor_GotDisconnected);
		
			Logger.LogHandler += new LogEventHandler(this.Log);
		}

		// The main entry point for the process
		static void Main(string[] args)
		{
			ServiceBase[] ServicesToRun;

			//the unhandled exception handler is set here as opposed to the constructor, since the Main does a lot more things that 
			//can cause errors.
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(DefaultErrorHandler);

			try
			{
				Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
				logger = LogManager.GetLogger(typeof(ExecutorService));
				XmlConfigurator.Configure(new FileInfo(string.Format("{0}.config",Assembly.GetExecutingAssembly ().Location)));

				//create directory and set permissions for dat directory...for logging.
				string datDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"dat");
				ServiceUtil.CreateDir(datDir,"SYSTEM");

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
				HandleAllUnknownErrors("Error in Main: ",ex);
			}
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
				logger.Info("Starting Alchemi Executor Service v."+Utils.AssemblyVersion);
				ThreadStart ts = new ThreadStart(Start);
				Thread t = new Thread(ts);
				t.Start();
				t.Join(25000);
			}
			catch (Exception e)
			{
				logger.Error("Error Starting Alchemi Executor Service " , e);
			}
		}

		private void Start()
		{
			//the executor service is for dedicated machines only
			try
			{
				//this will cause the ExecutorContainer to read the config from the Alchemi.Executor.config.xml
				execContainer.Start();
				logger.Info("Executor Service Started");
			}
			catch (Exception e)
			{
				logger.Warn("Exception starting Executor service...Reconnecting...",e);
				try
				{
					execContainer.Reconnect();
				}
				catch (Exception ex1)
				{
					logger.Error("Error trying to reconnect: ",ex1);
				}
			}
		}

		private void Stop()
		{
			try
			{
				if (execContainer!=null)
					execContainer.Stop();
				else
					logger.Debug("execContainer was null.");

				logger.Info("Executor Service Stopped");
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
			if (logger!=null)
			{
				logger.Error("Unhandled Exception in Alchemi Executor Service: sender = "+sender,e);
			}
			else
			{
				try
				{
					TextWriter tw = File.CreateText("alchemiExecutorError.txt");
					tw.WriteLine("Unhandled Error in Alchemi Executor Service. Logger is null. Sender ="+sender);
					tw.WriteLine(e.ToString());
					tw.Flush();
					tw.Close();
					tw = null;
				}
				catch
				{
					//can't do much more. perhaps throw it? so that atleast the user knows something is wrong?
					//throw new Exception("Unhandled Error in Alchemi Executor Service. Logger is null. Sender ="+sender,e);
				}
			}		
		}

		/// <summary>
		/// Stop this service.
		/// </summary>
		protected override void OnStop()
		{
			try
			{
				logger.Info("Stopping Executor Service...");
				ThreadStart ts = new ThreadStart(Stop);
				Thread t = new Thread(ts);
				t.Start();
				t.Join(25000);
			}
			catch (Exception e)
			{
				logger.Error("Error Stopping Service " ,e);
			}
		}

		private void Executor_GotDisconnected()
		{
			try
			{
				logger.Info("Got disconnected!");
				if (this.execContainer.Config.Dedicated && this.execContainer.Config.RetryConnect)
				{
					logger.Info("Trying to reconnect.");
					execContainer.Reconnect();					
				}
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
		}

		private static void uninstallService()
		{
			string path = string.Format ("/assemblypath={0}", Assembly.GetExecutingAssembly ().Location);
			ServiceHelper.uninstallService(new ProjectInstaller(),path);
		}

		private void Log(object sender, LogEventArgs e)
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
	}
}
