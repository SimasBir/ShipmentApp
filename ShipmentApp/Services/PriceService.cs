using ShipmentApp.Interfaces;
using ShipmentApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShipmentApp.Services
{
    public class PriceService
    {
        //private List<PricingInfo> _pricingInfo;
        private IFileService _fileService;

        public PriceService(IFileService fileService)
        {
            _fileService = fileService;
            //_pricingInfo = _fileService.LoadPricingInfo();
        }

        public async void CalculatePrice(List<Transaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                var pricingInfo = await _fileService.LoadPricingInfoAsync();
                var price = pricingInfo.FirstOrDefault(s => s.PackageSize == transaction.PackageSize
                && s.Provider == transaction.Provider);
                if (price != null)
                {
                    transaction.Price = price.Price;
                }
                else
                {
                    transaction.Valid = false;
                }
            }
        }

        public async void CalculateDiscounts(List<Transaction> transactions)
        {
            var previousMonth = 0;
            decimal monthlyLimit = 0;
            int countLP = 0; // third LP shipment free counter

            var pricingInfo = await _fileService.LoadPricingInfoAsync();
            decimal lowestSmallPrice = pricingInfo.OrderBy(p => p.Price).FirstOrDefault(s => s.PackageSize == "S").Price; //Find lowest S size cost

            var ordered = transactions.OrderBy(d => d.Date).Where(v=>v.Valid == true);
            foreach (var transaction in ordered)
            {
                var currentMonth = DateTime.ParseExact(transaction.Date, "yyyy-MM-dd", null).Month;
                if (currentMonth != previousMonth)
                {
                    monthlyLimit = 10.0M; //monthly limit
                    countLP = 0;
                }
                if (transaction.PackageSize == "S" && monthlyLimit > 0)
                {
                    var discount = transaction.Price - lowestSmallPrice;
                    monthlyLimit = AssignDiscount(transaction, monthlyLimit, discount);
                }
                if (transaction.PackageSize == "L" && transaction.Provider == "LP" && monthlyLimit > 0)
                {
                    countLP++;
                    if (countLP == 3) 
                    {
                        var discount = transaction.Price;
                        monthlyLimit = AssignDiscount(transaction, monthlyLimit, discount);
                    }
                }
                previousMonth = currentMonth;
            }
        }

        private decimal AssignDiscount(Transaction transaction, decimal monthlyLimit, decimal discount)
        {
            if (Math.Max(monthlyLimit - discount, 0) == 0)
            {
                discount = monthlyLimit;
                monthlyLimit = 0;
            }
            else
            {
                monthlyLimit = monthlyLimit - discount;
            }

            transaction.Discount = discount;
            transaction.Price = transaction.Price - discount;
            return monthlyLimit;
        }
    }
}
