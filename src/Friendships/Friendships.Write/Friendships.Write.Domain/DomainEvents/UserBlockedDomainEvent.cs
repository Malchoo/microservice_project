using Friendships.Write.Domain.Contracts;
using Friendships.Write.Domain.Ids;

namespace Friendships.Write.Domain.DomainEvents;

public sealed record UserBlockedDomainEvent(UserId UserId, BlockedId BlockedId) : IDomainEvent;

