using Friendships.Write.Domain.Contracts;
using Friendships.Write.Domain.Ids;

namespace Friendships.Write.Domain.DomainEvents;

public sealed record UserUnblockedDomainEvent(UserId UserId, BlockedId BlockedId) : IDomainEvent;

