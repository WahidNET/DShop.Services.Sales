using System.Threading.Tasks;
using DShop.Common.Handlers;
using DShop.Common.RabbitMq;
using DShop.Services.Sales.Core.Domain;
using DShop.Services.Sales.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace DShop.Services.Sales.Services.Events.Handlers
{
    public class ProductCreatedHandler : IEventHandler<ProductCreated>
    {
        private readonly ILogger<ProductCreatedHandler> _logger;
        private readonly IProductRepository _productRepository;

        public ProductCreatedHandler(ILogger<ProductCreatedHandler> logger,
            IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }
        
        public Task HandleAsync(ProductCreated @event, ICorrelationContext context)
        {
            _logger.LogInformation($"Product: {@event.Name}");
            
            return _productRepository.AddAsync(new Product(@event.Id,
                @event.Name, @event.Vendor, @event.Price));
        }
    }
}