using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{

    public class PDFReportGenerator : IReportGenerator
    {
        public string Generate<T>(List<T> data)
        {
            string result = "=== PDF REPORT ===\n";
            foreach (var item in data)
            {
                result += item.ToString() + "\n";
            }
            return result;
        }
    }

}