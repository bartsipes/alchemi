#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	ManagerTemplateForm.cs
* Project		:	Alchemi Manager 
* Created on	:	2005
* Copyright		:	Copyright © 2005 The University of Melbourne
*					This technology has been developed with the support of 
*					the Australian Research Council and the University of Melbourne
*					research grants as part of the Gridbus Project
*					within GRIDS Laboratory at the University of Melbourne, Australia.
* Author         :  Krishna Nadiminti (kna@csse.unimelb.edu.au) and Rajkumar Buyya (raj@csse.unimelb.edu.au)
* License        :  GPL
*					This program is free software; you can redistribute it and/or 
*					modify it under the terms of the GNU General Public
*					License as published by the Free Software Foundation;
*					See the GNU General Public License 
*					(http://www.gnu.org/copyleft/gpl.html) for more details.
*
*/
#endregion

using System;
using System.ComponentModel;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using Alchemi.Core;
using Alchemi.Core.Manager;
using Alchemi.ManagerExec;
using Alchemi.Manager;
using log4net.Appender;
using log4net;
using Alchemi.Manager.Storage;
using System.Collections;
using Alchemi.Core.Utility;
using Alchemi.Core.EndPointUtils;

public class ManagerTemplateForm : Form
{
    #region UI Controls

    private IContainer components;

    protected Button uiStartButton;
    protected TextBox uiManagerHostTextBox;
    protected TextBox uiManagerPortTextBox;
    protected TextBox uiIdTextBox;
    protected CheckBox uiIntermediateComboBox;
    protected Button uiStopButton;
    protected Button uiResetButton;
    protected CheckBox uiDedicatedCheckBox;

    protected NotifyIcon TrayIcon;

    protected ContextMenu TrayMenu;
    protected MenuItem tmExit;
    protected MainMenu MainMenu;
    protected MenuItem uiManagerExitMenuItem;
    protected MenuItem uiHelpAboutMenuItem;
    protected MenuItem uiManagerMenuItem;
    protected MenuItem uiHelpMenuItem;

    protected TabControl tabControl;
    protected Label uiManagerHostLabel;
    protected GroupBox uiNodeConfigurationGroupBox;
    protected Label uiIdLabel;
    protected Label uiManagerPortLabel;

    protected TabPage uiAdvancedTabPage;
    protected TextBox uiLogMessagesTextBox;
    protected Label uiLogMessagesLabel;
    protected GroupBox uiStorageConfigurationGroupBox;
    protected Label uiSchedulerLabel;
    protected ComboBox uiSchedulerComboBox;
    protected LinkLabel uiViewFullLogLinkLabel;
    protected GroupBox uiActionsGroupBox;

    protected StatusBar uiStatusBar;
    protected TabPage uiSetupConnectionTabPage;
    protected ProgressBar uiProgressBar;

    #endregion

    protected ManagerContainer _container = null;
    private GroupBox uiDatabaseServerGroupBox;
    protected TextBox uiDatabasePasswordTextBox;
    protected TextBox uiDatabaseServerTextBox;
    protected Label uiDatabaseUserLabel;
    protected Label uiDatabasePasswordLabel;
    protected Label uiDatabaseServerLabel;
    protected TextBox uiDatabaseNameTextBox;
    protected Label uiDatabaseNameLabel;
    protected TextBox uiDatabaseUserTextBox;
    private OpenFileDialog openFileDialog;
    protected Label uiDatabaseTypeLabel;
    protected ComboBox uiDatabaseTypeComboBox;
    protected GroupBox uiDatabaseFileGroupBox;
    private Button uiDatabaseFileButton;
    private TextBox uiDatabaseFileTextBox;
    private TabPage tabEndPoints;
    protected EndPointManagerControl ucEndPoints;
    protected static readonly Logger logger = new Logger();

    public ManagerTemplateForm()
    {
        InitializeComponent();
        ManagerContainer.ManagerStartEvent += new ManagerStartedEventHandler(this.Manager_StartStatusEvent);
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (components != null)
            {
                components.Dispose();
            }
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManagerTemplateForm));
        this.uiManagerHostLabel = new System.Windows.Forms.Label();
        this.uiManagerHostTextBox = new System.Windows.Forms.TextBox();
        this.uiStartButton = new System.Windows.Forms.Button();
        this.uiNodeConfigurationGroupBox = new System.Windows.Forms.GroupBox();
        this.uiDedicatedCheckBox = new System.Windows.Forms.CheckBox();
        this.uiIdLabel = new System.Windows.Forms.Label();
        this.uiIntermediateComboBox = new System.Windows.Forms.CheckBox();
        this.uiIdTextBox = new System.Windows.Forms.TextBox();
        this.uiManagerPortLabel = new System.Windows.Forms.Label();
        this.uiManagerPortTextBox = new System.Windows.Forms.TextBox();
        this.uiResetButton = new System.Windows.Forms.Button();
        this.uiStopButton = new System.Windows.Forms.Button();
        this.TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
        this.TrayMenu = new System.Windows.Forms.ContextMenu();
        this.tmExit = new System.Windows.Forms.MenuItem();
        this.MainMenu = new System.Windows.Forms.MainMenu(this.components);
        this.uiManagerMenuItem = new System.Windows.Forms.MenuItem();
        this.uiManagerExitMenuItem = new System.Windows.Forms.MenuItem();
        this.uiHelpMenuItem = new System.Windows.Forms.MenuItem();
        this.uiHelpAboutMenuItem = new System.Windows.Forms.MenuItem();
        this.uiActionsGroupBox = new System.Windows.Forms.GroupBox();
        this.uiStatusBar = new System.Windows.Forms.StatusBar();
        this.tabControl = new System.Windows.Forms.TabControl();
        this.uiSetupConnectionTabPage = new System.Windows.Forms.TabPage();
        this.uiAdvancedTabPage = new System.Windows.Forms.TabPage();
        this.uiSchedulerComboBox = new System.Windows.Forms.ComboBox();
        this.uiStorageConfigurationGroupBox = new System.Windows.Forms.GroupBox();
        this.uiDatabaseFileGroupBox = new System.Windows.Forms.GroupBox();
        this.uiDatabaseFileButton = new System.Windows.Forms.Button();
        this.uiDatabaseFileTextBox = new System.Windows.Forms.TextBox();
        this.uiDatabaseTypeLabel = new System.Windows.Forms.Label();
        this.uiDatabaseTypeComboBox = new System.Windows.Forms.ComboBox();
        this.uiDatabaseServerGroupBox = new System.Windows.Forms.GroupBox();
        this.uiDatabasePasswordTextBox = new System.Windows.Forms.TextBox();
        this.uiDatabaseServerTextBox = new System.Windows.Forms.TextBox();
        this.uiDatabaseUserLabel = new System.Windows.Forms.Label();
        this.uiDatabasePasswordLabel = new System.Windows.Forms.Label();
        this.uiDatabaseServerLabel = new System.Windows.Forms.Label();
        this.uiDatabaseNameTextBox = new System.Windows.Forms.TextBox();
        this.uiDatabaseNameLabel = new System.Windows.Forms.Label();
        this.uiDatabaseUserTextBox = new System.Windows.Forms.TextBox();
        this.uiSchedulerLabel = new System.Windows.Forms.Label();
        this.tabEndPoints = new System.Windows.Forms.TabPage();
        this.uiProgressBar = new System.Windows.Forms.ProgressBar();
        this.uiLogMessagesTextBox = new System.Windows.Forms.TextBox();
        this.uiLogMessagesLabel = new System.Windows.Forms.Label();
        this.uiViewFullLogLinkLabel = new System.Windows.Forms.LinkLabel();
        this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
        this.ucEndPoints = new Alchemi.Core.EndPointUtils.EndPointManagerControl();
        this.uiNodeConfigurationGroupBox.SuspendLayout();
        this.uiActionsGroupBox.SuspendLayout();
        this.tabControl.SuspendLayout();
        this.uiSetupConnectionTabPage.SuspendLayout();
        this.uiAdvancedTabPage.SuspendLayout();
        this.uiStorageConfigurationGroupBox.SuspendLayout();
        this.uiDatabaseFileGroupBox.SuspendLayout();
        this.uiDatabaseServerGroupBox.SuspendLayout();
        this.tabEndPoints.SuspendLayout();
        this.SuspendLayout();
        // 
        // uiManagerHostLabel
        // 
        this.uiManagerHostLabel.AutoSize = true;
        this.uiManagerHostLabel.Location = new System.Drawing.Point(38, 103);
        this.uiManagerHostLabel.Name = "uiManagerHostLabel";
        this.uiManagerHostLabel.Size = new System.Drawing.Size(74, 13);
        this.uiManagerHostLabel.TabIndex = 1;
        this.uiManagerHostLabel.Text = "Manager Host";
        this.uiManagerHostLabel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
        // 
        // uiManagerHostTextBox
        // 
        this.uiManagerHostTextBox.Location = new System.Drawing.Point(120, 100);
        this.uiManagerHostTextBox.Name = "uiManagerHostTextBox";
        this.uiManagerHostTextBox.Size = new System.Drawing.Size(104, 20);
        this.uiManagerHostTextBox.TabIndex = 9;
        // 
        // uiStartButton
        // 
        this.uiStartButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.uiStartButton.Location = new System.Drawing.Point(88, 50);
        this.uiStartButton.Name = "uiStartButton";
        this.uiStartButton.Size = new System.Drawing.Size(128, 23);
        this.uiStartButton.TabIndex = 12;
        this.uiStartButton.Text = "Start";
        this.uiStartButton.Click += new System.EventHandler(this.uiStartButton_Click);
        // 
        // uiNodeConfigurationGroupBox
        // 
        this.uiNodeConfigurationGroupBox.Controls.Add(this.uiDedicatedCheckBox);
        this.uiNodeConfigurationGroupBox.Controls.Add(this.uiIdLabel);
        this.uiNodeConfigurationGroupBox.Controls.Add(this.uiIntermediateComboBox);
        this.uiNodeConfigurationGroupBox.Controls.Add(this.uiIdTextBox);
        this.uiNodeConfigurationGroupBox.Controls.Add(this.uiManagerPortLabel);
        this.uiNodeConfigurationGroupBox.Controls.Add(this.uiManagerPortTextBox);
        this.uiNodeConfigurationGroupBox.Controls.Add(this.uiManagerHostLabel);
        this.uiNodeConfigurationGroupBox.Controls.Add(this.uiManagerHostTextBox);
        this.uiNodeConfigurationGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.uiNodeConfigurationGroupBox.Location = new System.Drawing.Point(8, 12);
        this.uiNodeConfigurationGroupBox.Name = "uiNodeConfigurationGroupBox";
        this.uiNodeConfigurationGroupBox.Size = new System.Drawing.Size(416, 171);
        this.uiNodeConfigurationGroupBox.TabIndex = 6;
        this.uiNodeConfigurationGroupBox.TabStop = false;
        this.uiNodeConfigurationGroupBox.Text = "Node Configuration";
        // 
        // uiDedicatedCheckBox
        // 
        this.uiDedicatedCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.uiDedicatedCheckBox.Location = new System.Drawing.Point(120, 68);
        this.uiDedicatedCheckBox.Name = "uiDedicatedCheckBox";
        this.uiDedicatedCheckBox.Size = new System.Drawing.Size(88, 24);
        this.uiDedicatedCheckBox.TabIndex = 8;
        this.uiDedicatedCheckBox.Text = "Dedicated";
        // 
        // uiIdLabel
        // 
        this.uiIdLabel.Location = new System.Drawing.Point(96, 44);
        this.uiIdLabel.Name = "uiIdLabel";
        this.uiIdLabel.Size = new System.Drawing.Size(16, 16);
        this.uiIdLabel.TabIndex = 12;
        this.uiIdLabel.Text = "Id";
        this.uiIdLabel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
        // 
        // uiIntermediateComboBox
        // 
        this.uiIntermediateComboBox.Enabled = false;
        this.uiIntermediateComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.uiIntermediateComboBox.Location = new System.Drawing.Point(120, 20);
        this.uiIntermediateComboBox.Name = "uiIntermediateComboBox";
        this.uiIntermediateComboBox.Size = new System.Drawing.Size(88, 24);
        this.uiIntermediateComboBox.TabIndex = 6;
        this.uiIntermediateComboBox.TabStop = false;
        this.uiIntermediateComboBox.Text = "Intermediate";
        // 
        // uiIdTextBox
        // 
        this.uiIdTextBox.Enabled = false;
        this.uiIdTextBox.Location = new System.Drawing.Point(120, 44);
        this.uiIdTextBox.Name = "uiIdTextBox";
        this.uiIdTextBox.Size = new System.Drawing.Size(240, 20);
        this.uiIdTextBox.TabIndex = 7;
        this.uiIdTextBox.TabStop = false;
        // 
        // uiManagerPortLabel
        // 
        this.uiManagerPortLabel.AutoSize = true;
        this.uiManagerPortLabel.Location = new System.Drawing.Point(41, 135);
        this.uiManagerPortLabel.Name = "uiManagerPortLabel";
        this.uiManagerPortLabel.Size = new System.Drawing.Size(71, 13);
        this.uiManagerPortLabel.TabIndex = 6;
        this.uiManagerPortLabel.Text = "Manager Port";
        this.uiManagerPortLabel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
        // 
        // uiManagerPortTextBox
        // 
        this.uiManagerPortTextBox.Location = new System.Drawing.Point(120, 132);
        this.uiManagerPortTextBox.Name = "uiManagerPortTextBox";
        this.uiManagerPortTextBox.Size = new System.Drawing.Size(104, 20);
        this.uiManagerPortTextBox.TabIndex = 10;
        // 
        // uiResetButton
        // 
        this.uiResetButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.uiResetButton.Location = new System.Drawing.Point(88, 20);
        this.uiResetButton.Name = "uiResetButton";
        this.uiResetButton.Size = new System.Drawing.Size(248, 23);
        this.uiResetButton.TabIndex = 11;
        this.uiResetButton.TabStop = false;
        this.uiResetButton.Text = "Reset";
        this.uiResetButton.Click += new System.EventHandler(this.uiResetButton_Click);
        // 
        // uiStopButton
        // 
        this.uiStopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.uiStopButton.Location = new System.Drawing.Point(224, 50);
        this.uiStopButton.Name = "uiStopButton";
        this.uiStopButton.Size = new System.Drawing.Size(112, 23);
        this.uiStopButton.TabIndex = 13;
        this.uiStopButton.Text = "Stop";
        this.uiStopButton.Click += new System.EventHandler(this.uiStopButton_Click);
        // 
        // TrayIcon
        // 
        this.TrayIcon.ContextMenu = this.TrayMenu;
        this.TrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("TrayIcon.Icon")));
        this.TrayIcon.Text = "Alchemi Manager";
        this.TrayIcon.Visible = true;
        this.TrayIcon.Click += new System.EventHandler(this.TrayIcon_Click);
        // 
        // TrayMenu
        // 
        this.TrayMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.tmExit});
        // 
        // tmExit
        // 
        this.tmExit.Index = 0;
        this.tmExit.Text = "Exit";
        this.tmExit.Click += new System.EventHandler(this.tmExit_Click);
        // 
        // MainMenu
        // 
        this.MainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.uiManagerMenuItem,
            this.uiHelpMenuItem});
        // 
        // uiManagerMenuItem
        // 
        this.uiManagerMenuItem.Index = 0;
        this.uiManagerMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.uiManagerExitMenuItem});
        this.uiManagerMenuItem.Text = "Manager";
        // 
        // uiManagerExitMenuItem
        // 
        this.uiManagerExitMenuItem.Index = 0;
        this.uiManagerExitMenuItem.Text = "Exit";
        this.uiManagerExitMenuItem.Click += new System.EventHandler(this.uiManagerExitMenuItem_Click);
        // 
        // uiHelpMenuItem
        // 
        this.uiHelpMenuItem.Index = 1;
        this.uiHelpMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.uiHelpAboutMenuItem});
        this.uiHelpMenuItem.Text = "Help";
        // 
        // uiHelpAboutMenuItem
        // 
        this.uiHelpAboutMenuItem.Index = 0;
        this.uiHelpAboutMenuItem.Text = "About";
        this.uiHelpAboutMenuItem.Click += new System.EventHandler(this.uiHelpAboutMenuItem_Click);
        // 
        // uiActionsGroupBox
        // 
        this.uiActionsGroupBox.Controls.Add(this.uiResetButton);
        this.uiActionsGroupBox.Controls.Add(this.uiStopButton);
        this.uiActionsGroupBox.Controls.Add(this.uiStartButton);
        this.uiActionsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.uiActionsGroupBox.Location = new System.Drawing.Point(23, 305);
        this.uiActionsGroupBox.Name = "uiActionsGroupBox";
        this.uiActionsGroupBox.Size = new System.Drawing.Size(416, 89);
        this.uiActionsGroupBox.TabIndex = 9;
        this.uiActionsGroupBox.TabStop = false;
        this.uiActionsGroupBox.Text = "Actions";
        // 
        // uiStatusBar
        // 
        this.uiStatusBar.Location = new System.Drawing.Point(0, 527);
        this.uiStatusBar.Name = "uiStatusBar";
        this.uiStatusBar.Size = new System.Drawing.Size(456, 22);
        this.uiStatusBar.TabIndex = 10;
        // 
        // tabControl
        // 
        this.tabControl.Controls.Add(this.uiSetupConnectionTabPage);
        this.tabControl.Controls.Add(this.uiAdvancedTabPage);
        this.tabControl.Controls.Add(this.tabEndPoints);
        this.tabControl.Location = new System.Drawing.Point(10, 10);
        this.tabControl.Name = "tabControl";
        this.tabControl.SelectedIndex = 0;
        this.tabControl.Size = new System.Drawing.Size(440, 289);
        this.tabControl.TabIndex = 12;
        // 
        // uiSetupConnectionTabPage
        // 
        this.uiSetupConnectionTabPage.Controls.Add(this.uiNodeConfigurationGroupBox);
        this.uiSetupConnectionTabPage.Location = new System.Drawing.Point(4, 22);
        this.uiSetupConnectionTabPage.Name = "uiSetupConnectionTabPage";
        this.uiSetupConnectionTabPage.Size = new System.Drawing.Size(432, 263);
        this.uiSetupConnectionTabPage.TabIndex = 0;
        this.uiSetupConnectionTabPage.Text = "Setup Connection";
        this.uiSetupConnectionTabPage.UseVisualStyleBackColor = true;
        // 
        // uiAdvancedTabPage
        // 
        this.uiAdvancedTabPage.Controls.Add(this.uiSchedulerComboBox);
        this.uiAdvancedTabPage.Controls.Add(this.uiStorageConfigurationGroupBox);
        this.uiAdvancedTabPage.Controls.Add(this.uiSchedulerLabel);
        this.uiAdvancedTabPage.Location = new System.Drawing.Point(4, 22);
        this.uiAdvancedTabPage.Name = "uiAdvancedTabPage";
        this.uiAdvancedTabPage.Padding = new System.Windows.Forms.Padding(3);
        this.uiAdvancedTabPage.Size = new System.Drawing.Size(432, 263);
        this.uiAdvancedTabPage.TabIndex = 1;
        this.uiAdvancedTabPage.Text = "Advanced";
        this.uiAdvancedTabPage.UseVisualStyleBackColor = true;
        // 
        // uiSchedulerComboBox
        // 
        this.uiSchedulerComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
        this.uiSchedulerComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
        this.uiSchedulerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.uiSchedulerComboBox.FormattingEnabled = true;
        this.uiSchedulerComboBox.Location = new System.Drawing.Point(118, 210);
        this.uiSchedulerComboBox.Name = "uiSchedulerComboBox";
        this.uiSchedulerComboBox.Size = new System.Drawing.Size(296, 21);
        this.uiSchedulerComboBox.TabIndex = 34;
        // 
        // uiStorageConfigurationGroupBox
        // 
        this.uiStorageConfigurationGroupBox.Controls.Add(this.uiDatabaseFileGroupBox);
        this.uiStorageConfigurationGroupBox.Controls.Add(this.uiDatabaseTypeLabel);
        this.uiStorageConfigurationGroupBox.Controls.Add(this.uiDatabaseTypeComboBox);
        this.uiStorageConfigurationGroupBox.Controls.Add(this.uiDatabaseServerGroupBox);
        this.uiStorageConfigurationGroupBox.Location = new System.Drawing.Point(9, 6);
        this.uiStorageConfigurationGroupBox.Name = "uiStorageConfigurationGroupBox";
        this.uiStorageConfigurationGroupBox.Size = new System.Drawing.Size(416, 155);
        this.uiStorageConfigurationGroupBox.TabIndex = 33;
        this.uiStorageConfigurationGroupBox.TabStop = false;
        this.uiStorageConfigurationGroupBox.Text = "Storage Configuration";
        // 
        // uiDatabaseFileGroupBox
        // 
        this.uiDatabaseFileGroupBox.Controls.Add(this.uiDatabaseFileButton);
        this.uiDatabaseFileGroupBox.Controls.Add(this.uiDatabaseFileTextBox);
        this.uiDatabaseFileGroupBox.Location = new System.Drawing.Point(9, 40);
        this.uiDatabaseFileGroupBox.Name = "uiDatabaseFileGroupBox";
        this.uiDatabaseFileGroupBox.Size = new System.Drawing.Size(378, 63);
        this.uiDatabaseFileGroupBox.TabIndex = 43;
        this.uiDatabaseFileGroupBox.TabStop = false;
        this.uiDatabaseFileGroupBox.Text = "Database File";
        // 
        // uiDatabaseFileButton
        // 
        this.uiDatabaseFileButton.Location = new System.Drawing.Point(330, 21);
        this.uiDatabaseFileButton.Name = "uiDatabaseFileButton";
        this.uiDatabaseFileButton.Size = new System.Drawing.Size(31, 23);
        this.uiDatabaseFileButton.TabIndex = 3;
        this.uiDatabaseFileButton.Text = "...";
        this.uiDatabaseFileButton.UseVisualStyleBackColor = true;
        this.uiDatabaseFileButton.Click += new System.EventHandler(this.uiDatabaseFileButton_Click);
        // 
        // uiDatabaseFileTextBox
        // 
        this.uiDatabaseFileTextBox.Enabled = false;
        this.uiDatabaseFileTextBox.Location = new System.Drawing.Point(12, 21);
        this.uiDatabaseFileTextBox.Name = "uiDatabaseFileTextBox";
        this.uiDatabaseFileTextBox.Size = new System.Drawing.Size(312, 20);
        this.uiDatabaseFileTextBox.TabIndex = 2;
        // 
        // uiDatabaseTypeLabel
        // 
        this.uiDatabaseTypeLabel.AutoSize = true;
        this.uiDatabaseTypeLabel.Location = new System.Drawing.Point(6, 22);
        this.uiDatabaseTypeLabel.Name = "uiDatabaseTypeLabel";
        this.uiDatabaseTypeLabel.Size = new System.Drawing.Size(31, 13);
        this.uiDatabaseTypeLabel.TabIndex = 42;
        this.uiDatabaseTypeLabel.Text = "Type";
        this.uiDatabaseTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // uiDatabaseTypeComboBox
        // 
        this.uiDatabaseTypeComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
        this.uiDatabaseTypeComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
        this.uiDatabaseTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.uiDatabaseTypeComboBox.FormattingEnabled = true;
        this.uiDatabaseTypeComboBox.Location = new System.Drawing.Point(43, 19);
        this.uiDatabaseTypeComboBox.Name = "uiDatabaseTypeComboBox";
        this.uiDatabaseTypeComboBox.Size = new System.Drawing.Size(296, 21);
        this.uiDatabaseTypeComboBox.TabIndex = 41;
        this.uiDatabaseTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.uiDatabaseTypeComboBox_SelectedIndexChanged);
        // 
        // uiDatabaseServerGroupBox
        // 
        this.uiDatabaseServerGroupBox.Controls.Add(this.uiDatabasePasswordTextBox);
        this.uiDatabaseServerGroupBox.Controls.Add(this.uiDatabaseServerTextBox);
        this.uiDatabaseServerGroupBox.Controls.Add(this.uiDatabaseUserLabel);
        this.uiDatabaseServerGroupBox.Controls.Add(this.uiDatabasePasswordLabel);
        this.uiDatabaseServerGroupBox.Controls.Add(this.uiDatabaseServerLabel);
        this.uiDatabaseServerGroupBox.Controls.Add(this.uiDatabaseNameTextBox);
        this.uiDatabaseServerGroupBox.Controls.Add(this.uiDatabaseNameLabel);
        this.uiDatabaseServerGroupBox.Controls.Add(this.uiDatabaseUserTextBox);
        this.uiDatabaseServerGroupBox.Location = new System.Drawing.Point(6, 49);
        this.uiDatabaseServerGroupBox.Name = "uiDatabaseServerGroupBox";
        this.uiDatabaseServerGroupBox.Size = new System.Drawing.Size(378, 86);
        this.uiDatabaseServerGroupBox.TabIndex = 33;
        this.uiDatabaseServerGroupBox.TabStop = false;
        this.uiDatabaseServerGroupBox.Text = "Database Server Configuration";
        // 
        // uiDatabasePasswordTextBox
        // 
        this.uiDatabasePasswordTextBox.Location = new System.Drawing.Point(260, 54);
        this.uiDatabasePasswordTextBox.Name = "uiDatabasePasswordTextBox";
        this.uiDatabasePasswordTextBox.PasswordChar = '*';
        this.uiDatabasePasswordTextBox.Size = new System.Drawing.Size(104, 20);
        this.uiDatabasePasswordTextBox.TabIndex = 35;
        // 
        // uiDatabaseServerTextBox
        // 
        this.uiDatabaseServerTextBox.Location = new System.Drawing.Point(68, 22);
        this.uiDatabaseServerTextBox.Name = "uiDatabaseServerTextBox";
        this.uiDatabaseServerTextBox.Size = new System.Drawing.Size(104, 20);
        this.uiDatabaseServerTextBox.TabIndex = 32;
        // 
        // uiDatabaseUserLabel
        // 
        this.uiDatabaseUserLabel.AutoSize = true;
        this.uiDatabaseUserLabel.Location = new System.Drawing.Point(187, 25);
        this.uiDatabaseUserLabel.Name = "uiDatabaseUserLabel";
        this.uiDatabaseUserLabel.Size = new System.Drawing.Size(55, 13);
        this.uiDatabaseUserLabel.TabIndex = 37;
        this.uiDatabaseUserLabel.Text = "Username";
        this.uiDatabaseUserLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // uiDatabasePasswordLabel
        // 
        this.uiDatabasePasswordLabel.AutoSize = true;
        this.uiDatabasePasswordLabel.Location = new System.Drawing.Point(187, 57);
        this.uiDatabasePasswordLabel.Name = "uiDatabasePasswordLabel";
        this.uiDatabasePasswordLabel.Size = new System.Drawing.Size(53, 13);
        this.uiDatabasePasswordLabel.TabIndex = 36;
        this.uiDatabasePasswordLabel.Text = "Password";
        this.uiDatabasePasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // uiDatabaseServerLabel
        // 
        this.uiDatabaseServerLabel.AutoSize = true;
        this.uiDatabaseServerLabel.Location = new System.Drawing.Point(9, 25);
        this.uiDatabaseServerLabel.Name = "uiDatabaseServerLabel";
        this.uiDatabaseServerLabel.Size = new System.Drawing.Size(38, 13);
        this.uiDatabaseServerLabel.TabIndex = 38;
        this.uiDatabaseServerLabel.Text = "Server";
        this.uiDatabaseServerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // uiDatabaseNameTextBox
        // 
        this.uiDatabaseNameTextBox.Location = new System.Drawing.Point(68, 54);
        this.uiDatabaseNameTextBox.Name = "uiDatabaseNameTextBox";
        this.uiDatabaseNameTextBox.Size = new System.Drawing.Size(104, 20);
        this.uiDatabaseNameTextBox.TabIndex = 33;
        // 
        // uiDatabaseNameLabel
        // 
        this.uiDatabaseNameLabel.AutoSize = true;
        this.uiDatabaseNameLabel.Location = new System.Drawing.Point(9, 57);
        this.uiDatabaseNameLabel.Name = "uiDatabaseNameLabel";
        this.uiDatabaseNameLabel.Size = new System.Drawing.Size(53, 13);
        this.uiDatabaseNameLabel.TabIndex = 39;
        this.uiDatabaseNameLabel.Text = "DB Name";
        this.uiDatabaseNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // uiDatabaseUserTextBox
        // 
        this.uiDatabaseUserTextBox.Location = new System.Drawing.Point(260, 22);
        this.uiDatabaseUserTextBox.Name = "uiDatabaseUserTextBox";
        this.uiDatabaseUserTextBox.Size = new System.Drawing.Size(104, 20);
        this.uiDatabaseUserTextBox.TabIndex = 34;
        // 
        // uiSchedulerLabel
        // 
        this.uiSchedulerLabel.AutoSize = true;
        this.uiSchedulerLabel.Location = new System.Drawing.Point(33, 213);
        this.uiSchedulerLabel.Name = "uiSchedulerLabel";
        this.uiSchedulerLabel.Size = new System.Drawing.Size(55, 13);
        this.uiSchedulerLabel.TabIndex = 32;
        this.uiSchedulerLabel.Text = "Scheduler";
        this.uiSchedulerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // tabEndPoints
        // 
        this.tabEndPoints.Controls.Add(this.ucEndPoints);
        this.tabEndPoints.Location = new System.Drawing.Point(4, 22);
        this.tabEndPoints.Name = "tabEndPoints";
        this.tabEndPoints.Padding = new System.Windows.Forms.Padding(3);
        this.tabEndPoints.Size = new System.Drawing.Size(432, 263);
        this.tabEndPoints.TabIndex = 2;
        this.tabEndPoints.Text = "End Points";
        this.tabEndPoints.UseVisualStyleBackColor = true;
        // 
        // uiProgressBar
        // 
        this.uiProgressBar.Location = new System.Drawing.Point(23, 575);
        this.uiProgressBar.Name = "uiProgressBar";
        this.uiProgressBar.Size = new System.Drawing.Size(414, 10);
        this.uiProgressBar.Step = 1;
        this.uiProgressBar.TabIndex = 13;
        this.uiProgressBar.Visible = false;
        // 
        // uiLogMessagesTextBox
        // 
        this.uiLogMessagesTextBox.BackColor = System.Drawing.SystemColors.Info;
        this.uiLogMessagesTextBox.Location = new System.Drawing.Point(22, 424);
        this.uiLogMessagesTextBox.Multiline = true;
        this.uiLogMessagesTextBox.Name = "uiLogMessagesTextBox";
        this.uiLogMessagesTextBox.ReadOnly = true;
        this.uiLogMessagesTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.uiLogMessagesTextBox.Size = new System.Drawing.Size(416, 145);
        this.uiLogMessagesTextBox.TabIndex = 15;
        this.uiLogMessagesTextBox.TabStop = false;
        // 
        // uiLogMessagesLabel
        // 
        this.uiLogMessagesLabel.Location = new System.Drawing.Point(20, 406);
        this.uiLogMessagesLabel.Name = "uiLogMessagesLabel";
        this.uiLogMessagesLabel.Size = new System.Drawing.Size(88, 15);
        this.uiLogMessagesLabel.TabIndex = 16;
        this.uiLogMessagesLabel.Text = "Log Messages";
        this.uiLogMessagesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // uiViewFullLogLinkLabel
        // 
        this.uiViewFullLogLinkLabel.AutoSize = true;
        this.uiViewFullLogLinkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.uiViewFullLogLinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
        this.uiViewFullLogLinkLabel.Location = new System.Drawing.Point(94, 404);
        this.uiViewFullLogLinkLabel.Name = "uiViewFullLogLinkLabel";
        this.uiViewFullLogLinkLabel.Size = new System.Drawing.Size(98, 15);
        this.uiViewFullLogLinkLabel.TabIndex = 17;
        this.uiViewFullLogLinkLabel.TabStop = true;
        this.uiViewFullLogLinkLabel.Text = "( View full log ... )";
        this.uiViewFullLogLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // openFileDialog
        // 
        this.openFileDialog.DefaultExt = "db";
        // 
        // ucEndPoints
        // 
        this.ucEndPoints.EndPoints = null;
        this.ucEndPoints.Location = new System.Drawing.Point(6, 6);
        this.ucEndPoints.Name = "ucEndPoints";
        this.ucEndPoints.Size = new System.Drawing.Size(420, 251);
        this.ucEndPoints.TabIndex = 0;
        // 
        // ManagerTemplateForm
        // 
        this.AcceptButton = this.uiStartButton;
        this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
        this.ClientSize = new System.Drawing.Size(456, 549);
        this.Controls.Add(this.uiViewFullLogLinkLabel);
        this.Controls.Add(this.uiLogMessagesLabel);
        this.Controls.Add(this.uiActionsGroupBox);
        this.Controls.Add(this.uiLogMessagesTextBox);
        this.Controls.Add(this.uiProgressBar);
        this.Controls.Add(this.tabControl);
        this.Controls.Add(this.uiStatusBar);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.MaximizeBox = false;
        this.Menu = this.MainMenu;
        this.Name = "ManagerTemplateForm";
        this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Alchemi Manager";
        this.Load += new System.EventHandler(this.ManagerTemplateForm_Load);
        this.uiNodeConfigurationGroupBox.ResumeLayout(false);
        this.uiNodeConfigurationGroupBox.PerformLayout();
        this.uiActionsGroupBox.ResumeLayout(false);
        this.tabControl.ResumeLayout(false);
        this.uiSetupConnectionTabPage.ResumeLayout(false);
        this.uiAdvancedTabPage.ResumeLayout(false);
        this.uiAdvancedTabPage.PerformLayout();
        this.uiStorageConfigurationGroupBox.ResumeLayout(false);
        this.uiStorageConfigurationGroupBox.PerformLayout();
        this.uiDatabaseFileGroupBox.ResumeLayout(false);
        this.uiDatabaseFileGroupBox.PerformLayout();
        this.uiDatabaseServerGroupBox.ResumeLayout(false);
        this.uiDatabaseServerGroupBox.PerformLayout();
        this.tabEndPoints.ResumeLayout(false);
        this.ResumeLayout(false);
        this.PerformLayout();

    }
    #endregion

    #region Methods to be implemented by sub-classes

    //These methods actually should be "abstract", so that the methods are forcibly implemented.
    //but we need the template class to be non-abstract, for designer-support.
    //this is why we have declared these as virtual

    protected virtual void StartManager() { }

    protected virtual void StopManager() { }

    protected virtual void ResetManager() { }

    protected virtual void Exit() { }

    protected virtual bool Started
    {
        get
        {
            throw new NotImplementedException("This property is meant to be implemented by a subclass");
        }
    }

    #endregion

    #region UI Events

    //the children forms have their own load method.
    private void ManagerTemplateForm_Load(object sender, EventArgs e)
    {
        //this should normally not create any problems, but then during design time it doesnt work, so we need to catch any exceptions
        //that may occur during design time.
        //try
        //{
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(DefaultErrorHandler);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

            // avoid multiple instances
            bool isOnlyInstance = false;
            Mutex mtx = new Mutex(true, "AlcheuiManagerMenuItem_Mutex", out isOnlyInstance);
            if (!isOnlyInstance)
            {
                MessageBox.Show(this, "An instance of this application is already running. The program will now exit.", "Alchemi Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Application.Exit();
            }

            LoadDefaultUI();

            if (_container != null)
                RefreshUIControls(_container.Config);

            uiStartButton.Focus();
        //}
        //catch { }
    }

    private void uiStopButton_Click(object sender, EventArgs e)
    {
        StopManager();
    }

    private void uiStartButton_Click(object sender, EventArgs e)
    {
        StartManager();
    }

    private void uiResetButton_Click(object sender, EventArgs e)
    {
        ResetManager();
    }

    private void tmExit_Click(object sender, EventArgs e)
    {
        Exit();
    }

    protected override void WndProc(ref Message m)
    {
        const int WM_SYSCOMMAND = 0x0112;
        const int SC_CLOSE = 0xF060;
        if (m.Msg == WM_SYSCOMMAND & (int)m.WParam == SC_CLOSE)
        {
            // 'x' button clicked .. minimise to system tray
            Hide();
            return;
        }
        base.WndProc(ref m);
    }

    private void TrayIcon_Click(object sender, EventArgs e)
    {
        Restore();
    }

    private void uiManagerExitMenuItem_Click(object sender, EventArgs e)
    {
        Exit();
    }

    private void uiHelpAboutMenuItem_Click(object sender, EventArgs e)
    {
        new SplashScreen().ShowDialog();
    }

    private void uiLogMessagesTextBox_DoubleClick(object sender, EventArgs e)
    {
        uiLogMessagesTextBox.Clear();
    }


    #endregion

    #region Other Events

    static void DefaultErrorHandler(object sender, UnhandledExceptionEventArgs args)
    {
        Exception e = (Exception)args.ExceptionObject;
        HandleAllUnknownErrors(sender.ToString(), e);
    }

    public void Manager_StartStatusEvent(object sender, EventArgs e)
    {
        //TODO: Do we really need detailed messages such as this:
        //uiStatusBar.Text = msg;
        //Log(msg);
        //if (percentDone==0)
        //{
        //    uiProgressBar.Value = 0;
        //    uiProgressBar.Visible = true;
        //}
        //else if (percentDone >= 100)
        //{
        //    uiProgressBar.Value = uiProgressBar.Maximum;
        //    uiProgressBar.Visible = false;
        //}
        //else
        //{
        //    if ((percentDone + uiProgressBar.Value) <= uiProgressBar.Maximum)
        //    {
        //        uiProgressBar.Value = percentDone;
        //    }
        //    else
        //    {
        //        uiProgressBar.Value = uiProgressBar.Maximum;
        //    }
        //}    	
    }

    private void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
    {
        HandleAllUnknownErrors(sender.ToString(), e.Exception);
    }

    #endregion

    static void HandleAllUnknownErrors(string sender, Exception e)
    {
        logger.Error("Unknown Error from: " + sender, e);
    }

    protected virtual void LoadDefaultUI()
    {
        //fill in the combo's and set default values
        ArrayList data = new ArrayList();
        data.Add(new ManagerDbTypeItem("In Memory", ManagerStorageEnum.InMemory));
        data.Add(new ManagerDbTypeItem("Microsoft SQL Server 2000", ManagerStorageEnum.SqlServer));
        data.Add(new ManagerDbTypeItem("MySQL Server 4.x / 5.x", ManagerStorageEnum.MySql));
        data.Add(new ManagerDbTypeItem("Postgresql 8.2.x", ManagerStorageEnum.Postgresql));
        data.Add(new ManagerDbTypeItem("db4o 6.3.x", ManagerStorageEnum.db4o));
        
        uiDatabaseTypeComboBox.DataSource = data;
        uiDatabaseTypeComboBox.DisplayMember = "Display";
        uiDatabaseTypeComboBox.ValueMember = "Value";
        uiDatabaseTypeComboBox.SelectedIndex = 0;

        uiSchedulerComboBox.Items.Add("Default");
        uiSchedulerComboBox.SelectedIndex = 0;
    }

    protected string GetLogFilePath()
    {
        string filename = null;
        IAppender[] appenders = LogManager.GetRepository().GetAppenders();
        foreach (IAppender appender in appenders)
        {
            //get the first rolling file appender.
            if (appender is RollingFileAppender)
            {
                RollingFileAppender rfa = appender as RollingFileAppender;
                filename = rfa.File;
                break;
            }
        }
        return filename;
    }

    protected Configuration GetConfigFromUI()
    {
        Alchemi.Manager.Configuration conf = new Configuration();

        conf.DbType = (ManagerStorageEnum)uiDatabaseTypeComboBox.SelectedValue;

        conf.DbServer = uiDatabaseServerTextBox.Text;
        conf.DbUsername = uiDatabaseUserTextBox.Text;
        conf.DbPassword = uiDatabasePasswordTextBox.Text;
        conf.DbName = uiDatabaseNameTextBox.Text;

        //conf.OwnPort = int.Parse(uiOwnPortTextBox.Text);
        conf.ManagerHost = uiManagerHostTextBox.Text;
        conf.ManagerPort = int.Parse(uiManagerPortTextBox.Text);
        conf.Intermediate = uiIntermediateComboBox.Checked;
        conf.Dedicated = uiDedicatedCheckBox.Checked;
        conf.Id = uiIdTextBox.Text;

        conf.EndPoints = ucEndPoints.EndPoints;

        return conf;
    }

    protected void RefreshUIControls(Configuration Config)
    {
        if (Config == null)
            return;

        uiDatabaseServerTextBox.Text = Config.DbServer;
        uiDatabaseUserTextBox.Text = Config.DbUsername;
        uiDatabasePasswordTextBox.Text = Config.DbPassword;
        uiDatabaseNameTextBox.Text = Config.DbName;

        uiDatabaseFileTextBox.Text = Config.DbFilePath;
        
        uiDatabaseTypeComboBox.SelectedValue = Config.DbType;
        
        //uiOwnPortTextBox.Text = Config.OwnPort.ToString();
        uiDedicatedCheckBox.Checked = Config.Dedicated;
        
        uiManagerHostTextBox.Text = Config.ManagerHost;
        uiManagerPortTextBox.Text = Config.ManagerPort.ToString();

        uiIdTextBox.Text = Config.Id;
        uiIntermediateComboBox.Checked = Config.Intermediate;

        //dont need to keep calling the property getter...
        //since it queries the status on each call.
        bool started = this.Started;

        uiStartButton.Enabled = !started;
        uiResetButton.Enabled = !started;
        //uiOwnPortTextBox.Enabled = !started;

        uiSchedulerComboBox.Enabled = !started;
        uiDatabaseTypeComboBox.Enabled = !started;

        uiIntermediateComboBox.Enabled = false /* !started */; // <-- hierarchical grid disabled for now
        uiDedicatedCheckBox.Enabled = !started & uiIntermediateComboBox.Checked;
        uiManagerHostTextBox.Enabled = !started & uiIntermediateComboBox.Checked;
        uiManagerPortTextBox.Enabled = !started & uiIntermediateComboBox.Checked;
        uiStopButton.Enabled = started;

        if (started)
        {
            uiStatusBar.Text = "Manager Started.";
        }
        else
        {
            uiStatusBar.Text = "Manager Stopped.";
        }

        uiProgressBar.Hide();
        uiProgressBar.Value = 0;

        ucEndPoints.EndPoints = Config.EndPoints;
    }

    private void Restore()
    {
        this.WindowState = FormWindowState.Normal;
        this.Show();
        this.Activate();
    }

    protected void Log(string s)
    {
        if (uiLogMessagesTextBox != null)
        {
            if (uiLogMessagesTextBox.Text.Length + s.Length >= uiLogMessagesTextBox.MaxLength)
            {
                //remove all old stuff except the last 10 lines.
                string[] s1 = new string[10];
                for (int i = 0; i < 10; i++)
                {
                    s1[9 - i] = uiLogMessagesTextBox.Lines[uiLogMessagesTextBox.Lines.Length - 1 - i];
                }
                uiLogMessagesTextBox.Lines = s1;
            }
            uiLogMessagesTextBox.AppendText(s + Environment.NewLine);
        }
        logger.Info(s);
    }

    private void uiDatabaseFileButton_Click(object sender, EventArgs e)
    {
        OpenFileDialog dialog = new OpenFileDialog();
        dialog.Filter =
           "db files (*.db)|*.db|All files (*.*)|*.*";
        //dialog.InitialDirectory = initialDirectory;
        dialog.InitialDirectory = Utils.GetFilePath("",AlchemiRole.Manager,true);
        dialog.Title = "Select a database file";
        uiDatabaseFileTextBox.Text = (dialog.ShowDialog() == DialogResult.OK)
           ? dialog.FileName : null;
    }

    private void uiDatabaseTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateUIStorage();
    }
    protected void UpdateUIStorage()
    {
        switch (((ManagerDbTypeItem)uiDatabaseTypeComboBox.SelectedItem).Value)
        {
            case ManagerStorageEnum.InMemory:
                uiDatabaseFileGroupBox.Visible = false;
                uiDatabaseServerGroupBox.Visible = false;
                break;
            case ManagerStorageEnum.db4o:
                uiDatabaseFileGroupBox.Visible = true;
                uiDatabaseFileGroupBox.Show();
                uiDatabaseServerGroupBox.Visible = false;
                uiDatabaseServerGroupBox.Hide();
                break;
            default:
                uiDatabaseFileGroupBox.Visible = false;
                uiDatabaseServerGroupBox.Visible = true;
                break;
        }
    }

    private void uiDatabaseFileButton_Click_1(object sender, EventArgs e)
    {

    }

}

//Class to hold values for the db-type combo-box
public class ManagerDbTypeItem
{
    private string _Display;
    private ManagerStorageEnum _Value;

    public string Display
    {
        get { return _Display; }
    }
    public ManagerStorageEnum Value
    {
        get { return _Value; }
    }

    public ManagerDbTypeItem(string display, ManagerStorageEnum value)
    {
        _Display = display;
        _Value = value;
    }

    public override string ToString()
    {
        return Display;
    }
}

