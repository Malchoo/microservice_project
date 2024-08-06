namespace Friendships.Write.Infrastructure.IntegrationEvents;

public record OutboxIntegrationEvent(string EventName, string EventContent);