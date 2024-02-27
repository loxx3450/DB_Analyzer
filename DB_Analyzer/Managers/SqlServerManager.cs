using DB_Analyzer.Analyzers;
using DB_Analyzer.Exceptions.Global;
using DB_Analyzer.Helpers.ReportItemsListsCreator;
using DB_Analyzer.ReportItems;
using DB_Analyzer.ReportItems.Flags;
using DB_Analyzer.ReportSavers;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.Managers
{
    public class SqlServerManager : Manager
    {
        public SqlServerManager(string connectionString)
            : base(connectionString, new SqlConnection(connectionString))
        { }

        public override Task Analyze(List<ReportItem> reportItems)
        {
            Analyzer = new SqlServerAnalyzer((SqlConnection)Connection);

            return Parallel.ForEachAsync(reportItems, async (item, state) =>
            {
                await item.Run(Analyzer);
            });
        }

        public override async Task ChangeConnectionString(string connectionString)
        {
            await CloseConnectionAsync();

            ConnectionString = connectionString;

            Connection = new SqlConnection(ConnectionString);
        }

        public override List<ReportItem> GetAllPossibleReportItems()
        {
            return ReportItemsListCreator.GetAllPossibleReportItems<ISqlServerReportItem>();
        }
    }
}
