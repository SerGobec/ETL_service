using ETL_ConsoleApp.Interfaces;
using ETL_ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ETL_ConsoleApp.Services
{
    class FileCheckerService : ICsvChecker, ITxtChecker
    {
        private DateTime _currentDate;
        private long _checkedFilesCounter;
        private List<string> _invalidFilesWays;
        private long _checkedLinesCounter;
        private long _invalidLinesCounter;
        private object _locker = new object();
        
        List<string> checkedTxt;
        List<string> checkedCsv;
        StartConfigWays _ways;
        ProccessedFilesService _readed;
        ParserService _parserService;

        public FileCheckerService()
        {
            _currentDate = DateTime.Today;
            _checkedFilesCounter = 0;
            _checkedLinesCounter = 0;
            _invalidLinesCounter = 0;
            checkedTxt = new List<string>();
            checkedCsv = new List<string>();
            _parserService = new ParserService();
            _invalidFilesWays = new List<string>();
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
                _checkedFilesCounter = CurentDayProcessedFiles();
            }
            try
            {
                _readed = new ProccessedFilesService(_ways.OutputFilesFolderWay + @"\readed.json");
                checkedTxt = _readed.GetReadedTxt();
                checkedCsv = _readed.GetReadedCsv();
            } catch(Exception ex)
            {
                return new KeyValuePair<bool, string>(false, ex.Message);
            }
            return new KeyValuePair<bool, string>(true, "started");
        }

        public void Pulse()
        {
            if(_currentDate != DateTime.Today)
            {
                CreateDayReport(this._currentDate);
                _currentDate = DateTime.Today;
            }
            CheckForNewTxtFiles();
            CheckForNewCsvFiles();
        }

        public async void CreateDayReport(DateTime dateTime)
        {
            await ReportService.SaveDayReportJsonAsync(new OutputDayReportModel()
            {
                Date = dateTime,
                DirectoryWay = this._ways.OutputFilesFolderWay,
                FoundErrors = this._invalidLinesCounter,
                InvalidFiles = this._invalidFilesWays,
                ParsedFiles = this._checkedFilesCounter,
                ParsedLines = this._checkedLinesCounter
            }) ;
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
                    FileReport report = _parserService.ParseTxtToReport(way);
                    long FilesCounter;
                    lock (_locker)
                    {
                        _checkedFilesCounter += 1;
                        FilesCounter = _checkedFilesCounter;
                    }
                    _checkedLinesCounter += report.ParsedLine;
                    if(report.InvalidLine > 0)
                    {
                        _invalidLinesCounter += report.InvalidLine;
                        _invalidFilesWays.Add(way);
                    }
                    await _readed.AddToProccessedAsync(new ReadedFileRecord()
                    {
                        Date = DateTime.Now,
                        Way = way
                    });
                    await ReportService.SaveFileReportJsonAsync(new OutputFileModel()
                    {
                        Date = _currentDate,
                        FileNumber = FilesCounter,
                        fileReport = report,
                        OutputDirWay = _ways.OutputFilesFolderWay
                    });
                }
            }
        }

        public async void CheckForNewCsvFiles()
        {
            List<string> csvfiles = Directory.GetFiles(_ways.InputFilesFolderWay, "*.csv").ToList();
            List<string> concat = csvfiles.Except(checkedCsv).ToList();
            if (concat.Count > 0)
            {
                foreach (string way in concat)
                {
                    Console.WriteLine(way);
                    checkedCsv.Add(way);
                    FileReport report = _parserService.ParseCsvToReport(way);
                    long FilesCounter;
                    lock (_locker)
                    {
                        _checkedFilesCounter += 1;
                        FilesCounter = _checkedFilesCounter;
                    }
                    _checkedLinesCounter += report.ParsedLine;
                    if (report.InvalidLine > 0)
                    {
                        _invalidLinesCounter += report.InvalidLine;
                        _invalidFilesWays.Add(way);
                    }
                    await _readed.AddToProccessedAsync(new ReadedFileRecord()
                    {
                        Date = DateTime.Now,
                        Way = way
                    });
                    await ReportService.SaveFileReportJsonAsync(new OutputFileModel()
                    {
                        Date = _currentDate,
                        FileNumber = _checkedFilesCounter,
                        fileReport = report,
                        OutputDirWay = _ways.OutputFilesFolderWay
                    });
                }
            }
        }

        public int CurentDayProcessedFiles()
        {
            if (!Directory.Exists(this._ways.OutputFilesFolderWay + @"\" + this._currentDate.ToString("MM-dd-yyyy"))) return 0;
            string[] ways =  Directory.GetFiles(this._ways.OutputFilesFolderWay + @"\" + this._currentDate.ToString("MM-dd-yyyy"));
            if (ways.Length != 0) return ways.Where(el => el.EndsWith(".txt")).Count();
            return 0;
        }
    }
}
