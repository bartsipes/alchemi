#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   GThreadException.cs
 * Project      :   Alchemi.Core.Owner
 * Created on   :   2007
 * Copyright    :   Copyright © 2007 The University of Melbourne
 *                  This technology has been developed with the support of 
 *                  the Australian Research Council and the University of Melbourne
 *                  research grants as part of the Gridbus Project
 *                  within GRIDS Laboratory at the University of Melbourne, Australia.
 * Author       :   Dusty Candland (candland@users.sourceforge.net)
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
using System.Runtime.Serialization;

namespace Alchemi.Core.Owner
{
	[Serializable]
	public class GThreadException : Exception
	{
		protected GThreadException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		public GThreadException(string message, Exception innerException) : base(message, innerException)
		{
			throw new NotImplementedException("Use new GThreadException(Exception e) constructor.");
		}

		public GThreadException(string message) : base(message)
		{
			throw new NotImplementedException("Use new GThreadException(Exception e) constructor.");
		}

		public GThreadException()
		{
			throw new NotImplementedException("Use new GThreadException(Exception e) constructor.");
		}

		public GThreadException(Exception e)
			: base(BuildMessage(e))
		{
		}
		
		private static string BuildMessage(Exception e)
		{
			string message = string.Empty;
			if (e != null)
			    message = e.GetType().FullName + Environment.NewLine + ExpectionStack(e);
			return message;
		}
		
		private static string ExpectionStack(Exception e)
		{
			string stack = e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine;
			if (e.InnerException != null)
				stack += ExpectionStack(e.InnerException);
			return stack;
		}
	}
}
