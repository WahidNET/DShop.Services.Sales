using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DShop.Services.Sales.Services.Clients
{
    public interface IOrdersServiceClient
    {
        Task<object> GetAsync(Guid id);
    }

    public class OrdersServiceClient : IOrdersServiceClient
    {
        private readonly HttpClient _httpClient;

        public OrdersServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<object> GetAsync(Guid id)
        {
            var response = await _httpClient
                .GetAsync($"http://localhost:5005/orders/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var order = JsonConvert.DeserializeObject(content);

            return order;
        }
    }
}