using CapitalGainsTaxes.Abstractions;
using CapitalGainsTaxes.Entities;

namespace CapitalGainsTaxes.Services
{
    public class Processor : IProcessor
    {
        private readonly IGainRule gainRules;

        public Processor(IGainRule _gainRule)
        {
            gainRules = _gainRule;
        }

        public List<TaxDetail> TaxesProcessor(Operation[] transactions)
        {
            var taxes = new List<TaxDetail>();
            if (transactions != null && transactions.Any())
            {
                decimal loss = 0;
                int currentStockquantity = 0;
                decimal weightedAvgPrice = 0;
                foreach (var operation in transactions)
                {
                    switch (operation.OperationType)
                    {
                        case "buy":
                            //calculare / recalculate AvgPrice
                            weightedAvgPrice = gainRules.CalculateWeightedAvg(currentStockquantity, weightedAvgPrice, operation.Quantity, operation.UnitCost);
                            //update stock
                            currentStockquantity += operation.Quantity;
                            //caclulate & add taxes
                            taxes.Add(new TaxDetail(gainRules.CalculateTaxes(0)));
                            break;
                        case "sell":
                            //evaluates if apply for taxes
                            if (gainRules.ApplyForTaxes(operation))
                            {
                                decimal overallProfit = 0;
                                //evaluates if has losses
                                if (gainRules.IsWithLosses(operation, weightedAvgPrice))
                                {
                                    //calculate & update losses
                                    loss += gainRules.CalculateLosses(weightedAvgPrice, operation.Quantity, operation.UnitCost);

                                    //calculate profits
                                    overallProfit = gainRules.CalculateProfits(operation, weightedAvgPrice, loss);

                                    //caclulate & add taxes
                                    taxes.Add(new TaxDetail(gainRules.CalculateTaxes(overallProfit)));
                                }
                                else
                                {
                                    //calculate profits
                                    overallProfit = gainRules.CalculateProfits(operation, weightedAvgPrice);
                                    if (loss > 0)
                                    {
                                        if (loss >= overallProfit)
                                        {
                                            loss -= overallProfit;
                                            overallProfit = 0;
                                        }
                                        else
                                        {
                                            overallProfit = overallProfit - loss;
                                            loss -= loss;
                                        }
                                    }
                                    taxes.Add(new TaxDetail(gainRules.CalculateTaxes(overallProfit)));
                                }
                            }
                            else
                            {
                                //evaluates if has losses
                                if (gainRules.IsWithLosses(operation, weightedAvgPrice))
                                {
                                    //calculate & update losses
                                    loss += gainRules.CalculateLosses(weightedAvgPrice, operation.Quantity, operation.UnitCost);
                                }
                                //caclulate & add taxes
                                taxes.Add(new TaxDetail(gainRules.CalculateTaxes(0)));
                            }
                            //update stock
                            currentStockquantity -= operation.Quantity;
                            break;
                    }
                }
            }
            return taxes;
        }
    }
}

