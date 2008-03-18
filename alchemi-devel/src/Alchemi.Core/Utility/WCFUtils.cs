using System;
using System.Collections.Generic;
using System.Text;

namespace Alchemi.Core.Utility
{
    /// <summary>
    /// This class contains some static helper functions for working with WCF connections.
    /// </summary>
    public sealed class WCFUtils
    {
        #region Method - ComposeAddress
        /// <summary>
        /// Composes an address from parts.
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="pathOnServer"></param>
        /// <returns></returns>
        public static string ComposeAddress(string protocol, string host, int port, string pathOnServer, WCFBinding binding)
        {
            string _protocol = "<none>";
            string _host = "<none>";

            if (protocol != string.Empty)
                _protocol = protocol;

            if (host != string.Empty)
                _host = host;


            string portPart = String.Format(":{0}", port.ToString());

            if (_protocol == "http" && port == 80)
                portPart = string.Empty;

            if (binding == WCFBinding.NetNamedPipeBinding)
                portPart = string.Empty;

            return string.Format("{0}://{1}{3}/{2}",
                _protocol, //0
                _host, //1
                pathOnServer.TrimStart('/'), //2
                portPart //3
            );
        }
        #endregion

        #region Method - BreakAddress
        /// <summary>
        /// Splits the address into address parts.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="protocol"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="pathOnServer"></param>
        public static void BreakAddress(string address, out string protocol, out string host, out int port, out string pathOnServer)
        {
            string _protocol = "<none>";
            string _host = "<none>";
            int _port = 0;
            string _pathOnServer = string.Empty;

            try
            {
                string[] addrsArr = address.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                if (addrsArr.Length > 0)
                    _protocol = addrsArr[0].TrimEnd(':');
                if (addrsArr.Length > 1)
                    _host = addrsArr[1];
                //set default port
                if (_protocol == "http")
                    _port = 80;
                if (_protocol == "https")
                    _port = 443;
                //read port if specified
                if (_host.IndexOf(':') != -1)
                {
                    string[] hArr = _host.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    _host = hArr[0];
                    _port = Int32.Parse(hArr[1]);
                }
                //join the other values to loacal address part
                _pathOnServer = string.Empty;
                for (int i = 2; i < addrsArr.Length; i++)
                {
                    _pathOnServer += String.Format("/{0}", addrsArr[i]);
                }
                _pathOnServer = _pathOnServer.Trim('/');

            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Invalid address format for address '{0}'.", address), ex);
            }

            protocol = _protocol;
            pathOnServer = _pathOnServer;
            host = _host;
            port = _port;
        }
        #endregion

        #region Method - GetWCFBinding
        /// <summary>
        /// Retuns the newly constructed and configured WCF bindig derived class instance of the specified type.
        /// </summary>
        /// <remarks>
        /// This function might get more important if other WCF bingis are supported.
        /// All settings to a specific binding shuld be made here.
        /// All binding are probably not needed.
        /// </remarks>
        /// <param name="endPoint">EndPoint entity containig iformation about, what type of binding to return and how to set it.</param>
        /// <returns>The new configured binding element.</returns>
        public static System.ServiceModel.Channels.Binding GetWCFBinding(EndPoint endPoint)
        {
            if (endPoint.RemotingMechanism == RemotingMechanism.TcpBinary)
                return null;

            System.ServiceModel.Channels.Binding ret = null;

            WCFBinding bindingEnum = endPoint.Binding;

            switch (bindingEnum)
            {
                #region BasicHttpBinding
                case WCFBinding.BasicHttpBinding:
                    {
                        //WARNING: untested code
                        System.ServiceModel.BasicHttpBinding bhb = null;
                        switch (endPoint.BindingSettingType)
                        {
                            case WCFBindingSettingType.Default:
                                bhb = new System.ServiceModel.BasicHttpBinding();
                                bhb.ReaderQuotas.MaxStringContentLength = Int32.MaxValue;
                                bhb.MaxReceivedMessageSize = Int32.MaxValue;
                                bhb.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.None;
                                break;
                            case WCFBindingSettingType.UseConfigFile:
                                bhb = new System.ServiceModel.BasicHttpBinding(endPoint.BindingConfigurationName);
                                break;
                        }
                        ret = bhb;
                        break;
                    }
                #endregion
                #region MsmqIntegrationBinding
                case WCFBinding.MsmqIntegrationBinding:
                    {
                        //WARNING: untested code
                        System.ServiceModel.MsmqIntegration.MsmqIntegrationBinding mib = null;
                        switch (endPoint.BindingSettingType)
                        {
                            case WCFBindingSettingType.Default:
                                mib = new System.ServiceModel.MsmqIntegration.MsmqIntegrationBinding();
                                mib.Security.Mode = System.ServiceModel.MsmqIntegration.MsmqIntegrationSecurityMode.None;
                                break;
                            case WCFBindingSettingType.UseConfigFile:
                                mib = new System.ServiceModel.MsmqIntegration.MsmqIntegrationBinding(endPoint.BindingConfigurationName);
                                break;
                        }
                        ret = mib;
                        break;
                    }
                #endregion
                #region NetMsmqBinding
                case WCFBinding.NetMsmqBinding:
                    {
                        //WARNING: untested code
                        System.ServiceModel.NetMsmqBinding nmb = null;
                        switch (endPoint.BindingSettingType)
                        {
                            case WCFBindingSettingType.Default:
                                nmb = new System.ServiceModel.NetMsmqBinding();
                                nmb.ReaderQuotas.MaxStringContentLength = Int32.MaxValue;
                                nmb.Security.Mode = System.ServiceModel.NetMsmqSecurityMode.None;
                                break;
                            case WCFBindingSettingType.UseConfigFile:
                                nmb = new System.ServiceModel.NetMsmqBinding(endPoint.BindingConfigurationName);
                                break;
                        }
                        ret = nmb;
                        break;
                    }
                #endregion
                #region NetNamedPipeBinding
                case WCFBinding.NetNamedPipeBinding:
                    {
                        //WARNING: untested code
                        System.ServiceModel.NetNamedPipeBinding nnpb = null;
                        switch (endPoint.BindingSettingType)
                        {
                            case WCFBindingSettingType.Default:
                                nnpb = new System.ServiceModel.NetNamedPipeBinding();
                                nnpb.ReaderQuotas.MaxStringContentLength = Int32.MaxValue;
                                nnpb.Security.Mode = System.ServiceModel.NetNamedPipeSecurityMode.None;
                                break;
                            case WCFBindingSettingType.UseConfigFile:
                                nnpb = new System.ServiceModel.NetNamedPipeBinding(endPoint.BindingConfigurationName);
                                break;
                        }
                        ret = nnpb;
                        break;
                    }
                #endregion
                #region NetPeerTcpBinding
                case WCFBinding.NetPeerTcpBinding:
                    {
                        //WARNING: untested code
                        System.ServiceModel.NetPeerTcpBinding nptb = null;
                        switch (endPoint.BindingSettingType)
                        {
                            case WCFBindingSettingType.Default:
                                nptb = new System.ServiceModel.NetPeerTcpBinding();
                                nptb.ReaderQuotas.MaxStringContentLength = Int32.MaxValue;
                                nptb.Security.Mode = System.ServiceModel.SecurityMode.None;
                                break;
                            case WCFBindingSettingType.UseConfigFile:
                                nptb = new System.ServiceModel.NetPeerTcpBinding(endPoint.BindingConfigurationName);
                                break;
                        }
                        ret = nptb;
                        break;
                    }
                #endregion
                #region NetTcpBinding
                case WCFBinding.NetTcpBinding:
                    {
                        System.ServiceModel.NetTcpBinding ntb = null;
                        switch (endPoint.BindingSettingType)
                        {
                            case WCFBindingSettingType.Default:
                                ntb = new System.ServiceModel.NetTcpBinding();
                                ntb.ReaderQuotas.MaxStringContentLength = Int32.MaxValue;
                                ntb.MaxReceivedMessageSize = Int32.MaxValue;
                                ntb.Security.Mode = System.ServiceModel.SecurityMode.None;
                                break;
                            case WCFBindingSettingType.UseConfigFile:
                                ntb = new System.ServiceModel.NetTcpBinding(endPoint.BindingConfigurationName);
                                break;
                        }
                        ret = ntb;
                        break;
                    }
                #endregion
                #region WSDualHttpBinding
                case WCFBinding.WSDualHttpBinding:
                    {
                        //WARNING: untested code
                        System.ServiceModel.WSDualHttpBinding wdhb = null;
                        switch (endPoint.BindingSettingType)
                        {
                            case WCFBindingSettingType.Default:
                                wdhb = new System.ServiceModel.WSDualHttpBinding();
                                wdhb.ReaderQuotas.MaxStringContentLength = Int32.MaxValue;
                                wdhb.Security.Mode = System.ServiceModel.WSDualHttpSecurityMode.None;
                                break;
                            case WCFBindingSettingType.UseConfigFile:
                                wdhb = new System.ServiceModel.WSDualHttpBinding(endPoint.BindingConfigurationName);
                                break;
                        }
                        ret = wdhb;
                        break;
                    }
                #endregion
                #region WSFederationHttpBinding
                case WCFBinding.WSFederationHttpBinding:
                    {
                        //WARNING: untested code
                        System.ServiceModel.WSFederationHttpBinding wfhb = null;
                        switch (endPoint.BindingSettingType)
                        {
                            case WCFBindingSettingType.Default:
                                wfhb = new System.ServiceModel.WSFederationHttpBinding();
                                wfhb.ReaderQuotas.MaxStringContentLength = Int32.MaxValue;
                                break;
                            case WCFBindingSettingType.UseConfigFile:
                                wfhb = new System.ServiceModel.WSFederationHttpBinding(endPoint.BindingConfigurationName);
                                break;
                        }
                        ret = wfhb;
                        break;
                    }
                #endregion
                #region WSHttpBinding
                case WCFBinding.WSHttpBinding:
                    {
                        System.ServiceModel.WSHttpBinding whb = null;
                        switch (endPoint.BindingSettingType)
                        {
                            case WCFBindingSettingType.Default:
                                whb = new System.ServiceModel.WSHttpBinding();
                                whb.ReaderQuotas.MaxStringContentLength = Int32.MaxValue;
                                whb.MaxReceivedMessageSize = Int32.MaxValue;
                                whb.Security.Mode = System.ServiceModel.SecurityMode.None;
                                break;
                            case WCFBindingSettingType.UseConfigFile:
                                whb = new System.ServiceModel.WSHttpBinding(endPoint.BindingConfigurationName);
                                break;
                        }
                        ret = whb;
                        break;
                    }
                #endregion
            }


            return ret;
        }
        #endregion

        #region Method - SetPublishingServiceHost
        /// <summary>
        /// Sets all the properties to the newly published service point
        /// </summary>
        /// <remarks>Maybe this shuld be done in configuration?? Jure Subara</remarks>
        /// <param name="host"></param>
        public static void SetPublishingServiceHost(System.ServiceModel.ServiceHost host)
        {
            //do not require authorization
            //host.Authorization.ImpersonateCallerForAllOperations = true;
            host.Authorization.PrincipalPermissionMode = System.ServiceModel.Description.PrincipalPermissionMode.None;

            //include exception details
            System.ServiceModel.Description.ServiceDebugBehavior sb = null;
            sb = host.Description.Behaviors[typeof(System.ServiceModel.Description.ServiceDebugBehavior)] as System.ServiceModel.Description.ServiceDebugBehavior;
            if (sb == null)
            {
                sb = new System.ServiceModel.Description.ServiceDebugBehavior();
                host.Description.Behaviors.Add(sb);
            }
            sb.IncludeExceptionDetailInFaults = true;

        }
        #endregion

        //Methods for adding RemotingMechanism info to databse field executor.host
        //When this data has it's own field in database, these methods should no longer be needed

        #region Method - GetHackInfoHostName
        /// <summary>
        /// A mechanism for storing RemotingMechanism information to Hostname field in database.
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="remotingMechanism"></param>
        /// <returns></returns>
        public static string GetHackInfoHostName(string hostName, RemotingMechanism remotingMechanism, string protocol)
        {
            string ret = hostName;
            switch (remotingMechanism)
            {
                case RemotingMechanism.WCF:
                    {
                        ret = String.Format("wcf${0}", hostName);
                        break;
                    }
                case RemotingMechanism.WCFHttp:
                    {
                        if (protocol == "https")
                            ret = String.Format("wcfHttps${0}", hostName);
                        else
                            ret = String.Format("wcfHttp${0}", hostName);
                        break;
                    }
                case RemotingMechanism.WCFTcp:
                    {
                        ret = String.Format("wcfTcp${0}", hostName);
                        break;
                    }
            }

            return ret;
        }
        #endregion

        #region Method - GetHackedInRemotingMechanism
        /// <summary>
        /// A mechanism for storing RemotingMechanism information to Hostname field in database.
        /// </summary>
        /// <param name="hostName"></param>
        /// <returns></returns>
        public static RemotingMechanism GetHackedInRemotingMechanism(string hostName)
        {
            RemotingMechanism ret = RemotingMechanism.TcpBinary;
            string executorRemotingMechanism = string.Empty;

            if (hostName.Contains("$"))
            {
                string[] sArr = hostName.Split('$');
                if (sArr.Length == 2)
                {
                    executorRemotingMechanism = sArr[0];
                    switch (executorRemotingMechanism)
                    {
                        case "wcfHttps":
                        case "wcfHttp":
                            ret = RemotingMechanism.WCFHttp;
                            break;
                        case "wcfTcp":
                            ret = RemotingMechanism.WCFTcp;
                            break;
                        case "wcf":
                            ret = RemotingMechanism.WCF;
                            break;
                    }
                }
            }

            return ret;
        }
        #endregion

        #region Method - GetHackedInHostName
        /// <summary>
        /// A mechanism for storing RemotingMechanism information to Hostname field in database.
        /// </summary>
        /// <param name="hostName"></param>
        /// <returns></returns>
        public static string GetHackedInHostName(string hostName)
        {
            string ret = hostName;
            string executorRemotingMechanism = string.Empty;

            if (hostName.Contains("$"))
            {
                string[] sArr = hostName.Split('$');
                if (sArr.Length == 2)
                {
                    ret = sArr[1];
                }
            }

            return ret;
        }
        #endregion

        #region Method - GetHackedInProtocol
        /// <summary>
        /// A mechanism for storing RemotingMechanism information to Hostname field in database.
        /// </summary>
        /// <param name="hostName"></param>
        /// <returns></returns>
        public static string GetHackedInProtocol(string hostName)
        {
            string ret = string.Empty;
            string executorRemotingMechanism = string.Empty;

            if (hostName.Contains("$"))
            {
                string[] sArr = hostName.Split('$');
                if (sArr.Length == 2)
                {
                    executorRemotingMechanism = sArr[0];
                    switch (executorRemotingMechanism)
                    {
                        case "wcfHttp":
                            ret = "http";
                            break;
                        case "wcfHttps":
                            ret = "https";
                            break;
                        case "wcfTcp":
                            ret = "net.tcp";
                            break;
                        case "wcf":
                            ret = string.Empty;
                            break;
                    }
                }
            }

            return ret;

        }
        #endregion
    }
}
