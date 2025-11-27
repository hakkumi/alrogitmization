using ConsoleApp4;
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        List<Person> people = new List<Person>()
        {
            new Person { Name="Гавноед", Age=25 },
            new Person { Name="Святослав можжевельник", Age=30 },
            new Person { Name="Наталья землеройка", Age=28 }
        };

        TestFormat("pdf", people);
        TestFormat("excel", people);
        TestFormat("html", people);
        TestFormat("json", people);
    }

    static void TestFormat<T>(string type, List<T> data)
    {
        IReportGenerator generator = ReportFactory.Create(type);
        string report = generator.Generate(data);

        Console.WriteLine("\n=== " + type.ToUpper() + " OUTPUT ===");
        Console.WriteLine(report);
    }
}
