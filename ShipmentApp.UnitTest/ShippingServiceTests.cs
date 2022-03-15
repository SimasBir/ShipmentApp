using FluentAssertions;
using ShipmentApp.Services;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace ShipmentApp.UnitTest
{
    public class ShippingServiceTests
    {   
        [Fact]
        public async Task ExecuteAsync_PrintingResultsToFile_ShouldBeEqualToExample()
        {
            var fileService = new FileService();
            var outputService = new OutputService();
            var priceService = new PriceService(fileService);
            var modelService = new ModelService();

            var shippingService = new MainService(outputService, modelService, priceService, fileService);
            await shippingService.ExecuteAsync();

            var processedTransactions = await File.ReadAllLinesAsync("Data/Output.txt");
            var sampleTransactions = await File.ReadAllLinesAsync("Data/Sample.txt");

            for (int i = 0; i < processedTransactions.Length; i++)
            {
                processedTransactions[i].Should().Be(sampleTransactions[i]);
            }
        }
    }
}
