using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL_ConsoleApp.Models
{
    public class Transaction
    {
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Address { get; set; }
        public Decimal Payment { get; set; }
        public DateTime Date { get; set; }
        public long Account_number { get; set; }
        public string Service { get; set; }
    }
}
