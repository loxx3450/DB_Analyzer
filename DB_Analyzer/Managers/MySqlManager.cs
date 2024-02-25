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
        static object locker = new object();

        public MySqlManager(string connectionString)
            : base(connectionString, new MySqlConnection(connectionString))
        { }

        public override Task Analyze(List<ReportItem> reportItems)
        {
            Analyzer = new MySqlAnalyzer((MySqlConnection)Connection);

            return Parallel.ForEachAsync(reportItems, async (item, state) =>
            {
                await item.Run(Analyzer);
            });
        }

        public override List<ReportItem> GetAllPossibleReportItems()
        {
            return ReportItemsListCreator.GetAllPossibleReportItems<IMySqlReportItem>();
        }
    }
}
