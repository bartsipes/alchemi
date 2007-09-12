namespace Alchemi.Console.PropertiesDialogs
{
    partial class GroupProperties
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupProperties));
            this.lvMembers = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.tabPermissions = new System.Windows.Forms.TabPage();
            this.lvPrm = new System.Windows.Forms.ListView();
            this.label2 = new System.Windows.Forms.Label();
            this.btnRemovePrm = new System.Windows.Forms.Button();
            this.btnAddPrm = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).BeginInit();
            this.tabs.SuspendLayout();
            this.tabPermissions.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // imgListSmall
            // 
            this.imgListSmall.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListSmall.ImageStream")));
            this.imgListSmall.Images.SetKeyName(0, "");
            this.imgListSmall.Images.SetKeyName(1, "");
            this.imgListSmall.Images.SetKeyName(2, "");
            this.imgListSmall.Images.SetKeyName(3, "");
            this.imgListSmall.Images.SetKeyName(4, "");
            this.imgListSmall.Images.SetKeyName(5, "");
            this.imgListSmall.Images.SetKeyName(6, "");
            this.imgListSmall.Images.SetKeyName(7, "");
            this.imgListSmall.Images.SetKeyName(8, "");
            this.imgListSmall.Images.SetKeyName(9, "");
            this.imgListSmall.Images.SetKeyName(10, "");
            this.imgListSmall.Images.SetKeyName(11, "");
            this.imgListSmall.Images.SetKeyName(12, "");
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.lvMembers);
            this.tabGeneral.Controls.Add(this.label1);
            this.tabGeneral.Controls.Add(this.btnRemove);
            this.tabGeneral.Controls.Add(this.btnAdd);
            this.tabGeneral.Controls.SetChildIndex(this.iconBox, 0);
            this.tabGeneral.Controls.SetChildIndex(this.lbName, 0);
            this.tabGeneral.Controls.SetChildIndex(this.lineLabel, 0);
            this.tabGeneral.Controls.SetChildIndex(this.btnAdd, 0);
            this.tabGeneral.Controls.SetChildIndex(this.btnRemove, 0);
            this.tabGeneral.Controls.SetChildIndex(this.label1, 0);
            this.tabGeneral.Controls.SetChildIndex(this.lvMembers, 0);
            // 
            // iconBox
            // 
            this.iconBox.Image = ((System.Drawing.Image)(resources.GetObject("iconBox.Image")));
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tabPermissions);
            this.tabs.Controls.SetChildIndex(this.tabPermissions, 0);
            this.tabs.Controls.SetChildIndex(this.tabGeneral, 0);
            // 
            // lvMembers
            // 
            this.lvMembers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvMembers.HideSelection = false;
            this.lvMembers.Location = new System.Drawing.Point(16, 96);
            this.lvMembers.MultiSelect = false;
            this.lvMembers.Name = "lvMembers";
            this.lvMembers.Size = new System.Drawing.Size(296, 176);
            this.lvMembers.SmallImageList = this.imgListSmall;
            this.lvMembers.TabIndex = 24;
            this.lvMembers.UseCompatibleStateImageBehavior = false;
            this.lvMembers.View = System.Windows.Forms.View.List;
            this.lvMembers.SelectedIndexChanged += new System.EventHandler(this.lvMembers_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "&Members:";
            // 
            // btnRemove
            // 
            this.btnRemove.Enabled = false;
            this.btnRemove.Location = new System.Drawing.Point(96, 280);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(72, 23);
            this.btnRemove.TabIndex = 22;
            this.btnRemove.Text = "&Remove";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(16, 280);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(72, 23);
            this.btnAdd.TabIndex = 21;
            this.btnAdd.Tag = "";
            this.btnAdd.Text = "&Add...";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // tabPermissions
            // 
            this.tabPermissions.Controls.Add(this.lvPrm);
            this.tabPermissions.Controls.Add(this.label2);
            this.tabPermissions.Controls.Add(this.btnRemovePrm);
            this.tabPermissions.Controls.Add(this.btnAddPrm);
            this.tabPermissions.Location = new System.Drawing.Point(4, 22);
            this.tabPermissions.Name = "tabPermissions";
            this.tabPermissions.Size = new System.Drawing.Size(328, 318);
            this.tabPermissions.TabIndex = 2;
            this.tabPermissions.Text = "Permissions";
            // 
            // lvPrm
            // 
            this.lvPrm.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvPrm.HideSelection = false;
            this.lvPrm.Location = new System.Drawing.Point(16, 40);
            this.lvPrm.MultiSelect = false;
            this.lvPrm.Name = "lvPrm";
            this.lvPrm.Size = new System.Drawing.Size(296, 224);
            this.lvPrm.SmallImageList = this.imgListSmall;
            this.lvPrm.TabIndex = 24;
            this.lvPrm.UseCompatibleStateImageBehavior = false;
            this.lvPrm.View = System.Windows.Forms.View.List;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Permissions assigned to this group:";
            // 
            // btnRemovePrm
            // 
            this.btnRemovePrm.Enabled = false;
            this.btnRemovePrm.Location = new System.Drawing.Point(96, 280);
            this.btnRemovePrm.Name = "btnRemovePrm";
            this.btnRemovePrm.Size = new System.Drawing.Size(72, 23);
            this.btnRemovePrm.TabIndex = 22;
            this.btnRemovePrm.Text = "&Remove";
            this.btnRemovePrm.Click += new System.EventHandler(this.btnRemovePrm_Click);
            // 
            // btnAddPrm
            // 
            this.btnAddPrm.Location = new System.Drawing.Point(16, 280);
            this.btnAddPrm.Name = "btnAddPrm";
            this.btnAddPrm.Size = new System.Drawing.Size(72, 23);
            this.btnAddPrm.TabIndex = 21;
            this.btnAddPrm.Tag = "";
            this.btnAddPrm.Text = "&Add...";
            this.btnAddPrm.Click += new System.EventHandler(this.btnAddPrm_Click);
            // 
            // GroupProperties2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 389);
            this.Name = "GroupProperties2";
            this.Text = "Group Properties";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).EndInit();
            this.tabs.ResumeLayout(false);
            this.tabPermissions.ResumeLayout(false);
            this.tabPermissions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvMembers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TabPage tabPermissions;
        private System.Windows.Forms.ListView lvPrm;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRemovePrm;
        private System.Windows.Forms.Button btnAddPrm;
    }
}