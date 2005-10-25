/*
 * Title        :  TestJobResultParsing.java
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

import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.InputStreamReader;

import org.apache.log4j.PropertyConfigurator;
import org.gridbus.alchemi.client.util.AlchemiXmlUtil;

/**
 * @author krishna
 *
 */
public class TestJobResultParsing {

	/**
	 * 
	 */
	public TestJobResultParsing() {
		super();
	}

	public static void main(String[] args) throws Exception{
		PropertyConfigurator.configure("log4j.properties");
		System.out.println("Enter result file name:");
		
		BufferedReader br = new BufferedReader(new InputStreamReader(System.in));
		String resultFile = br.readLine();
		br.close();
		br = null;
		
		String taskXml = "";
		FileReader fr = new FileReader(resultFile);
		br = new BufferedReader(fr);
		String temp = "";
		while ((temp=br.readLine())!=null){
			taskXml += temp;
		}
		br.close();
		br = null;
		
		AlchemiXmlUtil.parseXmlOutput(taskXml);
	
		
	}
}
