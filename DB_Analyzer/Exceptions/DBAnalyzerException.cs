using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.Exceptions
{
    public class DBAnalyzerException : Exception
    {
        public DBAnalyzerException() { }

        public DBAnalyzerException(string message) : base(message) { }

        public DBAnalyzerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
