using System;

namespace MCB.Demos.Orders.Gateways.WebApp.ViewModels.Responses
{
    public class OrdersResponse
    {
        public Order[] OrderArray { get; set; }
    }

    #region Models
    public class Order
    {
        public string Code { get; set; }
        public DateTime Date { get; set; }
    }
    #endregion
}
