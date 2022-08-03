using ETL_ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL_ConsoleApp.Interfaces
{
    public interface IProcessedFile
    {
        public Task AddToProccessedAsync(ReadedFileRecord record);
        public List<ReadedFileRecord> GetListReadedFiles();
    }
}
