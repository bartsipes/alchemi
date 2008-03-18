namespace Alchemi.Core.EndPointUtils
{
    partial class EndPointControl
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
            this.cbRemotingMechanism = new System.Windows.Forms.ComboBox();
            this.lblRemotingMechanism = new System.Windows.Forms.Label();
            this.lblHost = new System.Windows.Forms.Label();
            this.txPort = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblFulAddress = new System.Windows.Forms.Label();
            this.txFullAddress = new System.Windows.Forms.TextBox();
            this.lnkAdvanced = new System.Windows.Forms.LinkLabel();
            this.txHost = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cbRemotingMechanism
            // 
            this.cbRemotingMechanism.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbRemotingMechanism.FormattingEnabled = true;
            this.cbRemotingMechanism.Location = new System.Drawing.Point(3, 16);
            this.cbRemotingMechanism.Name = "cbRemotingMechanism";
            this.cbRemotingMechanism.Size = new System.Drawing.Size(355, 21);
            this.cbRemotingMechanism.TabIndex = 0;
            this.cbRemotingMechanism.SelectedIndexChanged += new System.EventHandler(this.cbRemotingMechanism_SelectedIndexChanged);
            // 
            // lblRemotingMechanism
            // 
            this.lblRemotingMechanism.Location = new System.Drawing.Point(3, 0);
            this.lblRemotingMechanism.Name = "lblRemotingMechanism";
            this.lblRemotingMechanism.Size = new System.Drawing.Size(135, 13);
            this.lblRemotingMechanism.TabIndex = 1;
            this.lblRemotingMechanism.Text = "Remoting Mechanism";
            // 
            // lblHost
            // 
            this.lblHost.Location = new System.Drawing.Point(109, 45);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(101, 13);
            this.lblHost.TabIndex = 3;
            this.lblHost.Text = "Host /IP Address";
            // 
            // txPort
            // 
            this.txPort.Location = new System.Drawing.Point(3, 61);
            this.txPort.Name = "txPort";
            this.txPort.Size = new System.Drawing.Size(97, 20);
            this.txPort.TabIndex = 2;
            this.txPort.TextChanged += new System.EventHandler(this.txPort_TextChanged);
            this.txPort.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txPort_KeyDown);
            this.txPort.Leave += new System.EventHandler(this.txPort_Leave);
            // 
            // lblPort
            // 
            this.lblPort.Location = new System.Drawing.Point(3, 45);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(100, 13);
            this.lblPort.TabIndex = 5;
            this.lblPort.Text = "Port";
            // 
            // lblFulAddress
            // 
            this.lblFulAddress.Location = new System.Drawing.Point(3, 89);
            this.lblFulAddress.Name = "lblFulAddress";
            this.lblFulAddress.Size = new System.Drawing.Size(100, 13);
            this.lblFulAddress.TabIndex = 10;
            this.lblFulAddress.Text = "Full address";
            // 
            // txFullAddress
            // 
            this.txFullAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txFullAddress.Location = new System.Drawing.Point(3, 105);
            this.txFullAddress.Name = "txFullAddress";
            this.txFullAddress.Size = new System.Drawing.Size(355, 20);
            this.txFullAddress.TabIndex = 4;
            this.txFullAddress.TextChanged += new System.EventHandler(this.txFullAddress_TextChanged);
            this.txFullAddress.Leave += new System.EventHandler(this.txFullAddress_Leave);
            // 
            // lnkAdvanced
            // 
            this.lnkAdvanced.AutoSize = true;
            this.lnkAdvanced.Location = new System.Drawing.Point(3, 135);
            this.lnkAdvanced.Name = "lnkAdvanced";
            this.lnkAdvanced.Size = new System.Drawing.Size(56, 13);
            this.lnkAdvanced.TabIndex = 5;
            this.lnkAdvanced.TabStop = true;
            this.lnkAdvanced.Text = "Advanced";
            this.lnkAdvanced.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAdvanced_LinkClicked);
            // 
            // txHost
            // 
            this.txHost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txHost.Location = new System.Drawing.Point(112, 61);
            this.txHost.Name = "txHost";
            this.txHost.Size = new System.Drawing.Size(246, 20);
            this.txHost.TabIndex = 3;
            // 
            // EndPointControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txHost);
            this.Controls.Add(this.lnkAdvanced);
            this.Controls.Add(this.txFullAddress);
            this.Controls.Add(this.lblFulAddress);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.txPort);
            this.Controls.Add(this.lblHost);
            this.Controls.Add(this.lblRemotingMechanism);
            this.Controls.Add(this.cbRemotingMechanism);
            this.Name = "EndPointControl";
            this.Size = new System.Drawing.Size(361, 163);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbRemotingMechanism;
        private System.Windows.Forms.Label lblRemotingMechanism;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.TextBox txPort;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblFulAddress;
        private System.Windows.Forms.TextBox txFullAddress;
        private System.Windows.Forms.LinkLabel lnkAdvanced;
        private System.Windows.Forms.TextBox txHost;
    }
}
