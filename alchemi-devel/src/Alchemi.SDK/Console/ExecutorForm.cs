#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	ExecutorForm.cs
* Project		:	Alchemi Console
* Created on	:	Sep 2005
* Copyright		:	Copyright © 2005 The University of Melbourne
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Alchemi.Console
{
	/// <summary>
	/// Summary description for ExecutorForm.
	/// </summary>
	public class ExecutorForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txHost;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txPort;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txCPUMax;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Label label4;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ExecutorForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

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
			this.label1 = new System.Windows.Forms.Label();
			this.txHost = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txPort = new System.Windows.Forms.TextBox();
			this.txCPUMax = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.TabIndex = 0;
			this.label1.Text = "Hostname";
			// 
			// txHost
			// 
			this.txHost.BackColor = System.Drawing.Color.White;
			this.txHost.Location = new System.Drawing.Point(104, 16);
			this.txHost.Name = "txHost";
			this.txHost.ReadOnly = true;
			this.txHost.Size = new System.Drawing.Size(176, 20);
			this.txHost.TabIndex = 1;
			this.txHost.Text = "txHost";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 48);
			this.label2.Name = "label2";
			this.label2.TabIndex = 2;
			this.label2.Text = "Port";
			// 
			// txPort
			// 
			this.txPort.BackColor = System.Drawing.Color.White;
			this.txPort.Location = new System.Drawing.Point(104, 48);
			this.txPort.Name = "txPort";
			this.txPort.ReadOnly = true;
			this.txPort.Size = new System.Drawing.Size(176, 20);
			this.txPort.TabIndex = 3;
			this.txPort.Text = "txPort";
			// 
			// txCPUMax
			// 
			this.txCPUMax.BackColor = System.Drawing.Color.White;
			this.txCPUMax.Location = new System.Drawing.Point(104, 80);
			this.txCPUMax.Name = "txCPUMax";
			this.txCPUMax.ReadOnly = true;
			this.txCPUMax.Size = new System.Drawing.Size(176, 20);
			this.txCPUMax.TabIndex = 5;
			this.txCPUMax.Text = "txCPUMax";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 80);
			this.label3.Name = "label3";
			this.label3.TabIndex = 4;
			this.label3.Text = "CPU Max.";
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(208, 240);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 6;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(32, 120);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(224, 88);
			this.label4.TabIndex = 7;
			this.label4.Text = "NEED TO PUT IN OTHER FIELDS HERE";
			// 
			// ExecutorForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.txCPUMax);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txPort);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txHost);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ExecutorForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ExecutorForm";
			this.ResumeLayout(false);

		}
		#endregion

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		public void SetData(ExecutorTreeNode exNode)
		{
			txHost.Text = exNode.host;
			txPort.Text = exNode.port;
			txCPUMax.Text = exNode.cpu_max;
			//TODO ...other fields here
		}
	}
}
