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
    }
    #endregion
}
