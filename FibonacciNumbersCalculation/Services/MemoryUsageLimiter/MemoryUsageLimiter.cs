using System;
namespace FibonacciNumbersCalculation.Services.MemoryUsageLimiter
{
    public class MemoryUsageLimiter : IMemoryUsageLimiter
    {
        private const long BytesInMegabyte = 1024 * 1024;
        private long _maxMemoryUsageInBytes;

        public bool IsMemoryUsageExceeded(long maxMemoryUsageInMegabytes)
        {
            _maxMemoryUsageInBytes = maxMemoryUsageInMegabytes * BytesInMegabyte;
            return GC.GetTotalMemory(false) > _maxMemoryUsageInBytes; ;
        } 

    }
}

