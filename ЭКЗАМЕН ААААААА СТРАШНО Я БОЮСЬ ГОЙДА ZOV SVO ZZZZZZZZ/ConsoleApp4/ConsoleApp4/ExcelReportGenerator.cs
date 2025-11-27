using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{

    public class ExcelReportGenerator : IReportGenerator
    {
        public string Generate<T>(List<T> data)
        {
            string result = "=== EXCEL REPORT ===\n";

            foreach (var item in data)
            {
                result += item.ToString() + "\t|\n";
            }

            return result;
        }
    }


}
