namespace MCB.Demos.Orders.Microservices.Customers.Messages.ImportCustomerIfNotExists.Events
{
    public class CustomerWasNotImportedSuccessfullyEvent
    {
        public Models.Customer Customer { get; set; }
    }
}
