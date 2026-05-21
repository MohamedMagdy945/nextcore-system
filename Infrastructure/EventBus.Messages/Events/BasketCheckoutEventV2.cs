namespace EventBus.Messages.Events
{
    public class BasketCheckoutEventV2 : BaseIntegrationEvent
    {
        public string UserName { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }

    }
}
