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
    public partial class GroupProperties : PropertiesForm
    {
        private bool UpdateNeeded = false;
        private ConsoleNode console;
        private GroupStorageView _Group;

        public GroupProperties(ConsoleNode console)
        {
            InitializeComponent();

            this.console = console;
        }


        #region Method - SetData
        public void SetData(GroupStorageView group)
        {
            this._Group = group;
            this.Text = _Group.GroupName + " Properties";
            this.lbName.Text = _Group.GroupName;

            GetMemberData();
            GetPermissionData();

            if (group.IsSystem)
            {
                btnAdd.Enabled = false;
                btnRemove.Enabled = false;

                btnAddPrm.Enabled = false;
                btnRemovePrm.Enabled = false;
            }
        } 
        #endregion


        #region Method - GetMemberData
        private void GetMemberData()
        {
            lvMembers.Items.Clear();
            try
            {
                UserStorageView[] users = console.Manager.Admon_GetGroupUsers(console.Credentials, _Group.GroupId);
                //get the group this user belongs to.

                foreach (UserStorageView user in users)
                {
                    UserItem usrItem = new UserItem(user.Username);
                    usrItem.User = user;
                    usrItem.ImageIndex = 3;
                    lvMembers.Items.Add(usrItem);
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
                    MessageBox.Show("Could not get group membership details. Error: " + ex.Message, "Console Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        } 
        #endregion


        #region Method - GetPermissionData
        private void GetPermissionData()
        {
            lvPrm.Items.Clear();
            try
            {
                //get the group this user belongs to.
                PermissionStorageView[] permissions = console.Manager.Admon_GetGroupPermissions(console.Credentials, _Group);

                foreach (PermissionStorageView permission in permissions)
                {
                    PermissionItem prmItem = new PermissionItem(permission.PermissionName);
                    prmItem.Permission = new PermissionStorageView(permission.PermissionId, permission.PermissionName);
                    prmItem.ImageIndex = 12;
                    lvPrm.Items.Add(prmItem);
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
                    MessageBox.Show("Could not get group permissions. Error: " + ex.Message, "Console Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        } 
        #endregion

        //add group members
        private void btnAdd_Click(object sender, EventArgs e)
        {
            SearchForm srch = new SearchForm(this.console, SearchForm.SearchMode.User);
            srch.ShowDialog(this);

            //get list of users from the search form.
            ListView.SelectedListViewItemCollection items = srch.lvMembers.SelectedItems;

            //for now only one item can be included
            if (items != null && items.Count > 0)
                lvMembers.Items.Clear();

            foreach (ListViewItem li in items)
            {
                UserItem user = new UserItem(li.Text);
                user.ImageIndex = li.ImageIndex;
                user.User = ((UserItem)li).User;
                //this loop should go only once: since only one item can be selected.
                lvMembers.Items.Add(user);
            }

            UpdateNeeded = UpdateNeeded || (lvMembers.Items != null && lvMembers.Items.Count > 0);
            btnApply.Enabled = UpdateNeeded;
        }


        //need to hide this till multiple group-memberships are implemented.
        private void btnRemove_Click(object sender, EventArgs e)
        {
            // TODO:
            //			bool removed = false;
            //			if (lvMembers.SelectedItems!=null)
            //			{
            //				foreach (ListViewItem li in lvMembers.SelectedItems)
            //				{
            //					if (console.Credentials.Username != li.Text)
            //					{
            //						li.Remove();
            //					}
            //					else
            //					{
            //						//for now, we can have only one group-membership, so we should enforce this.
            //						MessageBox.Show("Cannot remove self, from group-membership.", "Remove User", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //					}
            //					removed = true;
            //				}
            //			}
            //			lvMembers.Refresh();
            //
            //			if (removed)
            //			{
            //				UpdateNeeded = true;
            //				btnApply.Enabled = UpdateNeeded;
            //			}
        }

        private void lvMembers_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemove.Enabled = (lvMembers.SelectedItems != null && lvMembers.SelectedItems.Count > 0);
        }

        private void btnAddPrm_Click(object sender, EventArgs e)
        {
            SearchForm srch = new SearchForm(this.console, SearchForm.SearchMode.Permission);
            srch.ShowDialog(this);

            //get list of users from the search form.
            ListView.SelectedListViewItemCollection items = srch.lvMembers.SelectedItems;

            //for now only one item can be included
            if (items != null && items.Count > 0)
                lvPrm.Items.Clear();

            foreach (ListViewItem li in items)
            {
                PermissionItem prm = new PermissionItem(li.Text);
                prm.ImageIndex = li.ImageIndex;
                prm.Permission = ((PermissionItem)li).Permission;

                //this loop should go only once: since only one item can be selected.
                lvPrm.Items.Add(prm);
            }

            UpdateNeeded = UpdateNeeded || (lvPrm.Items != null && lvPrm.Items.Count > 0);
            btnApply.Enabled = UpdateNeeded;
        }

        private void btnRemovePrm_Click(object sender, EventArgs e)
        {
            // TODO remove permission from group
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}