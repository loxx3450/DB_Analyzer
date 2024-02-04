using DB_Analyzer.Analyzers;
using DB_Analyzer.Managers;
using DB_Analyzer.ReportItems;
using Microsoft.Data.SqlClient;
using System.Collections;
using System.ComponentModel.DataAnnotations;

string connString = @"Server=(localdb)\MSSQLLocalDB;Database=portal_db;Trusted_Connection=True;Encrypt=False";

SqlServerManager manager = new(connString);

await manager.ConnectToDBAsync();

NumberOfTablesReportItem item = new();

List<IReportItem<object>> reportItems = new();
reportItems.Add(new NumberOfTablesReportItem());
reportItems.Add(new TablesNamesReportItem());

await manager.Analyze(reportItems);

//reportItems.ForEach(reportItem => Console.WriteLine(reportItem.Value));

Console.WriteLine(reportItems[0].Value);
List<string> strings = (reportItems[1] as TablesNamesReportItem).Value;

strings.ForEach(item => Console.WriteLine(item));

#region reportItems

//List<IReportItem<object>> list = new List<IReportItem<object>>();
//list.Add(new IntReportItem());
//list.Add(new StringReportItem());

//list.ForEach(item => item.Execute());

//list.ForEach(item => Console.WriteLine(item.Value));

//int wow = (int)(list[0] as IntReportItem).Value;

//Console.WriteLine(wow);


//class ScalarValue<T> where T : struct
//{
//    public T Value { get; set; }


//    public static explicit operator T(ScalarValue<T> sv)
//    {
//        return sv.Value;
//    }

//    public static implicit operator ScalarValue<T>(T sv)
//    {
//        return new ScalarValue<T>() { Value = sv };
//    }

//    public override string ToString()
//    {
//        return Value.ToString();
//    }
//}

//interface IReportItem<out T> where T : class
//{
//    T Value { get; }
//    void Execute();
//}


//class IntReportItem : IReportItem<ScalarValue<int>>
//{
//    public ScalarValue<int> Value { get; private set; }

//    public void Execute()
//    {
//        Value = new ScalarValue<int> { Value = 12 };
//    }
//}

//class StringReportItem : IReportItem<string>
//{
//    public string Value { get; private set; }

//    public void Execute()
//    {
//        Value = "this is string";
//    }
//}
#endregion