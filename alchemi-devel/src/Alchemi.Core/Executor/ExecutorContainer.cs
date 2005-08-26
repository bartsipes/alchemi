using System;
using System.Threading;

namespace Alchemi.Core.Executor
{
	public delegate void ExecutorConnectStatusEventHandler(string statusMessage, int percentDone);
	
	/// <summary>
	/// Summary description for ExecutorContainer.
	/// </summary>
	public class ExecutorContainer
	{
		public static event GotDisconnectedEventHandler GotDisconnected;
		public static event NonDedicatedExecutingStatusChangedEventHandler NonDedicatedExecutingStatusChanged;
		public static event ExecutorConnectStatusEventHandler ExecConnectEvent;
		
		public Configuration Config=null;
		public GExecutor Executor = null;

		// Create a logger for use in this class
		private static readonly Logger logger = new Logger();

		public ExecutorContainer()
		{
			GExecutor.GotDisconnected += new GotDisconnectedEventHandler(GExecutor_GotDisconnected);
			GExecutor.NonDedicatedExecutingStatusChanged += new NonDedicatedExecutingStatusChangedEventHandler(GExecutor_NonDedicatedExecutingStatusChanged);
		}

		public bool Connected
		{
			get { return (Executor == null? false : true);}
		}

		public void Connect()
		{
			logger.Info("Connecting....");

			if (ExecConnectEvent!=null){
				ExecConnectEvent("Connecting....",0);
			}

			RemoteEndPoint rep = new RemoteEndPoint(
				Config.ManagerHost,
				Config.ManagerPort,
				RemotingMechanism.TcpBinary
				);

			logger.Debug("Created remote-end-point");
			if (ExecConnectEvent!=null)
			{
				ExecConnectEvent("Created remote-end-point",20);
			}

			OwnEndPoint oep = new OwnEndPoint(
				Config.OwnPort,
				RemotingMechanism.TcpBinary
				);

			logger.Debug("Created own-end-point");
			if (ExecConnectEvent!=null)
			{
				ExecConnectEvent("Created own-end-point",40);
			}

			// connect to manager
			Executor = new GExecutor(rep, oep, Config.Id, Config.Dedicated, new SecurityCredentials(Config.Username, Config.Password), AppDomain.CurrentDomain.BaseDirectory);
			//Executor.PingExecutor();
			
			if (ExecConnectEvent!=null)
			{
				ExecConnectEvent("Updating executor configuration.",80);
			}
			Config.ConnectVerified = true;
			Config.Id = Executor.Id;
			Config.Dedicated = Executor.Dedicated;
			Config.ConnectVerified = true;

			if (ExecConnectEvent!=null)
			{
				ExecConnectEvent("Saved configuration.",60);
			}

			Config.Slz();

			if (ExecConnectEvent!=null)
			{
				ExecConnectEvent("Connected successfully.",100);
			}

			logger.Info("Connected successfully.");
		}

		public void Reconnect()
		{
			//use default values
			Reconnect(3,5000);
		}

		public void Reconnect(int maxRetryCount, int retryInterval)
		{
			int retryCount = 0;
			try
			{
				//Config.
				while (retryCount < maxRetryCount)
				{
					logger.Debug ("Attempting to reconnect ... attempt: "+(retryCount+1));
					retryCount++;
					Connect();
					if (Executor == null)
					{
						Thread.Sleep(retryInterval);
					}
					else
					{
						break;
					}
				}
				if (!Executor.Dedicated)
				{
					logger.Debug("Reconnected successfully.[Non-dedicated mode.]");
					Executor.StartNonDedicatedExecuting(1000);
				}
			}
			catch {}
		}

		public void Disconnect()
		{
			if (Connected)
			{
				Executor.Disconnect();
				Executor = null;
				logger.Info("Disconnected successfully.");
			}
		}

		public void ReadConfig(bool useDefault)
		{
			if (!useDefault)
			{
				try
				{
					Config = Configuration.GetConfiguration();
					logger.Debug("Using configuration from Alchemi.Executor.config.xml ...");
				}
				catch (Exception)
				{
					useDefault = true;
				}
			}

			if (useDefault)
			{
				Config = new Configuration();
				logger.Debug("Using default configuration...");
			}
		}

		public bool ConnectVerified
		{
			get { return Config.ConnectVerified; }
		}

		public void Stop()
		{
			Config.Slz();
			if (Executor != null)
			{
				Disconnect();
			}
		}

		public void Start()
		{
			try
			{
				logger.Debug("debug mode: curdir env="+Environment.CurrentDirectory + " app-base="+AppDomain.CurrentDomain.BaseDirectory);

				ReadConfig(false);

				if (ConnectVerified)
				{
					logger.Info("Using last verified configuration ...");
					Connect();
				}
				else
				{
					if (!Connected)
						Connect();
				}
			}
			catch (Exception e)
			{
				logger.Error("Error starting exec-container.",e);
				throw e;
			}
		}

		private void GExecutor_GotDisconnected()
		{
			//bubble the event to whoever handles this.
			if (GotDisconnected!=null)
				GotDisconnected();
		}

		private void GExecutor_NonDedicatedExecutingStatusChanged()
		{
			//bubble the event up
			if (NonDedicatedExecutingStatusChanged!=null)
				NonDedicatedExecutingStatusChanged();
		}
	}
}
