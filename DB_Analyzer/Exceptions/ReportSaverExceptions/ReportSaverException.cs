using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.Exceptions.ReportSaverExceptions
{
    public class ReportSaverException : DBAnalyzerException
    {
        public ReportSaverException() { }

        public ReportSaverException(string message) : base(message) { }

        public ReportSaverException(string message, Exception innerException) : base(message, innerException) { }
    }
}
