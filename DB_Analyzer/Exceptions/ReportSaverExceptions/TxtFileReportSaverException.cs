using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.Exceptions.ReportSaverExceptions
{
    public class TxtFileReportSaverException : ReportSaverException
    {
        public static string tryToSaveUnknownType = "You are trying to save unknown type... Try to extend structure of Report!";

        public TxtFileReportSaverException() { }

        public TxtFileReportSaverException(string message) : base(message) { }

        public TxtFileReportSaverException(string message, Exception innerException) : base(message, innerException) { }
    }
}
