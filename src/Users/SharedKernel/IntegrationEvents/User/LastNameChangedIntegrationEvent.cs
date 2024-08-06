namespace SharedKernel.IntegrationEvents.User;

public sealed record LastNameChangedIntegrationEvent(
    Guid UserId,
    string LastName) : IIntegrationEvent;