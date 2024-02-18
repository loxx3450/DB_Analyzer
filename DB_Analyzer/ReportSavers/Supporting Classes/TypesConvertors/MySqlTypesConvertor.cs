using DB_Analyzer.ReportSavers.TypesConvertors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers.Supporting_Classes.TypesConvertors
{
    internal class MySqlTypesConvertor : TypesConvertor
    {
        public override string ConvertType(string dataType)
        {
            return dataType switch
            {
                "string" => "text",
                "uint64" => "bigint unsigned",
                "int32" => "int",
                "byte" => "smallint",
                _ => dataType
            };
        }
    }
}
