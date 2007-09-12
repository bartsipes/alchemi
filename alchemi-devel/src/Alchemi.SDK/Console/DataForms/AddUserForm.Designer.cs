namespace Alchemi.Console.DataForms
{
    partial class AddUserForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.lineLabel = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.cboGroup = new System.Windows.Forms.ComboBox();
            this.lbGroup = new System.Windows.Forms.Label();
            this.txPwd2 = new System.Windows.Forms.TextBox();
            this.lbPwd2 = new System.Windows.Forms.Label();
            this.txPwd = new System.Windows.Forms.TextBox();
            this.lbPwd1 = new System.Windows.Forms.Label();
            this.txUsername = new System.Windows.Forms.TextBox();
            this.lbUsername = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(8, 296);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(362, 2);
            this.label1.TabIndex = 20;
            // 
            // lineLabel
            // 
            this.lineLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lineLabel.Location = new System.Drawing.Point(8, 96);
            this.lineLabel.Name = "lineLabel";
            this.lineLabel.Size = new System.Drawing.Size(362, 2);
            this.lineLabel.TabIndex = 19;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(296, 320);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "&Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(208, 320);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 16;
            this.btnOK.Text = "&Create";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cboGroup
            // 
            this.cboGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGroup.Location = new System.Drawing.Point(120, 53);
            this.cboGroup.Name = "cboGroup";
            this.cboGroup.Size = new System.Drawing.Size(248, 21);
            this.cboGroup.Sorted = true;
            this.cboGroup.TabIndex = 11;
            this.cboGroup.SelectedIndexChanged += new System.EventHandler(this.cboGroup_SelectedIndexChanged);
            // 
            // lbGroup
            // 
            this.lbGroup.AutoSize = true;
            this.lbGroup.Location = new System.Drawing.Point(16, 56);
            this.lbGroup.Name = "lbGroup";
            this.lbGroup.Size = new System.Drawing.Size(36, 13);
            this.lbGroup.TabIndex = 18;
            this.lbGroup.Text = "Group";
            // 
            // txPwd2
            // 
            this.txPwd2.Location = new System.Drawing.Point(144, 144);
            this.txPwd2.Name = "txPwd2";
            this.txPwd2.PasswordChar = '*';
            this.txPwd2.Size = new System.Drawing.Size(224, 20);
            this.txPwd2.TabIndex = 14;
            // 
            // lbPwd2
            // 
            this.lbPwd2.AutoSize = true;
            this.lbPwd2.Location = new System.Drawing.Point(16, 144);
            this.lbPwd2.Name = "lbPwd2";
            this.lbPwd2.Size = new System.Drawing.Size(90, 13);
            this.lbPwd2.TabIndex = 15;
            this.lbPwd2.Text = "Confirm password";
            // 
            // txPwd
            // 
            this.txPwd.Location = new System.Drawing.Point(144, 112);
            this.txPwd.Name = "txPwd";
            this.txPwd.PasswordChar = '*';
            this.txPwd.Size = new System.Drawing.Size(224, 20);
            this.txPwd.TabIndex = 12;
            // 
            // lbPwd1
            // 
            this.lbPwd1.AutoSize = true;
            this.lbPwd1.Location = new System.Drawing.Point(16, 112);
            this.lbPwd1.Name = "lbPwd1";
            this.lbPwd1.Size = new System.Drawing.Size(53, 13);
            this.lbPwd1.TabIndex = 13;
            this.lbPwd1.Text = "Password";
            // 
            // txUsername
            // 
            this.txUsername.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txUsername.Location = new System.Drawing.Point(120, 22);
            this.txUsername.Name = "txUsername";
            this.txUsername.Size = new System.Drawing.Size(248, 20);
            this.txUsername.TabIndex = 9;
            this.txUsername.TextChanged += new System.EventHandler(this.txUsername_TextChanged);
            // 
            // lbUsername
            // 
            this.lbUsername.AutoSize = true;
            this.lbUsername.Location = new System.Drawing.Point(16, 24);
            this.lbUsername.Name = "lbUsername";
            this.lbUsername.Size = new System.Drawing.Size(58, 13);
            this.lbUsername.TabIndex = 10;
            this.lbUsername.Text = "&User name";
            // 
            // AddUserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 354);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lineLabel);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cboGroup);
            this.Controls.Add(this.lbGroup);
            this.Controls.Add(this.txPwd2);
            this.Controls.Add(this.lbPwd2);
            this.Controls.Add(this.txPwd);
            this.Controls.Add(this.lbPwd1);
            this.Controls.Add(this.txUsername);
            this.Controls.Add(this.lbUsername);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddUserForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New User";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Label label1;
        protected System.Windows.Forms.Label lineLabel;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cboGroup;
        private System.Windows.Forms.Label lbGroup;
        private System.Windows.Forms.TextBox txPwd2;
        private System.Windows.Forms.Label lbPwd2;
        private System.Windows.Forms.TextBox txPwd;
        private System.Windows.Forms.Label lbPwd1;
        private System.Windows.Forms.TextBox txUsername;
        private System.Windows.Forms.Label lbUsername;

    }
}