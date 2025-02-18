using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Domain.Model
{
    public class HistoricalExchangeRate
    {
        public decimal Amount { get; set; }

        public string? Base { get; set; }

        public string? Start_Date { get; set; }

        public string? End_Date { get; set; }

        public DateTime Date { get; set; }

        public Dictionary<string, Dictionary<string, decimal>>?  Rates { get; set; }
    }
}
