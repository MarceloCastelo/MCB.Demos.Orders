using Grpc.Core;
using MCB.Demos.Orders.Microservices.Customers.Ports.GRPCService.Protos.ImportCustomerIfNotExists;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MCB.Demos.Orders.Microservices.Customers.Ports.GRPCService.Services
{
    public class ImportCustomerService : Protos.ImportCustomerIfNotExists.Customers.CustomersBase
    {
        private readonly ILogger<ImportCustomerService> _logger;

        public ImportCustomerService(ILogger<ImportCustomerService> logger)
        {
            _logger = logger;
        }

        public async override Task<ImportCustomerIfNotExistsReply> ImportCustomerIfNotExists(ImportCustomerIfNotExistsRequest request, ServerCallContext context)
        {
            var reply = new ImportCustomerIfNotExistsReply
            {
                Success = !string.IsNullOrWhiteSpace(request?.Customer?.Code)
            };

            return await Task.FromResult(reply);
        }
    }
}
