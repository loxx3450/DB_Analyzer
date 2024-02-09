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
            string query = $"SELECT COUNT(*) " +
                $"FROM INFORMATION_SCHEMA.TABLES " +
                $"WHERE TABLE_SCHEMA = '{ Connection.Database }' " +
                $"  AND TABLE_TYPE='BASE TABLE';";

            MySqlCommand command = new MySqlCommand(query, (MySqlConnection)Connection);

            try
            {
                return Convert.ToInt32(await command.ExecuteScalarAsync());
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                command.Dispose();
            }
        }

        public async override Task<List<string>> GetTablesNames()
        {
            List<string> tablesNames = new List<string>();

            string query = $"SELECT TABLE_NAME " +
                $"FROM INFORMATION_SCHEMA.TABLES " +
                $"WHERE TABLE_SCHEMA='{Connection.Database}' " +
                $"  AND TABLE_TYPE = 'BASE TABLE';";

            MySqlCommand command = new MySqlCommand(query, (MySqlConnection)Connection);

            try
            {
                using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();
                {
                    while (reader.Read())
                    {
                        tablesNames.Add(reader.GetFieldValue<string>("table_name"));
                    }

                    return tablesNames;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                command.Dispose();
            }
        }

        public async override Task<DataTable> GetTablesFullInfo()
        {
            string query = $"SELECT * " +
                $"FROM INFORMATION_SCHEMA.TABLES " +
                $"WHERE TABLE_SCHEMA = '{Connection.Database}' " +
                $"  AND TABLE_TYPE='BASE TABLE';";

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
            catch (Exception)
            {
                throw;
            }
            finally
            {
                command.Dispose();
            }
        }

        public async override Task<int> GetNumberOfStoredProcedures()
        {
            string query = $"SELECT COUNT(ROUTINE_NAME) " +
                $"FROM INFORMATION_SCHEMA.ROUTINES " +
                $"WHERE ROUTINE_TYPE=\"PROCEDURE\" " +
                $"  AND ROUTINE_SCHEMA=\"{ Connection.Database }\";";

            MySqlCommand command = new MySqlCommand(query, (MySqlConnection)Connection);

            try
            {
                return Convert.ToInt32(await command.ExecuteScalarAsync());
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                command.Dispose();
            }
        }

        public async override Task<List<string>> GetStoredProceduresNames()
        {
            List<string> storedProceduresNames = new List<string>();

            string query = $"SELECT ROUTINE_NAME " +
                $"FROM INFORMATION_SCHEMA.ROUTINES " +
                $"WHERE ROUTINE_TYPE=\"PROCEDURE\" " +
                $"  AND ROUTINE_SCHEMA=\"{Connection.Database}\";";

            MySqlCommand command = new MySqlCommand(query, (MySqlConnection)Connection);

            try
            {
                using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();
                {
                    while (reader.Read())
                    {
                        storedProceduresNames.Add(reader.GetFieldValue<string>("ROUTINE_NAME"));
                    }

                    return storedProceduresNames;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                command.Dispose();
            }
        }

        public async override Task<DataTable> GetStoredProceduresFullInfo()
        {
            string query = $"SELECT * " +
                $"FROM INFORMATION_SCHEMA.ROUTINES " +
                $"WHERE ROUTINE_TYPE=\"PROCEDURE\" " +
                $"  AND ROUTINE_SCHEMA=\"{Connection.Database}\";";

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
            catch (Exception)
            {
                throw;
            }
            finally
            {
                command.Dispose();
            }
        }

        public override Task<int> GetNumberOfScalarFunctions()
        {
            throw new NotImplementedException();
        }

        public override Task<List<string>> GetScalarFunctionsNames()
        {
            throw new NotImplementedException();
        }

        public override Task<DataTable> GetScalarFunctionsFullInfo()
        {
            throw new NotImplementedException();
        }

        public override Task<int> GetNumberOfTableValuedFunctions()
        {
            throw new NotImplementedException();
        }

        public override Task<List<string>> GetTableValuedFunctionsNames()
        {
            throw new NotImplementedException();
        }

        public override Task<DataTable> GetTableValuedFunctionsFullInfo()
        {
            throw new NotImplementedException();
        }

        public async override Task<int> GetNumberOfFunctions()
        {
            string query = $"SELECT COUNT(ROUTINE_NAME) " +
                $"FROM INFORMATION_SCHEMA.ROUTINES " +
                $"WHERE ROUTINE_TYPE=\"FUNCTION\" " +
                $"  AND ROUTINE_SCHEMA=\"{Connection.Database}\";";

            MySqlCommand command = new MySqlCommand(query, (MySqlConnection)Connection);

            try
            {
                return Convert.ToInt32(await command.ExecuteScalarAsync());
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                command.Dispose();
            }
        }

        public async override Task<List<string>> GetFunctionsNames()
        {
            List<string> storedProceduresNames = new List<string>();

            string query = $"SELECT ROUTINE_NAME " +
                $"FROM INFORMATION_SCHEMA.ROUTINES " +
                $"WHERE ROUTINE_TYPE=\"FUNCTION\" " +
                $"  AND ROUTINE_SCHEMA=\"{Connection.Database}\";";

            MySqlCommand command = new MySqlCommand(query, (MySqlConnection)Connection);

            try
            {
                using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();
                {
                    while (reader.Read())
                    {
                        storedProceduresNames.Add(reader.GetFieldValue<string>("ROUTINE_NAME"));
                    }

                    return storedProceduresNames;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                command.Dispose();
            }
        }

        public async override Task<DataTable> GetFunctionsFullInfo()
        {
            string query = $"SELECT * " +
                $"FROM INFORMATION_SCHEMA.ROUTINES " +
                $"WHERE ROUTINE_TYPE=\"FUNCTION\" " +
                $"  AND ROUTINE_SCHEMA=\"{Connection.Database}\";";

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
            catch (Exception)
            {
                throw;
            }
            finally
            {
                command.Dispose();
            }
        }
    }
}
