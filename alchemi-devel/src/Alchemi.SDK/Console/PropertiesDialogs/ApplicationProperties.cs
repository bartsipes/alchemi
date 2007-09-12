using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Alchemi.Core.Manager.Storage;
using Alchemi.Core;
using Alchemi.Core.Owner;

namespace Alchemi.Console.PropertiesDialogs
{
    public partial class ApplicationProperties : PropertiesForm
    {
        private ApplicationStorageView _app;
        private ConsoleNode console;
        // Create a logger for use in this class
        private static readonly Logger logger = new Logger();

        public ApplicationProperties(ConsoleNode console)
        {
            InitializeComponent();

            this.console = console;
        }


        #region Method - SetData
        public void SetData(ApplicationStorageView application)
        {
            this._app = application;

            this.lbName.Text = _app.ApplicationName;

            txId.Text = _app.ApplicationId;
            txUsername.Text = _app.Username;

            if (_app.TimeCreatedSet)
            {
                txCreated.Text = _app.TimeCreated.ToString();
            }

            if (_app.TimeCompletedSet)
            {
                txCompleted.Text = _app.TimeCompleted.ToString();
            }

            txState.Text = _app.StateString;
            chkPrimary.Checked = _app.Primary;
            txNumThreads.Text = _app.TotalThreads.ToString();

            btnStop.Enabled = (_app.State != ApplicationState.Stopped);
        } 
        #endregion


        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // TODO
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            // TODO
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                //try to stop the application.
                console.Manager.Owner_StopApplication(console.Credentials, this._app.ApplicationId);
                MessageBox.Show("Application Stopped.", "Applcation Properties", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                if (ex is AuthorizationException)
                {
                    MessageBox.Show("Access denied. You do not have adequate permissions for this operation.", "Authorization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    logger.Error("Could not stop application. Error: " + ex.Message, ex);
                    MessageBox.Show("Could not stop application. Error: " + ex.Message, "Console Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}