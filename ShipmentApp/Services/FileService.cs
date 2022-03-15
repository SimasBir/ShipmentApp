using ShipmentApp.Interfaces;
using ShipmentApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ShipmentApp.Services
{
    public class FileService : IFileService
    {
        public async Task<string[]> ImportFileAsync(string name)
        {
            if (name == "")
            {
                name = "Input";
            }
            var lines = await File.ReadAllLinesAsync($"Data/{name}.txt");
            return lines;
        }
        public List<PricingInfo> LoadPricingInfo()
        {
            var lines = File.ReadAllLines("Data/PricingInfo.txt");
            List<PricingInfo> pricingList = new List<PricingInfo>();
            foreach (var line in lines)
            {
                string[] info = line.Split(" ");

                var provider = info[0];
                var size = info[1];
                var price =  decimal.Parse(info[2]);

                pricingList.Add(new PricingInfo
                {
                    Provider = provider,
                    PackageSize = size,
                    Price = price,
                });
            }         
            return pricingList;
        }
    }
}
