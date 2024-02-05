using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportItems
{
    public class ScalarValue<T> where T : struct
    {
        public T Value { get; set; }


        public static implicit operator T(ScalarValue<T> sv)
        {
            return sv.Value;
        }

        public static implicit operator ScalarValue<T>(T sv)
        {
            return new ScalarValue<T>() { Value = sv };
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
