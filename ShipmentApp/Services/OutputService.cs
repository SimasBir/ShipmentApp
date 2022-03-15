using ShipmentApp.Interfaces;
using System;
using System.IO;

namespace ShipmentApp.Services
{
    public class OutputService : IOutputService
    {
        public void Print(string message)
        {
            //File.AppendAllLines("D:/~Github/C#/ShipmentApp/ShipmentApp/Data/Output.txt", new[] { message });
            File.AppendAllText("Data/Output.txt",  message + Environment.NewLine);
        }
        public void Reset()
        {
            File.WriteAllText("Data/Output.txt", "");
        }
    }
}
