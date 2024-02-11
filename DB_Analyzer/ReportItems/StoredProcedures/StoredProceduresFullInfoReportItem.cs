﻿using DB_Analyzer.Analyzers;
using DB_Analyzer.ReportItems.Flags;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportItems.StoredProcedures
{
    public class StoredProceduresFullInfoReportItem : ReportItem<DataTable>, ISqlServerReportItem, IMySqlReportItem
    {
        public override string Name { get; } = "storedProceduresFullInfo";
        public override DataTable Value { get; protected set; }

        public async override Task Run(DbAnalyzer analyzer)
        {
            Value = await analyzer.GetStoredProceduresFullInfo();
        }
    }
}
