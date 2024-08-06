namespace SharedKernel.IntegrationEvents.User;

public sealed record UserUnblockedByAdminIntegrationEvent(Guid UserId) : IIntegrationEvent;