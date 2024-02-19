using DB_Analyzer.ReportItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB_Analyzer.ReportSavers.DataConvertors;

namespace DB_Analyzer.ReportSavers.DataInserters
{
    internal abstract class DataInserter
    {
        protected string AnalyzedDB_Name { get; set; }
        protected DataConvertor DataConvertor { get; set; }

        public DataInserter(string analyzedDB_Name)
        {
            AnalyzedDB_Name = analyzedDB_Name;
        }
    }
}
