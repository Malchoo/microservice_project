namespace SharedKernel.IntegrationEvents.User;

public sealed record UserCreatedIntegrationEvent(Guid UserId) : IIntegrationEvent;