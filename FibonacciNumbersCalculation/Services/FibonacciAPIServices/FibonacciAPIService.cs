using System;
using System.Diagnostics;
using System.Text.Json;
using FibonacciNumbersCalculation.Services.MemoryUsageLimiter;
using FibonacciNumbersCalculation.Services.ExecutionTimeLimiter;
using FibonacciNumbersCalculation.Services.CacheServices;
using FibonacciNumbersCalculation.Services.ExceptionFilters;
using FibonacciNumbersCalculation.Services.FibonacciServices;
using FibonacciNumbersCalculation.Models.CacheModel;
using FibonacciNumbersCalculation.Models.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using FibonacciNumbersCalculation.Models;

namespace FibonacciNumbersCalculation.Services.FibonacciAPIServices
{
    
    public class FibonacciAPIService : IFibonacciAPIService
    {
        private readonly IFibonacciService _fibonacciService;
        private readonly ICacheService _cacheService;
        private readonly IMemoryUsageLimiter _memoryUsageLimiter;
        private readonly IExecutionTimeLimiter _executionTimeLimiter;

        public FibonacciAPIService(IFibonacciService fibonacciService, ICacheService cacheService, IMemoryUsageLimiter memoryUsageLimiter, IExecutionTimeLimiter executionTimeLimiter)
        {
            _fibonacciService = fibonacciService;
            _cacheService = cacheService;
            _memoryUsageLimiter = memoryUsageLimiter;
            _executionTimeLimiter = executionTimeLimiter;
        }

        public async Task<string> GetFibonacciSubsequenceAsync(FibonacciSubsequenceRequestModel requestModel)
        {
            _executionTimeLimiter.StartTimer(requestModel.TimeOut);
            var result = new ConcurrentBag<long>();
            var timeOutResult = false;

            await Task.Run(() => Parallel.ForEach(Enumerable.Range(requestModel.StartIndex, requestModel.EndIndex - requestModel.StartIndex + 1), (i, loopState) =>
            {
                var fibonacciNumber = default(long);

                if (requestModel.UseCache)
                {
                    var cacheKey = $"Fibonacci_{i}";
                    var cacheEntry = _cacheService.GetAsync<CacheEntry<long>>(cacheKey, TimeSpan.FromMinutes(ConfigurationConstants.CacheExpirationTimeInMinutes)).GetAwaiter().GetResult();

                    if (cacheEntry != null && !cacheEntry.IsExpired(TimeSpan.FromMinutes(ConfigurationConstants.CacheExpirationTimeInMinutes)))
                    {
                        fibonacciNumber = cacheEntry.Value;
                    }
                }
                if (fibonacciNumber == default)
                {
                    var computationTask = _fibonacciService.GetFibonacciNumberAsync(i);

                    fibonacciNumber = computationTask.GetAwaiter().GetResult();

                    var cacheExpirationTimeInMinutes = ConfigurationConstants.CacheExpirationTimeInMinutes;
                    var cacheEntry = new CacheEntry<long>(fibonacciNumber);
                    _cacheService.SetAsync($"Fibonacci_{i}", cacheEntry, TimeSpan.FromMinutes(cacheExpirationTimeInMinutes)).GetAwaiter().GetResult();


                    if (_memoryUsageLimiter.IsMemoryUsageExceeded(requestModel.MaxMemoryUsage))
                    {
                        result.Add(fibonacciNumber);
                        loopState.Stop();
                        return;
                    }

                    if (_executionTimeLimiter.IsTimeLimitExceeded())
                    {
                        result.Add(fibonacciNumber);
                        timeOutResult = true;
                        loopState.Stop();
                        return;
                    }

                    Task.Delay(500).GetAwaiter().GetResult();
                }

                result.Add(fibonacciNumber);
            }));



            var response = new 
            {
                Result = result.OrderBy(s=>s),
                MemoryUsageExceeded = _memoryUsageLimiter.IsMemoryUsageExceeded(requestModel.MaxMemoryUsage),
                TimeOut = timeOutResult
            };

            return JsonSerializer.Serialize(response);
        }
    }
}

