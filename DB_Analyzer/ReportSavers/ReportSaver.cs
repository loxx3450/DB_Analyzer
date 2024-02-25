﻿using DB_Analyzer.ReportItems;
using DB_Analyzer.ReportSavers.DataInserters;
using DB_Analyzer.ReportSavers.StructureProviders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers
{
    public abstract class ReportSaver
    {
        internal IStructureProvider StructureProvider { get; set; }
        internal DataInserter DataInserter { get; set; }

        public abstract Task SaveReport(List<ReportItem> reportItems);
    }
}
