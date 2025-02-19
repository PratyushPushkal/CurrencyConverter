using System;
using System.Threading.Tasks;
using CurrencyConverter.Application.Service;
using CurrencyConverter.Domain.Model;
using CurrencyConverter.Infrastructure.Interface;
using Moq;
using Xunit;

namespace CurrencyConverter.UnitTest.Application
{
    public class CurrencyConverterServiceTests
    {
        private readonly Mock<IFrankfurterService> _frankfurterServiceMock;
        private readonly CurrencyConverterService _currencyConverterService;

        public CurrencyConverterServiceTests()
        {
            _frankfurterServiceMock = new Mock<IFrankfurterService>();
            _currencyConverterService = new CurrencyConverterService(_frankfurterServiceMock.Object);
        }

        [Fact]
        public async Task GetHistoricalRateByCurrency_ReturnsRates()
        {
            // Arrange
            var currency = "USD";
            var start = new DateTime(2023, 1, 1);
            var end = new DateTime(2023, 1, 31);
            var expectedRates = new HistoricalExchangeRate
            {
                Amount = 1,
                Base = "USD",
                Start_Date = "2023-01-01",
                End_Date = "2023-01-31",
                Date = new DateTime(2023, 1, 1),
                Rates = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, decimal>>
                {
                    { "2023-01-01", new System.Collections.Generic.Dictionary<string, decimal> { { "USD", 1 } } }
                }
            };

            _frankfurterServiceMock.Setup(service => service.GetHistoricalRateByCurrency(currency, start, end))
                .ReturnsAsync(expectedRates);

            // Act
            var result = await _currencyConverterService.GetHistoricalRateByCurrency(currency, start, end);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedRates.Base, result.Base);
        }

        [Fact]
        public async Task GetLatestExchangeRate_ReturnsRate()
        {
            // Arrange
            var currency = "USD";
            var expectedRate = new ExchangeRate
            {
                Amount = 1,
                Base = "USD",
                Date = new DateTime(2023, 1, 1),
                Rates = new System.Collections.Generic.Dictionary<string, decimal> { { "USD", 1 } }
            };

            _frankfurterServiceMock.Setup(service => service.GetLatestExchangeRateByCurrency(currency))
                .ReturnsAsync(expectedRate);

            // Act
            var result = await _currencyConverterService.GetLatestExchangeRate(currency);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedRate.Base, result.Base);
        }
    }
}
