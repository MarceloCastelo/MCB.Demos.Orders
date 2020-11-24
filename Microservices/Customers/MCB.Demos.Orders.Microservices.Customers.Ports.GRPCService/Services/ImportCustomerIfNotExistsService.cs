using Grpc.Core;
using MCB.Demos.Orders.Microservices.Customers.Messages.Commands.ImportCustomerIfNotExists;
using MCB.Demos.Orders.Microservices.Customers.Ports.GRPCService.Protos.ImportCustomerIfNotExists;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MCB.Demos.Orders.Microservices.Customers.Ports.GRPCService.Services
{
    public class ImportCustomerIfNotExistsService : Protos.ImportCustomerIfNotExists.Customers.CustomersBase
    {
        private readonly ILogger<ImportCustomerIfNotExistsService> _logger;

        public ImportCustomerIfNotExistsService(
            ILogger<ImportCustomerIfNotExistsService> logger
            )
        {
            _logger = logger;
        }

        public async override Task<ImportCustomerIfNotExistsReply> ImportCustomerIfNotExists(ImportCustomerIfNotExistsRequest request, ServerCallContext context)
        {
            var reply = new ImportCustomerIfNotExistsReply();

            var command = new ImportCustomerIfNotExistsCommand
            {
                Customer = new Messages.Commands.ImportCustomerIfNotExists.Models.Customer
                {
                    Code = request.Customer.Code,
                    Name = request.Customer.Name
                }
            };

            var connectionFactory = new RabbitMQ.Client.ConnectionFactory
            {
                HostName = "localhost",
                VirtualHost = "/",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            using var connection = connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            var queueName = "orders::microservices::customers::commands::queue";

            channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var message = JsonSerializer.Serialize(command);
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                mandatory: false,
                basicProperties: null,
                body: body
            );

            reply.Success = true;
            return await Task.FromResult(reply);
        }
    }
}
