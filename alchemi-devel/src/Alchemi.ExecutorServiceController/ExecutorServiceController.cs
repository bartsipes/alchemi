using System;
using System.Windows.Forms;
using Alchemi.ExecutorExec;

namespace Alchemi.ExecutorService
{
	/// <summary>
	/// Summary description for ExecutorServiceController.
	/// This class uses the ExecutorMainForm which can also acts as the service controller for the ExecutorService.
	/// </summary>
	public class ExecutorServiceController
	{

		public ExecutorServiceController()
		{
		}

		[STAThread]
		static void Main() 
		{
			Application.Run(new ExecutorMainForm(true));
		}
	}
}
