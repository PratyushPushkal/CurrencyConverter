using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyConverter.Domain.Model;

namespace CurrencyConverter.Infrastructure.Interface
{
    public interface IFrankfurterService
    {
        public Task<ExchangeRate?> GetLatestExchangeRateByCurrency(string currency);

        public Task<HistoricalExchangeRate?> GetHistoricalRateByCurrency(string currency, DateTime start, DateTime end);
    }
}
