namespace Alchemi.Core.EndPointUtils
{
    partial class EndPointManagerControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnNew = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lbEndPointList = new System.Windows.Forms.ListBox();
            this.txtEPName = new System.Windows.Forms.TextBox();
            this.ucEndPointConfig = new Alchemi.Core.EndPointUtils.EndPointControl();
            this.SuspendLayout();
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(3, 3);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 0;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(84, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(363, 207);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lbEndPointList
            // 
            this.lbEndPointList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lbEndPointList.FormattingEnabled = true;
            this.lbEndPointList.Location = new System.Drawing.Point(3, 32);
            this.lbEndPointList.Name = "lbEndPointList";
            this.lbEndPointList.Size = new System.Drawing.Size(156, 199);
            this.lbEndPointList.TabIndex = 3;
            this.lbEndPointList.SelectedIndexChanged += new System.EventHandler(this.lbEndPoints_SelectedIndexChanged);
            // 
            // txtEPName
            // 
            this.txtEPName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEPName.Enabled = false;
            this.txtEPName.Location = new System.Drawing.Point(165, 6);
            this.txtEPName.Name = "txtEPName";
            this.txtEPName.Size = new System.Drawing.Size(273, 20);
            this.txtEPName.TabIndex = 4;
            // 
            // ucEndPointConfig
            // 
            this.ucEndPointConfig.AddressPart = "";
            this.ucEndPointConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ucEndPointConfig.BindingConfigurationName = "";
            this.ucEndPointConfig.BindingSettingType = Alchemi.Core.WCFBindingSettingType.Default;
            this.ucEndPointConfig.Enabled = false;
            this.ucEndPointConfig.FixedAddressPart = false;
            this.ucEndPointConfig.FullAddress = "http://localhost:0/";
            this.ucEndPointConfig.Host = "localhost";
            this.ucEndPointConfig.HostNameForPublishing = "localhost";
            this.ucEndPointConfig.Location = new System.Drawing.Point(165, 32);
            this.ucEndPointConfig.Name = "ucEndPointConfig";
            this.ucEndPointConfig.Port = 0;
            this.ucEndPointConfig.Protocol = "";
            this.ucEndPointConfig.SelectedRemotingMechanism = Alchemi.Core.RemotingMechanism.WCF;
            this.ucEndPointConfig.ServiceConfigurationName = "";
            this.ucEndPointConfig.Size = new System.Drawing.Size(273, 169);
            this.ucEndPointConfig.TabIndex = 5;
            this.ucEndPointConfig.WCFBinding = Alchemi.Core.WCFBinding.None;
            // 
            // EndPointManagerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ucEndPointConfig);
            this.Controls.Add(this.txtEPName);
            this.Controls.Add(this.lbEndPointList);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnNew);
            this.Name = "EndPointManagerControl";
            this.Size = new System.Drawing.Size(441, 242);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ListBox lbEndPointList;
        private System.Windows.Forms.TextBox txtEPName;
        private EndPointControl ucEndPointConfig;
    }
}
