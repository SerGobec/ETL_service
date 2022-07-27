using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ETL_ConsoleApp.Services
{
    static class DirectoryService
    {
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

        public static string GetConfigFilePath()
        {
            DirectoryInfo directoryInfo = TryGetSolutionDirectoryInfo();
            return directoryInfo.FullName + @"\configs.json";
        }
    }
}
