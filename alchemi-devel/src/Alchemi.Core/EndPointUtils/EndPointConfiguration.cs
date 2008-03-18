using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using Alchemi.Core.Utility;

namespace Alchemi.Core.EndPointUtils
{
    /// <summary>
    /// Configuration class for managing EndPoint info in configuration files.
    /// </summary>
    public class EndPointConfiguration
    {
        #region Constructor
        /// <summary>
        /// Parameterles constructor for deserialization only.
        /// </summary>
        public EndPointConfiguration()
        {
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="aRole">For what alchemi node is the EndPointConfiguration beeing used.</param>
        /// <param name="configFileName">What is the name of the config file.</param>
        public EndPointConfiguration(AlchemiRole aRole, string configFileName)
        {
            ConfigFileName = configFileName;
            ConfigFile = Utils.GetFilePath(ConfigFileName, aRole, true);
        }
        /// <summary>
        /// Default constructor. ConfigFileName is set to "Alchemi.EndPoint.config.xml"
        /// </summary>
        /// <param name="aRole">For what alchemi node is the EndPointConfiguration beeing used.</param>
        public EndPointConfiguration(AlchemiRole aRole)
        {
            ConfigFile = Utils.GetFilePath(ConfigFileName, aRole, true);
        }
        #endregion

        #region Serializable members
        /// <summary>
        /// Host name of this end point.
        /// </summary>
        public string Host = "<none>";
        /// <summary>
        /// Port number of the connection to this node.
        /// </summary>
        public int Port = 80;
        /// <summary>
        /// Remoting mechanism used to connect to this node.
        /// </summary>
        public RemotingMechanism RemotingMechanism = RemotingMechanism.WCFCustom;
        /// <summary>
        /// Part of the address that is used on a computer to get the specified instance of this node.
        /// </summary>
        public string LocalAddressPart = "<none>";
        /// <summary>
        /// WCF binding to connect to this node.
        /// </summary>
        public WCFBinding Binding = WCFBinding.None;
        /// <summary>
        /// Protocol to use to connect to this node.
        /// </summary>
        public string Protocol = "<none>";
        /// <summary>
        /// Host name used for publishing this EndPoint.
        /// </summary>
        public string HostNameForPublishing = "localhost";
        /// <summary>
        /// How tho set the Binding used for accessing or publishing this EndPoint.
        /// </summary>
        public WCFBindingSettingType BindingSettingType = WCFBindingSettingType.Default;
        /// <summary>
        /// The name of the binding configuration in app.config.
        /// </summary>
        public string BindingConfigurationName = string.Empty;
        /// <summary>
        /// The name of the service configuration in app.config.
        /// </summary>
        public string ServiceConfigurationName = string.Empty;
        #endregion

        #region NonSerializable members
        private const string ConfigFileNameDefault = "Alchemi.EndPoint.config.xml";
        [NonSerialized]
        private string ConfigFile = "";
        [NonSerialized]
        private string ConfigFileName = ConfigFileNameDefault;
        #endregion

        #region Methods

        #region Public

        #region ResetEndPointFileName
        /// <summary>
        /// Sets the name of the config file to be used.
        /// </summary>
        /// <param name="aRole">For what alchemi node is the EndPointConfiguration beeing used.</param>
        /// <param name="configFileName">What is the name of the config file.</param>
        public void ResetEndPointFileName(AlchemiRole aRole, string configFileName)
        {
            ConfigFileName = configFileName;
            ConfigFile = Utils.GetFilePath(ConfigFileName, aRole, true);
        }
        /// <summary>
        /// Sets the name of the config file to be used.
        /// </summary>
        /// <param name="aRole">For what alchemi node is the EndPointConfiguration beeing used.</param>
        public void ResetEndPointFileName(AlchemiRole aRole)
        {
            ConfigFile = Utils.GetFilePath(ConfigFileName, aRole, true);
        }
        #endregion

        #region GetEndPoint
        public EndPoint GetEndPoint()
        {
            EndPoint ret = new EndPoint();
            ret.Host = this.Host;
            ret.Port = this.Port;
            ret.LocalAddressPart = this.LocalAddressPart;
            ret.Protocol = this.Protocol;
            ret.RemotingMechanism = this.RemotingMechanism;
            ret.Binding = this.Binding;
            ret.HostNameForPublishing = this.HostNameForPublishing;
            ret.BindingSettingType = this.BindingSettingType;
            ret.BindingConfigurationName = this.BindingConfigurationName;
            ret.ServiceConfigurationName = this.ServiceConfigurationName;

            return ret;
        }
        #endregion

        #region Slz
        /// <summary>
        ///  Serialises and saves the configuration to an xml file
        /// </summary>
        public void Slz()
        {
            XmlSerializer xs = new XmlSerializer(typeof(EndPointConfiguration));
            StreamWriter sw = new StreamWriter(ConfigFile);
            xs.Serialize(sw, this);
            sw.Close();
        }
        #endregion

        //static

        #region GetConfiguration
        /// <summary>
        /// Returns the configuration read from the xml file: "Alchemi.Manager.config.xml"
        /// </summary>
        /// <param name="aRole">For what alchemi node is the EndPointConfiguration beeing used.</param>
        /// <returns>Configuration object</returns>
        public static EndPointConfiguration GetConfiguration(AlchemiRole aRole)
        {
            string configFile = Utils.GetFilePath(ConfigFileNameDefault, aRole, true);
            EndPointConfiguration temp = DeSlz(configFile);
            temp.ConfigFileName = ConfigFileNameDefault;
            temp.ConfigFile = configFile;
            return temp;
        }
        /// <summary>
        /// Returns the configuration read from the xml file: "Alchemi.Manager.config.xml"
        /// </summary>
        /// <param name="aRole">For what alchemi node is the EndPointConfiguration beeing used.</param>
        /// <param name="configFileName">What is the name of the config file.</param>
        /// <returns>Configuration object</returns>
        public static EndPointConfiguration GetConfiguration(AlchemiRole aRole, string configFileName)
        {
            string configFile = Utils.GetFilePath(configFileName, aRole, true);
            EndPointConfiguration temp = DeSlz(configFile);
            temp.ConfigFileName = configFileName;
            temp.ConfigFile = configFile;
            return temp;
        }
        #endregion

        #endregion

        #region Protected

        #region ToString
        /// <summary>
        /// overide to string method.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0}By{1}On{2}", Host, Protocol, Port.ToString());
        }
        #endregion

        #endregion

        #region Private

        #region DeSlz
        /// <summary>
        /// Deserialises and reads the configuration from the given xml file
        /// 
        /// Updates:
        /// Jan 18, 2006 - tb@tbiro.com
        ///		Saved the file used to load into ConfigFile so serializing puts the file back to the original location.
        /// </summary>
        /// <param name="file">Name of the config file</param>
        /// <returns>Configuration object</returns>
        private static EndPointConfiguration DeSlz(string file)
        {
            XmlSerializer xs = new XmlSerializer(typeof(EndPointConfiguration));
            FileStream fs = new FileStream(file, FileMode.Open);
            EndPointConfiguration temp = (EndPointConfiguration)xs.Deserialize(fs);
            fs.Close();

            temp.ConfigFile = file;

            return temp;
        }
        #endregion

        #endregion

        #endregion
    }
}
