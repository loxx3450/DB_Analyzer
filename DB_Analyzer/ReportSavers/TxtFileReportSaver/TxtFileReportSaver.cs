using DB_Analyzer.ReportItems;
using DB_Analyzer.ReportSavers.Supporting_Classes.DataInserters;
using DB_Analyzer.ReportSavers.Supporting_Classes.DataInserters.TxtFileDataInserter;
using DB_Analyzer.ReportSavers.Supporting_Classes.StructureProviders.TxtFileStructureProvider;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers.TxtFileReportSaver
{
    public class TxtFileReportSaver : ReportSaver
    {
        public TxtFileReportSaver(DbConnection analyzedDbConnection)
            : base(analyzedDbConnection)
        {
            StructureProvider = new TxtFileStructureProvider();

            DataInserter = new TxtFileDataInserter(AnalyzedDbConnection);
        }

        public override Task SaveReport(List<ReportItem> reportItems)
        {
            Report structure = ((TxtFileStructureProvider)StructureProvider).ProvideStructure();

            return ((TxtFileDataInserter)DataInserter).InsertData(reportItems, structure);
        }
    }
}
