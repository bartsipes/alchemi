/*
 * Title        :  AlchemiXmlUtil.java
 * Package      :  org.gridbus.alchemi.client.util
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

package org.gridbus.alchemi.client.util;

import org.apache.log4j.Logger;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileWriter;
import java.net.URLEncoder;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;

import org.gridbus.alchemi.client.EmbeddedFileDependency;
import org.gridbus.alchemi.client.FileDependency;
import org.gridbus.alchemi.client.FileDependencyCollection;
import org.gridbus.alchemi.client.GApplication;
import org.gridbus.alchemi.client.GJob;
import org.gridbus.alchemi.client.GJobCollection;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.NamedNodeMap;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;
import org.w3c.dom.Text;

/**
 * @author krishna
 *
 */
public class AlchemiXmlUtil {
	/**
	 * Logger for this class
	 */
	private static final Logger logger = Logger.getLogger(AlchemiXmlUtil.class);

	/**
	 * Returns an XML string given a DOM document.
	 * @param doc
	 * @return
	 * @throws Exception
	 */
	private static String getStringFromDOM(Document doc) throws Exception{
		//get the XML from the DOM
		TransformerFactory transFac = TransformerFactory.newInstance();
		Transformer transformer = transFac.newTransformer();
		DOMSource source = new DOMSource(doc);
		ByteArrayOutputStream bout = new ByteArrayOutputStream();
		StreamResult result = new StreamResult(bout);
		transformer.transform(source, result);
		
		String xml = new String(bout.toByteArray());
		bout.close();
		bout = null;
		return xml;
	}
	
	private static Node getJobXml(Document doc, GJob job) throws Exception{
		
		//create the job
		Element jobEl = doc.createElement("job");
		//set the jobID
		jobEl.setAttribute("id",job.getJobID()+"");
		
		//add the input files
		Element input = doc.createElement("input");
		FileDependencyCollection inputs = job.getInputfiles();
		if (inputs!=null){
			for (int i=0;i<inputs.size();i++){
				input.appendChild(makeFileNode(doc, inputs.get(i),true));
			}
		}
		jobEl.appendChild(input);
		
		//set the work command
		Element work = doc.createElement("work");
		work.setAttribute("run_command", job.getRunCommand());
		jobEl.appendChild(work);
		
		//add the ouput files
		Element output = doc.createElement("output");
		FileDependencyCollection outputs = job.getOutputfiles();
		if (outputs!=null){
			for (int i=0;i<outputs.size();i++){
				output.appendChild(makeFileNode(doc, outputs.get(i),false));
			}
		}
		jobEl.appendChild(output);
		return jobEl;
	} 

	public static String getTaskXml(GApplication gapp) throws Exception{
		String taskXml = null;
		
		GJobCollection jobs = gapp.getJobs();
		FileDependencyCollection manifestFiles = gapp.getManifest();
		
		//create the jobXML
		DocumentBuilderFactory domfac = DocumentBuilderFactory.newInstance();
		DocumentBuilder builder = domfac.newDocumentBuilder();
		Document doc = builder.newDocument();
		Element taskRoot = doc.createElement("task");
		
		//set the manifest and add it to the root element
		Element manifest = doc.createElement("manifest");
		if (manifestFiles != null){
			for (int i=0; i < manifestFiles.size(); i++){
				manifest.appendChild(makeFileNode(doc,manifestFiles.get(i),true));
			}
		}
		taskRoot.appendChild(manifest);
		
		for (int i=0; i < jobs.size(); i++){
			GJob job = jobs.get(i);
			//add the job element to the root-task element
			Node jobNode = getJobXml(doc,job);
			taskRoot.appendChild(jobNode);
		}
	
		//finally add the root element to the document
		doc.appendChild(taskRoot);
		
		//doc.normalize();
		
		taskXml = getStringFromDOM(doc);
		
		if (logger.isDebugEnabled()){
			String content = taskXml;
			String filename = gapp.getLocalWorkingDirectory() + "/" + gapp.getTaskID() + "-taskXml.xml";
			FileWriter fw = new FileWriter(filename);
			fw.write(content);
			fw.close();	
			logger.debug("Generated taskXML saved to : "+filename);
		}
		
		taskXml = htmlEscape(taskXml);
		
		return taskXml;
	}

	/*
	 * <embedded_file name="input1.txt"> base64-Contents-HERE </embedded_file>
	 */
	private static Element makeFileNode(Document doc, FileDependency fileDep, boolean embed) throws Exception{
		Element embeddedFile = doc.createElement("embedded_file");
		
		//this is to strip the path on the client...
		//and just send the file name to the server in the xml description
		String filename = (new File(fileDep.getFilename())).getName();
		
		embeddedFile.setAttribute("name",filename);
		if ((fileDep instanceof EmbeddedFileDependency) && (embed==true)){
			logger.debug("Embedding file : "+fileDep.getFilename());
		    Text t = doc.createTextNode(Base64.encodeFromFile(fileDep.getFilename()));
		    embeddedFile.appendChild(t);
			logger.debug("DONE Embedding file : "+fileDep.getFilename());
		}
		return embeddedFile;
	}
	
	/**
	 * Parse the returned XML and unpack files into the localWorkingDirectory
	 * @param taskXml
	 * @param localWorkingDirectory
	 * @throws Exception
	 */
	public static void parseXmlOutput(String taskXml, String localWorkingDirectory) throws Exception{
		DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
		DocumentBuilder builder = factory.newDocumentBuilder();
		
		ByteArrayInputStream is = new ByteArrayInputStream(taskXml.getBytes());
		Document document = builder.parse(is);
		
		logger.debug("Parsing output XML. Extracting files to directory : '" + localWorkingDirectory + "'");
		
		NodeList task = document.getElementsByTagName("task");
		Element taskEl = (Element)task.item(0);
		String taskID = taskEl.getAttribute("id");
		
		NodeList jobs = document.getElementsByTagName("job");
		for (int i=0;i<jobs.getLength();i++){
			//for each job, get the output tag
			logger.debug("Parsing job element "+i);
			Element jobEl = (Element)jobs.item(i);
			String jobID = jobEl.getAttribute("id");
			
			logger.debug("Job element "+i+", jobID="+jobID+". Getting output files...");
			
			Element output = (Element)jobEl.getElementsByTagName("output").item(0);
			//we know that there would be only one output tag.
			NodeList files = output.getElementsByTagName("embedded_file");
			
			logger.debug("# of output files = "+files.getLength());
			for (int j=0;j<files.getLength();j++){
				Element file = (Element)files.item(j);
				String fileName = "task[" + taskID + "]-job[" + jobID + "]-" + file.getAttribute("name");				
				Text data = (Text)file.getFirstChild();
				String destFile = combinePath(localWorkingDirectory,fileName,"/");
				Base64.decodeToFile(data.getData(),destFile);
				logger.debug("Extracted file: "+destFile);
			}
			logger.debug("Done parsing job: "+jobID);
		}
		logger.debug("Done parsing XML task description");		
	}
	
	/**
	 * Parse the returned XML and unpack files to the current directory.
	 * @param taskXml
	 * @throws Exception
	 */
	public static void parseXmlOutput(String taskXml) throws Exception{
		parseXmlOutput(taskXml,"");
	}
	
	/**
	  * Replace characters having special meaning <em>inside</em> HTML tags
	  * with their escaped equivalents, using character entities such as <tt>'&amp;'</tt>.
	  *
	  * <P>The escaped characters are :
	  * <ul>
	  * <li> <
	  * <li> >
	  * <li> "
	  * <li> '
	  * <li> \
	  * <li> &
	  * </ul>
	  *
	  * <P>This method ensures that arbitrary text appearing inside a tag does not "confuse"
	  * the tag. For example, <tt>HREF='Blah.do?Page=1&Sort=ASC'</tt>
	  * does not comply with strict HTML because of the ampersand, and should be changed to
	  * <tt>HREF='Blah.do?Page=1&amp;Sort=ASC'</tt>. This is commonly seen in building
	  * query strings. (In JSTL, the c:url tag performs this task automatically.)
	  */
	//adapted from: http://www.javapractices.com/Topic96.cjp
	private static String htmlEscape(String tag) {
		final StringBuffer result = new StringBuffer();
		if (tag == null){
			return null;
		}else{
			for (int i = 0; i < tag.length(); i++) {
				char character = tag.charAt(i);
				if (character == '<') {
					result.append("&lt;");
				} else if (character == '>') {
					result.append("&gt;");
				} else if (character == '\"') {
					result.append("&quot;");
				} else if (character == '\'') {
					result.append("&#039;");
				} else if (character == '\\') {
					result.append("&#092;");
				} else if (character == '&') {
					result.append("&amp;");
				} else {
					//the char is not a special one
					//add it to the result as is
					result.append(character);
				}
			}
			//return Base64.encodeBytes(result.toString().getBytes());
			return result.toString();
		}
	}
	
	/**
	 * @param path1
	 * @param path2
	 * @param pathSeperator
	 * @return
	 */
	public static String combinePath(String path1, String path2, String pathSeperator){
		String path = null;
		//make sure there is a valid path seperator char. / or \
		if (pathSeperator==null || pathSeperator.trim().equals("")){
			pathSeperator = File.separator;
		}
		if (path1!=null && path2!=null){
			path1 = path1.trim();
			path2 = path2.trim();
			
			if (path1.equals(""))
				return path2;
			
			if (path2.equals(""))
				return path1;

			if (path1.endsWith(pathSeperator)){
				path = path1.substring(0,path1.length()-pathSeperator.length()) + pathSeperator + path2;
			}else{
				path = path1 + pathSeperator + path2;
			}
			if (path.endsWith(pathSeperator)){
				path = path.substring(0,path.length()-pathSeperator.length());
			}
		}
		return path;		
	}
}
