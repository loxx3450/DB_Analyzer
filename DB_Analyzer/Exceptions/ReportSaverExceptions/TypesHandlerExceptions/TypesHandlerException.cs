using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.Exceptions.ReportSaverExceptions.TypesHandlerExceptions
{
    public class TypesHandlerException : ReportSaverException
    {
        public TypesHandlerException() { }

        public TypesHandlerException(string message) : base(message) { }

        public TypesHandlerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
