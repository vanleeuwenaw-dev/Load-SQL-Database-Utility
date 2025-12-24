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
    /// Class  DbComputedColumnDetails defines the structure of a list object with column name
    /// and whether that column is a computed column, vice a column that we need to load Csv values
    /// into.
    /// </summary>
    public class DbComputedColumnDetails
    {
        private string columnName;
        private bool is_Computed;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbComputedColumnDetails"/> class.
        /// </summary>
        /// <param name="p_columnName">Name of the p column.</param>
        /// <param name="p_Is_Computed">if set to <c>true</c> [p is computed].</param>
        public DbComputedColumnDetails(string p_columnName, bool p_Is_Computed)
        {
            columnName = p_columnName;
            is_Computed = p_Is_Computed;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbComputedColumnDetails"/> class.
        /// </summary>
        public DbComputedColumnDetails()
        { }

        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        /// <value>
        /// The name of the column.
        /// </value>
        public string ColumnName
        { get { return columnName; } set { columnName = value; } }

        /// <summary>
        /// Gets or sets a value indicating whether this column "is computed".
        /// </summary>
        /// <value>
        ///   <c>true</c> if this column is computed; otherwise, <c>false</c>.
        /// </value>
        public bool Is_Computed
        { get { return is_Computed; } set { is_Computed = value; } }
    }
}