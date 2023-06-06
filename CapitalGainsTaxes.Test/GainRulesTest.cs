using CapitalGainsTaxes.Abstractions;
using CapitalGainsTaxes.Entities;
using CapitalGainsTaxes.Services;
using Microsoft.Extensions.Configuration;

namespace CapitalGainsTaxes.Test
{
	public class GainRulesTest
	{
        private readonly IGainRule gainRules;

        private readonly IConfiguration configuration;

        public GainRulesTest()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            this.gainRules = new GainRule(configuration);
        }

        [Fact]
        public void IsWhitLosses_IsTrue()
        {
            //arrange
            var operation = new Operation("sell", 9, 100);
            var weightedAvgPrice = 10;
            //act
            var result = gainRules.IsWithLosses(operation, weightedAvgPrice);
            //assert
            Assert.True(result);
        }

        [Fact]
        public void IsWhitLosses_IsFalse()
        {
            //arrange
            var operation = new Operation("sell", 10, 100);
            var weightedAvgPrice = 10;
            //act
            var result = gainRules.IsWithLosses(operation, weightedAvgPrice);
            //assert
            Assert.False(result);
        }

        [Fact]
        public void CalculateWeightedAvg_Should()
        {
            //arrange
            var currentStockQuantity = 100;
            var weightedAvgPrice = 10;
            var newStockQuantity = 100;
            var buyPrice = 10;
            //act
            var result = gainRules.CalculateWeightedAvg(currentStockQuantity, weightedAvgPrice, newStockQuantity, buyPrice);
            //assert
            Assert.Equal(weightedAvgPrice, result);
        }

        [Fact]
        public void CalculateLosses_Should()
        {
            //arrange
            var weightedAvgPrice = 10;
            var sellStockQuantity = 100;
            var sellPrice = 5;

            //act
            var result = gainRules.CalculateLosses(weightedAvgPrice, sellStockQuantity, sellPrice);

            //assert
            Assert.Equal((sellStockQuantity * weightedAvgPrice) - (sellStockQuantity * sellPrice), result);
        }

        [Fact]
        public void ApplyForTaxes_IsTrue()
        {
            //arrange
            var operation = new Operation("sell", 101, 200);

            //act
            var result = gainRules.ApplyForTaxes(operation);

            //assert
            Assert.True(result);
        }

        [Fact]
        public void CalculateTaxes_Should()
        {
            //arrange
            var overrallProfit = 100000;

            //act
            var result = gainRules.CalculateTaxes(overrallProfit);

            //assert
            Assert.Equal((overrallProfit * (decimal)0.20), result);
        }
    }
}

