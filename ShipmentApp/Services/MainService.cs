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
        }
    }
}
