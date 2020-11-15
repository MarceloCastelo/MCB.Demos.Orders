using Grpc.Net.Client;
using MCB.Demos.Orders.Gateways.WebApp.ViewModels.Payloads;
using MCB.Demos.Orders.Gateways.WebApp.ViewModels.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace MCB.Demos.Orders.Gateways.WebApp.Controllers.Orders
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly string _ordersMicroserviceURL;

        public OrdersController(IConfiguration configuration)
        {
            _ordersMicroserviceURL = configuration["Microservices:OrdersURL"];
        }

        [HttpGet("GetOrders")]
        public async Task<OrdersResponse> GetOrders()
        {
            var ordersResponse = new OrdersResponse();

            var channel = GrpcChannel.ForAddress(_ordersMicroserviceURL);
            var client = new Microservices.Orders.Ports.GRPCService.Protos.GetOrders.Orders.OrdersClient(channel);
            var reply = await client.GetOrdersAsync(new Microservices.Orders.Ports.GRPCService.Protos.GetOrders.GetOrdersRequest());

            ordersResponse.OrderArray = new Order[reply.OrderArray.Count];

            for (int i = 0; i < reply.OrderArray.Count; i++)
            {
                ordersResponse.OrderArray[i] = new Order
                {
                    Code = reply.OrderArray[i].Code,
                    Date = reply.OrderArray[i].Date.ToDateTime()
                };
            }

            return ordersResponse;
        }

        [HttpPost("ImportOrders")]
        public async Task<bool> ImportOrders([FromBody] ImportOrdersPayload importOrdersPayload)
        {
            return await Task.FromResult(importOrdersPayload?.ImportOrderModelArray?.Length > 0 == true);
        }
    }
}
