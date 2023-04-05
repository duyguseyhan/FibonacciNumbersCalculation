using System;
using System.Text.Json;
using FibonacciNumbersCalculation.Models;
using FibonacciNumbersCalculation.Models.CacheModel;
using FibonacciNumbersCalculation.Services.CacheServices;
using FibonacciNumbersCalculation.Services.ExecutionTimeLimiter;
using FibonacciNumbersCalculation.Services.FibonacciAPIServices;
using FibonacciNumbersCalculation.Services.FibonacciServices;
using FibonacciNumbersCalculation.Services.MemoryUsageLimiter;
using Moq;

namespace FibonacciNumbersCalculationTest.ServicesTests
{
	public class FibonacciAPIServiceTests
	{
        private readonly Mock<IFibonacciService> _fibonacciServiceMock = new Mock<IFibonacciService>();
        private readonly Mock<ICacheService> _cacheServiceMock = new Mock<ICacheService>();
        private readonly Mock<IMemoryUsageLimiter> _memoryUsageLimiterMock = new Mock<IMemoryUsageLimiter>();
        private readonly Mock<IExecutionTimeLimiter> _executionTimeLimiterMock = new Mock<IExecutionTimeLimiter>();

        [Fact]
        public async Task GetFibonacciSubsequenceAsync_WithCache_ReturnsExpectedResult()
        {
            // Arrange
            var requestModel = new FibonacciSubsequenceRequestModel
            {
                StartIndex = 1,
                EndIndex = 2,
                UseCache = true,
                MaxMemoryUsage = 1000000L,
                TimeOut = 30000
            };

            var expectedResponse = "{\"Result\":[1,1],\"MemoryUsageExceeded\":false,\"TimeOut\":false}";

            _cacheServiceMock.Setup(x => x.GetAsync<CacheEntry<long>>(It.IsAny<string>(), It.IsAny<TimeSpan>()))
                .ReturnsAsync(new CacheEntry<long>(1L));

            var service = new FibonacciAPIService(_fibonacciServiceMock.Object, _cacheServiceMock.Object, _memoryUsageLimiterMock.Object, _executionTimeLimiterMock.Object);

            // Act
            var response = await service.GetFibonacciSubsequenceAsync(requestModel);

            // Assert
            Assert.Equal(expectedResponse, response);
        }

        [Fact]
        public async Task GetFibonacciSubsequenceAsync_WithMemoryUsageExceeded_ReturnsExpectedResult()
        {
            // Arrange
            var mockFibonacciService = new Mock<IFibonacciService>();
            var mockCacheService = new Mock<ICacheService>();
            var mockMemoryUsageLimiter = new Mock<IMemoryUsageLimiter>();
            var mockExecutionTimeLimiter = new Mock<IExecutionTimeLimiter>();

            // Use a small amount of max memory to make sure it is exceeded
            long maxMemoryUsage = 1 * 1024 * 1024; // 1 MB
            mockMemoryUsageLimiter.Setup(m => m.IsMemoryUsageExceeded(It.IsAny<long>())).Returns(true);

            var service = new FibonacciAPIService(
                mockFibonacciService.Object,
                mockCacheService.Object,
                mockMemoryUsageLimiter.Object,
                mockExecutionTimeLimiter.Object
            );

            var requestModel = new FibonacciSubsequenceRequestModel
            {
                StartIndex = 0,
                EndIndex = 10,
                UseCache = false,
                MaxMemoryUsage = maxMemoryUsage,
                TimeOut = 0
            };

            // Act
            var result = await service.GetFibonacciSubsequenceAsync(requestModel);
            var responseObj = JsonSerializer.Deserialize<Dictionary<string, object>>(result);
            var memoryUsageExceeded = responseObj["MemoryUsageExceeded"];

            // Assert
            var expectedResponse = "{\"Result\":[],\"MemoryUsageExceeded\":true,\"TimeOut\":false}";

            var responseObjExpected = JsonSerializer.Deserialize<Dictionary<string, object>>(expectedResponse);
            var memoryUsageExceededExpected = responseObjExpected["MemoryUsageExceeded"];

            Assert.Equal(memoryUsageExceededExpected, memoryUsageExceeded);
        }

        [Fact]
        public async Task GetFibonacciSubsequenceAsync_WithCacheEnabled_CallsCacheService()
        {
            // Arrange
            var requestModel = new FibonacciSubsequenceRequestModel
            {
                StartIndex = 1,
                EndIndex = 10,
                UseCache = true,
                MaxMemoryUsage = 1024 * 1024, // 1 MB
                TimeOut = 500000
            };

            var fibonacciServiceMock = new Mock<IFibonacciService>();
            var cacheServiceMock = new Mock<ICacheService>();
            var memoryUsageLimiterMock = new Mock<IMemoryUsageLimiter>();
            var executionTimeLimiterMock = new Mock<IExecutionTimeLimiter>();

            // Setup cache service mock
            cacheServiceMock.Setup(mock => mock.GetAsync<CacheEntry<long>>(It.IsAny<string>(), It.IsAny<TimeSpan>())).ReturnsAsync(new CacheEntry<long>(1234));

            var service = new FibonacciAPIService(fibonacciServiceMock.Object, cacheServiceMock.Object, memoryUsageLimiterMock.Object, executionTimeLimiterMock.Object);

            // Act
            var result = await service.GetFibonacciSubsequenceAsync(requestModel);

            // Assert
            cacheServiceMock.Verify(mock => mock.GetAsync<CacheEntry<long>>(It.IsAny<string>(), It.IsAny<TimeSpan>()), Times.Exactly(10));
        }
    }
}

