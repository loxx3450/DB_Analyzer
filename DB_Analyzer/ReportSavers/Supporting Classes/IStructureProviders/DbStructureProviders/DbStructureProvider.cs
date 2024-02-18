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

namespace DB_Analyzer.ReportSavers.IStructureProviders.DbStructureProviders
{
    internal abstract class DbStructureProvider : IStructureProvider
    {
        protected DbConnection Connection { get; set; }
        protected TypesConvertor TypesConvertor { get; set; }

        public DbStructureProvider(DbConnection connection)
        {
            Connection = connection;
        }

        public virtual async Task ProvideStructure(List<IReportItem<object>> reportItems)
        {
            await ProvideDefaultStructure();

            await ProvideExtendedStructure(reportItems);
        }

        protected abstract Task ProvideDefaultStructure();

        private async Task ProvideExtendedStructure(List<IReportItem<object>> reportItems)
        {
            foreach (var reportItem in reportItems)
            {
                if (TypesHandler.TypesHandler.IsScalarValueType(reportItem.GetValueType()))
                {
                    await ProvideExtendedStructureForScalarValue(reportItem);
                }
                else if (TypesHandler.TypesHandler.IsReferenceValueType(reportItem.GetValueType()))
                {
                    await ProvideExtendedStructureForReferenceValue(reportItem);
                }
                else if (TypesHandler.TypesHandler.IsDataTableValueType(reportItem.GetValueType()))
                {
                    await ProvideExtendedStructureForDataTable(reportItem);
                }
            }
        }

        protected abstract Task ProvideExtendedStructureForReferenceValue(IReportItem<object> reportItem);

        protected abstract Task ProvideExtendedStructureForScalarValue(IReportItem<object> reportItem);

        protected abstract Task ProvideExtendedStructureForDataTable(IReportItem<object> reportItem);

        protected abstract Task<bool> Exists(string query);

        protected abstract Task ExecuteNonQueryAsync(string query);
    }
}
