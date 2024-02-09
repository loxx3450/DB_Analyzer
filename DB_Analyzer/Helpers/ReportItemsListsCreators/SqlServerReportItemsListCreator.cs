using DB_Analyzer.ReportItems;
using DB_Analyzer.ReportItems.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.Helpers.ReportItemsListsCreators
{
    internal class SqlServerReportItemsListCreator : ReportItemsListCreator
    {
        public override List<IReportItem<object>> GetAllPossibleReportItems()
        {
            List<IReportItem<object>> reportItems = new List<IReportItem<object>>();

            foreach (var item in allReportItems)
            {
                if (item is ISqlServerReportItem)
                {
                    reportItems.Add(item);
                }
            }

            return reportItems;
        }
    }
}
