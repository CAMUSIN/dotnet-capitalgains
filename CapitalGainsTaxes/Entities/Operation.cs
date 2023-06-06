using Newtonsoft.Json;

namespace CapitalGainsTaxes.Entities
{
    public class Operation
    {
        private string operationType;
        private decimal unitCost;
        private int quantity;

        public Operation(string operationType, decimal unitCost, int quantity)
        {
            this.operationType = operationType;
            this.unitCost = unitCost;
            this.quantity = quantity;
        }

        [JsonProperty("operation")]
        public string OperationType
        {
            get { return operationType; }
            set { operationType = value; }
        }

        [JsonProperty("unit-cost")]
        public decimal UnitCost
        {
            get { return unitCost; }
            set { unitCost = Math.Round(value, 2); }
        }

        [JsonProperty("quantity")]
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
    }
}

