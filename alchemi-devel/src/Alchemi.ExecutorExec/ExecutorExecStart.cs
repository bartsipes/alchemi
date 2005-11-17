#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	ExecutorExecStart.cs
* Project		:	Alchemi Core
* Created on	:	August 2005
* Copyright		:	Copyright © 2005 The University of Melbourne
*					This technology has been developed with the support of 
*					the Australian Research Council and the University of Melbourne
*					research grants as part of the Gridbus Project
*					within GRIDS Laboratory at the University of Melbourne, Australia.
* Author         :  Krishna Nadiminti (kna@csse.unimelb.edu.au) and Rajkumar Buyya (raj@csse.unimelb.edu.au)
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
