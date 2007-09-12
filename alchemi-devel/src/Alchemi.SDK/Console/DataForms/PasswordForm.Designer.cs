namespace Alchemi.Console.DataForms
{
    partial class PasswordForm
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.txPwd2 = new System.Windows.Forms.TextBox();
            this.lbPwd2 = new System.Windows.Forms.Label();
            this.txPwd = new System.Windows.Forms.TextBox();
            this.lbPwd1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(136, 72);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(48, 72);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txPwd2
            // 
            this.txPwd2.Location = new System.Drawing.Point(120, 40);
            this.txPwd2.Name = "txPwd2";
            this.txPwd2.PasswordChar = '*';
            this.txPwd2.Size = new System.Drawing.Size(120, 20);
            this.txPwd2.TabIndex = 14;
            // 
            // lbPwd2
            // 
            this.lbPwd2.AutoSize = true;
            this.lbPwd2.Location = new System.Drawing.Point(16, 42);
            this.lbPwd2.Name = "lbPwd2";
            this.lbPwd2.Size = new System.Drawing.Size(90, 13);
            this.lbPwd2.TabIndex = 16;
            this.lbPwd2.Text = "Confirm password";
            // 
            // txPwd
            // 
            this.txPwd.Location = new System.Drawing.Point(120, 16);
            this.txPwd.Name = "txPwd";
            this.txPwd.PasswordChar = '*';
            this.txPwd.Size = new System.Drawing.Size(120, 20);
            this.txPwd.TabIndex = 12;
            // 
            // lbPwd1
            // 
            this.lbPwd1.AutoSize = true;
            this.lbPwd1.Location = new System.Drawing.Point(16, 16);
            this.lbPwd1.Name = "lbPwd1";
            this.lbPwd1.Size = new System.Drawing.Size(53, 13);
            this.lbPwd1.TabIndex = 13;
            this.lbPwd1.Text = "Password";
            // 
            // PasswordForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(258, 103);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txPwd2);
            this.Controls.Add(this.lbPwd2);
            this.Controls.Add(this.txPwd);
            this.Controls.Add(this.lbPwd1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PasswordForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change Password";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txPwd2;
        private System.Windows.Forms.Label lbPwd2;
        private System.Windows.Forms.TextBox txPwd;
        private System.Windows.Forms.Label lbPwd1;
    }
}