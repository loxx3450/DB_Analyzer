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

        public DbDataInserter(DbConnection connection, string analyzedDB_Name) : base(analyzedDB_Name)
        { 
            Connection = connection;
        }

        public override async Task InsertData(List<IReportItem<object>> reportItems)
        {
            await InsertDataForReport();

            foreach (var reportItem in reportItems)
            {
                if (TypesHandler.TypesHandler.IsScalarValueType(reportItem.GetValueType()))
                {
                    await InsertDataForScalarValue(reportItem);
                }
                else if (TypesHandler.TypesHandler.IsReferenceValueType(reportItem.GetValueType()))
                {
                    await InsertDataForReferenceValue(reportItem);
                }
                else if (TypesHandler.TypesHandler.IsDataTableValueType(reportItem.GetValueType()))
                {
                    await InsertDataForDataTable(reportItem);
                }
            }
        }

        protected abstract Task InsertDataForReport();

        protected abstract Task InsertDataForScalarValue(IReportItem<object> reportItem);

        protected abstract Task InsertDataForReferenceValue(IReportItem<object> reportItem);

        protected abstract Task InsertDataForDataTable(IReportItem<object> reportItem);
    }
}
