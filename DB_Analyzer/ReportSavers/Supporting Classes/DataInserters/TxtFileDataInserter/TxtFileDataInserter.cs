using DB_Analyzer.ReportItems;
using DB_Analyzer.ReportSavers.DataInserters;
using DB_Analyzer.ReportSavers.Supporting_Classes.StructureProviders.TxtFileStructureProvider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data.Common;
using DB_Analyzer.Exceptions.ReportSaverExceptions;

namespace DB_Analyzer.ReportSavers.Supporting_Classes.DataInserters.TxtFileDataInserter
{
    internal class TxtFileDataInserter : DataInserter
    {
        public TxtFileDataInserter(DbConnection analyzedDbConnection)
            : base(analyzedDbConnection)
        { }

        //Report will be filled and saved
        public async Task InsertData(List<ReportItem> reportItems, Report report)
        {
            FillReportWithData(reportItems, report);

            string result = JsonConvert.SerializeObject(report);

            using (StreamWriter writer = new StreamWriter("./report.TXT", true))
            {
                await writer.WriteAsync(result);

                await writer.WriteAsync("\n\n\n\n\n\n\n");
            }
        }

        //Adds values of ReportItems in Report
        private void FillReportWithData(List<ReportItem> reportItems, Report report)
        {
            report.DbmsName = GetDbmsName();

            report.ServerName = AnalyzedDbConnection.DataSource;

            report.DbName = AnalyzedDbConnection.Database;

            report.CreationDate = DateTime.Now;

            foreach (var reportItem in reportItems)
            {
                if (TypesHandler.TypesHandler.IsScalarValueType(reportItem))
                {
                    AddScalarValueToReport(reportItem, report);
                }
                else if (TypesHandler.TypesHandler.IsReferenceValueType(reportItem))
                {
                    AddReferenceValueToReport(reportItem, report);
                }
                else if (TypesHandler.TypesHandler.IsDataTableValueType(reportItem))
                {
                    report.CollectionOfDataTables.Add(reportItem.Name, (DataTable)reportItem.Value);
                }
            }
        }

        private void AddScalarValueToReport(ReportItem reportItem, Report report)
        {
            switch (TypesHandler.TypesHandler.GetScalarValueTypeDescription(reportItem))
            {
                case "int32":
                    report.CollectionOfIntegers.Add(reportItem.Name, (int)reportItem.Value);
                    break;
                default:
                    throw new TxtFileReportSaverException(TxtFileReportSaverException.tryToSaveUnknownType);
            }
        }

        private void AddReferenceValueToReport(ReportItem reportItem, Report report)
        {
            switch (TypesHandler.TypesHandler.GetReferenceValueTypeDescription(reportItem))
            {
                case "List<string>":
                    report.CollectionOfStringLists.Add(reportItem.Name, (List<string>)reportItem.Value);
                    break;
                default:
                    throw new TxtFileReportSaverException(TxtFileReportSaverException.tryToSaveUnknownType);
            }
        }
    }
}
