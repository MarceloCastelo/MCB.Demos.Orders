using Grpc.Core;
using MCB.Demos.Orders.Microservices.Products.Ports.GRPCService.Protos.GetProducts;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MCB.Demos.Orders.Microservices.Products.Ports.GRPCService
{
    public class GetProductsService : Protos.GetProducts.Products.ProductsBase
    {
        private readonly ILogger<GetProductsService> _logger;

        public GetProductsService(ILogger<GetProductsService> logger)
        {
            _logger = logger;
        }

        public async override Task<GetProductsReply> GetProducts(GetProductsRequest request, ServerCallContext context)
        {
            var reply = new GetProductsReply();

            for (int i = 0; i < 10; i++)
            {
                reply.ProductArray.Add(new Product
                {
                    Code = (i + 1).ToString(),
                    Name = $"Product {i + 1}"
                });
            }

            return await Task.FromResult(reply);
        }
    }
}
