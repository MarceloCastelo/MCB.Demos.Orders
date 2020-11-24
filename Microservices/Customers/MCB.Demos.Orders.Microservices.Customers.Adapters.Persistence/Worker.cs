using MCB.Demos.Orders.Microservices.Customers.Adapters.Persistence.Consumers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MCB.Demos.Orders.Microservices.Customers.Adapters.Persistence
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly CustomerEventsConsumer _customerEventsConsumer;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _customerEventsConsumer = new CustomerEventsConsumer();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _customerEventsConsumer.StartConsumer();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
