using Grpc.Net.Client;
using MCB.Demos.Orders.Gateways.WebApp.ViewModels.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace MCB.Demos.Orders.Gateways.WebApp.Controllers.Orders
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly string _customersMicroserviceURL;

        public CustomersController(IConfiguration configuration)
        {
            _customersMicroserviceURL = configuration["Microservices:CustomersURL"];
        }

        [HttpGet("GetCustomers")]
        public async Task<CustomersResponse> GetCustomers()
        {
            var channel = GrpcChannel.ForAddress(_customersMicroserviceURL);
            var client = new Microservices.Customers.Ports.GRPCService.Customers.CustomersClient(channel);
            var reply = await client.GetCustomersAsync(new Microservices.Customers.Ports.GRPCService.GetCustomersRequest());

            var customersResponse = new CustomersResponse();
            customersResponse.CustomerArray = new Customer[reply.CustomerArray.Count];

            for (int i = 0; i < reply.CustomerArray.Count; i++)
            {
                customersResponse.CustomerArray[i] = new Customer
                {
                    Code = reply.CustomerArray[i].Code,
                    Name = reply.CustomerArray[i].Name
                };
            }

            return customersResponse;
        }
    }
}
