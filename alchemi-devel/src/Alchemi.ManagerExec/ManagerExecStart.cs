using System;
using System.Windows.Forms;

namespace Alchemi.ManagerExec
{
	/// <summary>
	/// Summary description for ManagerExec.
	/// This class exists to start the Manager as a normal Windows Forms application.
	/// The ManagerMainForm can also be used as a service-controller for the ManagerService
	/// </summary>
	public class ManagerExec
	{
		public ManagerExec()
		{
		}

		[STAThread]
		static int Main() 
		{
			Application.EnableVisualStyles();
			ManagerMainForm f = new ManagerMainForm(false);
			Application.DoEvents();
			Application.Run(f);
			int returnCode = f.returnCode;
			f = null;
			return returnCode;
		}
	}
}
