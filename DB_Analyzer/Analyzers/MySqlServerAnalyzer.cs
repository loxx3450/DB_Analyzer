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
    internal class MySqlServerAnalyzer : DbAnalyzer
    {
        public MySqlServerAnalyzer(MySqlConnection connection)
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
                return (int)(await command.ExecuteScalarAsync());
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

        public async override Task<int> GetNumberOfColumns(string tableName)
        {
            string query = $"SELECT COUNT(*) " +
                $"FROM INFORMATION_SCHEMA.COLUMNS " +
                $"WHERE TABLE_SCHEMA='{Connection.Database}' " +
                $"  AND TABLE_NAME = @Table;";

            MySqlCommand command = new MySqlCommand(query, (MySqlConnection)Connection);

            command.Parameters.Add(new MySqlParameter("Table", tableName));

            try
            {
                return (int)(await command.ExecuteScalarAsync());
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

        public async override Task<List<string>> GetColumnsNames(string tableName)
        {
            List<string> columnsNames = new List<string>();

            string query = $"SELECT COLUMN_NAME " +
                $"FROM INFORMATION_SCHEMA.COLUMNS " +
                $"WHERE TABLE_SCHEMA='{Connection.Database}' " +
                $"  AND TABLE_NAME = @Table";


            MySqlCommand command = new MySqlCommand(query, (MySqlConnection)Connection);

            command.Parameters.Add(new MySqlParameter("Table", tableName));

            try
            {
                using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();
                {
                    while (reader.Read())
                    {
                        columnsNames.Add(reader.GetFieldValue<string>("COLUMN_NAME"));
                    }

                    return columnsNames;
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

        public async override Task<DataTable> GetColumnsFullInfo(string tableName)
        {
            string query = $"SELECT * " +
                $"FROM INFORMATION_SCHEMA.COLUMNS " +
                $"WHERE TABLE_SCHEMA='{Connection.Database}' " +
                $"  AND TABLE_NAME = @Table";

            MySqlCommand command = new MySqlCommand(query, (MySqlConnection)Connection);

            command.Parameters.Add(new MySqlParameter("Table", tableName));

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
                return (int)(await command.ExecuteScalarAsync());
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
    }
}
