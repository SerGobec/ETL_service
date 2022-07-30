using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL_ConsoleApp.Models
{
    class FileReport
    {
        public List<CityReport> CityReports { get; set; }
        public long ParsedLine { get; set; }
        public long InvalidLine { get; set; }

        public FileReport()
        {
            CityReports = new List<CityReport>();
            ParsedLine = 0;
            InvalidLine = 0;
        }
    }
}
