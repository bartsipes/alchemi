/*
 * Title        :  TestAlchemi.java
 * Package      :  org.gridbus.alchemi.client.test
 * Project      :  AlchemiJavaAPI
 * Description	:  
 * Created on   :  3/08/2005
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

package org.gridbus.alchemi.client.test;

import junit.framework.TestCase;

import org.gridbus.alchemi.client.EmbeddedFileDependency;
import org.gridbus.alchemi.client.FileDependencyCollection;
import org.gridbus.alchemi.client.GApplication;
import org.gridbus.alchemi.client.GJob;
import org.gridbus.alchemi.client.IJobListener;
import org.gridbus.alchemi.client.SecurityCredentials;

/**
 * @author krishna
 *
 */
public class TestAlchemi extends TestCase implements IJobListener{

	private GApplication ga;
	
	public static void main(String[] args) {
		junit.textui.TestRunner.run(TestAlchemi.class);
	}

	/*
	 * @see TestCase#setUp()
	 */
	protected void setUp() throws Exception {
		super.setUp();
	}

	/*
	 * @see TestCase#tearDown()
	 */
	protected void tearDown() throws Exception {
		super.tearDown();
	}

	/**
	 * Constructor for TestAlchemiJob.
	 * @param name
	 */
	public TestAlchemi(String name) {
		super(name);
	}

	public final void testAlchemi(){
		SecurityCredentials sc = new SecurityCredentials("user","user");
		try{
			//create a new application, and set its manifest
			ga = new GApplication("http://localhost:81/Alchemi.CrossPlatformManager/CrossPlatformManager.asmx",sc);
			FileDependencyCollection manifest = new FileDependencyCollection();
			manifest.add(new EmbeddedFileDependency("workingDIR/Reverse.exe"));
			ga.setManifest(manifest);
			
			//create a new job
			GJob gj = new GJob();
			gj.setRunCommand("Reverse.exe abc.txt > abcRev.txt");
			
			FileDependencyCollection inputs = new FileDependencyCollection();
			inputs.add(new EmbeddedFileDependency("workingDIR/abc.txt"));
			gj.setInputfiles(inputs);
			
			FileDependencyCollection outputs = new FileDependencyCollection();
			outputs.add(new EmbeddedFileDependency("abcRev.txt"));
			gj.setOutputfiles(outputs);
			
			ga.addJob(gj);
			ga.addListener(this);
			
			System.out.println("Pinged manager.");
			
			ga.start();
			
			//System.out.println("Result of create task is : " + ga.tempCreateTask());
		}catch (Exception e){
			e.printStackTrace();
			fail("Unexpected exception: "+e.getMessage());
		}
	}

	/**
	 * Job finished event
	 * @see org.gridbus.alchemi.client.IJobListener#JobFinished(org.gridbus.alchemi.client.GJob)
	 */
	public void JobFinished(GJob job) {
		System.out.println("Job "+job.getHandle() + " finished.");
	}

	/**
	 * Job Failed Event
	 * @see org.gridbus.alchemi.client.IJobListener#JobFailed(org.gridbus.alchemi.client.GJob, java.lang.Exception)
	 */
	public void JobFailed(GJob job, Exception ex) {
		System.out.println("Job "+job.getHandle() + " failed.");
		ex.printStackTrace(); //job error
	}

	/**
	 * Application finished event
	 * @see org.gridbus.alchemi.client.IJobListener#ApplicationFinished()
	 */
	public void ApplicationFinished() {
		System.out.println("GApplication finished.");
		if (ga!=null){
			try{
				ga.stop();
			}catch (Exception e){
				e.printStackTrace();
			}
			ga = null;
		}
	}

}
