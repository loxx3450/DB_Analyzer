using DB_Analyzer.ReportItems;
using DB_Analyzer.ReportSavers.StructureProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers.Supporting_Classes.StructureProviders.TxtFileStructureProvider
{
    //internal class TxtFileStructureProvider : IStructureProvider
    //{
    //    public Task<Report> ProvideStructure(List<IReportItem<object>> reportItems)
    //    {
    //        Report report = new Report();

    //        foreach (var reportItem in reportItems)
    //        {
    //            if (TypesHandler.TypesHandler.IsScalarValueType(reportItem.GetValueType()))
    //            {
    //                await ProvideExtendedStructureForScalarValue(reportItem);
    //            }
    //            else if (TypesHandler.TypesHandler.IsReferenceValueType(reportItem.GetValueType()))
    //            {
    //                await ProvideExtendedStructureForReferenceValue(reportItem);
    //            }
    //            else if (TypesHandler.TypesHandler.IsDataTableValueType(reportItem.GetValueType()))
    //            {
    //                await ProvideExtendedStructureForDataTable(reportItem);
    //            }
    //        }
    //    }
    //}
}
