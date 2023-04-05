using System;
using FibonacciNumbersCalculation.Services.CacheServices;
using FibonacciNumbersCalculation.Services.ExecutionTimeLimiter;
using Moq;

namespace FibonacciNumbersCalculationTest.ServicesTests
{
	public class ExecutionTimeLimiterTests
	{
        private readonly IExecutionTimeLimiter executionTimeLimiter;

        public ExecutionTimeLimiterTests()
        {
            executionTimeLimiter = new ExecutionTimeLimiter();
        }
        [Fact]
        public void IsTimeLimitExceeded_ReturnsFalse_WhenExecutionTimeIsWithinTimeLimit()
        {
            // Arrange
            var executionTime = 1000; // 1 second

            // Act
            executionTimeLimiter.StartTimer(executionTime);
            var isTimeLimitExceeded = executionTimeLimiter.IsTimeLimitExceeded();

            // Assert
            Assert.False(isTimeLimitExceeded);
        }

        [Fact]
        public void IsTimeLimitExceeded_ReturnsTrue_WhenExecutionTimeExceedsTimeLimit()
        {
            // Arrange
            var executionTime = 100; // 100 milliseconds

            // Act
            executionTimeLimiter.StartTimer(executionTime);          
            Thread.Sleep(executionTime + 50); // Sleep for longer than the time limit

            var isTimeLimitExceeded = executionTimeLimiter.IsTimeLimitExceeded();

            // Assert
            Assert.True(isTimeLimitExceeded);
        }
    }
}

