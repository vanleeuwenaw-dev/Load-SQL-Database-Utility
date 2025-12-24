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
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CsvHelper;
using CsvHelper.Configuration;
using LoadSQLDatabase.Properties;

namespace LoadSQLDatabase
{
    /// <summary>
    /// MainForm of the LoadSQLDatabase program
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class MainForm : Form
    {
        // Connection String from Settings and Edit Connection Form
        private string ConnectionString;

        // Name of database from Settings and Edit Connection Form
        private string DatabaseName;

        // List of database tables for the database identified in the connection string.
        private List<DbTableDetails> MyTables = new List<DbTableDetails>();

        // selected schema and table names used to retrieve table column information.
        private string SelectedSchemaName;

        // selected table name used to retrieve table column information.
        private string SelectedTableName;

        // selected full table name including schema name.
        private string SelectedFullTableName;

        // List of Columns and column properties for the selected table.
        private List<DbColumnDetails> MyDbColumns = new List<DbColumnDetails>();

        // column count of the selected table
        private int DbColumnCnt;

        // List of columns and the IsComputed property
        private List<DbComputedColumnDetails> MyComputedColumns = new List<DbComputedColumnDetails>();

        // A dictionary object used to map datatable columns to database columns
        // omitting computed columns
        private Dictionary<string, string> ColumnMappings = new Dictionary<string, string>();

        // File Name of the selected CSV file.
        private string CSVFileName;

        // List of Columns and column ordinal position for the Csv File
        private List<CsvColumnDetails> MyCsvColumns = new List<CsvColumnDetails>();

        // Column count for the Csv file and Csv file delimiter
        private int CsvColumnCnt;

        private char CsvDelimiter = new char();

        // Type mapping dictionary from SQL data types to .NET data types
        private static Dictionary<string, Type> SqlToDotNetTypeMap = new Dictionary<string, Type>
        {
            { "int", typeof(int) },
            { "bigint", typeof(long) },
            { "smallint", typeof(short) },
            { "tinyint", typeof(Byte) },
            { "boolean", typeof(Boolean) },
            { "bit", typeof(Boolean) },
            { "decimal", typeof(Decimal) },
            { "money", typeof(Decimal) },
            { "smallmoney", typeof(Decimal) },
            { "numeric", typeof(Decimal) },
            { "real", typeof(Single) },
            { "float", typeof(Double) },
            { "double", typeof(Double) },
            { "nvarchar", typeof(string) },
            { "varchar", typeof(string) },
            { "char", typeof(string) },
            { "nchar", typeof(string) },
            { "text", typeof(string) },
            { "ntext", typeof(string) },
            { "string", typeof(string) },
            { "date", typeof(DateTime) },
            { "datetime", typeof(DateTime) },
            { "smalldatetime", typeof(DateTime) },
            { "datetimeoffset", typeof(DateTimeOffset) },
            { "time", typeof(TimeSpan) },
            { "timestamp", typeof(DateTime) },
            { "uniqueidentifier", typeof(Guid) },
         };

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        #region Form Load Events

        /// <summary>
        /// MainForm_Load handles the MainForm Load event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Check if this is the first time the application has been run
            // If so, then default values are saved to the user.config file
            // and action proceeds to EditConnectionForm to input the actual
            // connection string info.
            if (Settings.Default.IsFirstRun)
            {
                // save all connection variables to the settings file
                Properties.Settings.Default.DataSourceName = string.Empty;
                Properties.Settings.Default.DatabaseName = string.Empty;
                Properties.Settings.Default.UserID = string.Empty;
                Properties.Settings.Default.Password = string.Empty;
                Properties.Settings.Default.IntegratedSecurity = false;
                Properties.Settings.Default.EncryptData = false;
                Properties.Settings.Default.TrustServerCertificate = false;
                Properties.Settings.Default.UseWindowsAuthentication = false;
                Properties.Settings.Default.DatabaseConnectionString = string.Empty;
                Properties.Settings.Default.IsFirstRun = false;
                Properties.Settings.Default.Save();

                // Go to edit connection form to select data source and database name
                GetDatabaseConnectionString();
            }

            // Display application version on main form's status strip
            VersionLbl.Text = $"LoadSQLDatabase - Version {Application.ProductVersion}";

            // Display empty CSV DataGridView on form load
            DisplayEmptyCSVDataGridView();

            // Display empty Database Column DataGridView on form load
            DisplayEmptyDbColumnGridView();

            // clear checkbox to clear and reseed the Db table on the mainform
            ClearTableChkBx.Checked = false;

            // Load connection string from Settings and display in mainform textbox
            LoadDatabaseConnectionString();

            // Check to see if the Connection String is null or empty. If so, ask the user if he wants to
            // set the connection string, if so we load the Edit Connection Form. Otherwise, return.
            if (string.IsNullOrEmpty(ConnectionString))
            {
                string msg = "Connection String is empty, do you want to set it?";
                DialogResult result = MessageBox.Show(msg, "Connection String: ", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    // open edit connection form and build connection string
                    GetDatabaseConnectionString();

                    // load the newly created connection string from settings
                    LoadDatabaseConnectionString();
                }
                else
                {
                    // user does not want to set connection string, so we return w/o doing anything
                    return;
                }
            }
        }

        #endregion Form Load Events

        #region Controls - no code

        private void CSVFilenameTxtBx_TextChanged(object sender, EventArgs e)
        {
        }

        private void ConnectionStringTxtBx_TextChanged(object sender, EventArgs e)
        {
        }

        private void CSVColumnListView_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void DBSchemaDataListView_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void CSVFileColumnsDetectedTxtBx_TextChanged(object sender, EventArgs e)
        {
        }

        private void CSVFileDataRowsDetectedTxtBx_TextChanged(object sender, EventArgs e)
        {
        }

        private void DBColumnsDetectedTxtBx_TextChanged(object sender, EventArgs e)
        {
        }

        private void RowsTransferredTxtBx_TextChanged(object sender, EventArgs e)
        {
        }

        private void CSVDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void DatabaseTableColumnGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void ClearTableChkBx_CheckedChanged(object sender, EventArgs e)
        {
        }

        #endregion Controls - no code

        #region Controls

        /// <summary>
        /// GetCSVFileNameBtn_Click handles the Click event for the Get CSV File Name button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void GetCSVFileNameBtn_Click(object sender, EventArgs e)
        {
            // Get the CSV file name and path, return if null or empty
            CSVFileName = GetCSVFileName();

            // if the returned filename is null or empty return
            if (string.IsNullOrEmpty(CSVFileName) == true)
            {
                return;
            }

            // display CSV filepath in text box.
            CSVFilenameTxtBx.Text = CSVFileName;

            // Get CSV column names from the CSV header
            MyCsvColumns = GetCsvColumnDetails(CSVFileName);

            // Get the CSV file column count and row count and display in textboxes
            CsvColumnCnt = MyCsvColumns.Count;
            CSVFileColumnsDetectedTxtBx.Text = CsvColumnCnt.ToString();
            CSVFileDataRowsDetectedTxtBx.Text = GetCsvRecordCount(CSVFileName).ToString();

            // Display column names and ordinal position in the Csv File DataGridView Control
            DisplayCSVFileColumns();
        }

        /// <summary>
        /// GetDBConnectionStringBtn_Click handles the Click event of the GetDBConnectionString button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void GetDBConnectionStringBtn_Click(object sender, EventArgs e)
        {
            // Open edit connection form and build connection string
            GetDatabaseConnectionString();

            // Get connection string from settings and display on form
            LoadDatabaseConnectionString();
        }

        /// <summary>
        /// GetDatabaseConnectionString loads the EditConnectionForm to get connection information, generate a
        /// connection string and saves the connection information to the settings file or user.config file.
        /// </summary>
        private void GetDatabaseConnectionString()
        {
            // call the edit connection form - connection data is saved to user.config file
            EditConnectionForm form = new EditConnectionForm();
            form.ShowDialog();
        }

        /// <summary>
        /// LoadDatabaseConnectionString loads both the database name and connection string from the settings file.
        /// </summary>
        private void LoadDatabaseConnectionString()
        {
            // Get the database connection string and display on main form
            ConnectionString = LoadSQLDatabase.Properties.Settings.Default.DatabaseConnectionString;
            ConnectionStringTxtBx.Text = ConnectionString;

            // check to be sure that we have a connection string - return if empty
            if (string.IsNullOrEmpty(ConnectionString))
            {
                string msg = "Connection String is empty!";
                MessageBox.Show(msg, "Connection String: ", MessageBoxButtons.OK);
                return;
            }

            // Get database name and display on main form in textbox
            DatabaseName = LoadSQLDatabase.Properties.Settings.Default.DatabaseName;
            DatabaseNameTxtBx.Text = DatabaseName;

            // TestDatabaseConnection verifies that the connection string can connect to the database,
            // otherwise, a message box is displayed notifying the user that the connection fails and we
            // exit the procedure.
            if (!TestDatabaseConnection(ConnectionString))
            {
                return;
            }

            // clear the comboBox of previously displayed tables and views
            ClearDBTableNameComboBox();

            // load the comboBox with database table names
            LoadDbTableNameComboBox();
        }

        /// <summary>
        /// The TestDatabaseConnection procedure tests the connection to the database to verify the connection string.
        /// If the connection fails, then a message box is displayed informing the user.
        /// </summary>
        private bool TestDatabaseConnection(string connectionString)
        {
            bool testPassed = false;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    testPassed = true;
                }
                catch (Exception ex)
                {
                    string msg = "A connection with the database could not be established!" + ex.Message;
                    MessageBox.Show(msg, "Database Connection:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    testPassed = false;
                }
            }
            return testPassed;
        }

        /// <summary>
        /// LoadDbTableNameComboBox loads the database table name ComboBox displaying both tables and views in the
        /// current database.
        /// </summary>
        private void LoadDbTableNameComboBox()
        {
            // Clear the myTables List Object
            MyTables.Clear();

            // Get the database table information into myTables List Object.
            MyTables = GetDbTableInfo(ConnectionString);

            // Store table names in the combobox for user selection
            // Note that the tableName is displayed but the catalogId is returned when selected.
            DbTableNameComboBox.DataSource = MyTables;
            DbTableNameComboBox.DisplayMember = "tableName";
            DbTableNameComboBox.ValueMember = "catalogId";
        }

        /// <summary>
        /// ClearDBTableNameComboBox clears the database table name ComboBox when database is changed.
        /// </summary>
        private void ClearDBTableNameComboBox()
        {
            // Set datasource to null, clear list, and clear currently displayed name
            DbTableNameComboBox.DataSource = null;
            DbTableNameComboBox.Items.Clear();
            DbTableNameComboBox.Text = string.Empty;
        }

        /// <summary>
        /// DbTableNameComboBox_SelectionChangeCommitted handles the SelectionChangeCommitted event of
        /// the DbTableNameComboBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void DbTableNameComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // Get the index of the selected table name from the myTables List
            int selectedValue = (int)DbTableNameComboBox.SelectedValue;
            int index = selectedValue;

            // use the index to get the selected table name and selected schema name
            SelectedTableName = MyTables[index].TableName.Trim();
            SelectedSchemaName = MyTables[index].SchemaName.Trim();
            // concatenate Selected schema and table name to produce the SelectedFullTableName
            SelectedFullTableName = SelectedSchemaName + "." + SelectedTableName;

            // Using the selected table name, get the column data for and display in dategridview control
            LoadDBColumnGridView(SelectedTableName);

            // clear the RowsTransferredTxtBx textbox
            //RowsTransferredTxtBx.Text = string.Empty;
            RowsTransferredTxtBx.Clear();
        }

        /// <summary>
        /// GetDbColumnDisplayList gets a copy of the list of table columns and includes blank lines so
        /// no blank space is displayed in the control.
        /// </summary>
        /// <param name="dbColumnDetails">My database columns.</param>
        /// <returns>A list of myDbColumns plus empty lines</returns>
        private List<DbColumnDetails> GetDbColumnDisplayList(List<DbColumnDetails> dbColumnDetails)
        {
            // create a new list object to hold the augmented columns
            List<DbColumnDetails> ColumnDisplayList = new List<DbColumnDetails>(dbColumnDetails);

            // check to see if we have less than 14 columns, if so add blank lines
            if (ColumnDisplayList.Count < 14)
            {
                int currentCount = ColumnDisplayList.Count;
                for (int i = currentCount; i < 14; i++)
                {
                    ColumnDisplayList.Add(new DbColumnDetails
                    {
                        ColumnId = i + 1,
                        DatabaseName = string.Empty,
                        SchemaName = string.Empty,
                        TableName = string.Empty,
                        ColumnName = string.Empty,
                        ColumnOrdinalPosition = string.Empty,
                        ColumnDataType = string.Empty,
                        ColumnMaxLength = string.Empty,
                        ColumnIsNullable = string.Empty,
                        ColumnIsComputed = false
                    });
                }
                return ColumnDisplayList;
            }
            else
            {
                // we have 14 or more columns so just return the original list
                return ColumnDisplayList;
            }
        }

        /// <summary>
        /// DisplayEmptyDbColumnGridView() displays an empty database column grid view control.
        /// This routine is purely cosmetic to show an empty grid view on form load and before
        /// a table from the database is selected.
        /// </summary>
        private void DisplayEmptyDbColumnGridView()
        {
            // create an empty list of DbColumnDetails objects
            List<DbColumnDetails> EmptyDbList = new List<DbColumnDetails>();

            for (int i = 0; i < 14; i++)
            {
                EmptyDbList.Add(new DbColumnDetails
                {
                    ColumnId = i,
                    DatabaseName = string.Empty,
                    SchemaName = string.Empty,
                    TableName = string.Empty,
                    ColumnName = string.Empty,
                    ColumnOrdinalPosition = string.Empty,
                    ColumnDataType = string.Empty,
                    ColumnMaxLength = string.Empty,
                    ColumnIsNullable = string.Empty,
                    ColumnIsComputed = false
                });
            }

            // set the BindingSource to the EmptyDbList object
            DbColumnGridViewBindingSource.DataSource = EmptyDbList;
            DbColumnGridViewBindingSource.EndEdit();

            // set the DataSource of the DbTableColumnDataGridView control
            DbTableColumnDataGridView.DataSource = DbColumnGridViewBindingSource;
            DbTableColumnDataGridView.EndEdit();
        }

        /// <summary>
        /// LoadDBColumnGridView loads the database column grid view control. It clears the control and
        /// binding source, retrieves the column data for the selected table, gets the computed columns,
        /// and displays the data in the datagridview control.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        private void LoadDBColumnGridView(string tableName)
        {
            // clear the BindingSource for the DbColumnGridView control
            DbColumnGridViewBindingSource.DataSource = null;
            DbColumnGridViewBindingSource.EndEdit();

            // clear DatabaseTableColumnGridView control
            DbTableColumnDataGridView.DataSource = null;
            DbTableColumnDataGridView.Rows.Clear();
            DbTableColumnDataGridView.Refresh();
            DbTableColumnDataGridView.EndEdit();

            // clear the myDbColumns List Object
            MyDbColumns.Clear();

            // Get the column schema data for the selected table and display
            MyDbColumns = GetDatabaseColumns(tableName, SelectedSchemaName, ConnectionString);

            // Get the computed columns
            MyComputedColumns = GetComputedColumns(tableName, ConnectionString);

            // Add Is_Computed column flag to myDbColumns
            foreach (DbColumnDetails details in MyDbColumns)
            {
                string searchName = details.ColumnName;
                DbComputedColumnDetails column = MyComputedColumns.Find(p => p.ColumnName == searchName);
                if (column != null)
                {
                    details.ColumnIsComputed = column.Is_Computed;
                }
                // if you get here because column is null
            }

            // get the MyDBColumns display list to include blank lines for display purposes
            List<DbColumnDetails> dbColumns = GetDbColumnDisplayList(MyDbColumns);

            // Display table column data in datagridview
            DbColumnGridViewBindingSource.DataSource = dbColumns;
            DbColumnGridViewBindingSource.EndEdit();
            DbTableColumnDataGridView.DataSource = DbColumnGridViewBindingSource;
            DbTableColumnDataGridView.EndEdit();

            // Get the number of data columns and display in mainform textbox
            DBColumnsDetectedTxtBx.Text = MyDbColumns.Count.ToString();

            // Get the table size in rows for display in mainform textbox
            DbRowsDetectedTxtBx.Text = GetTableSize(SelectedTableName, SelectedSchemaName, ConnectionString).ToString();
        }

        /// <summary>
        /// TransferDataBtn_Click handles the Click event of the Transfer Data Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void TransferDataBtn_Click(object sender, EventArgs e)
        {
            int myRowCount;

            // Verify that the Csv File Exists
            if (string.IsNullOrEmpty(CSVFileName) || !File.Exists(CSVFileName))
            {
                // if CSVFileName == null then it will not show in the following msg
                string msg = $"The CSV File {CSVFileName} does not exist or has not been selected!";
                MessageBox.Show(msg, "File Error:", MessageBoxButtons.OK, MessageBoxIcon.None);
                return;
            }

            // Verify that the database table has been selected and Columns are defined
            if (MyDbColumns.Count == 0)
            {
                string msg = "A database table has not been selected!";
                MessageBox.Show(msg, "File Error:", MessageBoxButtons.OK, MessageBoxIcon.None);
                return;
            }

            // Confirm that the target database table does not include unsupported data types
            if (!VerifySQLDataTypesAreSupported(MyDbColumns))
            {
                // unsupported data types were detected, so we return
                return;
            }

            // Check if Clear Table and Reseed Table checkbox is checked
            if (ClearTableChkBx.Checked)
            {
                // Verify the user intends to delete table data and reseed the index
                if (VerifyUserIntentToClearDbTable(SelectedTableName))
                {
                    // Clear and reseed the table
                    ClearDatabaseTable(SelectedFullTableName, ConnectionString);
                    //ClearDatabaseTable(SelectedTableName, ConnectionString);

                    // reset the ClearTableChkBx
                    ClearTableChkBx.Checked = false;

                    // report the number of rows in the rows detected textbox
                    DbRowsDetectedTxtBx.Text = GetTableSize(SelectedTableName, SelectedSchemaName, ConnectionString).ToString();
                }
                else
                {
                    // the user does not intend to clear the database table.
                    ClearTableChkBx.Checked = false;
                    // return;
                }
            }



            // Get an empty datatable with the correct datatyped columns based on target table.
            DataTable dataTable = GetDataTable(MyDbColumns);

            // Get the column mappings omitting computed columns
            ColumnMappings = GetColumnMappings(MyDbColumns);

            // Set the CsvConfiguration variables
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8,
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
                MissingFieldFound = null,
                Delimiter = CsvDelimiter.ToString()
            };

            // use stream reader and CsvHelper to get the Csv data
            using (var reader = new StreamReader(CSVFileName.ToString()))
            using (var csv = new CsvReader(reader, config))
            {
                // Get header record out of the way
                csv.Read();
                csv.ReadHeader();
                string[] header = csv.Context.Reader.HeaderRecord;

                // get all the data records in the CSV file.
                var records = csv.GetRecords<dynamic>();

                // iterate through each record
                foreach (var record in records)
                {
                    // Get record or csvRow of data
                    string[] csvRow = csv.Context.Parser.Record;
                    int csvColumnCnt = csvRow.Length;

                    // Verify that csvColumnCnt == DbColumnCnt
                    if (csvColumnCnt != DbColumnCnt)
                    {
                        string message = $"CSV File csvColumnCnt = {csvColumnCnt} not equal to DbColumnCnt = {DbColumnCnt}.";
                        MessageBox.Show(message, "LoadSQLDatabase Column Count Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Get a new empty data row based on the dataTable
                    DataRow dtRow = dataTable.NewRow();
                    try
                    {
                        // Iterate over the columns in the datarow
                        for (int i = 0; i < csvColumnCnt; i++)
                        {
                            // Get the column map and set map to the correct column
                            DbColumnDetails map = new DbColumnDetails();
                            map = MyDbColumns[i];

                            // get data from CSV csvRow all columns returned from CSV helper are strings
                            var item = csvRow[i];

                            // Convert string data type to target data type in expected in datatable column
                            // Note the SQL Data Type is mapped to a .NET data type used in datatable
                            switch (map.ColumnDataType.ToLower())
                            {
                                case "int":
                                    dtRow[i] = ConvertToInt32(item, i + 1); break;
                                case "bigint":
                                    dtRow[i] = ConvertToInt64(item, i + 1); break;
                                case "smallint":
                                    dtRow[i] = ConvertToInt16(item, i + 1); break;
                                case "tinyint":
                                    dtRow[i] = ConvertToByte(item, i + 1); break;
                                case "text":
                                case "varchar":
                                case "ntext":
                                case "nvarchar":
                                case "nchar":
                                case "char":
                                    dtRow[i] = item.ToString(); break;
                                case "boolean":
                                case "bit":
                                    dtRow[i] = ConvertStringToBoolean(item, i + 1); break;  //Convert.ToBoolean(Convert.ToInt16(item)); break;
                                case "datetime":
                                case "date":
                                case "smalldatetime":
                                case "timestamp":
                                    dtRow[i] = ConvertStringToDateTime(item, i + 1); break;
                                case "time":
                                    dtRow[i] = ConvertStringToTimeSpan(item, i + 1); break;
                                case "decimal":
                                case "numeric":
                                case "smallmoney":
                                case "money":
                                    dtRow[i] = ConvertToDecimal(item, i + 1); break;
                                case "real":
                                    dtRow[i] = ConvertToSingle(item, i + 1); break;
                                case "float":
                                case "double":
                                    dtRow[i] = ConvertToDouble(item, i + 1); break;
                                case "uniqueidentifier":
                                    dtRow[i] = ConvertStringToGuid(item, i + 1); break;
                                case "datetimeoffset":
                                    dtRow[i] = ConvertStringToDateTimeOffset(item, i + 1); break;
                                default:
                                    dtRow[i] = item.ToString();
                                    break;
                            }
                        }
                        // add data Row to dataTable
                        dataTable.Rows.Add(dtRow);
                    }
                    catch (Exception)
                    {
                        // The purpose of this catch block is catch an unexpected error during
                        // data conversion and return to the main form.  The error notification 
                        // is handled in the conversion functions.  To prevent multiple error messages,
                        // only the first error is caught and the procedure exits.
                        return;
                    }
                }
                // get the dataTable Row count and display in textbox as rows transferred
                myRowCount = dataTable.Rows.Count;

            }

            // Copy the dataTable to the Database using SQLBulkCopy
            // The column mappings make sure that Computed Columns are not copied.
            try
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConnectionString, SqlBulkCopyOptions.KeepIdentity))
                {
                    bulkCopy.DestinationTableName = SelectedFullTableName;

                    foreach (var mapping in ColumnMappings)
                    {
                        bulkCopy.ColumnMappings.Add(mapping.Key, mapping.Value);
                    }
                    bulkCopy.WriteToServer(dataTable);
                }
                RowsTransferredTxtBx.Text = myRowCount.ToString();
            }
            catch (Exception ex)
            {
                string msg = $"Error: An unexpected error occurred: {ex.Message}";
                MessageBox.Show(msg, "LoadSQLDatabase SQLBulkCopy Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // update the row count for the table
            DbRowsDetectedTxtBx.Text = GetTableSize(SelectedTableName, SelectedSchemaName, ConnectionString).ToString();

            // Notify user that the transfer has completed
            string msgNotification = $"Data transfer has completed. {myRowCount} rows transfered.";
            DialogResult result = MessageBox.Show(msgNotification, "Transfer Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (result == DialogResult.OK)
            {
                return;
            }
        }

        /// <summary>
        /// ExitBtn_Click handles the Click event of the ExitBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion Controls

        #region Database Procedures

        /// <summary>
        /// GetDbTableInfo gets the database table information. Data about tables is
        /// retrieved from the database, and the tables are sorted by name and table type.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>A list of DbTableDetails</returns>
        public List<DbTableDetails> GetDbTableInfo(string connectionString)
        {
            // declare an index i and set to 0
            int i = 0;

            // create local object that will be returned as a result
            List<DbTableDetails> tableDetails = new List<DbTableDetails>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // open the connection
                connection.Open();

                // Retrieve schema information for the database tables
                DataTable tables = connection.GetSchema("Tables");

                // sort tableDetails to sort on fields Table_Name, and Table_Type
                DataView dv = tables.DefaultView;
                dv.Sort = "TABLE_NAME ASC, TABLE_TYPE ASC";

                DataTable sortedTable = dv.ToTable();

                // Iterate through the rows returned and add to globally defined MyTables
                // List<DbTableDetails> object
                foreach (DataRow row in sortedTable.Rows)
                {
                    DbTableDetails dbtables = new DbTableDetails
                    {
                        CatalogId = i++,
                        DatabaseName = row["TABLE_CATALOG"].ToString(),
                        SchemaName = row["TABLE_SCHEMA"].ToString(),
                        TableName = row["TABLE_NAME"].ToString(),
                        TableType = row["TABLE_TYPE"].ToString()
                    };
                    tableDetails.Add(dbtables);
                }
                return tableDetails;
            }
        }

        /// <summary>
        /// GetDatabaseColumns gets the database column and properties from the selected table name.
        /// </summary>
        /// <param name="table">Database table name</param>
        /// <param name="schema">schema or ownership of the table or view</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>A List of column columnDetails</returns>
        public List<DbColumnDetails> GetDatabaseColumns(string table, string schema, string connectionString)
        {
            // set index i to 0
            int i = 0;

            // declare local list of DbColumnDetails
            List<DbColumnDetails> columnDetails = new List<DbColumnDetails>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open the connection and get the column schema for the database table in question
                connection.Open();
                DataTable dt = connection.GetSchema("Columns", new string[] { null, schema, table, null });

                // Need to order the columns in the proper sequence based upon ordinal position
                DataView dataView = dt.DefaultView;
                dataView.Sort = "ORDINAL_POSITION ASC";
                DataTable sortedTable = dataView.ToTable();

                // Get the sorted column schema table and put into the columnDetails list object
                foreach (DataRow row in sortedTable.Rows)
                {
                    DbColumnDetails dbcolumns = new DbColumnDetails
                    {
                        ColumnId = ++i,
                        DatabaseName = row["TABLE_CATALOG"].ToString(),
                        SchemaName = row["TABLE_SCHEMA"].ToString(),
                        TableName = row["TABLE_NAME"].ToString(),
                        ColumnName = row["COLUMN_NAME"].ToString(),
                        ColumnOrdinalPosition = row["ORDINAL_POSITION"].ToString(),
                        ColumnDataType = row["DATA_TYPE"].ToString(),
                        ColumnMaxLength = row["CHARACTER_MAXIMUM_LENGTH"].ToString(),
                        ColumnIsNullable = row["IS_NULLABLE"].ToString()
                    };
                    columnDetails.Add(dbcolumns);
                }
                // Get the column count and return the columnDetails object
                DbColumnCnt = columnDetails.Count;
                return columnDetails;
            }
        }

        /// <summary>
        /// GetComputedColumns gets the computed columns from the database (sys.columns and
        /// sys.tables).  The column name and the Is_Computed flag is returned and stored in a
        /// List object.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="conconnectionString">The conconnection string.</param>
        /// <returns>returns a list of column names and state of Is_Computed flag</returns>
        public static List<DbComputedColumnDetails> GetComputedColumns(string table, string conconnectionString)
        {
            List<DbComputedColumnDetails> details = new List<DbComputedColumnDetails>();

            // query to get column name and Is_Computed value for all columns in table
            string query = @"SELECT c.name, c.IS_COMPUTED
                FROM sys.columns c
                INNER JOIN sys.tables t ON c.object_id = t.object_id
                WHERE T.NAME = @TableName";

            // Create datatable to store column name and Is_Computed flag
            DataTable ComputedColumns = new DataTable();

            // Get computed column data
            using (SqlConnection connection = new SqlConnection(conconnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TableName", table);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                connection.Open();
                adapter.Fill(ComputedColumns);
            }

            // Copy datatable to List<DbComputedcolumnDetails>
            foreach (DataRow row in ComputedColumns.Rows)
            {
                DbComputedColumnDetails columnDetails = new DbComputedColumnDetails()
                {
                    ColumnName = row["name"].ToString(),
                    Is_Computed = (bool)row["Is_Computed"]
                };
                details.Add(columnDetails);
            }
            return details;
        }

        /// <summary>
        /// GetColumnMappings gets the column mappings omitting computed columns.
        /// NOTE: We could also exlude columns with column length of -1 or specific data types in future
        /// updates.
        /// </summary>
        /// <param name="columnDetails">The column details.</param>
        /// <returns>Returns a dictionary object containing the column mappings omitting computed columns</returns>
        private static Dictionary<string, string> GetColumnMappings(List<DbColumnDetails> columnDetails)
        {
            // declare column mappings object.
            var columnMappings = new Dictionary<string, string>();

            foreach (DbColumnDetails details in columnDetails)
            {
                // add other conditions in the statement below if necessary
                // only map columns that do not have the computed flag set
                if (details.ColumnIsComputed == false)
                {
                    columnMappings.Add(details.ColumnName, details.ColumnName);
                }
            }
            return columnMappings;
        }

        #endregion Database Procedures

        #region Data Conversion Routines

        /// <summary>
        /// ConvertStringToBoolean converts the string value to a boolean. Converts "true" and "yes" to a boolean
        /// true value and "false" and "no" to a boolean false value. digits '1'  and '0' are directly converted
        /// to an integer and then to a boolean.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="ColumnNo">column ordinal number</param>
        /// <returns>a boolean datatype</returns>
        private bool ConvertStringToBoolean(string value, int ColumnNo)
        {
            // trim the string value and make sure it is lower case.
            string itemValue = value.Trim().ToLower();

            // check for "true" and "yes" values and return true.
            if (string.Equals(itemValue, "true") || string.Equals(itemValue, "yes")) { return true; }

            // check for "false" and "no" values and return false.
            if (string.Equals(itemValue, "false") || string.Equals(itemValue, "no")) { return false; }

            try
            {
                // assume string is a digit and can be converted to int and then to boolean
                return Convert.ToBoolean(Convert.ToInt16(itemValue));
            }
            catch (Exception)
            {
                string errorMessage = $"Column {ColumnNo}: Failed to convert \"{value}\" to Boolean Data Type!";
                MessageBox.Show(errorMessage, "Data Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.None);
                throw;
            }
        }

        /// <summary>
        /// ConvertStringToGuid converts the string to unique identifier Guid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="ColumnNo">column index</param>
        /// <returns>GUID datatype</returns>
        private Guid ConvertStringToGuid(string value, int ColumnNo)
        {
            try
            {
                Guid.TryParse(value, out Guid result);
                return result;
            }
            catch (Exception)
            {
                string errorMessage = $"Column {ColumnNo}: Failed to convert \"{value}\" to Guid Data Type!";
                MessageBox.Show(errorMessage, "Data Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.None);
                throw;
            }
        }

        /// <summary>
        /// ConvertStringToDateTime converts the string to date time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="ColumnNo">column ordinal number</param>
        /// <returns>DateTime datatype</returns>
        private DateTime ConvertStringToDateTime(string value, int ColumnNo)
        {
            try
            {
                DateTime.TryParse(value, out DateTime result);
                return result;
            }
            catch (Exception)
            {
                string errorMessage = $"Column {ColumnNo}: Failed to convert \"{value}\" to DateTime Data Type!";
                MessageBox.Show(errorMessage, "Data Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.None);
                throw;
            }
        }

        /// <summary>
        /// ConvertToInt16 converts string value to int16.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="ColumnNo">column ordinal number</param>
        /// <returns>Int16 datatype</returns>
        private static Int16 ConvertToInt16(string value, int ColumnNo)
        {
            try
            {
                Int16 result;
                result = Convert.ToInt16(value);
                return result;
            }
            catch (Exception)
            {
                string errorMessage = $"Column {ColumnNo}: Failed to convert \"{value}\" to Int16 Data Type!";
                MessageBox.Show(errorMessage, "Data Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.None);
                throw;
            }
        }

        /// <summary>
        /// ConvertToInt32 Converts a string value to int32.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="ColumnNo">column ordinal number</param>
        /// <returns>Int32 datatype</returns>
        private static Int32 ConvertToInt32(string value, int ColumnNo)
        {
            try
            {
                Int32 result;
                result = Convert.ToInt32(value);
                return result;
            }
            catch (Exception)
            {
                string errorMessage = $"Column {ColumnNo}: Failed to convert \"{value}\" to Int32 Data Type!";
                MessageBox.Show(errorMessage, "Data Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.None);
                throw;
            }
        }

        /// <summary>
        /// ConvertToInt64 converts a string value to int64.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="ColumnNo">column ordinal number</param>
        /// <returns>Int64 datatype</returns>
        private static Int64 ConvertToInt64(string value, int ColumnNo)
        {
            try
            {
                Int64 result;
                result = Convert.ToInt64(value);
                return result;
            }
            catch (Exception)
            {
                string errorMessage = $"Column {ColumnNo}: Failed to convert \"{value}\" to Int64 Data Type!";
                MessageBox.Show(errorMessage, "Data Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.None);
                throw;
            }
        }

        /// <summary>
        /// ConvertToDecimal converts string value to decimal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="ColumnNo">column ordinal number</param>
        /// <returns>Decimal Datatype value</returns>
        private static Decimal ConvertToDecimal(string value, int ColumnNo)
        {
            try
            {
                Decimal result;
                result = Convert.ToDecimal(value);
                return result;
            }
            catch (Exception)
            {
                string errorMessage = $"Column {ColumnNo}: Failed to convert \"{value}\" to Decimal Data Type!";
                MessageBox.Show(errorMessage, "Data Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.None);
                throw;
            }
        }

        /// <summary>
        /// ConvertToSingle converts a string value to single floating point.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="ColumnNo">column ordinal number</param>
        /// <returns>Single datatype</returns>
        private static Single ConvertToSingle(string value, int ColumnNo)
        {
            try
            {
                Single result;
                result = Convert.ToSingle(value);
                return result;
            }
            catch (Exception)
            {
                string errorMessage = $"Column {ColumnNo} : Failed to convert \"{value}\" to Single Data Type!";
                MessageBox.Show(errorMessage, "Data Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.None);
                throw;
            }
        }

        /// <summary>
        /// ConvertToDouble converts a string value to a double datatype.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="ColumnNo">column ordinal number</param>
        /// <returns>double datatype</returns>
        private static Double ConvertToDouble(string value, int ColumnNo)
        {
            try
            {
                Double result;
                result = Convert.ToDouble(value);
                return result;
            }
            catch (Exception)
            {
                string errorMessage = $"Column {ColumnNo} : Failed to convert \"{value}\" to Double Data Type!";
                MessageBox.Show(errorMessage, "Data Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.None);
                //return 0;
                throw;
            }
        }

        /// <summary>
        /// ConvertToByte converts a string value to byte datatype.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="ColumnNo">column ordinal number</param>
        /// <returns>byte datatype</returns>
        private static Byte ConvertToByte(string value, int ColumnNo)
        {
            try
            {
                byte result;
                result = Convert.ToByte(value);
                return result;
            }
            catch (Exception)
            {
                string errorMessage = $"Column {ColumnNo} : Failed to convert \"{value}\" to Byte Data Type!";
                MessageBox.Show(errorMessage, "Data Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.None);
                throw;
            }
        }

        //private static Byte[] ConvertToByteArray(string value)
        //{
        //    try
        //    {
        //        byte[] result;
        //        result = Convert.FromBase64CharArray(value);
        //        return result;
        //    }
        //    catch (Exception)
        //    {
        //        string errorMessage = $"Failed to convert {value} to Byte Data Type!";
        //        MessageBox.Show(errorMessage, "Data Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.None);
        //        //return 0;
        //throw;
        //    }
        //}

        /// <summary>
        /// ConvertStringToTimeSpan converts the string to time span datatype.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="ColumnNo">column ordinal number</param>
        /// <returns>a TimeSpan object</returns>
        private static TimeSpan ConvertStringToTimeSpan(string value, int ColumnNo)
        {
            try
            {
                TimeSpan span;
                span = TimeSpan.Parse(value);
                return span;
            }
            catch (Exception)
            {
                string errorMessage = $"Column {ColumnNo} : Failed to convert \"{value}\" to TimeSpan Data Type!";
                MessageBox.Show(errorMessage, "Data Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.None);
                throw;
            }
        }

        /// <summary>
        /// ConvertStringToDateTimeOffset converts the string to date time offset.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="ColumnNo">column ordinal number</param>
        /// <returns>the date time offset</returns>
        private static DateTimeOffset ConvertStringToDateTimeOffset(string value, int ColumnNo)
        {
            try
            {
                DateTimeOffset offset;
                offset = DateTimeOffset.Parse(value);
                return offset;
            }
            catch (Exception)
            {
                string errorMessage = $"Column {ColumnNo} : Failed to convert \"{value}\" to DataTimeOffset Data Type!";
                MessageBox.Show(errorMessage, "Data Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.None);
                throw;
            }
        }

        #endregion Data Conversion Routines

        #region Database Helper Methods

        /// <summary>
        /// GetDataTable generates a new and empty .Net DataTable.  This DataTable has the same column names
        /// as the database table but the SQL DB datatypes for each column are mapped to .NET datatypes.
        /// Therefore, the Csv File string data for each column is converted to the appropriate and respective
        /// .Net datatype; thereby, assisting the bulk import of data into the SQL server database.
        /// </summary>
        /// <param name="myList">My list.</param>
        /// <returns>a DataTable with column types set to compatible .NET types</returns>
        private static DataTable GetDataTable(List<DbColumnDetails> myList)
        {
            // Get a new empty DataTable structure
            DataTable dt = new DataTable();

            // Get the number columns in myList
            int cnt = myList.Count;

            // declare column parameters
            string colName;
            string colDataType;

            // iterate through myList and generate a column in the datatable
            for (int i = 0; i < cnt; i++)
            {
                // Get the column name and trim any leading or trailing spaces.
                colName = myList[i].ColumnName.Trim();

                // Get the SQL data type for the columns
                colDataType = myList[i].ColumnDataType;

                // Get the corresponding .NET data type
                var value = GetMyDataType(MainForm.SqlToDotNetTypeMap, colDataType.ToLower().Trim());

                // Get the number of characters for specified SQL datatype ---- NOT USED ----
                _ = myList[i].ColumnMaxLength;

                // add a new column to dataTable with the specified .NET datatype
                dt.Columns.Add(new DataColumn(colName, value));
            }
            return dt;
        }

        /// <summary>
        /// GetMyDataType gets the equivalent .Net data type for a column based on SQL server datatype for the
        /// column in the corresponding database table.
        ///
        /// SQL Server Types not supported includes the following:
        /// geography, geometry, sql_variant, hierchyid, and datetime2.
        /// if key not found, the default type is string.
        /// </summary>
        /// <param name="typeMap"></param>
        /// <param name="SqlDataType">SQL Server Datatype.</param>
        /// <returns>.NET datatype</returns>
        public static Type GetMyDataType(Dictionary<string, Type> typeMap, string SqlDataType)
        {
            // try to get the .Net datatype Type
            if (typeMap.TryGetValue(SqlDataType.ToLower(), out var type))
            {
                return (Type)type;
            }
            else
            {
                // should never get here because SQL data types are verified directly after
                // starting the data transfer process.
                string message = $"Data Type: {SqlDataType} not supported!";
                MessageBox.Show(message, "Map SQL to .NET Datatype Function", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Since the key was not found, the default datatype will be string
                typeMap.TryGetValue("string", out var stype);
                return (Type)stype;
            }
        }

        private static bool VerifySQLDataTypesAreSupported(List<DbColumnDetails> MyDbColumns)
        {
            foreach (DbColumnDetails details in MyDbColumns)
            {
                // Check to see if the datatype is supported
                if (!MainForm.SqlToDotNetTypeMap.ContainsKey(details.ColumnDataType.ToLower()))
                {
                    string message = $"Data Type: {details.ColumnDataType} not supported! Please remove or change the column \"{details.ColumnName}\" in the database table!";
                    MessageBox.Show(message, "Map SQL to .NET Datatype Function", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// VerifyUserIntentToClearDbTable verifies the user's intent to clear the database table 
        /// and reseed the index.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns>true or false if yser wants to clear database table</returns>
        private bool VerifyUserIntentToClearDbTable(string table)
        {
            string Msg = $"Are you sure you want to delete data in table: {table}?";
            DialogResult result = MessageBox.Show(Msg, "Database Table", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// ClearDatabaseTable clears the database table.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="connectionString">The connection string.</param>
        private static void ClearDatabaseTable(string table, string connectionString)
        {
            string DbQuery = $"EXEC sp_msforeachtable \"ALTER TABLE ? NOCHECK CONSTRAINT ALL\"; DELETE FROM {table} DBCC CHECKIDENT ('{table}',RESEED,0); EXEC sp_msforeachtable \"ALTER TABLE ? CHECK CONSTRAINT ALL\";";
            //string DbQuery = $"EXEC sp_msforeachtable \"ALTER TABLE ? NOCHECK CONSTRAINT ALL\"; DELETE FROM {table} DBCC CHECKIDENT ('{table}',RESEED,0); EXEC sp_msforeachtable \"ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL\";";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // open the connection and execute the query
                SqlCommand command = new SqlCommand(DbQuery, connection);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show($"{rowsAffected} rows were affected.", "Clear Table", MessageBoxButtons.OK);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Clear Table", MessageBoxButtons.OK);
                    return;
                }
            }
        }

        /// <summary>
        /// GetTableSize gets the size of the table in data rows. Note that in the Select Count
        /// query both the table name and schema name is required.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="schema">The schema name.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>database table row count</returns>
        private static int GetTableSize(string table, string schema, string connectionString)
        {
            // The query requires table name in the form of "dbo.table" or "schema.table"
            string schemaPlusTable = schema + "." + table;
            string query = $"SELECT COUNT (*) FROM {schemaPlusTable}";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                int rowCount = (int)command.ExecuteScalar();
                return rowCount;
            }
        }

        #endregion Database Helper Methods

        #region CSV File Helper Methods

        /// <summary>
        /// GetCSVFileName gets the name of the CSV file via OpenFileDialog.
        /// </summary>
        /// <returns>returns to full filepath of the CsvFile</returns>
        private string GetCSVFileName()
        {
            // Identify and focus on CSV, Txt, and All files.
            String myFilter = "CSV files(*.csv)|*.csv|txt files (*.txt)|*.txt|All files (*.*)|*.*";
            String filepath;

            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = myFilter,
                Multiselect = false,
                CheckFileExists = true,
                CheckPathExists = true,
                InitialDirectory = Environment.SpecialFolder.MyComputer.ToString()
            };

            DialogResult result = ofd.ShowDialog();

            if (result == DialogResult.OK)
            {
                filepath = ofd.FileName.ToString();

                // Verify file exists, is not empty or contains blank lines
                if (!CheckForValidCsvFile(filepath))
                {
                    MessageBox.Show("File is empty!", "Open File Dialog", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return null;
                }
                return filepath;
            }

            // If you get here then User pressed Cancel button

            ClearCSVDataGridView();
            CsvColumnCnt = 0;
            CSVFileColumnsDetectedTxtBx.Clear(); ;
            CSVFileDataRowsDetectedTxtBx.Clear();
            return null;
        }

        /// <summary>
        /// Checks for valid CSV file. Checks to see if the file exists, is not
        /// a zero length file, or the first line is not blank.
        /// </summary>
        /// <param name="csvFilePath">The CSV file path.</param>
        /// <returns>true if good file, false if empty</returns>
        private static bool CheckForValidCsvFile(string csvFilePath)
        {
            try
            {
                // Check to be sure file really exists.
                if (!File.Exists(csvFilePath)) return false;

                // Check if file has zero bytes.
                FileInfo csvFileInfo = new FileInfo(csvFilePath);
                if (csvFileInfo.Length == 0) return false;

                // Check first line is not null or file contains blank lines
                using (StreamReader sr = new StreamReader(csvFilePath))
                {
                    string firstLine = sr.ReadLine();
                    if (firstLine == null) return false;
                    else if (string.IsNullOrEmpty(firstLine)) return false;
                }
                // file has data
                return true;
            }
            catch (IOException ex)
            {
                MessageBox.Show($"I/O Error: {ex.Message}", "Check For Valid File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show($"Access Error: {ex.Message}", "Check For Valid File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// DetectDelimiter detects the Csv file delimiter used.  NOTE: a colon ':' is not a recommended
        /// delimiter because the colon is used in DateTime datatype when output as a string in a Csv file.
        /// Recommend either the comma or the vertical bar '|'.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>Returns the delimiter character</returns>
        public static char DetectDelimiter(string filePath)
        {
            // identify the standard delimiters
            var delimiters = new[] { ',', ';', '\t', '|', ':' };

            // create a dictionary to hold delimiter counts
            var delimiterCounts = new Dictionary<char, int>();

            // set counts to 0
            foreach (var delimiter in delimiters)
            {
                delimiterCounts[delimiter] = 0;
            }

            // read a single line from the Csv file and count delimeter characters
            using (var reader = new StreamReader(filePath))
            {
                string line = reader.ReadLine();
                if (line != null)
                {
                    foreach (var delimiter in delimiters)
                    {
                        delimiterCounts[delimiter] = line.Split(delimiter).Length - 1;
                    }
                }
            }

            // Return the delimiter with the highest count
            return delimiterCounts.Count > 0 ?
                delimiterCounts.Aggregate((x, y) => x.Value > y.Value ? x : y).Key : ',';
        }

        /// <summary>
        /// GetCsvColumnDetails gets the CSV column Details from the header record.
        /// </summary>
        /// <returns>returns list of columns headings and column order</returns>
        private List<CsvColumnDetails> GetCsvColumnDetails(string filepath)
        {
            // instantiates a new object orig defined as global variable
            List<CsvColumnDetails> columnDetails = new List<CsvColumnDetails>();

            if (filepath != null)
            {
                // identify the delimiter used.
                CsvDelimiter = DetectDelimiter(filepath);

                // set the delimiter char in the CsvHelper config file
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = CsvDelimiter.ToString(),
                };

                // read the Csv file header
                using (var reader = new StreamReader(filepath))
                {
                    var csv = new CsvReader(reader, config);
                    csv.Read();
                    csv.ReadHeader();
                    string[] headerRow = csv.HeaderRecord;

                    // get column count from header csvRow
                    int cnt = headerRow.Length;

                    // get column name and ordinal position
                    for (int i = 0; i < cnt; i++)
                    {
                        CsvColumnDetails item = new CsvColumnDetails
                        {
                            ColumnId = i + 1,
                            ColumnName = headerRow[i].ToString(),
                            ColumnOrdinalPosition = (i + 1).ToString()
                        };
                        columnDetails.Add(item);
                    }
                }
            }
            else
            {
                return null;
            }
            return columnDetails;
        }

        /// <summary>
        /// GetCsvRecordCount gets the CSV record count by reading the entire file.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <returns>count of records read</returns>
        private int GetCsvRecordCount(string filepath)
        {
            int rowCount = 0;

            if (filepath != null)
            {
                using (var reader = new StreamReader(filepath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    while (csv.Read())
                    {
                        rowCount++;
                    }
                    return rowCount - 1;
                }
            }
            return 0;
        }

        /// <summary>
        /// DisplayCSVFileColumns displays the CSV file columns names and ordinal position
        /// in the datagrid.
        /// </summary>
        private void DisplayCSVFileColumns()
        {
            // Clear binding source
            csvColumnDetailsBindingSource.DataSource = null;
            csvColumnDetailsBindingSource.EndEdit();

            // Clear datagridview control
            CSVDataGridView.DataSource = null;
            CSVDataGridView.Rows.Clear();
            CSVDataGridView.Refresh();
            CSVDataGridView.EndEdit();

            // Populate List of CSV file column data by adding blank rows
            List<CsvColumnDetails> DisplayCsvColumns = GetCsvColumnDisplayList(MyCsvColumns);

            // Set binding source to List of CSV file column data
            csvColumnDetailsBindingSource.DataSource = DisplayCsvColumns;
            csvColumnDetailsBindingSource.EndEdit();

            // Connect binding source to datagridview control
            CSVDataGridView.DataSource = csvColumnDetailsBindingSource;
            CSVDataGridView.AutoResizeColumn(1);
            CSVDataGridView.EndEdit();
        }

        /// <summary>
        /// ClearCSVDataGridView clears the CSV data grid view.
        /// </summary>
        private void ClearCSVDataGridView()
        {
            CSVDataGridView.DataSource = null;
            CSVDataGridView.Rows.Clear();
            CSVDataGridView.Refresh();
            CSVDataGridView.EndEdit();
        }

        /// <summary>
        /// Displays my CSV columns.
        /// </summary>
        /// <param name="csvColumnDetails">The List of Csv column details.</param>
        /// <returns>Returns List of CsvColumnDetails with added blank lines</returns>
        private List<CsvColumnDetails> GetCsvColumnDisplayList(List<CsvColumnDetails> csvColumnDetails)
        {
            // create a new list from the existing list
            List<CsvColumnDetails> columnDetails = new List<CsvColumnDetails>(csvColumnDetails);

            // get current row count 
            int currentRowCount = columnDetails.Count;
            if (currentRowCount < 14)
            {
                // add blank rows to make total of 14 rows
                for (int i = currentRowCount; i < 14; i++)
                {
                    columnDetails.Add(new CsvColumnDetails()
                    {
                        ColumnId = i + 1,
                        ColumnName = string.Empty,
                        ColumnOrdinalPosition = string.Empty
                    });
                }
                return columnDetails;
            }
            else
            {
                // already have 14 or more rows, just return the list
                return columnDetails;
            }
        }

        /// <summary>
        /// DisplayEmptyCSVDataGridView() Displays the empty rows in the data grid view. 
        /// This is purely a cosmetic feature to improve the look of the form when loaded and before
        /// data is selected to be viewed.
        /// </summary>
        private void DisplayEmptyCSVDataGridView()
        {
            // Create an EmptyList with 14 empty rows. 
            List<CsvColumnDetails> EmptyList = new List<CsvColumnDetails>();
            for (int i = 0; i < 14; i++)
            {
                EmptyList.Add(new CsvColumnDetails()
                {
                    ColumnId = i,
                    ColumnName = string.Empty,
                    ColumnOrdinalPosition = string.Empty
                });
            }

            // set the binding source to EmptyList
            csvColumnDetailsBindingSource.DataSource = EmptyList;
            csvColumnDetailsBindingSource.EndEdit();

            // connect binding source to datagridview control
            CSVDataGridView.DataSource = csvColumnDetailsBindingSource;
            CSVDataGridView.EndEdit();
        }

        #endregion CSV File Helper Methods
    }
}