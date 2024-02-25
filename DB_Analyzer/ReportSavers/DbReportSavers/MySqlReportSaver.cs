using DB_Analyzer.ReportItems;
using DB_Analyzer.ReportSavers.DataInserters.DbDataInserters;
using DB_Analyzer.ReportSavers.StructureProviders.DbStructureProviders;
using DB_Analyzer.ReportSavers.Supporting_Classes.DataInserters.DbDataInserters;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers.DbReportSavers
{
    public class MySqlReportSaver : DbReportSaver
    {
        public MySqlReportSaver(string connectionString, DbConnection analyzedDbConnection)
            : base(new MySqlConnection(connectionString), analyzedDbConnection)
        {
            StructureProvider = new MySqlStructureProvider((MySqlConnection)Connection);

            DataInserter = new MySqlDataInserter((MySqlConnection)Connection, AnalyzedDbConnection);
        }
    }
}
