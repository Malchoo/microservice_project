using Friendships.Write.Domain.DomainEvents;
using Friendships.Write.Infrastructure.Persistence;
using MediatR;
using SharedKernel.IntegrationEvents;
using SharedKernel.IntegrationEvents.Friendships.Friendships.Write;
using System.Text.Json;

namespace Friendships.Write.Infrastructure.IntegrationEvents.OutboxWriter;

public class OutboxWriterEventHandler
    : INotificationHandler<FriendshipCreatedDomainEvent>
{
    private readonly FriendshipListDbContext _dbContext;

    public OutboxWriterEventHandler(FriendshipListDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(FriendshipCreatedDomainEvent notification, CancellationToken ct)
    {
        var integrationEvent = new FriendshipCreatedIntegrationEvent(
            UserId: notification.Friendship.Id.Value,
            FriendId: notification.Friendship.FriendId.Value);

        await AddOutboxIntegrationEventAsync(integrationEvent);
    }

    private async Task AddOutboxIntegrationEventAsync(IIntegrationEvent integrationEvent)
    {
        await _dbContext.OutboxIntegrationEvents.AddAsync(new OutboxIntegrationEvent(
            EventName: integrationEvent.GetType().Name,
            EventContent: JsonSerializer.Serialize(integrationEvent)));

        await _dbContext.SaveChangesAsync();
    }
}
