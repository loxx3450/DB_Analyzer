﻿using DB_Analyzer.Analyzers;
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
        public DbConnection Connection { get; protected set; }
        public string ConnectionString { get; set; }
        protected DbAnalyzer Analyzer { get; set; }
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

        public async Task SaveReport(ReportSaver reportSaver, List<IReportItem<object>> reportItems)
        {
            await reportSaver.SaveReport(reportItems);
        }

        public abstract Task Analyze(List<IReportItem<object>> reportItems);
        public abstract List<IReportItem<object>> GetAllPossibleReportItems();

        ~Manager()
        {
            try
            {
                Connection.Close();
            }
            catch (Exception ex)
            {
                throw new ConnectionException(ConnectionException.unableToCloseConnection + ex.Message, ex);
            }
        }
    }
}
