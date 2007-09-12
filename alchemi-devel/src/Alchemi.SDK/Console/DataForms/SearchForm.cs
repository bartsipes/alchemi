using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Alchemi.Core.Manager.Storage;

namespace Alchemi.Console.DataForms
{
    public partial class SearchForm : Form
    {
        public enum SearchMode
        {
            User,
            Group,
            Permission
        }


        public SearchForm(ConsoleNode console, SearchMode mode)
        {
            InitializeComponent();

            //fill the data
            SetData(console, mode);
        }


        #region Method - SetData
        private void SetData(ConsoleNode console, SearchMode mode)
        {
            try
            {
                lvMembers.Items.Clear();
                if (mode == SearchMode.User)
                {
                    lbMembers.Text = "&Users:";
                    this.Text = "Users";
                    UserStorageView[] users = console.Manager.Admon_GetUserList(console.Credentials);
                    foreach (UserStorageView user in users)
                    {
                        UserItem ui = new UserItem(user.Username);
                        ui.ImageIndex = 3;
                        ui.User = user;
                        lvMembers.Items.Add(ui);
                    }
                }
                else if (mode == SearchMode.Group)
                {
                    lbMembers.Text = "&Groups:";
                    this.Text = "Groups";
                    GroupStorageView[] groups = console.Manager.Admon_GetGroups(console.Credentials);
                    foreach (GroupStorageView group in groups)
                    {
                        GroupItem gi = new GroupItem(group.GroupName);
                        gi.ImageIndex = 2;
                        gi.GroupView = group;
                        lvMembers.Items.Add(gi);
                    }
                }
                else if (mode == SearchMode.Permission)
                {
                    lbMembers.Text = "&Permissions:";
                    this.Text = "Permissions";
                    PermissionStorageView[] permissions = console.Manager.Admon_GetPermissions(console.Credentials);
                    foreach (PermissionStorageView permission in permissions)
                    {
                        PermissionItem prm = new PermissionItem(permission.PermissionName);
                        prm.ImageIndex = 12;
                        prm.Permission = new PermissionStorageView(permission.PermissionId, permission.PermissionName);
                        lvMembers.Items.Add(prm);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error filling search list:" + ex.Message, "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion


        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}