using DB_Analyzer.Analyzers;
using DB_Analyzer.Managers;
using DB_Analyzer.ReportItems;
using DB_Analyzer.ReportItems.Flags;
//using DB_Analyzer.ReportItems.Functions.Global;
//using DB_Analyzer.ReportItems.Functions.Scalar;
//using DB_Analyzer.ReportItems.Functions.TableValued;
//using DB_Analyzer.ReportItems.StoredProcedures;
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

#region main

//string connString = @"Server=127.0.0.1;port=3306;uid=root;Database=portal_db;";
//MySqlManager manager = new(connString);

string connString = @"Server=(localdb)\MSSQLLocalDB;Database=portal_db;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true";
SqlServerManager manager = new(connString);


await manager.ConnectToDBAsync();


List<ReportItem> reportItems = manager.GetAllPossibleReportItems();


await manager.Analyze(reportItems);


//ReportSaver reportSaver = new MySqlReportSaver(@"Server=127.0.0.1;port=3306;uid=root;Database=test", manager.Connection);
//ReportSaver reportSaver = new SqlServerReportSaver(@"Server = (localdb)\MSSQLLocalDB; Database = s; Trusted_Connection = True; Encrypt = False", manager.Connection);
ReportSaver reportSaver = new TxtFileReportSaver(manager.Connection);

await manager.SaveReport(reportSaver, reportItems);

#endregion

#region test
//foreach (var reportItem in reportItems)
//{
//    if (reportItem is IReportItem<int>)
//        Console.WriteLine(reportItem.GetValue<int>());
//    else if (reportItem is IReportItem<List<string>>)
//    {
//        List<string> s = reportItem.GetValue<List<string>>();

//        s.ForEach(s => Console.WriteLine(s));
//    }
//    else if (reportItem is IReportItem<DataTable>)
//    {
//        DataTable dt = reportItem.GetValue<DataTable>();

//        foreach (DataRow dr in dt.Rows)
//        {
//            for (int i = 0; i < dt.Columns.Count; i++)
//            {
//                Console.Write(dr[i]);
//            }

//            Console.WriteLine();
//        }
//    }
//}
#endregion