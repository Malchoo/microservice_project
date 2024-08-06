namespace SharedKernel.IntegrationEvents.User;

public sealed record MobileNumberChangedIntegrationEvent(
    Guid UserId,
    string MobileNumber) : IIntegrationEvent;