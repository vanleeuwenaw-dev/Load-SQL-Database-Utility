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
namespace LoadSQLDatabase
{
    /// <summary>
    /// Class DbTableDetails is a class that describes the elements of data for tables in the database.
    /// </summary>
    public class DbTableDetails
    {
        private int catalogId;
        private string databaseName;
        private string schemaName;
        private string tableName;
        private string tableType;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbTableDetails"/> class.
        /// </summary>
        /// <param name="p_catalogId">The p catalog identifier.</param>
        /// <param name="p_databaseName">Name of the p database.</param>
        /// <param name="p_schemaName">Name of the p schema.</param>
        /// <param name="p_tableName">Name of the p table.</param>
        /// <param name="p_tableType">Type of the p table.</param>
        public DbTableDetails(int p_catalogId, string p_databaseName, string p_schemaName, string p_tableName, string p_tableType)
        {
            catalogId = p_catalogId;
            databaseName = p_databaseName;
            schemaName = p_schemaName;
            tableName = p_tableName;
            tableType = p_tableType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbTableDetails"/> class.
        /// </summary>
        public DbTableDetails()
        { }

        /// <summary>
        /// Gets or sets the catalog identifier.
        /// </summary>
        /// <value>
        /// The catalog identifier.
        /// </value>
        public int CatalogId
        {
            get { return catalogId; }
            set { catalogId = value; }
        }

        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        /// <value>
        /// The name of the database.
        /// </value>
        public string DatabaseName
        { get { return databaseName; } set { databaseName = value; } }

        /// <summary>
        /// Gets or sets the name of the schema.
        /// </summary>
        /// <value>
        /// The name of the schema. Indicates the owner of the table e.g. dbo.tablename
        /// </value>
        public string SchemaName
        { get { return schemaName; } set { schemaName = value; } }

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public string TableName
        { get { return tableName; } set { tableName = value; } }

        /// <summary>
        /// Gets or sets the type of the table.
        /// </summary>
        /// <value>
        /// The type of the table: "Base Table" or "View"
        /// </value>
        public string TableType
        { get { return tableType; } set { tableType = value; } }
    }
}