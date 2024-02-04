using DB_Analyzer.Analyzers;
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
    public class MySqlManager : Manager
    {
        public MySqlManager(string connectionString)
            : base(connectionString, new MySqlConnection(connectionString))
        { }

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
            Analyzer = new MySqlServerAnalyzer((MySqlConnection)Connection);

            foreach (var reportItem in reportItems)
                await reportItem.Run(Analyzer);
        }
    }
}
