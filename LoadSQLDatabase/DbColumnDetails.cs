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
    /// Public class DbColumnDetails describes the data for each column in the database table.
    /// </summary>
    public class DbColumnDetails
    {
        private int columnId;
        private string databaseName;
        private string schemaName;
        private string tableName;
        private string columnName;
        private string columnOrdinalPosition;
        private string columnDataType;
        private string columnMaxLength;
        private string columnIsNullable;
        private bool columnIsComputed;

        /// <summary>
        /// Constructor that initializes a new instance of the <see cref="DbColumnDetails"/> class.
        /// </summary>
        /// <param name="p_columnId">The p column identifier.</param>
        /// <param name="p_databaseName">Name of the p database.</param>
        /// <param name="p_schemaName">Name of the p schema.</param>
        /// <param name="p_tableName">Name of the p table.</param>
        /// <param name="p_columnName">Name of the p column.</param>
        /// <param name="p_columnOrdinalPosition">The p column ordinal position.</param>
        /// <param name="p_columnDataType">Type of the p column data.</param>
        /// <param name="p_colMaxLength">Maximum length of the p col.</param>
        /// <param name="p_columnIsNullable">The p column is nullable.</param>
        /// <param name="p_columnIsComputed">if set to <c>true</c> [p column is computed].</param>
        public DbColumnDetails(int p_columnId, string p_databaseName, string p_schemaName, string p_tableName,
            string p_columnName, string p_columnOrdinalPosition, string p_columnDataType,
            string p_colMaxLength, string p_columnIsNullable, bool p_columnIsComputed)
        {
            columnId = p_columnId;
            databaseName = p_databaseName;
            schemaName = p_schemaName;
            tableName = p_tableName;
            columnName = p_columnName;
            columnOrdinalPosition = p_columnOrdinalPosition;
            columnDataType = p_columnDataType;
            columnMaxLength = p_colMaxLength;
            columnIsNullable = p_columnIsNullable;
            columnIsComputed = p_columnIsComputed;
        }

        /// <summary>
        /// Constructor that initializes a new empty instance of the <see cref="DbColumnDetails"/> class.
        /// </summary>
        public DbColumnDetails()
        { }

        /// <summary>
        /// Gets or sets the column identifier.
        /// </summary>
        /// <value>
        /// The column identifier.
        /// </value>
        public int ColumnId
        { get { return columnId; } set { columnId = value; } }

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
        /// The name of the schema.
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
        /// Gets or sets the name of the column.
        /// </summary>
        /// <value>
        /// The name of the column.
        /// </value>
        public string ColumnName
        { get { return columnName; } set { columnName = value; } }

        /// <summary>
        /// Gets or sets the column ordinal position.
        /// </summary>
        /// <value>
        /// The column ordinal position.
        /// </value>
        public string ColumnOrdinalPosition
        { get { return columnOrdinalPosition; } set { columnOrdinalPosition = value; } }

        /// <summary>
        /// Gets or sets the type of the column data.
        /// </summary>
        /// <value>
        /// The type of the column data.
        /// </value>
        public string ColumnDataType
        { get { return columnDataType; } set { columnDataType = value; } }

        /// <summary>
        /// Gets or sets the maximum length of the column.
        /// </summary>
        /// <value>
        /// The maximum length of the column.
        /// </value>
        public string ColumnMaxLength
        { get { return columnMaxLength; } set { columnMaxLength = value; } }

        /// <summary>
        /// Gets or sets the column is nullable.
        /// </summary>
        /// <value>
        /// The column is nullable.
        /// </value>
        public string ColumnIsNullable
        { get { return columnIsNullable; } set { columnIsNullable = value; } }

        /// <summary>
        /// Gets or sets a value indicating whether [column is computed].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [column is computed]; otherwise, <c>false</c>.
        /// </value>
        public bool ColumnIsComputed
        { get { return columnIsComputed; } set { columnIsComputed = value; } }
    }
}