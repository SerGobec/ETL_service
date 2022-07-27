using ETL_ConsoleApp.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ETL_ConsoleApp.Services
{
    class EtlService : IEtlService
    {
        Thread _thread;

        public string Start()
        {
            FileStream stream = null;
            try
            {
                if (_thread != null && _thread.IsAlive) return "System already works";
                stream = File.OpenRead(DirectoryService.GetConfigFilePath());
                Console.WriteLine("Thread started");
                _thread = new Thread(Checking);
                _thread.Start();
                return "Thread succesfully started!";
            } catch (Exception ex)
            {
                return ex.Message;
            } finally
            {
                if(stream != null) stream.Close();
            }

            
            throw new NotImplementedException();
        }

        public string Stop()
        {
            try
            {
                _thread.Abort();
                Console.WriteLine("Stopped");
                return "Thread stopped";
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }
        public string Restart()
        {
            Stop();
            Start();
            return "Restarted";
        }

        public void Checking()
        {
            while (true)
            {
                Console.WriteLine("Hello");
                Thread.Sleep(1000);
                return;
            }
        }
    }
}
