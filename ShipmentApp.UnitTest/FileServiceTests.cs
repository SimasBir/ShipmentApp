using FluentAssertions;
using Moq;
using ShipmentApp.Interfaces;
using ShipmentApp.Models;
using ShipmentApp.Services;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace ShipmentApp.UnitTest
{
    public class FileServiceTests
    {
        [Fact]
        public async Task ImportTransactionsAsync_GivenCorrectData_ParsesDataSucessfully()
        {
            var fileService = new FileService();

            //Act
            var transactions = await fileService.ImportFileAsync("");

            //Assert
            transactions.Should().HaveCount(21);
            transactions[0].Should().Be("2015-02-01 S MR");
            transactions[20].Should().Be("2015-03-01 S MR");
        } 
        [Fact]
        public async Task ImportTransactionsAsync_GivenCorrectData_ParsesDataAsInputSample()
        {
            var fileService = new FileService();

            //Act
            var transactions = await fileService.ImportFileAsync("");
            var processedTransactions = await File.ReadAllLinesAsync("Data/InputSample.txt");

            //Assert
            transactions.Should().BeEquivalentTo(processedTransactions);
        }

        [Fact]
        public async Task LoadPricingInfosAsync_GivenCorrectData_ParsesDataSucessfully()
        {
            var sample = new List<PricingInfo>
            {
               new PricingInfo{ Provider = "LP", PackageSize = "S", Price = 1.5M },
               new PricingInfo{ Provider = "LP", PackageSize = "M", Price = 4.90M }
            };

            var fileService = new FileService();

            //Act
            var pricingInfos = await fileService.LoadPricingInfoAsync();

            //Assert
            pricingInfos.Count.Should().Be(6);
            pricingInfos[0].Price.Should().Be(1.5M);
            pricingInfos[0].PackageSize.Should().Be("S");
            pricingInfos[0].Provider.Should().Be("LP");

            pricingInfos[1].Should().BeEquivalentTo(sample[1]);
        }
    }
}
