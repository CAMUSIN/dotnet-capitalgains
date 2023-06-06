using Newtonsoft.Json;

namespace CapitalGainsTaxes.Entities
{
    public class TaxDetail
    {
        private decimal tax;

        public TaxDetail(decimal tax)
        {
            this.tax = tax;
        }

        [JsonProperty("tax")]
        public decimal Tax
        {
            get { return tax; }
            set { tax = Math.Round(value, 2); }
        }
    }
}

