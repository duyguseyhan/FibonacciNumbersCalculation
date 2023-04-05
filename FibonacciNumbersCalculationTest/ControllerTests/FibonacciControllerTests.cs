using System;
using System.Text.Json;
using FibonacciNumbersCalculation.Controllers;
using FibonacciNumbersCalculation.Models;
using FibonacciNumbersCalculation.Services.FibonacciAPIServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FibonacciNumbersCalculationTest.ControllerTests
{
	public class FibonacciControllerTests
	{

        private readonly Mock<IFibonacciAPIService> _mockFibonacciAPIService;
        private readonly FibonacciController _fibonacciController;

        public FibonacciControllerTests()
        {
            _mockFibonacciAPIService = new Mock<IFibonacciAPIService>();
            _fibonacciController = new FibonacciController(_mockFibonacciAPIService.Object);
        }

        [Fact]
        public async Task GetFibonacciSubsequenceAsync_ReturnsOkResult_WhenServiceReturnsResult()
        {
            // Arrange
            var expected = "1, 1, 2, 3, 5";

            var requestModel = new FibonacciSubsequenceRequestModel
            {
                StartIndex = 1,
                EndIndex = 5,
                UseCache = true,
                MaxMemoryUsage = 100000000,
                TimeOut = 5000
            };

            _mockFibonacciAPIService.Setup(s => s.GetFibonacciSubsequenceAsync(requestModel))
                .ReturnsAsync(expected);
            // Act
            var result = await _fibonacciController.GetFibonacciSubsequenceAsync(requestModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(expected, okResult.Value);
        }

        [Fact]
        public async Task GetFibonacciSubsequenceAsync_WhenTimeoutTrue_ReturnsExpectedResult()
        {
            // Arrange

            var requestModel = new FibonacciSubsequenceRequestModel
            {
                StartIndex = 1,
                EndIndex = 20,
                UseCache = false,
                MaxMemoryUsage = 100000000,
                TimeOut = 1
            };

            var expextedResult = "{\"Result\":[1,1,2,3,5,8,13,21,34,55,89,144,233],\"MemoryUsageExceeded\":false,\"TimedOut\":true}";

            _mockFibonacciAPIService.Setup(s => s.GetFibonacciSubsequenceAsync(requestModel))
                .ReturnsAsync(expextedResult);

            var controller = new FibonacciController(_mockFibonacciAPIService.Object);

            // Act
            var result = await controller.GetFibonacciSubsequenceAsync(requestModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(expextedResult, okResult.Value);          
           
        }

    }
}

