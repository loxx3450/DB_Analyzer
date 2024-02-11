using DB_Analyzer.Analyzers;
using DB_Analyzer.ReportItems.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportItems
{
    //temp
    public class GetBoolReportItem : ReportItem<ScalarValue<bool>>, ISqlServerReportItem, IMySqlReportItem
    {
        public override string Name { get; } = "GetBool";

        public override ScalarValue<bool> Value { get; protected set; }

        public async override Task Run(DbAnalyzer analyzer)
        {
            Value = await analyzer.GetBool();
        }
    }
    //temp
}
