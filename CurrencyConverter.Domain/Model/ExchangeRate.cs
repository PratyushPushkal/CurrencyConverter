using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Domain.Model
{
    public class ExchangeRate
    {
        public decimal Amount { get; set; }

        public string? Base { get; set; }

        public DateTime Date { get; set; }

        public Dictionary<string, decimal>? Rates { get; set; }
    }
}
