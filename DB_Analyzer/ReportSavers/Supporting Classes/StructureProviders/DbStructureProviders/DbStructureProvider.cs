using DB_Analyzer.Exceptions.ReportSaverExceptions;
using DB_Analyzer.ReportItems;
using DB_Analyzer.ReportSavers.TypesConvertors;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers.StructureProviders.DbStructureProviders
{
    internal abstract class DbStructureProvider : IStructureProvider
    {
        //Global
        protected DbConnection Connection { get; set; }
        protected TypesConvertor TypesConvertor { get; set; }

        //Flags
        protected bool DefaultStructureIsProvided { get; set; }

        public DbStructureProvider(DbConnection connection)
        {
            Connection = connection;
        }

        public async Task ProvideStructure(List<ReportItem> reportItems)
        {
            if (!DefaultStructureIsProvided)
                await ProvideDefaultStructure();

            await ProvideExtendedStructure(reportItems);
        }

        protected abstract Task ProvideDefaultStructure();

        protected abstract Task CreateTableIfNotExists(string dbName, string parameters);

        private Task ProvideExtendedStructure(List<ReportItem> reportItems)
        {
            return Parallel.ForEachAsync(reportItems, async (reportItem, state) =>
            {
                if (TypesHandler.TypesHandler.IsScalarValueType(reportItem))
                {
                    await ProvideExtendedStructureForScalarValue(reportItem);
                }
                else if (TypesHandler.TypesHandler.IsReferenceValueType(reportItem))
                {
                    await ProvideExtendedStructureForReferenceValue(reportItem);
                }
                else if (TypesHandler.TypesHandler.IsDataTableValueType(reportItem))
                {
                    await ProvideExtendedStructureForDataTable(reportItem);
                }
            });
        }

        protected abstract Task ProvideExtendedStructureForScalarValue(ReportItem reportItem);

        protected abstract Task ProvideExtendedStructureForReferenceValue(ReportItem reportItem);

        protected abstract Task ProvideExtendedStructureForDataTable(ReportItem reportItem);

        protected abstract Task<bool> Exists(string query);

        protected abstract Task ExecuteNonQueryAsync(string query);
    }
}
