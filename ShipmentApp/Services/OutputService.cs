using ShipmentApp.Interfaces;
using System;
using System.IO;

namespace ShipmentApp.Services
{
    public class OutputService : IOutputService
    {
        public void Print(string message)
        {
            File.AppendAllText("Data/Output.txt",  message + Environment.NewLine);
        }
        public void Reset()
        {
            File.WriteAllText("Data/Output.txt", "");
        }
    }
}
