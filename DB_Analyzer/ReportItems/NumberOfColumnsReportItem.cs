using DB_Analyzer.Analyzers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportItems
{
    public class NumberOfColumnsReportItem : IReportItem<ScalarValue<int>>
    {
        public ScalarValue<int> Value { get; private set; }
        public string TableName { get; set; }

        public NumberOfColumnsReportItem(string tableName)
        {
            TableName = tableName;
        }

        public async Task Run(DbAnalyzer analyzer)
        {
            Value = await analyzer.GetNumberOfColumns(TableName);
        }
    }
}
