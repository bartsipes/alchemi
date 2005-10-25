/**
 * CrossPlatformManagerLocator.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis 1.2.1 Jun 14, 2005 (09:15:57 EDT) WSDL2Java emitter.
 */

package org.gridbus.alchemi.client.stubs;

public class CrossPlatformManagerLocator extends org.apache.axis.client.Service implements CrossPlatformManager {

    public CrossPlatformManagerLocator() {
    }


    public CrossPlatformManagerLocator(org.apache.axis.EngineConfiguration config) {
        super(config);
    }

    public CrossPlatformManagerLocator(java.lang.String wsdlLoc, javax.xml.namespace.QName sName) throws javax.xml.rpc.ServiceException {
        super(wsdlLoc, sName);
    }

    // Use to get a proxy class for CrossPlatformManagerSoap
    private java.lang.String CrossPlatformManagerSoap_address = "http://localhost/Alchemi.CrossPlatformManager/CrossPlatformManager.asmx";

    public java.lang.String getCrossPlatformManagerSoapAddress() {
        return CrossPlatformManagerSoap_address;
    }

    // The WSDD service name defaults to the port name.
    private java.lang.String CrossPlatformManagerSoapWSDDServiceName = "CrossPlatformManagerSoap";

    public java.lang.String getCrossPlatformManagerSoapWSDDServiceName() {
        return CrossPlatformManagerSoapWSDDServiceName;
    }

    public void setCrossPlatformManagerSoapWSDDServiceName(java.lang.String name) {
        CrossPlatformManagerSoapWSDDServiceName = name;
    }

    public CrossPlatformManagerSoap getCrossPlatformManagerSoap() throws javax.xml.rpc.ServiceException {
       java.net.URL endpoint;
        try {
            endpoint = new java.net.URL(CrossPlatformManagerSoap_address);
        }
        catch (java.net.MalformedURLException e) {
            throw new javax.xml.rpc.ServiceException(e);
        }
        return getCrossPlatformManagerSoap(endpoint);
    }

    public CrossPlatformManagerSoap getCrossPlatformManagerSoap(java.net.URL portAddress) throws javax.xml.rpc.ServiceException {
        try {
            CrossPlatformManagerSoapStub _stub = new CrossPlatformManagerSoapStub(portAddress, this);
            _stub.setPortName(getCrossPlatformManagerSoapWSDDServiceName());
            return _stub;
        }
        catch (org.apache.axis.AxisFault e) {
            return null;
        }
    }

    public void setCrossPlatformManagerSoapEndpointAddress(java.lang.String address) {
        CrossPlatformManagerSoap_address = address;
    }

    /**
     * For the given interface, get the stub implementation.
     * If this service has no port for the given interface,
     * then ServiceException is thrown.
     */
    public java.rmi.Remote getPort(Class serviceEndpointInterface) throws javax.xml.rpc.ServiceException {
        try {
            if (CrossPlatformManagerSoap.class.isAssignableFrom(serviceEndpointInterface)) {
                CrossPlatformManagerSoapStub _stub = new CrossPlatformManagerSoapStub(new java.net.URL(CrossPlatformManagerSoap_address), this);
                _stub.setPortName(getCrossPlatformManagerSoapWSDDServiceName());
                return _stub;
            }
        }
        catch (java.lang.Throwable t) {
            throw new javax.xml.rpc.ServiceException(t);
        }
        throw new javax.xml.rpc.ServiceException("There is no stub implementation for the interface:  " + (serviceEndpointInterface == null ? "null" : serviceEndpointInterface.getName()));
    }

    /**
     * For the given interface, get the stub implementation.
     * If this service has no port for the given interface,
     * then ServiceException is thrown.
     */
    public java.rmi.Remote getPort(javax.xml.namespace.QName portName, Class serviceEndpointInterface) throws javax.xml.rpc.ServiceException {
        if (portName == null) {
            return getPort(serviceEndpointInterface);
        }
        java.lang.String inputPortName = portName.getLocalPart();
        if ("CrossPlatformManagerSoap".equals(inputPortName)) {
            return getCrossPlatformManagerSoap();
        }
        else  {
            java.rmi.Remote _stub = getPort(serviceEndpointInterface);
            ((org.apache.axis.client.Stub) _stub).setPortName(portName);
            return _stub;
        }
    }

    public javax.xml.namespace.QName getServiceName() {
        return new javax.xml.namespace.QName("http://www.alchemi.net", "CrossPlatformManager");
    }

    private java.util.HashSet ports = null;

    public java.util.Iterator getPorts() {
        if (ports == null) {
            ports = new java.util.HashSet();
            ports.add(new javax.xml.namespace.QName("http://www.alchemi.net", "CrossPlatformManagerSoap"));
        }
        return ports.iterator();
    }

    /**
    * Set the endpoint address for the specified port name.
    */
    public void setEndpointAddress(java.lang.String portName, java.lang.String address) throws javax.xml.rpc.ServiceException {
        
if ("CrossPlatformManagerSoap".equals(portName)) {
            setCrossPlatformManagerSoapEndpointAddress(address);
        }
        else 
{ // Unknown Port Name
            throw new javax.xml.rpc.ServiceException(" Cannot set Endpoint Address for Unknown Port" + portName);
        }
    }

    /**
    * Set the endpoint address for the specified port name.
    */
    public void setEndpointAddress(javax.xml.namespace.QName portName, java.lang.String address) throws javax.xml.rpc.ServiceException {
        setEndpointAddress(portName.getLocalPart(), address);
    }

}
