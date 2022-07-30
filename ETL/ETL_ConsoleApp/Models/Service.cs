using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL_ConsoleApp.Models
{
    class Service
    {
        public string Name{get;set;}
        public List<Payer> Payers { get; set; }
        public decimal Total { get; set; }

        public void AddPayer(Payer payer)
        {
            this.Payers.Add(payer);
            this.Total += payer.Payment;
        }

        public Service()
        {
            Payers = new List<Payer>();
        }
    }
}
