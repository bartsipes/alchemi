/*
 * Title		:	GJob.java
 * Package		:	org.gridbus.alchemi
 * Project		:	Alchemi Java client API
 * Description	:	 
 * Created on	:	Aug 1, 2005
 * Author		:	Krishna Nadiminti (kna@cs.mu.oz.au)
 * Copyright	:	(c) 2005, Grid Computing and Distributed Systems Laboratory, 
 *					Dept. of Computer Science and Software Engineering,
 *					University of Melbourne, Australia.
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
 */
public class GJob {
	//to assign Ids to jobs
	private static long currentJob = 0;

    /** Unknown status */
    public static final int Unknown = -1;
    
    /** Ready to execute */
    public static final int Ready = 0;
    
    /** Executor has been assigned */ 
    public static final int Scheduled = 1;
    
    /** Executor is executing the job */
    public static final int Started = 2; 
    
    /** Executor has returned the finished thread */
    public static final int Finished = 3;
    
    /** Returned to owner OR aborted */
    public static final int Dead = 4; 
    
    private String handle = null;
  
    private FileDependencyCollection inputfiles;
    private FileDependencyCollection outputfiles;
    private String runCommand;
    
    private long jobID;
    private AlchemiJobHandle jobHandle = null;
    
    private int status;

	private Exception executionException;
	    
    /**
     * Represents a Job on the Alchemi Manager
     */
    public GJob() {
        super();
        inputfiles = new FileDependencyCollection();
        outputfiles = new FileDependencyCollection();
        jobID = currentJob;
        currentJob++;
    }

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
	public long getJobID() {
		return jobID;
	}
	/**
	 * @return Returns the inputfiles.
	 */
	public FileDependencyCollection getInputfiles() {
		return inputfiles;
	}
	/**
	 * @param inputfiles The inputfiles to set.
	 */
	public void setInputfiles(FileDependencyCollection inputfiles) {
		this.inputfiles = inputfiles;
	}
	/**
	 * @return Returns the outputfiles.
	 */
	public FileDependencyCollection getOutputfiles() {
		return outputfiles;
	}
	/**
	 * @param outputfiles The outputfiles to set.
	 */
	public void setOutputfiles(FileDependencyCollection outputfiles) {
		this.outputfiles = outputfiles;
	}
	/**
	 * @return Returns the runCommand.
	 */
	public String getRunCommand() {
		return runCommand;
	}
	/**
	 * @param runCommand The runCommand to set.
	 */
	public void setRunCommand(String runCommand) {
		this.runCommand = runCommand;
	}
    /**
	 * @param status
	 */
	protected void setStatus(int status) {
		this.status = status;
	}
	/**
	 * @return Returns the status.
	 */
	public int getStatus() {
		return status;
	}
	
	public Exception getExecutionException(){
		return executionException;
	}
	
	/**
	 * @param executionException The executionException to set.
	 */
	protected void setExecutionException(Exception executionException) {
		this.executionException = executionException;
	}
}
