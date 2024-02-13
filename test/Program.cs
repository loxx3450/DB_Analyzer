using DB_Analyzer.Analyzers;
using DB_Analyzer.Managers;
using DB_Analyzer.ReportItems;
using DB_Analyzer.ReportItems.Flags;
using DB_Analyzer.ReportItems.Functions.Global;
using DB_Analyzer.ReportItems.Functions.Scalar;
using DB_Analyzer.ReportItems.Functions.TableValued;
using DB_Analyzer.ReportItems.StoredProcedures;
using DB_Analyzer.ReportItems.Tables;
using DB_Analyzer.ReportSavers;
using Microsoft.Data.SqlClient;
using MySqlX.XDevAPI.Common;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

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

await manager.SaveReport(new SqlServerReportSaver(@"Server = (localdb)\MSSQLLocalDB; Database = s; Trusted_Connection = True; Encrypt = False"), reportItems);

#endregion

#region test

#endregion