namespace Alchemi.Core.EndPointUtils
{
    partial class AdvancedEndPointControl
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
            this.cbBinding = new System.Windows.Forms.ComboBox();
            this.lblBinding = new System.Windows.Forms.Label();
            this.txFullAddress = new System.Windows.Forms.TextBox();
            this.lblFulAddress = new System.Windows.Forms.Label();
            this.lblAddressPart = new System.Windows.Forms.Label();
            this.txAddresPart = new System.Windows.Forms.TextBox();
            this.lblProtocol = new System.Windows.Forms.Label();
            this.cbProtocol = new System.Windows.Forms.ComboBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.txPort = new System.Windows.Forms.TextBox();
            this.lblHost = new System.Windows.Forms.Label();
            this.txHost = new System.Windows.Forms.TextBox();
            this.lblRemotingMechanism = new System.Windows.Forms.Label();
            this.cbRemotingMechanism = new System.Windows.Forms.ComboBox();
            this.cbBindingSettingsType = new System.Windows.Forms.ComboBox();
            this.lblBindingSetType = new System.Windows.Forms.Label();
            this.txtBindingConfigurationName = new System.Windows.Forms.TextBox();
            this.lblBindingConfigurationName = new System.Windows.Forms.Label();
            this.txtServiceConfigurationName = new System.Windows.Forms.TextBox();
            this.lblServiceConfigurationName = new System.Windows.Forms.Label();
            this.txtHostNameForPublish = new System.Windows.Forms.TextBox();
            this.lblHostForPublishing = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbBinding
            // 
            this.cbBinding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBinding.FormattingEnabled = true;
            this.cbBinding.Location = new System.Drawing.Point(3, 140);
            this.cbBinding.Name = "cbBinding";
            this.cbBinding.Size = new System.Drawing.Size(115, 21);
            this.cbBinding.TabIndex = 6;
            // 
            // lblBinding
            // 
            this.lblBinding.Location = new System.Drawing.Point(3, 124);
            this.lblBinding.Name = "lblBinding";
            this.lblBinding.Size = new System.Drawing.Size(98, 13);
            this.lblBinding.TabIndex = 25;
            this.lblBinding.Text = "Binding";
            // 
            // txFullAddress
            // 
            this.txFullAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txFullAddress.Location = new System.Drawing.Point(3, 219);
            this.txFullAddress.Name = "txFullAddress";
            this.txFullAddress.Size = new System.Drawing.Size(441, 20);
            this.txFullAddress.TabIndex = 10;
            this.txFullAddress.TextChanged += new System.EventHandler(this.txFullAddress_TextChanged);
            this.txFullAddress.Leave += new System.EventHandler(this.txFullAddress_Leave);
            // 
            // lblFulAddress
            // 
            this.lblFulAddress.Location = new System.Drawing.Point(3, 203);
            this.lblFulAddress.Name = "lblFulAddress";
            this.lblFulAddress.Size = new System.Drawing.Size(98, 13);
            this.lblFulAddress.TabIndex = 24;
            this.lblFulAddress.Text = "Full address";
            // 
            // lblAddressPart
            // 
            this.lblAddressPart.Location = new System.Drawing.Point(80, 86);
            this.lblAddressPart.Name = "lblAddressPart";
            this.lblAddressPart.Size = new System.Drawing.Size(98, 13);
            this.lblAddressPart.TabIndex = 23;
            this.lblAddressPart.Text = "Address part";
            // 
            // txAddresPart
            // 
            this.txAddresPart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txAddresPart.Location = new System.Drawing.Point(83, 102);
            this.txAddresPart.Name = "txAddresPart";
            this.txAddresPart.Size = new System.Drawing.Size(361, 20);
            this.txAddresPart.TabIndex = 5;
            this.txAddresPart.TextChanged += new System.EventHandler(this.txAddresPart_TextChanged);
            this.txAddresPart.Leave += new System.EventHandler(this.txAddresPart_Leave);
            // 
            // lblProtocol
            // 
            this.lblProtocol.Location = new System.Drawing.Point(3, 45);
            this.lblProtocol.Name = "lblProtocol";
            this.lblProtocol.Size = new System.Drawing.Size(98, 13);
            this.lblProtocol.TabIndex = 22;
            this.lblProtocol.Text = "Protocol";
            // 
            // cbProtocol
            // 
            this.cbProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProtocol.FormattingEnabled = true;
            this.cbProtocol.Location = new System.Drawing.Point(3, 61);
            this.cbProtocol.MinimumSize = new System.Drawing.Size(10, 0);
            this.cbProtocol.Name = "cbProtocol";
            this.cbProtocol.Size = new System.Drawing.Size(115, 21);
            this.cbProtocol.TabIndex = 2;
            this.cbProtocol.SelectedIndexChanged += new System.EventHandler(this.cbProtocol_SelectedIndexChanged);
            this.cbProtocol.Leave += new System.EventHandler(this.cbProtocol_Leave);
            // 
            // lblPort
            // 
            this.lblPort.Location = new System.Drawing.Point(3, 85);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(98, 13);
            this.lblPort.TabIndex = 20;
            this.lblPort.Text = "Port";
            // 
            // txPort
            // 
            this.txPort.Location = new System.Drawing.Point(3, 101);
            this.txPort.Name = "txPort";
            this.txPort.Size = new System.Drawing.Size(72, 20);
            this.txPort.TabIndex = 4;
            this.txPort.TextChanged += new System.EventHandler(this.txPort_TextChanged);
            this.txPort.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txPort_KeyDown);
            this.txPort.Leave += new System.EventHandler(this.txPort_Leave);
            // 
            // lblHost
            // 
            this.lblHost.Location = new System.Drawing.Point(142, 45);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(99, 13);
            this.lblHost.TabIndex = 18;
            this.lblHost.Text = "Host /IP Address";
            // 
            // txHost
            // 
            this.txHost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txHost.Location = new System.Drawing.Point(135, 61);
            this.txHost.Name = "txHost";
            this.txHost.Size = new System.Drawing.Size(309, 20);
            this.txHost.TabIndex = 3;
            this.txHost.TextChanged += new System.EventHandler(this.txHost_TextChanged);
            this.txHost.Leave += new System.EventHandler(this.txHost_Leave);
            // 
            // lblRemotingMechanism
            // 
            this.lblRemotingMechanism.Location = new System.Drawing.Point(3, 5);
            this.lblRemotingMechanism.Name = "lblRemotingMechanism";
            this.lblRemotingMechanism.Size = new System.Drawing.Size(133, 13);
            this.lblRemotingMechanism.TabIndex = 15;
            this.lblRemotingMechanism.Text = "Remoting Mechanism";
            // 
            // cbRemotingMechanism
            // 
            this.cbRemotingMechanism.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbRemotingMechanism.FormattingEnabled = true;
            this.cbRemotingMechanism.Location = new System.Drawing.Point(3, 21);
            this.cbRemotingMechanism.Name = "cbRemotingMechanism";
            this.cbRemotingMechanism.Size = new System.Drawing.Size(441, 21);
            this.cbRemotingMechanism.TabIndex = 0;
            this.cbRemotingMechanism.SelectedIndexChanged += new System.EventHandler(this.cbRemotingMechanism_SelectedIndexChanged);
            // 
            // cbBindingSettingsType
            // 
            this.cbBindingSettingsType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBindingSettingsType.FormattingEnabled = true;
            this.cbBindingSettingsType.Items.AddRange(new object[] {
            "Default",
            "Config File"});
            this.cbBindingSettingsType.Location = new System.Drawing.Point(124, 140);
            this.cbBindingSettingsType.Name = "cbBindingSettingsType";
            this.cbBindingSettingsType.Size = new System.Drawing.Size(121, 21);
            this.cbBindingSettingsType.TabIndex = 6;
            this.cbBindingSettingsType.SelectedIndexChanged += new System.EventHandler(this.cbBindingSettingsType_TabIndexChanged);
            this.cbBindingSettingsType.TabIndexChanged += new System.EventHandler(this.cbBindingSettingsType_TabIndexChanged);
            // 
            // lblBindingSetType
            // 
            this.lblBindingSetType.Location = new System.Drawing.Point(121, 125);
            this.lblBindingSetType.Name = "lblBindingSetType";
            this.lblBindingSetType.Size = new System.Drawing.Size(100, 13);
            this.lblBindingSetType.TabIndex = 28;
            this.lblBindingSetType.Text = "Binding Settings";
            // 
            // txtBindingConfigurationName
            // 
            this.txtBindingConfigurationName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBindingConfigurationName.Location = new System.Drawing.Point(252, 140);
            this.txtBindingConfigurationName.Name = "txtBindingConfigurationName";
            this.txtBindingConfigurationName.Size = new System.Drawing.Size(192, 20);
            this.txtBindingConfigurationName.TabIndex = 7;
            // 
            // lblBindingConfigurationName
            // 
            this.lblBindingConfigurationName.Location = new System.Drawing.Point(252, 124);
            this.lblBindingConfigurationName.Name = "lblBindingConfigurationName";
            this.lblBindingConfigurationName.Size = new System.Drawing.Size(126, 13);
            this.lblBindingConfigurationName.TabIndex = 30;
            this.lblBindingConfigurationName.Text = "Binding Config Name";
            // 
            // txtServiceConfigurationName
            // 
            this.txtServiceConfigurationName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServiceConfigurationName.Location = new System.Drawing.Point(252, 180);
            this.txtServiceConfigurationName.Name = "txtServiceConfigurationName";
            this.txtServiceConfigurationName.Size = new System.Drawing.Size(192, 20);
            this.txtServiceConfigurationName.TabIndex = 9;
            // 
            // lblServiceConfigurationName
            // 
            this.lblServiceConfigurationName.Location = new System.Drawing.Point(252, 164);
            this.lblServiceConfigurationName.Name = "lblServiceConfigurationName";
            this.lblServiceConfigurationName.Size = new System.Drawing.Size(164, 13);
            this.lblServiceConfigurationName.TabIndex = 32;
            this.lblServiceConfigurationName.Text = "Service Config Name";
            // 
            // txtHostNameForPublish
            // 
            this.txtHostNameForPublish.Location = new System.Drawing.Point(3, 180);
            this.txtHostNameForPublish.Name = "txtHostNameForPublish";
            this.txtHostNameForPublish.Size = new System.Drawing.Size(242, 20);
            this.txtHostNameForPublish.TabIndex = 8;
            // 
            // lblHostForPublishing
            // 
            this.lblHostForPublishing.Location = new System.Drawing.Point(3, 164);
            this.lblHostForPublishing.Name = "lblHostForPublishing";
            this.lblHostForPublishing.Size = new System.Drawing.Size(100, 13);
            this.lblHostForPublishing.TabIndex = 34;
            this.lblHostForPublishing.Text = "Publish Hostname";
            // 
            // AdvancedEndPointControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblHostForPublishing);
            this.Controls.Add(this.txtHostNameForPublish);
            this.Controls.Add(this.lblServiceConfigurationName);
            this.Controls.Add(this.txtServiceConfigurationName);
            this.Controls.Add(this.lblBindingConfigurationName);
            this.Controls.Add(this.txtBindingConfigurationName);
            this.Controls.Add(this.lblBindingSetType);
            this.Controls.Add(this.cbBindingSettingsType);
            this.Controls.Add(this.cbBinding);
            this.Controls.Add(this.lblBinding);
            this.Controls.Add(this.txFullAddress);
            this.Controls.Add(this.lblFulAddress);
            this.Controls.Add(this.lblAddressPart);
            this.Controls.Add(this.txAddresPart);
            this.Controls.Add(this.lblProtocol);
            this.Controls.Add(this.cbProtocol);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.txPort);
            this.Controls.Add(this.lblHost);
            this.Controls.Add(this.txHost);
            this.Controls.Add(this.lblRemotingMechanism);
            this.Controls.Add(this.cbRemotingMechanism);
            this.Name = "AdvancedEndPointControl";
            this.Size = new System.Drawing.Size(447, 256);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbBinding;
        private System.Windows.Forms.Label lblBinding;
        private System.Windows.Forms.TextBox txFullAddress;
        private System.Windows.Forms.Label lblFulAddress;
        private System.Windows.Forms.Label lblAddressPart;
        private System.Windows.Forms.TextBox txAddresPart;
        private System.Windows.Forms.Label lblProtocol;
        private System.Windows.Forms.ComboBox cbProtocol;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txPort;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.TextBox txHost;
        private System.Windows.Forms.Label lblRemotingMechanism;
        private System.Windows.Forms.ComboBox cbRemotingMechanism;
        private System.Windows.Forms.ComboBox cbBindingSettingsType;
        private System.Windows.Forms.Label lblBindingSetType;
        private System.Windows.Forms.TextBox txtBindingConfigurationName;
        private System.Windows.Forms.Label lblBindingConfigurationName;
        private System.Windows.Forms.TextBox txtServiceConfigurationName;
        private System.Windows.Forms.Label lblServiceConfigurationName;
        private System.Windows.Forms.TextBox txtHostNameForPublish;
        private System.Windows.Forms.Label lblHostForPublishing;
    }
}
