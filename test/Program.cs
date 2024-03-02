using DB_Analyzer.Analyzers;
using DB_Analyzer.Managers;
using DB_Analyzer.ReportItems;
using DB_Analyzer.ReportItems.Flags;
using DB_Analyzer.ReportItems.Tables;
using DB_Analyzer.ReportSavers;
using DB_Analyzer.ReportSavers.DbReportSavers;
using DB_Analyzer.ReportSavers.TxtFileReportSaver;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using DB_Analyzer.ReportSavers.Supporting_Classes.StructureProviders.TxtFileStructureProvider;
using System.Data.Common;

#region main

//string connString = @"Server=127.0.0.1;port=3306;uid=root;Database=portal_db;";
//MySqlManager manager = new(connString);

string connString = @"Server=(localdb)\MSSQLLocalDB;Database=portal_db;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true";
SqlServerManager manager = new(connString);


await manager.ConnectToDBAsync();


List<ReportItem> reportItems = manager.GetAllPossibleReportItems();


await manager.Analyze(reportItems);


//ReportSaver reportSaver = new MySqlReportSaver(@"Server=127.0.0.1;port=3306;uid=root;Database=test", manager.Connection);
//ReportSaver reportSaver = new SqlServerReportSaver(@"Server=(localdb)\MSSQLLocalDB;Database=s;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true", manager.Connection);
ReportSaver reportSaver = new TxtFileReportSaver(manager.Connection);

await manager.SaveReport(reportSaver, reportItems);

#endregion