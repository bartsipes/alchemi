#region Alchemi copyright notice
/*
  Alchemi [.NET Grid Computing Framework]
  Copyright (c) 2002-2004 Akshay Luther
  http://www.alchemi.net
---------------------------------------------------------------------------

  This program is free software; you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation; either version 2 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program; if not, write to the Free Software
  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/
#endregion

using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;
using Alchemi.Core;
using Alchemi.Core.Executor;

namespace Alchemi.ScreenSaverExec
{
    public class ScreenSaverForm : System.Windows.Forms.Form
    {
        private System.ComponentModel.IContainer components;
        private Point MouseXY;
        private int ScreenNumber;
        private GExecutor Executor;

        public ScreenSaverForm(int scrn)
        {
            InitializeComponent();
            ScreenNumber = scrn;
        }

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

        private void ScreenSaverForm_Load(object sender, System.EventArgs e)
        {
            this.Bounds = Screen.AllScreens[ScreenNumber].Bounds;
            Cursor.Hide();
            TopMost = true;

            try
            {
                // get executor configuration
                RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Alchemi\\Executor");
                Configuration config = Configuration.GetConfiguration(key.GetValue("InstallLocation").ToString());

                // get reference to local executor
                Executor = (GExecutor) GNode.GetRemoteRef(new RemoteEndPoint(
                    "localhost",
                    config.OwnPort,
                    RemotingMechanism.TcpBinary
                    ));

                // start non-dedicated executing
                Executor.PingExecutor();
                Executor.StartNonDedicatedExecuting(5000);
            }
            catch {}
        }

        private void OnMouseEvent(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!MouseXY.IsEmpty)
            {
                if (MouseXY != new Point(e.X, e.Y))
                    Close();
                if (e.Clicks > 0)
                    Close();
            }
            MouseXY = new Point(e.X, e.Y);
        }
		
        private void ScreenSaverForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Close();
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // ScreenSaverForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(480, 273);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ScreenSaverForm";
            this.Text = "ScreenSaver";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ScreenSaverForm_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseEvent);
            this.Load += new System.EventHandler(this.ScreenSaverForm_Load);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseEvent);

        }
        #endregion

    }
}
