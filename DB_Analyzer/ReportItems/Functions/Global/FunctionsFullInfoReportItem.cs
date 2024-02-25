using DB_Analyzer.Analyzers;
using DB_Analyzer.ReportItems.Flags;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportItems.Functions.Global
{
    public class FunctionsFullInfoReportItem : ReportItem, IReportItem<DataTable>, ISqlServerReportItem, IMySqlReportItem
    {
        public override string Name { get; } = "functions_full_info";

        public async override Task Run(DbAnalyzer analyzer)
        {
            Value = await analyzer.GetFunctionsFullInfo();
        }
    }
}
