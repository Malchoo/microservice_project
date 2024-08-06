namespace SharedKernel.IntegrationEvents.User;

public sealed record UserBlockedByAdminIntegrationEvent(Guid UserId) : IIntegrationEvent;