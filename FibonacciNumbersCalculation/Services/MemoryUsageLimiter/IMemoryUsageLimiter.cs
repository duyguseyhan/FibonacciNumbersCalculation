using System;
namespace FibonacciNumbersCalculation.Services.MemoryUsageLimiter
{
	public interface IMemoryUsageLimiter
    {
        public bool IsMemoryUsageExceeded(long maxMemoryUsageInMegabytes);
    }
}

