using MCB.Demos.Orders.Microservices.Customers.Messages.GetCustomers.QueryResults.Models;
using System.Collections.Generic;

namespace MCB.Demos.Orders.Microservices.Customers.Messages.GetCustomers.QueryResults
{
    public class GetCustomersQueryResult
    {
        public IEnumerable<CustomerDTO> CustomerCollection { get; set; }
    }
}
