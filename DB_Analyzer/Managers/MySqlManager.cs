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

        public override async Task Analyze(List<ReportItem> reportItems)
        {
            Analyzer = new MySqlAnalyzer((MySqlConnection)Connection);

            await Parallel.ForEachAsync(reportItems, async (item, state) =>
            {
                await item.Run(Analyzer);
            });

            //Analyzer = new MySqlAnalyzer((MySqlConnection)Connection);

            //Exception? exception = null;

            //var cts = new CancellationTokenSource();

            //CancellationToken token = cts.Token;

            //await Parallel.ForEachAsync(reportItems, async (item, state) =>
            //{
            //    token.ThrowIfCancellationRequested();

            //    try
            //    {
            //        await item.Run(Analyzer);
            //    }
            //    catch (Exception ex)
            //    {
            //        lock (locker)
            //        {
            //            exception = ex;

            //            cts.Cancel();
            //        }
            //    }
            //});

            //if (exception is not null)
            //    throw exception;
        }

        public override List<ReportItem> GetAllPossibleReportItems()
        {
            return ReportItemsListCreator.GetAllPossibleReportItems<IMySqlReportItem>();
        }
    }
}
