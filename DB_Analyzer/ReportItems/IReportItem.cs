﻿using DB_Analyzer.Analyzers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportItems
{
    public interface IReportItem<out T> where T : class
    {
        T Value { get; }
        Task Run(DbAnalyzer analyzer);
    }
}
