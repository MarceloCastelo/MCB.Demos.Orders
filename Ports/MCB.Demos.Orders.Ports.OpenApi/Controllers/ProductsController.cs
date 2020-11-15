using MCB.Demos.Orders.Gateways.WebApp.ViewModels.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MCB.Demos.Orders.Ports.OpenApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly string _gatewayURL;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ProductsController(IConfiguration configuration)
        {
            _gatewayURL = configuration["GatewayURL"];
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        [HttpGet]
        public async Task<ProductsResponse> Get()
        {
            var response = await _httpClient.GetAsync($"{_gatewayURL}/api/Products/GetProducts");
            var responseContent = await response.Content.ReadAsStringAsync();

            var productResponseString = JsonSerializer.Deserialize<ProductsResponse>(responseContent, _jsonSerializerOptions);

            return productResponseString;
        }
    }
}
