using ETL_ConsoleApp.Interfaces;
using ETL_ConsoleApp.Models;
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
                    "\n3. Restart system" +
                    "\n0. Exit" +
                    "\n|" +
                    "\n|___-> ";
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
                Console.Write(introduction);
                string answ = Console.ReadLine();
                int num;
                Console.Clear();
                if(int.TryParse(answ, out num))
                {
                    KeyValuePair<MessageTypeResult, string> result = default;
                    switch (num)
                    {
                        case 1:
                            result = _etlService.Start();
                            WriteMessage(result.Value, result.Key);
                            break;
                        case 2:
                            result = _etlService.Stop();
                            WriteMessage(result.Value, result.Key);
                            break;
                        case 3:
                            result = _etlService.Restart();
                            WriteMessage(result.Value, result.Key);
                            break;
                        case 0:
                            _etlService.Exit();
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
            
            Console.WriteLine("\nResult: " + message + "\n");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void WriteMessage(string message, MessageTypeResult result)
        {
            if(result == MessageTypeResult.Fault) Console.ForegroundColor = ConsoleColor.Red;
            if(result == MessageTypeResult.Success) Console.ForegroundColor = ConsoleColor.Green;
            if (result == MessageTypeResult.Notification) Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nResult: " + message + "\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void Dispose()
        {
            _etlService = null;
        }
    }
}
