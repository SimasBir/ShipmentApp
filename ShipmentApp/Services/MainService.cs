using ShipmentApp.Interfaces;
using System.Threading.Tasks;

namespace ShipmentApp.Services
{
    public class MainService
    {
        private IOutputService _outputService;
        private ModelService _modelService;
        private PriceService _priceService;
        private IFileService _fileService;

        public MainService(IOutputService outputService, ModelService modelService, PriceService priceService, IFileService fileService)
        {
            _outputService = outputService;
            _modelService = modelService;
            _priceService = priceService;
            _fileService = fileService;
        }

        public async Task ExecuteAsync()
        {
            var lines = await _fileService.ImportFileAsync("");
            var transactionList = _modelService.ModelChange(lines);
            _priceService.CalculatePrice(transactionList);
            _priceService.CalculateDiscounts(transactionList);
            _outputService.Reset();

            foreach (var transaction in transactionList)
            {
                if (transaction.Valid == false)
                {
                    _outputService.Print($"{transaction.Date} {transaction.PackageSize} {transaction.Provider}".TrimEnd() + " Ignored");
                }
                else
                {
                    string discountText = transaction.Discount == 0 ? "-" : transaction.Discount.ToString("0.00");
                    _outputService.Print($"{transaction.Date} {transaction.PackageSize} {transaction.Provider} " +
                        $"{transaction.Price.ToString("0.00")} {discountText}");
                }
            }

            //Can be run from cmd "dotnet run" while in solution folder and "dotnet test" in test folder

            //bool loop = true;
            //while (loop == true)
            //{
            //    Console.Write("Select between 'run file {filename}' and 'run test', 'run exit': ");
            //    string[] select = Console.ReadLine().Split(" ");
            //    if (select[0] == "run")
            //    {
            //        switch (select[1].ToLower())
            //        {
            //            case "file":
            //                Console.WriteLine("runFile");

            //                break;

            //            case "test":
            //                Console.WriteLine("runTest");
            //                break;

            //            case "exit":
            //                loop = false;
            //                break;

            //            default:
            //                Console.WriteLine("Continue");
            //                break;
            //        }
            //    }
            //    else
            //    {
            //        Console.WriteLine("Enter a valid command");
            //    }
            //}

        }
    }
}
