using ETL_ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL_ConsoleApp.Interfaces
{
    interface IEtlThreadService
    {
        KeyValuePair<MessageTypeResult, string> Start();
        KeyValuePair<MessageTypeResult, string> Stop();
        KeyValuePair<MessageTypeResult, string> Restart();
        void Exit();
    }
}
