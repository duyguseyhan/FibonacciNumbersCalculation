using System;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using FibonacciNumbersCalculation.SwaggerExtension;
using FibonacciNumbersCalculation.Services.FibonacciAPIServices;
using FibonacciNumbersCalculation.Services.ExceptionFilters;
using FibonacciNumbersCalculation.Models;

namespace FibonacciNumbersCalculation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FibonacciController : ControllerBase
	{
       
        private readonly IFibonacciAPIService _fibonacciAPIService;

        public FibonacciController(IFibonacciAPIService fibonacciAPIService)
        {
            _fibonacciAPIService = fibonacciAPIService;
        }
      
        [HttpGet("fibonacci")]
        [FibonacciExceptionFilter]

        public async Task<ActionResult<string>> GetFibonacciSubsequenceAsync([FromQuery]FibonacciSubsequenceRequestModel requestModel)
        {
            var result = await _fibonacciAPIService.GetFibonacciSubsequenceAsync(requestModel);

            return Ok(result);
        }

    }
}


