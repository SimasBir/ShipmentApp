using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipmentApp.Models
{
    public class PricingInfo
    {
        public string Provider { get; set; }
        public string PackageSize { get; set; }
        public decimal Price { get; set; }
    }
}
