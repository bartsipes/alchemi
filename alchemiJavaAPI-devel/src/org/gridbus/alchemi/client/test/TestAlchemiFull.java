/*
 * Title        :  TestAlchemiFull.java
 * Package      :  org.gridbus.alchemi.client.test
 * Project      :  AlchemiJavaAPI
 * Description	:  
 * Created on   :  19/10/2005
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

import org.apache.log4j.PropertyConfigurator;
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
public class TestAlchemiFull implements IJobListener{

	private GApplication ga;

	/**
	 * 
	 */
	public TestAlchemiFull() {
		super();
	}

	public static void main(String[] args) {
		SecurityCredentials sc = new SecurityCredentials("user","user");
		TestAlchemiFull taf = new TestAlchemiFull();
		PropertyConfigurator.configure("log4j.properties");
		
		try{
			GApplication ga = taf.ga;
			//create a new application, and set its manifest
			ga = new GApplication("http://localhost:80/Alchemi.CrossPlatformManager/CrossPlatformManager.asmx",sc);
			FileDependencyCollection manifest = new FileDependencyCollection();
//			manifest.add(new EmbeddedFileDependency("workingDIR/Reverse.exe"));
			manifest.add(new EmbeddedFileDependency("workingDIR/calc.exe"));
			ga.setManifest(manifest);
			
			//create a new job
			GJob gj = new GJob();
			gj.setJobID(1);
			gj.setRunCommand("calc.exe 0 0");
			//gj.setRunCommand("Reverse.exe abc.txt > abcRev.txt");
			
//			FileDependencyCollection inputs = new FileDependencyCollection();
//			inputs.add(new EmbeddedFileDependency("workingDIR/abc.txt"));
//			gj.setInputfiles(inputs);
			
//			FileDependencyCollection outputs = new FileDependencyCollection();
//			outputs.add(new EmbeddedFileDependency("abcRev.txt"));
//			gj.setOutputfiles(outputs);
			
			FileDependencyCollection outputs = new FileDependencyCollection();
			outputs.add(new EmbeddedFileDependency("output"));
			gj.setOutputfiles(outputs);
			
			ga.addJob(gj);
			ga.addListener(taf);
			
			System.out.println("Pinged manager.");
			
			ga.start();
			
			while (!ga.isFinished()){
				Thread.sleep(2000);
			}
			
			System.out.println("GApplication completed. Test successful....");
			
		}catch (Exception e){
			e.printStackTrace();
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
