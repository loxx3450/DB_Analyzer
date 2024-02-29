using DB_Analyzer.Analyzers;
using DB_Analyzer.Exceptions.Global;
using DB_Analyzer.Helpers.ReportItemsListsCreator;
using DB_Analyzer.ReportItems;
using DB_Analyzer.ReportSavers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.Managers
{
    public abstract class Manager
    {
        //Connection
        public DbConnection Connection { get; protected set; }
        public string ConnectionString { get; protected set; }

        //Analyzer
        protected DbAnalyzer Analyzer { get; set; }

        //Helper
        internal ReportItemsListCreator ReportItemsListCreator { get; set; }

        public Manager(string connectionString, DbConnection connection) 
        { 
            ConnectionString = connectionString;

            Connection = connection;

            ReportItemsListCreator = new ReportItemsListCreator();
        }

        public async Task ConnectToDBAsync()
        {
            try
            {
                await Connection.OpenAsync();
            }
            catch (Exception ex)
            {
                throw new ConnectionException(ConnectionException.unableToOpenConnection + ex.Message, ex);
            }
        }

        //Changing Connection in Runtime
        public abstract Task ChangeConnectionString(string connectionString);

        public abstract List<ReportItem> GetAllPossibleReportItems();

        public abstract Task Analyze(List<ReportItem> reportItems);

        public Task SaveReport(ReportSaver reportSaver, List<ReportItem> reportItems)
        {
            return reportSaver.SaveReport(reportItems);
        }

        protected async Task CloseConnectionAsync()
        {
            try
            {
                if (Connection.State == System.Data.ConnectionState.Open)
                    await Connection.CloseAsync();
            }
            catch (Exception ex)
            {
                throw new ConnectionException(ConnectionException.unableToCloseConnection + ex.Message, ex);
            }
        }

        ~Manager()
        {
            try
            {
                if (Connection.State == System.Data.ConnectionState.Open)
                    Connection.Close();
            }
            catch (Exception ex)
            {
                throw new ConnectionException(ConnectionException.unableToCloseConnection + ex.Message, ex);
            }
        }
    }
}
