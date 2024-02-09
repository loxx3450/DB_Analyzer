using DB_Analyzer.Analyzers;
using DB_Analyzer.ReportItems.Flags;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportItems.StoredProcedures
{
    public class StoredProceduresFullInfoReportItem : IReportItem<DataTable>, ISqlServerReportItem, IMySqlReportItem
    {
        public DataTable Value { get; private set; }

        public async Task Run(DbAnalyzer analyzer)
        {
            Value = await analyzer.GetStoredProceduresFullInfo();
        }
    }
}
