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
    internal class MySqlAnalyzer : DbAnalyzer
    {
        public MySqlAnalyzer(MySqlConnection connection)
            : base(connection)
        { }

        public override Task<int> GetNumberOfTables()
        {
            return GetIntValueAsync($"SELECT COUNT(*) " +
                $"FROM INFORMATION_SCHEMA.TABLES " +
                $"WHERE TABLE_SCHEMA = '{ Connection.Database }' " +
                $"  AND TABLE_TYPE='BASE TABLE';");
        }

        public override Task<List<string>> GetTablesNames()
        {
            return GetListOfStringValuesAsync($"SELECT TABLE_NAME " +
                $"FROM INFORMATION_SCHEMA.TABLES " +
                $"WHERE TABLE_SCHEMA='{Connection.Database}' " +
                $"  AND TABLE_TYPE = 'BASE TABLE';", "TABLE_NAME");
        }

        public override Task<DataTable> GetTablesFullInfo()
        {
            return GetTableOfValuesAsync($"SELECT TABLE_NAME as name, TABLE_TYPE as type, CREATE_TIME as creation_date " +
                $"FROM INFORMATION_SCHEMA.TABLES " +
                $"WHERE TABLE_SCHEMA = '{Connection.Database}' " +
                $"  AND TABLE_TYPE='BASE TABLE';");
        }

        public override Task<int> GetNumberOfStoredProcedures()
        {
            return GetIntValueAsync($"SELECT COUNT(ROUTINE_NAME) " +
                $"FROM INFORMATION_SCHEMA.ROUTINES " +
                $"WHERE ROUTINE_TYPE=\"PROCEDURE\" " +
                $"  AND ROUTINE_SCHEMA=\"{ Connection.Database }\";");
        }

        public override Task<List<string>> GetStoredProceduresNames()
        {
            return GetListOfStringValuesAsync($"SELECT ROUTINE_NAME " +
                $"FROM INFORMATION_SCHEMA.ROUTINES " +
                $"WHERE ROUTINE_TYPE=\"PROCEDURE\" " +
                $"  AND ROUTINE_SCHEMA=\"{Connection.Database}\";", "ROUTINE_NAME");
        }

        public override Task<DataTable> GetStoredProceduresFullInfo()
        {
            return GetTableOfValuesAsync($"SELECT ROUTINE_NAME as name, ROUTINE_TYPE as type, CREATED as creation_date " +
                $"FROM INFORMATION_SCHEMA.ROUTINES " +
                $"WHERE ROUTINE_TYPE=\"PROCEDURE\" " +
                $"  AND ROUTINE_SCHEMA=\"{Connection.Database}\";");
        }

        public override Task<int> GetNumberOfScalarFunctions()
        {
            throw new MySqlAnalyzerException(MySqlAnalyzerException.methodIsNotSupported);
        }

        public override Task<List<string>> GetScalarFunctionsNames()
        {
            throw new MySqlAnalyzerException(MySqlAnalyzerException.methodIsNotSupported);
        }

        public override Task<DataTable> GetScalarFunctionsFullInfo()
        {
            throw new MySqlAnalyzerException(MySqlAnalyzerException.methodIsNotSupported);
        }

        public override Task<int> GetNumberOfTableValuedFunctions()
        {
            throw new MySqlAnalyzerException(MySqlAnalyzerException.methodIsNotSupported);
        }

        public override Task<List<string>> GetTableValuedFunctionsNames()
        {
            throw new MySqlAnalyzerException(MySqlAnalyzerException.methodIsNotSupported);
        }

        public override Task<DataTable> GetTableValuedFunctionsFullInfo()
        {
            throw new MySqlAnalyzerException(MySqlAnalyzerException.methodIsNotSupported);
        }

        public override Task<int> GetNumberOfFunctions()
        {
            return GetIntValueAsync($"SELECT COUNT(ROUTINE_NAME) " +
                $"FROM INFORMATION_SCHEMA.ROUTINES " +
                $"WHERE ROUTINE_TYPE=\"FUNCTION\" " +
                $"  AND ROUTINE_SCHEMA=\"{Connection.Database}\";");
        }

        public override Task<List<string>> GetFunctionsNames()
        {
            return GetListOfStringValuesAsync($"SELECT ROUTINE_NAME " +
                $"FROM INFORMATION_SCHEMA.ROUTINES " +
                $"WHERE ROUTINE_TYPE=\"FUNCTION\" " +
                $"  AND ROUTINE_SCHEMA=\"{Connection.Database}\";", "ROUTINE_NAME");
        }

        public override Task<DataTable> GetFunctionsFullInfo()
        {
            return GetTableOfValuesAsync($"SELECT ROUTINE_NAME as name, ROUTINE_TYPE as type, CREATED as creation_date " +
                $"FROM INFORMATION_SCHEMA.ROUTINES " +
                $"WHERE ROUTINE_TYPE=\"FUNCTION\" " +
                $"  AND ROUTINE_SCHEMA=\"{Connection.Database}\";");
        }

        protected override async Task<int> GetIntValueAsync(string query)
        {
            using (MySqlConnection connection = new MySqlConnection(Connection.ConnectionString))
            {
                await connection.OpenAsync();

                MySqlCommand command = new MySqlCommand(query, connection);

                try
                {
                    return Convert.ToInt32(await command.ExecuteScalarAsync());
                }
                catch (Exception ex)
                {
                    throw new MySqlAnalyzerException(AnalyzerException.problemDuringHandling + ex.Message, ex);
                }
                finally
                {
                    await connection.CloseAsync();

                    await command.DisposeAsync();
                }
            }
        }

        protected override async Task<List<string>> GetListOfStringValuesAsync(string query, string column)
        {
            using (MySqlConnection connection = new MySqlConnection(Connection.ConnectionString))
            {
                await connection.OpenAsync();

                List<string> result = new List<string>();

                MySqlCommand command = new MySqlCommand(query, connection);

                try
                {
                    using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();
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
                    throw new MySqlAnalyzerException(AnalyzerException.problemDuringHandling + ex.Message, ex);
                }
                finally
                {
                    await connection.CloseAsync();

                    await command.DisposeAsync();
                }
            }
        }

        protected override async Task<DataTable> GetTableOfValuesAsync(string query)
        {
            using (MySqlConnection connection = new MySqlConnection(Connection.ConnectionString))
            {
                await connection.OpenAsync();

                MySqlCommand command = new MySqlCommand(query, connection);

                try
                {
                    using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();
                    {
                        DataTable dataTable = new DataTable();

                        dataTable.Load(reader);

                        return dataTable;
                    }
                }
                catch (Exception ex)
                {
                    throw new MySqlAnalyzerException(AnalyzerException.problemDuringHandling + ex.Message, ex);
                }
                finally
                {
                    await connection.CloseAsync();

                    await command.DisposeAsync();
                }
            }
        }
    }
}
