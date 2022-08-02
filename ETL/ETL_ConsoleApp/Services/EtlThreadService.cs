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
        private Thread _thread;
        private CancellationTokenSource cancelTokenSource;
        private CancellationToken token;
        FileCheckerService _fileCheckerService;

        public EtlThreadService()
        {
            
        }
        public KeyValuePair<MessageTypeResult, string> Start()
        {
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;
            if(_fileCheckerService == null)
            {
                _fileCheckerService = new FileCheckerService();
                KeyValuePair<bool,string> result = _fileCheckerService.Create();
                if (!result.Key) return new KeyValuePair<MessageTypeResult, string>(MessageTypeResult
                    .Fault, result.Value);
            }
            try
            {
                if (_thread != null && _thread.IsAlive) return new KeyValuePair<MessageTypeResult, string>(MessageTypeResult.Notification, "System already works");
                _thread = new Thread(Action);
                _thread.Start();
                return new KeyValuePair<MessageTypeResult, string>(MessageTypeResult.Success, "Thread succesfully started!");
            } catch (FileNotFoundException)
            {
                return new KeyValuePair<MessageTypeResult, string>(MessageTypeResult.Fault, "Config file was not found. System NOT STARTED!");
            }
            catch (Exception ex)
            {
                return new KeyValuePair<MessageTypeResult, string>(MessageTypeResult.Fault, ex.Message);
            }
        }

        public KeyValuePair<MessageTypeResult, string> Stop()
        {
            try
            {
                if (cancelTokenSource == null) return new KeyValuePair<MessageTypeResult, string>(MessageTypeResult.Notification, "Thread wasn`t started.");
                cancelTokenSource.Cancel();
                return new KeyValuePair<MessageTypeResult, string>(MessageTypeResult.Success, "Thread stopped");
            } catch (Exception ex)
            {
                return new KeyValuePair<MessageTypeResult, string>(MessageTypeResult.Fault, ex.Message);
            }
        }
        public KeyValuePair<MessageTypeResult, string> Restart()
        {
            Stop();
            Start();
            return new KeyValuePair<MessageTypeResult, string>(MessageTypeResult.Success, "Restarted");
        }

        public void Exit()
        {
            Stop();
            _thread = null;
            _fileCheckerService = null;
            cancelTokenSource = null;
        }

        public void Action()
        {
            
            while (true)
            {
                if (token.IsCancellationRequested) return;
                _fileCheckerService.Pulse();
                Thread.Sleep(1000);
            }
        }
    }
}
