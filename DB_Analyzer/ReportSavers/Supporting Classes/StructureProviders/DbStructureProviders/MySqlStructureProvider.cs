using DB_Analyzer.Exceptions.ReportSaverExceptions;
using DB_Analyzer.ReportItems;
using DB_Analyzer.ReportSavers.Supporting_Classes.TypesConvertors;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DB_Analyzer.ReportSavers.StructureProviders.DbStructureProviders
{
    internal class MySqlStructureProvider : DbStructureProvider
    {
        public MySqlStructureProvider(MySqlConnection connection)
            : base(connection)
        {
            TypesConvertor = new MySqlTypesConvertor();
        }

        protected override async Task ProvideDefaultStructure()
        {
            await CreateTableIfNotExists("reports",
                "id INT AUTO_INCREMENT PRIMARY KEY, " +
                "dbms_name VARCHAR(255) NOT NULL, " +
                "server_name VARCHAR(255) NOT NULL, " +
                "db_name VARCHAR(255) NOT NULL, " +
                "creation_date DATETIME NOT NULL");

            await CreateTableIfNotExists("scalar_values",
                "id INT AUTO_INCREMENT PRIMARY KEY, " +
                "report_id INT NOT NULL, " +
                "CONSTRAINT FK_scalar_values_reports FOREIGN KEY (report_id) REFERENCES reports(id)");

            await CreateTableIfNotExists("scalar_values_types",
                "id INT AUTO_INCREMENT PRIMARY KEY, " +
                "scalar_value_name TEXT NOT NULL, " +
                "type_name TEXT NOT NULL");

            await CreateTableIfNotExists("reference_values",
                "id INT AUTO_INCREMENT PRIMARY KEY, " +
                "report_id INT NOT NULL, " +
                "CONSTRAINT FK_reference_values_reports FOREIGN KEY (report_id) REFERENCES reports(id)");

            await CreateTableIfNotExists("reference_values_types",
                "id INT AUTO_INCREMENT PRIMARY KEY, " +
                "reference_value_name TEXT NOT NULL, " +
                "type_name TEXT NOT NULL");
        }

        protected override async Task CreateTableIfNotExists(string dbName, string parameters)
        {
            if (!await Exists($"SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{Connection.Database}' AND TABLE_NAME = '{dbName}';"))
            {
                await ExecuteNonQueryAsync($"CREATE TABLE {dbName} ({parameters});");
            }
        }

        protected override async Task ProvideExtendedStructureForScalarValue(ReportItem reportItem)
        {
            if (!await Exists($"SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'scalar_values' AND COLUMN_NAME = '{reportItem.Name}'"))
            {
                string type = TypesConvertor.ConvertType(TypesHandler.TypesHandler.GetScalarValueTypeDescription(reportItem));

                await ExecuteNonQueryAsync($"ALTER TABLE scalar_values ADD COLUMN {reportItem.Name} {type} NULL");

                await ExecuteNonQueryAsync($"INSERT INTO scalar_values_types(scalar_value_name, type_name) " +
                    $"VALUES ('{reportItem.Name}', '{type}')");
            }
        }

        protected override async Task ProvideExtendedStructureForReferenceValue(ReportItem reportItem)
        {
            if (!await Exists($"SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'reference_values' AND COLUMN_NAME = '{reportItem.Name}'"))
            {
                await ExecuteNonQueryAsync($"ALTER TABLE reference_values ADD COLUMN {reportItem.Name} TEXT NULL");

                await ExecuteNonQueryAsync($"INSERT INTO reference_values_types(reference_value_name, type_name) " +
                    $"VALUES ('{reportItem.Name}', '{TypesHandler.TypesHandler.GetReferenceValueTypeDescription(reportItem)}')");
            }
        }

        protected override async Task ProvideExtendedStructureForDataTable(ReportItem reportItem)
        {
            if (!await Exists($"SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{Connection.Database}' AND TABLE_NAME = '{reportItem.Name}'"))
            {
                string query = $"   CREATE TABLE {reportItem.Name} (" +
                    "id INT AUTO_INCREMENT PRIMARY KEY, ";

                DataTable dt = (DataTable)reportItem.Value;

                foreach (DataColumn column in dt.Columns)
                {
                    query += $"{column.ColumnName} {TypesConvertor.ConvertType(TypesHandler.TypesHandler.GetDataColumnValueTypeDescription(column))} NULL, ";
                }

                query += "        report_id INT NOT NULL," +
                            $"        CONSTRAINT FK_{reportItem.Name}_report_id FOREIGN KEY (report_id) REFERENCES reports(id)" +
                            "    )";

                await ExecuteNonQueryAsync(query);
            }
        }

        protected override async Task<bool> Exists(string query)
        {
            MySqlCommand command = new MySqlCommand(query, (MySqlConnection)Connection);

            try
            {
                return Convert.ToBoolean(await command.ExecuteScalarAsync());
            }
            catch (Exception ex)
            {
                throw new MySqlReportSaverException(MySqlReportSaverException.problemDuringProvidingStructure + ex.Message, ex);
            }
            finally
            {
                await command.DisposeAsync();
            }
        }

        protected override async Task ExecuteNonQueryAsync(string query)
        {
            MySqlCommand command = new MySqlCommand(query, (MySqlConnection)Connection);

            try
            {
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new MySqlReportSaverException(MySqlReportSaverException.problemDuringProvidingStructure + ex.Message, ex);
            }
            finally
            {
                await command.DisposeAsync();
            }
        }
    }
}
