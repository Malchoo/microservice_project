using SharedKernel.IntegrationEvents;

namespace SharedKernel.IntegrationEvents.User;

public sealed record LanguageChangedIntegrationEvent(
    Guid UserId,
    string Language) : IIntegrationEvent;