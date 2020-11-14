using MCB.Demos.Orders.Gateways.WebApp.ViewModels.Payloads;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MCB.Demos.Orders.Ports.OpenApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportOrdersController
        : ControllerBase
    {
        [HttpPost]
        public async Task<bool> Post([FromBody] ImportOrdersPayload importOrdersPayload)
        {
            return await Task.FromResult(importOrdersPayload?.ImportOrderModelArray?.Length > 0 == true);
        }
    }
}
