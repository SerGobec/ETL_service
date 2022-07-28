using ETL_ConsoleApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ETL_ConsoleApp.Services
{
    class ConsoleService : IDisposable
    {
        IEtlThreadService _etlService;
        string introduction = "Please, enter num of action:" +
                    "\n1. Start system" +
                    "\n2. Stop system" +
                    "\n3. Create default packege" +
                    "\n0. Exit";
        public ConsoleService()
        {
            _etlService = new EtlThreadService();
        }

        public void Start()
        {
            
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("-----------ETL-----------\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(introduction);
                string answ = Console.ReadLine();
                int num;
                Console.Clear();
                if(int.TryParse(answ, out num))
                {
                    switch (num)
                    {
                        case 1:
                            WriteNotifMessage(_etlService.Start());
                            break;
                        case 2:
                            WriteNotifMessage(_etlService.Stop());
                            break;
                        case 3:
                            WriteNotifMessage(_etlService.Restart());
                            break;
                        case 0:
                            return;
                        default:
                            ConsoleWriteErrorMessage("Unknown command!");
                            break;
                    }
                }
                else
                {
                    ConsoleWriteErrorMessage("Enter a number please.");
                }
            }
        }

        private void ConsoleWriteErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void WriteNotifMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nResult: " + message + "\n");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void Dispose()
        {
            _etlService = null;
        }
    }
}
