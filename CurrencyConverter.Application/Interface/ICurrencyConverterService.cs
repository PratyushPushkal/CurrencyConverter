using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyConverter.Domain.Model;

namespace CurrencyConverter.Application.Interface
{
    public interface ICurrencyConverterService
    {
        public Task<ExchangeRate?> GetLatestExchangeRate(string currency);

        public Task<HistoricalExchangeRate?> GetHistoricalRateByCurrency(string currency, DateTime start, DateTime end);
    }
}
