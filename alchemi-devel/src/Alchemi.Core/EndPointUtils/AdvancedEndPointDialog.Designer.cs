namespace Alchemi.Core.EndPointUtils
{
    partial class AdvancedEndPointDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvancedEndPointDialog));
            this.ucAdvancedEP = new Alchemi.Core.EndPointUtils.AdvancedEndPointControl();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ucAdvancedEP
            // 
            this.ucAdvancedEP.AddressPart = "";
            this.ucAdvancedEP.BindingConfigurationName = "";
            this.ucAdvancedEP.BindingSettingType = Alchemi.Core.WCFBindingSettingType.Default;
            this.ucAdvancedEP.FixedAddressPart = false;
            this.ucAdvancedEP.FullAddress = "<none>://localhost:0/";
            this.ucAdvancedEP.Host = "localhost";
            this.ucAdvancedEP.HostNameForPublishing = "localhost";
            this.ucAdvancedEP.Location = new System.Drawing.Point(12, 12);
            this.ucAdvancedEP.Name = "ucAdvancedEP";
            this.ucAdvancedEP.Port = 0;
            this.ucAdvancedEP.Protocol = "";
            this.ucAdvancedEP.SelectedRemotingMechanism = Alchemi.Core.RemotingMechanism.WCF;
            this.ucAdvancedEP.ServiceConfigurationName = "";
            this.ucAdvancedEP.Size = new System.Drawing.Size(419, 261);
            this.ucAdvancedEP.TabIndex = 0;
            this.ucAdvancedEP.WCFBinding = Alchemi.Core.WCFBinding.None;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(275, 279);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(356, 279);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // AdvancedEndPointDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 323);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.ucAdvancedEP);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AdvancedEndPointDialog";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Advanced EndPoint Settings";
            this.ResumeLayout(false);

        }

        #endregion

        private Alchemi.Core.EndPointUtils.AdvancedEndPointControl ucAdvancedEP;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}