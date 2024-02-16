using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers.DataConvertors
{
    internal class SqlServerDataConvertor : DataConvertor
    {
        public override string ConvertData(object data, Type type)
        {
            if (TypesHandler.TypesHandler.IsReferenceValueType(type))
            {
                return JsonSerializer.Serialize(data, type);
            }

            string? value = data.ToString();

            return value switch
            {
                "True" => "1",
                "False" => "0",
                null => "null",
                _ => value
            };
        }

        public override string ConvertDataTableValue(object data, Type type)
        {
            if (type == typeof(DateTime))
            {
                string newDateString = DateTime.ParseExact(data.ToString(), "dd.MM.yyyy HH:mm:ss", null).ToString("yyyy-MM-dd HH:mm:ss");
                return $"CONVERT(datetime, '{newDateString}', 120)";
            }

            string? value = data.ToString();

            return value switch
            {
                "True" => "1",
                "False" => "0",
                null => "null",
                _ => $"'{value}'"
            };
        }
    }
}
