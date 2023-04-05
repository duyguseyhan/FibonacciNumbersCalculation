using System;
using FibonacciNumbersCalculation.Models;

namespace FibonacciNumbersCalculation.Services.FibonacciAPIServices
{
	public interface IFibonacciAPIService
	{

        Task<string> GetFibonacciSubsequenceAsync(FibonacciSubsequenceRequestModel requestModel);
    }
}

