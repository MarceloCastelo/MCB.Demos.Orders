using Grpc.Core;
using MCB.Demos.Orders.Microservices.Products.Ports.GRPCService.Protos.ImportProductIfNotExists;
using System.Threading.Tasks;

namespace MCB.Demos.Orders.Microservices.Products.Ports.GRPCService.Services
{
    public class ImportProductIfNotExistsService : Protos.ImportProductIfNotExists.Products.ProductsBase
    {
        public override async Task<ImportProductIfNotExistsReply> ImportProductIfNotExists(ImportProductIfNotExistsRequest request, ServerCallContext context)
        {
            return await Task.FromResult(new ImportProductIfNotExistsReply() { 
                Success = !string.IsNullOrWhiteSpace(request?.Product?.Code)
            });
        }
    }
}
