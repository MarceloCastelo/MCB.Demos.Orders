using MCB.Demos.Orders.Gateways.WebApp.ViewModels.Payloads;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MCB.Demos.Orders.Ports.OpenApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportOrdersController
        : ControllerBase
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly string _gatewayURL;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ImportOrdersController(IConfiguration configuration)
        {
            _gatewayURL = configuration["GatewayURL"];
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        [HttpPost]
        public async Task<string> Post([FromBody] ImportOrdersPayload importOrdersPayload)
        {
            var stringContent = new StringContent(
                JsonSerializer.Serialize(importOrdersPayload),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync($"{_gatewayURL}/api/Orders/ImportOrders", stringContent);
            var responseContentString = await response.Content.ReadAsStringAsync();

            return responseContentString;
        }
    }
}
