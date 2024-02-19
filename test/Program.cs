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
using DB_Analyzer.ReportSavers.DbReportSavers;
using Microsoft.Data.SqlClient;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json;

#region main

string connString = @"Server=127.0.0.1;port=3306;uid=root;Database=portal_db";
MySqlManager manager = new(connString);

//string connString = @"Server=(localdb)\MSSQLLocalDB;Database=portal_db;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true";
//SqlServerManager manager = new(connString);



await manager.ConnectToDBAsync();


List<IReportItem<object>> reportItems = manager.GetAllPossibleReportItems();


await manager.Analyze(reportItems);

//ReportSaver reportSaver = new MySqlReportSaver(@"Server=127.0.0.1;port=3306;uid=root;Database=test", manager.Connection.Database);
ReportSaver reportSaver = new SqlServerReportSaver(@"Server = (localdb)\MSSQLLocalDB; Database = s; Trusted_Connection = True; Encrypt = False", manager.Connection.Database);

await manager.SaveReport(reportSaver, reportItems);

#endregion

#region test

#endregion