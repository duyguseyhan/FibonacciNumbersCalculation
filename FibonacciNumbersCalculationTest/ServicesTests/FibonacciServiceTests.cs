using System;
using System.Collections.Concurrent;
using FibonacciNumbersCalculation.Services.FibonacciServices;
using Moq;
using Xunit;

namespace FibonacciNumbersCalculationTest.ServicesTests
{
	public class FibonacciServiceTests
	{
        private readonly IFibonacciService _fibonacciService;

        public FibonacciServiceTests()
        {
            _fibonacciService = new FibonacciService();
        }

        [Fact]
        public async Task GivenIndexLessThanTwo_WhenGetFibonacciNumberAsyncCalled_ThenReturnIndex()
        {
            // Arrange
            int index = 0;
            long expected = 0;

            // Act
            long actual = await _fibonacciService.GetFibonacciNumberAsync(index);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GivenIndexGreaterThanOne_WhenGetFibonacciNumberAsyncCalled_ThenReturnCorrectFibonacciNumber()
        {
            // Arrange
            int index = 6;
            long expected = 8;

            // Act
            long actual = await _fibonacciService.GetFibonacciNumberAsync(index);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GivenNegativeIndex_WhenGetFibonacciNumberAsyncCalled_ThenThrowArgumentException()
        {
            // Arrange
            int index = -1;

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _fibonacciService.GetFibonacciNumberAsync(index));
        }

    }
}

