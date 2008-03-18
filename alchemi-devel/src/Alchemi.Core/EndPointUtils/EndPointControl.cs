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
    /// User control for setting of EndPoint element.
    /// It has only simple settings, advanced settings are available by link to a window with advanced settings.
    /// </summary>
    public partial class EndPointControl : UserControl
    {
        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        public EndPointControl()
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
        private string _Protocol = "http";
        public string Protocol
        {
            get
            {
                return _Protocol;
            }

            set
            {
                _Protocol = value;
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
        private string _AddressPart = "Node";
        public string AddressPart
        {
            get
            {
                return _AddressPart;
            }

            set
            {
                _AddressPart = value;
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
        }
        #endregion

        #region WCFBinding
        private WCFBinding _WCFBinding = WCFBinding.None;
        public WCFBinding WCFBinding
        {
            get
            {
                return _WCFBinding;
            }

            set
            {
                _WCFBinding = value;
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
            }
        }
        #endregion

        #region HostNameForPublishing
        private string _HostNameForPublishing = "localhost";
        public string HostNameForPublishing
        {
            get
            {
                return _HostNameForPublishing;
            }
            set
            {
                _HostNameForPublishing = value;
            }
        }
        #endregion

        #region BindingSettingType
        private WCFBindingSettingType _BindingSettingType = WCFBindingSettingType.Default;
        public WCFBindingSettingType BindingSettingType
        {
            get
            {
                return _BindingSettingType;
            }
            set
            {
                _BindingSettingType = value;
            }
        }
        #endregion

        #region BindingConfigurationName
        private string _BindingConfigurationName = string.Empty;
        public string BindingConfigurationName
        {
            get
            {
                return _BindingConfigurationName;
            }
            set
            {
                _BindingConfigurationName = value;
            }
        }
        #endregion

        #region ServiceConfigurationName
        private string _ServiceConfigurationName = string.Empty;
        public string ServiceConfigurationName
        {
            get
            {
                return _ServiceConfigurationName;
            }
            set
            {
                _ServiceConfigurationName = value;
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
            txFullAddress.Enabled = false;
            txHost.Enabled = false;
            txPort.Enabled = false;

            WCFBinding = WCFBinding.None;

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
            txFullAddress.Enabled = true;
            txPort.Enabled = true;
            txHost.Enabled = true;

            WCFBinding = WCFBinding.NetTcpBinding;

            AllowedProtocols.Clear();
            AllowedProtocols.Add("net.tcp");
            Protocol = "net.tcp";

            SetFullAddressFromOther();
        }
        #endregion

        #region ShowForWCFHttp
        private void ShowForWCFHttp()
        {
            txFullAddress.Enabled = true;
            txPort.Enabled = true;
            txHost.Enabled = true;

            WCFBinding = WCFBinding.WSHttpBinding;

            Protocol = "http";

            AllowedProtocols.Clear();
            AllowedProtocols.Add("http");
            AllowedProtocols.Add("https");

            SetFullAddressFromOther();
        }
        #endregion

        #region ShowForWCF
        private void ShowForWCF()
        {
            txFullAddress.Enabled = true;
            txPort.Enabled = true;
            txHost.Enabled = true;

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
            txFullAddress.Enabled = false;

            WCFBinding = WCFBinding.None;
        }
        #endregion

        //Helper methods for parsing full address from address part and the other way

        #region SetFullAddressFromOther
        private void SetFullAddressFromOther()
        {
            txFullAddress.Text = Utility.WCFUtils.ComposeAddress(Protocol, txHost.Text, Port, AddressPart, WCFBinding);
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
            Protocol = _protocol;
            AddressPart = _pathOnServer;
        }
        #endregion

        //other helper functions

        #region CustomInit
        private void CustomInit()
        {
            BindRemotingMechanisms();
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

        #region GetEndPoint
        /// <summary>
        /// Returns a new EndPoint instance. Configured as the control is set.
        /// </summary>
        /// <returns></returns>
        public EndPoint GetEndPoint()
        {
            EndPoint ret = new EndPoint();
            ret.Host = this.Host;
            ret.Port = this.Port;
            ret.LocalAddressPart = this.AddressPart;
            ret.Protocol = this.Protocol;
            ret.RemotingMechanism = this.SelectedRemotingMechanism;
            ret.Binding = this.WCFBinding;
            ret.BindingSettingType = this.BindingSettingType;
            ret.BindingConfigurationName = this.BindingConfigurationName;
            ret.ServiceConfigurationName = this.ServiceConfigurationName;
            ret.HostNameForPublishing = this.HostNameForPublishing;

            return ret;
        }
        #endregion

        #region WriteEndPointConfiguration
        /// <summary>
        /// Write settings from this control to EndPointConfiguration instance.
        /// </summary>
        /// <param name="epConf">EndPointConfiguration instance to set.</param>
        public void WriteEndPointConfiguration(EndPointConfiguration epConf)
        {
            epConf.Host = this.Host;
            epConf.Port = this.Port;
            epConf.LocalAddressPart = this.AddressPart;
            epConf.Protocol = this.Protocol;
            epConf.RemotingMechanism = this.SelectedRemotingMechanism;
            epConf.Binding = this.WCFBinding;
            epConf.BindingSettingType = this.BindingSettingType;
            epConf.BindingConfigurationName = this.BindingConfigurationName;
            epConf.ServiceConfigurationName = this.ServiceConfigurationName;
            epConf.HostNameForPublishing = this.HostNameForPublishing;
        }
        #endregion

        #region ReadEndPointConfiguration
        /// <summary>
        /// Set the values of this control acording to the EndPointConfiguration instance.
        /// </summary>
        /// <param name="epConf">EndPointConfiguration instance to read from.</param>
        public void ReadEndPointConfiguration(EndPointConfiguration epConf)
        {
            this.Host = epConf.Host;
            this.Port = epConf.Port;
            this.AddressPart = epConf.LocalAddressPart;
            this.SelectedRemotingMechanism = epConf.RemotingMechanism;
            this.Protocol = epConf.Protocol;
            this.WCFBinding = epConf.Binding;
            this.BindingSettingType = epConf.BindingSettingType;
            this.BindingConfigurationName = epConf.BindingConfigurationName;
            this.ServiceConfigurationName = epConf.ServiceConfigurationName;
            this.HostNameForPublishing = epConf.HostNameForPublishing;
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

        #region txFullAddress_Leave
        private void txFullAddress_Leave(object sender, EventArgs e)
        {
            if (SelectedRemotingMechanism != RemotingMechanism.TcpBinary)
                SetOthersFromFullAddress();
        }
        #endregion

        #region lnkAdvanced_LinkClicked
        private void lnkAdvanced_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AdvancedEndPointDialog aepd = new AdvancedEndPointDialog();
            aepd.SelectedRemotingMechanism = SelectedRemotingMechanism;
            aepd.Protocol = Protocol;
            aepd.Host = Host;
            aepd.Port = Port;
            aepd.AddressPart = AddressPart;
            aepd.RemotingMechanisms = RemotingMechanisms;
            aepd.WCFBinding = WCFBinding;
            aepd.FixedAddressPart = FixedAddressPart;
            aepd.BindingSettingType = BindingSettingType;
            aepd.BindingConfigurationName = BindingConfigurationName;
            aepd.ServiceConfigurationName = ServiceConfigurationName;
            aepd.HostNameForPublishing = HostNameForPublishing;

            if (aepd.ShowDialog() == DialogResult.OK)
            {
                SelectedRemotingMechanism = aepd.SelectedRemotingMechanism;
                Protocol = aepd.Protocol;
                Host = aepd.Host;
                Port = aepd.Port;
                AddressPart = aepd.AddressPart;
                WCFBinding = aepd.WCFBinding;
                FixedAddressPart = aepd.FixedAddressPart;
                BindingSettingType = aepd.BindingSettingType;
                BindingConfigurationName = aepd.BindingConfigurationName;
                ServiceConfigurationName = aepd.ServiceConfigurationName;
                HostNameForPublishing = aepd.HostNameForPublishing;
            }

            aepd.Dispose();
            aepd = null;
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
