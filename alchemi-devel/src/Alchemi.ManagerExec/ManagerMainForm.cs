#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	ManagerMainForm.cs
* Project		:	Alchemi Manager Application
* Created on	:	2003
* Copyright		:	Copyright © 2006 The University of Melbourne
*					This technology has been developed with the support of 
*					the Australian Research Council and the University of Melbourne
*					research grants as part of the Gridbus Project
*					within GRIDS Laboratory at the University of Melbourne, Australia.
* Author         :  Akshay Luther (akshayl@csse.unimelb.edu.au), Rajkumar Buyya (raj@csse.unimelb.edu.au), and Krishna Nadiminti (kna@csse.unimelb.edu.au)
* License        :  GPL
*					This program is free software; you can redistribute it and/or 
*					modify it under the terms of the GNU General Public
*					License as published by the Free Software Foundation;
*					See the GNU General Public License 
*					(http://www.gnu.org/copyleft/gpl.html) for more details.
*
*/ 
#endregion

using System;
using System.ComponentModel;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using Alchemi.Core;
using Alchemi.Core.Manager;
using Alchemi.Manager;
using log4net;
using Alchemi.Core.Utility;
using System.Diagnostics;


// Configure log4net using the .config file
[assembly: log4net.Config.XmlConfigurator(Watch=true)]

namespace Alchemi.ManagerExec
{
	public class ManagerMainForm : ManagerTemplateForm
	{
		public ManagerMainForm() : base()
		{
			InitializeComponent();
			this.Text = "Alchemi Manager";
			Logger.LogHandler += new LogEventHandler(LogHandler);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.tabPage1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.gpBoxNodeConfig.SuspendLayout();
            this.gpBoxActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbIntermediate
            // 
            this.cbIntermediate.CheckedChanged += new System.EventHandler(this.cbIntermediate_CheckedChanged);
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 537);
            // 
            // lnkViewLog
            // 
            this.lnkViewLog.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkViewLog_LinkClicked);
            // 
            // ManagerMainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(458, 559);
            this.Name = "ManagerMainForm";
            this.Load += new System.EventHandler(this.ManagerMainForm_Load);
            this.tabPage1.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.gpBoxNodeConfig.ResumeLayout(false);
            this.gpBoxNodeConfig.PerformLayout();
            this.gpBoxActions.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		//-----------------------------------------------------------------------------------------------    

        private void lnkViewLog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //show the log .
            string logFile = null;
            try
            {
                logFile = GetLogFilePath();
                Process p = new Process();
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.FileName = logFile;
                p.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Could not show log file {0}! Error : {1}", logFile, ex.Message), "Alchemi Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

		private void LogHandler(object sender, LogEventArgs e)
		{
			// Create a logger for use in this class
			ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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
					logger.Warn(e.Message, e.Exception);
					break;
			}
		}
   
		//-----------------------------------------------------------------------------------------------    

		private void ManagerMainForm_Load(object sender, EventArgs e)
		{
			//normal startup. not a service
			_container = new  ManagerContainer();
			_container.ReadConfig();
			
			RefreshUIControls(_container.Config);
			btStart.Focus();
		}

		//-----------------------------------------------------------------------------------------------    

		private void cbIntermediate_CheckedChanged(object sender, EventArgs e)
		{
			_container.Config.Intermediate = cbIntermediate.Checked;
            RefreshUIControls(_container.Config);
		}

		#region Implementation of methods from ManagerTemplateForm
		protected override bool Started
		{
			get
			{
				bool started = false;
				if (_container != null && _container.Started)
				{
					started = true;
				}
				return started;
			}
		}
		protected override void Exit()
		{
			StopManager();

			this.Close();
			Application.Exit();
		}

		protected override void ResetManager()
		{
			_container.ReadConfig();
			RefreshUIControls(_container.Config);
		}
     
		protected override void StopManager()
		{
			if (Started)
			{
				pbar.Value = 0;
				pbar.Show();
				pbar.Value = pbar.Maximum / 2;
				
				statusBar.Text = "Stopping Manager...";
				Log("Stopping Manager...");
				btStop.Enabled = false;
				btStart.Enabled = false;
				try
				{
					_container.Stop();
					_container = null;
				}
				catch (Exception ex)
				{
					logger.Error("Error stopping manager",ex);
				}

				pbar.Value = pbar.Maximum;
	
				Log("Manager stopped.");

			}
			
			RefreshUIControls(_container.Config);

		}
    
		protected override void StartManager()
		{

			if (Started && _container!=null)
			{
				RefreshUIControls(_container.Config);
				return;
			}

			//to avoid people from clicking this again
			btStart.Enabled = false;
			btReset.Enabled = false;
			btStop.Enabled = false;

			statusBar.Text = "Starting Manager...";

			Log("Attempting to start Manager...");

			pbar.Value = 0;
			pbar.Show();

			if (_container == null)
				_container = new ManagerContainer();

			_container.Config = GetConfigFromUI();
			_container.RemotingConfigFile = Assembly.GetExecutingAssembly().Location + ".config";

			try
			{
				_container.Start();

				Log("Manager started");

				//for heirarchical stuff
				//				if (Config.Intermediate)
				//				{
				//					//Config.Id = Manager.Id;
				//					//Config.Dedicated = Manager.Dedicated;
				//				}

			}
			catch (Exception ex)
			{
				string errorMsg = string.Format("Could not start Manager. Reason: {0}{1}", Environment.NewLine, ex.Message);
				if (ex.InnerException != null)
				{
					errorMsg += string.Format("{0}", ex.InnerException.Message);
				}
				Log(errorMsg);
				logger.Error(errorMsg,ex);
			}
			RefreshUIControls(_container.Config);
		}
		#endregion

	}
}
