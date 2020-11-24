namespace MCB.Demos.Orders.Microservices.Customers.Messages.ImportCustomerIfNotExists.Events
{
    public class CustomerWasSuccessfullyImportedEvent
    {
        public Models.Customer ImportedCustomer { get; set; }
    }
}
