using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Alchemi.Core.EndPointUtils
{
    /// <summary>
    /// The window for advanced setting of EndPoint element.
    /// </summary>
    public partial class AdvancedEndPointDialog : Form
    {
        #region Constuctor
        /// <summary>
        /// Constructor.
        /// </summary>
        public AdvancedEndPointDialog()
        {
            InitializeComponent();
        }
        #endregion

        #region Public Properties

        #region SelectedRemotingMechanism
        /// <summary>
        /// Selected remoting Mechanism.
        /// </summary>
        public RemotingMechanism SelectedRemotingMechanism
        {
            get
            {
                return ucAdvancedEP.SelectedRemotingMechanism;
            }

            set
            {
                ucAdvancedEP.SelectedRemotingMechanism = value;
            }
        }
        #endregion

        #region Protocol
        public string Protocol
        {
            get
            {
                return ucAdvancedEP.Protocol;
            }

            set
            {
                ucAdvancedEP.Protocol = value;
            }
        }
        #endregion

        #region Host
        public string Host
        {
            get
            {
                return ucAdvancedEP.Host;
            }

            set
            {
                ucAdvancedEP.Host = value;
            }
        }
        #endregion

        #region Port
        /// <summary>
        /// Selected port number.
        /// </summary>
        public int Port
        {
            get
            {
                return ucAdvancedEP.Port;
            }

            set
            {
                ucAdvancedEP.Port = value;
            }
        }
        #endregion

        #region AddressPart
        public string AddressPart
        {
            get
            {
                return ucAdvancedEP.AddressPart;
            }

            set
            {
                ucAdvancedEP.AddressPart = value;
            }
        }
        #endregion

        #region RemotingMechanisms
        public List<RemotingMechanism> RemotingMechanisms
        {
            get
            {
                return ucAdvancedEP.RemotingMechanisms;
            }
            set
            {
                ucAdvancedEP.RemotingMechanisms = value;
            }
        }
        #endregion

        #region WCFBinding
        public WCFBinding WCFBinding
        {
            get
            {
                return ucAdvancedEP.WCFBinding;
            }

            set
            {
                ucAdvancedEP.WCFBinding = value;
            }
        }
        #endregion

        #region FixedAddressPart
        /// <summary>
        /// Shuld the AddressPart be read only ( executor shuld have predefined AddresPart ).
        /// </summary>
        public bool FixedAddressPart
        {
            get
            {
                return ucAdvancedEP.FixedAddressPart;
            }
            set
            {
                ucAdvancedEP.FixedAddressPart = value;
            }
        }
        #endregion

        #region HostNameForPublishing
        public string HostNameForPublishing
        {
            get
            {
                return ucAdvancedEP.HostNameForPublishing;
            }
            set
            {
                ucAdvancedEP.HostNameForPublishing = value;
            }
        }
        #endregion

        #region BindingSettingType
        public WCFBindingSettingType BindingSettingType
        {
            get
            {
                return ucAdvancedEP.BindingSettingType;
            }
            set
            {
                ucAdvancedEP.BindingSettingType = value;
            }
        }
        #endregion

        #region BindingConfigurationName
        public string BindingConfigurationName
        {
            get
            {
                return ucAdvancedEP.BindingConfigurationName;
            }
            set
            {
                ucAdvancedEP.BindingConfigurationName = value;
            }
        }
        #endregion

        #region ServiceConfigurationName
        public string ServiceConfigurationName
        {
            get
            {
                return ucAdvancedEP.ServiceConfigurationName;
            }
            set
            {
                ucAdvancedEP.ServiceConfigurationName = value;
            }
        }
        #endregion

        #endregion        
    }
}