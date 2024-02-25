using DB_Analyzer.ReportItems;
using DB_Analyzer.ReportSavers.Supporting_Classes.DataInserters;
using DB_Analyzer.ReportSavers.Supporting_Classes.DataInserters.TxtFileDataInserter;
using DB_Analyzer.ReportSavers.Supporting_Classes.StructureProviders.TxtFileStructureProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers.TxtFileReportSaver
{
    public class TxtFileReportSaver : ReportSaver
    {
        public TxtFileReportSaver(string analyzedDB_Name)
        {
            StructureProvider = new TxtFileStructureProvider();

            DataInserter = new TxtFileDataInserter(analyzedDB_Name);
        }

        public override async Task SaveReport(List<ReportItem> reportItems)
        {
            await ((TxtFileDataInserter)DataInserter).InsertData(reportItems, ((TxtFileStructureProvider)StructureProvider).ProvideStructure());
        }
    }
}
