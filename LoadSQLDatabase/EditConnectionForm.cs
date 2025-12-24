//   Copyright 2025 Anthony W. van Leeuwen
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace LoadSQLDatabase
{
    /// <summary>
    /// The EditConnectionForm is used to enter data elements used to build the connection string
    /// to the database.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class EditConnectionForm : Form
    {
        // the Connection string and other connection variables are stored
        // in the user.config file in the users's appdata/local/LoadSQLDatabase folder
        private string ConnectionString;

        private string MyDataSourceName;
        private string MyDatabaseName;
        private string MyUserID;
        private string MyPassword;
        private bool IntegratedSecurityFlag;
        private bool EncryptDataFlag;
        private bool TrustServerCertificateFlag;
        private bool UseWindowsAuthenticationFlag;

        private bool ConnectionChangedFlag;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditConnectionForm"/> class.
        /// </summary>
        public EditConnectionForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Load event of the EditConnectionForm.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void EditConnectionForm_Load(object sender, EventArgs e)
        {
            // Load Connection data from the settings file
            LoadConnectionData();
            ConnectionChangedFlag = false;
        }

        /// <summary>
        /// Loads the connection data and populates form controls.
        /// </summary>
        private void LoadConnectionData()
        {
            // Load local variables from settings that comprise components of the connection string
            ConnectionString = LoadSQLDatabase.Properties.Settings.Default.DatabaseConnectionString;
            MyDataSourceName = LoadSQLDatabase.Properties.Settings.Default.DataSourceName;
            MyDatabaseName = LoadSQLDatabase.Properties.Settings.Default.DatabaseName;
            MyUserID = LoadSQLDatabase.Properties.Settings.Default.UserID;
            MyPassword = LoadSQLDatabase.Properties.Settings.Default.Password;
            IntegratedSecurityFlag = LoadSQLDatabase.Properties.Settings.Default.IntegratedSecurity;
            EncryptDataFlag = LoadSQLDatabase.Properties.Settings.Default.EncryptData;
            TrustServerCertificateFlag = LoadSQLDatabase.Properties.Settings.Default.TrustServerCertificate;
            UseWindowsAuthenticationFlag = LoadSQLDatabase.Properties.Settings.Default.UseWindowsAuthentication;

            // display components of the connection string on the form's controls
            DataSourceTxtBx.Text = MyDataSourceName.ToString();
            ConnectionStringTxtBx.Text = ConnectionString.ToString();
            DatabaseNameTxtBx.Text = MyDatabaseName.ToString();
            IntegratedSecurityCBx.Checked = IntegratedSecurityFlag;
            TrustServerCerticateCBx.Checked = TrustServerCertificateFlag;
            EncryptDataCBx.Checked = EncryptDataFlag;
            UseWindowsAuthenticationCBx.Checked = UseWindowsAuthenticationFlag;
            UserIDTxtBx.Text = MyUserID.ToString();
            PasswordTxtBx.Text = MyPassword.ToString();

            // Hide textboxes for userID and Password when using windows authentication
            if (UseWindowsAuthenticationFlag == true)
            {
                UserIDTxtBx.Enabled = false;
                PasswordTxtBx.Enabled = false;
            }
            // set ConnectionChangedFlag to false
            ConnectionChangedFlag = false;
        }

        /// <summary>
        /// Saves the connection data to settings file. User settings are saved to the user's
        /// user.config file located in the user's Appdata/local/LoadSQLDatabase folder.
        /// </summary>
        private void SaveConnectionData()
        {
            // if textbox value changed then save to forms global variable
            if (!string.IsNullOrEmpty(DataSourceTxtBx.Text))
            {
                if (!string.Equals(MyDataSourceName, DataSourceTxtBx.Text, StringComparison.OrdinalIgnoreCase))
                {
                    MyDataSourceName = DataSourceTxtBx.Text;
                }
            }

            // if textbox value changed then save to forms global variable
            if (!string.IsNullOrEmpty(DatabaseNameTxtBx.Text))
            {
                if (!string.Equals(MyDatabaseName, DatabaseNameTxtBx.Text, StringComparison.OrdinalIgnoreCase))
                {
                    MyDatabaseName = DatabaseNameTxtBx.Text;
                }
            }

            // if textbox value changed then save to forms global variable
            if (!string.IsNullOrEmpty(ConnectionStringTxtBx.Text))
            {
                if (!string.Equals(ConnectionString, ConnectionStringTxtBx.Text, StringComparison.OrdinalIgnoreCase))
                {
                    ConnectionString = ConnectionStringTxtBx.Text;
                }
            }

            // if textbox value changed then save to forms global variable
            if (!string.IsNullOrEmpty(UserIDTxtBx.Text) && UseWindowsAuthenticationFlag)
            {
                MyUserID = UserIDTxtBx.Text;
            }
            else
            {
                MyUserID = string.Empty;
            }

            // if textbox value changed then save to forms global variable
            if (!string.IsNullOrEmpty(PasswordTxtBx.Text) && UseWindowsAuthenticationFlag)
            {
                MyPassword = PasswordTxtBx.Text;
            }
            else
            {
                MyPassword = string.Empty;
            }

            // save all connection variables to the settings file
            LoadSQLDatabase.Properties.Settings.Default.DataSourceName = MyDataSourceName;
            LoadSQLDatabase.Properties.Settings.Default.DatabaseName = MyDatabaseName;
            LoadSQLDatabase.Properties.Settings.Default.UserID = MyUserID;
            LoadSQLDatabase.Properties.Settings.Default.Password = MyPassword;
            LoadSQLDatabase.Properties.Settings.Default.IntegratedSecurity = IntegratedSecurityFlag;
            LoadSQLDatabase.Properties.Settings.Default.EncryptData = EncryptDataFlag;
            LoadSQLDatabase.Properties.Settings.Default.TrustServerCertificate = TrustServerCertificateFlag;
            LoadSQLDatabase.Properties.Settings.Default.UseWindowsAuthentication = UseWindowsAuthenticationFlag;
            LoadSQLDatabase.Properties.Settings.Default.DatabaseConnectionString = ConnectionString;

            // Save connections variables permanently in the user.config file
            Properties.Settings.Default.Save();

            // Reset the connectionChangedFlag
            ConnectionChangedFlag = false;
        }

        /// <summary>
        /// Builds the connection string.
        /// Optional parameters: ConnectTimeout
        /// </summary>
        /// <returns>A database connection string</returns>
        private string BuildConnectionString()
        {
            // Build connection string using windows authentication
            if (UseWindowsAuthenticationFlag)
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
                {
                    DataSource = MyDataSourceName,
                    InitialCatalog = MyDatabaseName,
                    IntegratedSecurity = IntegratedSecurityFlag,
                    Encrypt = EncryptDataFlag,
                    TrustServerCertificate = TrustServerCertificateFlag,
                };
                string ConnectionString = builder.ConnectionString;
                ConnectionStringTxtBx.Text = ConnectionString;
                return ConnectionString;
            }
            else    // Build connection string using SQL Authentication
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
                {
                    DataSource = MyDataSourceName,
                    InitialCatalog = MyDatabaseName,
                    UserID = MyUserID,
                    Password = MyPassword,
                    Encrypt = EncryptDataFlag,
                    TrustServerCertificate = TrustServerCertificateFlag,
                };
                string ConnectionString = builder.ConnectionString;
                ConnectionStringTxtBx.Text = ConnectionString;
                return ConnectionString;
            }
        }

        #region Controls

        /// <summary>
        /// Handles the TextChanged event of the DataSourceTxtBx control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void DataSourceTxtBx_TextChanged(object sender, EventArgs e)
        {
            ConnectionChangedFlag = true;
            MyDataSourceName = DataSourceTxtBx.Text;
        }

        /// <summary>
        /// Handles the TextChanged event of the DatabaseNameTxtBx control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void DatabaseNameTxtBx_TextChanged(object sender, EventArgs e)
        {
            ConnectionChangedFlag = true;
            MyDatabaseName = DatabaseNameTxtBx.Text;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the IntegratedSecurityCBx control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void IntegratedSecurityCBx_CheckedChanged(object sender, EventArgs e)
        {
            ConnectionChangedFlag = true;
            if (IntegratedSecurityCBx.Checked)
            {
                IntegratedSecurityFlag = true;
            }
            else
            {
                IntegratedSecurityFlag = false;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the TrustServerCerticateCBx control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void TrustServerCerticateCBx_CheckedChanged(object sender, EventArgs e)
        {
            ConnectionChangedFlag = true;
            if (TrustServerCerticateCBx.Checked)
            {
                TrustServerCertificateFlag = true;
            }
            else
            {
                TrustServerCertificateFlag = false;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the EncryptDataCBx control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void EncryptDataCBx_CheckedChanged(object sender, EventArgs e)
        {
            ConnectionChangedFlag = true;
            if (EncryptDataCBx.Checked)
            {
                EncryptDataFlag = true;
            }
            else
            {
                EncryptDataFlag = false;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the UseWindowsAuthenticationCBx control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void UseWindowsAuthenticationCBx_CheckedChanged(object sender, EventArgs e)
        {
            ConnectionChangedFlag = true;
            if (UseWindowsAuthenticationCBx.Checked)
            {
                UseWindowsAuthenticationFlag = true;
                UserIDTxtBx.Clear();
                UserIDTxtBx.Enabled = false;
                PasswordTxtBx.Clear();
                PasswordTxtBx.Enabled = false;
            }
            else
            {
                UseWindowsAuthenticationFlag = false;
                UserIDTxtBx.Enabled = true;
                PasswordTxtBx.Enabled = true;
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the UserIDTxtBx control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void UserIDTxtBx_TextChanged(object sender, EventArgs e)
        {
            ConnectionChangedFlag = true;
            MyUserID = UserIDTxtBx.Text;
        }

        /// <summary>
        /// Handles the TextChanged event of the PasswordTxtBx control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void PasswordTxtBx_TextChanged(object sender, EventArgs e)
        {
            ConnectionChangedFlag = true;
            MyPassword = PasswordTxtBx.Text;
        }

        /// <summary>
        /// Handles the Click event of the GenerateConnectionStringBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void GenerateConnectionStringBtn_Click(object sender, EventArgs e)
        {
            // generate the connection string
            BuildConnectionString();
        }

        /// <summary>
        /// TestConnectionBtn_Click handles the Click event of the TestConnectionBtn control. A test
        /// to connect to the database is performed.  The user is informed of the success or failure.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void TestConnectionBtn_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    MessageBox.Show("Connection Success!", "Test Connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Test Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the SaveConnectionStringBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SaveConnectionStringBtn_Click(object sender, EventArgs e)
        {
            // if the connection variables have been changed, then save the connection
            if (ConnectionChangedFlag)
            {
                SaveConnectionData();
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the ConnectionStringTxtBx control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ConnectionStringTxtBx_TextChanged(object sender, EventArgs e)
        {
            ConnectionChangedFlag = true;
        }

        /// <summary>
        /// Handles the Click event of the OKBtn control to close the form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OKBtn_Click(object sender, EventArgs e)
        {
            if (ConnectionChangedFlag)
            {
                DialogResult result = MessageBox.Show(
                    "Connection parameters have not been saved.  Do you want to continue?",
                    "Edit Connection Form",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                    );
                if (result == DialogResult.No)
                {
                    return;
                }
            }

            // Reset ConnectionChangedFlag and then close
            ConnectionChangedFlag = false;
            this.Close();
        }

        #endregion Controls
    }
}