#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
* Title			:	ConsoleForm.cs
* Project		:	Alchemi Console
* Created on	:	Sep 2005
* Copyright		:	Copyright � 2005 The University of Melbourne
*					This technology has been developed with the support of 
*					the Australian Research Council and the University of Melbourne
*					research grants as part of the Gridbus Project
*					within GRIDS Laboratory at the University of Melbourne, Australia.
* Author         :  Krishna Nadiminti (kna@cs.mu.oz.au) and Rajkumar Buyya (raj@cs.mu.oz.au)
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
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Alchemi.Core;
using Alchemi.Core.Manager.Storage;
using Alchemi.Core.Owner;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Drawing.Drawing2D;
//using NPlot;
using log4net;

namespace Alchemi.Console
{
	/// <summary>
	/// Summary description for ConsoleForm.
	/// </summary>
	public class ConsoleForm : System.Windows.Forms.Form
	{
		private bool connected = false;
		private ConsoleNode console = null;

		//we need to keep a reference to these nodes, since they are special and 
		//need to be access across functions
		private SpecialParentNode userParentNode;
		private SpecialParentNode execParentNode;
		private SpecialParentNode appParentNode;

		// Create a logger for use in this class
		private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private System.ComponentModel.IContainer components;

		private System.Windows.Forms.ImageList imgListSmall;
		private System.Windows.Forms.ImageList imgListBig;

		//menus
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem mnuGrid;
		private System.Windows.Forms.MenuItem mnuConnect;
		private System.Windows.Forms.MenuItem mnuView;
		private System.Windows.Forms.MenuItem mnuAction;
		//context-menu
		private System.Windows.Forms.ContextMenu rightClickMenu;
		private System.Windows.Forms.MenuItem mnuGrid_Sep1;
		private System.Windows.Forms.MenuItem mnuContextView;
		private System.Windows.Forms.StatusBar sbar;
		private System.Windows.Forms.MenuItem mnuLargeIcons;
		private System.Windows.Forms.MenuItem mnuSmallIcons;
		private System.Windows.Forms.MenuItem mnuList;
		private System.Windows.Forms.MenuItem mnuDetails;
		private System.Windows.Forms.MenuItem mnuContextNew;
		private System.Windows.Forms.MenuItem mnuContextEdit;
		private System.Windows.Forms.MenuItem mnuContextLargeIcons;
		private System.Windows.Forms.MenuItem mnuContextSmallIcons;
		private System.Windows.Forms.MenuItem mnuContextList;
		private System.Windows.Forms.MenuItem mnuContextDetails;
		private System.Windows.Forms.MenuItem mnuContextProperties;
		private System.Windows.Forms.MenuItem mnuNew;
		private System.Windows.Forms.MenuItem mnuEdit;
		private System.Windows.Forms.MenuItem mnuDelete;
		private System.Windows.Forms.MenuItem mnuProperties;
		private System.Windows.Forms.MenuItem mnuContextDelete;
		private System.Windows.Forms.TreeView tv;
		private System.Windows.Forms.Splitter split;
		private System.Windows.Forms.ListView lv;
		private System.Windows.Forms.ToolBarButton tbtnProperties;
		private System.Windows.Forms.ToolBarButton tbtnRefresh;
		private System.Windows.Forms.ToolBarButton tbtnNew;
		private System.Windows.Forms.ToolBarButton tbtDelete;
		private System.Windows.Forms.ToolBarButton tbtnSep1;
		private System.Windows.Forms.ToolBar tbar;
		private System.Windows.Forms.ToolBarButton tbtnSep2;
		private System.Windows.Forms.ImageList imgLstTbar;
		private System.Windows.Forms.ColumnHeader ch1;
		private System.Windows.Forms.ColumnHeader ch2;
		private System.Windows.Forms.MenuItem mnuClose;
		private System.Windows.Forms.MenuItem mnuContextAction;

		public ConsoleForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			InitTreeView();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ConsoleForm));
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.mnuGrid = new System.Windows.Forms.MenuItem();
			this.mnuConnect = new System.Windows.Forms.MenuItem();
			this.mnuGrid_Sep1 = new System.Windows.Forms.MenuItem();
			this.mnuClose = new System.Windows.Forms.MenuItem();
			this.mnuAction = new System.Windows.Forms.MenuItem();
			this.mnuNew = new System.Windows.Forms.MenuItem();
			this.mnuEdit = new System.Windows.Forms.MenuItem();
			this.mnuDelete = new System.Windows.Forms.MenuItem();
			this.mnuProperties = new System.Windows.Forms.MenuItem();
			this.mnuView = new System.Windows.Forms.MenuItem();
			this.mnuLargeIcons = new System.Windows.Forms.MenuItem();
			this.mnuSmallIcons = new System.Windows.Forms.MenuItem();
			this.mnuList = new System.Windows.Forms.MenuItem();
			this.mnuDetails = new System.Windows.Forms.MenuItem();
			this.mnuContextNew = new System.Windows.Forms.MenuItem();
			this.mnuContextEdit = new System.Windows.Forms.MenuItem();
			this.mnuContextDelete = new System.Windows.Forms.MenuItem();
			this.mnuContextLargeIcons = new System.Windows.Forms.MenuItem();
			this.mnuContextSmallIcons = new System.Windows.Forms.MenuItem();
			this.mnuContextList = new System.Windows.Forms.MenuItem();
			this.mnuContextDetails = new System.Windows.Forms.MenuItem();
			this.rightClickMenu = new System.Windows.Forms.ContextMenu();
			this.mnuContextView = new System.Windows.Forms.MenuItem();
			this.mnuContextAction = new System.Windows.Forms.MenuItem();
			this.mnuContextProperties = new System.Windows.Forms.MenuItem();
			this.imgListBig = new System.Windows.Forms.ImageList(this.components);
			this.imgListSmall = new System.Windows.Forms.ImageList(this.components);
			this.sbar = new System.Windows.Forms.StatusBar();
			this.tbar = new System.Windows.Forms.ToolBar();
			this.tbtnRefresh = new System.Windows.Forms.ToolBarButton();
			this.tbtnSep1 = new System.Windows.Forms.ToolBarButton();
			this.tbtnNew = new System.Windows.Forms.ToolBarButton();
			this.tbtDelete = new System.Windows.Forms.ToolBarButton();
			this.tbtnSep2 = new System.Windows.Forms.ToolBarButton();
			this.tbtnProperties = new System.Windows.Forms.ToolBarButton();
			this.imgLstTbar = new System.Windows.Forms.ImageList(this.components);
			this.tv = new System.Windows.Forms.TreeView();
			this.split = new System.Windows.Forms.Splitter();
			this.lv = new System.Windows.Forms.ListView();
			this.ch1 = new System.Windows.Forms.ColumnHeader();
			this.ch2 = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuGrid,
																					  this.mnuAction,
																					  this.mnuView});
			// 
			// mnuGrid
			// 
			this.mnuGrid.Index = 0;
			this.mnuGrid.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuConnect,
																					this.mnuGrid_Sep1,
																					this.mnuClose});
			this.mnuGrid.Text = "Grid";
			// 
			// mnuConnect
			// 
			this.mnuConnect.Index = 0;
			this.mnuConnect.Text = "Connect...";
			this.mnuConnect.Click += new System.EventHandler(this.mnuConnect_Click);
			// 
			// mnuGrid_Sep1
			// 
			this.mnuGrid_Sep1.Index = 1;
			this.mnuGrid_Sep1.Text = "-";
			// 
			// mnuClose
			// 
			this.mnuClose.Index = 2;
			this.mnuClose.Text = "Close";
			this.mnuClose.Click += new System.EventHandler(this.mnuClose_Click);
			// 
			// mnuAction
			// 
			this.mnuAction.Index = 1;
			this.mnuAction.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuNew,
																					  this.mnuEdit,
																					  this.mnuDelete,
																					  this.mnuProperties});
			this.mnuAction.Text = "Action";
			// 
			// mnuNew
			// 
			this.mnuNew.Index = 0;
			this.mnuNew.Text = "New...";
			this.mnuNew.Click += new System.EventHandler(this.mnuAction_Click);
			// 
			// mnuEdit
			// 
			this.mnuEdit.Index = 1;
			this.mnuEdit.Text = "Edit...";
			this.mnuEdit.Click += new System.EventHandler(this.mnuAction_Click);
			// 
			// mnuDelete
			// 
			this.mnuDelete.Index = 2;
			this.mnuDelete.Text = "Delete";
			this.mnuDelete.Click += new System.EventHandler(this.mnuAction_Click);
			// 
			// mnuProperties
			// 
			this.mnuProperties.Index = 3;
			this.mnuProperties.Text = "Properties";
			this.mnuProperties.Click += new System.EventHandler(this.mnuAction_Click);
			// 
			// mnuView
			// 
			this.mnuView.Index = 2;
			this.mnuView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuLargeIcons,
																					this.mnuSmallIcons,
																					this.mnuList,
																					this.mnuDetails});
			this.mnuView.Text = "View";
			// 
			// mnuLargeIcons
			// 
			this.mnuLargeIcons.Index = 0;
			this.mnuLargeIcons.Text = "Large Icons";
			this.mnuLargeIcons.Click += new System.EventHandler(this.mnuView_Click);
			// 
			// mnuSmallIcons
			// 
			this.mnuSmallIcons.Index = 1;
			this.mnuSmallIcons.Text = "Small Icons";
			this.mnuSmallIcons.Click += new System.EventHandler(this.mnuView_Click);
			// 
			// mnuList
			// 
			this.mnuList.Index = 2;
			this.mnuList.Text = "List";
			this.mnuList.Click += new System.EventHandler(this.mnuView_Click);
			// 
			// mnuDetails
			// 
			this.mnuDetails.Index = 3;
			this.mnuDetails.Text = "Details";
			this.mnuDetails.Click += new System.EventHandler(this.mnuView_Click);
			// 
			// mnuContextNew
			// 
			this.mnuContextNew.Index = 0;
			this.mnuContextNew.Text = "New...";
			this.mnuContextNew.Click += new System.EventHandler(this.mnuAction_Click);
			// 
			// mnuContextEdit
			// 
			this.mnuContextEdit.Index = 1;
			this.mnuContextEdit.Text = "Edit...";
			this.mnuContextEdit.Click += new System.EventHandler(this.mnuAction_Click);
			// 
			// mnuContextDelete
			// 
			this.mnuContextDelete.Index = 2;
			this.mnuContextDelete.Text = "Delete";
			this.mnuContextDelete.Click += new System.EventHandler(this.mnuAction_Click);
			// 
			// mnuContextLargeIcons
			// 
			this.mnuContextLargeIcons.Index = 0;
			this.mnuContextLargeIcons.Text = "Large Icons";
			this.mnuContextLargeIcons.Click += new System.EventHandler(this.mnuView_Click);
			// 
			// mnuContextSmallIcons
			// 
			this.mnuContextSmallIcons.Index = 1;
			this.mnuContextSmallIcons.Text = "Small Icons";
			this.mnuContextSmallIcons.Click += new System.EventHandler(this.mnuView_Click);
			// 
			// mnuContextList
			// 
			this.mnuContextList.Index = 2;
			this.mnuContextList.Text = "List";
			this.mnuContextList.Click += new System.EventHandler(this.mnuView_Click);
			// 
			// mnuContextDetails
			// 
			this.mnuContextDetails.Index = 3;
			this.mnuContextDetails.Text = "Details";
			this.mnuContextDetails.Click += new System.EventHandler(this.mnuView_Click);
			// 
			// rightClickMenu
			// 
			this.rightClickMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						   this.mnuContextView,
																						   this.mnuContextAction,
																						   this.mnuContextProperties});
			// 
			// mnuContextView
			// 
			this.mnuContextView.Index = 0;
			this.mnuContextView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						   this.mnuContextLargeIcons,
																						   this.mnuContextSmallIcons,
																						   this.mnuContextList,
																						   this.mnuContextDetails});
			this.mnuContextView.Text = "View";
			// 
			// mnuContextAction
			// 
			this.mnuContextAction.Index = 1;
			this.mnuContextAction.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							 this.mnuContextNew,
																							 this.mnuContextEdit,
																							 this.mnuContextDelete});
			this.mnuContextAction.Text = "Action";
			// 
			// mnuContextProperties
			// 
			this.mnuContextProperties.Index = 2;
			this.mnuContextProperties.Text = "Properties";
			this.mnuContextProperties.Click += new System.EventHandler(this.mnuAction_Click);
			// 
			// imgListBig
			// 
			this.imgListBig.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imgListBig.ImageSize = new System.Drawing.Size(32, 32);
			this.imgListBig.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListBig.ImageStream")));
			this.imgListBig.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// imgListSmall
			// 
			this.imgListSmall.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
			this.imgListSmall.ImageSize = new System.Drawing.Size(16, 16);
			this.imgListSmall.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListSmall.ImageStream")));
			this.imgListSmall.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// sbar
			// 
			this.sbar.Location = new System.Drawing.Point(0, 559);
			this.sbar.Name = "sbar";
			this.sbar.Size = new System.Drawing.Size(864, 22);
			this.sbar.TabIndex = 0;
			// 
			// tbar
			// 
			this.tbar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.tbar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																					this.tbtnRefresh,
																					this.tbtnSep1,
																					this.tbtnNew,
																					this.tbtDelete,
																					this.tbtnSep2,
																					this.tbtnProperties});
			this.tbar.DropDownArrows = true;
			this.tbar.ImageList = this.imgLstTbar;
			this.tbar.Location = new System.Drawing.Point(0, 0);
			this.tbar.Name = "tbar";
			this.tbar.ShowToolTips = true;
			this.tbar.Size = new System.Drawing.Size(864, 28);
			this.tbar.TabIndex = 4;
			this.tbar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbar_ButtonClick);
			// 
			// tbtnRefresh
			// 
			this.tbtnRefresh.ImageIndex = 1;
			// 
			// tbtnSep1
			// 
			this.tbtnSep1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tbtnNew
			// 
			this.tbtnNew.ImageIndex = 0;
			// 
			// tbtDelete
			// 
			this.tbtDelete.ImageIndex = 2;
			// 
			// tbtnSep2
			// 
			this.tbtnSep2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tbtnProperties
			// 
			this.tbtnProperties.ImageIndex = 3;
			// 
			// imgLstTbar
			// 
			this.imgLstTbar.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
			this.imgLstTbar.ImageSize = new System.Drawing.Size(16, 16);
			this.imgLstTbar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgLstTbar.ImageStream")));
			this.imgLstTbar.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// tv
			// 
			this.tv.Dock = System.Windows.Forms.DockStyle.Left;
			this.tv.HideSelection = false;
			this.tv.ImageList = this.imgListSmall;
			this.tv.Location = new System.Drawing.Point(0, 28);
			this.tv.Name = "tv";
			this.tv.Size = new System.Drawing.Size(208, 531);
			this.tv.TabIndex = 5;
			this.tv.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tv_AfterExpand);
			this.tv.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tv_AfterSelect);
			// 
			// split
			// 
			this.split.Location = new System.Drawing.Point(208, 28);
			this.split.Name = "split";
			this.split.Size = new System.Drawing.Size(3, 531);
			this.split.TabIndex = 6;
			this.split.TabStop = false;
			// 
			// lv
			// 
			this.lv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																				 this.ch1,
																				 this.ch2});
			this.lv.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lv.FullRowSelect = true;
			this.lv.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lv.HideSelection = false;
			this.lv.LargeImageList = this.imgListBig;
			this.lv.Location = new System.Drawing.Point(211, 28);
			this.lv.MultiSelect = false;
			this.lv.Name = "lv";
			this.lv.Size = new System.Drawing.Size(653, 531);
			this.lv.SmallImageList = this.imgListSmall;
			this.lv.TabIndex = 7;
			this.lv.DoubleClick += new System.EventHandler(this.lv_DoubleClick);
			// 
			// ch1
			// 
			this.ch1.Text = "Name";
			this.ch1.Width = 250;
			// 
			// ch2
			// 
			this.ch2.Text = "";
			this.ch2.Width = 150;
			// 
			// ConsoleForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(864, 581);
			this.Controls.Add(this.lv);
			this.Controls.Add(this.split);
			this.Controls.Add(this.tv);
			this.Controls.Add(this.tbar);
			this.Controls.Add(this.sbar);
			this.Menu = this.mainMenu1;
			this.Name = "ConsoleForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Alchemi Console";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Closing += new System.ComponentModel.CancelEventHandler(this.ConsoleForm_Closing);
			this.ResumeLayout(false);

		}
		#endregion

		#region "user-generated methods"
		
		private void RefreshUI()
		{
			if (connected)
			{
				sbar.Text = string.Format("Connected to grid at {0}:{1}.", console.Connection.Host, console.Connection.Port);
				mnuConnect.Text = "Disconnect...";
			}
			else
			{
				sbar.Text = "Not connected.";
				mnuConnect.Text = "Connect...";
			}

			SetColumnHeaders();
			SetMenuState();
			SetToolbarState();

			this.Refresh();
		}

		private void SetMenuState()
		{
			//todo

			//todo also right click menus and their events
		}

		private void SetToolbarState()
		{
			//todo
		}

		private void SetColumnHeaders()
		{
			//set the column headers according to the view.
			if (tv.SelectedNode!=null)
			{
				lv.Columns[1].Width=200;
				if (tv.SelectedNode is ExecutorTreeNode)
				{
					lv.Columns[1].Text = "Host : Port";
				}
				else if (tv.SelectedNode is UserTreeNode)
				{
					lv.Columns[1].Text = "Group";
				}
				else if (tv.SelectedNode is ThreadTreeNode)
				{
					lv.Columns[1].Text = "Application";
				}
				else if (tv.SelectedNode is SpecialParentNode)
				{
					SpecialParentNode spn = (SpecialParentNode)tv.SelectedNode;
					if (spn.NodeType == SpecialParentNodeType.Users) 
					{
						lv.Columns[1].Text = "# of users";
					}
					else
					{
						lv.Columns[1].Text = "";
						lv.Columns[1].Width=0;
					}
				}
				else 
				{
					lv.Columns[1].Text = "";
					lv.Columns[1].Width=0;
				}
			}
		}


		private void Disconnect()
		{
			console = null;
			connected = false;
		}

		private void ShowDisconnectedMessage()
		{
			tv.Nodes.Clear();
			TreeNode node = new TreeNode();
			node.Text = "No connection available.";
			node.ImageIndex = 0;
			tv.Nodes.Add(node);
			
			lv.Items.Clear();
			ListViewItem li = new ListViewItem();
			li.Text = "No connection available.";
			li.ImageIndex = 0;
			lv.Items.Add(li);
			sbar.Text = "No connection available.";
		}

		private void InitTreeView()
		{
			tv.Nodes.Clear();
			if (connected)
			{
				//Add Console Root
				TreeNode root = new TreeNode("Console Root", 1, 1);

				userParentNode = new SpecialParentNode("Users and Groups",8,8);
				userParentNode.NodeType = SpecialParentNodeType.Users;
				userParentNode.Nodes.Add(new DummyTreeNode("", 999, 999));
				root.Nodes.Add(userParentNode);

				execParentNode = new SpecialParentNode("Executors",9,9);
				execParentNode.NodeType = SpecialParentNodeType.Executors;
				execParentNode.Nodes.Add(new DummyTreeNode("", 999, 999));
				root.Nodes.Add(execParentNode);

				appParentNode = new SpecialParentNode("Applications",10,10);
				appParentNode.NodeType = SpecialParentNodeType.Applications;
				appParentNode.Nodes.Add(new DummyTreeNode("", 999, 999));
				root.Nodes.Add(appParentNode);

				tv.Nodes.Add(root);
				tv.Refresh();

				lv.Items.Clear();
				lv.Refresh();
			}
			else
			{
				ShowDisconnectedMessage();
			}
			RefreshUI();
		}

		#endregion

		
		private void mnuConnect_Click(object sender, System.EventArgs e)
		{
			if (!connected)
			{
				logger.Debug("Showing connection dialog...");
				GConnectionDialog gcd = new GConnectionDialog();
				if (gcd.ShowDialog() == DialogResult.OK)
				{
					console = new ConsoleNode(gcd.Connection);
					connected = true;
					InitTreeView();
					RefreshUI();
				}
			}
			else
			{
				Disconnect();
				RefreshUI();
			}
		}

		private void mnuClose_Click(object sender, System.EventArgs e)
		{
			Disconnect();
			this.Close();
		}

		#region "Users and Groups" 
		
		private void ShowUsers()
		{
			if (connected)
			{
				try
				{
					TreeNode rootNode = userParentNode; //use the Users parent node as root
					rootNode.Nodes.Clear();

					//get users and groups.
					//select grp_id, grp_name from grp 
					GroupStorageView[] groups = console.Manager.Admon_GetGroups(console.Credentials);
					//select usr_name, password, grp.grp_id from usr inner join grp on grp.grp_id = usr.grp_id
					UserStorageView[] users = console.Manager.Admon_GetUserList(console.Credentials);

					foreach (GroupStorageView group in groups)
					{
						if (group.GroupName != null)
						{
							GroupTreeNode grpNode = new GroupTreeNode(group.GroupName, 2, 2);
							string grpId = group.GroupId.ToString(); //keep the grp_id in the tag for later
							grpNode.grp_id = grpId;
							grpNode.grp_name = group.GroupName;
							rootNode.Nodes.Add(grpNode);

							//filter the users table.
							//DataRow[] grpUsers = users.Select("grp_id="+grpId , "usr_name");
							foreach (UserStorageView user in users)
							{
								if (user.Username != null && user.GroupId == group.GroupId)
								{
									UserTreeNode usrNode = new UserTreeNode(user.Username, 3, 3);
									usrNode.usr_name = user.Username;
									usrNode.grp_id = grpId;
									//add this user to the parent grp node.
									grpNode.Nodes.Add(usrNode);
								}
							}
						}
						tv.Refresh();
						Application.DoEvents();
					}

				}			
				catch (Exception ex)
				{
					MessageBox.Show("Could not get list of users. Error: "+ex.Message,"Console Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
					sbar.Text = "Couldnot get list of users. Error: " + ex.Message;
				}
			}
			else
			{
				ShowDisconnectedMessage();
			}
			RefreshUI();
		}

		#endregion

		#region "Executors" 
		
		private void ShowExecutors()
		{
			if (connected)
			{
				try
				{
					TreeNode rootNode = execParentNode; //use the Executors parent node as root
					rootNode.Nodes.Clear();

					//select executor_id, host, port, usr_name, is_connected, is_dedicated, cpu_max, convert(varchar, cpu_totalusage * cpu_max / (3600 * 1000)) as cpu_totalusage from executor order by is_connected desc
					ExecutorStorageView[] executors = console.Manager.Admon_GetExecutors(console.Credentials);
					foreach (ExecutorStorageView executor in executors)
					{
						if (executor.HostName != null)
						{
							ExecutorTreeNode exNode = new ExecutorTreeNode(executor.HostName);
							exNode.cpu_max = executor.MaxCpu.ToString();
							exNode.cpu_totalusage = executor.TotalCpuUsage.ToString();
							exNode.executor_id = executor.ExecutorId;
							exNode.host = executor.HostName;
							exNode.is_connected = executor.Connected;
							exNode.is_dedicated = executor.Dedicated;
							exNode.port = executor.Port.ToString();
							exNode.usr_name = executor.Username;

							if (exNode.is_connected)
							{
								exNode.ImageIndex = 5;
							}
							else
							{
								exNode.ImageIndex = 6;
							}
							exNode.SelectedImageIndex = exNode.ImageIndex;
							rootNode.Nodes.Add(exNode);
						}
						tv.Refresh();
						Application.DoEvents();
					}

				}			
				catch (Exception ex)
				{
					MessageBox.Show("Could not get list of executors. Error: "+ex.Message,"Console Error",MessageBoxButtons.OK,MessageBoxIcon.Error );
					sbar.Text = "Couldnot get list of executors. Error: " + ex.Message;
				}
			}
			else
			{
				ShowDisconnectedMessage();
			}
			RefreshUI();
		}

		#endregion

		#region "Applications" 
		
		private void ShowApplications()
		{
			if (connected)
			{
				try
				{
					TreeNode rootNode = appParentNode; //use the Apps parent node as root
					rootNode.Nodes.Clear();

					//get apps and jobs
					//select application_id, [state], time_created, is_primary, usr_name, application_name, time_completed from application
					DataSet ds = console.Manager.Admon_ExecQuery(console.Credentials,
						Alchemi.Core.Manager.Permission.ManageOwnApp,
						"select application_id, [state], time_created, is_primary, usr_name, application_name, time_completed from application");

					DataRowCollection apps = ds.Tables[0].Rows;

					foreach (DataRow app in apps)
					{
						string nodeText = app.IsNull("application_name") ? "Noname: "+app["application_id"].ToString() : app["application_name"].ToString();
						string appId = app["application_id"].ToString(); //keep the id in the tag for later
						
						ApplicationTreeNode appNode = new ApplicationTreeNode(nodeText, 7, 7);
						appNode.application_id = appId;
						appNode.application_name = app["application_name"].ToString();
						appNode.is_primary = (bool)app["is_primary"];
						appNode.state = (ApplicationState)app["state"];
						appNode.time_completed = app["time_completed"].ToString();
						appNode.time_created = app["time_created"].ToString();
						appNode.usr_name = app["usr_name"].ToString();

						rootNode.Nodes.Add(appNode);

						//select thread_id, state, time_started, time_finished, executor_id, priority, failed from thread where application_id = {appId}
						ThreadStorageView[] threads = console.Manager.Admon_GetThreadList(console.Credentials, appId);
						int iterations = 0;
						foreach (ThreadStorageView thread in threads)
						{
							ThreadTreeNode thrNode = new ThreadTreeNode(thread.ThreadId.ToString(), 7, 7);
							thrNode.appId = appId; //keep the id in the tag for later
							thrNode.executor_id = thread.ExecutorId;
							thrNode.failed = thread.Failed; 
							thrNode.priority = thread.Priority;
							thrNode.state = thread.State;
							thrNode.thread_id = thread.ThreadId.ToString();
							thrNode.time_finished = thread.TimeFinished.ToString();
							thrNode.time_started = thread.TimeStarted.ToString();

							//add this thread to the parent app node.
							appNode.Nodes.Add(thrNode);
							iterations++;

							//this is there to make the app more responsive as a GUI to the user,
							//in case there are lots and lots of threads.
							if (iterations >= 49)
							{
								iterations = 0;
								Application.DoEvents();
							}
						}
						
						//set the # of threads here
						appNode.num_threads = threads.Length;

						tv.Refresh();
						Application.DoEvents();
					}

				}			
				catch (Exception ex)
				{
					MessageBox.Show("Couldnot get list of applications. Error: "+ex.StackTrace,"Console Error",MessageBoxButtons.OK,MessageBoxIcon.Error );
					sbar.Text = "Couldnot get list of applications. Error: " + ex.Message;
				}
			}
			else
			{
				ShowDisconnectedMessage();
			}
			RefreshUI();
		}

		#endregion

		private void tv_AfterSelect(object sender, TreeViewEventArgs e)
		{
			logger.Debug("After Select");

			if (!connected)
				return;

			lv.Items.Clear();

			if (e.Node==null)
				return;

			//show the items in the list view.
			if (e.Node.GetNodeCount(false)!=0)
			{
				
				//selected node has children
				foreach (TreeNode node in e.Node.Nodes)
				{
					//add the node to the listview.
					ListViewItem li = new ListViewItem(node.Text);
					li.ImageIndex = node.ImageIndex;

					//store a reference to the tree-view node in the listview item's tag
					li.Tag = node;

					//special cases
					if (e.Node is SpecialParentNode)
					{
						SpecialParentNode spn = (SpecialParentNode)e.Node;
						if (spn.NodeType == SpecialParentNodeType.Users)
						{
							if (!(node is DummyTreeNode))
								li.SubItems.Add(node.GetNodeCount(false).ToString());
						}
					}

					lv.Items.Add(li);
					lv.Refresh();
					Application.DoEvents(); //to make sure the GUI is responsive
				}
			}
			else
			{
				//add the node to the listview.
				ListViewItem li = new ListViewItem(e.Node.Text);
				li.ImageIndex = e.Node.ImageIndex;

				//special cases
				if (e.Node is UserTreeNode)
				{
					UserTreeNode utn = (UserTreeNode)e.Node;
					li.SubItems.Add(utn.Parent.Text);
				} 
				else if (e.Node is ExecutorTreeNode)
				{
					ExecutorTreeNode etn = (ExecutorTreeNode)e.Node;
					li.SubItems.Add(etn.host+":"+etn.port);
				}
				else if (e.Node is ThreadTreeNode)
				{
					ThreadTreeNode ttn = (ThreadTreeNode)e.Node;
					li.SubItems.Add(ttn.Parent.Text);
				}

				//store a reference to the tree-view node in the listview item's tag
				li.Tag = e.Node;
				lv.Items.Add(li);
				lv.Refresh();
			}

			
			RefreshUI();
		}

		private void mnuView_Click(object sender, System.EventArgs e)
		{
			if (sender == mnuLargeIcons || sender == mnuContextLargeIcons)
			{
				lv.View = View.LargeIcon;
			}
			else if (sender == mnuSmallIcons || sender == mnuContextSmallIcons)
			{
				lv.View = View.SmallIcon;
			}
			else if (sender == mnuList || sender == mnuContextList)
			{
				lv.View = View.List;
			}
			else if (sender == mnuDetails || sender == mnuContextDetails)
			{
				lv.View = View.Details;
			}

			RefreshUI();
			//need to make double sure.??
			//when the context menu appears in the list view, this menu doesnt disappear when clicked the first time.
		}

		private void mnuAction_Click(object sender, System.EventArgs e)
		{
			//todo code for action items...
			if (sender == mnuNew || sender == mnuContextNew)
			{
			}
			else if (sender == mnuEdit || sender == mnuContextEdit)
			{
			}
			else if (sender == mnuDelete || sender == mnuContextDelete)
			{
			}
			else if (sender == mnuProperties || sender == mnuContextProperties)
			{
			}
			RefreshUI();
		}

		private void tbar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			//todo
			if (e.Button == tbtnRefresh)
			{
				RefreshUI();
			}
		}

		//still need to finish this.... need to pop up 
//		private void treelist_MouseUp(object sender, MouseEventArgs e)
//		{
////			if (!connected)
////				return;
////
////			Control senderControl = (Control)sender;
////			CurrentView view = findCurrentView();
////			bool showMenu = false;
////			if (e.Button == MouseButtons.Right)
////			{
////				//set the appropriate state for menus
////				mnuNew.Visible = false;
////				if (view == CurrentView.Executors)
////				{
////					mnuDel.Enabled = false; //should be enabled in the future...and need to check before deleting, if a thread is running on this Executor...
////					
////					//showMenu only if an Executor node is selected.
////					if (tv.SelectedNode != null && tv.SelectedNode is ExecutorTreeNode)
////						showMenu = true;
////				}
////				else if (view == CurrentView.Users)
////				{
////					mnuNew.Visible = true;
////					mnuEdit.Visible = true;
////					//showMenu only if an Usr/Grp node is selected.
////					if (tv.SelectedNode != null && (tv.SelectedNode is UserTreeNode || tv.SelectedNode is GroupTreeNode))
////						showMenu = true;
////				}
////				else if (view == CurrentView.Applications)
////				{
////					//showMenu only if an App/Thread node is selected.
////					if (tv.SelectedNode != null && (tv.SelectedNode is ApplicationTreeNode || tv.SelectedNode is ThreadTreeNode))
////						showMenu = true;
////				}
////
////				if (showMenu)
////					senderControl.ContextMenu.Show(senderControl, new Point(e.X, e.Y));
////			}
//		}

//		private void mnuRightClick_Click(object sender, System.EventArgs e)
//		{
//			if (!connected)
//				return;
//
//			CurrentView view = findCurrentView();
//			if (sender == mnuNew)
//			{
//				//view can only be users.
//				//find if we are adding a user/group and proceed
//			}
//			else if (sender == mnuEdit)
//			{
//				//view can only be users.
//				//find if we are editing a user/group and proceed
//			}
//			else if (sender == mnuDel)
//			{
//				//view can be user/executor/application
//			}
//			else if (sender == mnuProperties)
//			{
//				//view can be users/executors/applications
//				if (view == CurrentView.Executors)
//				{
//					//find out if any executor node is selected.
//					TreeNode node = tv.SelectedNode;
//					if (node!=null && node is ExecutorTreeNode)
//					{
//						ExecutorForm ef = new ExecutorForm();
//						ef.SetData((ExecutorTreeNode)node);
//						ef.ShowDialog(this);
//					}
//				}
//			}
		//		}

		private void tv_AfterExpand(object sender, TreeViewEventArgs e)
		{
			logger.Debug("After Expand");
			//if the node is a special parent node do stuff here to fill its child nodes.
			if (e.Node is SpecialParentNode)
			{
				SpecialParentNode node = (SpecialParentNode) e.Node;
				if (node.NodeType == SpecialParentNodeType.Users)
				{
					ShowUsers();
				}
				else if (node.NodeType == SpecialParentNodeType.Executors)
				{
					ShowExecutors();
				}
				else if (node.NodeType == SpecialParentNodeType.Applications)
				{
					ShowApplications();
				}
			}
		}

		private void lv_DoubleClick(object sender, EventArgs e)
		{
			//this should do the same thing as the tree-view expansion.
			//this event is raised only when an item is clicked
			try
			{
				if (lv.SelectedItems != null && lv.SelectedItems[0]!=null)
				{
					ListViewItem li = lv.SelectedItems[0];

					//we know that we would have stored the tree-node corresponding to it in the tag;
					TreeNode node = (TreeNode)li.Tag;
					if (node!=null)
					{
						//expand/collapse it. and then select it.
						node.Expand();
						node.EnsureVisible();
						tv.SelectedNode = node;
						RefreshUI();
					}
				}
			}
			catch (Exception ex)
			{
				logger.Debug(ex.ToString());
			} //ignore
		}

		private void ConsoleForm_Closing(object sender, CancelEventArgs e)
		{
			//just make sure the grid is disconnected.
			Disconnect();
		}
	}
}