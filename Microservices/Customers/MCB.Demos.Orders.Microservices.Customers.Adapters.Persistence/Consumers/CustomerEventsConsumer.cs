using MCB.Demos.Orders.Microservices.Customers.Messages.Commands.ImportCustomerIfNotExists;
using MCB.Demos.Orders.Microservices.Customers.Messages.ImportCustomerIfNotExists.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MCB.Demos.Orders.Microservices.Customers.Adapters.Persistence.Consumers
{
    public class CustomerEventsConsumer
    {
        private IModel _channel;

        public CustomerEventsConsumer()
        {

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

            var exchangeName = "orders::microservices::customers::events::exchange";
            var queueName = $"orders::microservices::customers::events::queue::persistence";

            _channel.ExchangeDeclare(
                exchange: exchangeName,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                arguments: null
            );
            _channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
            _channel.QueueBind(
                queue: queueName,
                exchange: exchangeName,
                routingKey: typeof(CustomerWasNotImportedSuccessfullyEvent).FullName,
                arguments: null
            );
            _channel.QueueBind(
                queue: queueName,
                exchange: exchangeName,
                routingKey: typeof(CustomerWasSuccessfullyImportedEvent).FullName,
                arguments: null
            );
            _channel.QueueBind(
                queue: queueName,
                exchange: exchangeName,
                routingKey: typeof(ImportCustomerIfNotExistsFailedEvent).FullName,
                arguments: null
            );

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var command = JsonSerializer.Deserialize<ImportCustomerIfNotExistsCommand>(message);

                    Console.WriteLine($"Receive {ea.RoutingKey}");

                    ea.Redelivered = false;
                    _channel.BasicAck(ea.DeliveryTag, false);
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
