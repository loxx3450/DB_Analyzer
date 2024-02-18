﻿using DB_Analyzer.Exceptions.AnalyzerExceptions;
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

        public async override Task<int> GetNumberOfTables()
        {
            return await GetIntValueAsync($"SELECT COUNT(*) " +
                $"FROM INFORMATION_SCHEMA.TABLES " +
                $"WHERE TABLE_SCHEMA = '{ Connection.Database }' " +
                $"  AND TABLE_TYPE='BASE TABLE';");
        }

        public async override Task<List<string>> GetTablesNames()
        {
            return await GetListOfStringValuesAsync($"SELECT TABLE_NAME " +
                $"FROM INFORMATION_SCHEMA.TABLES " +
                $"WHERE TABLE_SCHEMA='{Connection.Database}' " +
                $"  AND TABLE_TYPE = 'BASE TABLE';", "TABLE_NAME");
        }

        public async override Task<DataTable> GetTablesFullInfo()
        {
            return await GetTableOfValuesAsync($"SELECT * " +
                $"FROM INFORMATION_SCHEMA.TABLES " +
                $"WHERE TABLE_SCHEMA = '{Connection.Database}' " +
                $"  AND TABLE_TYPE='BASE TABLE';");
        }

        public async override Task<int> GetNumberOfStoredProcedures()
        {
            return await GetIntValueAsync($"SELECT COUNT(ROUTINE_NAME) " +
                $"FROM INFORMATION_SCHEMA.ROUTINES " +
                $"WHERE ROUTINE_TYPE=\"PROCEDURE\" " +
                $"  AND ROUTINE_SCHEMA=\"{ Connection.Database }\";");
        }

        public async override Task<List<string>> GetStoredProceduresNames()
        {
            return await GetListOfStringValuesAsync($"SELECT ROUTINE_NAME " +
                $"FROM INFORMATION_SCHEMA.ROUTINES " +
                $"WHERE ROUTINE_TYPE=\"PROCEDURE\" " +
                $"  AND ROUTINE_SCHEMA=\"{Connection.Database}\";", "ROUTINE_NAME");
        }

        public async override Task<DataTable> GetStoredProceduresFullInfo()
        {
            return await GetTableOfValuesAsync($"SELECT * " +
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

        public async override Task<int> GetNumberOfFunctions()
        {
            return await GetIntValueAsync($"SELECT COUNT(ROUTINE_NAME) " +
                $"FROM INFORMATION_SCHEMA.ROUTINES " +
                $"WHERE ROUTINE_TYPE=\"FUNCTION\" " +
                $"  AND ROUTINE_SCHEMA=\"{Connection.Database}\";");
        }

        public async override Task<List<string>> GetFunctionsNames()
        {
            return await GetListOfStringValuesAsync($"SELECT ROUTINE_NAME " +
                $"FROM INFORMATION_SCHEMA.ROUTINES " +
                $"WHERE ROUTINE_TYPE=\"FUNCTION\" " +
                $"  AND ROUTINE_SCHEMA=\"{Connection.Database}\";", "ROUTINE_NAME");
        }

        public async override Task<DataTable> GetFunctionsFullInfo()
        {
            return await GetTableOfValuesAsync($"SELECT * " +
                $"FROM INFORMATION_SCHEMA.ROUTINES " +
                $"WHERE ROUTINE_TYPE=\"FUNCTION\" " +
                $"  AND ROUTINE_SCHEMA=\"{Connection.Database}\";");
        }

        protected override async Task<int> GetIntValueAsync(string query)
        {
            MySqlCommand command = new MySqlCommand(query, (MySqlConnection)Connection);

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
                await command.DisposeAsync();
            }
        }

        protected override async Task<List<string>> GetListOfStringValuesAsync(string query, string column)
        {
            List<string> result = new List<string>();

            MySqlCommand command = new MySqlCommand(query, (MySqlConnection)Connection);

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
                await command.DisposeAsync();
            }
        }

        protected override async Task<DataTable> GetTableOfValuesAsync(string query)
        {
            MySqlCommand command = new MySqlCommand(query, (MySqlConnection)Connection);

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
                await command.DisposeAsync();
            }
        }
    }
}
