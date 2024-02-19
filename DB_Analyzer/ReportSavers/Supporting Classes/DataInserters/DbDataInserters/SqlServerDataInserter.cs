using DB_Analyzer.Exceptions.ReportSaverExceptions;
using DB_Analyzer.ReportItems;
using DB_Analyzer.ReportSavers.DataConvertors;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers.DataInserters.DbDataInserters
{
    internal class SqlServerDataInserter : DbDataInserter
    {
        public SqlServerDataInserter(SqlConnection connection, string analyzedDB_Name)
            : base(connection, analyzedDB_Name)
        {
            DataConvertor = new SqlServerDataConvertor();
        }

        protected override async Task InsertDataForReport()
        {
            await ExecuteNonQueryAsync($"INSERT INTO reports (db_name, creation_date) VALUES ('{AnalyzedDB_Name}', GETDATE())");

            ReportID = await GetReportID();

            FirstScalarValue = true;
            FirstReferenceValue = true;
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

        protected override async Task InsertDataForScalarValue(IReportItem<object> reportItem)
        {
            string value = DataConvertor.ConvertValue(reportItem.Value, reportItem.GetValueType());

            if (FirstScalarValue)
            {
                await ExecuteNonQueryAsync($"INSERT INTO scalar_values ({reportItem.Name}, report_id) " +
                    $"VALUES ({value}, {ReportID})");

                FirstScalarValue = false;
            }
            else
            {
                await ExecuteNonQueryAsync($"UPDATE scalar_values " +
                    $"SET {reportItem.Name} = {value} " +
                    $"WHERE report_id = {ReportID}");
            }
        }

        protected override async Task InsertDataForReferenceValue(IReportItem<object> reportItem)
        {
            string value = DataConvertor.ConvertValue(reportItem.Value, reportItem.GetValueType());

            if (FirstReferenceValue)
            {
                await ExecuteNonQueryAsync($"INSERT INTO reference_values ({reportItem.Name}, report_id) " +
                    $"VALUES ('{value}', {ReportID})");

                FirstReferenceValue = false;
            }
            else
            {
                await ExecuteNonQueryAsync($"UPDATE reference_values " +
                    $"SET {reportItem.Name} = '{value}' " +
                    $"WHERE report_id = {ReportID}");
            }
        }

        protected override async Task InsertDataForDataTable(IReportItem<object> reportItem)
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
