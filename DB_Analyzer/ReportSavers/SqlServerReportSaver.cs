using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers
{
    public class SqlServerReportSaver : ReportSaver
    {
        private SqlConnection Connection { get; set; }

        public SqlServerReportSaver(string connectionString)
        {
            Connection = new SqlConnection(connectionString);
        }

        private async Task OpenDBAsync()
        {
            try
            {
                await Connection.OpenAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async override Task SaveReport()
        {
            await OpenDBAsync();

            await ProvideStructureForSaving();
        }

        private async Task ProvideStructureForSaving()
        {
            await ProvideDefaultStructureForSaving();

            await ProvideExtendedStructureForSaving();
        }

        private async Task ProvideDefaultStructureForSaving()
        {
            await CreateTableIfNotExists("reports",
                "id INT PRIMARY KEY IDENTITY(1,1), " +
                "db_name NVARCHAR(MAX) NOT NULL, " +
                "creation_date DATETIME NOT NULL");

            await CreateTableIfNotExists("scalar_values",
                "id INT PRIMARY KEY IDENTITY(1,1), " +
                "report_id INT NOT NULL, " +
                "CONSTRAINT FK_scalar_values_reports FOREIGN KEY (report_id) REFERENCES reports(id)");

            await CreateTableIfNotExists("scalar_values_types",
                "id INT PRIMARY KEY IDENTITY(1,1), " +
                "scalar_value_name NVARCHAR(MAX) NOT NULL, " +
                "type_name NVARCHAR(MAX) NOT NULL");

            await CreateTableIfNotExists("reference_values",
                "id INT PRIMARY KEY IDENTITY(1,1), " +
                "report_id INT NOT NULL, " +
                "CONSTRAINT FK_reference_values_reports FOREIGN KEY (report_id) REFERENCES reports(id)");

            await CreateTableIfNotExists("reference_values_types",
                "id INT PRIMARY KEY IDENTITY(1,1), " +
                "reference_value_name NVARCHAR(MAX) NOT NULL, " +
                "type_name NVARCHAR(MAX) NOT NULL");
        }

        private async Task CreateTableIfNotExists(string dbName, string parameters)
        {
            SqlCommand command = new SqlCommand() { Connection = Connection };

            command.CommandText = $"SELECT 1 FROM sys.tables WHERE name = '{dbName}'";

            try
            {
                bool tableExists = Convert.ToBoolean(await command.ExecuteScalarAsync());

                if (!tableExists)
                {
                    command.CommandText = $"CREATE TABLE {dbName} ({parameters})";

                    await command.ExecuteNonQueryAsync();
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

        private async Task ProvideExtendedStructureForSaving()
        {

        }

        ~SqlServerReportSaver()
        {
            try
            {
                Connection.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
