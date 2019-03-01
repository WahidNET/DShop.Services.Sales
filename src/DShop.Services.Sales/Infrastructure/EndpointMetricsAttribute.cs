using Microsoft.AspNetCore.Mvc.Filters;

namespace DShop.Services.Sales.Infrastructure
{
    public class EndpointMetricsAttribute : ActionFilterAttribute
    {
        private readonly string _endpoint;

        public EndpointMetricsAttribute(string endpoint)
        {
            _endpoint = endpoint;
        }
        
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var metricsRegistry = context.HttpContext.RequestServices
                .GetService(typeof(ICustomMetricsRegistry)) as ICustomMetricsRegistry;

            switch (_endpoint)
            {
                default: metricsRegistry.IncreaseProductsEndpointRequests();
                    break;
            }
            base.OnActionExecuted(context);
        }
    }
}