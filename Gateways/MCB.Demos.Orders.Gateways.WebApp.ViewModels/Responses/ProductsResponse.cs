namespace MCB.Demos.Orders.Gateways.WebApp.ViewModels.Responses
{
    public class ProductsResponse
    {
        public Product[] ProductArray { get; set; }
    }

    #region Models
    public class Product
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
    #endregion
}
