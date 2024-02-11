using DB_Analyzer.Analyzers;
using DB_Analyzer.ReportItems.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportItems.StoredProcedures
{
    public class StoredProceduresNamesReportItem : ReportItem<List<string>>, ISqlServerReportItem, IMySqlReportItem
    {
        public override string Name { get; } = "storedProceduresNames";
        public override List<string> Value { get; protected set; }

        public async override Task Run(DbAnalyzer analyzer)
        {
            Value = await analyzer.GetStoredProceduresNames();
        }
    }
}
