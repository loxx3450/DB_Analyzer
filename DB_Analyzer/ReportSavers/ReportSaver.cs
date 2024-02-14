using DB_Analyzer.ReportItems;
using DB_Analyzer.ReportSavers.StructureProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers
{
    public abstract class ReportSaver
    {
        internal StructureProvider StructureProvider { get; set; }
        public abstract Task SaveReport(List<IReportItem<object>> reportItems);
    }
}
