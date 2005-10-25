/*
 * Title        :  EmbeddedFileDependency.java
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

import org.gridbus.alchemi.client.util.Base64;

/**
 * @author krishna
 *
 */
public class EmbeddedFileDependency extends FileDependency {

	protected String base64EncodedContents;

	/**
	 * 
	 */
	public EmbeddedFileDependency(String filename) {
		super(filename);
	}

	/**
	 * @see org.gridbus.alchemi.client.FileDependency#UnPack(java.lang.String)
	 */
	public void UnPack(String fileLocation) {
		Base64.decodeToFile(base64EncodedContents,fileLocation);
	}
	
	public void Pack(String fileLocation){
		base64EncodedContents = Base64.encodeFromFile(fileLocation);
	}

	/**
	 * @return Returns the base64EncodedContents.
	 */
	public String getBase64EncodedContents() {
		return base64EncodedContents;
	}
	/**
	 * @param base64EncodedContents The base64EncodedContents to set.
	 */
	public void setBase64EncodedContents(String base64EncodedContents) {
		this.base64EncodedContents = base64EncodedContents;
	}
}
