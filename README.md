# Load SQL Database Utility
# By Anthony van Leeuwen
# December 2, 2025
## What Does the Load SQL Database Utility Do?
The Load SQL Database Utility reads a Comma Separated Value (CSV) file and transfers the data to the associated database table.  The utility reads the column schema of both the CSV file and the associated database table, creates a .NET data table, reads the CSV file, performs data conversions, inserts the data into the .NET data table, and then bulk copies the data to the associated SQL Server database table.
## Why Was This Utility Developed?
The Load SQL Database Utility was originally developed due to obstacles the author encountered attempting to publish changes to a Microsoft SQL Server Express database from a Visual Studio database project.  The problems encountered included the following:

1. If the database contained data, changes to tables or to stored procedures were not published to the database from the Visual Studio Project.  As a result, changes had to be made manually to the database using SQL Server Management Studio (SSMS).  Making changes twice was not considered to be a good practice.
2. Manually removing data from the database tables was difficult if not impossible due to foreign key constraints.
3. Deleting the database allowed Visual Studio to publish the updated database to SQL server, but the data would be lost.
4. Exporting data into a CSV file and then re-importing the data did not always work due to data conversion issues and/or improperly configured CSV files.
5. Manually re-entering the data using SQL Server Management Studio (SSMS) or the associated windows application being developed was too time consuming. 

**NOTE:** To be honest some of these issues do have work arounds known to more experienced developers and database experts.

## What Makes Load SQL Database Utility Different?
The Load SQL Database Utility is different in that it is not dedicated to a specific database or to a specific database table, but works with all kinds of Microsoft SQL Server databases and tables with exceptions as mentioned below.  The utility reads the header on the CSV file and displays the column data to the user.  In addition, after the database name is defined, the utility will get the names of each table in the database and then when the user selects the table of interest, the column data will be displayed to the user.  When the user is satisfied that the CSV and the database data columns match a single button push starts the process of transferring the data. 

## Load SQL Database Utility Compatible Data Types.
The Load SQL Database Utility can be used with any Microsoft SQL Server Database and associated CSV files providing the SQL data types used do not include the following:

* datetime2
* geography
* geometry
* hierarchyid
* sql_variant
* xml
* rowversion, image, binary, varbinary

The Load SQL Database Utility is compatible with the following SQL data types:

* int, bigint, smallint, tinyint
* boolean, bit
* decimal, money, smallmoney, numeric, 
* real, float, double, 
* char, nchar, varchar, nvarchar, text, ntext, string
* datetime, smalldatetime, datetimeoffset, date, time, timestamp
* uniqueidentifier

## Limitations of the Load SQL Database Utility are as follows:

1. If for some reason the data type is not found in our dictionary, see Appendix A, then a warning message will be displayed to the user, and the data transfer process will not proceed.
2. This utility is meant to aid the software developer and not meant to replace excellent tools such as SQL Server Management Studio.

## How Is the Load SQL Database Utility Used?
During the development of a windows application the need to update the associated database project will occur.  This is where the Load SQL Database Utility is used to restore the data in the database tables.  The process the author envisioned is as follows: 

1. Update the Visual Studio Database Project.
2. Save database table data in CSV data files using SQL Server Management Studio.
3. Delete the database using SQL Server Management Studio.
4. Publish the updated Database Project using Visual Studio.  (Publishing recreates the database including tables and stored procedures but with NO data.)
5. If the table column structure has not changed, then use the Load SQL Database Utility to restore the data into the database table.  In the event the database table structure has changed, see the next topic.

Another potential use for this utility is to simply load sample data into the database where the sample data is created with an ordinary editor or even a spreadsheet program.


## When should A CSV File Be Edited with a spreadsheet Program?
There are several reasons why a spreadsheet program e.g., Microsoft™ Excel, should be used to review and/or edit the CSV file before using the CSV file to import data to the database table. 

Some of the reasons are as follows:
* Add, correct, or modify data in one or more data columns.
* Delete a data column that has been removed in the new table structure.
* Insert a new data column that has been defined in the new table structure and fill that column with data if possible.
* Insert a header row with column names.
* Reorder the sequence of rows to match the database table.

To ensure that you get the best CSV formatted data as possible, heed the warnings that follow.

**NOTE:** Use SSMS to export the CSV file using the following instructions:

1. Open SSMS and connect to your database.
2. Right-click the database name in the Object Explorer, select Tasks, and then choose Export Data.
3. In the wizard, select your data source (the current database) and click Next.
4. For the destination, choose Flat File Destination, specify the file path, and set the file format to “.csv.”
5. Select the table or write a custom query to specify the data for export.
6. Configure column delimiters (e.g., commas), text qualifiers (e.g., quotation marks), and row delimiters (e.g., new lines).
7. Click Finish to execute the export process.

For more information see: [How to Export Data from Microsoft SQL Server to a CSV File] (https://learnsql.com/blog/export-csv-file-ms-sql-server/ "").

** NOTE:** If your database table contains columns having datetime values, do not use the colon ":" as a field delimiter.  Datetime columns use a colon to separate parts of a date time structure e.g., "2006-07-01 00:00:00".

** NOTE:** Do not open the CSV file in Microsoft Excel as you would any other file.  Perform the following steps:

1. Open Microsoft Excel.  Create a new blank workbook.
2. Select the Data Tab in the ribbon at the top.
3. In the Get & Transform Data section at the ribbon top left, select the From Text/CSV button to open the file.
4. After the file is opened via the file open dialog, a sample of CSV file’s data is presented to the user.  Inspect the sample data, and then press the Load button to fill in the workbook.
5. Inspect all data in the workbook.  Verify the column headers, verify all columns have the correct data, verify that all rows have the same number of columns. 
 
## What Are the Requirements of the CSV File and Database Table?
The requirements for the CSV File and Database table are as follows:

* The columns in the CSV file should correspond one-for-one with the columns in the database table.  In other words, in the same column order.
* The CSV file should have a header with the column names which should preferably match the database table column names.
* The database table should contain columns using only datatypes compatible with the Load SQL Database Utility. 

**NOTE:** The Load SQL Database Utility assumes the first line contains the header.  Therefore, when reading CSV file data the first line is omitted.

## When Should Load SQL Database Utility NOT be used?
The utility is designed to transfer smaller amounts of data during the development of a .NET application and associated database and not designed to transfer millions of rows of data from a CSV file.  All data is held in memory. 

## Can Microsoft SQL Server Be Accessed Remotely?
The Load SQL Database Utility can access a Microsoft SQL Server remotely.  The only difference is that a username and password is required to access the remote machine and the SQL server. 

# Load SQL Database Utility Design
The Load SQL Database Utility consists of a Main Form and an auxiliary form called the Edit Connection Form. 

## Edit Connection Form
The Edit Connection Form is used to specify the information used to create, save, and test the Database connection string (See Figure 1).  If windows authentication is chosen, the user ID and user Password textboxes are disabled; otherwise, the user ID and Password are included in the connection string, out in the open.

![2025-11-27_130618.png](2025-11-27_130618.png "")Figure 1. Edit Connection Form

## User Input
The Edit Connection Form has controls allowing the user to identify the data source, database name, windows authentication, integrated security, trust server certificate, user id, user password, encrypt data.  Controls are provided to generate, display, save, and test the connection string. The OK button is provided to close the form and return to the main form.

The connection data is saved to the user’s setting file and will be available the next time the user opens the application. 

### Generate Connection String Button
Press this button to generate and display the connection string.
### Save Connection String Button
Press this button to save the connection string to the user settings file.
### Test Connection Button
Press this button to test the connection string after it is saved.
### OK Button
Press this button to close the Edit Connection Form and return to the Main Form.

## Main Form
The main form has two buttons at the top of the form, see Figure 2.  One button titled “Get CSV Filename” is used to select the CSV file.  The second button is titled “Get Database Connection String” which opens the Edit Connection Form to obtain or change the database connection string.  If a database connection string has already been saved, it will automatically load when the application starts.

![2025-11-27_130757.png](2025-11-27_130757.png "")
Figure 2.  Load SQL Database Utility Main Form

### Select CSV File Using File Open Dialog.
The file open dialog is used to select the full file path of the CSV file.  After CSV File selection, CSVHelper is used to read the CSV file’s header and extract the columns names, automatically detect the delimiter used, and generate the column ordinal position for each column of data.  Both the column name and ordinal position are displayed to the user in a DataGridView control.
### Processing Data Records Using CSVHelper 
The Load SQL Database Utility uses CSVHelper to process data records or rows in the CSV file.  For each record or data row in the CSV file, CSVHelper returns an array of string data where each array element contains the data corresponding to a column in the data row or record.  The number of columns in each CSV file record is counted and compared against the number of columns in the database table.  This assures that if mismatches occur, the user will be warned to correct the CSV file.

###CSV File Statistics
The utility also displays the number of columns and the number of rows detected in the CSV file.

###Selecting The Corresponding Table from The Database
Once the connection string has been defined, the database schema is retrieved from the SQL server to identify all the tables in the database.  A combo box then displays a list of tables allowing the user to select the one that corresponds to the CSV file data.

After the user selects the database table, the table’s column data is retrieved and displayed to the user.  The data displayed is as follows:  

* The Computed column checkbox if checked indicates that the column is computed and therefore will not be transferred to the database table.
* The Column Name identifies the column data. 
* The Column Data Type identifies the SQL Server Data Type.
* The Column Length indicates the length of certain columns. 
* The Column Ordinal Position indicates the order of the column in the table. 
* The Column Isnullable flag indicates if a null is allowed in the column.

It should be noted that the Column Length and the Isnullable flag are not used by the utility and are presented to the user for information only.  

### Creating A .NET Data Table 
After the user selects a database table, the database table’s column information is retrieved from the database.  The SQL data types for each column are mapped to corresponding .NET datatypes which are then used to create an empty .NET table.  See Appendix A for a list of mapped datatypes that are currently implemented.

### Database Table Statistics 
After the user selects a database table, the number of columns and rows are detected and displayed to the user.  
Database Actions and Results
This section displays a check box that when checked will clear and reseed the table before data is transferred.  This provides a facility to redo the data transfer.  Also there is a button to initiate the Data Transfer process as well as a textbox displaying the number of rows transferred to the database.
### Initiating And Completing the Data Transfer Process
After the user presses the button to initiate the data transfer a series of actions are performed and are summarized as follows:

1. Verify that the CSV Filename is valid and the file exists.
2. Verify that a database table has been selected.
3. Check if the Clear Table and Reseed checkbox has been selected and confirm with the user that they want to clear the table.  If so, the table is cleared and the number of rows affected is reported back to the user along with the number of rows currently in the table.
4. Verify that no unsupported SQL Data Types are in the target table.  If so, exit the data transfer process.
5. An empty .NET data table is created with the same number of columns as the table the user selected, but with .Net datatypes that correspond to the SQL Data Types in the database table.
6. The CSV Configuration values for the CSVHelper are set including identifying the automatically detected delimiter used in the CSV file. 
7. The CSV file is read omitting the header record and all remaining records are loaded.
8. Iterate through each record, verify the number of columns, get a data row from the .NET data table, and convert the string data to the appropriate .NET datatype and insert into the data row.  When done, add the data row to the data table, and repeat until all CSV records are processed. 
9. Get the total number of rows in the data table and publish the count to the Rows Processed textbox.
10. Use SQL Bulk Copy to transfer the table rows to the table in the database.
11. Detect the total number of data rows in the database table and report that number to the Rows detected textbox.  
12. Notify the user via message box that the transfer has been completed and the total number of rows transferred. 

##Error Detection
The following are a list of errors that are checked after the data transfer process has been initiated:

* Null or empty CSV filename.
* The CSV file exists.
* Database Table has been selected and has one or more columns.
* The CSV record has more or fewer columns than the database table.
* Verify that no unsupported SQL Data Types are used.
* Conversion errors when converting CSV file string data to .NET datatypes.  Only the first data conversion error is caught and reported to the user.  Usually this means that an extra column occurs in the CSV record and columns are no longer in the correct sequence.
* SQL Bulk Copy errors encountered.

**IMPORTANT:** At the current time, there must be a one-to-one column correspondence between the CSV file columns and the database table columns.  Column names in the CSV header are not used to map the data to the database table, but the order of the columns is!  Obviously, the datatypes must correspond.

Once the data transfer finishes a message box is displayed informing the user the process has finished, and the number of records transferred to the SQL Server database.
##Future Potential Revisions
There are several possible changes to enhance the utility including the following:
1. Direct mapping of the CSV file column to the target database table.  Providing that columns containing unsupported SQL Datatypes are omitted.
2. Determine if any of the unsupported SQL Datatypes can be incorporated into the existing utility?
3. Highlight unsupported SQL Datatypes on the main form via background coloring and potentially provide a way to omit them perhaps by designating the column as a computed column.  This would allow the user to import all the other data and then produce an alternate way to repopulated that column.  
4. Provide a method to automatically generate a query that can be plugged into the SQL Import and Export Wizard (comes with SSMS and SQL Server) to omit unsupported SQL Datatypes.
5. Modify the CSV file method that reads the file and reports how many rows are contained in the file to include verifying the number of columns in each row.  
6. Add a checkbox to the main form indicating the CSV file has no column headings and then automatically generate column headings.


# Appendix A
# Datatype Mapping
|No.|SQL Datatype|.NET Datatype|
|:--:|:------------:|:-------------:|
|1|int|int|
|2|bigint|long|
|3|smallint|short|
|4|tinyint|Byte|
|5|boolean|Boolean|
|6|bit|Boolean|
|7|decimal|Decimal|
|8|money|Decimal|
|9|smallmoney|Decimal|
|10|numeric|Decimal|
|11|real|Single|
|12|float|Double|
|13|double|Double|
|14|nvarchar|string|
|15|char|string|
|16|nchar|string|
|17|text|string|
|18|ntext|string|
|19|date|Datetime|
|20|datetime|Datetime|
|21|smalldatetime|Datetime|
|22|datetimeoffset|DatetimeOffset|
|23|time|TimeSpan|
|24|timestamp|Datetime|
|25|uniqueidentifier|GUID|




## Load SQL Database Project
## © 2025 Anthony W. van Leeuwen
   
  
