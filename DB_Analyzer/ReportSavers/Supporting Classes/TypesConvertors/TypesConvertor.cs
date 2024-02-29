using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers.TypesConvertors
{
    internal abstract class TypesConvertor
    {
        //Converts types from C# in correct analog in DBMS
        public abstract string ConvertType(string dataType);
    }
}
