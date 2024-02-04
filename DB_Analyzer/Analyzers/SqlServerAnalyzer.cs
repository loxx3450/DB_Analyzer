using Microsoft.Data.SqlClient;
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

        public async override Task<int> GetNumberOfTables()
        {
            string query = "SELECT COUNT(*) FROM sys.tables";

            SqlCommand command = new SqlCommand(query, (SqlConnection)Connection);

            try
            {
                return (int)await command.ExecuteScalarAsync();
            }
            catch (Exception ex) 
            {
                throw;
            }
        }

        public async override Task<List<string>> GetTablesNames()
        {
            List<string> tablesNames = new List<string>();

            string query = "SELECT table_name FROM information_schema.tables WHERE table_type = 'base table'";

            SqlCommand command = new SqlCommand(query, (SqlConnection)Connection);

            try
            {
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                {
                    while (reader.Read()) 
                    {
                        tablesNames.Add(reader.GetFieldValue<string>("table_name"));
                    }

                    return tablesNames;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async override Task<int> GetNumberOfColumns(string tableName)
        {
            string query = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @Table";

            SqlCommand command = new SqlCommand(query, (SqlConnection)Connection);

            command.Parameters.Add(new SqlParameter()
            {
                ParameterName = "@Table",
                SqlDbType = SqlDbType.NVarChar,
                Value = tableName
            });

            try
            {
                return (int)await command.ExecuteScalarAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async override Task<List<string>> GetColumnsNames(string tableName)
        {
            List<string> columnsNames = new List<string>();

            string query = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @Table";

            SqlCommand command = new SqlCommand(query, (SqlConnection)Connection);

            command.Parameters.Add(new SqlParameter() 
            { 
                ParameterName = "@Table", 
                SqlDbType = SqlDbType.NVarChar, 
                Value = tableName
            });

            try
            {
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                {
                    while (reader.Read())
                    {
                        columnsNames.Add(reader.GetFieldValue<string>("COLUMN_NAME"));
                    }

                    return columnsNames;
                }
            }
            catch(Exception ex) 
            {
                throw;
            }
        }
    }
}
