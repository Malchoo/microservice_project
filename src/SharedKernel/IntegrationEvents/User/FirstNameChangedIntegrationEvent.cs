namespace SharedKernel.IntegrationEvents.User;

public sealed record FirstNameChangedIntegrationEvent(
    Guid UserId,
    string FirstName) : IIntegrationEvent;