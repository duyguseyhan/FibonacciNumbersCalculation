using System;
using FibonacciNumbersCalculation.Models.CacheModel;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;
using FibonacciNumbersCalculation.Services.Lock;

namespace FibonacciNumbersCalculation.Services.CacheServices
{
    public class CacheService : ICacheService
    {
        private readonly MemoryCache _cache;
        private readonly AsyncLock _asyncLock = new AsyncLock();

        public CacheService()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan timeSpan)
        {
            var entry = new CacheEntry<T>(value);

            using (await _asyncLock.LockAsync().ConfigureAwait(false))
            {
                await Task.Run(() => _cache.Set(key, entry)).ConfigureAwait(false);
            }
        }

        public async Task<T> GetAsync<T>(string key, TimeSpan timeSpan)
        {
            using (await _asyncLock.LockAsync().ConfigureAwait(false))
            {
                if (_cache.TryGetValue(key, out CacheEntry<T> entry))
                {
                    if (!entry.IsExpired(timeSpan))
                    {
                        entry.Touch();
                        return entry.Value;
                    }

                    await RemoveAsync(key).ConfigureAwait(false);
                }

                return default;
            }
        }

        public async Task RemoveAsync(string key)
        {

            await Task.Run(() => _cache.Remove(key)).ConfigureAwait(false);
 
        }
    }

}

