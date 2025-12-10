using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Text;
using System.Threading.Tasks;

namespace UserPortalValdiationsDBContext.Filters
{
    public class ResultCacheFilter : IAsyncResultFilter
    {
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _duration;

        public ResultCacheFilter(IMemoryCache cache, TimeSpan duration)
        {
            _cache = cache;
            _duration = duration;
        }

        private static string BuildCacheKey(ResultExecutingContext context)
        {
            var sb = new StringBuilder();
            sb.Append(context.HttpContext.Request.Path);
            foreach (var q in context.HttpContext.Request.Query.OrderBy(k => k.Key))
            {
                sb.Append($"|{q.Key}:{q.Value}");
            }
            return sb.ToString();
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var key = BuildCacheKey(context);
            if (_cache.TryGetValue(key, out ObjectResult cachedObj))
            {
                context.Result = cachedObj;
                return;
            }

            var executedContext = await next();

            if (executedContext.Result is ObjectResult obj)
            {
                _cache.Set(key, obj, _duration);
            }
        }
    }
}
