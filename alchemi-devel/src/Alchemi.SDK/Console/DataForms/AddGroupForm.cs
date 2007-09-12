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
    public partial class AddGroupForm : Form
    {
        private ConsoleNode _ConsoleNode;

        public AddGroupForm(ConsoleNode consoleNode)
        {
            InitializeComponent();

            _ConsoleNode = consoleNode;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // TODO:
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            // TODO:
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (GroupIsValid())
            {
                AddGroup();
                this.Close();
            }            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtGroupName_TextChanged(object sender, EventArgs e)
        {
            btnCreate.Enabled = !String.IsNullOrEmpty(txtGroupName.Text);
        }


        #region Method - GroupIsValid
        private bool GroupIsValid()
        {
            bool valid = true;
            string reason = "";

            if (txtGroupName.Text == null ||
                txtGroupName.Text.Trim() == "" ||
                Utils.IsSqlSafe(txtGroupName.Text.Trim()) == false)
            {
                reason = "Groupname is empty or contains invalid characters.";
                valid = false;
            }

            if (!valid)
            {
                MessageBox.Show("Cannot add group:\n" + reason, "Add Group", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return valid;
        } 
        #endregion


        #region Method - AddGroup
        private void AddGroup()
        {
            try
            {
                // TODO:
                //console.Manager.
                AddedGroup = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding Group:" + ex.Message, "Add Group", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } 
        #endregion


        #region Property - AddedGroup
        private bool _AddedGroup = false;
        public bool AddedGroup
        {
            get { return _AddedGroup; }
            private set { _AddedGroup = value; }
        } 
        #endregion

    }
}