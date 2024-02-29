using DB_Analyzer.Exceptions;
using DB_Analyzer.Exceptions.AnalyzerExceptions;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.Analyzers
{
    internal class SqlServerAnalyzer : DbAnalyzer
    {
        public SqlServerAnalyzer(SqlConnection connection)
            : base(connection)
        { }

        //Methods to get some info
        public override Task<int> GetNumberOfTables()
        {
            return GetIntValueAsync("SELECT COUNT(*) " +
                "FROM SYS.TABLES");
        }

        public override Task<List<string>> GetTablesNames()
        {
            return GetListOfStringValuesAsync("SELECT TABLE_NAME " +
                "FROM INFORMATION_SCHEMA.TABLES " +
                "WHERE TABLE_TYPE = 'BASE TABLE'", "TABLE_NAME");
        }

        public override Task<DataTable> GetTablesFullInfo()
        {
            return GetTableOfValuesAsync("SELECT name as name, type_desc as type, create_date as creation_date " +
                "FROM SYS.TABLES");
        }

        public override Task<int> GetNumberOfStoredProcedures()
        {
            return GetIntValueAsync("SELECT COUNT(*) " +
                "FROM SYS.PROCEDURES");
        }

        public override Task<List<string>> GetStoredProceduresNames()
        {
            return GetListOfStringValuesAsync("SELECT NAME " +
                "FROM SYS.PROCEDURES", "NAME");
        }

        public override Task<DataTable> GetStoredProceduresFullInfo()
        {
            return GetTableOfValuesAsync("SELECT name as name, type_desc as type, create_date as creation_date " +
                "FROM SYS.PROCEDURES");
        }

        public override Task<int> GetNumberOfScalarFunctions()
        {
            return GetIntValueAsync("SELECT COUNT(*)  " +
                "FROM SYS.SQL_MODULES M " +
                "INNER JOIN SYS.OBJECTS OBJ " +
                "   ON M.OBJECT_ID=OBJ.OBJECT_ID " +
                "WHERE TYPE_DESC LIKE '%scalar_function%'");
        }

        public override Task<List<string>> GetScalarFunctionsNames()
        {
            return GetListOfStringValuesAsync("SELECT NAME  " +
                "FROM SYS.SQL_MODULES M " +
                "INNER JOIN SYS.OBJECTS OBJ " +
                "   ON M.OBJECT_ID=OBJ.OBJECT_ID " +
                "WHERE TYPE_DESC LIKE '%scalar_function%'", "NAME");
        }

        public override Task<DataTable> GetScalarFunctionsFullInfo()
        {
            return GetTableOfValuesAsync("SELECT name as name, type_desc as type, create_date as creation_date " +
                "FROM SYS.SQL_MODULES M " +
                "INNER JOIN SYS.OBJECTS OBJ " +
                "   ON M.OBJECT_ID=OBJ.OBJECT_ID " +
                "WHERE TYPE_DESC LIKE '%scalar_function%'");
        }

        public override Task<int> GetNumberOfTableValuedFunctions()
        {
            return GetIntValueAsync("SELECT COUNT(*)  " +
                "FROM SYS.SQL_MODULES M " +
                "INNER JOIN SYS.OBJECTS OBJ " +
                "   ON M.OBJECT_ID=OBJ.OBJECT_ID " +
                "WHERE TYPE_DESC LIKE '%table_valued_function%'");
        }

        public override Task<List<string>> GetTableValuedFunctionsNames()
        {
            return GetListOfStringValuesAsync("SELECT NAME  " +
                "FROM SYS.SQL_MODULES M " +
                "INNER JOIN SYS.OBJECTS OBJ " +
                "   ON M.OBJECT_ID=OBJ.OBJECT_ID " +
                "WHERE TYPE_DESC LIKE '%table_valued_function%'", "NAME");
        }

        public override Task<DataTable> GetTableValuedFunctionsFullInfo()
        {
            return GetTableOfValuesAsync("SELECT name as name, type_desc as type, create_date as creation_date " +
                "FROM SYS.SQL_MODULES M " +
                "INNER JOIN SYS.OBJECTS OBJ " +
                "   ON M.OBJECT_ID=OBJ.OBJECT_ID " +
                "WHERE TYPE_DESC LIKE '%table_valued_function%'");
        }

        public override Task<int> GetNumberOfFunctions()
        {
            return GetIntValueAsync("SELECT COUNT(*)  " +
                "FROM SYS.SQL_MODULES M " +
                "INNER JOIN SYS.OBJECTS OBJ " +
                "   ON M.OBJECT_ID=OBJ.OBJECT_ID " +
                "WHERE TYPE_DESC LIKE '%function%'");
        }

        public override Task<List<string>> GetFunctionsNames()
        {
            return GetListOfStringValuesAsync("SELECT NAME  " +
                "FROM SYS.SQL_MODULES M " +
                "INNER JOIN SYS.OBJECTS OBJ " +
                "   ON M.OBJECT_ID=OBJ.OBJECT_ID " +
                "WHERE TYPE_DESC LIKE '%function%'", "NAME");
        }

        public override Task<DataTable> GetFunctionsFullInfo()
        {
            return GetTableOfValuesAsync("SELECT name as name, type_desc as type, create_date as creation_date " +
                "FROM SYS.SQL_MODULES M " +
                "INNER JOIN SYS.OBJECTS OBJ " +
                "   ON M.OBJECT_ID=OBJ.OBJECT_ID " +
                "WHERE TYPE_DESC LIKE '%function%'");
        }

        //Routine Methods to execute queries in different ways
        protected override async Task<int> GetIntValueAsync(string query)
        {
            SqlCommand command = new SqlCommand(query, (SqlConnection)Connection);

            try
            {
                return Convert.ToInt32(await command.ExecuteScalarAsync());
            }
            catch (Exception ex)
            {
                throw new SqlServerAnalyzerException(AnalyzerException.problemDuringHandling + ex.Message, ex);
            }
            finally
            {
                await command.DisposeAsync();
            }
        }

        protected override async Task<List<string>> GetListOfStringValuesAsync(string query, string column)
        {
            List<string> result = new List<string>();

            SqlCommand command = new SqlCommand(query, (SqlConnection)Connection);

            try
            {
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                {
                    while (reader.Read())
                    {
                        result.Add(reader.GetFieldValue<string>(column));
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new SqlServerAnalyzerException(AnalyzerException.problemDuringHandling + ex.Message, ex);
            }
            finally
            {
                await command.DisposeAsync();
            }
        }

        protected override async Task<DataTable> GetTableOfValuesAsync(string query)
        {
            SqlCommand command = new SqlCommand(query, (SqlConnection)Connection);

            try
            {
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                {
                    DataTable dataTable = new DataTable();

                    dataTable.Load(reader);

                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                throw new SqlServerAnalyzerException(AnalyzerException.problemDuringHandling + ex.Message, ex);
            }
            finally
            {
                await command.DisposeAsync();
            }
        }
    }
}
