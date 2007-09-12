using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Alchemi.Core.Manager.Storage;
using Alchemi.Core.Utility;
using Alchemi.Core;

namespace Alchemi.Console.DataForms
{
    public partial class AddUserForm : Form
    {
        private ConsoleNode _ConsoleNode;
        private GroupStorageView[] _AllGroups;


        public AddUserForm(ConsoleNode consoleNode)
        {
            InitializeComponent();

            _ConsoleNode = consoleNode;

            SecurityCredentials sc = _ConsoleNode.Credentials;
            _AllGroups = _ConsoleNode.Manager.Admon_GetGroups(sc);
        }

        private void txUsername_TextChanged(object sender, EventArgs e)
        {
            EnableCreateButton();
        }

        private void cboGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableCreateButton();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (UserIsValid())
            {
                AddUser();
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        #region Method - EnableCreateButton
        private void EnableCreateButton()
        {
            btnOK.Enabled = ((txUsername.Text != "") && (cboGroup.SelectedIndex != -1));
        }  
        #endregion       


        #region Method - SetData
        private void SetData()
        {
            cboGroup.Items.Clear();
            foreach (GroupStorageView group in _AllGroups)
            {
                cboGroup.Items.Add(group.GroupName);
            }
        } 
        #endregion


        #region Method - ValidateUser
        private bool UserIsValid()
        {
            bool valid = true;
            string reason = "";

            if (txUsername.Text == null || txUsername.Text.Trim() == "" || Utils.IsSqlSafe(txUsername.Text.Trim()) == false)
            {
                reason = "Username is empty or contains invalid characters.";
                valid = false;
            }

            if (cboGroup.SelectedIndex == -1)
            {
                reason = reason + "\nNo Group selected.";
                valid = false;
            }

            if (txPwd.Text == null || Utils.IsSqlSafe(txPwd.Text.Trim()) == false)
            {
                reason = reason + "\nPassword is empty or contains invalid characters.";
                valid = false;
            }

            if (txPwd.Text != txPwd2.Text)
            {
                reason = reason + "\nPassword and confirm password do not match.";
                valid = false;
            }

            if (valid && txPwd.Text == "")
            {
                DialogResult result =
                    MessageBox.Show("Are you sure you want to set an empty password?", "Add User", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    valid = false;
                    return valid; //no reason, user will re-enter password.
                }
            }

            if (!valid)
            {
                MessageBox.Show("Cannot add user:\n" + reason, "Add User", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return valid;
        } 
        #endregion


        #region Method - GetUsers
        private UserStorageView[] GetUsers()
        {
            UserStorageView[] users = new UserStorageView[1];
            string username = Utils.MakeSqlSafe(txUsername.Text);
            string password = Utils.MakeSqlSafe(txPwd.Text);
            int groupId = -1;

            foreach (GroupStorageView group in _AllGroups)
            {
                if (group.GroupName == cboGroup.SelectedItem.ToString())
                {
                    groupId = group.GroupId;
                    break;
                }
            }
            users[0] = new UserStorageView(username, password, groupId);

            return users;
        } 
        #endregion


        #region Method - AddUser
        private void AddUser()
        {
            try
            {
                UserStorageView[] users = GetUsers();
                _ConsoleNode.Manager.Admon_AddUsers(_ConsoleNode.Credentials, users);
                AddedUser = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding user:" + ex.Message, "Add User", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } 
        #endregion


        #region Property - AddedUser
        private bool _AddedUser = false;
        public bool AddedUser
        {
            get { return _AddedUser; }
            private set { _AddedUser = value; }
        }
        #endregion


    }
}