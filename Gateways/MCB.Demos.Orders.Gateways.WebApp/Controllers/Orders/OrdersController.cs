using MCB.Demos.Orders.Gateways.WebApp.ViewModels.Payloads;
using MCB.Demos.Orders.Gateways.WebApp.ViewModels.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MCB.Demos.Orders.Gateways.WebApp.Controllers.Orders
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        [HttpGet("GetOrders")]
        public async Task<OrdersResponse> GetOrders()
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

            return await Task.FromResult(ordersResponse);
        }

        [HttpPost("ImportOrders")]
        public async Task<bool> ImportOrders([FromBody] ImportOrdersPayload importOrdersPayload)
        {
            return await Task.FromResult(importOrdersPayload?.ImportOrderModelArray?.Length > 0 == true);
        }
    }
}
