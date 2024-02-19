using DB_Analyzer.ReportItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers
{
    internal class TxtFileReportSaver : ReportSaver
    {
        public override async Task SaveReport(List<IReportItem<object>> reportItems)
        {
            //StructureProvider.ProvideStructure();

            //DataInserter.InsertData(reportItems);
        }
    }
}
