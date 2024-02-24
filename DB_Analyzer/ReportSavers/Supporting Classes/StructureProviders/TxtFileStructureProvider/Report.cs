using DB_Analyzer.ReportItems;
using Mysqlx.Datatypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DB_Analyzer.ReportSavers.Supporting_Classes.StructureProviders.TxtFileStructureProvider
{
    [Serializable]
    internal class Report
    {
        public string DbName { get; set; }

        public DateTime CreationDate { get; set; }

        public List<int> CollectionOfIntegers { get; set; }

        public List<List<string>> CollectionOfStringLists { get; set; }

        public List<DataTable> CollectionOfDataTables { get; set; }

        public Report()
        { 
            DbName = string.Empty;

            CreationDate = DateTime.MinValue;

            CollectionOfIntegers = new List<int>(); 
            CollectionOfStringLists = new List<List<string>>();
            CollectionOfDataTables = new List<DataTable>();
        }
    }
}
