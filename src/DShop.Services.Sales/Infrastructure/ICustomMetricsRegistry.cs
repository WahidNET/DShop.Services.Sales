using App.Metrics;
using App.Metrics.Counter;
using MongoDB.Driver;

namespace DShop.Services.Sales.Infrastructure
{
    public interface ICustomMetricsRegistry
    {
        void IncreaseSalesMetrics();
        void IncreaseProductsEndpointRequests();
    }

    public class CustomMetricsRegistry : ICustomMetricsRegistry
    {
        private readonly IMetricsRoot _metricsRoot;

        private readonly CounterOptions _salesCounter = new CounterOptions
        {
            Name = "sales-counter"
        };
        
        private readonly CounterOptions _productsEndpointCounter = new CounterOptions
        {
            Name = "products-endpoint-counter"
        };

        public CustomMetricsRegistry(IMetricsRoot metricsRoot)
        {
            _metricsRoot = metricsRoot;
        }
        
        public void IncreaseSalesMetrics()
        {
            _metricsRoot.Measure.Counter.Increment(_salesCounter);
        }

        public void IncreaseProductsEndpointRequests()
        {
            _metricsRoot.Measure.Counter.Increment(_productsEndpointCounter);
        }
    }
}