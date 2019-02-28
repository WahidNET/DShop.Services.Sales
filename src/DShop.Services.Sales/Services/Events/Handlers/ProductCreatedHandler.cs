using System.Threading.Tasks;
using DShop.Common.Handlers;
using DShop.Common.RabbitMq;
using Microsoft.Extensions.Logging;

namespace DShop.Services.Sales.Services.Events.Handlers
{
    public class ProductCreatedHandler : IEventHandler<ProductCreated>
    {
        private readonly ILogger<ProductCreatedHandler> _logger;

        public ProductCreatedHandler(ILogger<ProductCreatedHandler> logger)
        {
            _logger = logger;
        }
        
        public Task HandleAsync(ProductCreated @event, ICorrelationContext context)
        {
            _logger.LogInformation($"Product: {@event.Name}");

            return Task.CompletedTask;
        }
    }
}