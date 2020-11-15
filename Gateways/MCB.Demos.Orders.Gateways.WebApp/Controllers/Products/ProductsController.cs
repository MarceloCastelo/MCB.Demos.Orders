using Grpc.Net.Client;
using MCB.Demos.Orders.Gateways.WebApp.ViewModels.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace MCB.Demos.Orders.Gateways.WebApp.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly string _productsMicroserviceURL;

        public ProductsController(IConfiguration configuration)
        {
            _productsMicroserviceURL = configuration["Microservices:ProductsURL"];
        }

        [HttpGet("GetProducts")]
        public async Task<ProductsResponse> GetProducts()
        {
            var productsResponse = new ProductsResponse();

            var channel = GrpcChannel.ForAddress(_productsMicroserviceURL);
            var client = new Microservices.Products.Ports.GRPCService.Protos.GetProducts.Products.ProductsClient(channel);
            var reply = await client.GetProductsAsync(new Microservices.Products.Ports.GRPCService.Protos.GetProducts.GetProductsRequest());

            productsResponse.ProductArray = new Product[reply.ProductArray.Count];

            for (int i = 0; i < reply.ProductArray.Count; i++)
            {
                productsResponse.ProductArray[i] = new Product
                {
                    Code = reply.ProductArray[i].Code,
                    Name = reply.ProductArray[i].Name
                };
            }

            return productsResponse;
        }
    }
}
