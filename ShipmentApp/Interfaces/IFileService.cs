using ShipmentApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShipmentApp.Interfaces
{
    public interface IFileService
    {
        Task<string[]> ImportFileAsync(string name);
        List<PricingInfo> LoadPricingInfo();
    }
}
