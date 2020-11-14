using MCB.Demos.Orders.Gateways.WebApp.ViewModels.Responses;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MCB.Demos.Orders.Ports.OpenApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        [HttpGet]
        public OrdersResponse Get()
        {
            var ordersResponse = new OrdersResponse
            {
                OrderArray = new Order[10]
            };

            for (int i = 0; i < 10; i++)
                ordersResponse.OrderArray[i] = new Order
                {
                    Code = (i + 1).ToString(),
                    Date = DateTime.UtcNow.AddDays(-i)
                };

            return ordersResponse;
        }
    }
}
