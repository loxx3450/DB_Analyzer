﻿using DB_Analyzer.Exceptions.ReportSaverExceptions.TypesHandlerExceptions;
using DB_Analyzer.ReportItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers.TypesHandler
{
    internal static class TypesHandler
    {
        public static string GetScalarValueType(Type type)
        {
            if (!TypesHandler.IsScalarValueType(type))
            {
                throw new TypesHandlerException("Invalid argument by TypesHandler", new ArgumentException());
            }

            return type.GenericTypeArguments[0].Name.ToLower();
        }

        public static bool IsScalarValueType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ScalarValue<>);
        }
    }
}
