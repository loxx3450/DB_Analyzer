using DB_Analyzer.ReportSavers.TypesHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers.DataConvertors
{
    //Converts values from C# in correct analog in exact DBMS
    internal abstract class DataConvertor
    {
        public abstract string ConvertValue(object value, Type type);

        public abstract string ConvertDataTableValue(object value, Type type);
    }
}
