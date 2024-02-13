using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers.TypesConvertor
{
    internal static class TypesConvertor
    {
        public static string ConvertTypeForSqlServer(string dataType)
        {
            return dataType switch
            {
                "string" => "nvarchar(max)",
                "int32" => "int",
                "boolean" => "bit",
                "byte" => "smallint",
                _ => dataType
            };
        }
    }
}
