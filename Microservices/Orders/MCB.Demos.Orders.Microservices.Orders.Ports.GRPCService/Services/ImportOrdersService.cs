using Grpc.Core;
using MCB.Demos.Orders.Microservices.Orders.Ports.GRPCService.Protos.ImportOrders;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MCB.Demos.Orders.Microservices.Orders.Ports.GRPCService.Services
{
    public class ImportOrdersService : Protos.ImportOrders.Orders.OrdersBase
    {
        private readonly ILogger<ImportOrdersService> _logger;

        public ImportOrdersService(ILogger<ImportOrdersService> logger)
        {
            _logger = logger;
        }

        public async override Task<ImportOrdersReply> ImportOrders(ImportOrdersRequest request, ServerCallContext context)
        {
            var reply = new ImportOrdersReply
            {
                Success = request?.OrderArray?.Count > 0
            };

            return await Task.FromResult(reply);
        }
    }
}
