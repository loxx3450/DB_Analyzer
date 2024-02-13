using DB_Analyzer.Analyzers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.Exceptions.Global
{
    internal class ConnectionException : DBAnalyzerException
    {
        public static string unableToOpenConnection = "Connection can't be opened by some issue... ";
        public static string unableToCloseConnection = "Connection can't be closed by some issue... ";

        public ConnectionException() { }

        public ConnectionException(string message) : base(message) { }

        public ConnectionException(string message, Exception innerException) : base(message, innerException) { }
    }
}
