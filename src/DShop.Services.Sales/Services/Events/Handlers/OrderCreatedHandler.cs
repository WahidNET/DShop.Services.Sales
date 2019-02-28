using System.Threading.Tasks;
using DShop.Common.Consul;
using DShop.Common.Fabio;
using DShop.Common.Handlers;
using DShop.Common.RabbitMq;
using DShop.Services.Sales.Core.Domain;
using DShop.Services.Sales.Core.Repositories;
using DShop.Services.Sales.Services.Clients;

namespace DShop.Services.Sales.Services.Events.Handlers
{
    public class OrderCreatedHandler : IEventHandler<OrderCreated>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrdersServiceClient _ordersServiceClient;

        public OrderCreatedHandler(IOrderRepository orderRepository,
            IOrdersServiceClient ordersServiceClient)
        {
            _orderRepository = orderRepository;
            _ordersServiceClient = ordersServiceClient;
        }
        
        public async Task HandleAsync(OrderCreated @event, ICorrelationContext context)
        {
            var orderDto = await _ordersServiceClient.GetAsync(@event.Id);
//            var order = new Order(@event.Id,)

//            _orderRepository.AddAsync();
        }
    }
}