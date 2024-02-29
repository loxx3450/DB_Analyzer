using DB_Analyzer.ReportItems;
using DB_Analyzer.ReportSavers.DataInserters;
using DB_Analyzer.ReportSavers.StructureProviders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers
{
    public abstract class ReportSaver
    {
        //Connection of analyzed DB
        internal DbConnection AnalyzedDbConnection { get; set; }

        //Supporting Objects for saving Report
        internal IStructureProvider StructureProvider { get; set; }
        internal DataInserter DataInserter { get; set; }

        public ReportSaver(DbConnection analyzedDbConnection)
        {
            AnalyzedDbConnection = analyzedDbConnection;
        }

        public abstract Task SaveReport(List<ReportItem> reportItems);
    }
}
