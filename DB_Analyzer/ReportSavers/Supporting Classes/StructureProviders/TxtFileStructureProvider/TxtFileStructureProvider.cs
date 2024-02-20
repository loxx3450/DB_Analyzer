using DB_Analyzer.ReportItems;
using DB_Analyzer.ReportSavers.StructureProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers.Supporting_Classes.StructureProviders.TxtFileStructureProvider
{
    internal class TxtFileStructureProvider : IStructureProvider
    {
        public Report ProvideStructure()
        {
            return new Report();
        }
    }
}
