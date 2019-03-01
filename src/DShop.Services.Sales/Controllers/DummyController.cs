using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Registry;
using DShop.Common.Dispatchers;
using DShop.Services.Sales.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;

namespace DShop.Services.Sales.Controllers
{
    public class DummyController : BaseController
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;
        private readonly ICustomMetricsRegistry _metricsRegistry;

        public DummyController(IDispatcher dispatcher,
                IMemoryCache memoryCache, ICustomMetricsRegistry metricsRegistry) : base(dispatcher)
        {
            _memoryCache = memoryCache;
//            _distributedCache = distributedCache;
//            _database = database;
            _metricsRegistry = metricsRegistry;
        }
        
        [HttpGet("sales")]
        public IActionResult Sales()
        {
            _metricsRegistry.IncreaseSalesMetrics();

            return Ok();
        }

        [HttpGet("error")]
        public IActionResult Error()
        {
            throw new ArgumentException("ooops", "arg1");
        }

        [HttpGet("memory-cache")]
//        [ResponseCache]
        public ActionResult<string> GetFromMemoryCache()
        {
            var value = "test";
            _memoryCache.Set("key", value, TimeSpan.FromSeconds(10));

            return _memoryCache.Get<string>("key");
        }

        [HttpGet("redis-cache")]
//        [ResponseCache]
        public async Task<ActionResult<string>> GetFromRedisCache()
        {
            var value = "test";
            await _distributedCache.SetStringAsync("key", value, new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(10)
            });
            
            return await _distributedCache.GetStringAsync("key");
        }
    }
}