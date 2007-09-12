namespace Alchemi.Console.PropertiesDialogs
{
    partial class ApplicationProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApplicationProperties));
            this.btnStop = new System.Windows.Forms.Button();
            this.chkPrimary = new System.Windows.Forms.CheckBox();
            this.txNumThreads = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txState = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txCompleted = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txCreated = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txUsername = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txId = new System.Windows.Forms.TextBox();
            this.lbId = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).BeginInit();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
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
            this.tabGeneral.Controls.Add(this.btnStop);
            this.tabGeneral.Controls.Add(this.chkPrimary);
            this.tabGeneral.Controls.Add(this.txNumThreads);
            this.tabGeneral.Controls.Add(this.label6);
            this.tabGeneral.Controls.Add(this.txState);
            this.tabGeneral.Controls.Add(this.label5);
            this.tabGeneral.Controls.Add(this.txCompleted);
            this.tabGeneral.Controls.Add(this.label4);
            this.tabGeneral.Controls.Add(this.txCreated);
            this.tabGeneral.Controls.Add(this.label3);
            this.tabGeneral.Controls.Add(this.txUsername);
            this.tabGeneral.Controls.Add(this.label2);
            this.tabGeneral.Controls.Add(this.txId);
            this.tabGeneral.Controls.Add(this.lbId);
            this.tabGeneral.Controls.SetChildIndex(this.iconBox, 0);
            this.tabGeneral.Controls.SetChildIndex(this.lbName, 0);
            this.tabGeneral.Controls.SetChildIndex(this.lineLabel, 0);
            this.tabGeneral.Controls.SetChildIndex(this.lbId, 0);
            this.tabGeneral.Controls.SetChildIndex(this.txId, 0);
            this.tabGeneral.Controls.SetChildIndex(this.label2, 0);
            this.tabGeneral.Controls.SetChildIndex(this.txUsername, 0);
            this.tabGeneral.Controls.SetChildIndex(this.label3, 0);
            this.tabGeneral.Controls.SetChildIndex(this.txCreated, 0);
            this.tabGeneral.Controls.SetChildIndex(this.label4, 0);
            this.tabGeneral.Controls.SetChildIndex(this.txCompleted, 0);
            this.tabGeneral.Controls.SetChildIndex(this.label5, 0);
            this.tabGeneral.Controls.SetChildIndex(this.txState, 0);
            this.tabGeneral.Controls.SetChildIndex(this.label6, 0);
            this.tabGeneral.Controls.SetChildIndex(this.txNumThreads, 0);
            this.tabGeneral.Controls.SetChildIndex(this.chkPrimary, 0);
            this.tabGeneral.Controls.SetChildIndex(this.btnStop, 0);
            // 
            // iconBox
            // 
            this.iconBox.Image = ((System.Drawing.Image)(resources.GetObject("iconBox.Image")));
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(248, 288);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 33;
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // chkPrimary
            // 
            this.chkPrimary.AutoCheck = false;
            this.chkPrimary.Location = new System.Drawing.Point(16, 256);
            this.chkPrimary.Name = "chkPrimary";
            this.chkPrimary.Size = new System.Drawing.Size(104, 24);
            this.chkPrimary.TabIndex = 32;
            this.chkPrimary.Text = "Primary";
            // 
            // txNumThreads
            // 
            this.txNumThreads.BackColor = System.Drawing.Color.White;
            this.txNumThreads.Location = new System.Drawing.Point(88, 232);
            this.txNumThreads.Name = "txNumThreads";
            this.txNumThreads.ReadOnly = true;
            this.txNumThreads.Size = new System.Drawing.Size(120, 20);
            this.txNumThreads.TabIndex = 31;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 232);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 30;
            this.label6.Text = "# of threads:";
            // 
            // txState
            // 
            this.txState.BackColor = System.Drawing.Color.White;
            this.txState.Location = new System.Drawing.Point(88, 200);
            this.txState.Name = "txState";
            this.txState.ReadOnly = true;
            this.txState.Size = new System.Drawing.Size(120, 20);
            this.txState.TabIndex = 29;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 200);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "State:";
            // 
            // txCompleted
            // 
            this.txCompleted.BackColor = System.Drawing.Color.White;
            this.txCompleted.Location = new System.Drawing.Point(88, 168);
            this.txCompleted.Name = "txCompleted";
            this.txCompleted.ReadOnly = true;
            this.txCompleted.Size = new System.Drawing.Size(224, 20);
            this.txCompleted.TabIndex = 27;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 168);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "Completed:";
            // 
            // txCreated
            // 
            this.txCreated.BackColor = System.Drawing.Color.White;
            this.txCreated.Location = new System.Drawing.Point(88, 136);
            this.txCreated.Name = "txCreated";
            this.txCreated.ReadOnly = true;
            this.txCreated.Size = new System.Drawing.Size(224, 20);
            this.txCreated.TabIndex = 25;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Created:";
            // 
            // txUsername
            // 
            this.txUsername.BackColor = System.Drawing.Color.White;
            this.txUsername.Location = new System.Drawing.Point(88, 104);
            this.txUsername.Name = "txUsername";
            this.txUsername.ReadOnly = true;
            this.txUsername.Size = new System.Drawing.Size(224, 20);
            this.txUsername.TabIndex = 23;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "User name:";
            // 
            // txId
            // 
            this.txId.BackColor = System.Drawing.Color.White;
            this.txId.Location = new System.Drawing.Point(88, 72);
            this.txId.Name = "txId";
            this.txId.ReadOnly = true;
            this.txId.Size = new System.Drawing.Size(224, 20);
            this.txId.TabIndex = 21;
            // 
            // lbId
            // 
            this.lbId.AutoSize = true;
            this.lbId.Location = new System.Drawing.Point(16, 72);
            this.lbId.Name = "lbId";
            this.lbId.Size = new System.Drawing.Size(19, 13);
            this.lbId.TabIndex = 20;
            this.lbId.Text = "Id:";
            // 
            // ApplicationProperties2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 389);
            this.Name = "ApplicationProperties2";
            this.Text = "Application Properties";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).EndInit();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.CheckBox chkPrimary;
        private System.Windows.Forms.TextBox txNumThreads;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txState;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txCompleted;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txCreated;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txUsername;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txId;
        private System.Windows.Forms.Label lbId;
    }
}