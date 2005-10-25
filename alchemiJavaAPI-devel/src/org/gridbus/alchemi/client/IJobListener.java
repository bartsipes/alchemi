/*
 * Title        :  IJobListener.java
 * Package      :  org.gridbus.alchemi.client
 * Project      :  AlchemiJavaAPI
 * Description	:  
 * Created on   :  5/08/2005
 * Author		:  Krishna Nadiminti (kna@cs.mu.oz.au)
 * Copyright    :  (c) 2005, Grid Computing and Distributed Systems Laboratory, 
 * 					   Dept. of Computer Science and Software Engineering,
 * 					   University of Melbourne, Australia.
 * 
 * This program is free software; you can redistribute it and/or modify it under
 * the terms of the GNU General Public License as published by the Free Software
 * Foundation; either version 2 of the License, or (at your option) any later  version.
 * See the GNU General Public License (http://www.gnu.org/copyleft/gpl.html)for more details.
 * 
 */

package org.gridbus.alchemi.client;

/**
 * @author krishna
 *
 */
public interface IJobListener {
	
	public void JobFinished(GJob job);
	public void JobFailed(GJob job, Exception ex);
	public void ApplicationFinished();
}
