using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    public class HTMLReportGenerator : IReportGenerator
    {
        public string Generate<T>(List<T> data)
        {
            string result = "<html><body><h2>HTML Report</h2><ul>";

            foreach (var item in data)
            {
                result += "<li>" + item.ToString() + "</li>";
            }

            result += "</ul></body></html>";
            return result;
        }
    }


}
