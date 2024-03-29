﻿using DB_Analyzer.ReportItems;
using DB_Analyzer.ReportItems.Flags;
using DB_Analyzer.ReportItems.Functions.Global;
using DB_Analyzer.ReportItems.Functions.Scalar;
using DB_Analyzer.ReportItems.Functions.TableValued;
using DB_Analyzer.ReportItems.StoredProcedures;
using DB_Analyzer.ReportItems.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.Helpers.ReportItemsListsCreator
{
    internal class ReportItemsListCreator
    {
        //Hardcorded collection
        protected List<ReportItem> allReportItems = new List<ReportItem>()
        {
            new NumberOfTablesReportItem(),
            new TablesNamesReportItem(),
            new TablesFullInfoReportItem(),
            new NumberOfStoredProceduresReportItem(),
            new StoredProceduresNamesReportItem(),
            new StoredProceduresFullInfoReportItem(),
            new NumberOfFunctionsReportItem(),
            new FunctionsNamesReportItem(),
            new FunctionsFullInfoReportItem(),
            new NumberOfScalarFunctionsReportItem(),
            new ScalarFunctionsNamesReportItem(),
            new ScalarFunctionsFullInfoReportItem(),
            new NumberOfTableValuedFunctionsReportItem(),
            new TableValuedFunctionsNamesReportItem(),
            new TableValuedFunctionsFullInfoReportItem()
        };

        //Checks ReportItem on realizing specific interface-flag (ISqlServerReportItem, IMySqlReportItem etc.)
        public List<ReportItem> GetAllPossibleReportItems<T>()
        {
            List<ReportItem> reportItems = new List<ReportItem>();

            foreach (var item in allReportItems)
            {
                if (item is T)
                {
                    reportItems.Add(item);
                }
            }

            return reportItems;
        }
    }
}
