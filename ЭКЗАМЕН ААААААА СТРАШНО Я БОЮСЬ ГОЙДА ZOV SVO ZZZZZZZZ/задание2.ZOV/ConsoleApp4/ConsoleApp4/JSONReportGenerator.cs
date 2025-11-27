using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{

    public class JSONReportGenerator : IReportGenerator
    {
        public string Generate<T>(List<T> data)
        {
            var list = new List<string>();

            foreach (var item in data)
            {
                // Экранируем кавычки
                string value = item.ToString().Replace("\"", "\\\"");
                list.Add("\"" + value + "\"");
            }

            return "[\n  " + string.Join(",\n  ", list) + "\n]";
        }
    }

}
