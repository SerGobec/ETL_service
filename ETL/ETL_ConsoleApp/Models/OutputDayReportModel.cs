using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL_ConsoleApp.Models
{
    class OutputDayReportModel
    {
        public long ParsedFiles{get;set;}
        public long ParsedLines{get;set;}
        public long FoundErrors{get;set;}
        public List<string> InvalidFiles { get; set; }
        public DateTime Date { get; set; }
        public string DirectoryWay { get; set; }
        public OutputDayReportModel()
        {
            this.InvalidFiles = new List<string>();
        }
    }
}
