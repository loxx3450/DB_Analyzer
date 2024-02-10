using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.Exceptions.AnalyzerExceptions
{
    public class MySqlAnalyzerException : AnalyzerException
    {
        public const string methodIsNotSupported = "Method you try to execute is not supported by MySqlAnalyzer... Read documentation!!!";

        public MySqlAnalyzerException(string message) : base(message) { }
    }
}
