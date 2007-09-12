namespace Alchemi.Console.DataForms
{
    partial class AddGroupForm
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
            this.lvMembers = new System.Windows.Forms.ListView();
            this.label2 = new System.Windows.Forms.Label();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lineLabel = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.txtGroupName = new System.Windows.Forms.TextBox();
            this.lbGroupname = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lvMembers
            // 
            this.lvMembers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvMembers.HideSelection = false;
            this.lvMembers.Location = new System.Drawing.Point(16, 91);
            this.lvMembers.MultiSelect = false;
            this.lvMembers.Name = "lvMembers";
            this.lvMembers.Size = new System.Drawing.Size(352, 176);
            this.lvMembers.TabIndex = 33;
            this.lvMembers.UseCompatibleStateImageBehavior = false;
            this.lvMembers.View = System.Windows.Forms.View.List;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 32;
            this.label2.Text = "&Members:";
            // 
            // btnRemove
            // 
            this.btnRemove.Enabled = false;
            this.btnRemove.Location = new System.Drawing.Point(96, 275);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(72, 23);
            this.btnRemove.TabIndex = 31;
            this.btnRemove.Text = "&Remove";
            this.btnRemove.Visible = false;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(16, 275);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(72, 23);
            this.btnAdd.TabIndex = 30;
            this.btnAdd.Tag = "";
            this.btnAdd.Text = "&Add...";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lineLabel
            // 
            this.lineLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lineLabel.Location = new System.Drawing.Point(8, 51);
            this.lineLabel.Name = "lineLabel";
            this.lineLabel.Size = new System.Drawing.Size(362, 2);
            this.lineLabel.TabIndex = 29;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(296, 315);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 28;
            this.btnClose.Text = "&Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Enabled = false;
            this.btnCreate.Location = new System.Drawing.Point(208, 315);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 27;
            this.btnCreate.Text = "&Create";
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // txtGroupName
            // 
            this.txtGroupName.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtGroupName.Location = new System.Drawing.Point(120, 17);
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(248, 20);
            this.txtGroupName.TabIndex = 25;
            this.txtGroupName.TextChanged += new System.EventHandler(this.txtGroupName_TextChanged);
            // 
            // lbGroupname
            // 
            this.lbGroupname.AutoSize = true;
            this.lbGroupname.Location = new System.Drawing.Point(16, 19);
            this.lbGroupname.Name = "lbGroupname";
            this.lbGroupname.Size = new System.Drawing.Size(65, 13);
            this.lbGroupname.TabIndex = 26;
            this.lbGroupname.Text = "&Group name";
            // 
            // AddGroupForm2
            // 
            this.AcceptButton = this.btnCreate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(378, 354);
            this.ControlBox = false;
            this.Controls.Add(this.lvMembers);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lineLabel);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.txtGroupName);
            this.Controls.Add(this.lbGroupname);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddGroupForm2";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Group";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvMembers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        protected System.Windows.Forms.Label lineLabel;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.TextBox txtGroupName;
        private System.Windows.Forms.Label lbGroupname;
    }
}