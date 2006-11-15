/**
 * 
 */
package org.gridbus.alchemi.client;

/**
 * @author Krishna
 *
 */
public abstract class GThread {

	protected String id = null;
	
	/**
	 * Sub-classes should implement this
	 */
	public abstract void run();

}
