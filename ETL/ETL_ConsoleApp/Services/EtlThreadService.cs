using ETL_ConsoleApp.Interfaces;
using ETL_ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
                StartConfigWays startConfigWays = new StartConfigWays();
                using (StreamReader streamReader = new StreamReader(DirectoryService.GetConfigFilePath()))
                {
                    string config = streamReader.ReadToEnd();
                    if (config == "") return "Config file is empty";
                    StartConfigWays ways2 = new StartConfigWays();
                    ways2 = (StartConfigWays)JsonSerializer.Deserialize(config, ways2.GetType(), null);
                    
                    Console.WriteLine(ways2.InputFilesFolderWay);
                    Console.WriteLine(ways2.OutputFilesFolderWay);
                }
                if (_thread != null && _thread.IsAlive) return "System already works";
                stream = File.OpenRead(DirectoryService.GetConfigFilePath());
                _thread = new Thread(Action);
                _thread.Start();
                return "Thread succesfully started!";
            } catch (FileNotFoundException)
            {
                return "Config file was not found. System NOT STARTED!";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                if(stream != null) stream.Close();
            }
        }

        public string Stop()
        {
            try
            {
                cancelTokenSource.Cancel();
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

        public void Action()
        {
            
            while (true)
            {
                if (token.IsCancellationRequested) return;
                Thread.Sleep(1000);
            }
        }
    }
}
