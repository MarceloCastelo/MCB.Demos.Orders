using MCB.Demos.Orders.Gateways.WebApp.ViewModels.Responses;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MCB.Demos.Orders.Ports.OpenApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public ProductsResponse Get()
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
