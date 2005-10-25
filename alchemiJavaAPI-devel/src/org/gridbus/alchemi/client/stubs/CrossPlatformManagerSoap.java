/**
 * CrossPlatformManagerSoap.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis 1.2.1 Jun 14, 2005 (09:15:57 EDT) WSDL2Java emitter.
 */

package org.gridbus.alchemi.client.stubs;

public interface CrossPlatformManagerSoap extends java.rmi.Remote {
    public int getJobState(java.lang.String username, java.lang.String password, java.lang.String taskId, int jobId) throws java.rmi.RemoteException;
    public java.lang.String getFinishedJobs(java.lang.String username, java.lang.String password, java.lang.String taskId) throws java.rmi.RemoteException;
    public void addJob(java.lang.String username, java.lang.String password, java.lang.String taskId, int jobId, int priority, java.lang.String jobXml) throws java.rmi.RemoteException;
    public java.lang.String submitTask(java.lang.String username, java.lang.String password, java.lang.String taskXml) throws java.rmi.RemoteException;
    public java.lang.String createTask(java.lang.String username, java.lang.String password) throws java.rmi.RemoteException;
    public void ping() throws java.rmi.RemoteException;
    public int getApplicationState(java.lang.String username, java.lang.String password, java.lang.String taskId) throws java.rmi.RemoteException;
    public void abortTask(java.lang.String username, java.lang.String password, java.lang.String taskId) throws java.rmi.RemoteException;
    public void abortJob(java.lang.String username, java.lang.String password, java.lang.String taskId, int jobId) throws java.rmi.RemoteException;
    public java.lang.String getFailedJobException(java.lang.String username, java.lang.String password, java.lang.String taskId, int jobId) throws java.rmi.RemoteException;
}
