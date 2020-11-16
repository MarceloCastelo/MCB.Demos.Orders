using Grpc.Net.Client;
using MCB.Demos.Orders.Gateways.WebApp.ViewModels.Payloads;
using MCB.Demos.Orders.Gateways.WebApp.ViewModels.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
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
            var importOrdersRequest = new Microservices.Orders.Ports.GRPCService.Protos.ImportOrders.ImportOrdersRequest();
            for (int i = 0; i < 10; i++)
            {
                var order = new Microservices.Orders.Ports.GRPCService.Protos.ImportOrders.Order
                {
                    Code = $"{i + 1}",
                    Date = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow.AddDays(-i)),
                    Customer = new Microservices.Orders.Ports.GRPCService.Protos.ImportOrders.Customer
                    {
                        Code = $"{i + 1}",
                        Name = $"Customer {i + 1}"
                    }
                };

                importOrdersRequest.OrderArray.Add(order);

                for (int j = 0; j < 10; j++)
                {
                    order.OrderItemArray.Add(new Microservices.Orders.Ports.GRPCService.Protos.ImportOrders.OrderItem
                    {
                        Sequence = j + 1,
                        Product = new Microservices.Orders.Ports.GRPCService.Protos.ImportOrders.Product
                        {
                            Code = $"{j + 1}",
                            Name = $"Product {j + 1}"
                        },
                        Quantity = 105,
                        QuantityNanos = 1,
                        Value = 25010,
                        ValueNanos = 2
                    });
                }
            }

            var channel = GrpcChannel.ForAddress(_ordersMicroserviceURL);
            var client = new Microservices.Orders.Ports.GRPCService.Protos.ImportOrders.Orders.OrdersClient(channel);
            var reply = await client.ImportOrdersAsync(importOrdersRequest);

            return reply.Success;
        }
    }
}
