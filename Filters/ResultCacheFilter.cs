using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System.Text;

namespace UserPortalValdiationsDBContext.Filters
{
    public class ResultCacheFilter : IResultFilter
    {
        private readonly IMemoryCache _cache;
        //  private readonly TimeSpan _duration = TimeSpan.FromSeconds(10);
        private readonly TimeSpan _duration = TimeSpan.FromMinutes(2);

        public ResultCacheFilter(IMemoryCache cache)
        {
            _cache = cache;
        }

        // FIXED: Now accepts HttpContext (common for both contexts)
        private static string BuildCacheKey(HttpContext http)
        {
            var sb = new StringBuilder();

            sb.Append(http.Request.Path);

            foreach (var q in http.Request.Query.OrderBy(x => x.Key))
            {
                sb.Append($"|{q.Key}:{q.Value}");
            }

            return sb.ToString();
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            var key = BuildCacheKey(context.HttpContext);  // FIXED

            if (_cache.TryGetValue(key, out ObjectResult cachedObj))
            {
                context.Result = cachedObj;
                return;
            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            var key = BuildCacheKey(context.HttpContext);  // FIXED

            if (context.Result is ObjectResult obj)
            {
                _cache.Set(key, obj, _duration);
            }
        }
    }
}


/*Result Filters are used to intercept, inspect, modify, or replace the FINAL RESPONSE just before it is sent to the client.*/

/*Request
   ↓
Authorization Filters
   ↓
Action Filters
   ↓
ACTION METHOD (Controller logic)
   ↓
Result Filters   👈 YOU ARE HERE
   ↓
Response sent to Browser / Client
*/

/*ResultCacheFilter exists to avoid executing the SAME expensive code again and again when the OUTPUT is the SAME.*/

// Result Filters run AFTER the action method has executed
// They deal ONLY with IActionResult / ObjectResult / ViewResult



/*Its main goal is to cache the results of controller actions in memory 
 * for a short period of time, so that repeated requests for the same URL do not trigger the action logic again and again.
 * */










/*using Microsoft.AspNetCore.Mvc;
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
*/