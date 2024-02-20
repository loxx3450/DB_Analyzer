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

namespace DB_Analyzer.ReportSavers.Supporting_Classes.DataInserters.TxtFileDataInserter
{
    internal class TxtFileDataInserter : DataInserter
    {
        public TxtFileDataInserter(string analyzedDB_Name) 
            : base(analyzedDB_Name)
        { }

        public async Task InsertData(List<IReportItem<object>> reportItems, Report report)
        {
            FillReportWithData(reportItems, report);

            string result = JsonSerializer.Serialize(report, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                Converters = { new JsonSerializerTypeConverter.JsonSerializerTypeConverter() }
            });

            using (StreamWriter writer = new StreamWriter("./report.TXT", true))
            {
                await writer.WriteAsync(result);

                await writer.WriteAsync("\n\n\n\n\n\n\n\n\n\n");
            }

            //Report? news = JsonSerializer.Deserialize<Report>(result, new JsonSerializerOptions
            //{
            //    //ReferenceHandler = ReferenceHandler.Preserve,
            //    Converters = { new TypeConverter() }
            //});

            //Console.Read();
        }

        private void FillReportWithData(List<IReportItem<object>> reportItems, Report report)
        {
            Type reportItemType;

            report.DbName = AnalyzedDB_Name;

            report.CreationDate = DateTime.Now;

            foreach (var reportItem in reportItems)
            {
                reportItemType = reportItem.GetValueType();

                if (TypesHandler.TypesHandler.IsScalarValueType(reportItemType))
                {
                    AddScalarValueToReport(report, reportItem, reportItemType);
                }
                else if (TypesHandler.TypesHandler.IsReferenceValueType(reportItemType))
                {
                    AddReferenceValueToReport(report, reportItem, reportItemType);
                }
                else if (TypesHandler.TypesHandler.IsDataTableValueType(reportItemType))
                {
                    report.CollectionOfDataTables.Add((DataTable)reportItem.Value);
                }
            }
        }

        private void AddScalarValueToReport(Report report, IReportItem<object> reportItem, Type reportItemType)
        {
            switch (TypesHandler.TypesHandler.GetScalarValueType(reportItemType))
            {
                case "int32":
                    report.CollectionOfIntegers.Add(Convert.ToInt32(reportItem.Value.ToString()));
                    break;
            }
        }

        private void AddReferenceValueToReport(Report report, IReportItem<object> reportItem, Type reportItemType)
        {
            switch (TypesHandler.TypesHandler.GetReferenceValueType(reportItemType))
            {
                case "List<string>":
                    report.CollectionOfStringLists.Add((List<string>)reportItem.Value);
                    break;
            }
        }
    }
}
