namespace SharedKernel.IntegrationEvents.Friendships.Friendships.Write;

public sealed record FriendshipCreatedIntegrationEvent(Guid UserId, Guid FriendId) : IIntegrationEvent;
