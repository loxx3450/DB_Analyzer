using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.Exceptions.AnalyzerExceptions
{
    public class SqlServerAnalyzerException : AnalyzerException
    {
        public const string methodIsNotSupported = "Method you try to execute is not supported by SqlServerAnalyzer... Read documentation!!!";

        public SqlServerAnalyzerException() { }

        public SqlServerAnalyzerException(string message) : base(message) { }

        public SqlServerAnalyzerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
