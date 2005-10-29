#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	GConnectionDialog.cs
* Project		:	Alchemi Core
* Created on	:	2003
* Copyright		:	Copyright � 2005 The University of Melbourne
*					This technology has been developed with the support of 
*					the Australian Research Council and the University of Melbourne
*					research grants as part of the Gridbus Project
*					within GRIDS Laboratory at the University of Melbourne, Australia.
* Author         :  Akshay Luther (akshayl@cs.mu.oz.au) and Rajkumar Buyya (raj@cs.mu.oz.au)
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
