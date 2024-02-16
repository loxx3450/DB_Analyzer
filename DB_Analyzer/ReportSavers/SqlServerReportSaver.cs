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
using DB_Analyzer.ReportSavers.DataInserters;
using DB_Analyzer.ReportSavers.DataInserters.DbDataInserters;
using DB_Analyzer.ReportSavers.StructureProviders.DbStructureProviders;

namespace DB_Analyzer.ReportSavers
{
    public class SqlServerReportSaver : ReportSaver
    {
        private SqlConnection Connection { get; set; }

        public SqlServerReportSaver(string connectionString, string analyzedDB_Name)
        {
            Connection = new SqlConnection(connectionString);

            StructureProvider = new SqlServerStructureProvider(Connection);

            DataInserter = new SqlServerDataInserter(Connection, analyzedDB_Name);
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
