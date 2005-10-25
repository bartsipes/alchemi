/*
 * Title        :  FileDependencyCollection.java
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

import java.util.ArrayList;

/**
 * @author krishna
 *
 */
public class FileDependencyCollection{
	
	private ArrayList files;
	
	public FileDependencyCollection(){
		files = new ArrayList();
	}
	
	/**
	 * 
	 */
	public int size() {	
		return files.size();
	}

	/**
	 * 
	 */
	public boolean isEmpty() {		
		return files.isEmpty();
	}

	/**
	 * 
	 */
	public boolean contains(FileDependency fileDep) {
		return files.contains(fileDep);
	}

	/**
	 *
	 */
	public FileDependency[] toArray() {
		Object[] obj = files.toArray();
		FileDependency[] fileDeps = new FileDependency[obj.length];
		for (int i=0; i < obj.length; i++)
			fileDeps[i] = (FileDependency)obj[i];
		return fileDeps;
	}

	/**
	 * 
	 */
	public boolean add(FileDependency fileDep) {
		if (this.contains(fileDep))
			return false;
		else
			return files.add(fileDep);
	}

	/**
	 * 
	 */
	public boolean remove(FileDependency fileDep) {
		return files.remove(fileDep);
	}

	/**
	 * 
	 */
	public void clear() {
		files.clear();
	}

	/**
	 * 
	 */
	public FileDependency get(int index) {
		return (FileDependency)files.get(index);
	}

	/**
	 * 
	 */
	public FileDependency set(int index, FileDependency fileDep) {
		return (FileDependency)files.set(index, fileDep);
	}

	/**
	 * 
	 */
	public FileDependency remove(int index) {
		return (FileDependency)files.remove(index);
	}
}
