using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipmentApp.Models
{
    public class Transaction
    {
        public string Date { get; set; }
        public string PackageSize { get; set; }
        public string Provider { get; set; }
        public decimal Price { get; set; } = decimal.Zero;
        public decimal Discount { get; set; } = decimal.Zero;
        public bool Valid { get; set; } = true;
    }
}
