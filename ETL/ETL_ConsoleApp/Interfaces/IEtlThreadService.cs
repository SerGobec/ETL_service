using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL_ConsoleApp.Interfaces
{
    interface IEtlThreadService
    {
        string Start();
        string Stop();
        string Restart();
        void Exit();
    }
}
