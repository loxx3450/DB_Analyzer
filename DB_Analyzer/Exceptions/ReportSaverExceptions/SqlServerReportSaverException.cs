using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.Exceptions.ReportSaverExceptions
{
    public class SqlServerReportSaverException : ReportSaverException
    {
        public static string problemDuringProvidingStructure = "Something went wrong during providing the structure... ";

        public SqlServerReportSaverException() { }

        public SqlServerReportSaverException(string message) : base(message) { }

        public SqlServerReportSaverException(string message, Exception innerException) : base(message, innerException) { }
    }
}
