using FluentAssertions;
using Moq;
using ShipmentApp.Interfaces;
using ShipmentApp.Models;
using ShipmentApp.Services;
using System.Collections.Generic;
using Xunit;

namespace ShipmentApp.UnitTest
{
    public class PriceServiceTests
    {

        [Fact]
        public void CalculatePrice_GivenCorrectData_GivesCorrectPrice()
        {
            var importFileServiceMock = new Mock<IFileService>();

            importFileServiceMock.Setup(ifs => ifs.LoadPricingInfo()).Returns(new List<PricingInfo>
            {
               new PricingInfo{Provider = "LP", PackageSize = "S", Price = 1.5M},
               new PricingInfo{Provider = "MR", PackageSize = "M", Price = 3M}
               });

            var transactions = new List<Transaction>
            {
                new Transaction{Date = "2015-09-09", PackageSize = "S", Provider = "LP"},
                new Transaction{Date = "2015-09-09", PackageSize = "M", Provider = "MR"},
            };
            var priceService = new PriceService(importFileServiceMock.Object);

            priceService.CalculatePrice(transactions);
            transactions[0].Price.Should().Be(1.5M);
            transactions[1].Price.Should().Be(3M);
        }
        [Fact]
        public void CalculatePrice_GivenIncorrectData_SetsInvalid()
        {
            var importFileServiceMock = new Mock<IFileService>();

            importFileServiceMock.Setup(ifs => ifs.LoadPricingInfo()).Returns(new List<PricingInfo>
            {
               new PricingInfo{Provider = "LP", PackageSize = "S", Price = 1.5M},
               new PricingInfo{Provider = "MR", PackageSize = "M", Price = 3M}
               });

            var transactions = new List<Transaction>
            {
                new Transaction{Date = "2015-09-09", PackageSize = "A", Provider = "LP"},
                new Transaction{Date = "2015-09-09", PackageSize = "S", Provider = "MBB"},
            };
            var priceService = new PriceService(importFileServiceMock.Object);

            priceService.CalculatePrice(transactions);
            transactions[0].Valid.Should().Be(false);
            transactions[1].Valid.Should().Be(false);
        }
        [Fact]
        public void CalculateDiscounts_GivenCorrectData_GivesSmallPackageDiscount()
        {
            var importFileServiceMock = new Mock<IFileService>();

            importFileServiceMock.Setup(ifs => ifs.LoadPricingInfo()).Returns(new List<PricingInfo>
            {
               new PricingInfo{Provider = "LP", PackageSize = "S", Price = 1.5M},
               new PricingInfo{Provider = "MR", PackageSize = "S", Price = 2M},
             });

            var transactions = new List<Transaction>
            {
                new Transaction{Date = "2015-09-09", PackageSize = "S", Provider = "LP", Price = 1.5M, Valid = true},
                new Transaction{Date = "2015-09-10", PackageSize = "S", Provider = "MR", Price = 2M, Valid = true},
            };

            var priceService = new PriceService(importFileServiceMock.Object);

            priceService.CalculateDiscounts(transactions);
            transactions[0].Discount.Should().Be(0M);
            transactions[0].Price.Should().Be(1.50M);
            transactions[1].Discount.Should().Be(0.50M);
            transactions[1].Price.Should().Be(1.50M);
        }
        [Fact]
        public void CalculateDiscounts_GivenCorrectData_GivesLPLargePackageDiscount()
        {
            var importFileServiceMock = new Mock<IFileService>();

            importFileServiceMock.Setup(ifs => ifs.LoadPricingInfo()).Returns(new List<PricingInfo>
            {
               new PricingInfo{Provider = "LP", PackageSize = "L", Price = 6.90M},
               new PricingInfo{Provider = "LP", PackageSize = "S", Price = 1.5M},
             });

            var transactions = new List<Transaction>
            {
                new Transaction{Date = "2015-09-09", PackageSize = "L", Provider = "LP", Price = 6.90M, Valid = true},
                new Transaction{Date = "2015-09-12", PackageSize = "L", Provider = "LP", Price = 6.90M, Valid = true},
                new Transaction{Date = "2015-09-15", PackageSize = "L", Provider = "LP", Price = 6.90M, Valid = true},
                new Transaction{Date = "2015-09-20", PackageSize = "L", Provider = "LP", Price = 6.90M, Valid = true},
            };

            var priceService = new PriceService(importFileServiceMock.Object);

            priceService.CalculateDiscounts(transactions);
            transactions[0].Discount.Should().Be(0M);
            transactions[2].Discount.Should().Be(6.90M);
            transactions[3].Discount.Should().Be(0M);
            transactions[0].Price.Should().Be(6.90M);
            transactions[2].Price.Should().Be(0M);
            transactions[3].Price.Should().Be(6.90M);
        }
        [Fact]
        public void CalculateDiscounts_GivenCorrectData_10MonthlyDiscountLimit()
        {
            var importFileServiceMock = new Mock<IFileService>();

            importFileServiceMock.Setup(ifs => ifs.LoadPricingInfo()).Returns(new List<PricingInfo>
            {
               new PricingInfo{Provider = "LP", PackageSize = "L", Price = 6.90M},
               new PricingInfo{Provider = "LP", PackageSize = "S", Price = 1.5M},
             });

            var transactions = new List<Transaction>
            {
                new Transaction{Date = "2015-09-09", PackageSize = "S", Provider = "LP", Price = 8.50M, Valid = true},
                new Transaction{Date = "2015-09-10", PackageSize = "S", Provider = "MR", Price = 8.50M, Valid = true},
                new Transaction{Date = "2015-09-12", PackageSize = "S", Provider = "LP", Price = 8.50M, Valid = true},
                new Transaction{Date = "2015-10-12", PackageSize = "S", Provider = "MR", Price = 8.50M, Valid = true},
            };

            var priceService = new PriceService(importFileServiceMock.Object);

            priceService.CalculateDiscounts(transactions);
            transactions[0].Discount.Should().Be(7M);
            transactions[1].Discount.Should().Be(3M);
            transactions[2].Discount.Should().Be(0M);
            transactions[3].Discount.Should().Be(7M);
            transactions[0].Price.Should().Be(1.50M);
            transactions[1].Price.Should().Be(5.50M);
            transactions[2].Price.Should().Be(8.50M);
            transactions[3].Price.Should().Be(1.50M);

        }
    }
}
