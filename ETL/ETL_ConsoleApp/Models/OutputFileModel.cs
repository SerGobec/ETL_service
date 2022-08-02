using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL_ConsoleApp.Models
{
    public class OutputFileModel
    {
        public FileReport fileReport { get; set; }
        public string OutputDirWay { get; set; }
        public long FileNumber { get; set; }
        public DateTime Date { get; set; }
    }
}
