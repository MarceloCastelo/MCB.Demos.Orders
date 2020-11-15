using MCB.Demos.Orders.Gateways.WebApp.ViewModels.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MCB.Demos.Orders.Gateways.WebApp.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly string _productsMicroserviceURL;

        public ProductsController(IConfiguration configuration)
        {
            _productsMicroserviceURL = configuration["Microservices:ProductsURL"];
        }

        [HttpGet("GetProducts")]
        public ProductsResponse GetProducts()
        {
            var ordersResponse = new ProductsResponse
            {
                ProductArray = new Product[10]
            };

            for (int i = 0; i < 10; i++)
                ordersResponse.ProductArray[i] = new Product
                {
                    Code = (i + 1).ToString(),
                    Name = $"Product {i + 1}"
                };

            return ordersResponse;
        }
    }
}
