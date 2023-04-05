using System;
using FibonacciNumbersCalculation.Services.MemoryUsageLimiter;

namespace FibonacciNumbersCalculationTest.ServicesTests
{
	public class MemoryUsageLimiterTests
	{
        [Fact]
        public void IsMemoryUsageExceeded_ReturnsFalse_WhenMemoryUsageIsBelowLimit()
        {
            // Arrange
            var limiter = new MemoryUsageLimiter();
            var maxMemoryUsageInMegabytes = 100;

            // Act
            var isExceeded = limiter.IsMemoryUsageExceeded(maxMemoryUsageInMegabytes);

            // Assert
            Assert.False(isExceeded);
        }

        [Fact]
        public void IsMemoryUsageExceeded_ReturnsTrue_WhenMemoryUsageIsAboveLimit()
        {
            // Arrange
            var limiter = new MemoryUsageLimiter();
            var maxMemoryUsageInMegabytes = 1;

            // Act
            var isExceeded = limiter.IsMemoryUsageExceeded(maxMemoryUsageInMegabytes);

            // Assert
            Assert.True(isExceeded);
        }
    }
}

