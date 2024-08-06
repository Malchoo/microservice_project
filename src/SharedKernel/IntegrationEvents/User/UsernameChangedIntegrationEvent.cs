namespace SharedKernel.IntegrationEvents.User;

public sealed record UsernameChangedIntegrationEvent(
    Guid UserId,
    string Username) : IIntegrationEvent;