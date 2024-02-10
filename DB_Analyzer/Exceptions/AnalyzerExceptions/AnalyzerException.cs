using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.Exceptions.AnalyzerExceptions
{
    public class AnalyzerException : Exception
    {
        public const string problemDuringHandling = "Something went wrong by command execution...\t";

        public AnalyzerException() { }

        public AnalyzerException(string message) : base(message) { }

        public AnalyzerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
