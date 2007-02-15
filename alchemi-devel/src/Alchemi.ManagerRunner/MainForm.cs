using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Alchemi.Manager;
using System.Reflection;

namespace Alchemi.ManagerRunner
{
    public partial class MainForm : Form
    {
        ManagerContainer _managerContainer = new ManagerContainer();

        public MainForm()
        {
            InitializeComponent();

            try
            {
                // position in lower right (with buffer)
                this.Left = Screen.PrimaryScreen.WorkingArea.Right - this.Width - 5;
                this.Top = Screen.PrimaryScreen.WorkingArea.Bottom - this.Height - 5;

                this.InitializeManagerContainer();

                this.uiTimer.Start();
            }
            catch( Exception exception )
            {
                HandleException( exception );
            }
        }

        private void UpdateUi()
        {
            this.SetButtonStateOnManagerState();
        }

        void InitializeManagerContainer()
        {
            try
            {
                _managerContainer.Config = this.GetConfiguration();
                _managerContainer.RemotingConfigFile = Assembly.GetExecutingAssembly().Location + ".config";
            }
            catch( Exception exception )
            {
                HandleException( exception );
            }
        }

        void SetButtonStateOnManagerState()
        {
            if( this._managerContainer.Started )
            {
                this.uiStartStopButton.ImageIndex = 1;
            }
            else
            {
                this.uiStartStopButton.ImageIndex = 0;
            }
        }

        Alchemi.Manager.Configuration GetConfiguration()
        {
            Alchemi.Manager.Configuration configuration;

            try
            {
                configuration = Configuration.GetConfiguration();
            }
            catch( System.IO.FileNotFoundException fileNotFoundException )
            {
                // if the configuration doesn't exist, let's create it
                configuration = new Configuration();
            }

            return configuration;
        }

        private void uiTimer_Tick( object sender, EventArgs e )
        {
            try
            {
                this.UpdateUi();
            }
            catch( Exception exception )
            {
                this.HandleException( exception );
            }
        }

        private void HandleException( System.Exception exception )
        {
            if( !this._managerContainer.Started )
            {
                this.uiToolStripStatusLabel.Text = "Stopped";
            }
            else
            {
                this.uiToolStripStatusLabel.Text = "Running";
            }

            MessageBox.Show( exception.Message + "\r\n" + exception.StackTrace );
        }

        void StartManager()
        {
            Cursor.Current = Cursors.WaitCursor;
            this.uiToolStripStatusLabel.Text = "Starting...";
            Application.DoEvents();
            this._managerContainer.Start();
            this.uiToolStripStatusLabel.Text = "Running";
            Cursor.Current = Cursors.Default;
        }

        void StopManager()
        {
            Cursor.Current = Cursors.WaitCursor;
            this.uiToolStripStatusLabel.Text = "Stopping...";
            Application.DoEvents();
            this._managerContainer.Stop();
            this.uiToolStripStatusLabel.Text = "Stopped";
            Cursor.Current = Cursors.Default;
        }

        private void uiStartStopButton_Click( object sender, EventArgs e )
        {
            try
            {
                if( this._managerContainer.Started )
                {
                    this.StopManager();
                }
                else
                {
                    this.StartManager();
                }
            }
            catch( Exception ex )
            {
                this.HandleException( ex );
            }
        }

        private void Form1_FormClosing( object sender, FormClosingEventArgs e )
        {
            e.Cancel = true;
            this.Visible = false;
//            this.StopManager();
        }

        private void configureManagerToolStripMenuItem_Click( object sender, EventArgs e )
        {
            if( this._managerContainer.Started )
            {
                MessageBox.Show( "You must stop the Manager to access the configuration settings.", "Warning" );
                return;
            }

            ConfigurationForm form = new ConfigurationForm();
            form.SetConfiguration( this._managerContainer.Config );

            if( form.ShowDialog() == DialogResult.OK )
            {
                this._managerContainer.Config = form.GetConfiguration();

                // and store it in the config file
                this._managerContainer.Config.Slz();
            }
        }

        private void uiNotifyIcon_MouseClick( object sender, MouseEventArgs e )
        {
            if( e.Button == MouseButtons.Left )
            {
                this.Visible = !this.Visible;
                this.BringToFront();
            }
        }

        private void aboutToolStripMenuItem_Click( object sender, EventArgs e )
        {
            new SplashScreen().ShowDialog();
        }

        private void exitToolStripMenuItem_Click( object sender, EventArgs e )
        {
            this.StopManager();

            while( this._managerContainer.Started ) ;

            this.uiNotifyIcon.Dispose();

            System.Environment.Exit( 0 );
        }
    }
}
