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
            string query = "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = 'dbo' and TABLE_TYPE='BASE TABLE'";

            MySqlCommand command = new MySqlCommand(query, (MySqlConnection)Connection);

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

            string query = "SELECT table_name FROM information_schema.tables WHERE table_type = 'base table' AND table_schema='test';";

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
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
