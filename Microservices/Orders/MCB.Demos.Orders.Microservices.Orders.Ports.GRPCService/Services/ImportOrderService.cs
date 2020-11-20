using Grpc.Core;
using MCB.Demos.Orders.Microservices.Orders.Ports.GRPCService.Protos.ImportOrder;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MCB.Demos.Orders.Microservices.Orders.Ports.GRPCService.Services
{
    public class ImportOrderService : Protos.ImportOrder.Orders.OrdersBase
    {
        private readonly ILogger<ImportOrderService> _logger;

        public ImportOrderService(ILogger<ImportOrderService> logger)
        {
            _logger = logger;
        }

        public async override Task<ImportOrderReply> ImportOrder(ImportOrderRequest request, ServerCallContext context)
        {
            var reply = new ImportOrderReply
            {
                Success = !string.IsNullOrWhiteSpace(request?.Order?.Code)
            };

            return await Task.FromResult(reply);
        }
    }
}
