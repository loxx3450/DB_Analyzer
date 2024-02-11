using Microsoft.Data.SqlClient;
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

        public abstract Task<int> GetNumberOfStoredProcedures();

        public abstract Task<List<string>> GetStoredProceduresNames();

        public abstract Task<DataTable> GetStoredProceduresFullInfo();

        public abstract Task<int> GetNumberOfScalarFunctions();

        public abstract Task<List<string>> GetScalarFunctionsNames();

        public abstract Task<DataTable> GetScalarFunctionsFullInfo();

        public abstract Task<int> GetNumberOfTableValuedFunctions();

        public abstract Task<List<string>> GetTableValuedFunctionsNames();

        public abstract Task<DataTable> GetTableValuedFunctionsFullInfo();

        public abstract Task<int> GetNumberOfFunctions();

        public abstract Task<List<string>> GetFunctionsNames();

        public abstract Task<DataTable> GetFunctionsFullInfo();

        //temp
        public abstract Task<bool> GetBool();
        //temp
    }
}
