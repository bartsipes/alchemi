/*
 * Title        :  GApplication.java
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

import org.apache.log4j.Logger;

import java.net.URL;
import java.util.Hashtable;
import java.util.Iterator;
import java.util.Vector;

import org.gridbus.alchemi.client.stubs.CrossPlatformManager;
import org.gridbus.alchemi.client.stubs.CrossPlatformManagerLocator;
import org.gridbus.alchemi.client.stubs.CrossPlatformManagerSoap;
import org.gridbus.alchemi.client.util.AlchemiXmlUtil;

/**
 * @author krishna
 *
 */
public class GApplication {
	/**
	 * Logger for this class
	 */
	private final Logger logger = Logger.getLogger(GApplication.class);
	
	public static final int Status_Unknown = -1;
	public static final int Status_AwaitingManifest = 0;
	public static final int Status_Ready = 1;
	public static final int Status_Stopped = 2;

	private CrossPlatformManager cm = null;
	private CrossPlatformManagerSoap manager = null;
	
	private GJobCollection jobs = null;
	private FileDependencyCollection manifest = null;
	private SecurityCredentials securityCredentials = null;
	
	private String managerURL = null;
	private String taskID = null;
	
	private Vector jobListeners = null;
	
	private JobMonitor monitor = null;
	
	private boolean finished = false;
	
	private String localWorkingDirectory = "";
	
	/**
	 * Creates a new instance of the Grid application on the given manager.
	 * 
	 * @param managerURL
	 * @param securityCredentials
	 * @throws Exception - if the manager cannot be contacted.
	 * 
	 */
	public GApplication(String managerURL, SecurityCredentials securityCredentials) throws Exception {
		super();
		this.managerURL = managerURL;
		this.securityCredentials = securityCredentials;
		getManagerClient(managerURL); //initialises the cm, manager
		monitor = new JobMonitor();
		jobListeners = new Vector();
		jobs = new GJobCollection();
	}
	
	private void getManagerClient(String managerURL) throws Exception{
		//makes sure the manager stubs are initted
		if (cm==null || manager==null){
			cm = new CrossPlatformManagerLocator();
			manager = cm.getCrossPlatformManagerSoap(new URL(managerURL)); //set the proper url here
		}
	}

	/**
	 * Submit this application to the given Alchemi Manager Webservice.
	 * 
	 * @throws Exception
	 */
	public void start() throws Exception{
		String taskXml=null;
		
		getManagerClient(managerURL);
		
		taskXml = AlchemiXmlUtil.getTaskXml(this);
		
		logger.debug("Generating XML description for GApplication...");
		//logger.debug("XML Description:\n"+taskXml);
		
		if (taskXml==null || taskXml.trim().equals("")){
			throw new Exception("Invalid Job Description.");
		}
		
		logger.debug("Submitting application to Alchemi manager service at : "+managerURL);
		this.taskID = manager.submitTask(securityCredentials.getUsername(),securityCredentials.getPassword(),taskXml);
		logger.debug("Application submitted successfully. ApplicationID="+taskID);
		//setJobHandle for each job
		for (int i=0;i<this.jobs.size();i++){
			GJob j = jobs.get(i);
			String handle = managerURL + "/" + taskID + ":" + j.getJobID();
			j.setHandle(handle);
			logger.debug("Setting job handle for job:"+i+", handle = "+handle);
		}
		
		//start the monitoring thread
		Thread jobMon = new Thread(monitor);
		jobMon.start();
		logger.debug("Started Job monitor thread");
	}
	
	public boolean isFinished(){
		return finished;
	}
	
	/**
	 * Abors this application.
	 * @throws Exception
	 */
	public void stop() throws Exception{
		monitor.stop();
	    manager.abortTask(securityCredentials.getUsername(),securityCredentials.getPassword(),taskID);	    
	}

	
	private void getFinishedJobs() throws Exception{
		logger.debug("Getting finished jobs so far...");
		String resultXml = manager.getFinishedJobs(securityCredentials.getUsername(),securityCredentials.getPassword(),taskID);
		logger.debug("Server returned response: \n"+resultXml);
		AlchemiXmlUtil.parseXmlOutput(resultXml,localWorkingDirectory);
	}

	/**
	 * @return Returns the jobs.
	 */
	public GJobCollection getJobs() {
		return jobs;
	}
	/**
	 * @param jobs The jobs to set.
	 */
	public void setJobs(GJobCollection jobs) {
		this.jobs = jobs;
	}
	/**
	 * @return Returns the manifest.
	 */
	public FileDependencyCollection getManifest() {
		return manifest;
	}
	/**
	 * @param manifest The manifest to set.
	 */
	public void setManifest(FileDependencyCollection manifest) {
		this.manifest = manifest;
	}

	/**
	 * Queries the status of the job with the given handle.
	 * @param handle
	 * @return
	 * @throws Exception
	 */
	public synchronized int queryJobStatus(String handle) throws Exception{
		int status = GJob.Unknown;
		//the handle is assumed to be of the form:
		//<managerURL>/taskID:jobID
	    AlchemiJobHandle jobHandle = new AlchemiJobHandle(handle);
	    String taskID = jobHandle.getTaskID();
		Integer jobID = new Integer(jobHandle.getJobID());
		
		logger.debug("Querying status for job:"+handle);
		status = manager.getJobState(securityCredentials.getUsername(),securityCredentials.getPassword(),taskID,jobID.intValue());
		GJob j = getJobById(jobID.intValue());
		if (j!=null)
			j.setStatus(status);
		logger.debug("Job "+handle+" status :"+status);
		return status;
	}
	
	public synchronized int queryApplicationStatus() throws Exception{
		int status = GApplication.Status_Unknown;
		logger.debug("Querying application status...");
		status = manager.getApplicationState(securityCredentials.getUsername(),securityCredentials.getPassword(),this.taskID);
		logger.debug("Application status: "+status);
		return status;
	}
	
	private GJob getJobById(int jobID){
		GJob j = null;
		if (this.jobs==null || this.jobs.size()==0)
			return null;
		
		for (int i=0;i<this.jobs.size();i++){
			GJob temp = this.jobs.get(i);
			if (temp!=null && temp.getJobID()==jobID){
				j = temp;
				break;
			}
		}
		
		return j;
	}
	
	public void addJob(GJob job){
		jobs.add(job);
	}
	
	/**
	 * Aborts the job with the given handle.
	 * @param handle
	 * @throws Exception
	 */
	public void abortJob(String handle) throws Exception{
		//assumes the handle is already set.
		//the handle will have the managerURL.
		//the handle is assumed to be of the form:
		//<managerURL>/taskID:jobID
	    AlchemiJobHandle jobHandle = new AlchemiJobHandle(handle);
		String taskID = jobHandle.getTaskID();
		Integer jobID = new Integer(jobHandle.getJobID());
		String managerURL = jobHandle.getManagerURL();
		logger.debug("Aborting job "+handle+", managerURL="+managerURL+", taskID="+taskID+", jobID="+jobID);
		manager.abortJob(securityCredentials.getUsername(),securityCredentials.getPassword(),taskID,jobID.intValue());
	}
	
	/**
	 * This class is the source of the events.
	 * So we have a system below for registering / unregistering listeners
	 * and notifying them of updates. 
	 * @param jl 
	 */
	public synchronized void addListener(IJobListener jl){
		if (jobListeners==null){
			jobListeners = new Vector();
		}
		if (jl==null)
		    return;
		
		synchronized (jobListeners){
			if (!jobListeners.contains(jl)) jobListeners.add(jl);
		}
	}
	
	/** 
	 * @param jl
	 */
	public void removeListener(IJobListener jl){
		synchronized (jobListeners){
			if ((jobListeners!=null) && (jl!=null)){
				jobListeners.remove(jl);
			}
		}
	}
	
	/**
	 *
	 */
	public void removeAllListeners(){
		synchronized (jobListeners){
			jobListeners.removeAllElements();
		}
	}
	
	class JobMonitor implements Runnable{

		private static final int POLLTIME=2500;
		
		//to keep track of how many times a query-status failed for each job
		//private Hashtable jobqueryCounter = null;

	    private boolean running = false;
	    
	    private ThreadGroup group;
	    
	    //keep a simple count of jobs.
	    private int totalJobs = 0;
	    private int doneJobs = 0;
	    private int failedJobs = 0;

		/**
		 * 
		 */
		public JobMonitor() {
			super();
			group = new ThreadGroup("EventThreads");
		}

		/**
		 * @see java.lang.Runnable#run()
		 */
		public void run() {
			running = true;
			if (jobs!=null)
				totalJobs = jobs.size();
			
			while (isRunning()){
				try{
					for (int i=0;i<jobs.size();i++){
						GJob j = jobs.get(i);
						int status = queryJobStatus(j.getHandle());
						if (status == GJob.Finished || status == GJob.Dead){
							//check if we need to raise the finished/failed event
							if (j.getExecutionException()!=null){ 
								logger.debug("Raising job failed event: "+j.getHandle());
								raiseJobFailedEvent(j);
								failedJobs++;
							}else{
								logger.debug("Raising job done event: "+j.getHandle());
								raiseJobFinishedEvent(j);
								doneJobs++;
							}
							
							//get the finished jobs as soon as they are done.
							getFinishedJobs();
						}
					}
					
					int appStatus = queryApplicationStatus();
					if (appStatus == GApplication.Status_Stopped || (totalJobs==doneJobs+failedJobs)){
						//raise the finished event..but make sure all job-event threads are done
						//group.wait();
						logger.debug("Raising application finished event");
						raiseAppFinishedEvent();
						
						//stop the monitor
						running = false;
						break;
					}
					
					Thread.sleep(POLLTIME);
				}catch (Exception e){
					//ignore for now. later log.
					logger.debug("Exception monitoring jobs..."+e.getMessage()+". Continuing...",e);
				}
			}
			logger.debug("Job Monitor stopped.");
		}

		protected void stop(){
			running = false;
		}
		
		protected boolean isRunning(){
			return running;
		}
		
		//comments on events
		//If this is made synchronized, then the notification is blocking.
		//instead we choose to clone the listeners collection, and go on by notifying the the cloned collection
		//also, we need to have the argument "final" so that it can be used inside the anonymous inner class

		private void raiseJobFailedEvent(final GJob j){
			new Thread(group,"Job-failed-event thread:"+j.getJobID())
			{   
				public void run()
				{
					try{
						Vector tempJL = null;
						synchronized (jobListeners){
							tempJL = (Vector)jobListeners.clone();
						}
						
						if ((tempJL!=null)&&(j!=null)){
							for (Iterator it=tempJL.iterator();it.hasNext();){
								IJobListener jl = (IJobListener)it.next();
								if (jl!=null) jl.JobFailed(j,j.getExecutionException());
							}
						}					
					}catch (Exception e){
						logger.debug("Error raising JobFailed event: "+j.getJobID()+"\n "+e.getMessage());
					}
				}
			}.start();
		}
		
		private void raiseJobFinishedEvent(final GJob j){
			new Thread(group,"Job-finished-event thread:"+j.getJobID())
			{   
				public void run()
				{
					try{
						Vector tempJL = null;
						synchronized (jobListeners){
							tempJL = (Vector)jobListeners.clone();
						}
						
						if ((tempJL!=null)&&(j!=null)){
							for (Iterator it=tempJL.iterator();it.hasNext();){
								IJobListener jl = (IJobListener)it.next();
								if (jl!=null) jl.JobFinished(j);
							}
						}					
					}catch (Exception e){
						logger.debug("Error raising JobFinsihed event: "+j.getJobID()+"\n "+e.getMessage());
					}
				}
			}.start();
		}
		
		private void raiseAppFinishedEvent(){
			new Thread(group,"App-finished-event thread:")
			{   
				public void run()
				{
					try{
						Vector tempJL = null;
						synchronized (jobListeners){
							tempJL = (Vector)jobListeners.clone();
						}
						
						finished = true;
						
						if (tempJL!=null){
							for (Iterator it=tempJL.iterator();it.hasNext();){
								IJobListener al = (IJobListener)it.next();
								if (al!=null){
									al.ApplicationFinished();
								}
							}
						}				
					}catch (Exception e){
						logger.debug("Error raising ApplicationFinished event: \n "+e.getMessage());
					}
				}
			}.start();
		}

	}

	/**
	 * @return Returns the localWorkingDirectory.
	 */
	public String getLocalWorkingDirectory() {
		return localWorkingDirectory;
	}

	/**
	 * @param localWorkingDirectory The localWorkingDirectory to set.
	 */
	public void setLocalWorkingDirectory(String localWorkingDirectory) {
		this.localWorkingDirectory = localWorkingDirectory;
	}

}
