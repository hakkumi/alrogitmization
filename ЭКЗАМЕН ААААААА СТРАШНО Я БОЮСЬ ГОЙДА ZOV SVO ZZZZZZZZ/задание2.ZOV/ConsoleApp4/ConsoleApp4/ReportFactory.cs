using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{

    public static class ReportFactory
    {
        public static IReportGenerator Create(string type)
        {
            type = type.ToLower();

            if (type == "pdf")
                return new PDFReportGenerator();
            if (type == "excel")
                return new ExcelReportGenerator();
            if (type == "html")
                return new HTMLReportGenerator();
            if (type == "json")
                return new JSONReportGenerator();

            throw new ArgumentException("Unknown report type: " + type);
        }
    }

}
