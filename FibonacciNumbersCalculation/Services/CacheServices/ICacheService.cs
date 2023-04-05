using System;
namespace FibonacciNumbersCalculation.Services.CacheServices
{
	public interface ICacheService
	{
        Task SetAsync<T>(string key, T value, TimeSpan timeSpan);
        Task<T> GetAsync<T>(string key, TimeSpan timeSpan);
        Task RemoveAsync(string key);
    }
}

