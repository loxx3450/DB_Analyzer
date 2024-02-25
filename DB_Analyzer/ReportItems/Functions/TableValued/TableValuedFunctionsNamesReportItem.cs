﻿using DB_Analyzer.Analyzers;
using DB_Analyzer.ReportItems.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportItems.Functions.TableValued
{
    public class TableValuedFunctionsNamesReportItem : ReportItem, IReportItem<List<string>>, ISqlServerReportItem
    {
        public override string Name { get; } = "table_valued_functions_names";

        public async override Task Run(DbAnalyzer analyzer)
        {
            Value = await analyzer.GetTableValuedFunctionsNames();
        }
    }
}
