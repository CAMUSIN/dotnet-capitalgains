
using CapitalGainsTaxes.Abstractions;

namespace CapitalGainsTaxes.Services
{
    public class Sender : ISender
    {
        public void Send(string taxes)
        {
            Console.WriteLine(taxes);
        }
    }
}

