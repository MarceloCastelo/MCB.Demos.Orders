using MCB.Demos.Orders.Microservices.Customers.Application.AppServices;
using MCB.Demos.Orders.Microservices.Customers.Messages.Commands.ImportCustomerIfNotExists;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;

namespace MCB.Demos.Orders.Microservices.Customers.Consumers.Commands.Consumers
{
    public class CustomerCommandsConsumer
    {
        private readonly CustomerAppService _customerAppService;
        private IModel _channel;

        public CustomerCommandsConsumer()
        {
            _customerAppService = new CustomerAppService();
        }

        public void StartConsumer()
        {
            var connectionFactory = new RabbitMQ.Client.ConnectionFactory
            {
                HostName = "localhost",
                VirtualHost = "/",
                Port = 5672,
                UserName = "guest",
                Password = "guest",
                DispatchConsumersAsync = true
            };
            var connection = connectionFactory.CreateConnection();
            _channel = connection.CreateModel();

            var queueName = "orders::microservices::customers::commands::queue";

            _channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async  (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var command = JsonSerializer.Deserialize<ImportCustomerIfNotExistsCommand>(message);

                    var commandResult = await _customerAppService.ImportCustomerIfNotExists(command);

                    if (commandResult)
                    {
                        ea.Redelivered = false;
                        _channel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        ea.Redelivered = true;
                        _channel.BasicNack(ea.DeliveryTag, false, true);
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            };

            _channel.BasicConsume(
                queue: queueName,
                autoAck: false,
                consumerTag: "",
                noLocal: false,
                exclusive: false,
                arguments: null,
                consumer: consumer);
        }
    }
}
