﻿using DB_Analyzer.Analyzers;
using DB_Analyzer.ReportItems.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportItems.Functions.TableValued
{
    public class NumberOfTableValuedFunctionsReportItem : ReportItem, IReportItem<int>, ISqlServerReportItem
    {
        public override string Name { get; } = "number_of_table_valued_functions";

        public async override Task Run(DbAnalyzer analyzer)
        {
            Value = await analyzer.GetNumberOfTableValuedFunctions();
        }
    }
}
