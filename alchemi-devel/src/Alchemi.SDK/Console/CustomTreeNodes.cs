#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	CustomTreeNodes.cs
* Project		:	Alchemi Console
* Created on	:	Sep 2005
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

namespace Alchemi.Console
{

	public enum SpecialParentNodeType
	{
		None,
		Users,
		Executors,
		Applications
	}

	//a dummy node is there just for having a '+' for its parent. It doesnt have any text.
	public class DummyTreeNode : System.Windows.Forms.TreeNode
	{

		public DummyTreeNode(System.String text) : base("")
		{
		}

		public DummyTreeNode (System.String text, System.Int32 imageIndex,  System.Int32 selectedImageIndex ) : base("", imageIndex, selectedImageIndex)
		{
		}

	}

	public class SpecialParentNode : System.Windows.Forms.TreeNode
	{
		public SpecialParentNodeType NodeType = SpecialParentNodeType.None;

		public SpecialParentNode(System.String text) : base(text)
		{
		}

		public SpecialParentNode (System.String text, System.Int32 imageIndex,  System.Int32 selectedImageIndex ) : base(text, imageIndex, selectedImageIndex)
		{
		}

	}

	public class ApplicationTreeNode : System.Windows.Forms.TreeNode
	{

		public string application_id;
		public Alchemi.Core.Owner.ApplicationState state;
		public string time_created;
		public bool is_primary;
		public string usr_name;
		public string application_name;
		public string time_completed;

		public int num_threads;

		//application_id, [state], time_created, is_primary, usr_name, application_name, time_completed
		//num_threads
		public ApplicationTreeNode(System.String text) : base(text)
		{
		}

		public ApplicationTreeNode (System.String text, System.Int32 imageIndex,  System.Int32 selectedImageIndex ) : base(text, imageIndex, selectedImageIndex)
		{
		}

	}

	public class ThreadTreeNode : System.Windows.Forms.TreeNode
	{
		public string thread_id;
		public Alchemi.Core.Owner.ThreadState state;
		public string time_started;
		public string time_finished;
		public string executor_id;
		public int priority;
		public bool failed;
		public string appId;

		//thread_id, state, time_started, time_finished, executor_id, priority, failed, appId

		public ThreadTreeNode(System.String text) : base(text)
		{
		}

		public ThreadTreeNode (System.String text, System.Int32 imageIndex,  System.Int32 selectedImageIndex ) : base(text, imageIndex, selectedImageIndex)
		{
		}
	}

	public class ExecutorTreeNode : System.Windows.Forms.TreeNode
	{

		public string executor_id;
		public string host;
		public string port;
		public string usr_name;
		public bool is_connected;
		public bool is_dedicated;
		public string cpu_max;
		public string cpu_totalusage;
		
		//executor_id, host, port, usr_name, is_connected, is_dedicated, cpu_max, 
		//convert(varchar, cpu_totalusage * cpu_max / (3600 * 1000)) as cpu_totalusage

		public ExecutorTreeNode(System.String text) : base(text)
		{
		}

		public ExecutorTreeNode (System.String text, System.Int32 imageIndex,  System.Int32 selectedImageIndex ) : base(text, imageIndex, selectedImageIndex)
		{
		}

	}

	public class UserTreeNode : System.Windows.Forms.TreeNode
	{
		public string usr_name;
		public string grp_id;
		
		//usr_name, password, grp.grp_id


		public UserTreeNode(System.String text) : base(text)
		{
		}

		public UserTreeNode (System.String text, System.Int32 imageIndex,  System.Int32 selectedImageIndex ) : base(text, imageIndex, selectedImageIndex)
		{
		}

	}

	public class GroupTreeNode : System.Windows.Forms.TreeNode
	{
		public string grp_id;
		public string grp_name;

		//grp_id, grp_name

		public GroupTreeNode(System.String text) : base(text)
		{
		}

		public GroupTreeNode (System.String text, System.Int32 imageIndex,  System.Int32 selectedImageIndex ) : base(text, imageIndex, selectedImageIndex)
		{
		}

	}
}
