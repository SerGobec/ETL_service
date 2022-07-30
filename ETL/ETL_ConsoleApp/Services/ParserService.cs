using ETL_ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL_ConsoleApp.Services
{
    class ParserService
    {
        public FileReport ParseTxtToObject(string way)
        {
            FileReport report = new FileReport();
            using (StreamReader reader = new StreamReader(way))
            {
                string line;
                do
                {
                    line = reader.ReadLine();
                    if (line == "" || line == null) break;
                    try
                    {
                        List<string> parts = new List<string>();
                        int level = -1;
                        string current = "";
                        /*foreach (char c in line)
                        {
                            if (c == ',' && level == -1)
                            {
                                parts.Add(current);
                                current = "";
                            }
                            if (c == '\"' || c == '\'' || c == '“' || c == '”')
                            {
                                level *= -1;
                            }
                            if(c == '\n')
                            {
                                parts.Add(current);
                            }
                            current += c;
                        }*/
                        for(int i = 0;i < line.Length; i++)
                        {
                            if (line[i] == ',' && level == -1)
                            {
                                parts.Add(current.Replace("\"", "").Replace("\'", "").Replace("“", "").Replace("”", "").Trim());
                                current = "";
                                continue;
                            }
                            if (line[i] == '\"' || line[i] == '\'' || line[i] == '“' || line[i] == '”')
                            {
                                level *= -1;
                            }
                            if (line[i] == '\n')
                            {
                                parts.Add(current.Replace("\"", "").Replace("\'", "").Replace("“", "").Replace("”", "").Trim());
                                continue;
                            }
                            current += line[i];
                            if(i == line.Length - 1)
                            {
                                parts.Add(current.Replace("\"", "").Replace("\'", "").Replace("“", "").Replace("”", "").Trim());
                            }
                        }
                        if (parts.Count < 7) throw new Exception();
                        Transaction transaction = new Transaction();
                        transaction.First_name = parts[0];
                        transaction.Last_name = parts[1];
                        transaction.Address = parts[2];
                        transaction.Payment = Convert.ToDecimal(parts[3], new CultureInfo("en-US"));
                        transaction.Date = DateTime.ParseExact(parts[4],"yyyy-dd-MM", null);
                        transaction.Account_number = long.Parse( parts[5]);
                        transaction.Service = parts[6];
                        if(transaction.First_name.Split(' ').Length == 2 && transaction.Last_name == "")
                        {
                            transaction.Last_name = transaction.First_name.Split(' ')[1];
                            transaction.First_name = transaction.First_name.Split(' ')[0];
                        }
                        Payer payer = new Payer();
                        payer.Name = transaction.First_name + " " + transaction.Last_name;
                        payer.Payment = transaction.Payment;
                        payer.Date = transaction.Date;
                        payer.Account_number = transaction.Account_number;
                        Service service = new Service();
                        service.Name = transaction.Service;
                        service.Payers.Add(payer);
                        service.Total = transaction.Payment;
                        CityReport cityReport = new CityReport();
                        cityReport.City = transaction.Address.Split(",")[0];
                        cityReport.Services.Add(service);
                        cityReport.Total = transaction.Payment;
                        if (report.CityReports.Where(el => el.City == cityReport.City).Count() == 0)
                        {
                            report.CityReports.Add(cityReport);
                        } else
                        {
                            CityReport city = report.CityReports.Where(el => el.City == cityReport.City).FirstOrDefault();
                            if(city.Services.Where(el => el.Name == service.Name).Count() == 0)
                            {
                                city.AddService(service);
                            } else
                            {
                                Service servi = city.Services.Where(el => el.Name == service.Name).FirstOrDefault();
                                servi.AddPayer(payer);
                                city.Total += payer.Payment;
                            }
                          
                        }
                    } catch
                    {
                        report.InvalidLine += 1;
                    } finally
                    {
                        report.ParsedLine += 1;
                    }
                } while (line != "");
            }
            return report;
        }
    }
}
