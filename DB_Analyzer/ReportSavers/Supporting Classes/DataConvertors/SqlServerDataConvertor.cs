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
        public override string ConvertValue(object value, Type type)
        {
            if (TypesHandler.TypesHandler.IsReferenceValueType(type))
            {
                return JsonSerializer.Serialize(value, type);
            }

            string? data = value.ToString();

            return data switch
            {
                "True" => "1",
                "False" => "0",
                null => "null",
                _ => data
            };
        }

        public override string ConvertDataTableValue(object value, Type type)
        {
            if (type == typeof(DateTime))
            {
                string newDateString = DateTime.ParseExact(value.ToString(), "dd.MM.yyyy HH:mm:ss", null).ToString("yyyy-MM-dd HH:mm:ss");
                return $"CONVERT(datetime, '{newDateString}', 120)";
            }

            string? data = value.ToString();

            return data switch
            {
                "True" => "1",
                "False" => "0",
                null => "null",
                _ => $"'{data}'"
            };
        }
    }
}
