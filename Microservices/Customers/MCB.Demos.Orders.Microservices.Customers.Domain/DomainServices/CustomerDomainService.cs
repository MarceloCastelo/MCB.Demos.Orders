using MCB.Demos.Orders.Microservices.Customers.Domain.DomainModels;
using System.Threading.Tasks;

namespace MCB.Demos.Orders.Microservices.Customers.Domain.DomainServices
{
    public class CustomerDomainService
    {
        public async Task<bool> ImportCustomer(CustomerDomainModel customer)
        {
            if (customer == null)
            {
                throw new System.Exception("error");
            }
            else if (customer.Code == "1")
            {
                return await Task.FromResult(true);
            }
            else if (customer.Code == "2")
            {
                return await Task.FromResult(false);
            }
            else if (customer.Code == "3")
            {
                throw new System.Exception("error");
            }

            return await Task.FromResult(true);
        }
    }
}
