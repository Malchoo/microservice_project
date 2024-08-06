namespace SharedKernel.IntegrationEvents.User;

public sealed record ThemeChangedIntegrationEvent(
    Guid UserId,
    string Theme) : IIntegrationEvent;