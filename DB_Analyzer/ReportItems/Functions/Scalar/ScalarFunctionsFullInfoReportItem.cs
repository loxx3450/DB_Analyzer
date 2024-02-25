using DB_Analyzer.Analyzers;
using DB_Analyzer.ReportItems.Flags;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportItems.Functions.Scalar
{
    public class ScalarFunctionsFullInfoReportItem : ReportItem, IReportItem<DataTable>, ISqlServerReportItem
    {
        public override string Name { get; } = "scalar_functions_full_info";

        public async override Task Run(DbAnalyzer analyzer)
        {
            Value = await analyzer.GetScalarFunctionsFullInfo();
        }
    }
}
