using ETL_ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ETL_ConsoleApp.Services
{
    class FileCheckerService
    {
        private DateTime _curentDate;
        private int _checkedFilesCounter;
        private int _checkedLinesCounter;
        private int _invalidLinesCounter;
        List<string> checkedTxt;
        StartConfigWays _ways;
        ReadedFilesService _readed;

        public FileCheckerService()
        {
            _curentDate = DateTime.Today;
            _checkedFilesCounter = 0;
            _checkedLinesCounter = 0;
            _invalidLinesCounter = 0;
            checkedTxt = new List<string>();
        }
        public KeyValuePair<bool, string> Create()
        {
            StartConfigWays startConfigWays = new StartConfigWays();
            using (StreamReader streamReader = new StreamReader(DirectoryService.GetConfigFilePath()))
            {
                string config = streamReader.ReadToEnd();
                if (config == "") return new KeyValuePair<bool, string>(false, "Config file is empty");
                this._ways = new StartConfigWays();
                _ways = (StartConfigWays)JsonSerializer.Deserialize(config, _ways.GetType(), null);
                if(_ways.InputFilesFolderWay == "") return new KeyValuePair<bool, string>(false, "Input files way wasn`t set");
                if(_ways.OutputFilesFolderWay == "") return new KeyValuePair<bool, string>(false, "Output files way wasn`t set");
                if(!Directory.Exists(_ways.InputFilesFolderWay)) return new KeyValuePair<bool, string>(false, "Directory for input files doesn`t exist");
                if(!Directory.Exists(_ways.InputFilesFolderWay)) return new KeyValuePair<bool, string>(false, "Directory for output files doesn`t exist");
                
            }
            try
            {
                _readed = new ReadedFilesService(_ways.OutputFilesFolderWay + @"\readed.json");
                checkedTxt = _readed.GetReadedTxt();
            } catch(Exception ex)
            {
                return new KeyValuePair<bool, string>(false, ex.Message);
            }
            return new KeyValuePair<bool, string>(true, "started");
        }

        public void Pulse()
        {
            if(_curentDate != DateTime.Today)
            {
                CreateDayReport();
            }
            CheckForNewTxtFiles();
        }

        public void CreateDayReport()
        {

        }

        public async void CheckForNewTxtFiles()
        {
            List<string> txtfiles = Directory.GetFiles(_ways.InputFilesFolderWay, "*.txt").ToList();
            List<string> concat = txtfiles.Except(checkedTxt).ToList();
            if(concat.Count > 0)
            {
                foreach(string way in concat)
                {
                    Console.WriteLine(way);
                    checkedTxt.Add(way);
                    await _readed.AddToProccessedAsync(new ReadedFileRecord()
                    {
                        Date = DateTime.Now,
                        Way = way
                    });
                }
            }
        }
    }
}
