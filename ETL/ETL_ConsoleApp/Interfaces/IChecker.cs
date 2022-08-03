using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL_ConsoleApp.Interfaces
{
    interface IChecker
    {
        public void Pulse();
        public void CreateDayReport(DateTime dateTime);
        public int CurentDayProcessedFiles();
    }
}
