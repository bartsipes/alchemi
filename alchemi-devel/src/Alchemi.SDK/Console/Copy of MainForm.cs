using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Alchemi.Core;

namespace Console
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
        private Alchemi.Core.GConnectionDialog dlgConn;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.TreeView tvMain;
        private System.Windows.Forms.ImageList ilThreadState;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.ListView lvThreads;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.Timer tmListViewRefresh;

        private Console console = null;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.ContextMenu cmRoot;
        private System.Windows.Forms.MenuItem cmRegisterGrid;
        private TreeNode listNode = null;

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            RefreshUI();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
       	{
       		if( disposing )
       		{
       			if (components != null) 
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
                     // </summary>
                     private void InitializeComponent()
                     {
                         this.components = new System.ComponentModel.Container();
                         System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
                         this.dlgConn = new Alchemi.Core.GConnectionDialog(this.components);
                         this.cmRoot = new System.Windows.Forms.ContextMenu();
                         this.cmRegisterGrid = new System.Windows.Forms.MenuItem();
                         this.ilThreadState = new System.Windows.Forms.ImageList(this.components);
                         this.tvMain = new System.Windows.Forms.TreeView();
                         this.pnlLeft = new System.Windows.Forms.Panel();
                         this.mainMenu1 = new System.Windows.Forms.MainMenu();
                         this.lvThreads = new System.Windows.Forms.ListView();
                         this.pnlRight = new System.Windows.Forms.Panel();
                         this.splitter2 = new System.Windows.Forms.Splitter();
                         this.menuItem1 = new System.Windows.Forms.MenuItem();
                         this.menuItem2 = new System.Windows.Forms.MenuItem();
                         this.tmListViewRefresh = new System.Windows.Forms.Timer(this.components);
                         this.pnlBottom = new System.Windows.Forms.Panel();
                         this.pnlLeft.SuspendLayout();
                         this.pnlRight.SuspendLayout();
                         this.pnlBottom.SuspendLayout();
                         this.SuspendLayout();
                         // 
                         // cmRoot
                         // 
                         this.cmRoot.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                                this.cmRegisterGrid});
                         // 
                         // cmRegisterGrid
                         // 
                         this.cmRegisterGrid.Index = 0;
                         this.cmRegisterGrid.Text = "Register New Grid...";
                         this.cmRegisterGrid.Click += new System.EventHandler(this.cmRegisterGrid_Click);
                         // 
                         // ilThreadState
                         // 
                         this.ilThreadState.ImageSize = new System.Drawing.Size(16, 16);
                         this.ilThreadState.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilThreadState.ImageStream")));
                         this.ilThreadState.TransparentColor = System.Drawing.Color.Transparent;
                         // 
                         // tvMain
                         // 
                         this.tvMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                         this.tvMain.ContextMenu = this.cmRoot;
                         this.tvMain.Dock = System.Windows.Forms.DockStyle.Fill;
                         this.tvMain.ForeColor = System.Drawing.SystemColors.WindowText;
                         this.tvMain.ImageIndex = -1;
                         this.tvMain.Location = new System.Drawing.Point(0, 0);
                         this.tvMain.Name = "tvMain";
                         this.tvMain.SelectedImageIndex = -1;
                         this.tvMain.Size = new System.Drawing.Size(288, 450);
                         this.tvMain.TabIndex = 4;
                         this.tvMain.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvMain_AfterSelect);
                         this.tvMain.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvMain_BeforeSelect);
                         // 
                         // pnlLeft
                         // 
                         this.pnlLeft.BackColor = System.Drawing.SystemColors.Control;
                         this.pnlLeft.Controls.Add(this.tvMain);
                         this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
                         this.pnlLeft.Location = new System.Drawing.Point(10, 10);
                         this.pnlLeft.Name = "pnlLeft";
                         this.pnlLeft.Size = new System.Drawing.Size(288, 450);
                         this.pnlLeft.TabIndex = 6;
                         // 
                         // mainMenu1
                         // 
                         this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                                   this.menuItem1});
                         this.mainMenu1.RightToLeft = System.Windows.Forms.RightToLeft.No;
                         // 
                         // lvThreads
                         // 
                         this.lvThreads.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                         this.lvThreads.Dock = System.Windows.Forms.DockStyle.Fill;
                         this.lvThreads.LargeImageList = this.ilThreadState;
                         this.lvThreads.Location = new System.Drawing.Point(0, 0);
                         this.lvThreads.Name = "lvThreads";
                         this.lvThreads.Size = new System.Drawing.Size(628, 450);
                         this.lvThreads.SmallImageList = this.ilThreadState;
                         this.lvThreads.StateImageList = this.ilThreadState;
                         this.lvThreads.TabIndex = 0;
                         this.lvThreads.View = System.Windows.Forms.View.SmallIcon;
                         // 
                         // pnlRight
                         // 
                         this.pnlRight.BackColor = System.Drawing.SystemColors.Control;
                         this.pnlRight.Controls.Add(this.pnlBottom);
                         this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
                         this.pnlRight.Location = new System.Drawing.Point(298, 10);
                         this.pnlRight.Name = "pnlRight";
                         this.pnlRight.Size = new System.Drawing.Size(628, 450);
                         this.pnlRight.TabIndex = 8;
                         this.pnlRight.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlRight_Paint);
                         // 
                         // splitter2
                         // 
                         this.splitter2.BackColor = System.Drawing.SystemColors.Control;
                         this.splitter2.Location = new System.Drawing.Point(298, 10);
                         this.splitter2.Name = "splitter2";
                         this.splitter2.RightToLeft = System.Windows.Forms.RightToLeft.No;
                         this.splitter2.Size = new System.Drawing.Size(3, 450);
                         this.splitter2.TabIndex = 10;
                         this.splitter2.TabStop = false;
                         // 
                         // menuItem1
                         // 
                         this.menuItem1.Index = 0;
                         this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                                   this.menuItem2});
                         this.menuItem1.Text = "File";
                         // 
                         // menuItem2
                         // 
                         this.menuItem2.Index = 0;
                         this.menuItem2.Text = "Register New Grid...";
                         this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
                         // 
                         // tmListViewRefresh
                         // 
                         this.tmListViewRefresh.Interval = 1000;
                         this.tmListViewRefresh.Tick += new System.EventHandler(this.tmListViewRefresh_Tick);
                         // 
                         // pnlBottom
                         // 
                         this.pnlBottom.Controls.Add(this.lvThreads);
                         this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
                         this.pnlBottom.Location = new System.Drawing.Point(0, 0);
                         this.pnlBottom.Name = "pnlBottom";
                         this.pnlBottom.Size = new System.Drawing.Size(628, 450);
                         this.pnlBottom.TabIndex = 1;
                         // 
                         // MainForm
                         // 
                         this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
                         this.ClientSize = new System.Drawing.Size(936, 470);
                         this.Controls.Add(this.splitter2);
                         this.Controls.Add(this.pnlRight);
                         this.Controls.Add(this.pnlLeft);
                         this.DockPadding.All = 10;
                         this.Menu = this.mainMenu1;
                         this.Name = "MainForm";
                         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                         this.Text = "Alchemi Console";
                         this.Load += new System.EventHandler(this.MainForm_Load);
                         this.pnlLeft.ResumeLayout(false);
                         this.pnlRight.ResumeLayout(false);
                         this.pnlBottom.ResumeLayout(false);
                         this.ResumeLayout(false);

                     }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
        [STAThread]
        static void Main() 
        {
            Application.Run(new MainForm());
            Application.EnableVisualStyles();
        }

        private void RefreshUI()
        {
            bool connected = console == null ? false : true;
        }


        private void MainForm_Load(object sender, System.EventArgs e)
        {
            // create root node
            TreeNode tn = new TreeNode("Alchemi Grids");
            tn.Tag = new NodeInfo(NodeType.Root);
            tvMain.Nodes.Add(tn);
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            tvMain.Visible = !tvMain.Visible;
        }

        private void tvMain_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            PopulateNode(e.Node, false);

            // change current listview node
            NodeInfo info = (NodeInfo) e.Node.Tag;
            switch (info.Type)
            {
                case NodeType.Application:
                    listNode = e.Node;
                    RefreshListView();
                    tmListViewRefresh.Enabled = true;
                    break;
            }
        }

        private void PopulateNode(TreeNode node, bool forcePopulate)
        {
            NodeInfo info = (NodeInfo) node.Tag;
            
            switch (info.Type)
            {
                case NodeType.ApplicationList:
                    if (forcePopulate | (!info.Populated))
                    {
                        DataSet apps = console.Manager.Owner_GetLiveApplicationList(console.Credentials);
                        info.Data = apps;
                        info.Populated = true;

                        // populate treeview
                        foreach (DataRow app in ((DataSet) info.Data).Tables[0].Rows)
                        {
                            string appId = app["application_id"].ToString();
                            TreeNode t = new TreeNode(appId);
                            t.Tag = new NodeInfo(NodeType.Application);
                            node.Nodes.Add(t);
                        }
                    }
                    break;

                case NodeType.Application:
                    if (forcePopulate | (!info.Populated))
                    {
                        DataSet threads = console.Manager.Owner_GetThreadList(console.Credentials, node.Text);
                        info.Data = threads;
                        info.Populated = true;

                        // populate treeview
                        node.Nodes.Clear();
                        foreach (DataRow thread in ((DataSet) info.Data).Tables[0].Rows)
                        {
                            string threadId = thread["thread_id"].ToString();
                            TreeNode t = new TreeNode(threadId);
                            NodeInfo i = new NodeInfo(NodeType.Thread);
                            i.Data = thread;
                            t.Tag = info;
                            node.Nodes.Add(t);
                        }
                    }
                    break;

                case NodeType.ExecutorList:
                    if (forcePopulate | (!info.Populated))
                    {
                        node.Nodes.Add("(executor list)");
                    }
                    break;
            }
        }

        private void RefreshListView()
        {
            NodeInfo info = (NodeInfo) listNode.Tag;
            
            switch (info.Type)
            {
                case NodeType.Application:
                    //lvThreads.Clear();

                    if (lvThreads.Items.Count == 0)
                    {
                        foreach (DataRow thread in ((DataSet) info.Data).Tables[0].Rows)
                        {
                            string threadId = thread["thread_id"].ToString();
                            ListViewItem lvi = new ListViewItem(threadId);
                            lvi.ImageIndex = int.Parse(thread["state"].ToString());
                            lvThreads.Items.Add(lvi);
                        }
                    }
                    else
                    {
                        foreach (ListViewItem lvi in lvThreads.Items)
                        {
                            string threadId = lvi.Text;
                            DataSet ds = (DataSet) info.Data;
                            ds.Tables[0].PrimaryKey = new DataColumn[] { ds.Tables[0].Columns["thread_id"] };
                            DataRow threadRow = ds.Tables[0].Rows.Find(threadId);
                            lvi.ImageIndex = int.Parse(threadRow["state"].ToString());
                        }
                    }

                    break;
            }
        }

        private void tmListViewRefresh_Tick(object sender, System.EventArgs e)
        {
            PopulateNode(listNode, true);
            RefreshListView();
        }

        private void pnlRight_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
        
        }

        private void panel2_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
        
        }

        private void menuItem2_Click(object sender, System.EventArgs e)
        {

        }

        private void tvMain_BeforeSelect(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
        {
        
        }

        private void cmRegisterGrid_Click(object sender, System.EventArgs e)
        {
        
        }


    }

    public enum NodeType
    {
        Root,
        Grid,
        ApplicationList,
        Application,
        Thread,
        ExecutorList,
        Executor
    }
    
    public class NodeInfo
    {
        public NodeType Type;
        public bool Populated = false;
        public object Data;

        //public string AppId;
        //public int ThreadId;
        //public string ExecutorId;

        public NodeInfo(NodeType type)
        {
            Type = type;
        }
    }
}
