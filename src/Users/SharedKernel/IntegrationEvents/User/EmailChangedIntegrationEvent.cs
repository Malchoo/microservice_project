namespace SharedKernel.IntegrationEvents.User;

public sealed record EmailChangedIntegrationEvent(
    Guid UserId,
    string Email) : IIntegrationEvent;