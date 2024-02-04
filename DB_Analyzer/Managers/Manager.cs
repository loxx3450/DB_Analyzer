using DB_Analyzer.Analyzers;
using DB_Analyzer.ReportItems;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.Managers
{
    public abstract class Manager
    {
        public DbConnection Connection { get; protected set; }
        public string ConnectionString { get; protected set; }
        public DbAnalyzer Analyzer { get; protected set; }
        public Manager(string connectionString, DbConnection connection) 
        { 
            ConnectionString = connectionString;

            Connection = connection;
        }

        public abstract Task ConnectToDBAsync();
        public abstract Task Analyze(List<IReportItem<object>> reportItems);
    }
}
