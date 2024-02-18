using DB_Analyzer.ReportItems;
using DB_Analyzer.ReportSavers.TypesConvertors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers.IStructureProviders
{
    internal interface IStructureProvider
    {
        Task ProvideStructure(List<IReportItem<object>> reportItems);
    }
}
