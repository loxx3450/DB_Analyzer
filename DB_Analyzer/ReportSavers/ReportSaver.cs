using DB_Analyzer.ReportItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers
{
    public abstract class ReportSaver
    {
        public abstract Task SaveReport(List<IReportItem<object>> reportItems);
    }
}
