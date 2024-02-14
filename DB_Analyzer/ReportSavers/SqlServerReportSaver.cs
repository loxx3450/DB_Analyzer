using DB_Analyzer.ReportItems;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB_Analyzer.ReportSavers.TypesHandler;
using DB_Analyzer.Exceptions.ReportSaverExceptions;
using DB_Analyzer.Exceptions.Global;
using System.Data;
using DB_Analyzer.ReportSavers.StructureProviders;

namespace DB_Analyzer.ReportSavers
{
    public class SqlServerReportSaver : ReportSaver
    {
        private SqlConnection Connection { get; set; }

        public SqlServerReportSaver(string connectionString)
        {
            Connection = new SqlConnection(connectionString);

            StructureProvider = new SqlServerStructureProvider(Connection);
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
        }
        
        ~SqlServerReportSaver()
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
