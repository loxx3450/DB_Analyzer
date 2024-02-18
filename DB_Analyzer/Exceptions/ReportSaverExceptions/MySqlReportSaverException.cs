using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.Exceptions.ReportSaverExceptions
{
    public class MySqlReportSaverException : ReportSaverException
    {
        public static string problemDuringProvidingStructure = "Something went wrong during providing the structure... ";
        public static string problemDuringInsertingData = "Something went wrong during inserting the data of report... ";

        public MySqlReportSaverException() { }

        public MySqlReportSaverException(string message) : base(message) { }

        public MySqlReportSaverException(string message, Exception innerException) : base(message, innerException) { }
    }
}
