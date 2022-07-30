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

        public void AddService(Service service)
        {
            this.Services.Add(service);
            this.Total += service.Total;
        }

        public CityReport()
        {
            this.Services = new List<Service>();
        }
    }
}
