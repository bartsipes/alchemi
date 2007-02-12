#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   GConnectionDialog.cs
 * Project      :   Alchemi.Core.Owner
 * Created on   :   2003
 * Copyright    :   Copyright © 2006 The University of Melbourne
 *                  This technology has been developed with the support of 
 *                  the Australian Research Council and the University of Melbourne
 *                  research grants as part of the Gridbus Project
 *                  within GRIDS Laboratory at the University of Melbourne, Australia.
 * Author       :   Akshay Luther (akshayl@csse.unimelb.edu.au)
 *                  Rajkumar Buyya (raj@csse.unimelb.edu.au)
 *                  Krishna Nadiminti (kna@csse.unimelb.edu.au)
 * License      :   GPL
 *                  This program is free software; you can redistribute it and/or 
 *                  modify it under the terms of the GNU General Public
 *                  License as published by the Free Software Foundation;
 *                  See the GNU General Public License 
 *                  (http://www.gnu.org/copyleft/gpl.html) for more details.
 *
 */
#endregion

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace Alchemi.Core.Owner
{
    /// <summary>
    /// Represents the dialog box that is used to connect to the manager.
    /// This class is a wrapper around the GConnectionDialogForm
    /// </summary>
    public partial class GConnectionDialog : Component
    {
        private GConnectionDialogForm _form = new GConnectionDialogForm();

        public GConnectionDialog()
        {
            InitializeComponent();
            _form.ReadConfig();
        }

        public GConnectionDialog(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }


        /// <summary>
        /// Shows the dialog form
        /// </summary>
        /// <returns></returns>
        public DialogResult ShowDialog()
        {
            return _form.ShowDialog();
        }

        /// <summary>
        /// Gets the GConnection object
        /// </summary>
        public GConnection Connection
        {
            get
            {                
                return _form.Connection;
            }
        }
    }
}
