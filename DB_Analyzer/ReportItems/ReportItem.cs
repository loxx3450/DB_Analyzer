using DB_Analyzer.Analyzers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportItems
{
    public abstract class ReportItem
    {
        public abstract string Name { get; }
        public object Value { get; protected set; }

        public K GetValue<K>()
        {
            return (K)Value;
        }

        public abstract Task Run(DbAnalyzer analyzer);
    }
}
