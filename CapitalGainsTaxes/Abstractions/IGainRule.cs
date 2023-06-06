using CapitalGainsTaxes.Entities;

namespace CapitalGainsTaxes.Abstractions
{
    public interface IGainRule
    {
        bool ApplyForTaxes(Operation operation);
        decimal CalculateLosses(decimal weightedAvgPrice, int sellStockQuantity, decimal sellPrice);
        decimal CalculateProfits(Operation operation, decimal weightedAvgPrice);
        decimal CalculateProfits(Operation operation, decimal weightedAvgPrice, decimal losses);
        decimal CalculateTaxes(decimal overallProfit);
        decimal CalculateWeightedAvg(int currentStockQuantity, decimal weightedAvgPrice, int newStockQuantity, decimal newPrice);
        bool IsWithLosses(Operation operation, decimal weightedAvgPrice);
    }
}