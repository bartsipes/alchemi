using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Alchemi.Manager;
using Alchemi.Manager.Storage;

namespace Alchemi.ManagerRunner
{
    public partial class ConfigurationForm : Form
    {
        int _port = 9000;
        ManagerStorageEnum _managerStorageEnum = ManagerStorageEnum.InMemory;
        string _serverName = string.Empty;
        string _databaseName = string.Empty;
        string _username = string.Empty;
        string _password = string.Empty;

        public ConfigurationForm()
        {
            InitializeComponent();

            this.Initialize();

            this.UpdateUi();
        }

        void Initialize()
        {
            List<KeyValuePair<ManagerStorageEnum, string>> storageTypes = new List<KeyValuePair<ManagerStorageEnum, string>>();

            storageTypes.Add( new KeyValuePair<ManagerStorageEnum, string>( ManagerStorageEnum.InMemory, "In Memory" ) );
            storageTypes.Add( new KeyValuePair<ManagerStorageEnum, string>( ManagerStorageEnum.MySql, "MySql" ) );
            storageTypes.Add( new KeyValuePair<ManagerStorageEnum, string>( ManagerStorageEnum.SqlServer, "SQL Server" ) );

            this.uiStorageMethodComboBox.DisplayMember = "Value";
            this.uiStorageMethodComboBox.ValueMember = "Key";
            this.uiStorageMethodComboBox.DataSource = storageTypes;
        }

        void UpdateUi()
        {
            // port
            this.uiConnectionPortTextBox.Text = this._port.ToString();

            // manager storage
            this.uiStorageMethodComboBox.SelectedValue = this._managerStorageEnum;
            switch( this._managerStorageEnum )
            {
                case ManagerStorageEnum.InMemory:
                    this.uiServerNameLabel.Enabled = false;
                    this.uiServerNameTextBox.Enabled = false;
                    this.uiDatabaseNameLabel.Enabled = false;
                    this.uiDatabaseNameTextBox.Enabled = false;
                    this.uiUsernameLabel.Enabled = false;
                    this.uiUsernameTextBox.Enabled = false;
                    this.uiPasswordLabel.Enabled = false;
                    this.uiPasswordTextBox.Enabled = false;
                    break;

                case ManagerStorageEnum.MySql:
                case ManagerStorageEnum.SqlServer:
                    this.uiServerNameLabel.Enabled = true;
                    this.uiServerNameTextBox.Enabled = true;
                    this.uiDatabaseNameLabel.Enabled = true;
                    this.uiDatabaseNameTextBox.Enabled = true;
                    this.uiUsernameLabel.Enabled = true;
                    this.uiUsernameTextBox.Enabled = true;
                    this.uiPasswordLabel.Enabled = true;
                    this.uiPasswordTextBox.Enabled = true;
                    break;
            }

            // server name
            this.uiServerNameTextBox.Text = this._serverName;

            // database name
            this.uiDatabaseNameTextBox.Text = this._databaseName;

            // username
            this.uiUsernameTextBox.Text = this._username;

            // password
            this.uiPasswordTextBox.Text = this._password;
        }

        public void SetConfiguration( Alchemi.Manager.Configuration configuration )
        {
            this._port = configuration.OwnPort;
            this._managerStorageEnum = configuration.DbType;
            this._serverName = configuration.DbServer;
            this._databaseName = configuration.DbName;
            this._username = configuration.DbUsername;
            this._password = configuration.DbPassword;

            this.UpdateUi();
        }

        private void uiStorageMethodComboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            this._managerStorageEnum = (ManagerStorageEnum) this.uiStorageMethodComboBox.SelectedValue;

            this.UpdateUi();
        }

        public Configuration GetConfiguration()
        {
            Alchemi.Manager.Configuration configuration = new Configuration();

            configuration.DbType = this._managerStorageEnum;
            configuration.DbServer = this._serverName;
            configuration.DbName = this._databaseName;
            configuration.DbUsername = this._username;
            configuration.DbPassword = this._password;

            configuration.OwnPort = this._port;
            configuration.ManagerHost = "localhost";
            configuration.ManagerPort = 0;
            configuration.Intermediate = false;
            configuration.Dedicated = false;
            configuration.Id = "";

            return configuration;
        }

        private void uiConnectionPortTextBox_TextChanged( object sender, EventArgs e )
        {
            try
            {
                this._port = int.Parse( this.uiConnectionPortTextBox.Text );
                this.uiErrorProvider.SetError( this.uiConnectionPortTextBox, "" );
            }
            catch( Exception ex )
            {
                this.uiErrorProvider.SetError( this.uiConnectionPortTextBox, ex.Message );
            }
        }

        private void uiServerNameTextBox_TextChanged( object sender, EventArgs e )
        {
            this._serverName = this.uiServerNameTextBox.Text;
        }

        private void uiDatabaseNameTextBox_TextChanged( object sender, EventArgs e )
        {
            this._databaseName = this.uiDatabaseNameTextBox.Text;
        }

        private void uiUsernameTextBox_TextChanged( object sender, EventArgs e )
        {
            this._username = this.uiUsernameTextBox.Text;
        }

        private void uiPasswordTextBox_TextChanged( object sender, EventArgs e )
        {
            this._password = this.uiPasswordTextBox.Text;
        }

        private void uiOkButton_Click( object sender, EventArgs e )
        {
            this.Close();
        }

        private void uiCancelButton_Click( object sender, EventArgs e )
        {
            this.Close();
        }
    }
}