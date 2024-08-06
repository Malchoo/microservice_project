namespace SharedKernel.IntegrationEvents.User;

public sealed record MiddleNameChangedIntegrationEvent(
    Guid UserId,
    string MiddleName) : IIntegrationEvent;