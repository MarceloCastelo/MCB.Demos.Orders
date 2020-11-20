using Grpc.Net.Client;
using MCB.Demos.Orders.Gateways.WebApp.ViewModels.Payloads;
using MCB.Demos.Orders.Gateways.WebApp.ViewModels.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MCB.Demos.Orders.Gateways.WebApp.Controllers.Orders
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly string _ordersMicroserviceURL;
        private readonly string _customersMicroserviceURL;
        private readonly string _productsMicroserviceURL;

        public OrdersController(IConfiguration configuration)
        {
            _ordersMicroserviceURL = configuration["Microservices:OrdersURL"];
            _customersMicroserviceURL = configuration["Microservices:CustomersURL"];
            _productsMicroserviceURL = configuration["Microservices:ProductsURL"];
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
        public async Task<string> ImportOrders([FromBody] ImportOrdersPayload importOrdersPayload)
        {
            var resultStringBuilder = new StringBuilder();

            var ordersChannel = GrpcChannel.ForAddress(_ordersMicroserviceURL);
            var importOrdersClient = new Microservices.Orders.Ports.GRPCService.Protos.ImportOrder.Orders.OrdersClient(ordersChannel);

            var customersChannel = GrpcChannel.ForAddress(_customersMicroserviceURL);
            var importCustomersClient = new Microservices.Customers.Ports.GRPCService.Protos.ImportCustomerIfNotExists.Customers.CustomersClient(customersChannel);

            var productsChannel = GrpcChannel.ForAddress(_productsMicroserviceURL);
            var importProductsClient = new Microservices.Products.Ports.GRPCService.Protos.ImportProductIfNotExists.Products.ProductsClient(productsChannel);

            foreach (var importOrderViewModel in importOrdersPayload.ImportOrderModelArray)
            {
                // Import Customer
                var customerReply = await importCustomersClient.ImportCustomerIfNotExistsAsync(new Microservices.Customers.Ports.GRPCService.Protos.ImportCustomerIfNotExists.ImportCustomerIfNotExistsRequest
                {
                    Customer = new Microservices.Customers.Ports.GRPCService.Protos.ImportCustomerIfNotExists.Customer
                    {
                        Code = importOrderViewModel.Customer.Code,
                        Name = importOrderViewModel.Customer.Name
                    }
                });
                resultStringBuilder.AppendLine($"type customer - code {importOrderViewModel.Customer.Code} - success {customerReply.Success}");

                // Import Order
                var order = new Microservices.Orders.Ports.GRPCService.Protos.ImportOrder.Order
                {
                    Code = importOrderViewModel.Code,
                    Date = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(importOrderViewModel.Data),
                    Customer = new Microservices.Orders.Ports.GRPCService.Protos.ImportOrder.Customer
                    {
                        Code = importOrderViewModel.Customer.Code,
                        Name = importOrderViewModel.Customer.Name
                    }
                };

                foreach (var orderItemModel in importOrderViewModel.OrderItemCollection)
                {
                    // Import product
                    var product = new Microservices.Products.Ports.GRPCService.Protos.ImportProductIfNotExists.Product
                    {
                        Code = orderItemModel.Product.Code,
                        Name = orderItemModel.Product.Name
                    };
                    var productReply = await importProductsClient.ImportProductIfNotExistsAsync(new Microservices.Products.Ports.GRPCService.Protos.ImportProductIfNotExists.ImportProductIfNotExistsRequest
                    {
                        Product = product
                    });
                    resultStringBuilder.AppendLine($"type product - code {product.Code} - success {productReply.Success}");

                    order.OrderItemArray.Add(new Microservices.Orders.Ports.GRPCService.Protos.ImportOrder.OrderItem
                    {
                        Product = new Microservices.Orders.Ports.GRPCService.Protos.ImportOrder.Product
                        {
                            Code = orderItemModel.Product.Code,
                            Name = orderItemModel.Product.Name
                        },
                        Quantity = Convert.ToDouble(orderItemModel.Quantity),
                        Value = Convert.ToDouble(orderItemModel.Value)
                    });
                }

                var orderReply = await importOrdersClient.ImportOrderAsync(new Microservices.Orders.Ports.GRPCService.Protos.ImportOrder.ImportOrderRequest { 
                    Order = order
                });

                resultStringBuilder.AppendLine($"type order - code {order.Code} - success {orderReply.Success}");

            }

            return resultStringBuilder.ToString();
        }
    }
}
