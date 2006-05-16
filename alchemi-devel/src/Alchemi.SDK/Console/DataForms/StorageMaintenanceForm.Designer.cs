#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
* Title         :  StorageMaintenanceForm.Designer.cs
* Project       :  Alchemi.Console.DataForms
* Created on    :  05 May 2006
* Copyright     :  Copyright © 2006 Tibor Biro (tb@tbiro.com)
* Author        :  Tibor Biro (tb@tbiro.com)
* License       :  GPL
*                    This program is free software; you can redistribute it and/or
*                    modify it under the terms of the GNU General Public
*                    License as published by the Free Software Foundation;
*                    See the GNU General Public License
*                    (http://www.gnu.org/copyleft/gpl.html) for more details.
*
*/
#endregion

namespace Alchemi.Console.DataForms
{
    partial class StorageMaintenanceForm
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
            this.btnPerformMaintenance = new System.Windows.Forms.Button();
            this.chkRemoveAllExecutors = new System.Windows.Forms.CheckBox();
            this.chkRemoveAllApplications = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnPerformMaintenance
            // 
            this.btnPerformMaintenance.Location = new System.Drawing.Point(126, 289);
            this.btnPerformMaintenance.Name = "btnPerformMaintenance";
            this.btnPerformMaintenance.Size = new System.Drawing.Size(164, 23);
            this.btnPerformMaintenance.TabIndex = 0;
            this.btnPerformMaintenance.Text = "Perform Maintenance";
            this.btnPerformMaintenance.UseVisualStyleBackColor = true;
            this.btnPerformMaintenance.Click += new System.EventHandler(this.btnPerformMaintenance_Click);
            // 
            // chkRemoveAllExecutors
            // 
            this.chkRemoveAllExecutors.AutoSize = true;
            this.chkRemoveAllExecutors.Location = new System.Drawing.Point(50, 70);
            this.chkRemoveAllExecutors.Name = "chkRemoveAllExecutors";
            this.chkRemoveAllExecutors.Size = new System.Drawing.Size(130, 17);
            this.chkRemoveAllExecutors.TabIndex = 1;
            this.chkRemoveAllExecutors.Text = "Remove All Executors";
            this.chkRemoveAllExecutors.UseVisualStyleBackColor = true;
            // 
            // chkRemoveAllApplications
            // 
            this.chkRemoveAllApplications.AutoSize = true;
            this.chkRemoveAllApplications.Location = new System.Drawing.Point(50, 24);
            this.chkRemoveAllApplications.Name = "chkRemoveAllApplications";
            this.chkRemoveAllApplications.Size = new System.Drawing.Size(140, 17);
            this.chkRemoveAllApplications.TabIndex = 2;
            this.chkRemoveAllApplications.Text = "Remove All Applications";
            this.chkRemoveAllApplications.UseVisualStyleBackColor = true;
            // 
            // StorageMaintenanceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 341);
            this.Controls.Add(this.chkRemoveAllApplications);
            this.Controls.Add(this.chkRemoveAllExecutors);
            this.Controls.Add(this.btnPerformMaintenance);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StorageMaintenanceForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Storage Maintenance Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPerformMaintenance;
        private System.Windows.Forms.CheckBox chkRemoveAllExecutors;
        private System.Windows.Forms.CheckBox chkRemoveAllApplications;
    }
}