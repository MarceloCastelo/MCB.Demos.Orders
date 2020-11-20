using System;

namespace MCB.Demos.Orders.Gateways.WebApp.ViewModels.Payloads
{
    public class ImportOrdersPayload
    {
        public ImportOrderModel[] ImportOrderModelArray { get; set; }
    }

    #region Models
    public class ImportOrderModel
    {
        public string Code { get; set; }
        public DateTime Data { get; set; }
        public CustomerModel Customer { get; set; }
        public OrderItemModel[] OrderItemCollection { get; set; }
    }
    public class CustomerModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
    public class ProductModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
    public class OrderItemModel
    {
        public int Sequence { get; set; }
        public decimal Quantity { get; set; }
        public decimal Value { get; set; }
        public ProductModel Product { get; set; }
    }
    #endregion
}
