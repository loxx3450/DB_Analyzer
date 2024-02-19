using DB_Analyzer.Analyzers;
using DB_Analyzer.Exceptions.Global;
using DB_Analyzer.Helpers;
using DB_Analyzer.Helpers.ReportItemsListsCreator;
using DB_Analyzer.ReportItems;
using DB_Analyzer.ReportItems.Flags;
using DB_Analyzer.ReportSavers;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.Managers
{
    public class MySqlManager : Manager
    {
        public MySqlManager(string connectionString)
            : base(connectionString, new MySqlConnection(connectionString))
        { }

        public async override Task Analyze(List<IReportItem<object>> reportItems)
        {
            Analyzer = new MySqlAnalyzer((MySqlConnection)Connection);

            //foreach (var reportItem in reportItems)
            //    await reportItem.Run(Analyzer);

            await Parallel.ForEachAsync(reportItems, async (item, state) =>
            {
                await item.Run(Analyzer);
            });
        }

        public override List<IReportItem<object>> GetAllPossibleReportItems()
        {
            return ReportItemsListCreator.GetAllPossibleReportItems<IMySqlReportItem>();
        }
    }
}
