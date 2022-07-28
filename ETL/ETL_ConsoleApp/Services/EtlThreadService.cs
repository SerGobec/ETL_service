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
    class EtlThreadService : IEtlThreadService
    {
        Thread _thread;
        private CancellationTokenSource cancelTokenSource;
        private CancellationToken token;
        public string Start()
        {
            FileStream stream = null;
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;
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
        }

        public string Stop()
        {
            try
            {
                cancelTokenSource.Cancel();
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
                if (token.IsCancellationRequested) return;
                Console.WriteLine("Hello");
                Thread.Sleep(1000);
            }
        }
    }
}
