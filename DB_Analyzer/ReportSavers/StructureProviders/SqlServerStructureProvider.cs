using DB_Analyzer.Exceptions.ReportSaverExceptions;
using DB_Analyzer.ReportItems;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers.StructureProviders
{
    internal class SqlServerStructureProvider : StructureProvider
    {
        protected SqlConnection Connection { get; set; }
        public SqlServerStructureProvider(SqlConnection connection) 
        { 
            Connection = connection;
        }

        public override async Task ProvideStructure(List<IReportItem<object>> reportItems)
        {
            await ProvideDefaultStructure();

            await ProvideExtendedStructure(reportItems);
        }

        private async Task ProvideDefaultStructure()
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
            catch (Exception ex)
            {
                throw new SqlServerReportSaverException(SqlServerReportSaverException.problemDuringProvidingStructure + ex.Message, ex);
            }
            finally
            {
                await command.DisposeAsync();
            }
        }

        private async Task ProvideExtendedStructure(List<IReportItem<object>> reportItems)
        {
            foreach (var reportItem in reportItems)
            {
                if (TypesHandler.TypesHandler.IsScalarValueType(reportItem.GetValueType()))
                {
                    await ProvideExtendedStructureForScalarValue(reportItem);
                }
                else if (reportItem.Value.GetType() != typeof(DataTable))
                {
                    await ProvideExtendedStructureForReferenceValue(reportItem);
                }
                else
                {
                    await ProvideExtendedStructureForDataTable(reportItem);
                }
            }
        }

        private async Task ProvideExtendedStructureForReferenceValue(IReportItem<object> reportItem)
        {
            SqlCommand command = new SqlCommand() { Connection = Connection };

            command.CommandText = $"SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'reference_values' AND COLUMN_NAME = '{reportItem.Name}'";

            try
            {
                bool columnExists = Convert.ToBoolean(await command.ExecuteScalarAsync());

                if (!columnExists)
                {
                    command.CommandText = $"ALTER TABLE reference_values ADD {reportItem.Name} NVARCHAR(MAX) NULL";

                    await command.ExecuteNonQueryAsync();


                    command.CommandText = $"INSERT INTO reference_values_types(reference_value_name, type_name) VALUES ('{reportItem.Name}', '{TypesHandler.TypesHandler.GetReferenceType(reportItem.GetValueType())}')";

                    await command.ExecuteNonQueryAsync();
                }
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

        private async Task ProvideExtendedStructureForScalarValue(IReportItem<object> reportItem)
        {
            SqlCommand command = new SqlCommand() { Connection = Connection };

            command.CommandText = $"SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'scalar_values' AND COLUMN_NAME = '{reportItem.Name}'";

            try
            {
                bool columnExists = Convert.ToBoolean(await command.ExecuteScalarAsync());

                if (!columnExists)
                {
                    command.CommandText = $"ALTER TABLE scalar_values ADD {reportItem.Name} {TypesConvertor.TypesConvertor.ConvertTypeForSqlServer(TypesHandler.TypesHandler.GetScalarValueType(reportItem.GetValueType()))} NULL";

                    await command.ExecuteNonQueryAsync();


                    command.CommandText = $"INSERT INTO scalar_values_types(scalar_value_name, type_name) VALUES ('{reportItem.Name}', '{TypesHandler.TypesHandler.GetScalarValueType(reportItem.GetValueType())}')";

                    await command.ExecuteNonQueryAsync();
                }
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

        private async Task ProvideExtendedStructureForDataTable(IReportItem<object> reportItem)
        {
            SqlCommand command = new SqlCommand() { Connection = Connection };

            command.CommandText = $"SELECT 1 FROM sys.tables WHERE name = '{reportItem.Name}'";

            try
            {
                bool tableExists = Convert.ToBoolean(await command.ExecuteScalarAsync());

                if (!tableExists)
                {
                    string query = $"   CREATE TABLE {reportItem.Name} (" +
                        "id INT PRIMARY KEY IDENTITY(1,1),";

                    DataTable dt = (DataTable)reportItem.Value;

                    foreach (DataColumn column in dt.Columns)
                    {
                        query += $"{column.ColumnName} {TypesConvertor.TypesConvertor.ConvertTypeForSqlServer(TypesHandler.TypesHandler.GetDataColumnValueType(column))} NULL, ";
                    }

                    query += "        report_id INT NOT NULL," +
                                $"        CONSTRAINT FK_{reportItem.Name}_report_id FOREIGN KEY (report_id) REFERENCES reports(id)" +
                                "    )";

                    command.CommandText = query;

                    await command.ExecuteNonQueryAsync();
                }
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
