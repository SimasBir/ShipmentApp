using ShipmentApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipmentApp.Services
{
    public class ConsoleService : IOutputService
    {
        public void Print(string message)
        {
            Console.WriteLine(message);
        }   
        public void Reset()
        {
        }
    }
}
