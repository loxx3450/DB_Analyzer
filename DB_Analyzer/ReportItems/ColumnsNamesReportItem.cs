using DB_Analyzer.Analyzers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportItems
{
    public class ColumnsNamesReportItem : IReportItem<List<string>>
    {
        public List<string> Value { get; private set; }
        public string TableName { get; set; }

        public ColumnsNamesReportItem(string tableName)
        {
            TableName = tableName;
        }

        public async Task Run(DbAnalyzer analyzer)
        {
            Value = await analyzer.GetColumnsNames(TableName);
        }
    }
}
