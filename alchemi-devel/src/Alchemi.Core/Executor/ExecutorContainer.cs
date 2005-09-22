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

		/// <summary>
		/// Reconnect to the Manager.
		/// </summary>
		public void Reconnect()
		{
			Reconnect(Config.RetryMax ,Config.RetryInterval);
		}

		/// <summary>
		/// Try to Reconnect to the Manager.
		///
		/// </summary>
		/// <param name="maxRetryCount">Maximum number of times to retry, if connection fails. -1 signifies: try forever.</param>
		/// <param name="retryInterval">Retry connection after every 'retryInterval' seconds.</param>
		public void Reconnect(int maxRetryCount, int retryInterval)
		{
			int retryCount = 0;
			const int DEFAULT_RETRY_INTERVAL = 30; //30 seconds
			while (true)
			{
				if (maxRetryCount >= 0 && retryCount < maxRetryCount)
					break;

				logger.Debug ("Attempting to reconnect ... attempt: "+(retryCount+1));
				retryCount++;
				try //handle the error since we want to retry later.
				{
					Connect();						
				}
				catch{} //ignore the error. retry later.

				if (!Connected)
				{
					//play safe.
					if (retryInterval<0 || retryInterval>System.Int32.MaxValue)
						retryInterval = DEFAULT_RETRY_INTERVAL;
					Thread.Sleep(retryInterval*1000); //convert to milliseconds
				}
				else
				{
					break;
				}
			}

			//if Executor is null, then it is not Connected. The Connected property actually checks for that.
			if (Executor!=null)
			{
				if (Executor.Dedicated)
				{
					logger.Debug("Reconnected successfully.[Dedicated mode.]");
				}
				else //not dedicated...
				{
					logger.Debug("Reconnected successfully.[Non-dedicated mode.]");
					Executor.StartNonDedicatedExecuting(1000);
				}
			}
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

		/// <summary>
		/// Read the configuration file.
		/// </summary>
		/// <param name="useDefault"></param>
		public void ReadConfig(bool useDefault)
		{
			if (!useDefault)
			{
				//handle the error and lets use default if the config cannot be found.
				try
				{
					Config = Configuration.GetConfiguration();
					logger.Debug("Using configuration from Alchemi.Executor.config.xml ...");
				}
				catch (Exception ex)
				{
					logger.Debug("Error getting existing config. Continuing with default config...",ex);
					useDefault = true;
				}
			}

			//this needs to be a seperate if-block, since we may have a problem getting existing config. then we use default
			if (useDefault)
			{
				Config = new Configuration();
				logger.Debug("Using default configuration...");
			}
		}

		/// <summary>
		/// Returns a value specifying whether the Connection has been verified previously.
		/// </summary>
		public bool ConnectVerified
		{
			get { return Config.ConnectVerified; }
		}

		/// <summary>
		/// Stops the Executor Container
		/// </summary>
		public void Stop()
		{
			if (Config!=null)
			{
				Config.Slz();
			}
			if (Executor != null)
			{
				Disconnect();
			}
		}

		/// <summary>
		/// Starts the Executor Container
		/// </summary>
		public void Start()
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
