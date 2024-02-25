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

namespace DB_Analyzer.ReportSavers.Supporting_Classes.DataInserters.TxtFileDataInserter
{
    internal class TxtFileDataInserter : DataInserter
    {
        public TxtFileDataInserter(DbConnection analyzedDbConnection)
            : base(analyzedDbConnection)
        { }

        public async Task InsertData(List<ReportItem> reportItems, Report report)
        {
            FillReportWithData(reportItems, report);

            string result = JsonConvert.SerializeObject(report);

            using (StreamWriter writer = new StreamWriter("./report.TXT", true))
            {
                await writer.WriteAsync(result);

                await writer.WriteAsync("\n\n\n\n\n\n\n");
            }

            Report rep = JsonConvert.DeserializeObject<Report>(result);
        }

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
                    report.CollectionOfDataTables.Add((DataTable)reportItem.Value);
                }
            }
        }

        private void AddScalarValueToReport(ReportItem reportItem, Report report)
        {
            switch (TypesHandler.TypesHandler.GetScalarValueTypeDescription(reportItem))
            {
                case "int32":
                    report.CollectionOfIntegers.Add((int)reportItem.Value);
                    break;
            }
        }

        private void AddReferenceValueToReport(ReportItem reportItem, Report report)
        {
            switch (TypesHandler.TypesHandler.GetReferenceValueTypeDescription(reportItem))
            {
                case "List<string>":
                    report.CollectionOfStringLists.Add((List<string>)reportItem.Value);
                    break;
            }
        }
    }
}
