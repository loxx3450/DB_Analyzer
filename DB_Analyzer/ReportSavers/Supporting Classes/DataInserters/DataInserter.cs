using DB_Analyzer.ReportItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB_Analyzer.ReportSavers.DataConvertors;
using System.Data.Common;
using MySql.Data.MySqlClient;
using Microsoft.Data.SqlClient;
using DB_Analyzer.Exceptions.ReportSaverExceptions;

namespace DB_Analyzer.ReportSavers.DataInserters
{
    internal abstract class DataInserter
    {
        protected DbConnection AnalyzedDbConnection { get; set; }
        protected DataConvertor? DataConvertor { get; set; } = null;

        public DataInserter(DbConnection analyzedDbConnection)
        {
            AnalyzedDbConnection = analyzedDbConnection;
        }

        protected string GetDbmsName()
        {
            if (AnalyzedDbConnection is MySqlConnection)
                return "MySql";
            if (AnalyzedDbConnection is SqlConnection)
                return "SqlServer";

            throw new ReportSaverException("Unknown DBMS...");
        }
    }
}
