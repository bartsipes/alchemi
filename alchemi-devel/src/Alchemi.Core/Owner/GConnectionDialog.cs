#region Alchemi copyright notice
/*
  Alchemi [.NET Grid Computing Framework]
  http://www.alchemi.net
  
  Copyright (c)  Akshay Luther (2002-2004) & Rajkumar Buyya (2003-to-date), 
  GRIDS Lab, The University of Melbourne, Australia.
  
  Maintained and Updated by: Krishna Nadiminti (2005-to-date)
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
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using Alchemi.Core.Owner;

namespace Alchemi.Core.Owner
{
	/// <summary>
	/// Represents the dialog box that is used to connect to the manager.
	/// This class is a wrapper around the GConnectionDialogForm
	/// </summary>
	public class GConnectionDialog : Component
	{
        GConnectionDialogForm form = new GConnectionDialogForm();
        
		/// <summary>
		/// Shows the dialog form
		/// </summary>
		/// <returns></returns>
        public System.Windows.Forms.DialogResult ShowDialog()
        {
            return form.ShowDialog();
        }
        
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Creates an instance of the GConnectionDialog
		/// </summary>
		/// <param name="container"></param>
		public GConnectionDialog(System.ComponentModel.IContainer container)
		{
			container.Add(this);
			InitializeComponent();
		}

		/// <summary>
		/// Creates an instance of the GConnectionDialog
		/// </summary>
		public GConnectionDialog()
		{
			InitializeComponent();
            form.ReadConfig();
		}

		/// <summary>
		/// Disposes the GConnectionDialog object and performs clean up operations.
		/// </summary>
		/// <param name="disposing"></param>
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

		/// <summary>
		/// Gets the GConnection object
		/// </summary>
        public GConnection Connection
        {
            get 
            {
                return form.Connection;
            }
        }

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion
	}
}
