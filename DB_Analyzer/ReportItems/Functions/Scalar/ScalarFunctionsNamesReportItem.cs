using DB_Analyzer.Analyzers;
using DB_Analyzer.ReportItems.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportItems.Functions.Scalar
{
    public class ScalarFunctionsNamesReportItem : ReportItem<List<string>>, ISqlServerReportItem
    {
        public override string Name { get; } = "scalarFunctionsNames";
        public override List<string> Value { get; protected set; }

        public async override Task Run(DbAnalyzer analyzer)
        {
            Value = await analyzer.GetScalarFunctionsNames();
        }
    }
}
