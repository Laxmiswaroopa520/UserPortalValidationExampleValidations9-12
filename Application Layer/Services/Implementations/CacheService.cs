using UserPortalValdiationsDBContext.Services.Interfaces;

namespace UserPortalValdiationsDBContext.Services.Implementations
{
    public class CacheService : ICacheService
    {
        private readonly Dictionary<string, object> _cache = new();

        public void Set(string key, object value) => _cache[key] = value;

        public object? Get(string key) => _cache.ContainsKey(key) ? _cache[key] : null;
    }
}
