using CurrencyConverter.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Logging;

namespace CurrencyConverter.Presentation.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireAdministratorRole")]
    public class CurrencyConverterController : ControllerBase
    {
        private readonly ICurrencyConverterService _currencyConverter;
        private readonly ILogger<CurrencyConverterController> _logger;
        private readonly string[] _excludedCurrency = { "TRY", "PLN", "THB", "MXN" };

        public CurrencyConverterController(
            ICurrencyConverterService currencyConverter,
            ILogger<CurrencyConverterController> logger)
        {
            _currencyConverter = currencyConverter;
            _logger = logger;
        }

        [HttpGet]
        [Route("latestrate")]
        [EnableRateLimiting("policyname")]
        public async Task<IActionResult> Get(string basecurrency)
        {
            _logger.LogInformation($@"Endpoint Name: {this.GetType().Name.ToString()}, Method : GET");
            int time = DateTime.Now.Microsecond;
            var result = await _currencyConverter.GetLatestExchangeRate(basecurrency);
            if (result is null)
            {
                _logger.LogInformation($@"Status Code : {HttpContext.Response.StatusCode.ToString()}, Response Time  : {time - DateTime.Now.Microsecond} ms.");
                var message = @$"Information not available for Base Currency : {basecurrency}";
                _logger.LogInformation(message);
                return NotFound(message);
            }
            else
            {
                _logger.LogInformation($@"Status Code : {HttpContext.Response.StatusCode.ToString()}, Response Time  : {time - DateTime.Now.Microsecond} ms.");
                return Ok(result.Rates);
            }
        }

        [HttpGet]
        [Route("convertcurrency")]
        [EnableRateLimiting("policyname")]
        public async Task<IActionResult> Get(decimal currencyValue, string baseCurrency, string targetCurrency)
        {
            _logger.LogInformation($@"Endpoint Name: {this.GetType().Name.ToString()}, Method : GET");
            if (_excludedCurrency.Contains(baseCurrency) || _excludedCurrency.Contains(targetCurrency))
            {
                _logger.LogInformation($@"Bad Currency Involved : TRY,PLN,THB,MXN");
                return BadRequest();
            }

            int time = DateTime.Now.Microsecond;
            var result = await _currencyConverter.GetLatestExchangeRate(baseCurrency);
            if (result is null)
            {
                _logger.LogInformation($@"Status Code : {HttpContext.Response.StatusCode.ToString()}, Response Time  : {time - DateTime.Now.Microsecond} ms.");
                var message = @$"Information not available for Base Currency : {baseCurrency}";
                _logger.LogInformation(message);
                return NotFound(message);
            }
            else
            {
                _logger.LogInformation($@"Status Code : {HttpContext.Response.StatusCode.ToString()}, Response Time  : {time - DateTime.Now.Microsecond} ms.");
                decimal val = 0;
                result.Rates?.TryGetValue(targetCurrency, out val);
                return Ok(val * currencyValue);
            }
        }

        [HttpGet]
        [Route("historicaldata")]
        [EnableRateLimiting("policyname")]
        public async Task<IActionResult> Get(string baseCurrency, DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation($@"Endpoint Name: {this.GetType().Name.ToString()}, Method : GET");
            int time = DateTime.Now.Microsecond;
            var result = await _currencyConverter.GetHistoricalRateByCurrency(baseCurrency, startDate, endDate);
            if (result is null)
            {
                _logger.LogInformation($@"Status Code : {HttpContext.Response.StatusCode.ToString()}, Response Time  : {time - DateTime.Now.Microsecond} ms.");
                var message = @$"Information not available for Base Currency : {baseCurrency}";
                _logger.LogInformation(message);
                return NotFound(message);
            }
            else
            {
                _logger.LogInformation($@"Status Code : {HttpContext.Response.StatusCode.ToString()}, Response Time  : {time - DateTime.Now.Microsecond} ms.");
                return Ok(result.Rates);
            }
        }
    }
}
