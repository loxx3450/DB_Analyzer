using DB_Analyzer.Exceptions.ReportSaverExceptions;
using DB_Analyzer.ReportItems;
using DB_Analyzer.ReportSavers.TypesConvertors;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers.StructureProviders.DbStructureProviders
{
    internal class SqlServerStructureProvider : DbStructureProvider
    {
        public SqlServerStructureProvider(SqlConnection connection)
            : base(connection)
        {
            TypesConvertor = new SqlServerTypesConvertor();
        }

        protected override async Task ProvideDefaultStructure()
        {
            await CreateTableIfNotExists("reports",
                "id INT PRIMARY KEY IDENTITY(1,1), " +
                "dbms_name NVARCHAR(MAX) NOT NULL, " +
                "server_name NVARCHAR(MAX) NOT NULL, " +
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

            DefaultStructureIsProvided = true;
        }

        protected override async Task CreateTableIfNotExists(string dbName, string parameters)
        {
            if (!await Exists($"SELECT 1 FROM sys.tables WHERE name = '{dbName}'"))
            {
                await ExecuteNonQueryAsync($"CREATE TABLE {dbName} ({parameters})");
            }
        }

        protected override async Task ProvideExtendedStructureForScalarValue(ReportItem reportItem)
        {
            if (!await Exists($"SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'scalar_values' AND COLUMN_NAME = '{reportItem.Name}'"))
            {
                string type = TypesConvertor.ConvertType(TypesHandler.TypesHandler.GetScalarValueTypeDescription(reportItem));

                await ExecuteNonQueryAsync($"ALTER TABLE scalar_values ADD {reportItem.Name} {type} NULL");

                await ExecuteNonQueryAsync($"INSERT INTO scalar_values_types(scalar_value_name, type_name) " +
                    $"VALUES ('{reportItem.Name}', '{type}')");
            }
        }

        protected override async Task ProvideExtendedStructureForReferenceValue(ReportItem reportItem)
        {
            if (!await Exists($"SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'reference_values' AND COLUMN_NAME = '{reportItem.Name}'"))
            {
                await ExecuteNonQueryAsync($"ALTER TABLE reference_values ADD {reportItem.Name} NVARCHAR(MAX) NULL");

                await ExecuteNonQueryAsync($"INSERT INTO reference_values_types(reference_value_name, type_name) " +
                    $"VALUES ('{reportItem.Name}', '{TypesHandler.TypesHandler.GetReferenceValueType(reportItem)}')");
            }
        }

        protected override async Task ProvideExtendedStructureForDataTable(ReportItem reportItem)
        {
            if (!await Exists($"SELECT 1 FROM sys.tables WHERE name = '{reportItem.Name}'"))
            {
                string query = $"   CREATE TABLE {reportItem.Name} (" +
                    "id INT PRIMARY KEY IDENTITY(1,1),";

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
            SqlCommand command = new SqlCommand(query, (SqlConnection)Connection);

            try
            {
                return Convert.ToBoolean(await command.ExecuteScalarAsync());
            }
            catch (Exception ex)
            {
                throw new SqlServerReportSaverException(SqlServerReportSaverException.problemDuringProvidingStructure + ex.Message, ex);
            }
            finally
            {
                await command.DisposeAsync();
            }
        }

        protected override async Task ExecuteNonQueryAsync(string query)
        {
            SqlCommand command = new SqlCommand(query, (SqlConnection)Connection);

            try
            {
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new SqlServerReportSaverException(SqlServerReportSaverException.problemDuringProvidingStructure + ex.Message, ex);
            }
            finally
            {
                await command.DisposeAsync();
            }
        }
    }
}
