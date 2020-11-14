namespace MCB.Demos.Orders.Gateways.WebApp.ViewModels.Responses
{
    public class CustomersResponse
    {
        public Customer[] CustomerArray { get; set; }
    }

    #region Models
    public class Customer
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
    #endregion
}
