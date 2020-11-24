using MCB.Demos.Orders.Microservices.Customers.Domain.DomainModels;
using MCB.Demos.Orders.Microservices.Customers.Domain.DomainServices;
using MCB.Demos.Orders.Microservices.Customers.Messages.Commands.ImportCustomerIfNotExists;
using MCB.Demos.Orders.Microservices.Customers.Messages.GetCustomers.QueryResults;
using MCB.Demos.Orders.Microservices.Customers.Messages.GetCustomers.QueryResults.Models;
using MCB.Demos.Orders.Microservices.Customers.Messages.ImportCustomerIfNotExists.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MCB.Demos.Orders.Microservices.Customers.Application.AppServices
{
    public class CustomerAppService
    {
        private readonly CustomerDomainService _customerDomainService;

        public CustomerAppService()
        {
            _customerDomainService = new CustomerDomainService();
        }

        public GetCustomersQueryResult GetCustomers()
        {
            var queryResult = new GetCustomersQueryResult();
            var customerCollection = new List<CustomerDTO>();

            for (int i = 0; i < 10; i++)
                customerCollection.Add(new CustomerDTO
                {
                    Code = $"{i + 1}",
                    Name = $"Customer {i + 1}"
                });

            queryResult.CustomerCollection = customerCollection;

            return queryResult;
        }
        public async Task<bool> ImportCustomerIfNotExists(ImportCustomerIfNotExistsCommand importCustomerIfNotExistsCommand)
        {
            var customerDomainModel = new CustomerDomainModel
            {
                Code = importCustomerIfNotExistsCommand.Customer.Code,
                Name = importCustomerIfNotExistsCommand.Customer.Name
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

            var exchangeName = "orders::microservices::customers::events::exchange";

            channel.ExchangeDeclare(
                exchange: exchangeName,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                arguments: null
            );

            try
            {
                var importCustomerResult = await _customerDomainService.ImportCustomer(customerDomainModel);

                if (importCustomerResult)
                {
                    var message = JsonSerializer.Serialize(new CustomerWasSuccessfullyImportedEvent
                    {
                        ImportedCustomer = new Messages.ImportCustomerIfNotExists.Events.Models.Customer
                        {
                            Code = customerDomainModel.Code,
                            Name = customerDomainModel.Name
                        }
                    });
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(
                        exchange: exchangeName,
                        routingKey: typeof(CustomerWasSuccessfullyImportedEvent).FullName,
                        mandatory: false,
                        basicProperties: null,
                        body: body
                    );
                }
                else
                {
                    var message = JsonSerializer.Serialize(new CustomerWasNotImportedSuccessfullyEvent
                    {
                        Customer = new Messages.ImportCustomerIfNotExists.Events.Models.Customer
                        {
                            Code = customerDomainModel.Code,
                            Name = customerDomainModel.Name
                        }
                    });
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(
                        exchange: exchangeName,
                        routingKey: typeof(CustomerWasNotImportedSuccessfullyEvent).FullName,
                        mandatory: false,
                        basicProperties: null,
                        body: body
                    );
                }

                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                var message = JsonSerializer.Serialize(new ImportCustomerIfNotExistsFailedEvent
                {
                    Customer = new Messages.ImportCustomerIfNotExists.Events.Models.Customer
                    {
                        Code = importCustomerIfNotExistsCommand.Customer.Code,
                        Name = importCustomerIfNotExistsCommand.Customer.Name
                    }
                });
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(
                    exchange: exchangeName,
                    routingKey: typeof(ImportCustomerIfNotExistsFailedEvent).FullName,
                    mandatory: false,
                    basicProperties: null,
                    body: body
                );

                return await Task.FromResult(false);
            }
        }
    }
}
