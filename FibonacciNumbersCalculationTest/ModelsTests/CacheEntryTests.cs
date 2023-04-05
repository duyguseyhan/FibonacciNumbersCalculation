using System;
using FibonacciNumbersCalculation.Models.CacheModel;

namespace FibonacciNumbersCalculationTest.ModelsTests
{
	public class CacheEntryTests
	{
        [Fact]
        public void IsExpired_ReturnsFalse_WhenExpirationTimeHasNotPassed()
        {
            // Arrange
            var value = 42;
            var entry = new CacheEntry<int>(value);
            var expirationTime = TimeSpan.FromSeconds(30);

            // Act
            var result = entry.IsExpired(expirationTime);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsExpired_ReturnsTrue_WhenExpirationTimeHasPassed()
        {
            // Arrange
            var value = 42;
            var entry = new CacheEntry<int>(value);
            var expirationTime = TimeSpan.FromSeconds(1);

            // Simulate the entry being created 2 seconds ago
            entry.GetType().GetProperty("LastAccessTime").SetValue(entry, DateTime.UtcNow.AddSeconds(-2));

            // Act
            var result = entry.IsExpired(expirationTime);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Touch_UpdatesLastAccessTime()
        {
            // Arrange
            var value = 42;
            var entry = new CacheEntry<int>(value);
            var initialLastAccessTime = entry.LastAccessTime;

            // Act
            entry.Touch();
            var newLastAccessTime = entry.LastAccessTime;

            // Assert
            Assert.NotEqual(initialLastAccessTime, newLastAccessTime);
        }
    }
}

