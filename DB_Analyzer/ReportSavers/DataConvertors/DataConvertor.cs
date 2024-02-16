using DB_Analyzer.ReportSavers.TypesHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers.DataConvertors
{
    internal abstract class DataConvertor
    {
        public abstract string ConvertData(object data, Type type);

        public abstract string ConvertDataTableValue(object data, Type type);
    }
}
