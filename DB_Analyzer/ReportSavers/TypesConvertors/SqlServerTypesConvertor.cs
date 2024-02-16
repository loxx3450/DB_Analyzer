using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers.TypesConvertors
{
    internal class SqlServerTypesConvertor : TypesConvertor
    {
        public override string ConvertType(string dataType)
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
