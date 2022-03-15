using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipmentApp.Interfaces
{
    public interface IOutputService
    {
        public void Print(string message);
        public void Reset();
    }
}
