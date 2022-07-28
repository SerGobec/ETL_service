using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL_ConsoleApp.Models
{
    class CityReport
    {
        public string City { get; set; }
        public List<Service> Services { get; set; }
        public decimal Total { get; set; }
    }
}
