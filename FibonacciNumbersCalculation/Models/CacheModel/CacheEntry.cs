using System;
namespace FibonacciNumbersCalculation.Models.CacheModel
{
	public class CacheEntry<TValue>
    {
        public CacheEntry(TValue value)
        {
            Value = value;
            LastAccessTime = DateTime.UtcNow;
        }
        public TValue Value { get; }
        public DateTime LastAccessTime { get; private set; }

        public bool IsExpired(TimeSpan expirationTime) => DateTime.UtcNow >= LastAccessTime + expirationTime;

        public void Touch() => LastAccessTime = DateTime.UtcNow;
    }
}

