using System;
using System.Threading.Tasks;
using RestEase;

namespace DShop.Services.Sales.Services.Clients
{
    public interface IOrdersServiceClient
    {
        [Get("orders/{id}")]
        Task<object> GetAsync([Path] Guid id);
    }
}