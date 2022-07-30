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
    class ProccessedFilesService
    {
        readonly string fileWay;
        object locker = new object();
        List<ReadedFileRecord> records;
        public ProccessedFilesService(string fileWay)
        {
            if (File.Exists(fileWay))
            {
                this.fileWay = fileWay;
                using (StreamReader streamReader = new StreamReader(fileWay))
                {
                    string config = streamReader.ReadToEnd();
                    records = new List<ReadedFileRecord>();
                    if (config != "") records = (List<ReadedFileRecord>)JsonSerializer.Deserialize(config, records.GetType(), null);
                }
            } else
            {
                records = new List<ReadedFileRecord>();
                lock (locker)
                {
                    using (FileStream fileStream = File.Create(fileWay)) { }
                    this.fileWay = fileWay;
                }
            }
           
        }
        public async Task AddToProccessedAsync(ReadedFileRecord record)
        {
            
            await Task.Run(() =>
            {
                lock (locker)
                {
                    records.Add(record);
                    string json = JsonSerializer.Serialize(records, records.GetType());
                    using (StreamWriter streamWriter = new StreamWriter(fileWay, false))
                    {
                        streamWriter.Write(json);
                    }
                }
            });
        }

        public List<ReadedFileRecord> GetListReadedFiles()
        {
            return records;
        }

        public List<string> GetReadedTxt()
        {
            return records.Where(el => el.Way.EndsWith(".txt")).Select(el => el.Way).ToList();
        }

        public List<string> GetReadedCsv()
        {
            return records.Where(el => el.Way.EndsWith(".csv")).Select(el => el.Way).ToList();
        }
    }
}
