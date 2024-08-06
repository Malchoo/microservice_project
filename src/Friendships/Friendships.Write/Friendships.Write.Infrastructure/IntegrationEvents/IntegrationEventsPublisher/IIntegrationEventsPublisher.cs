using SharedKernel.IntegrationEvents;

namespace Friendships.Write.Infrastructure.IntegrationEvents.IntegrationEventsPublisher;

public interface IIntegrationEventsPublisher
{
    public void PublishEvent(IIntegrationEvent integrationEvent);
}