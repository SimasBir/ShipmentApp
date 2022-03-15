using ShipmentApp.Models;
using System;
using System.Collections.Generic;

namespace ShipmentApp.Services
{
    public class ModelService
    {
        public List<Transaction> ModelChange(string[] lines)
        {
            List<Transaction> list = new List<Transaction>();
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                string[] info = line.Split(" ");
                var dateText = info[0];
                if (info.Length != 3)
                {
                    dateText = line;
                }
                try
                {                   
                    DateTime.ParseExact(dateText, "yyyy-MM-dd", null);
                    var size = info[1];
                    var provider = info[2];

                    list.Add(new Transaction
                    {
                        Date = dateText,
                        PackageSize = size,
                        Provider = provider,
                    });
                }
                catch {
                    list.Add(new Transaction
                    {
                        Date = dateText,
                        Valid = false
                    });
                };
            }
            return list;

        }
    }
}
