using System.Collections.Generic;
using System.Threading.Tasks;
using DShop.Common.Dispatchers;
using DShop.Services.Sales.Infrastructure;
using DShop.Services.Sales.Services;
using DShop.Services.Sales.Services.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DShop.Services.Sales.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService,
            IDispatcher dispatcher) : base(dispatcher)
        {
            _productService = productService;
        }

        [HttpGet]
        [EndpointMetrics("products")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Get()
            => Ok(await _productService.GetAllAsync());
    }
}