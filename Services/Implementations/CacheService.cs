using System;
using Microsoft.Extensions.Caching.Memory;
using UserPortalValdiationsDBContext.Services.Interfaces;

namespace UserPortalValdiationsDBContext.Services.Implementations
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Set(string key, object value, TimeSpan? expiration = null)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(10) // default 10 min
            };

            _memoryCache.Set(key, value, options);
        }

        public object? Get(string key)
        {
            _memoryCache.TryGetValue(key, out var value);
            return value;
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
