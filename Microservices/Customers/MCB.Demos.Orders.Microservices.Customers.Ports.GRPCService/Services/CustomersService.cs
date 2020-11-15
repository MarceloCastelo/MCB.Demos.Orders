using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCB.Demos.Orders.Microservices.Customers.Ports.GRPCService.Services
{
    public class CustomersService : Customers.CustomersBase
    {
        private readonly ILogger<CustomersService> _logger;

        public CustomersService(ILogger<CustomersService> logger)
        {
            _logger = logger;
        }

        public override Task<GetCustomersReply> GetCustomers(GetCustomersRequest request, ServerCallContext context)
        {
            var reply = new GetCustomersReply();

            for (int i = 0; i < 10; i++)
            {
                reply.CustomerArray.Add(new GetCustomersReplyItem
                {
                    Code = (i + 1).ToString(),
                    Name = $"Customer {i + 1}"
                });
            }

            return Task.FromResult(reply);
        }
    }
}
