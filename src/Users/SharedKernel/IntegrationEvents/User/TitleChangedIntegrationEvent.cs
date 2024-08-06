namespace SharedKernel.IntegrationEvents.User;

public sealed record TitleChangedIntegrationEvent(
    Guid UserId,
    string Title) : IIntegrationEvent;