#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
* Title         :  StorageMaintenanceForm.cs
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Alchemi.Core.Manager.Storage;

namespace Alchemi.Console.DataForms
{
    public partial class StorageMaintenanceForm : Form
    {
        private ConsoleNode m_console;

        public StorageMaintenanceForm(ConsoleNode console)
        {
            InitializeComponent();

            m_console = console;
        }

        private void btnPerformMaintenance_Click(object sender, EventArgs e)
        {
            StorageMaintenanceParameters maintenanceParameters = new StorageMaintenanceParameters();

            // TODO: put this on another thread so it does not block the UI
            m_console.Manager.Admon_PerformStorageMaintenance(m_console.Credentials, maintenanceParameters);
        }
    }
}