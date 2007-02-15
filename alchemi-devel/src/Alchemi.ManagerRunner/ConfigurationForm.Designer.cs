namespace Alchemi.ManagerRunner
{
    partial class ConfigurationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.uiConnectionPortTextBox = new System.Windows.Forms.TextBox();
            this.uiConnectionPortLabel = new System.Windows.Forms.Label();
            this.uiStorageMethodComboBox = new System.Windows.Forms.ComboBox();
            this.uiStorageMethodLabel = new System.Windows.Forms.Label();
            this.uiServerNameLabel = new System.Windows.Forms.Label();
            this.uiServerNameTextBox = new System.Windows.Forms.TextBox();
            this.uiUsernameLabel = new System.Windows.Forms.Label();
            this.uiUsernameTextBox = new System.Windows.Forms.TextBox();
            this.uiPasswordLabel = new System.Windows.Forms.Label();
            this.uiPasswordTextBox = new System.Windows.Forms.TextBox();
            this.uiCancelButton = new System.Windows.Forms.Button();
            this.uiOkButton = new System.Windows.Forms.Button();
            this.uiDatabaseNameLabel = new System.Windows.Forms.Label();
            this.uiDatabaseNameTextBox = new System.Windows.Forms.TextBox();
            this.uiErrorProvider = new System.Windows.Forms.ErrorProvider( this.components );
            ( (System.ComponentModel.ISupportInitialize) ( this.uiErrorProvider ) ).BeginInit();
            this.SuspendLayout();
            // 
            // uiConnectionPortTextBox
            // 
            this.uiConnectionPortTextBox.Location = new System.Drawing.Point( 104, 12 );
            this.uiConnectionPortTextBox.Name = "uiConnectionPortTextBox";
            this.uiConnectionPortTextBox.Size = new System.Drawing.Size( 121, 20 );
            this.uiConnectionPortTextBox.TabIndex = 0;
            this.uiConnectionPortTextBox.TextChanged += new System.EventHandler( this.uiConnectionPortTextBox_TextChanged );
            // 
            // uiConnectionPortLabel
            // 
            this.uiConnectionPortLabel.AutoSize = true;
            this.uiConnectionPortLabel.Location = new System.Drawing.Point( 12, 15 );
            this.uiConnectionPortLabel.Name = "uiConnectionPortLabel";
            this.uiConnectionPortLabel.Size = new System.Drawing.Size( 86, 13 );
            this.uiConnectionPortLabel.TabIndex = 1;
            this.uiConnectionPortLabel.Text = "Connection Port:";
            this.uiConnectionPortLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // uiStorageMethodComboBox
            // 
            this.uiStorageMethodComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uiStorageMethodComboBox.FormattingEnabled = true;
            this.uiStorageMethodComboBox.Location = new System.Drawing.Point( 104, 64 );
            this.uiStorageMethodComboBox.Name = "uiStorageMethodComboBox";
            this.uiStorageMethodComboBox.Size = new System.Drawing.Size( 121, 21 );
            this.uiStorageMethodComboBox.TabIndex = 2;
            this.uiStorageMethodComboBox.SelectedIndexChanged += new System.EventHandler( this.uiStorageMethodComboBox_SelectedIndexChanged );
            // 
            // uiStorageMethodLabel
            // 
            this.uiStorageMethodLabel.AutoSize = true;
            this.uiStorageMethodLabel.Location = new System.Drawing.Point( 12, 67 );
            this.uiStorageMethodLabel.Name = "uiStorageMethodLabel";
            this.uiStorageMethodLabel.Size = new System.Drawing.Size( 86, 13 );
            this.uiStorageMethodLabel.TabIndex = 3;
            this.uiStorageMethodLabel.Text = "Storage Method:";
            this.uiStorageMethodLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // uiServerNameLabel
            // 
            this.uiServerNameLabel.AutoSize = true;
            this.uiServerNameLabel.Location = new System.Drawing.Point( 26, 94 );
            this.uiServerNameLabel.Name = "uiServerNameLabel";
            this.uiServerNameLabel.Size = new System.Drawing.Size( 72, 13 );
            this.uiServerNameLabel.TabIndex = 5;
            this.uiServerNameLabel.Text = "Server Name:";
            this.uiServerNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // uiServerNameTextBox
            // 
            this.uiServerNameTextBox.Location = new System.Drawing.Point( 104, 91 );
            this.uiServerNameTextBox.Name = "uiServerNameTextBox";
            this.uiServerNameTextBox.Size = new System.Drawing.Size( 121, 20 );
            this.uiServerNameTextBox.TabIndex = 4;
            this.uiServerNameTextBox.TextChanged += new System.EventHandler( this.uiServerNameTextBox_TextChanged );
            // 
            // uiUsernameLabel
            // 
            this.uiUsernameLabel.AutoSize = true;
            this.uiUsernameLabel.Location = new System.Drawing.Point( 39, 147 );
            this.uiUsernameLabel.Name = "uiUsernameLabel";
            this.uiUsernameLabel.Size = new System.Drawing.Size( 58, 13 );
            this.uiUsernameLabel.TabIndex = 7;
            this.uiUsernameLabel.Text = "Username:";
            this.uiUsernameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // uiUsernameTextBox
            // 
            this.uiUsernameTextBox.Location = new System.Drawing.Point( 103, 144 );
            this.uiUsernameTextBox.Name = "uiUsernameTextBox";
            this.uiUsernameTextBox.Size = new System.Drawing.Size( 121, 20 );
            this.uiUsernameTextBox.TabIndex = 6;
            this.uiUsernameTextBox.TextChanged += new System.EventHandler( this.uiUsernameTextBox_TextChanged );
            // 
            // uiPasswordLabel
            // 
            this.uiPasswordLabel.AutoSize = true;
            this.uiPasswordLabel.Location = new System.Drawing.Point( 41, 173 );
            this.uiPasswordLabel.Name = "uiPasswordLabel";
            this.uiPasswordLabel.Size = new System.Drawing.Size( 56, 13 );
            this.uiPasswordLabel.TabIndex = 9;
            this.uiPasswordLabel.Text = "Password:";
            this.uiPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // uiPasswordTextBox
            // 
            this.uiPasswordTextBox.Location = new System.Drawing.Point( 103, 170 );
            this.uiPasswordTextBox.Name = "uiPasswordTextBox";
            this.uiPasswordTextBox.PasswordChar = '*';
            this.uiPasswordTextBox.Size = new System.Drawing.Size( 121, 20 );
            this.uiPasswordTextBox.TabIndex = 8;
            this.uiPasswordTextBox.TextChanged += new System.EventHandler( this.uiPasswordTextBox_TextChanged );
            // 
            // uiCancelButton
            // 
            this.uiCancelButton.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this.uiCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.uiCancelButton.Location = new System.Drawing.Point( 103, 222 );
            this.uiCancelButton.Name = "uiCancelButton";
            this.uiCancelButton.Size = new System.Drawing.Size( 121, 23 );
            this.uiCancelButton.TabIndex = 10;
            this.uiCancelButton.Text = "Cancel";
            this.uiCancelButton.UseVisualStyleBackColor = true;
            this.uiCancelButton.Click += new System.EventHandler( this.uiCancelButton_Click );
            // 
            // uiOkButton
            // 
            this.uiOkButton.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this.uiOkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.uiOkButton.Location = new System.Drawing.Point( 11, 222 );
            this.uiOkButton.Name = "uiOkButton";
            this.uiOkButton.Size = new System.Drawing.Size( 86, 23 );
            this.uiOkButton.TabIndex = 11;
            this.uiOkButton.Text = "OK";
            this.uiOkButton.UseVisualStyleBackColor = true;
            this.uiOkButton.Click += new System.EventHandler( this.uiOkButton_Click );
            // 
            // uiDatabaseNameLabel
            // 
            this.uiDatabaseNameLabel.AutoSize = true;
            this.uiDatabaseNameLabel.Location = new System.Drawing.Point( 12, 120 );
            this.uiDatabaseNameLabel.Name = "uiDatabaseNameLabel";
            this.uiDatabaseNameLabel.Size = new System.Drawing.Size( 87, 13 );
            this.uiDatabaseNameLabel.TabIndex = 13;
            this.uiDatabaseNameLabel.Text = "Database Name:";
            this.uiDatabaseNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // uiDatabaseNameTextBox
            // 
            this.uiDatabaseNameTextBox.Location = new System.Drawing.Point( 104, 117 );
            this.uiDatabaseNameTextBox.Name = "uiDatabaseNameTextBox";
            this.uiDatabaseNameTextBox.Size = new System.Drawing.Size( 121, 20 );
            this.uiDatabaseNameTextBox.TabIndex = 12;
            this.uiDatabaseNameTextBox.TextChanged += new System.EventHandler( this.uiDatabaseNameTextBox_TextChanged );
            // 
            // uiErrorProvider
            // 
            this.uiErrorProvider.ContainerControl = this;
            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 242, 257 );
            this.ControlBox = false;
            this.Controls.Add( this.uiDatabaseNameLabel );
            this.Controls.Add( this.uiDatabaseNameTextBox );
            this.Controls.Add( this.uiOkButton );
            this.Controls.Add( this.uiCancelButton );
            this.Controls.Add( this.uiPasswordLabel );
            this.Controls.Add( this.uiPasswordTextBox );
            this.Controls.Add( this.uiUsernameLabel );
            this.Controls.Add( this.uiUsernameTextBox );
            this.Controls.Add( this.uiServerNameLabel );
            this.Controls.Add( this.uiServerNameTextBox );
            this.Controls.Add( this.uiStorageMethodLabel );
            this.Controls.Add( this.uiStorageMethodComboBox );
            this.Controls.Add( this.uiConnectionPortLabel );
            this.Controls.Add( this.uiConnectionPortTextBox );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ConfigurationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configure Manager";
            ( (System.ComponentModel.ISupportInitialize) ( this.uiErrorProvider ) ).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox uiConnectionPortTextBox;
        private System.Windows.Forms.Label uiConnectionPortLabel;
        private System.Windows.Forms.ComboBox uiStorageMethodComboBox;
        private System.Windows.Forms.Label uiStorageMethodLabel;
        private System.Windows.Forms.Label uiServerNameLabel;
        private System.Windows.Forms.TextBox uiServerNameTextBox;
        private System.Windows.Forms.Label uiUsernameLabel;
        private System.Windows.Forms.TextBox uiUsernameTextBox;
        private System.Windows.Forms.Label uiPasswordLabel;
        private System.Windows.Forms.TextBox uiPasswordTextBox;
        private System.Windows.Forms.Button uiCancelButton;
        private System.Windows.Forms.Button uiOkButton;
        private System.Windows.Forms.Label uiDatabaseNameLabel;
        private System.Windows.Forms.TextBox uiDatabaseNameTextBox;
        private System.Windows.Forms.ErrorProvider uiErrorProvider;
    }
}