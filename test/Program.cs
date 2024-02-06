using DB_Analyzer.Analyzers;
using DB_Analyzer.Managers;
using DB_Analyzer.ReportItems;
using Microsoft.Data.SqlClient;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Data;

#region main

string connString = @"Server=(localdb)\MSSQLLocalDB;Database=portal_db;Trusted_Connection=True;Encrypt=False";
SqlServerManager manager = new(connString);


//string connString = @"Server=127.0.0.1;port=3306;uid=root;Database=portal_db";
//MySqlManager manager = new(connString);

await manager.ConnectToDBAsync();



List<IReportItem<object>> reportItems = new()
{
    new NumberOfTablesReportItem(),
    new TablesNamesReportItem(),
    new NumberOfColumnsReportItem("users"),
    new ColumnsNamesReportItem("users"),
    new NumberOfStoredProceduresReportItem(),
    new StoredProceduresNamesReportItem()
};


await manager.Analyze(reportItems);



Console.WriteLine(reportItems[0].Value);
int wow = ((NumberOfTablesReportItem)reportItems[0]).Value;

List<string> strings = ((TablesNamesReportItem)reportItems[1]).Value;
strings.ForEach(item => Console.WriteLine(item));

Console.WriteLine(reportItems[2].Value);

strings = ((ColumnsNamesReportItem)reportItems[3]).Value;
strings.ForEach(item => Console.WriteLine(item));

Console.WriteLine(reportItems[4].Value);

strings = ((StoredProceduresNamesReportItem)reportItems[5]).Value;
strings.ForEach(item => Console.WriteLine(item));

#endregion

#region example

////string connString = @"Server=(localdb)\MSSQLLocalDB;Database=portal_db;Trusted_Connection=True;Encrypt=False";
//string connString = @"Server=127.0.0.1;port=3306;uid=root;Database=portal_db";

//MySqlManager manager = new(connString);

//await manager.ConnectToDBAsync();

//NumberOfTablesReportItem item = new();

//List<IReportItem<object>> reportItems = new();
//reportItems.Add(new NumberOfTablesReportItem());
//reportItems.Add(new TablesNamesReportItem());

//await manager.Analyze(reportItems);

//int tablesCount = (int)(reportItems[0] as NumberOfTablesReportItem).Value;
//List<string> tablesNames = (reportItems[1] as TablesNamesReportItem).Value;

//Console.WriteLine(tablesCount);

//for (int i = 0; i < tablesCount; i++)
//{
//    reportItems.Clear();

//    Console.WriteLine(tablesNames[i]);

//    reportItems.Add(new NumberOfColumnsReportItem(tablesNames[i]));
//    reportItems.Add(new ColumnsNamesReportItem(tablesNames[i]));

//    Console.WriteLine();

//    await manager.Analyze(reportItems);

//    int columnsCount = (int)(reportItems[0] as NumberOfColumnsReportItem).Value;
//    List<string> columnsNames = (reportItems[1] as ColumnsNamesReportItem).Value;

//    for (int j = 0; j < columnsCount; j++)
//    {
//        Console.WriteLine(columnsNames[j]);
//    }

//    Console.WriteLine();
//}

#endregion

#region reportItems

//List<IReportItem<object>> list = new List<IReportItem<object>>();
//list.Add(new IntReportItem());
//list.Add(new StringReportItem());

//list.ForEach(item => item.Execute());

//list.ForEach(item => Console.WriteLine(item.Value));

//int wow = (list[0] as IntReportItem).Value;

//Console.WriteLine(wow);


//class ScalarValue<T> where T : struct
//{
//    public T Value { get; set; }


//    //public static explicit operator T(ScalarValue<T> sv)
//    //{
//    //    return sv.Value;
//    //}

//    public static implicit operator T(ScalarValue<T> sv)
//    {
//        return sv.Value;
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