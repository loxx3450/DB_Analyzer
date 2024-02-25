using DB_Analyzer.Exceptions.ReportSaverExceptions.TypesHandlerExceptions;
using DB_Analyzer.ReportItems;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers.TypesHandler
{
    internal static class TypesHandler
    {
        // === Getting Value Type/Description ===
        // Scalar types Values
        public static Type GetScalarValueType(ReportItem reportItem)
        {
            if (!IsScalarValueType(reportItem))
                throw new TypesHandlerException("Invalid argument by TypesHandler", new ArgumentException());

            return GetTypeOfReportItem(reportItem);
        }

        public static string GetScalarValueTypeDescription(ReportItem reportItem)
        {
            return GetScalarValueType(reportItem).Name.ToLower();
        }

        //Reference types Values
        public static Type GetReferenceValueType(ReportItem reportItem)
        {
            if (!IsReferenceValueType(reportItem))
                throw new TypesHandlerException("Invalid argument by TypesHandler", new ArgumentException());

            return GetTypeOfReportItem(reportItem);
        }

        public static string GetReferenceValueTypeDescription(ReportItem reportItem)
        {
            Type type = GetReferenceValueType(reportItem);

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>) && type.GetGenericArguments()[0] == typeof(string)) { return "List<string>"; }

            throw new TypesHandlerException("Unknown data type...", new ArgumentException());
        }

        //DataTable Values
        public static Type GetDataColumnValueType(DataColumn column)
        {
            return column.DataType;
        }

        public static string GetDataColumnValueTypeDescription(DataColumn column)
        {
            return GetDataColumnValueType(column).Name.ToLower();
        }

        //  === Checking ValueTypes ===
        // Scalar types Values
        public static bool IsScalarValueType(ReportItem reportItem)        
        {
            return GetTypeOfReportItem(reportItem).IsPrimitive;
        }

        public static bool IsScalarValueType(Type type)
        {
            return type.IsPrimitive;
        }

        //Reference types Values
        public static bool IsReferenceValueType(ReportItem reportItem)
        {
            return !IsDataTableValueType(reportItem) && !IsScalarValueType(reportItem);
        }

        public static bool IsReferenceValueType(Type type)
        {
            return !IsDataTableValueType(type) && !IsScalarValueType(type);
        }

        //DataTable Values
        public static bool IsDataTableValueType(ReportItem reportItem)
        {
            return GetTypeOfReportItem(reportItem) == typeof(DataTable);
        }

        public static bool IsDataTableValueType(Type type)
        {
            return type == typeof(DataTable);
        }

        //Global
        private static Type GetTypeOfReportItem(ReportItem reportItem)
        {
            foreach (Type iface in reportItem.GetType().GetInterfaces())
            {
                if (iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(IReportItem<>))
                {
                    if (iface.GetGenericArguments().Length != 0)
                    {
                        return iface.GetGenericArguments()[0];
                    }

                    break;
                }
            }

            return null;
        }
    }
}
