using DB_Analyzer.Exceptions.Global;
using DB_Analyzer.ReportItems;
using DB_Analyzer.ReportSavers.DataInserters;
using DB_Analyzer.ReportSavers.StructureProviders;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers.DbReportSavers
{
    public abstract class DbReportSaver : ReportSaver
    {
        protected DbConnection Connection { get; set; }

        public DbReportSaver(DbConnection connection)
        {
            Connection = connection;
        }

        private async Task OpenDBAsync()
        {
            try
            {
                await Connection.OpenAsync();
            }
            catch (Exception ex)
            {
                throw new ConnectionException(ConnectionException.unableToOpenConnection, ex);
            }
        }

        public async override Task SaveReport(List<IReportItem<object>> reportItems)
        {
            await OpenDBAsync();

            await StructureProvider.ProvideStructure(reportItems);

            await DataInserter.InsertData(reportItems);
        }

        ~DbReportSaver()
        {
            try
            {
                Connection.Close();
            }
            catch (Exception ex)
            {
                throw new ConnectionException(ConnectionException.unableToCloseConnection, ex);
            }
        }
    }
}
