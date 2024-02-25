using DB_Analyzer.ReportItems;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers.DataInserters.DbDataInserters
{
    internal abstract class DbDataInserter : DataInserter
    {
        //Connection
        protected DbConnection Connection { get; set; }

        //Data for insert
        protected int ReportID { get; set; }

        //Flags
        protected bool FirstScalarValue { get; set; }
        protected bool FirstReferenceValue { get; set; }

        public DbDataInserter(DbConnection connection, DbConnection analyzedDbConnection) 
            : base(analyzedDbConnection)
        {
            Connection = connection;
        }

        public async Task InsertData(List<ReportItem> reportItems)
        {
            await InsertDataForReport();

            foreach (var reportItem in reportItems)
            {
                if (TypesHandler.TypesHandler.IsScalarValueType(reportItem))
                {
                    await InsertDataForScalarValue(reportItem);
                }
                else if (TypesHandler.TypesHandler.IsReferenceValueType(reportItem))
                {
                    await InsertDataForReferenceValue(reportItem);
                }
                else if (TypesHandler.TypesHandler.IsDataTableValueType(reportItem))
                {
                    await InsertDataForDataTable(reportItem);
                }
            }
        }

        protected abstract Task InsertDataForReport();

        protected abstract Task<int> GetReportID();

        protected abstract Task InsertDataForScalarValue(ReportItem reportItem);

        protected abstract Task InsertDataForReferenceValue(ReportItem reportItem);

        protected abstract Task InsertDataForDataTable(ReportItem reportItem);

        protected abstract Task ExecuteNonQueryAsync(string query);
    }
}
