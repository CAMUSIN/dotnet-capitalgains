using CapitalGainsTaxes.Abstractions;
using CapitalGainsTaxes.Entities;
using Newtonsoft.Json;

namespace CapitalGainsTaxes
{
	public class App
	{
        private readonly IProcessor processor;
        private readonly ISender sender;

        public App(IProcessor _processor, ISender _sender)
        {
            processor = _processor;
            sender = _sender;
        }

        public void Run(string[] args)
        {
            if (args.Any() && args != null)
            {
                foreach (var arg in args)
                {
                    try
                    {
                        var transactions = JsonConvert.DeserializeObject<Operation[]>(arg)!;
                        var taxes = processor.TaxesProcessor(transactions);
                        sender.Send(JsonConvert.SerializeObject(taxes));
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException(ex.Message);
                    }
                }
            }
            else
            {
                var argsReaded = Console.ReadLine();
                if (argsReaded != null && argsReaded.Any())
                {
                    var transactions = JsonConvert.DeserializeObject<Operation[]>(argsReaded)!;
                    var taxes = processor.TaxesProcessor(transactions);
                    sender.Send(JsonConvert.SerializeObject(taxes));
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            Console.Read();
        }
    }
}

