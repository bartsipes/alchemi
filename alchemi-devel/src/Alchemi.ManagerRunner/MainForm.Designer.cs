namespace Alchemi.ManagerRunner
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( MainForm ) );
            this.uiTimer = new System.Windows.Forms.Timer( this.components );
            this.uiStatusStrip = new System.Windows.Forms.StatusStrip();
            this.uiToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.uiImageList = new System.Windows.Forms.ImageList( this.components );
            this.uiStartStopButton = new System.Windows.Forms.Button();
            this.uiContextMenuStrip = new System.Windows.Forms.ContextMenuStrip( this.components );
            this.configureManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uiNotifyIcon = new System.Windows.Forms.NotifyIcon( this.components );
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.uiStatusStrip.SuspendLayout();
            this.uiContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiTimer
            // 
            this.uiTimer.Tick += new System.EventHandler( this.uiTimer_Tick );
            // 
            // uiStatusStrip
            // 
            this.uiStatusStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.uiToolStripStatusLabel} );
            this.uiStatusStrip.Location = new System.Drawing.Point( 0, 107 );
            this.uiStatusStrip.Name = "uiStatusStrip";
            this.uiStatusStrip.Size = new System.Drawing.Size( 104, 22 );
            this.uiStatusStrip.SizingGrip = false;
            this.uiStatusStrip.TabIndex = 3;
            this.uiStatusStrip.Text = "uiStatusStrip";
            // 
            // uiToolStripStatusLabel
            // 
            this.uiToolStripStatusLabel.Name = "uiToolStripStatusLabel";
            this.uiToolStripStatusLabel.Size = new System.Drawing.Size( 47, 17 );
            this.uiToolStripStatusLabel.Text = "Stopped";
            // 
            // uiImageList
            // 
            this.uiImageList.ImageStream = ( (System.Windows.Forms.ImageListStreamer) ( resources.GetObject( "uiImageList.ImageStream" ) ) );
            this.uiImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.uiImageList.Images.SetKeyName( 0, "media-playback-start.png" );
            this.uiImageList.Images.SetKeyName( 1, "media-playback-stop.png" );
            // 
            // uiStartStopButton
            // 
            this.uiStartStopButton.ContextMenuStrip = this.uiContextMenuStrip;
            this.uiStartStopButton.ImageList = this.uiImageList;
            this.uiStartStopButton.Location = new System.Drawing.Point( 12, 12 );
            this.uiStartStopButton.Name = "uiStartStopButton";
            this.uiStartStopButton.Size = new System.Drawing.Size( 80, 80 );
            this.uiStartStopButton.TabIndex = 4;
            this.uiStartStopButton.UseVisualStyleBackColor = true;
            this.uiStartStopButton.Click += new System.EventHandler( this.uiStartStopButton_Click );
            // 
            // uiContextMenuStrip
            // 
            this.uiContextMenuStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.configureManagerToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem} );
            this.uiContextMenuStrip.Name = "uiContextMenuStrip";
            this.uiContextMenuStrip.Size = new System.Drawing.Size( 179, 76 );
            // 
            // configureManagerToolStripMenuItem
            // 
            this.configureManagerToolStripMenuItem.Name = "configureManagerToolStripMenuItem";
            this.configureManagerToolStripMenuItem.Size = new System.Drawing.Size( 178, 22 );
            this.configureManagerToolStripMenuItem.Text = "&Configure Manager...";
            this.configureManagerToolStripMenuItem.Click += new System.EventHandler( this.configureManagerToolStripMenuItem_Click );
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size( 178, 22 );
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler( this.aboutToolStripMenuItem_Click );
            // 
            // uiNotifyIcon
            // 
            this.uiNotifyIcon.ContextMenuStrip = this.uiContextMenuStrip;
            this.uiNotifyIcon.Icon = ( (System.Drawing.Icon) ( resources.GetObject( "uiNotifyIcon.Icon" ) ) );
            this.uiNotifyIcon.Text = "Alchemi Manager";
            this.uiNotifyIcon.Visible = true;
            this.uiNotifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler( this.uiNotifyIcon_MouseClick );
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size( 178, 22 );
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler( this.exitToolStripMenuItem_Click );
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size( 175, 6 );
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 104, 129 );
            this.ContextMenuStrip = this.uiContextMenuStrip;
            this.Controls.Add( this.uiStartStopButton );
            this.Controls.Add( this.uiStatusStrip );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ( (System.Drawing.Icon) ( resources.GetObject( "$this.Icon" ) ) );
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.Form1_FormClosing );
            this.uiStatusStrip.ResumeLayout( false );
            this.uiStatusStrip.PerformLayout();
            this.uiContextMenuStrip.ResumeLayout( false );
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer uiTimer;
        private System.Windows.Forms.StatusStrip uiStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel uiToolStripStatusLabel;
        private System.Windows.Forms.ImageList uiImageList;
        private System.Windows.Forms.Button uiStartStopButton;
        private System.Windows.Forms.ContextMenuStrip uiContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem configureManagerToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon uiNotifyIcon;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}

