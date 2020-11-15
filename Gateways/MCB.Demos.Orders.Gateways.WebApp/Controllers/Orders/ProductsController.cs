using MCB.Demos.Orders.Gateways.WebApp.ViewModels.Responses;
using Microsoft.AspNetCore.Mvc;

namespace MCB.Demos.Orders.Gateways.WebApp.Controllers.Orders
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
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
