using System;
using FibonacciNumbersCalculation.Services.CacheServices;

namespace FibonacciNumbersCalculationTest.ServicesTests
{
	public class CacheServicesTests
	{
        private readonly ICacheService _cacheService;

        public CacheServicesTests()
        {
            _cacheService = new CacheService();
        }

        [Fact]
        public async Task SetAsync_Should_Add_Entry_To_Cache()
        {
            // Arrange
            string key = "myKey";
            string value = "myValue";

            // Act
            await _cacheService.SetAsync(key, value, TimeSpan.FromMinutes(10));

            // Assert
            string cachedValue = await _cacheService.GetAsync<string>(key, TimeSpan.FromSeconds(30));
            Assert.Equal(value, cachedValue);
        }

        [Fact]
        public async Task GetAsync_Should_Return_Default_Value_If_Key_Is_Not_In_Cache()
        {
            // Arrange
            string key = "nonExistingKey";

            // Act
            string cachedValue = await _cacheService.GetAsync<string>(key, TimeSpan.FromSeconds(30));

            // Assert
            Assert.Null(cachedValue);
        }

        [Fact]
        public async Task RemoveAsync_Should_Remove_Entry_From_Cache()
        {
            // Arrange
            string key = "myKey";
            string value = "myValue";

            await _cacheService.SetAsync(key, value, TimeSpan.FromMinutes(10));

            // Act
            await _cacheService.RemoveAsync(key);

            // Assert
            string cachedValue = await _cacheService.GetAsync<string>(key, TimeSpan.FromSeconds(30));
            Assert.Null(cachedValue);
        }

        [Fact]
        public async Task GetAsync_Should_Return_Default_Value_If_Entry_Is_Expired()
        {
            // Arrange
            string key = "myKey";
            string value = "myValue";
            TimeSpan expirationTime = TimeSpan.FromSeconds(1);

            await _cacheService.SetAsync(key, value, expirationTime);

            // Wait for expiration time
            await Task.Delay(expirationTime);

            // Act
            string cachedValue = await _cacheService.GetAsync<string>(key, expirationTime);

            // Assert
            Assert.Null(cachedValue);
        }

        [Fact]
        public async Task SetAsync_WhenCalled_SetsValueInCache()
        {
            // Arrange
            var key = "testKey";
            var value = "testValue";

            // Act
            await _cacheService.SetAsync(key, value, TimeSpan.FromMinutes(10));

            // Assert
            var result = await _cacheService.GetAsync<string>(key, TimeSpan.FromMinutes(1));
            Assert.Equal(value, result);
        }

        [Fact]
        public async Task GetAsync_WhenValueIsExpired_ReturnsDefaultValue()
        {
            // Arrange
            var key = "testKey";
            var value = "testValue";
            var expirationTime = TimeSpan.FromSeconds(1);

            await _cacheService.SetAsync(key, value, expirationTime);
            await Task.Delay(expirationTime + TimeSpan.FromMilliseconds(500)); // Wait for value to expire

            // Act
            var result = await _cacheService.GetAsync<string>(key, expirationTime);

            // Assert
            Assert.Equal(default(string), result);
        }

        [Fact]
        public async Task RemoveAsync_WhenCalled_RemovesValueFromCache()
        {
            // Arrange
            var key = "testKey";
            var value = "testValue";

            await _cacheService.SetAsync(key, value, TimeSpan.FromMinutes(10));

            // Act
            await _cacheService.RemoveAsync(key);

            // Assert
            var result = await _cacheService.GetAsync<string>(key, TimeSpan.Zero);
            Assert.Equal(default(string), result);
        }
    }
}

