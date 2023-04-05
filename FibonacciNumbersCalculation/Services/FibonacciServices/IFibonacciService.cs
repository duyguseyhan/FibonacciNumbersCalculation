using System;
namespace FibonacciNumbersCalculation.Services.FibonacciServices
{
	public interface IFibonacciService
	{
        Task<long> GetFibonacciNumberAsync(int index);
    }
}

