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

        public DbDataInserter(DbConnection connection, DbConnection analyzedDbConnection) 
            : base(analyzedDbConnection)
        {
            Connection = connection;
        }

        //Full path of inserting Data
        public async Task InsertData(List<ReportItem> reportItems)
        {
            await InsertDefaultDataForReport();

            await Parallel.ForEachAsync(reportItems, async (reportItem, state) =>
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
            });
        }

        protected abstract Task InsertDefaultDataForReport();

        protected abstract Task<int> GetReportID();

        protected abstract Task InsertDataForScalarValue(ReportItem reportItem);

        protected abstract Task InsertDataForReferenceValue(ReportItem reportItem);

        protected abstract Task InsertDataForDataTable(ReportItem reportItem);

        //Routine Methods to execute queries in different ways
        protected abstract Task ExecuteNonQueryAsync(string query);
    }
}
