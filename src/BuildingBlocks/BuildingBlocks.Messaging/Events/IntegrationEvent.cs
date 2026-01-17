namespace BuildingBlocks.Messaging.Events;

public record IntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();

    public string EventType => GetType().AssemblyQualifiedName!;

    public DateTime Timestamp { get; init; } = DateTime.Now;
}
