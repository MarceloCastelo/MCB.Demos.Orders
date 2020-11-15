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
    public class OrdersController : ControllerBase
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly string _gatewayURL;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public OrdersController(IConfiguration configuration)
        {
            _gatewayURL = configuration["GatewayURL"];
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        [HttpGet]
        public async Task<OrdersResponse> Get()
        {
            var response = await _httpClient.GetAsync($"{_gatewayURL}/api/Orders/GetOrders");
            var responseContent = await response.Content.ReadAsStringAsync();

            var orderResponseString = JsonSerializer.Deserialize<OrdersResponse>(responseContent, _jsonSerializerOptions);

            return orderResponseString;
        }
    }
}
