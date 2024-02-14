using DB_Analyzer.ReportItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers.StructureProviders
{
    internal abstract class StructureProvider
    {
        public abstract Task ProvideStructure(List<IReportItem<object>> reportItems);
    }
}
