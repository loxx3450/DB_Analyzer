using DB_Analyzer.Analyzers;
using DB_Analyzer.ReportItems.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportItems.Functions.TableValued
{
    public class TableValuedFunctionsNamesReportItem : IReportItem<List<string>>, ISqlServerReportItem
    {
        public List<string> Value { get; private set; }

        public async Task Run(DbAnalyzer analyzer)
        {
            Value = await analyzer.GetTableValuedFunctionsNames();
        }
    }
}
