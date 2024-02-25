using DB_Analyzer.Analyzers;
using DB_Analyzer.ReportItems.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportItems.Tables
{
    public class NumberOfTablesReportItem : ReportItem, IReportItem<int>, ISqlServerReportItem, IMySqlReportItem
    {
        public override string Name { get; } = "number_of_tables";

        public override async Task Run(DbAnalyzer analyzer)
        {
            Value = await analyzer.GetNumberOfTables();
        }
    }
}
