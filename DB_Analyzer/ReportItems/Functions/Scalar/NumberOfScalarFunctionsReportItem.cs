using DB_Analyzer.Analyzers;
using DB_Analyzer.ReportItems.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportItems.Functions.Scalar
{
    public class NumberOfScalarFunctionsReportItem : ReportItem, IReportItem<int>, ISqlServerReportItem
    {
        public override string Name { get; } = "number_of_scalar_functions";

        public async override Task Run(DbAnalyzer analyzer)
        {
            Value = await analyzer.GetNumberOfScalarFunctions();
        }
    }
}
