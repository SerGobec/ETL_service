using ETL_ConsoleApp.Models;
using ETL_ConsoleApp.Services;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ETL_ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleService consoleService = new ConsoleService();
            consoleService.Start();
            /*Console.WriteLine("Hello World!");
            StartConfigWays ways = new StartConfigWays();
            ways.InputFilesFolderWay = TryGetSolutionDirectoryInfo().FullName + @"\INPUT_FILES";
            ways.OutputFilesFolderWay = TryGetSolutionDirectoryInfo().FullName + @"\OUTPUT_FILES";
            string serialised = JsonSerializer.Serialize(ways, ways.GetType());
             DirectoryInfo info = TryGetSolutionDirectoryInfo();*/
            //Console.WriteLine(info.FullName + @"\configs.json");

            //Directory.CreateDirectory(ways.InputFilesFolderWay);
            //Directory.CreateDirectory(ways.OutputFilesFolderWay);
            /*using (StreamReader streamReader = new StreamReader(info.FullName + @"\configs.json"))
            {
                string test = streamReader.ReadToEnd();
                StartConfigWays ways2 = new StartConfigWays();
                try
                {
                    ways2 = (StartConfigWays)JsonSerializer.Deserialize(test, ways2.GetType(), null);
                } catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.WriteLine(ways2
                    .InputFilesFolderWay);

            }
            Console.WriteLine(serialised);*/

        }


        public static DirectoryInfo TryGetSolutionDirectoryInfo(string currentPath = null)
        {
            var directory = new DirectoryInfo(
                currentPath ?? Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory;
        }
    }
}
