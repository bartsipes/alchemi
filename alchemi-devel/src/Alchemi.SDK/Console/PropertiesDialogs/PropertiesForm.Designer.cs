namespace Alchemi.Console.PropertiesDialogs
{
    partial class PropertiesForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertiesForm));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.imgListSmall = new System.Windows.Forms.ImageList(this.components);
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.lineLabel = new System.Windows.Forms.Label();
            this.lbName = new System.Windows.Forms.Label();
            this.iconBox = new System.Windows.Forms.PictureBox();
            this.tabs = new System.Windows.Forms.TabControl();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).BeginInit();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(96, 359);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "OK";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(182, 359);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            // 
            // btnApply
            // 
            this.btnApply.Enabled = false;
            this.btnApply.Location = new System.Drawing.Point(268, 359);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 7;
            this.btnApply.Text = "&Apply";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(16, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // imgListSmall
            // 
            this.imgListSmall.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListSmall.ImageStream")));
            this.imgListSmall.TransparentColor = System.Drawing.Color.Transparent;
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
            this.tabGeneral.Controls.Add(this.lineLabel);
            this.tabGeneral.Controls.Add(this.lbName);
            this.tabGeneral.Controls.Add(this.iconBox);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Size = new System.Drawing.Size(328, 318);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            // 
            // lineLabel
            // 
            this.lineLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lineLabel.Location = new System.Drawing.Point(8, 56);
            this.lineLabel.Name = "lineLabel";
            this.lineLabel.Size = new System.Drawing.Size(312, 2);
            this.lineLabel.TabIndex = 3;
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Location = new System.Drawing.Point(64, 24);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(77, 13);
            this.lbName.TabIndex = 1;
            this.lbName.Text = "Name Property";
            // 
            // iconBox
            // 
            this.iconBox.Image = ((System.Drawing.Image)(resources.GetObject("iconBox.Image")));
            this.iconBox.Location = new System.Drawing.Point(16, 16);
            this.iconBox.Name = "iconBox";
            this.iconBox.Size = new System.Drawing.Size(32, 32);
            this.iconBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.iconBox.TabIndex = 0;
            this.iconBox.TabStop = false;
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tabGeneral);
            this.tabs.Location = new System.Drawing.Point(8, 8);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(336, 344);
            this.tabs.TabIndex = 10;
            // 
            // PropertiesForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(352, 389);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PropertiesForm2";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PropertiesForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).EndInit();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Button btnOK;
        protected System.Windows.Forms.Button btnCancel;
        protected System.Windows.Forms.Button btnApply;
        protected System.Windows.Forms.PictureBox pictureBox1;
        protected System.Windows.Forms.ImageList imgListSmall;
        protected System.Windows.Forms.TabPage tabGeneral;
        protected System.Windows.Forms.Label lineLabel;
        protected System.Windows.Forms.Label lbName;
        protected System.Windows.Forms.PictureBox iconBox;
        protected System.Windows.Forms.TabControl tabs;
    }
}