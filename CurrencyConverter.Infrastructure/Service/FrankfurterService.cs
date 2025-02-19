using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using CurrencyConverter.Domain;
using CurrencyConverter.Domain.Model;
using CurrencyConverter.Infrastructure.Interface;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace CurrencyConverter.Infrastructure.Service
{
    public class FrankfurterService : IFrankfurterService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly HttpClient _httpClient;
        public FrankfurterService(IMemoryCache memoryCache, HttpClient httpClient)
        {
            _memoryCache = memoryCache;
            _httpClient = httpClient;
        }

        public async Task<HistoricalExchangeRate?> GetHistoricalRateByCurrency(string currency, DateTime start, DateTime end)
        {
            string sdate = start.ToString("yyyy-MM-dd");
            string edate = end.ToString("yyyy-MM-dd");
            var cacheRates = _memoryCache.Get<HistoricalExchangeRate>(currency + sdate + edate);
            if (cacheRates is not null)
            {
                return cacheRates;
            }
            _httpClient.BaseAddress = new Uri(Common.FrankfuterUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = new HttpResponseMessage();
            response = await _httpClient.GetAsync($@"{sdate}..{edate}?base={currency}").ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var result = data == null ? null : JsonConvert.DeserializeObject<HistoricalExchangeRate>(data);
                _memoryCache.Set(currency + sdate + edate, result);
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<ExchangeRate?> GetLatestExchangeRateByCurrency(string currency)
        {
            var cacheRates = _memoryCache.Get<ExchangeRate>(currency);
            if (cacheRates is not null)
            {
                return cacheRates;
            }
            _httpClient.BaseAddress = new Uri(Common.FrankfuterUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = new HttpResponseMessage();
            response = await _httpClient.GetAsync($@"latest?base={currency}").ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var result = data == null ? null : JsonConvert.DeserializeObject<ExchangeRate>(data);
                _memoryCache.Set(currency, result);
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
