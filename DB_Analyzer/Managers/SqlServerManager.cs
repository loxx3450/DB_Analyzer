using DB_Analyzer.Analyzers;
using DB_Analyzer.Helpers.ReportItemsListsCreators;
using DB_Analyzer.ReportItems;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.Managers
{
    public class SqlServerManager : Manager
    {
        public SqlServerManager(string connectionString)
            : base(connectionString, new SqlConnection(connectionString))
        {
            ReportItemsListCreator = new SqlServerReportItemsListCreator();

        }

        public async override Task ConnectToDBAsync()
        {
            try
            {
                await Connection.OpenAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async override Task Analyze(List<IReportItem<object>> reportItems)
        {
            Analyzer = new SqlServerAnalyzer((SqlConnection)Connection);

            foreach (var reportItem in reportItems)
                await reportItem.Run(Analyzer);
        }

        public override List<IReportItem<object>> GetAllPossibleReportItems()
        {
            return ReportItemsListCreator.GetAllPossibleReportItems();
        }
    }
}
