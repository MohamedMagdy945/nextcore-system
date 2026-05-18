namespace EventBus.Messages.Events
{
    public class BasketCheckoutEvent : BaseIntegrationEvent
    {
        public string UserName { get; init; } = string.Empty;
        public decimal TotalPrice { get; init; }

        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string EmailAddress { get; init; } = string.Empty;

        public string AddressLine { get; init; } = string.Empty;
        public string Country { get; init; } = string.Empty;
        public string State { get; init; } = string.Empty;
        public string ZipCode { get; init; } = string.Empty;

        public string CardName { get; init; } = string.Empty;
        public string CardNumber { get; init; } = string.Empty;
        public string Expiration { get; init; } = string.Empty;
        public string CVV { get; init; } = string.Empty;

        public int PaymentMethod { get; init; }
    }
}
