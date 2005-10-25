
/*
 * Title        :  GJobCollection.java
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
public class GJobCollection {

	private ArrayList jobs;
	
	/**
	 * Represents a strongly typed collection of GJobs 
	 */
	public GJobCollection() {
		super();
		jobs = new ArrayList();
	}
	
	/**
	 * Returns the size of the Collection
	 */
	public int size() {	
		return jobs.size();
	}

	/**
	 * Returns true if the collection is empty, false otherwise.
	 * 
	 */
	public boolean isEmpty() {		
		return jobs.isEmpty();
	}

	/**
	 * Returns true if the collection contains the given job, false otherwise.
	 * @param job
	 * @return
	 */
	public boolean contains(GJob job) {
		return jobs.contains(job);
	}

	/**
	 * 
	 */
	public GJob[] toArray() {
		Object[] obj = jobs.toArray();
		GJob[] jobs = new GJob[obj.length];
		for (int i=0; i < obj.length; i++)
			jobs[i] = (GJob)obj[i];
		return jobs;
	}

	/**
	 * 
	 */
	public boolean add(GJob job) {
		if (this.contains(job))
			return false;
		else
			return jobs.add(job);
	}

	/**
	 * 
	 */
	public boolean remove(GJob job) {
		return jobs.remove(job);
	}

	/**
	 * 
	 */
	public void clear() {
		jobs.clear();
	}

	/**
	 * 
	 */
	public GJob get(int index) {
		return (GJob)jobs.get(index);
	}

	/**
	 * 
	 */
	public GJob set(int index, GJob job) {
		return (GJob)jobs.set(index, job);
	}

	/**
	 * 
	 */
	public GJob remove(int index) {
		return (GJob)jobs.remove(index);
	}

}
