namespace Alchemi.Console.PropertiesDialogs
{
    partial class UserProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserProperties));
            this.btnChgPwd = new System.Windows.Forms.Button();
            this.tabMemberOf = new System.Windows.Forms.TabPage();
            this.lvGrp = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).BeginInit();
            this.tabs.SuspendLayout();
            this.tabMemberOf.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnApply
            // 
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
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
            this.tabGeneral.Controls.Add(this.btnChgPwd);
            this.tabGeneral.Controls.SetChildIndex(this.iconBox, 0);
            this.tabGeneral.Controls.SetChildIndex(this.lbName, 0);
            this.tabGeneral.Controls.SetChildIndex(this.lineLabel, 0);
            this.tabGeneral.Controls.SetChildIndex(this.btnChgPwd, 0);
            // 
            // iconBox
            // 
            this.iconBox.Image = ((System.Drawing.Image)(resources.GetObject("iconBox.Image")));
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tabMemberOf);
            this.tabs.Controls.SetChildIndex(this.tabMemberOf, 0);
            this.tabs.Controls.SetChildIndex(this.tabGeneral, 0);
            // 
            // btnChgPwd
            // 
            this.btnChgPwd.Location = new System.Drawing.Point(200, 72);
            this.btnChgPwd.Name = "btnChgPwd";
            this.btnChgPwd.Size = new System.Drawing.Size(120, 23);
            this.btnChgPwd.TabIndex = 5;
            this.btnChgPwd.Text = "Change Password...";
            this.btnChgPwd.Click += new System.EventHandler(this.btnChgPwd_Click);
            // 
            // tabMemberOf
            // 
            this.tabMemberOf.Controls.Add(this.lvGrp);
            this.tabMemberOf.Controls.Add(this.label1);
            this.tabMemberOf.Controls.Add(this.btnRemove);
            this.tabMemberOf.Controls.Add(this.btnAdd);
            this.tabMemberOf.Location = new System.Drawing.Point(4, 22);
            this.tabMemberOf.Name = "tabMemberOf";
            this.tabMemberOf.Size = new System.Drawing.Size(328, 318);
            this.tabMemberOf.TabIndex = 2;
            this.tabMemberOf.Text = "Member Of";
            // 
            // lvGrp
            // 
            this.lvGrp.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvGrp.HideSelection = false;
            this.lvGrp.Location = new System.Drawing.Point(16, 40);
            this.lvGrp.MultiSelect = false;
            this.lvGrp.Name = "lvGrp";
            this.lvGrp.Size = new System.Drawing.Size(296, 232);
            this.lvGrp.SmallImageList = this.imgListSmall;
            this.lvGrp.TabIndex = 16;
            this.lvGrp.UseCompatibleStateImageBehavior = false;
            this.lvGrp.View = System.Windows.Forms.View.List;
            this.lvGrp.SelectedIndexChanged += new System.EventHandler(this.lvGrp_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "&Member of:";
            // 
            // btnRemove
            // 
            this.btnRemove.Enabled = false;
            this.btnRemove.Location = new System.Drawing.Point(96, 282);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(72, 23);
            this.btnRemove.TabIndex = 14;
            this.btnRemove.Text = "&Remove";
            this.btnRemove.Visible = false;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(16, 282);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(72, 23);
            this.btnAdd.TabIndex = 13;
            this.btnAdd.Text = "&Change...";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // UserProperties2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 389);
            this.Name = "UserProperties2";
            this.Text = "User Properties";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).EndInit();
            this.tabs.ResumeLayout(false);
            this.tabMemberOf.ResumeLayout(false);
            this.tabMemberOf.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnChgPwd;
        private System.Windows.Forms.TabPage tabMemberOf;
        private System.Windows.Forms.ListView lvGrp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
    }
}