using System;
using CapitalGainsTaxes.Abstractions;
using CapitalGainsTaxes.Services;
using CapitalGainsTaxes.Entities;
using Microsoft.Extensions.Configuration;

namespace CapitalGainsTaxes.Test
{
	public class ProcessorTest
	{
        private readonly IProcessor processor;
        private readonly IGainRule gainRules;
        private readonly IConfiguration configuration;

        public ProcessorTest()
		{
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            gainRules = new GainRule(configuration);
            processor = new Processor(gainRules);
        }

        [Fact]
        public void TaxesProcessor_NoTransactions() {
            //arrange
            Operation[]? operations = null;

            //act
            var result = processor.TaxesProcessor(operations);

            //assert
            Assert.Empty(result);
        }

        [Fact]
        public void TaxesProcessor_TransactionsNoTaxes()
        {
            //arrange
            Operation[]? operations = new Operation[2];
            operations[0] = new Operation("buy",  1, 1);
            operations[1] = new Operation("sell", 1, 1);

            //act
            var result = processor.TaxesProcessor(operations);

            //assert
            Assert.Equal(2,result.Count);
        }

        [Fact]
        public void TaxesProcessor_TransactionsWithTaxes()
        {
            //arrange
            Operation[]? operations = new Operation[2];
            operations[0] = new Operation("buy", 1000, 21);
            operations[1] = new Operation("sell", 1001, 21);

            //act
            var result = processor.TaxesProcessor(operations);

            //assert
            Assert.Equal(2, result.Count);
            Assert.Equal((decimal)4.20, result[1].Tax);
        }
    }
}

