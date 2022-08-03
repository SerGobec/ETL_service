using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL_ConsoleApp.Interfaces
{
    interface ITxtChecker : IChecker
    {
        public void CheckForNewTxtFiles();
    }
}
