using Grpc.Core;
using MCB.Demos.Orders.Microservices.Customers.Ports.GRPCService.Protos.GetCustomers;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MCB.Demos.Orders.Microservices.Customers.Ports.GRPCService.Services
{
    public class GetCustomersService : Protos.GetCustomers.Customers.CustomersBase
    {
        private readonly ILogger<GetCustomersService> _logger;

        public GetCustomersService(ILogger<GetCustomersService> logger)
        {
            _logger = logger;
        }

        public async override Task<GetCustomersReply> GetCustomers(GetCustomersRequest request, ServerCallContext context)
        {
            var reply = new GetCustomersReply();

            for (int i = 0; i < 10; i++)
            {
                reply.CustomerArray.Add(new Customer
                {
                    Code = (i + 1).ToString(),
                    Name = $"Customer {i + 1}"
                });
            }

            return await Task.FromResult(reply);
        }
    }
}
