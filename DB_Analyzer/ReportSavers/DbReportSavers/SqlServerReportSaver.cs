using DB_Analyzer.ReportItems;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB_Analyzer.ReportSavers.TypesHandler;
using DB_Analyzer.Exceptions.ReportSaverExceptions;
using DB_Analyzer.Exceptions.Global;
using System.Data;
using DB_Analyzer.ReportSavers.StructureProviders;
using DB_Analyzer.ReportSavers.DataInserters;
using DB_Analyzer.ReportSavers.DataInserters.DbDataInserters;
using DB_Analyzer.ReportSavers.StructureProviders.DbStructureProviders;
using System.Data.Common;

namespace DB_Analyzer.ReportSavers.DbReportSavers
{
    public class SqlServerReportSaver : DbReportSaver
    {
        public SqlServerReportSaver(string connectionString, DbConnection analyzedDbConnection)
            : base(new SqlConnection(connectionString), analyzedDbConnection)
        {
            StructureProvider = new SqlServerStructureProvider((SqlConnection)Connection);

            DataInserter = new SqlServerDataInserter((SqlConnection)Connection, AnalyzedDbConnection);
        }
    }
}
