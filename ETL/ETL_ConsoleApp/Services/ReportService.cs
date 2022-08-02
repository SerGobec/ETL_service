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
    class ReportService
    {
        public static async Task SaveFileReportJsonAsync(OutputFileModel model)
        {
            await Task.Run(() =>
            {
                string directory = model.OutputDirWay + @"\" + model.Date.ToString("MM-dd-yyyy");
                if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
                string json = JsonSerializer.Serialize(model.fileReport.CityReports, model.fileReport.CityReports.GetType());
                using (StreamWriter writer = new StreamWriter(directory + @"\Output" + model.FileNumber + ".txt"))
                {
                    writer.Write(json);
                }
            });
        }

        public static async Task SaveDayReportJsonAsync(OutputDayReportModel model)
        {
            await Task.Run(() =>
            {
                string way = model.DirectoryWay + @"\" + model.Date.ToString("MM-dd-yyyy") + @"\" + "meta.log";
                using (StreamWriter writer = new StreamWriter(way))
                {
                    writer.WriteLine("Parsed_files: " + model.ParsedFiles);
                    writer.WriteLine("Parsed_lines: " + model.ParsedLines);
                    writer.WriteLine("Fouund_errors: " + model.FoundErrors);
                    string json = JsonSerializer.Serialize(model.InvalidFiles, model.InvalidFiles.GetType());
                    writer.Write("Invalid files: [");
                    for(int i = 0; i < model.InvalidFiles.Count; i++)
                    {
                        writer.Write(model.InvalidFiles[i]);
                        if (i != model.InvalidFiles.Count - 1) writer.Write(", ");
                    }
                    writer.Write("]");
                }
            });
        }
    }
}
