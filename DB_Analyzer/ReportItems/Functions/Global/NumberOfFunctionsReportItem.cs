﻿using DB_Analyzer.Analyzers;
using DB_Analyzer.ReportItems.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportItems.Functions.Global
{
    public class NumberOfFunctionsReportItem : ReportItem, IReportItem<int>, ISqlServerReportItem, IMySqlReportItem
    {
        public override string Name { get; } = "number_of_functions";

        public async override Task Run(DbAnalyzer analyzer)
        {
            Value = await analyzer.GetNumberOfFunctions();
        }
    }
}
