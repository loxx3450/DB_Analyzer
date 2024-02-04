using DB_Analyzer.Analyzers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportItems
{
    public class NumberOfTablesReportItem : IReportItem<ScalarValue<int>>
    {
        public ScalarValue<int> Value { get; private set; }

        public async Task Run(DbAnalyzer analyzer)
        {
            Value = await analyzer.GetNumberOfTables();
        }
    }
}
