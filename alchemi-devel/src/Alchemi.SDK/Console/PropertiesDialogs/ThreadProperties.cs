using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Alchemi.Core.Manager.Storage;
using Alchemi.Core.Owner;
using Alchemi.Core;

namespace Alchemi.Console.PropertiesDialogs
{
    public partial class ThreadProperties : PropertiesForm
    {
        // Create a logger for use in this class
        private static readonly Logger logger = new Logger();

        private ThreadStorageView _thread;
        private ConsoleNode console;

        public ThreadProperties(ConsoleNode console)
        {
            InitializeComponent();

            this.console = console;
        }


        #region Method - SetData
        public void SetData(ThreadStorageView thread)
        {
            try
            {
                this._thread = thread;

                btnStop.Enabled = (_thread.State != ThreadState.Dead && _thread.State != ThreadState.Finished);

                this.lbName.Text = _thread.ThreadId.ToString();
                txApplication.Text = _thread.ApplicationId;

                txExecutor.Text = _thread.ExecutorId;

                if (_thread.TimeStarted != DateTime.MinValue)
                    txCreated.Text = _thread.TimeStarted.ToString();

                if (_thread.TimeFinished != DateTime.MinValue)
                    txCompleted.Text = _thread.TimeFinished.ToString();

                txState.Text = _thread.StateString;
                txPriority.Text = _thread.Priority.ToString();

                ExecutorStorageView executor = console.Manager.Admon_GetExecutor(console.Credentials, _thread.ExecutorId);
                if (executor != null && executor.HostName != null)
                {
                    txExecutor.Text = executor.HostName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error getting thread properties:" + ex.Message, "Thread properties", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        } 
        #endregion


        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                //try to stop the application.
                ThreadIdentifier ti = new ThreadIdentifier(_thread.ApplicationId, _thread.ThreadId);
                console.Manager.Owner_AbortThread(console.Credentials, ti);
                MessageBox.Show("Thread Aborted.", "Applcation Properties", MessageBoxButtons.OK, MessageBoxIcon.Information);
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