using DB_Analyzer.Exceptions.ReportSaverExceptions;
using DB_Analyzer.ReportItems;
using DB_Analyzer.ReportSavers.DataConvertors;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = System.Type;

namespace DB_Analyzer.ReportSavers.DataInserters.DbDataInserters
{
    internal class SqlServerDataInserter : DbDataInserter
    {
        public SqlServerDataInserter(SqlConnection connection, DbConnection analyzedDbConnection)
            : base(connection, analyzedDbConnection)
        {
            DataConvertor = new SqlServerDataConvertor();
        }

        protected override async Task InsertDefaultDataForReport()
        {
            await ExecuteNonQueryAsync($"INSERT INTO reports (dbms_name, server_name, db_name, creation_date) " +
                $"VALUES ('{GetDbmsName()}', '{AnalyzedDbConnection.DataSource}', '{AnalyzedDbConnection.Database}', GETDATE())");

            ReportID = await GetReportID();

            await ExecuteNonQueryAsync($"INSERT INTO scalar_values (report_id) VALUES ({ReportID})");
            await ExecuteNonQueryAsync($"INSERT INTO reference_values (report_id) VALUES ({ReportID})");
        }

        protected override async Task<int> GetReportID()
        {
            SqlCommand command = new SqlCommand("SELECT MAX(id) FROM reports", (SqlConnection)Connection);

            try
            {
                return Convert.ToInt32(await command.ExecuteScalarAsync());
            }
            catch (Exception ex)
            {
                throw new SqlServerReportSaverException("Something went wrong during reading inserted data of report... " + ex.Message, ex);
            }
            finally
            {
                await command.DisposeAsync();
            }
        }

        protected override async Task InsertDataForScalarValue(ReportItem reportItem)
        {
            Type type = TypesHandler.TypesHandler.GetScalarValueType(reportItem);

            string value = DataConvertor.ConvertValue(reportItem.Value, type);

            await ExecuteNonQueryAsync($"UPDATE scalar_values " +
                $"SET {reportItem.Name} = {value} " +
                $"WHERE report_id = {ReportID}");
        }

        protected override async Task InsertDataForReferenceValue(ReportItem reportItem)
        {
            Type type = TypesHandler.TypesHandler.GetReferenceValueType(reportItem);

            string value = DataConvertor.ConvertValue(reportItem.Value, type);

            await ExecuteNonQueryAsync($"UPDATE reference_values " +
                $"SET {reportItem.Name} = '{value}' " +
                $"WHERE report_id = {ReportID}");
        }

        protected override async Task InsertDataForDataTable(ReportItem reportItem)
        {
            string query = $"INSERT INTO {reportItem.Name} (";

            DataTable dataTable = (DataTable)reportItem.Value;

            if (dataTable.Rows.Count == 0)
                return;

            bool firstColumn = true;

            foreach (DataColumn column in dataTable.Columns)
            {
                if (!firstColumn)
                    query += $", ";

                query += column.ColumnName;

                firstColumn = false;
            }

            query += ", report_id) VALUES";

            foreach (DataRow row in dataTable.Rows)
            {
                query += '(';

                firstColumn = true;

                for (int i = 0; i < dataTable.Columns.Count; ++i)
                {
                    if (!firstColumn)
                        query += $", ";

                    query += DataConvertor.ConvertDataTableValue(row[i], dataTable.Columns[i].DataType);

                    firstColumn = false;
                }

                query += $", {ReportID}),";
            }

            query = query.Remove(query.Length - 1);

            await ExecuteNonQueryAsync(query);
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
                throw new SqlServerReportSaverException(SqlServerReportSaverException.problemDuringInsertingData + ex.Message, ex);
            }
            finally
            {
                await command.DisposeAsync();
            }
        }
    }
}
