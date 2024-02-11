using DB_Analyzer.Analyzers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportItems
{
    public abstract class ReportItem<T> : IReportItem<T> where T : class
    {
        public abstract string Name { get; }
        public abstract T Value { get; protected set; }

        public Type GetValueType()
        {
            return typeof(T);
        }

        public abstract Task Run(DbAnalyzer analyzer);
    }
}
