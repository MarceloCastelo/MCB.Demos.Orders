﻿using MCB.Demos.Orders.Gateways.WebApp.ViewModels.Responses;
using Microsoft.AspNetCore.Mvc;

namespace MCB.Demos.Orders.Ports.OpenApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        [HttpGet]
        public CustomersResponse Get()
        {
            var ordersResponse = new CustomersResponse
            {
                CustomerArray = new Customer[10]
            };

            for (int i = 0; i < 10; i++)
                ordersResponse.CustomerArray[i] = new Customer
                {
                    Code = (i + 1).ToString(),
                    Name = $"Customer {i + 1}"
                };

            return ordersResponse;
        }
    }
}