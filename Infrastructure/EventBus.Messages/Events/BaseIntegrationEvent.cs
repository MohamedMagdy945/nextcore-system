namespace EventBus.Messages.Events
{
    public class BaseIntegrationEvent
    {
        public string CorrelationId { get; set; }
        public DateTime CreationDate { get; set; }
        public BaseIntegrationEvent()
        {
            CorrelationId = Guid.NewGuid().ToString();
            CreationDate = DateTime.UtcNow;
        }
        public BaseIntegrationEvent(string correlationId, DateTime creationDate)
        {
            CorrelationId = correlationId;
            CreationDate = creationDate;
        }

    }
}
