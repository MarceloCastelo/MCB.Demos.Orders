using MCB.Demos.Orders.Microservices.Customers.Consumers.Commands.Consumers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MCB.Demos.Orders.Microservices.Customers.Consumers.Commands
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly CustomerCommandsConsumer _customerCommandsConsumer;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _customerCommandsConsumer = new CustomerCommandsConsumer();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _customerCommandsConsumer.StartConsumer();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
