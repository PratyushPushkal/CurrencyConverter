using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyConverter.Application.Interface;
using CurrencyConverter.Domain.Model;
using CurrencyConverter.Infrastructure.Interface;

namespace CurrencyConverter.Application.Service
{
    public class CurrencyConverterService : ICurrencyConverterService
    {
        private readonly IFrankfurterService _frankfurterService;
        public CurrencyConverterService(IFrankfurterService frankfurterService) 
        {
            _frankfurterService = frankfurterService;
        }

        public async Task<HistoricalExchangeRate?> GetHistoricalRateByCurrency(string currency, DateTime start, DateTime end)
        {
            return await _frankfurterService.GetHistoricalRateByCurrency(currency, start, end);
        }

        public async Task<ExchangeRate?> GetLatestExchangeRate(string currency)
        {
            return await _frankfurterService.GetLatestExchangeRateByCurrency(currency);
        }
    }
}
