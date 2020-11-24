using Grpc.Core;
using MCB.Demos.Orders.Microservices.Customers.Application.AppServices;
using MCB.Demos.Orders.Microservices.Customers.Ports.GRPCService.Protos.GetCustomers;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MCB.Demos.Orders.Microservices.Customers.Ports.GRPCService.Services
{
    public class GetCustomersService : Protos.GetCustomers.Customers.CustomersBase
    {
        private readonly ILogger<GetCustomersService> _logger;
        private readonly CustomerAppService _customerAppService;

        public GetCustomersService(ILogger<GetCustomersService> logger)
        {
            _logger = logger;
            _customerAppService = new CustomerAppService();
        }

        public async override Task<GetCustomersReply> GetCustomers(GetCustomersRequest request, ServerCallContext context)
        {
            var reply = new GetCustomersReply();

            foreach (var customerDTO in _customerAppService.GetCustomers().CustomerCollection)
            {
                reply.CustomerArray.Add(new Customer
                {
                    Code = customerDTO.Code,
                    Name = customerDTO.Name
                });
            }

            return await Task.FromResult(reply);
        }
    }
}
