/*
 * Title        :  AlchemiJobHandle.java
 * Package      :  org.gridbus.alchemi.client
 * Project      :  AlchemiJavaAPI
 * Description	:  
 * Created on   :  4/08/2005
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
public class AlchemiJobHandle {

	private String managerURL;
	private String taskID;
	private String jobID;
	
	private String handle;
	
	/**
	 * @return Returns the handle.
	 */
	public String getHandle() {
		return handle;
	}
	/**
	 * @param handle The handle to set.
	 */
	public void setHandle(String handle) {
		this.handle = handle;
	}
	/**
	 * @return Returns the jobID.
	 */
	public String getJobID() {
		return jobID;
	}
	/**
	 * @param jobID The jobID to set.
	 */
	public void setJobID(String jobID) {
		this.jobID = jobID;
	}
	/**
	 * @return Returns the managerURL.
	 */
	public String getManagerURL() {
		return managerURL;
	}
	/**
	 * @param managerURL The managerURL to set.
	 */
	public void setManagerURL(String managerURL) {
		this.managerURL = managerURL;
	}
	/**
	 * @return Returns the taskID.
	 */
	public String getTaskID() {
		return taskID;
	}
	/**
	 * @param taskID The taskID to set.
	 */
	public void setTaskID(String taskID) {
		this.taskID = taskID;
	}
	
	/**
	 * Represents an Alchemi job handle.
	 * An alchemi job handle is of the form:
	 * http://crossPlatformManagerHost:port/<path-to-webservice>/taskID:jobID
	 */
	public AlchemiJobHandle(String handle) {
		super();
		this.handle = handle;
		parseHandle();
	}

	/**
	 * Parses the given handle.
	 * assumes the handle is of the form:
	 * http://crossPlatformManagerHost:port/<path-to-webservice>/taskID:jobID
	 */
	private void parseHandle(){
		//get the taskID and jobID here.
		String[] handleParts;
		String[] taskJobID;
		
		if (handle!=null){
			handleParts = handle.split("/");
			if (handleParts!=null && handleParts.length>0){
				taskJobID = handleParts[handleParts.length-1].split(":");
				//now the taskJobID has the bit taskID:jobID
				if (taskJobID!=null && taskJobID.length>0){
					this.taskID = taskJobID[0];
					this.jobID = taskJobID[1];
				}
				//set the managerURL
				this.managerURL = handle.substring(0,handle.lastIndexOf('/'));
			}
		}
	}
}
