using CapitalGainsTaxes.Abstractions;
using CapitalGainsTaxes.Entities;
using Microsoft.Extensions.Configuration;

namespace CapitalGainsTaxes.Services
{
    public class GainRule : IGainRule
    {
        private readonly IConfiguration config;

        public GainRule(IConfiguration _config)
        {
            config = _config;
        }

        //Calculates the weighted average price of current stocks quantity
        public decimal CalculateWeightedAvg(int currentStockQuantity, decimal weightedAvgPrice, int newStockQuantity, decimal newPrice)
        {
            if (currentStockQuantity == 0)
            {
                return newPrice;
            }
            return ((currentStockQuantity * weightedAvgPrice) + (newStockQuantity * newPrice)) / (currentStockQuantity + newStockQuantity);
        }

        //Calculates losses generated in case you sell fro a proce lower the average
        public decimal CalculateLosses(decimal weightedAvgPrice, int sellStockQuantity, decimal sellPrice)
        {
            return (sellStockQuantity * weightedAvgPrice) - (sellStockQuantity * sellPrice);
        }

        //Calculates the amount of taxes base on the applicable rule: Current 20% of profits.
        public decimal CalculateTaxes(decimal overallProfit)
        {
            var taxPercentage = config.GetSection("Rules").GetValue<decimal>("TaxPercentage");
            return Math.Round((overallProfit * taxPercentage), 2);
        }

        //Evaluates if the operation apply for taxes base on the applicable rules.
        public bool ApplyForTaxes(Operation operation)
        {
            var minAmouttoApplyTaxes = config.GetSection("Rules").GetValue<decimal>("OperationAmount");
            if ((operation.Quantity * operation.UnitCost) > minAmouttoApplyTaxes)
                return true;
            return false;
        }

        //Evaluates if the operation has losses base on the applicable rules.
        public bool IsWithLosses(Operation operation, decimal weightedAvgPrice)
        {
            if (operation.UnitCost < weightedAvgPrice)
                return true;
            return false;
        }

        //Calculates the real profit base on the applicable rules.
        public decimal CalculateProfits(Operation operation, decimal weightedAvgPrice)
        {
            return (operation.Quantity * operation.UnitCost) - (operation.Quantity * weightedAvgPrice);
        }

        //Calculates the real profit base on the applicable rules and losses.
        public decimal CalculateProfits(Operation operation, decimal weightedAvgPrice, decimal losses)
        {
            var transactionAmount = (operation.Quantity * operation.UnitCost);
            if (losses >= transactionAmount)
            {
                return 0;
            }
            else
            {
                transactionAmount = ((operation.Quantity * weightedAvgPrice) - transactionAmount) - losses;
            }
            return transactionAmount;
        }
    }
}

