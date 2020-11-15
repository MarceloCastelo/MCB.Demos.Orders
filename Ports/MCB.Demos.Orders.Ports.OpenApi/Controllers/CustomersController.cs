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
    public class CustomersController : ControllerBase
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly string _gatewayURL;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public CustomersController(IConfiguration configuration)
        {
            _gatewayURL = configuration["GatewayURL"];
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        [HttpGet]
        public async Task<CustomersResponse> Get()
        {
            var response = await _httpClient.GetAsync($"{_gatewayURL}/api/Customers/GetCustomers");
            var responseContent = await response.Content.ReadAsStringAsync();

            var customerResponseString = JsonSerializer.Deserialize<CustomersResponse>(responseContent, _jsonSerializerOptions);

            return customerResponseString;
        }
    }
}
