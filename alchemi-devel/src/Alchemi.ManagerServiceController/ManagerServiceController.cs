using System;
using System.Windows.Forms;
using Alchemi.ManagerExec;

namespace Alchemi.ManagerService
{
	/// <summary>
	/// Summary description for ManagerServiceController.
	/// This class uses the ManagerMainForm which can also acts as the service controller for the ManagerService.
	/// </summary>
	public class ManagerServiceController
	{

		public ManagerServiceController()
		{
			try
			{
				System.IO.Directory.CreateDirectory("dat");
			}
			catch {}
		}

		[STAThread]
		static void Main() 
		{
			Application.Run(new ManagerMainForm(true));
		}
	}
}
