using System;
using System.Windows.Forms;

namespace Alchemi.ExecutorExec
{
	/// <summary>
	/// Summary description for ExecutorExec.
	/// This class exists to start the Executor as a normal Windows Forms application.
	/// The ExecutorMainForm can also be used as a service-controller for the ExecutorService
	/// </summary>
	public class ExecutorExec
	{
		public ExecutorExec()
		{
		}

		[STAThread]
		static int Main() 
		{
			Application.EnableVisualStyles();
			ExecutorMainForm f = new ExecutorMainForm(false);
			Application.DoEvents();
			Application.Run(f);
			int returnCode = f.returnCode;
			f = null;
			return returnCode;
		}
	}
}
