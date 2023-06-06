using CapitalGainsTaxes.Entities;

namespace CapitalGainsTaxes.Abstractions
{
    public interface IProcessor
    {
        List<TaxDetail> TaxesProcessor(Operation[] transactions);
    }
}