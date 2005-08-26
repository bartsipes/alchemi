using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using Alchemi.Core.Manager;
using Alchemi.Core.Utility;
using log4net;

namespace Alchemi.ManagerService
{
	public class ManagerService : ServiceBase
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		private ManagerContainer _container = null;

		// Create a logger for use in this class
		private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		//The value that will be returned to appstart
		private static int returnCode=0;

		public ManagerService()
		{
			// This call is required by the Windows.Forms Component Designer.
			InitializeComponent();

			_container = new  ManagerContainer();
			_container.RemotingConfigFile = "Alchemi.ManagerService.exe.config";
		}

		// The main entry point for the process
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

					ManagerService ms = new ManagerService();
					if (!ServiceHelper.checkServiceInstallation(ms.ServiceName))
					{
						//installService();
					}

					ServicesToRun = new ServiceBase[] { ms };
					ServiceBase.Run(ServicesToRun);
					ms = null;
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
			this.ServiceName = "Alchemi Manager Service";
			this.AutoLog = true;
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
				ThreadStart ts = new ThreadStart(Start);
				Thread t = new Thread(ts);
				t.Start();
				EventLog.WriteEntry("Starting Alchemi Manager Service: v."+Utils.AssemblyVersion);
			}
			catch (Exception e)
			{
				EventLog.WriteEntry("Error Starting Service: " + e,EventLogEntryType.Error);
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
			logger.Error("AlchemiManagerService Unknown Error: " + sender,e);
			//EventLog.WriteEntry("AlchemiManagerService Unknown Error: " + e,System.Diagnostics.EventLogEntryType.Error);
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
				EventLog.WriteEntry("Error Stopping Service " + e,EventLogEntryType.Error);
			}
		}

		private void Start()
		{
			try
			{
				_container.Start();
			}
			catch (Exception ex)
			{
				logger.Error("Error starting manager container",ex);
				EventLog.WriteEntry("Error Stopping Service " + ex,EventLogEntryType.Error);
				OnStop();
			}
		}

		private void Stop()
		{
			try
			{
				_container.Stop();	
			}
			catch (Exception ex)
			{
				logger.Error("Error stopping manager container",ex);
				EventLog.WriteEntry("Error Stopping Service " + ex,EventLogEntryType.Error);
			}
			finally 
			{
				_container = null;
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
//			Console.WriteLine("Alchemi Manager Service installed successfully.");
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
//			Console.WriteLine("Alchemi Manager Service uninstalled successfully.");
		}
	}
}
