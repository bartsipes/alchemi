using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Alchemi.Core.Manager.Storage;

namespace Alchemi.Console.PropertiesDialogs
{
    public partial class PermissionProperties : PropertiesForm
    {
        private PermissionStorageView _prm;

        public PermissionProperties()
        {
            InitializeComponent();
        }


        #region Method - SetData
        public void SetData(PermissionStorageView permission)
        {
            this._prm = permission;

            this.Text = this._prm.PermissionName + " Properties";
            this.lbName.Text = _prm.PermissionName;
        } 
        #endregion


        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}