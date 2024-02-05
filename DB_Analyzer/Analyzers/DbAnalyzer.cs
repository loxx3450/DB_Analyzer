using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.Analyzers
{
    public abstract class DbAnalyzer
    {
        public DbConnection Connection { get; set; }

        public DbAnalyzer(DbConnection connection) 
        { 
            Connection = connection;
        }

        public abstract Task<int> GetNumberOfTables();

        public abstract Task<List<string>> GetTablesNames();

        public abstract Task<DataTable> GetTablesFullInfo();

        public abstract Task<int> GetNumberOfColumns(string tableName);

        public abstract Task<List<string>> GetColumnsNames(string tableName);

        public abstract Task<DataTable> GetColumnsFullInfo(string tableName);

        public abstract Task<int> GetNumberOfStoredProcedures();

        public abstract Task<List<string>> GetStoredProceduresNames();

        public abstract Task<DataTable> GetStoredProceduresFullInfo();
    }
}
