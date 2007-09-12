namespace Alchemi.Console.PropertiesDialogs
{
    partial class ThreadProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThreadProperties));
            this.btnStop = new System.Windows.Forms.Button();
            this.txPriority = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txExecutor = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txState = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txCompleted = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txCreated = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txApplication = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
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
            this.tabGeneral.Controls.Add(this.txPriority);
            this.tabGeneral.Controls.Add(this.label6);
            this.tabGeneral.Controls.Add(this.txExecutor);
            this.tabGeneral.Controls.Add(this.label2);
            this.tabGeneral.Controls.Add(this.txState);
            this.tabGeneral.Controls.Add(this.label5);
            this.tabGeneral.Controls.Add(this.txCompleted);
            this.tabGeneral.Controls.Add(this.label4);
            this.tabGeneral.Controls.Add(this.txCreated);
            this.tabGeneral.Controls.Add(this.label3);
            this.tabGeneral.Controls.Add(this.txApplication);
            this.tabGeneral.Controls.Add(this.label1);
            this.tabGeneral.Controls.SetChildIndex(this.iconBox, 0);
            this.tabGeneral.Controls.SetChildIndex(this.lbName, 0);
            this.tabGeneral.Controls.SetChildIndex(this.lineLabel, 0);
            this.tabGeneral.Controls.SetChildIndex(this.label1, 0);
            this.tabGeneral.Controls.SetChildIndex(this.txApplication, 0);
            this.tabGeneral.Controls.SetChildIndex(this.label3, 0);
            this.tabGeneral.Controls.SetChildIndex(this.txCreated, 0);
            this.tabGeneral.Controls.SetChildIndex(this.label4, 0);
            this.tabGeneral.Controls.SetChildIndex(this.txCompleted, 0);
            this.tabGeneral.Controls.SetChildIndex(this.label5, 0);
            this.tabGeneral.Controls.SetChildIndex(this.txState, 0);
            this.tabGeneral.Controls.SetChildIndex(this.label2, 0);
            this.tabGeneral.Controls.SetChildIndex(this.txExecutor, 0);
            this.tabGeneral.Controls.SetChildIndex(this.label6, 0);
            this.tabGeneral.Controls.SetChildIndex(this.txPriority, 0);
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
            this.btnStop.TabIndex = 43;
            this.btnStop.Text = "Abort";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // txPriority
            // 
            this.txPriority.BackColor = System.Drawing.Color.White;
            this.txPriority.Location = new System.Drawing.Point(88, 232);
            this.txPriority.Name = "txPriority";
            this.txPriority.ReadOnly = true;
            this.txPriority.Size = new System.Drawing.Size(120, 20);
            this.txPriority.TabIndex = 42;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 232);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 41;
            this.label6.Text = "Priority:";
            // 
            // txExecutor
            // 
            this.txExecutor.BackColor = System.Drawing.Color.White;
            this.txExecutor.Location = new System.Drawing.Point(88, 104);
            this.txExecutor.Name = "txExecutor";
            this.txExecutor.ReadOnly = true;
            this.txExecutor.Size = new System.Drawing.Size(224, 20);
            this.txExecutor.TabIndex = 40;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 39;
            this.label2.Text = "Executor:";
            // 
            // txState
            // 
            this.txState.BackColor = System.Drawing.Color.White;
            this.txState.Location = new System.Drawing.Point(88, 200);
            this.txState.Name = "txState";
            this.txState.ReadOnly = true;
            this.txState.Size = new System.Drawing.Size(120, 20);
            this.txState.TabIndex = 38;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 200);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 37;
            this.label5.Text = "State:";
            // 
            // txCompleted
            // 
            this.txCompleted.BackColor = System.Drawing.Color.White;
            this.txCompleted.Location = new System.Drawing.Point(88, 168);
            this.txCompleted.Name = "txCompleted";
            this.txCompleted.ReadOnly = true;
            this.txCompleted.Size = new System.Drawing.Size(224, 20);
            this.txCompleted.TabIndex = 36;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 168);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 35;
            this.label4.Text = "Completed:";
            // 
            // txCreated
            // 
            this.txCreated.BackColor = System.Drawing.Color.White;
            this.txCreated.Location = new System.Drawing.Point(88, 136);
            this.txCreated.Name = "txCreated";
            this.txCreated.ReadOnly = true;
            this.txCreated.Size = new System.Drawing.Size(224, 20);
            this.txCreated.TabIndex = 34;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 33;
            this.label3.Text = "Created:";
            // 
            // txApplication
            // 
            this.txApplication.BackColor = System.Drawing.Color.White;
            this.txApplication.Location = new System.Drawing.Point(88, 72);
            this.txApplication.Name = "txApplication";
            this.txApplication.ReadOnly = true;
            this.txApplication.Size = new System.Drawing.Size(224, 20);
            this.txApplication.TabIndex = 32;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 31;
            this.label1.Text = "Application:";
            // 
            // ThreadProperties2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 389);
            this.Name = "ThreadProperties2";
            this.Text = "Thread Properties";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).EndInit();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TextBox txPriority;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txExecutor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txState;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txCompleted;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txCreated;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txApplication;
        private System.Windows.Forms.Label label1;
    }
}