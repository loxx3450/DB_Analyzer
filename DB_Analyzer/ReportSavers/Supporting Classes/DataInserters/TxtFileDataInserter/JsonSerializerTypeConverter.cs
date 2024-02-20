using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers.Supporting_Classes.DataInserters.TxtFileDataInserter.JsonSerializerTypeConverter
{
    public class JsonSerializerTypeConverter : System.Text.Json.Serialization.JsonConverter<Type>
    {
        public override Type Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string typeName = reader.GetString();
            return Type.GetType(typeName);
        }

        public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.AssemblyQualifiedName);
        }
    }
}
