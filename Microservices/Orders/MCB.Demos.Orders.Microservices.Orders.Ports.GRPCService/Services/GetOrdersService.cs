using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MCB.Demos.Orders.Microservices.Orders.Ports.GRPCService.Protos.GetOrders;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MCB.Demos.Orders.Microservices.Orders.Ports.GRPCService.Services
{
    public class GetOrdersService : Protos.GetOrders.Orders.OrdersBase
    {
        private readonly ILogger<GetOrdersService> _logger;

        public GetOrdersService(ILogger<GetOrdersService> logger)
        {
            _logger = logger;
        }

        public async override Task<GetOrdersReply> GetOrders(GetOrdersRequest request, ServerCallContext context)
        {
            var reply = new GetOrdersReply();

            for (int i = 0; i < 10; i++)
            {
                reply.OrderArray.Add(new Order
                {
                    Code = (i + 1).ToString(),
                    Date = Timestamp.FromDateTime(DateTime.UtcNow.AddDays(-i))
                });
            }

            return await Task.FromResult(reply);
        }
    }
}
