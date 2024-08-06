using Friendships.Write.Domain.Contracts;
using Friendships.Write.Domain.Entities;

namespace Friendships.Write.Domain.DomainEvents;

public sealed record UserUnblockedByAdminDomainEvent(FriendshipList FriendshipList) : IDomainEvent;