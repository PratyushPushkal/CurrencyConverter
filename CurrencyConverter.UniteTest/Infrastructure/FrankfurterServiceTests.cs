using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CurrencyConverter.Domain.Model;
using CurrencyConverter.Infrastructure.Service;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

namespace CurrencyConverter.UniteTest.Infrastructure
{
    public class FrankfurterServiceTests
    {
        private readonly Mock<IMemoryCache> _memoryCacheMock;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly FrankfurterService _frankfurterService;

        public FrankfurterServiceTests()
        {
            _memoryCacheMock = new Mock<IMemoryCache>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _frankfurterService = new FrankfurterService(_memoryCacheMock.Object, _httpClient);
        }

        [Fact]
        public async Task GetHistoricalRateByCurrency_ReturnsRates_WhenCacheIsEmpty()
        {
            // Arrange
            var currency = "USD";
            var start = new DateTime(2023, 1, 1);
            var end = new DateTime(2023, 1, 31);
            var expectedRates = GetHistoricalExchangeRate();
            var responseContent = new StringContent(JsonConvert.SerializeObject(expectedRates));
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = responseContent };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            _memoryCacheMock.Setup(mc => mc.TryGetValue(It.IsAny<object>(), out expectedRates)).Returns(false);
            _memoryCacheMock.Setup(mc => mc.Set(It.IsAny<string>(), It.IsAny<HistoricalExchangeRate>(), It.IsAny<MemoryCacheEntryOptions>()));

            // Act
            var result = await _frankfurterService.GetHistoricalRateByCurrency(currency, start, end);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedRates.Base, result.Base);
        }

        [Fact]
        public async Task GetLatestExchangeRateByCurrency_ReturnsRates_WhenCacheIsEmpty()
        {
            // Arrange
            var currency = "USD";
            var expectedRate = GetExchangeRate();
            var responseContent = new StringContent(JsonConvert.SerializeObject(expectedRate));
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = responseContent };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            _memoryCacheMock.Setup(mc => mc.TryGetValue(It.IsAny<object>(), out expectedRate)).Returns(false);
            _memoryCacheMock.Setup(mc => mc.Set(It.IsAny<string>(), It.IsAny<ExchangeRate>(), It.IsAny<MemoryCacheEntryOptions>()));

            // Act
            var result = await _frankfurterService.GetLatestExchangeRateByCurrency(currency);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedRate.Base, result.Base);
        }

        private HistoricalExchangeRate GetHistoricalExchangeRate()
        {
            return new HistoricalExchangeRate
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
        }

        private ExchangeRate GetExchangeRate()
        {
            return new ExchangeRate
            {
                Amount = 1,
                Base = "USD",
                Date = new DateTime(2023, 1, 1),
                Rates = new System.Collections.Generic.Dictionary<string, decimal> { { "USD", 1 } }
            };
        }
    }
}
