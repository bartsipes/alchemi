using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Alchemi.Core.Manager.Storage;
using Alchemi.Core;
using Alchemi.Console.DataForms;

namespace Alchemi.Console.PropertiesDialogs
{
    public partial class UserProperties : PropertiesForm
    {
        private ConsoleNode console;
        private UserStorageView _User = null;
        private bool UpdateNeeded = false;

        public UserProperties(ConsoleNode console)
        {
            InitializeComponent();

            this.console = console;
        }


        #region Method - SetData
        public void SetData(UserStorageView User)
        {
            this._User = User;
            this.Text = User.Username + " Properties";
            this.lbName.Text = User.Username;

            GetGroupMembershipData();

            if (User.IsSystem)
            {
                //we cant change group membership
                btnAdd.Enabled = false;
                btnRemove.Enabled = false;
            }
        } 
        #endregion


        #region Method - GetGroupMembershipData
        private void GetGroupMembershipData()
        {
            try
            {
                lvGrp.Items.Clear();
                //get the group this user belongs to.
                GroupStorageView groupStorageView = console.Manager.Admon_GetGroup(console.Credentials, _User.GroupId);

                if (groupStorageView != null)
                {
                    GroupItem grpItem = new GroupItem(groupStorageView.GroupName);
                    grpItem.GroupView = groupStorageView;
                    grpItem.ImageIndex = 2;
                    lvGrp.Items.Add(grpItem);
                }
            }
            catch (Exception ex)
            {
                if (ex is AuthorizationException)
                {
                    MessageBox.Show("Access denied. You do not have adequate permissions for this operation.", "Authorization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Could not get user-group membership details. Error: " + ex.Message, "Console Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        } 
        #endregion


        #region Method - UpdateUser
        private void UpdateUser()
        {
            int groupId = -1;
            try
            {
                //get the groupId from the listview.
                if (lvGrp.Items != null && lvGrp.Items.Count > 0)
                {
                    if (lvGrp.Items[0] is GroupItem)
                    {
                        GroupItem grpItem = (GroupItem)lvGrp.Items[0];
                        groupId = grpItem.GroupView.GroupId; //set the group Id. For now, a user can be part of one group only.
                    }
                }

                if ((groupId != _User.GroupId) && (groupId != -1))
                {
                    UserStorageView[] users = new UserStorageView[1];
                    users[0] = _User;
                    console.Manager.Admon_UpdateUsers(console.Credentials, users);
                }
                else
                {
                    if (groupId == -1)
                    {
                        //dont update the user.
                        MessageBox.Show("Cannot update user: The User is not assigned to any group!", "Edit User", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating user:" + ex.Message, "Update User", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } 
        #endregion


        private void btnChgPwd_Click(object sender, EventArgs e)
        {
            bool changed = false;
            try
            {
                PasswordForm pwdform = new PasswordForm();
                pwdform.ShowDialog(this);
                //try to change the password for this user.
                if (pwdform.Password != null)
                {
                    UserStorageView[] users = new UserStorageView[1];
                    users[0] = _User;
                    _User.Password = pwdform.Password;
                    console.Manager.Admon_UpdateUsers(console.Credentials, users);

                    changed = true;

                    //update the console credentials if needed
                    if (console.Credentials.Username == _User.Username)
                    {
                        console.Credentials.Password = pwdform.Password;
                    }
                }
            }
            catch (Exception ex)
            {
                changed = false;
                MessageBox.Show("Error changing password:" + ex.Message, "Change Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (changed)
                {
                    MessageBox.Show("Password changed successfully.", "Change Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            UpdateUser();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (UpdateNeeded)
                UpdateUser();
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                SearchForm srch = new SearchForm(this.console, SearchForm.SearchMode.Group);
                srch.ShowDialog(this);

                //get list of groups from the search form.
                ListView.SelectedListViewItemCollection items = srch.lvMembers.SelectedItems;

                //first update the database, then get it from there.
                //for now only one item can be included
                if (items != null && items.Count > 0)
                {
                    _User.GroupId = ((GroupItem)items[0]).GroupView.GroupId;

                    UserStorageView[] users = new UserStorageView[1];
                    users[0] = this._User;

                    console.Manager.Admon_UpdateUsers(console.Credentials, users);
                }

                GetGroupMembershipData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error changing membership:" + ex.Message, "User Properties", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //need to hide this till multiple group-memberships are present.
        private void btnRemove_Click(object sender, EventArgs e)
        {
            //			bool removed = false;
            //			if (lvGrp.SelectedItems!=null)
            //			{
            //				foreach (ListViewItem li in lvGrp.SelectedItems)
            //				{
            //					li.Remove();
            //					removed = true;
            //				}
            //			}
            //			lvGrp.Refresh();
            //
            //			if (removed)
            //			{
            //				UpdateNeeded = true;
            //				btnApply.Enabled = UpdateNeeded;
            //			}
        }

        private void lvGrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemove.Enabled = (lvGrp.SelectedItems != null && lvGrp.SelectedItems.Count > 0);
        }



    }
}