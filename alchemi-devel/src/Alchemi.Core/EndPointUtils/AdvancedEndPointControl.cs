using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Alchemi.Core.EndPointUtils
{
    /// <summary>
    /// The control for advanced setting of EndPoint element.
    /// </summary>
    public partial class AdvancedEndPointControl : UserControl
    {
        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        public AdvancedEndPointControl()
        {
            InitializeComponent();
            CustomInit();
        }
        #endregion

        #region Properties

        #region Private

        #region AllowedProtocols
        private System.Collections.Generic.List<string> _allowedProtocols = null;
        private System.Collections.Generic.List<string> AllowedProtocols
        {
            get
            {
                if (_allowedProtocols == null)
                    _allowedProtocols = new System.Collections.Generic.List<string>();

                return _allowedProtocols;
            }
        }
        #endregion

        #endregion

        #region Public

        #region SelectedRemotingMechanism
        /// <summary>
        /// Selected remoting Mechanism.
        /// </summary>
        public RemotingMechanism SelectedRemotingMechanism
        {
            get
            {
                RemotingMechanism rm = RemotingMechanism.TcpBinary; //set default remoting mechanism
                switch (cbRemotingMechanism.Text)
                {
                    case "TcpBinary(Remoting)":
                        {
                            rm = RemotingMechanism.TcpBinary;
                            break;
                        }
                    case "Custom(WCF)":
                        {
                            rm = RemotingMechanism.WCFCustom;
                            break;
                        }
                    case "Tcp(WCF)":
                        {
                            rm = RemotingMechanism.WCFTcp;
                            break;
                        }
                    case "Http(WCF)":
                        {
                            rm = RemotingMechanism.WCFHttp;
                            break;
                        }
                    case "WCF":
                        {
                            rm = RemotingMechanism.WCF;
                            break;
                        }
                }

                return rm;
            }

            set
            {
                switch (value)
                {
                    case RemotingMechanism.TcpBinary:
                        {
                            SetTextToComboBox("TcpBinary(Remoting)", cbRemotingMechanism);
                            break;
                        }
                    case RemotingMechanism.WCFCustom:
                        {
                            SetTextToComboBox("Custom(WCF)", cbRemotingMechanism);
                            break;
                        }
                    case RemotingMechanism.WCFHttp:
                        {
                            SetTextToComboBox("Http(WCF)", cbRemotingMechanism);
                            break;
                        }
                    case RemotingMechanism.WCFTcp:
                        {
                            SetTextToComboBox("Tcp(WCF)", cbRemotingMechanism);
                            break;
                        }
                    case RemotingMechanism.WCF:
                        {
                            SetTextToComboBox("WCF", cbRemotingMechanism);
                            break;
                        }
                }
            }
        }
        #endregion

        #region Protocol
        public string Protocol
        {
            get
            {
                return cbProtocol.Text;
            }

            set
            {
                SetTextToComboBox(value, cbProtocol);
            }
        }
        #endregion

        #region Host
        public string Host
        {
            get
            {
                return txHost.Text;
            }

            set
            {
                txHost.Text = value;
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
                int port;
                try
                {
                    port = int.Parse(txPort.Text);
                }
                catch (System.FormatException)
                {
                    MessageBox.Show("Invalid name for 'Port' field.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                }

                return port;
            }

            set
            {
                txPort.Text = value.ToString();
            }
        }
        #endregion

        #region AddressPart
        public string AddressPart
        {
            get
            {
                return txAddresPart.Text;
            }

            set
            {
                txAddresPart.Text = value;
            }
        }
        #endregion

        #region FullAddress
        public string FullAddress
        {
            get
            {
                return txFullAddress.Text;
            }

            set
            {
                txFullAddress.Text = value;
                SetOthersFromFullAddress();
            }
        }
        #endregion

        #region RemotingMechanisms
        private List<RemotingMechanism> _remotingMechanisms;
        public List<RemotingMechanism> RemotingMechanisms
        {
            get
            {
                if (_remotingMechanisms == null)
                {
                    _remotingMechanisms = new List<RemotingMechanism>();
                    _remotingMechanisms.Add(RemotingMechanism.TcpBinary);
                    _remotingMechanisms.Add(RemotingMechanism.WCFCustom);
                    _remotingMechanisms.Add(RemotingMechanism.WCFHttp);
                    _remotingMechanisms.Add(RemotingMechanism.WCFTcp);
                    _remotingMechanisms.Add(RemotingMechanism.WCF);
                }

                return _remotingMechanisms;
            }

            set
            {
                _remotingMechanisms = value;
            }
        }
        #endregion

        #region WCFBinding
        public WCFBinding WCFBinding
        {
            get
            {
                WCFBinding bind = WCFBinding.None;
                switch (cbBinding.Text)
                {
                    case "BasicHttpBinding":
                        bind = WCFBinding.BasicHttpBinding;
                        break;
                    case "WSHttpBinding":
                        bind = WCFBinding.WSHttpBinding;
                        break;
                    case "WSDualHttpBinding":
                        bind = WCFBinding.WSDualHttpBinding;
                        break;
                    case "WSFederationHttpBinding":
                        bind = WCFBinding.WSFederationHttpBinding;
                        break;
                    case "NetTcpBinding":
                        bind = WCFBinding.NetTcpBinding;
                        break;
                    case "NetNamedPipeBinding":
                        bind = WCFBinding.NetNamedPipeBinding;
                        break;
                    case "NetMsmqBinding":
                        bind = WCFBinding.NetMsmqBinding;
                        break;
                    case "NetPeerTcpBinding":
                        bind = WCFBinding.NetPeerTcpBinding;
                        break;
                    case "MsmqIntegrationBinding":
                        bind = WCFBinding.MsmqIntegrationBinding;
                        break;
                }

                return bind;
            }

            set
            {
                switch (value)
                {
                    case WCFBinding.BasicHttpBinding:
                        SetTextToComboBox("BasicHttpBinding", cbBinding);
                        break;
                    case WCFBinding.WSHttpBinding:
                        SetTextToComboBox("WSHttpBinding", cbBinding);
                        break;
                    case WCFBinding.WSDualHttpBinding:
                        SetTextToComboBox("WSDualHttpBinding", cbBinding);
                        break;
                    case WCFBinding.WSFederationHttpBinding:
                        SetTextToComboBox("WSFederationHttpBinding", cbBinding);
                        break;
                    case WCFBinding.NetTcpBinding:
                        SetTextToComboBox("NetTcpBinding", cbBinding);
                        break;
                    case WCFBinding.NetNamedPipeBinding:
                        SetTextToComboBox("NetNamedPipeBinding", cbBinding);
                        break;
                    case WCFBinding.NetMsmqBinding:
                        SetTextToComboBox("NetMsmqBinding", cbBinding);
                        break;
                    case WCFBinding.NetPeerTcpBinding:
                        SetTextToComboBox("NetPeerTcpBinding", cbBinding);
                        break;
                    case WCFBinding.MsmqIntegrationBinding:
                        SetTextToComboBox("MsmqIntegrationBinding", cbBinding);
                        break;
                    case WCFBinding.None:
                        SetTextToComboBox("", cbBinding);
                        break;
                    default:
                        SetTextToComboBox("", cbBinding);
                        break;

                }
            }
        }
        #endregion

        #region FixedAddressPart
        private bool _fixedAddressPart = false;
        /// <summary>
        /// Shuld the AddressPart be read only ( executor shuld have predefined AddresPart ).
        /// </summary>
        public bool FixedAddressPart
        {
            get
            {
                return _fixedAddressPart;
            }
            set
            {
                _fixedAddressPart = value;
                txAddresPart.Enabled = !_fixedAddressPart;
            }
        }
        #endregion

        #region HostNameForPublishing
        public string HostNameForPublishing
        {
            get
            {
                return txtHostNameForPublish.Text;
            }
            set
            {
                txtHostNameForPublish.Text = value;
            }
        }
        #endregion

        #region BindingSettingType
        public WCFBindingSettingType BindingSettingType
        {
            get
            {
                WCFBindingSettingType ret = WCFBindingSettingType.None;
                switch (cbBindingSettingsType.Text)
                {
                    case "Default":
                        ret = WCFBindingSettingType.Default;
                        break;
                    case "Config File":
                        ret = WCFBindingSettingType.UseConfigFile;
                        break;
                }

                return ret;
            }
            set
            {
                switch (value)
                {
                    case WCFBindingSettingType.Default:
                        cbBindingSettingsType.Text = "Default";
                        break;
                    case WCFBindingSettingType.UseConfigFile:
                        cbBindingSettingsType.Text = "Config File";
                        break;
                }
            }
        }
        #endregion

        #region BindingConfigurationName
        public string BindingConfigurationName
        {
            get
            {
                return txtBindingConfigurationName.Text;
            }
            set
            {
                txtBindingConfigurationName.Text = value;
            }
        }
        #endregion

        #region ServiceConfigurationName
        public string ServiceConfigurationName
        {
            get
            {
                return txtServiceConfigurationName.Text;
            }
            set
            {
                txtServiceConfigurationName.Text = value;
            }
        }
        #endregion

        #endregion

        #endregion

        #region Methods

        #region Private

        //set form for selected remoting mechanism

        #region ShowForCustomWCF
        private void ShowForCustomWCF()
        {
            txAddresPart.Enabled = false;
            cbProtocol.Enabled = false;
            txFullAddress.Enabled = false;
            txHost.Enabled = false;
            txPort.Enabled = false;

            WCFBinding = WCFBinding.None;
            cbBinding.Enabled = false;
            txtServiceConfigurationName.Enabled = true;
            cbBindingSettingsType.Enabled = false;
            txtBindingConfigurationName.Enabled = false;
            txtHostNameForPublish.Enabled = false;

            cbProtocol.Items.Clear();
            cbProtocol.Items.Add("net.pipe");
            cbProtocol.Items.Add("http");
            cbProtocol.Items.Add("https");
            cbProtocol.Items.Add("net.p2p");
            cbProtocol.Items.Add("net.msmq");
            cbProtocol.Items.Add("net.tcp");

            AllowedProtocols.Clear();
            AllowedProtocols.Add("<none>");
            AllowedProtocols.Add("net.pipe");
            AllowedProtocols.Add("http");
            AllowedProtocols.Add("https");
            AllowedProtocols.Add("net.p2p");
            AllowedProtocols.Add("net.msmq");
            AllowedProtocols.Add("net.tcp");

            SetFullAddressFromOther();
        }
        #endregion

        #region ShowForWCFTcp
        private void ShowForWCFTcp()
        {
            txAddresPart.Enabled = !_fixedAddressPart;
            cbProtocol.Enabled = false;
            txFullAddress.Enabled = true;
            txPort.Enabled = true;
            txHost.Enabled = true;
            txtServiceConfigurationName.Enabled = false;
            cbBindingSettingsType.Enabled = true;
            txtBindingConfigurationName.Enabled = (BindingSettingType == WCFBindingSettingType.UseConfigFile);
            txtHostNameForPublish.Enabled = true;

            WCFBinding = WCFBinding.NetTcpBinding;
            cbBinding.Enabled = false;

            AllowedProtocols.Clear();
            AllowedProtocols.Add("net.tcp");
            cbProtocol.Text = "net.tcp";

            SetFullAddressFromOther();
        }
        #endregion

        #region ShowForWCFHttp
        private void ShowForWCFHttp()
        {
            txAddresPart.Enabled = !_fixedAddressPart;
            cbProtocol.Enabled = true;
            txFullAddress.Enabled = true;
            txPort.Enabled = true;
            txHost.Enabled = true;
            txtServiceConfigurationName.Enabled = false;
            cbBindingSettingsType.Enabled = true;
            txtBindingConfigurationName.Enabled = (BindingSettingType == WCFBindingSettingType.UseConfigFile);
            txtHostNameForPublish.Enabled = true;

            WCFBinding = WCFBinding.WSHttpBinding;
            cbBinding.Enabled = false;

            cbProtocol.Items.Clear();
            cbProtocol.Items.Add("http");
            cbProtocol.Items.Add("https");
            cbProtocol.Text = "http";

            AllowedProtocols.Clear();
            AllowedProtocols.Add("http");
            AllowedProtocols.Add("https");

            SetFullAddressFromOther();
        }
        #endregion

        #region ShowForWCF
        private void ShowForWCF()
        {
            txAddresPart.Enabled = true;
            cbProtocol.Enabled = true;
            txFullAddress.Enabled = true;
            txPort.Enabled = true;
            txHost.Enabled = true;
            cbBinding.Enabled = true;
            txtServiceConfigurationName.Enabled = false;
            cbBindingSettingsType.Enabled = true;
            txtBindingConfigurationName.Enabled = (BindingSettingType == WCFBindingSettingType.UseConfigFile);
            txtHostNameForPublish.Enabled = true;

            cbProtocol.Items.Clear();
            cbProtocol.Items.Add("net.pipe");
            cbProtocol.Items.Add("http");
            cbProtocol.Items.Add("https");
            cbProtocol.Items.Add("net.p2p");
            cbProtocol.Items.Add("net.msmq");
            cbProtocol.Items.Add("net.tcp");

            AllowedProtocols.Clear();
            AllowedProtocols.Add("<none>");
            AllowedProtocols.Add("net.pipe");
            AllowedProtocols.Add("http");
            AllowedProtocols.Add("https");
            AllowedProtocols.Add("net.p2p");
            AllowedProtocols.Add("net.msmq");
            AllowedProtocols.Add("net.tcp");

            SetFullAddressFromOther();
        }
        #endregion

        #region ShowForTcpBinaryRemoting
        private void ShowForTcpBinaryRemoting()
        {
            txHost.Enabled = true;
            txPort.Enabled = true;
            txAddresPart.Enabled = false;
            cbProtocol.Enabled = false;
            txFullAddress.Enabled = false;
            txtServiceConfigurationName.Enabled = false;
            cbBindingSettingsType.Enabled = false;
            txtBindingConfigurationName.Enabled = false;
            txtHostNameForPublish.Enabled = false;

            WCFBinding = WCFBinding.None;
            cbBinding.Enabled = false;
        }
        #endregion

        //Helper methods for parsing full address from address part and the other way

        #region SetFullAddressFromOther
        private void SetFullAddressFromOther()
        {
            txFullAddress.Text = Utility.WCFUtils.ComposeAddress(cbProtocol.Text, txHost.Text, Port, txAddresPart.Text, WCFBinding);
        }
        #endregion

        #region SetOthersFromFullAddress
        private void SetOthersFromFullAddress()
        {
            string _protocol = "<none>";
            string _host = "<none>";
            int _port = 0;
            string _pathOnServer = string.Empty;

            Utility.WCFUtils.BreakAddress(txFullAddress.Text, out _protocol, out _host, out _port, out _pathOnServer);

            txHost.Text = _host;
            txPort.Text = _port.ToString();
            cbProtocol.Text = _protocol;
            txAddresPart.Text = _pathOnServer;
        }
        #endregion

        //other helper functions

        #region CustomInit
        private void CustomInit()
        {
            BindRemotingMechanisms();
            BindWCFBindings();
            Clear();
        }
        #endregion

        #region CalculateLastMatchIndex
        int CalculateLastMatchIndex(string input, string word)
        {
            int index = 0;
            int checkLen = word.Length + 1;
            if (input.Length + 1 < checkLen)
                checkLen = input.Length + 1;

            for (int i = 1; i < checkLen; i++)
            {
                if (input.ToLower().Substring(0, i) == word.ToLower().Substring(0, i))
                    index = i;
            }

            return index;
        }
        #endregion

        #region AddRemotingMechanismToCb
        private void AddRemotingMechanismToCb(RemotingMechanism remotingMechanism)
        {
            string fName = string.Empty;
            switch (remotingMechanism)
            {
                case RemotingMechanism.TcpBinary:
                    fName = "TcpBinary(Remoting)";
                    break;
                case RemotingMechanism.WCFCustom:
                    fName = "Custom(WCF)";
                    break;
                case RemotingMechanism.WCFHttp:
                    fName = "Http(WCF)";
                    break;
                case RemotingMechanism.WCFTcp:
                    fName = "Tcp(WCF)";
                    break;
                case RemotingMechanism.WCF:
                    fName = "WCF";
                    break;
            }

            cbRemotingMechanism.Items.Add(fName);
        }
        #endregion

        #region BindWCFBindings
        private void BindWCFBindings()
        {
            cbBinding.Items.Clear();
            cbBinding.Items.Add("BasicHttpBinding");
            cbBinding.Items.Add("WSHttpBinding");
            cbBinding.Items.Add("WSDualHttpBinding");
            cbBinding.Items.Add("WSFederationHttpBinding");
            cbBinding.Items.Add("NetTcpBinding");
            cbBinding.Items.Add("NetNamedPipeBinding");
            cbBinding.Items.Add("NetMsmqBinding");
            cbBinding.Items.Add("NetPeerTcpBinding");
            cbBinding.Items.Add("MsmqIntegrationBinding");
        }
        #endregion

        #endregion

        #region Public

        #region BindRemotingMechanisms
        /// <summary>
        /// Need to call after setting RemotingMechanisms list, for the control to get updated.
        /// </summary>
        /// <remarks>Functions AddRemotingMechanism and RemoveRemotingMechanism call this functios internaly.</remarks>
        public void BindRemotingMechanisms()
        {
            cbRemotingMechanism.Items.Clear();
            for (int i = 0; i < RemotingMechanisms.Count; i++)
            {
                AddRemotingMechanismToCb(RemotingMechanisms[i]);
            }
        }
        #endregion

        #region AddRemotingMechanism
        /// <summary>
        /// Adds another remoting mechanism.
        /// </summary>
        /// <param name="remotingMechanism">The remoting mechanism to add.</param>
        public void AddRemotingMechanism(RemotingMechanism remotingMechanism)
        {
            if (!RemotingMechanisms.Contains(remotingMechanism))
                RemotingMechanisms.Add(remotingMechanism);

            BindRemotingMechanisms();
        }
        #endregion

        #region RemoveRemotingMechanism
        /// <summary>
        /// Remove an existing remoting mechanism.
        /// </summary>
        /// <param name="remotingMechanism">The remoting mechanism to remove.</param>
        public void RemoveRemotingMechanism(RemotingMechanism remotingMechanism)
        {
            if (RemotingMechanisms.Contains(remotingMechanism))
                RemotingMechanisms.Remove(remotingMechanism);

            BindRemotingMechanisms();
        }
        #endregion

        #region Clear
        /// <summary>
        /// Reset the control to default values.
        /// </summary>
        public void Clear()
        {
            this.Host = "localhost";
            this.Port = 0;
            this.AddressPart = string.Empty;
            this.SelectedRemotingMechanism = RemotingMechanism.WCF;
            this.Protocol = string.Empty;
            this.WCFBinding = WCFBinding.None;
            this.HostNameForPublishing = "localhost";
            this.BindingSettingType = WCFBindingSettingType.Default;
            this.BindingConfigurationName = string.Empty;
            this.ServiceConfigurationName = string.Empty;
        }
        #endregion

        #endregion

        #endregion

        #region Handlers

        #region txPort_KeyDown
        private void txPort_KeyDown(object sender, KeyEventArgs e)
        {
            char c = Convert.ToChar(e.KeyCode);
            if (!Char.IsPunctuation(c) && !Char.IsNumber(c) && !Char.IsControl(c))
                e.SuppressKeyPress = true;
        }
        #endregion

        #region cbRemotingMechanism_SelectedIndexChanged
        private void cbRemotingMechanism_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (SelectedRemotingMechanism)
            {
                case RemotingMechanism.TcpBinary:
                    {
                        ShowForTcpBinaryRemoting();
                        break;
                    }
                case RemotingMechanism.WCFCustom:
                    {
                        ShowForCustomWCF();
                        break;
                    }
                case RemotingMechanism.WCFHttp:
                    {
                        ShowForWCFHttp();
                        break;
                    }
                case RemotingMechanism.WCFTcp:
                    {
                        ShowForWCFTcp();
                        break;
                    }
                case RemotingMechanism.WCF:
                    {
                        ShowForWCF();
                        break;
                    }
            }
        }
        #endregion

        #region cbProtocol_SelectedIndexChanged
        private void cbProtocol_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedRemotingMechanism != RemotingMechanism.TcpBinary)
                SetFullAddressFromOther();
        }
        #endregion

        #region txHost_TextChanged
        private void txHost_TextChanged(object sender, EventArgs e)
        {
            if (SelectedRemotingMechanism != RemotingMechanism.TcpBinary)
                SetFullAddressFromOther();
        }
        #endregion

        #region txPort_TextChanged
        private void txPort_TextChanged(object sender, EventArgs e)
        {
            if (SelectedRemotingMechanism != RemotingMechanism.TcpBinary)
                SetFullAddressFromOther();
        }
        #endregion

        #region txAddresPart_TextChanged
        private void txAddresPart_TextChanged(object sender, EventArgs e)
        {
            if (SelectedRemotingMechanism != RemotingMechanism.TcpBinary)
                SetFullAddressFromOther();
        }
        #endregion

        #region txFullAddress_TextChanged
        private void txFullAddress_TextChanged(object sender, EventArgs e)
        {
            if (isInEvent)
                return;

            int curLocation = txFullAddress.SelectionStart;

            string willBeStr = txFullAddress.Text;
            int protCount = AllowedProtocols.Count;
            if (protCount > 0)
            {
                int lastCorrectIndex = 0;
                bool isOfType = false;
                for (int i = 0; i < protCount && !isOfType; i++)
                {
                    string word = String.Format("{0}://", AllowedProtocols[i]);
                    int wordLen = word.Length;
                    if (willBeStr.Length < wordLen)
                        wordLen = willBeStr.Length;
                    if (curLocation < wordLen)
                        wordLen = curLocation;
                    if (willBeStr.Substring(0, wordLen).ToLower() == word.Substring(0, wordLen).ToLower())
                    {
                        isOfType = true;
                    }
                    else
                    {
                        int curCorIndex = CalculateLastMatchIndex(previousText, word);
                        if (curCorIndex > lastCorrectIndex)
                            lastCorrectIndex = curCorIndex;
                    }

                }

                if (!isOfType)
                {
                    isInEvent = true;
                    txFullAddress.Text = previousText;
                    isInEvent = false;
                    txFullAddress.SelectionStart = lastCorrectIndex;
                }
                else
                {
                    previousText = txFullAddress.Text;
                }
            }
        }
        //event helper members
        string previousText = string.Empty;
        bool isInEvent = false;
        #endregion

        #region cbProtocol_Leave
        private void cbProtocol_Leave(object sender, EventArgs e)
        {
            if (SelectedRemotingMechanism != RemotingMechanism.TcpBinary)
                SetFullAddressFromOther();
        }
        #endregion

        #region txHost_Leave
        private void txHost_Leave(object sender, EventArgs e)
        {
            if (SelectedRemotingMechanism != RemotingMechanism.TcpBinary)
                SetFullAddressFromOther();
        }
        #endregion

        #region txPort_Leave
        private void txPort_Leave(object sender, EventArgs e)
        {
            if (SelectedRemotingMechanism != RemotingMechanism.TcpBinary)
                SetFullAddressFromOther();
        }
        #endregion

        #region txAddresPart_Leave
        private void txAddresPart_Leave(object sender, EventArgs e)
        {
            if (SelectedRemotingMechanism != RemotingMechanism.TcpBinary)
                SetFullAddressFromOther();
        }
        #endregion

        #region txFullAddress_Leave
        private void txFullAddress_Leave(object sender, EventArgs e)
        {
            if (SelectedRemotingMechanism != RemotingMechanism.TcpBinary)
                SetOthersFromFullAddress();
        }
        #endregion

        #region cbBindingSettingsType_TabIndexChanged
        private void cbBindingSettingsType_TabIndexChanged(object sender, EventArgs e)
        {
            txtBindingConfigurationName.Enabled = (BindingSettingType == WCFBindingSettingType.UseConfigFile);
        }
        #endregion

        #endregion

        #region Cross-thred helpers

        private delegate void SetTextToComboBoxDel(string text, ComboBox cbControl);

        private void SetTextToComboBox(string text, ComboBox cbControl)
        {
            if (cbControl.InvokeRequired)
            {
                SetTextToComboBoxDel method = new SetTextToComboBoxDel(SetTextToComboBox);
                cbControl.Invoke(method, text, cbControl);
            }
            else
            {
                cbControl.Text = text;
            }
        }

        #endregion

    }
}
