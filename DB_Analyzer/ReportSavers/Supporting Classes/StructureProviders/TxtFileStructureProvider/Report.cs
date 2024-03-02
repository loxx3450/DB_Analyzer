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
    //Object which contains results of ReportItems and could be easily serialized
    public class Report
    {
        public string DbmsName { get; set; }

        public string ServerName { get; set; }

        public string DbName { get; set; }

        public DateTime CreationDate { get; set; }

        public Dictionary<string, int> CollectionOfIntegers { get; set; }

        public Dictionary<string, List<string>> CollectionOfStringLists { get; set; }

        public Dictionary<string, DataTable> CollectionOfDataTables { get; set; }

        public Report()
        {
            DbName = string.Empty;

            CreationDate = DateTime.MinValue;

            CollectionOfIntegers = new Dictionary<string, int>();
            CollectionOfStringLists = new Dictionary<string, List<string>>();
            CollectionOfDataTables = new Dictionary<string, DataTable>();
        }
    }
}
