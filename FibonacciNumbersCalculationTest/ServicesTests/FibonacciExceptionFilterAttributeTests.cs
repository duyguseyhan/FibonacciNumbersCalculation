using System;
using FibonacciNumbersCalculation.Services.ExceptionFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using FibonacciNumbersCalculation.Models;

namespace FibonacciNumbersCalculationTest.ServicesTests
{
	public class FibonacciExceptionFilterAttributeTests
	{
        [Fact]
        public async Task OnExceptionAsync_Should_Return_BadRequest_When_ArgumentException_Is_Thrown()
        {
            // Arrange
            var context = new ExceptionContext(new ActionContext
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor(),
            }, new List<IFilterMetadata>());

            context.Exception = new ArgumentException("Invalid argument");
            context.RouteData.Values["controller"] = "FibonacciAPIService";
            context.RouteData.Values["action"] = "GetFibonacciSubsequenceAsync";

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request.Path).Returns("/FibonacciAPIService/GetFibonacciSubsequenceAsync");
            httpContext.Setup(x => x.Response.StatusCode).Returns(StatusCodes.Status400BadRequest);

            context.HttpContext = httpContext.Object;

            var filter = new FibonacciExceptionFilterAttribute();

            // Act
            await filter.OnExceptionAsync(context);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(context.Result);
            var errorResponse = Assert.IsType<ErrorModel>(result.Value);
            Assert.Equal("FibonacciAPIService", errorResponse.Controller);
            Assert.Equal("GetFibonacciSubsequenceAsync", errorResponse.Action);
            Assert.Equal("Invalid argument", errorResponse.ErrorMessage);
        }

        [Fact]
        public async Task OnExceptionAsync_Should_Return_InternalServerError_When_Generic_Exception_Is_Thrown()
        {
            // Arrange
            var context = new ExceptionContext(new ActionContext
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor(),
            }, new List<IFilterMetadata>());

            context.Exception = new Exception("Something went wrong");
            context.RouteData.Values["controller"] = "FibonacciAPIService";
            context.RouteData.Values["action"] = "GetFibonacciSubsequenceAsync";

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request.Path).Returns("/FibonacciAPIService/GetFibonacciSubsequenceAsync");
            httpContext.Setup(x => x.Response.StatusCode).Returns(StatusCodes.Status500InternalServerError);

            context.HttpContext = httpContext.Object;

            var filter = new FibonacciExceptionFilterAttribute();

            // Act
            await filter.OnExceptionAsync(context);

            // Assert
            var result = Assert.IsType<ObjectResult>(context.Result);
            var errorResponse = Assert.IsType<ErrorModel>(result.Value);
            Assert.Equal("FibonacciAPIService", errorResponse.Controller);
            Assert.Equal("GetFibonacciSubsequenceAsync", errorResponse.Action);
            Assert.Equal("Something went wrong", errorResponse.ErrorMessage);
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

    }
}

