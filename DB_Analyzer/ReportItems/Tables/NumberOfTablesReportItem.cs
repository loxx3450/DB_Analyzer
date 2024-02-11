using DB_Analyzer.Analyzers;
using DB_Analyzer.ReportItems.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportItems.Tables
{
    public class NumberOfTablesReportItem : ReportItem<ScalarValue<int>>, ISqlServerReportItem, IMySqlReportItem
    {
        public override string Name { get; } = "numberOfTables";
        public override ScalarValue<int> Value { get; protected set; }

        public async override Task Run(DbAnalyzer analyzer)
        {
            Value = await analyzer.GetNumberOfTables();
        }
    }
}
