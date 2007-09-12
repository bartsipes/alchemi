using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Alchemi.Core.Utility;

namespace Alchemi.Console.DataForms
{
    public partial class PasswordForm : Form
    {
        public PasswordForm()
        {
            InitializeComponent();
        }


        #region Property - Password
        private string _Password = null;
        public string Password
        {
            get { return _Password; }
            private set { _Password = value; }
        } 
        #endregion


        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txPwd.Text != txPwd2.Text)
            {
                MessageBox.Show("The two passwords entered are not the same. Please confirm the password.", "Change password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (Utils.IsSqlSafe(txPwd.Text) == false)
            {
                MessageBox.Show("The password entered has invalid characters ' or \" .", "Change password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.Password = Utils.MakeSqlSafe(txPwd.Text);
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Password = null;
            this.Close();
        }
    }
}