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
        //Data for insert
        private int ReportID { get; set; }

        //Flags
        private bool FirstScalarValue { get; set; }
        private bool FirstReferenceValue { get; set; }


        public SqlServerDataInserter(SqlConnection connection, string analyzedDB_Name)
            : base(connection, analyzedDB_Name)
        {
            DataConvertor = new SqlServerDataConvertor();
        }

        protected override async Task InsertDataForReport()
        {
            SqlCommand command = new SqlCommand() { Connection = (SqlConnection)Connection };

            command.CommandText = $"INSERT INTO reports (db_name, creation_date) VALUES ('{AnalyzedDB_Name}', GETDATE())";

            try
            {
                await command.ExecuteNonQueryAsync();

                ReportID = await GetReportID();

                FirstScalarValue = true;
                FirstReferenceValue = true;
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

        private async Task<int> GetReportID()
        {
            SqlCommand command = new SqlCommand() { Connection = (SqlConnection)Connection };

            command.CommandText = $"SELECT MAX(id) FROM reports";

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
            SqlCommand command = new SqlCommand() { Connection = (SqlConnection)Connection };

            if (FirstScalarValue)
            {
                command.CommandText = $"INSERT INTO scalar_values ({reportItem.Name}, report_id) " +
                    $"VALUES ({DataConvertor.ConvertValue(reportItem.Value, reportItem.GetValueType())}, {ReportID})";
            }
            else
            {
                command.CommandText = $"UPDATE scalar_values " +
                    $"SET {reportItem.Name} = {DataConvertor.ConvertValue(reportItem.Value, reportItem.GetValueType())} " +
                    $"WHERE report_id = {ReportID}";
            }

            try
            {
                await command.ExecuteNonQueryAsync();

                FirstScalarValue = false;
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

        protected override async Task InsertDataForReferenceValue(IReportItem<object> reportItem)
        {
            SqlCommand command = new SqlCommand() { Connection = (SqlConnection)Connection };

            if (FirstReferenceValue)
            {
                command.CommandText = $"INSERT INTO reference_values ({reportItem.Name}, report_id) " +
                    $"VALUES ('{DataConvertor.ConvertValue(reportItem.Value, reportItem.GetValueType())}', {ReportID})";
            }
            else
            {
                command.CommandText = $"UPDATE reference_values " +
                    $"SET {reportItem.Name} = '{DataConvertor.ConvertValue(reportItem.Value, reportItem.GetValueType())}' " +
                    $"WHERE report_id = {ReportID}";
            }

            try
            {
                await command.ExecuteNonQueryAsync();

                FirstReferenceValue = false;
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

        protected override async Task InsertDataForDataTable(IReportItem<object> reportItem)
        {
            SqlCommand command = new SqlCommand() { Connection = (SqlConnection)Connection };

            string query = $"INSERT INTO {reportItem.Name} (";

            DataTable dataTable = (DataTable)reportItem.Value;

            bool firstColumn = true;
            
            foreach (DataColumn column in dataTable.Columns)
            {
                if (!firstColumn)
                    query += $", ";

                query += column.ColumnName;

                firstColumn = false;
            }

            query += ", report_id) VALUES (";

            firstColumn = true;

            for (int i = 0; i < dataTable.Columns.Count; ++i)
            {
                if (!firstColumn)
                    query += $", ";

                query += DataConvertor.ConvertDataTableValue(dataTable.Rows[0][i], dataTable.Columns[i].DataType);

                firstColumn = false;
            }

            query += $", {ReportID})";

            command.CommandText = query;

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
