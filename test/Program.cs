using DB_Analyzer.Analyzers;
using DB_Analyzer.Managers;
using DB_Analyzer.ReportItems;
using DB_Analyzer.ReportItems.Flags;
using DB_Analyzer.ReportItems.Functions.Global;
using DB_Analyzer.ReportItems.Functions.Scalar;
using DB_Analyzer.ReportItems.Functions.TableValued;
using DB_Analyzer.ReportItems.StoredProcedures;
using DB_Analyzer.ReportItems.Tables;
using Microsoft.Data.SqlClient;
using MySqlX.XDevAPI.Common;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json;

#region main

//string connString = @"Server=127.0.0.1;port=3306;uid=root;Database=portal_db";
//MySqlManager manager = new(connString);

//string connString = @"Server=(localdb)\MSSQLLocalDB;Database=portal_db;Trusted_Connection=True;Encrypt=False";
string connString = @"Server=(localdb)\MSSQLLocalDB;Database=test;Trusted_Connection=True;Encrypt=False";
SqlServerManager manager = new(connString);



await manager.ConnectToDBAsync();



List<IReportItem<object>> reportItems = manager.GetAllPossibleReportItems();

//List<IReportItem<object>> reportItems = new()
//{
//    new NumberOfTablesReportItem(),
//    new TablesNamesReportItem(),
//    new TablesFullInfoReportItem(),
//    new NumberOfStoredProceduresReportItem(),
//    new StoredProceduresNamesReportItem(),
//    new StoredProceduresFullInfoReportItem(),
//    new NumberOfFunctionsReportItem(),
//    new FunctionsNamesReportItem(),
//    new FunctionsFullInfoReportItem(),
//};



await manager.Analyze(reportItems);



//foreach (var reportItem in reportItems)
//{
//    if (reportItem.Value.GetType() == typeof(ScalarValue<int>))
//    {
//        Console.WriteLine(reportItem.Value);
//    }
//    else if (reportItem.Value.GetType() == typeof(List<string>)) 
//    {
//        List<string> strings = (List<string>)reportItem.Value;
//        strings.ForEach(item => Console.WriteLine(item));
//        Console.WriteLine();
//    }
//}


#endregion

#region test

string query = string.Empty;

query = "IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'reports') " +
    "BEGIN" +
    "   CREATE TABLE reports (" +
    "        ID INT PRIMARY KEY IDENTITY(1,1)," +
    "        DB_Name NVARCHAR(100) NOT NULL," + 
    "        Creation_Date datetime NOT NULL" +
    "    )" +
    "END";



SqlCommand cmd = new(query, (SqlConnection)manager.Connection);

cmd.ExecuteNonQuery();

query = "IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'scalarValues') " +
    "BEGIN" +
    "   CREATE TABLE scalarValues (" +
    "        ID INT PRIMARY KEY IDENTITY(1,1)," +
    "        Report_ID INT NOT NULL," +
    $"        CONSTRAINT FK_ScalarValues_Report_ID FOREIGN KEY (Report_ID) REFERENCES Reports(ID)" +
    "    )" +
    "END";

cmd.CommandText = query;

cmd.ExecuteNonQuery();

query = "IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'referenceValues') " +
    "BEGIN" +
    "   CREATE TABLE referenceValues (" +
    "        ID INT PRIMARY KEY IDENTITY(1,1)," +
    "        Report_ID INT NOT NULL," +
    $"        CONSTRAINT FK_ReferenceValues_Report_ID FOREIGN KEY (Report_ID) REFERENCES Reports(ID)" +
    "    )" +
    "END";

cmd.CommandText = query;

cmd.ExecuteNonQuery();

query = "IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'referenceValues_types') " +
    "BEGIN" +
    "   CREATE TABLE referenceValues_types (" +
    "        ID INT PRIMARY KEY IDENTITY(1,1)," +
    "        referenceValue_Name NVARCHAR(MAX) NOT NULL," +
    "        type_Name NVARCHAR(MAX) NOT NULL" +
    "    )" +
    "END";

cmd.CommandText = query;

cmd.ExecuteNonQuery();

DataTable dt = new DataTable();

foreach (var reportItem in reportItems)
{
    query = string.Empty;

    if (reportItem.GetValueType().IsGenericType && reportItem.GetValueType().GetGenericTypeDefinition() == typeof(ScalarValue<>))
    {
        query = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'scalarValues' AND COLUMN_NAME = '{reportItem.Name}'";

        cmd.CommandText = query;

        int result = (int)cmd.ExecuteScalar();

        if (result == 0)
        {
            query = $"ALTER TABLE scalarValues ADD {reportItem.Name} {ConvertTypesForSqlServer(GetScalarValueType(reportItem.GetValueType()))} NULL";

            cmd.CommandText = query;

            cmd.ExecuteNonQuery();
        }
    }
    else if (reportItem.Value.GetType() == typeof(DataTable))
    {
        query = $"IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = '{reportItem.Name}') " +
                    "BEGIN" +
                    $"   CREATE TABLE {reportItem.Name} (" +
                    "        ID INT PRIMARY KEY,";

        dt = (DataTable)reportItem.Value;

        foreach (DataColumn column in dt.Columns)
        {
            query += $"{column.ColumnName} {ConvertTypesForSqlServer(column.DataType.Name.ToLower())} NULL,";
        }

        query += "        Report_ID INT NOT NULL," +
                    $"        CONSTRAINT FK_{reportItem.Name}_Report_ID FOREIGN KEY (Report_ID) REFERENCES Reports(ID)" +
                    "    )" +
                    "END";

        cmd.CommandText = query;

        cmd.ExecuteNonQuery();
    }
    else
    {
        query = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'referenceValues' AND COLUMN_NAME = '{reportItem.Name}'";

        cmd.CommandText = query;

        int result = (int)cmd.ExecuteScalar();

        if (result == 0)
        {
            query = $"ALTER TABLE referenceValues ADD {reportItem.Name} NVARCHAR(255) NULL";

            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            query = $"INSERT INTO referenceValues_types (referenceValue_Name, type_Name) VALUES('{reportItem.Name}', '{GetReferenceType(reportItem.GetValueType())}')";

            cmd.CommandText = query;

            cmd.ExecuteNonQuery();
        }
    }
}

//    DataTable dt = new DataTable();

//foreach (var reportItem in reportItems)
//{
//    query = string.Empty;

//    query += $"IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = '{reportItem.Name}') " +
//                "BEGIN" +
//                $"   CREATE TABLE {reportItem.Name} (" +
//                "        ID INT PRIMARY KEY,";

//    if (reportItem.Value.GetType() == typeof(ScalarValue<int>))
//    {
//        query += "        Value INT NOT NULL,";
//    }
//    else if (reportItem.Value.GetType() == typeof(List<string>))
//    {
//        query += "        Value NVARCHAR(100) NOT NULL,";
//    }
//    else if (reportItem.Value.GetType() == typeof(DataTable))
//    {
//        dt = (DataTable)reportItem.Value;

//        foreach(DataColumn column in dt.Columns)
//        {
//            query += $"{column.ColumnName} {ConvertTypesForSqlServer(column.DataType.Name.ToLower())} NULL,";
//        }
//    }

//    query += "        Report_ID INT NOT NULL," +
//                $"        CONSTRAINT FK_{reportItem.Name}_Report_ID FOREIGN KEY (Report_ID) REFERENCES Reports(ID)" +
//                "    )" +
//                "END";

//    cmd.CommandText = query;

//    cmd.ExecuteNonQuery();
//}

cmd.Dispose();

string ConvertTypesForSqlServer(string dataType)
{
    return dataType switch
    {
        "string" => "nvarchar(max)",
        "int32" => "int",
        "boolean" => "bit",
        "byte" => "smallint",
        _ => dataType
    };
}

string GetReferenceType(Type dataType)
{
    if (dataType.IsGenericType && dataType.GetGenericTypeDefinition() == typeof(List<>) && dataType.GetGenericArguments()[0] == typeof(string)) { return "List<string>"; }

    return "unknown";
}

static string GetScalarValueType(Type type)
{
    return type.GenericTypeArguments[0].Name.ToLower();
}
#endregion