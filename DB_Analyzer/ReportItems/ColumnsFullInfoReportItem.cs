using DB_Analyzer.Analyzers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportItems
{
    public class ColumnsFullInfoReportItem : IReportItem<DataTable>
    {
        public DataTable Value { get; private set; }
        public string TableName { get; set; }

        public ColumnsFullInfoReportItem(string tableName)
        {
            TableName = tableName;
        }

        public async Task Run(DbAnalyzer analyzer)
        {
            Value = await analyzer.GetColumnsFullInfo(TableName);
        }
    }
}
